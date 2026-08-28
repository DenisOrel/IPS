// Decompiled with JetBrains decompiler
// Type: Intermech.Server.Service.IntermechServerService
// Assembly: Intermech.Server.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E91FE21E-230A-49EC-A627-5E0B3AE2517E
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Server.Service.exe

using Intermech.ApplicationModel;
using Intermech.ApplicationModel.NinjectIntegration;
using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Data.Metadata;
using Intermech.Interfaces.Plugins;
using Intermech.Interfaces.Server;
using Intermech.Kernel;
using Intermech.Kernel.Services;
using Intermech.Protection;
using Intermech.Security;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Services;
using System.ServiceProcess;
using System.Threading;

#nullable disable
namespace Intermech.Server.Service;

internal class IntermechServerService : ServiceInstanceBase
{
  private StandardKernel iocContainer;
  private ApplicationStateService applicationStateService;
  private ObjRef imserverRef;
  private IntermechServer imserver;
  private string imserverUri;
  private IProtectionKey protectionKey;
  private OutputViewService outputViewService;
  private IPSFatalExceptionLogger fatalExceptionHandler;
  private CustomServices customServices;
  private RemotingInfoService remotingInfoService;
  private IContainer components;

  public IntermechServerService() => this.InitializeComponent();

  protected override IEventLogWriter CreateFileEventLogWriter()
  {
    return (IEventLogWriter) ApplicationEventLogWriters.CreateTextFileWriter("Intermech.Server.Service.log");
  }

  private void DoStartupTimeout()
  {
    try
    {
      string s = ConfigurationManager.AppSettings.Get("StartupTimeout");
      int num = 0;
      ref int local = ref num;
      if (!int.TryParse(s, out local))
        return;
      Thread.Sleep(TimeSpan.FromSeconds((double) num));
    }
    catch (Exception ex)
    {
    }
  }

  protected override bool DoStartService()
  {
    if (!base.DoStartService())
      return false;
    this.DoStartupTimeout();
    this.CreateIOCContainer();
    this.CreateCustomServicesContainer();
    this.CreateApplicationStateEventsService();
    this.CreateOutputViewService();
    this.InitializeExceptionHandlers();
    this.InitializeRoleBasedSecurity();
    this.InitializeApplicationMode();
    if (!this.TryStartRemoting() || !this.TryCreateProtectionKey())
      return false;
    this.CreateRemotingInfoService();
    this.CreateIMServer();
    this.StartIMServer();
    return true;
  }

  protected override void DoStopService(bool errorMode)
  {
    if (this.applicationStateService != null)
      this.applicationStateService.RaiseExit();
    this.RemoveIMServer();
    this.RemoveRemotingServiceInfo();
    this.InvokeSilently(new Action(this.RemoveProtectionKey));
    this.RemoveExceptionHandlers();
    this.RemoveOutputViewService();
    this.RemoveApplicationStateEventsService();
    this.RemoveCustomServicesContainer();
    this.RemoveIOCContainer();
    base.DoStopService(errorMode);
  }

  protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
  {
    bool flag = powerStatus == PowerBroadcastStatus.QuerySuspend || powerStatus == PowerBroadcastStatus.Suspend ? this.protectionKey == null || this.protectionKey != null && this.protectionKey.CheckHibernate() == IntermechServerService.NativeMethods.IntPtrTrue : base.OnPowerEvent(powerStatus);
    if (this.EventLogService != null)
      this.EventLogService.DefaultLog.Write($"События питания: служба обработала событие {powerStatus} и вернула ответ {flag}.");
    return flag;
  }

  private void CreateIOCContainer()
  {
    this.iocContainer = new StandardKernel(Array.Empty<INinjectModule>());
    this.iocContainer.Load((INinjectModule) new MainApplicationNinjectModule());
    ApplicationServices.Container.ServiceResolver = this.iocContainer.Get<IApplicationServiceResolver>();
    this.iocContainer.Load((INinjectModule) new AssemblyNinjectModule());
    this.iocContainer.Bind<StackTraceBuilder>().To<IPSStackTraceBuilder>();
  }

  private void RemoveIOCContainer()
  {
    if (this.iocContainer == null)
      return;
    this.iocContainer.Dispose();
    this.iocContainer = (StandardKernel) null;
  }

  private void CreateApplicationStateEventsService()
  {
    this.applicationStateService = new ApplicationStateService();
    ApplicationServices.Container.AddService(typeof (IApplicationStateEventsService), (object) this.applicationStateService);
  }

  private void RemoveApplicationStateEventsService()
  {
    if (this.applicationStateService == null)
      return;
    ApplicationServices.Container.RemoveService(typeof (IApplicationStateEventsService));
    this.applicationStateService = (ApplicationStateService) null;
  }

  private void CreateOutputViewService()
  {
    this.outputViewService = new OutputViewService();
    ServerServices.AddService(typeof (IOutputView), (object) this.outputViewService);
    this.customServices.AddService(typeof (IOutputViewHistory), (object) this.outputViewService);
  }

  private void RemoveOutputViewService()
  {
    if (this.outputViewService == null)
      return;
    ServerServices.RemoveService(typeof (IOutputView));
    this.customServices.RemoveService(typeof (IOutputViewHistory));
    this.outputViewService.Dispose();
    this.outputViewService = (OutputViewService) null;
  }

  private void InitializeRoleBasedSecurity() => RBSServer.InitializeSecurityContext();

  private void InitializeApplicationMode()
  {
    AdminUtilsService.ServerRunMode = ServerRunModes.ServiceNT;
  }

  private void CreateCustomServicesContainer()
  {
    this.customServices = new CustomServices();
    ServerServices.AddService(typeof (ICustomServices), (object) this.customServices);
  }

  private void RemoveCustomServicesContainer()
  {
    if (this.customServices == null)
      return;
    ServerServices.RemoveService(typeof (ICustomServices));
    this.customServices = (CustomServices) null;
  }

  private void CreateRemotingInfoService()
  {
    this.remotingInfoService = new RemotingInfoService();
    TrackingServices.RegisterTrackingHandler((ITrackingHandler) this.remotingInfoService);
    ServerServices.AddService(typeof (IRemotingInfoService), (object) this.remotingInfoService);
    this.customServices.AddService(typeof (IRemotingInfoService), (object) this.remotingInfoService);
  }

  private void RemoveRemotingServiceInfo()
  {
    if (this.remotingInfoService == null)
      return;
    TrackingServices.UnregisterTrackingHandler((ITrackingHandler) this.remotingInfoService);
    ServerServices.RemoveService(typeof (IRemotingInfoService));
    this.customServices.RemoveService(typeof (IRemotingInfoService));
    this.remotingInfoService = (RemotingInfoService) null;
  }

  private void CreateIMServer() => this.imserver = new IntermechServer();

  private void RemoveIMServer()
  {
    if (this.imserver == null)
      return;
    RemotingServices.Disconnect((MarshalByRefObject) this.imserver);
    this.imserver.CloseServer();
    this.imserver = (IntermechServer) null;
  }

  private void StartIMServer()
  {
    this.imserverRef = RemotingServices.Marshal((MarshalByRefObject) this.imserver, this.imserverUri);
    this.imserver.Initialize(new IntermechServerInitParams()
    {
      SharedLibraryInitializerService = this.iocContainer.Get<ISharedLibraryInitializerService>(),
      MetadataChangeMonitor = this.iocContainer.Get<IMetadataChangeMonitor>(),
      MetadataResolversFactory = this.iocContainer.Get<MetadataResolverFactory>(),
      CustomServices = (ICustomServices) this.customServices,
      PluginManagerConfigureAction = new Action<PluginManager>(this.ConfigurePluginManager)
    });
  }

  private void ConfigurePluginManager(PluginManager pluginManager)
  {
    pluginManager.PackageActivator = this.iocContainer.Get<IPackageActivator>();
  }

  private bool TryStartRemoting()
  {
    ServerRemotingConfigurator remotingConfigurator = new ServerRemotingConfigurator();
    remotingConfigurator.Configure();
    this.imserverUri = remotingConfigurator.IMServerUri;
    return true;
  }

  private bool TryCreateProtectionKey()
  {
    Exception exception = (Exception) null;
    int num1 = 335;
    byte[][] numArray = new byte[32 /*0x20*/][]
    {
      new byte[16 /*0x10*/]
      {
        (byte) 162,
        (byte) 51,
        (byte) 137,
        (byte) 208 /*0xD0*/,
        (byte) 56,
        (byte) 119,
        (byte) 86,
        (byte) 181,
        (byte) 87,
        (byte) 230,
        (byte) 75,
        (byte) 94,
        (byte) 7,
        (byte) 88,
        (byte) 2,
        (byte) 102
      },
      new byte[16 /*0x10*/]
      {
        (byte) 190,
        (byte) 116,
        (byte) 245,
        (byte) 107,
        (byte) 200,
        (byte) 91,
        (byte) 150,
        (byte) 211,
        (byte) 27,
        (byte) 19,
        (byte) 6,
        (byte) 32 /*0x20*/,
        (byte) 229,
        (byte) 127 /*0x7F*/,
        (byte) 119,
        (byte) 240 /*0xF0*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 202,
        (byte) 166,
        (byte) 81,
        (byte) 0,
        (byte) 11,
        (byte) 32 /*0x20*/,
        (byte) 106,
        (byte) 43,
        (byte) 11,
        (byte) 203,
        (byte) 157,
        (byte) 97,
        (byte) 58,
        (byte) 44,
        (byte) 217,
        (byte) 213
      },
      new byte[16 /*0x10*/]
      {
        (byte) 18,
        (byte) 150,
        (byte) 0,
        (byte) 13,
        (byte) 100,
        (byte) 208 /*0xD0*/,
        (byte) 111,
        (byte) 209,
        (byte) 227,
        (byte) 78,
        (byte) 124,
        (byte) 230,
        (byte) 242,
        (byte) 68,
        (byte) 143,
        (byte) 231
      },
      new byte[16 /*0x10*/]
      {
        (byte) 209,
        (byte) 133,
        (byte) 11,
        (byte) 77,
        (byte) 169,
        (byte) 167,
        (byte) 214,
        (byte) 248,
        (byte) 59,
        (byte) 5,
        (byte) 205,
        (byte) 100,
        (byte) 31 /*0x1F*/,
        (byte) 209,
        (byte) 190,
        (byte) 17
      },
      new byte[16 /*0x10*/]
      {
        (byte) 14,
        (byte) 22,
        (byte) 173,
        (byte) 152,
        (byte) 235,
        (byte) 205,
        (byte) 151,
        (byte) 242,
        (byte) 60,
        (byte) 94,
        (byte) 164,
        (byte) 21,
        (byte) 59,
        (byte) 190,
        (byte) 149,
        (byte) 99
      },
      new byte[16 /*0x10*/]
      {
        (byte) 115,
        (byte) 252,
        (byte) 90,
        (byte) 45,
        (byte) 247,
        (byte) 145,
        (byte) 168,
        (byte) 130,
        (byte) 201,
        (byte) 93,
        (byte) 196,
        (byte) 109,
        (byte) 218,
        (byte) 244,
        (byte) 122,
        (byte) 43
      },
      new byte[16 /*0x10*/]
      {
        (byte) 187,
        (byte) 66,
        (byte) 107,
        (byte) 140,
        (byte) 131,
        (byte) 124,
        (byte) 89,
        (byte) 47,
        (byte) 137,
        (byte) 119,
        (byte) 4,
        (byte) 48 /*0x30*/,
        (byte) 165,
        (byte) 14,
        (byte) 74,
        (byte) 219
      },
      new byte[16 /*0x10*/]
      {
        (byte) 13,
        (byte) 118,
        (byte) 140,
        (byte) 130,
        (byte) 77,
        (byte) 181,
        (byte) 189,
        (byte) 35,
        (byte) 207,
        (byte) 89,
        (byte) 196,
        (byte) 5,
        (byte) 82,
        (byte) 5,
        (byte) 11,
        (byte) 58
      },
      new byte[16 /*0x10*/]
      {
        (byte) 5,
        (byte) 174,
        (byte) 185,
        (byte) 222,
        (byte) 22,
        (byte) 123,
        (byte) 212,
        (byte) 140,
        (byte) 179,
        (byte) 53,
        (byte) 159,
        (byte) 106,
        (byte) 184,
        (byte) 49,
        (byte) 231,
        (byte) 136
      },
      new byte[16 /*0x10*/]
      {
        (byte) 12,
        (byte) 139,
        (byte) 237,
        (byte) 62,
        (byte) 209,
        (byte) 171,
        (byte) 221,
        (byte) 63 /*0x3F*/,
        (byte) 220,
        (byte) 136,
        (byte) 131,
        (byte) 75,
        (byte) 227,
        (byte) 105,
        (byte) 29,
        (byte) 70
      },
      new byte[16 /*0x10*/]
      {
        (byte) 144 /*0x90*/,
        (byte) 250,
        (byte) 213,
        (byte) 223,
        (byte) 96 /*0x60*/,
        (byte) 174,
        (byte) 38,
        (byte) 200,
        (byte) 117,
        (byte) 250,
        (byte) 59,
        (byte) 224 /*0xE0*/,
        (byte) 53,
        (byte) 13,
        (byte) 106,
        (byte) 89
      },
      new byte[16 /*0x10*/]
      {
        (byte) 139,
        (byte) 162,
        (byte) 87,
        (byte) 169,
        (byte) 68,
        (byte) 60,
        (byte) 196,
        (byte) 190,
        (byte) 63 /*0x3F*/,
        (byte) 54,
        (byte) 94,
        (byte) 158,
        (byte) 204,
        (byte) 163,
        (byte) 48 /*0x30*/,
        (byte) 196
      },
      new byte[16 /*0x10*/]
      {
        (byte) 58,
        (byte) 246,
        (byte) 63 /*0x3F*/,
        (byte) 22,
        (byte) 39,
        (byte) 170,
        (byte) 74,
        (byte) 208 /*0xD0*/,
        (byte) 83,
        (byte) 240 /*0xF0*/,
        (byte) 153,
        (byte) 173,
        (byte) 11,
        (byte) 170,
        (byte) 162,
        (byte) 73
      },
      new byte[16 /*0x10*/]
      {
        (byte) 82,
        (byte) 137,
        (byte) 97,
        (byte) 78,
        (byte) 7,
        (byte) 60,
        (byte) 89,
        (byte) 98,
        (byte) 152,
        (byte) 77,
        (byte) 166,
        (byte) 197,
        (byte) 49,
        (byte) 113,
        (byte) 64 /*0x40*/,
        (byte) 102
      },
      new byte[16 /*0x10*/]
      {
        (byte) 38,
        (byte) 13,
        (byte) 130,
        (byte) 55,
        (byte) 17,
        (byte) 92,
        (byte) 126,
        (byte) 156,
        (byte) 110,
        (byte) 144 /*0x90*/,
        (byte) 243,
        (byte) 42,
        (byte) 130,
        (byte) 99,
        (byte) 156,
        (byte) 147
      },
      new byte[16 /*0x10*/]
      {
        (byte) 59,
        (byte) 126,
        (byte) 122,
        (byte) 176 /*0xB0*/,
        (byte) 174,
        (byte) 98,
        (byte) 245,
        (byte) 208 /*0xD0*/,
        (byte) 199,
        (byte) 47,
        (byte) 254,
        (byte) 154,
        (byte) 77,
        (byte) 62,
        (byte) 114,
        (byte) 56
      },
      new byte[16 /*0x10*/]
      {
        (byte) 144 /*0x90*/,
        (byte) 137,
        (byte) 168,
        (byte) 17,
        (byte) 112 /*0x70*/,
        (byte) 58,
        (byte) 146,
        (byte) 75,
        (byte) 81,
        (byte) 145,
        (byte) 51,
        (byte) 133,
        (byte) 120,
        (byte) 70,
        (byte) 225,
        (byte) 59
      },
      new byte[16 /*0x10*/]
      {
        (byte) 87,
        (byte) 190,
        (byte) 249,
        (byte) 89,
        (byte) 7,
        (byte) 207,
        (byte) 63 /*0x3F*/,
        (byte) 78,
        (byte) 72,
        (byte) 244,
        (byte) 16 /*0x10*/,
        (byte) 247,
        (byte) 150,
        (byte) 254,
        (byte) 141,
        (byte) 205
      },
      new byte[16 /*0x10*/]
      {
        (byte) 198,
        (byte) 141,
        (byte) 176 /*0xB0*/,
        (byte) 87,
        (byte) 175,
        (byte) 252,
        (byte) 184,
        (byte) 125,
        (byte) 224 /*0xE0*/,
        (byte) 158,
        (byte) 181,
        (byte) 193,
        (byte) 64 /*0x40*/,
        (byte) 136,
        (byte) 36,
        (byte) 10
      },
      new byte[16 /*0x10*/]
      {
        (byte) 246,
        (byte) 247,
        (byte) 99,
        (byte) 78,
        (byte) 41,
        (byte) 128 /*0x80*/,
        (byte) 22,
        (byte) 240 /*0xF0*/,
        (byte) 148,
        (byte) 145,
        (byte) 135,
        (byte) 215,
        (byte) 61,
        (byte) 190,
        (byte) 21,
        (byte) 89
      },
      new byte[16 /*0x10*/]
      {
        (byte) 211,
        (byte) 126,
        (byte) 27,
        (byte) 191,
        (byte) 199,
        (byte) 67,
        (byte) 196,
        (byte) 118,
        (byte) 193,
        (byte) 179,
        (byte) 99,
        (byte) 203,
        (byte) 138,
        (byte) 86,
        (byte) 146,
        (byte) 167
      },
      new byte[16 /*0x10*/]
      {
        (byte) 218,
        (byte) 109,
        (byte) 109,
        (byte) 114,
        (byte) 120,
        (byte) 231,
        (byte) 95,
        (byte) 107,
        (byte) 62,
        (byte) 43,
        (byte) 103,
        (byte) 185,
        (byte) 81,
        (byte) 254,
        (byte) 57,
        (byte) 200
      },
      new byte[16 /*0x10*/]
      {
        (byte) 90,
        (byte) 108,
        (byte) 43,
        (byte) 100,
        (byte) 8,
        (byte) 24,
        (byte) 226,
        (byte) 25,
        (byte) 103,
        (byte) 103,
        (byte) 148,
        (byte) 134,
        (byte) 235,
        (byte) 9,
        (byte) 167,
        (byte) 72
      },
      new byte[16 /*0x10*/]
      {
        (byte) 162,
        (byte) 49,
        (byte) 164,
        (byte) 85,
        (byte) 219,
        (byte) 36,
        (byte) 15,
        (byte) 215,
        (byte) 168,
        (byte) 8,
        (byte) 196,
        (byte) 117,
        (byte) 71,
        (byte) 103,
        (byte) 17,
        (byte) 138
      },
      new byte[16 /*0x10*/]
      {
        (byte) 160 /*0xA0*/,
        (byte) 197,
        (byte) 137,
        (byte) 86,
        (byte) 95,
        (byte) 33,
        (byte) 190,
        (byte) 120,
        (byte) 164,
        (byte) 108,
        (byte) 34,
        (byte) 145,
        (byte) 154,
        (byte) 55,
        (byte) 132,
        (byte) 58
      },
      new byte[16 /*0x10*/]
      {
        (byte) 223,
        (byte) 182,
        (byte) 28,
        (byte) 40,
        (byte) 145,
        (byte) 26,
        (byte) 85,
        (byte) 14,
        (byte) 152,
        (byte) 247,
        (byte) 15,
        (byte) 253,
        (byte) 25,
        (byte) 92,
        (byte) 159,
        (byte) 149
      },
      new byte[16 /*0x10*/]
      {
        (byte) 45,
        (byte) 249,
        (byte) 147,
        (byte) 37,
        (byte) 249,
        (byte) 192 /*0xC0*/,
        (byte) 64 /*0x40*/,
        (byte) 214,
        (byte) 165,
        (byte) 253,
        (byte) 76,
        (byte) 113,
        (byte) 189,
        (byte) 187,
        (byte) 209,
        (byte) 118
      },
      new byte[16 /*0x10*/]
      {
        (byte) 87,
        (byte) 190,
        (byte) 53,
        (byte) 12,
        (byte) 25,
        (byte) 191,
        (byte) 126,
        (byte) 182,
        (byte) 94,
        (byte) 150,
        (byte) 202,
        (byte) 111,
        (byte) 34,
        (byte) 233,
        (byte) 35,
        (byte) 215
      },
      new byte[16 /*0x10*/]
      {
        (byte) 148,
        (byte) 152,
        (byte) 180,
        (byte) 245,
        (byte) 153,
        (byte) 147,
        (byte) 93,
        (byte) 236,
        (byte) 223,
        (byte) 108,
        (byte) 180,
        (byte) 170,
        (byte) 13,
        (byte) 167,
        (byte) 213,
        (byte) 132
      },
      new byte[16 /*0x10*/]
      {
        (byte) 25,
        (byte) 220,
        (byte) 218,
        (byte) 154,
        (byte) 225,
        (byte) 17,
        (byte) 55,
        (byte) 135,
        (byte) 219,
        (byte) 150,
        (byte) 189,
        (byte) 227,
        (byte) 20,
        (byte) 17,
        (byte) 189,
        (byte) 76
      },
      new byte[16 /*0x10*/]
      {
        (byte) 190,
        (byte) 75,
        (byte) 170,
        (byte) 98,
        (byte) 145,
        (byte) 59,
        (byte) 208 /*0xD0*/,
        (byte) 105,
        (byte) 245,
        (byte) 63 /*0x3F*/,
        (byte) 1,
        (byte) 111,
        (byte) 79,
        (byte) 168,
        (byte) 128 /*0x80*/,
        (byte) 120
      }
    };
    int index1 = (Environment.TickCount & 15) * 2;
    byte[] query = numArray[index1];
    byte[] reply = numArray[index1 + 1];
    ProtectionService.Provider = (IServiceProvider) ServerServices.ServiceContainer;
    ProtectionService.HasUI = false;
    try
    {
      this.protectionKey = (IProtectionKey) new LocalKey(num1, query, reply);
      this.EventLogService.AllLogs.Write("Используется локальный ключ.");
    }
    catch (Exception ex)
    {
      exception = ex;
    }
    int num2 = 10;
    int num3 = 30;
    NameValueCollection appSettings = ConfigurationManager.AppSettings;
    NetworkKey.SetSpareServers(appSettings["Protection.SpareServers"]);
    NetworkKey.SetInformAdmins(appSettings["Protection.InformAdmins"]);
    string s1 = appSettings["waitProtectCount"];
    int result = 0;
    if (!string.IsNullOrEmpty(s1) && int.TryParse(s1, out result))
      num2 = result;
    if (num2 > 10)
      num2 = 10;
    string s2 = appSettings["waitProtectDelay"];
    result = 0;
    if (!string.IsNullOrEmpty(s2) && int.TryParse(s2, out result))
      num3 = result;
    if (num3 > 600)
      num3 = 600;
    if (num3 < 1)
      num3 = 1;
    for (int index2 = 0; index2 < num2; ++index2)
    {
      try
      {
        if (this.protectionKey == null)
        {
          this.protectionKey = (IProtectionKey) new NetworkKey(num1, query, reply);
          this.EventLogService.AllLogs.Write("Используется сетевой ключ.");
        }
        else
          break;
      }
      catch (Exception ex)
      {
        this.EventLogService.AllLogs.Write($"{"Protection"}: {ex.Message}", EventLogItemType.Error);
      }
      if (this.protectionKey == null)
        Thread.Sleep(num3 * 1000);
      else
        break;
    }
    try
    {
      if (this.protectionKey == null)
      {
        this.protectionKey = (IProtectionKey) new NetworkKey(num1, query, reply);
        this.EventLogService.AllLogs.Write("Используется сетевой ключ.");
      }
    }
    catch (Exception ex)
    {
      if (exception != null)
        this.EventLogService.AllLogs.Write($"{"Protection"}: {exception.Message}", EventLogItemType.Warning);
      this.EventLogService.AllLogs.Write($"{"Protection"}: {ex.Message}", EventLogItemType.Warning);
      return false;
    }
    if (this.protectionKey != null)
    {
      ServerServices.AddService(typeof (IProtectionKey), (object) this.protectionKey);
      ServerServices.AddService(typeof (ILicenser), (object) this.protectionKey);
      return true;
    }
    this.EventLogService.AllLogs.Write($"{"Protection"}: {"Превышено количество попыток подключения к менеджеру лицензий."}", EventLogItemType.Error);
    return false;
  }

  private void RemoveProtectionKey()
  {
    if (this.protectionKey == null)
      return;
    ServerServices.RemoveService(typeof (IProtectionKey));
    ServerServices.RemoveService(typeof (ILicenser));
    this.InvokeSilently((Action) (() => this.protectionKey.Dispose()), "this.protectionKey.Dispose()");
    this.protectionKey = (IProtectionKey) null;
  }

  private void InitializeExceptionHandlers()
  {
    ExceptionServices.StackTraceBuilderFactory = this.iocContainer.Get<Func<StackTraceBuilder>>();
    this.fatalExceptionHandler = new IPSFatalExceptionLogger(this.EventLogService.AllLogs);
    this.fatalExceptionHandler.Activate();
  }

  private void RemoveExceptionHandlers()
  {
    if (this.fatalExceptionHandler == null)
      return;
    this.fatalExceptionHandler.Deactivate();
    this.fatalExceptionHandler = (IPSFatalExceptionLogger) null;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.CanHandlePowerEvent = true;
    this.ServiceName = "Сервер приложений IPS";
  }

  private static class NativeMethods
  {
    public static readonly IntPtr IntPtrFalse = IntPtr.Zero;
    public static readonly IntPtr IntPtrTrue = new IntPtr(1);
  }
}

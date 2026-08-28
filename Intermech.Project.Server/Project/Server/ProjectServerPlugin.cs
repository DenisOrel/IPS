// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.ProjectServerPlugin
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Plugins;
using Intermech.Interfaces.Server;
using System;
using System.Reflection;

#nullable disable
namespace Intermech.Project.Server;

internal class ProjectServerPlugin : IPackage, IPackageExtension
{
  private bool _postInited;

  [NotNull]
  internal static Assembly Assembly => typeof (ProjectServerPlugin).Assembly;

  public void Load([NotNull] IServiceProvider serviceProvider)
  {
  }

  public void Unload()
  {
  }

  [NotNull]
  public string Name => "Intermech.Project.Server";

  public bool PostInit()
  {
    if (this._postInited)
      return true;
    this._postInited = true;
    ICustomServices service1 = ApplicationServices.Container.GetService<ICustomServices>();
    IUserSession sessionTemporaryClone = ApplicationServices.Container.GetService<IDBTimedEvents>().GetSystemSessionTemporaryClone("improject.startup");
    try
    {
      Intermech.Project.Library.Init(ApplicationServices.Container.WithCustomServices(), sessionTemporaryClone);
      IPortalEventsService service2 = ApplicationServices.Container.GetService<IPortalEventsService>(false);
      if (service2 != null)
      {
        service2.GetTaskByTypeEvent += new GetTaskByTypeEventHandler(PortalHandler.GetTaskByTypeEvent);
        service2.ObjectImportedEvent += new ObjectImportedEventHandler(PortalHandler.ObjectImportedEvent);
        service2.BeforeObjectRefreshEvent += new BeforeObjectRefreshEventHandler(PortalHandler.BeforeObjectRefreshEvent);
      }
      ApplicationServices.Container.GetService<ILinkedObjectsService>(false)?.RegisterHandler((ILinkedObjectsHandler) new ProjectLinkedObjectsHandler());
    }
    finally
    {
      sessionTemporaryClone?.Logout("improject.startup");
    }
    Assembly.GetExecutingAssembly().RegisterDbCreators();
    service1.AddService(typeof (IProjectServer), (object) new ProjectPatсher());
    ProjectTimersService.Register();
    return true;
  }
}

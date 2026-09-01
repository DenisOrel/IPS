// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.PluginServerModule
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using DXP;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;

#nullable disable
namespace CSharpPlugin;

[ClassInterface(ClassInterfaceType.AutoDispatch)]
public class PluginServerModule : ServerModule
{
  private readonly AddinReferenceResolver _addinResolver;

  public PluginServerModule(IClient argClient, string argModuleName, string assemblyPath)
    : base(argClient, argModuleName)
  {
    this._addinResolver = new AddinReferenceResolver();
    ChannelServices.RegisterChannel((IChannel) new IpcChannel((IDictionary) new Hashtable()
    {
      [(object) "name"] = (object) "ipc",
      [(object) "portName"] = (object) $"IPSAddIn_{Process.GetCurrentProcess().SessionId}"
    }, (IClientChannelSinkProvider) new BinaryClientFormatterSinkProvider(), (IServerChannelSinkProvider) new BinaryServerFormatterSinkProvider()
    {
      TypeFilterLevel = TypeFilterLevel.Full
    }), false);
    RemotingConfiguration.RegisterWellKnownServiceType(typeof (IPSAddInProxy), "server.rem", WellKnownObjectMode.Singleton);
  }

  protected override void InitializeCommands()
  {
    ((DXP.CommandLauncher) this.CommandLauncher).RegisterCommand("ProjectPropertiesView", new CommandProc(Commands.ProjectPropertiesViewCommand), new GetStateProc(this.GetState_Enabled));
    ((DXP.CommandLauncher) this.CommandLauncher).RegisterCommand("DocumentPropertiesView", new CommandProc(Commands.DocumentPropertiesViewCommand), new GetStateProc(this.GetState_Enabled));
    ((DXP.CommandLauncher) this.CommandLauncher).RegisterCommand("ProjectImport", new CommandProc(Commands.ProjectImportCommand), new GetStateProc(this.GetState_Enabled));
    ((DXP.CommandLauncher) this.CommandLauncher).RegisterCommand("ProjectSaveChanges", new CommandProc(Commands.ProjectSaveChangesCommand), new GetStateProc(this.GetState_Enabled));
    ((DXP.CommandLauncher) this.CommandLauncher).RegisterCommand("DocumentSaveChanges", new CommandProc(Commands.DocumentSaveChangesCommand), new GetStateProc(this.GetState_Enabled));
    ((DXP.CommandLauncher) this.CommandLauncher).RegisterCommand("ProjectExtendedSave", new CommandProc(Commands.ProjectExtendedSaveCommand), new GetStateProc(this.GetState_Enabled));
    ((DXP.CommandLauncher) this.CommandLauncher).RegisterCommand("CreateSpecification", new CommandProc(Commands.CreateSpecificationCommand), new GetStateProc(this.GetState_Enabled));
    ((DXP.CommandLauncher) this.CommandLauncher).RegisterCommand("CreateElementList", new CommandProc(Commands.CreateElementListCommand), new GetStateProc(this.GetState_Enabled));
    ((DXP.CommandLauncher) this.CommandLauncher).RegisterCommand("OpenIPSProject", new CommandProc(Commands.OpenIPSProjectCommand), new GetStateProc(this.GetState_Enabled));
    ((DXP.CommandLauncher) this.CommandLauncher).RegisterCommand("CaptureChangesCommand", (CommandProc) ((IServerDocumentView view, ref string arg) =>
    {
      try
      {
        Commands.ExecuteComMethod((Action<object>) (app =>
        {
          // ISSUE: reference to a compiler-generated field
          if (PluginServerModule.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            PluginServerModule.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CaptureChangesBase", (IEnumerable<Type>) null, typeof (PluginServerModule), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PluginServerModule.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) PluginServerModule.\u003C\u003Eo__2.\u003C\u003Ep__0, app);
        }));
      }
      catch (Exception ex)
      {
        Commands.ThrowClientException(ex);
      }
    }), new GetStateProc(this.GetState_Enabled));
  }

  private void GetState_Enabled(
    IServerDocumentView argContext,
    string argParameters,
    ref bool Enabled,
    ref bool Checked,
    ref bool Visible,
    ref string Caption,
    ref string ImageFile)
  {
    Enabled = true;
    Visible = true;
  }

  protected override IServerDocument NewDocumentInstance(string kind, string fileName)
  {
    return (IServerDocument) null;
  }
}

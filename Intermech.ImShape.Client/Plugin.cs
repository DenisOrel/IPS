// Decompiled with JetBrains decompiler
// Type: Intermech.ImShape.Client.Plugin
// Assembly: Intermech.ImShape.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EAEE73DE-1C1F-4401-8BB6-D181BFA32870
// Assembly location: D:\IPS\Client\Intermech.ImShape.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Plugins;
using Intermech.Localization;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Runtime.ComInterop;
using Intermech.Tools.Integrators.Notifications;
using Interop.IMShape;
using System;

#nullable disable
namespace Intermech.ImShape.Client;

internal sealed class Plugin : IPackage
{
  private static readonly ComObjectProvider shapeProvider = (ComObjectProvider) new ClsidProvider(typeof (ShapeComClass).GUID, true);

  public void Load(IServiceProvider serviceProvider)
  {
    try
    {
      if (!this.IsIMShapeInstalled())
        return;
      ServicesManager.AddService(typeof (ImShapeSystemSettingsService), (object) new ImShapeSystemSettingsService());
      IPropertyPagesService service1 = ServiceUtils.GetService<IPropertyPagesService>((object) ServicesManager.ServiceContainer, false);
      if (service1 != null && (ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole).IsAdmin)
        service1.AddPage(LocalizationHolder.rm.GetString("ImShape.SystemPage.Path"), (IPropertyPage) new ImShapeSystemSettingsViewPage());
      IFactory service2 = ServiceUtils.GetService<IFactory>((object) ServicesManager.ServiceContainer, false);
      if (service2 != null)
      {
        MenuTemplate contextMenuTemplate = service2.ContextMenuTemplate;
        contextMenuTemplate.BeginUpdate();
        try
        {
          MenuTemplateNode node = new MenuTemplateNode("IMShape", "IMShape", -1, 4, 1);
          contextMenuTemplate.Nodes.Add(node);
          node.Nodes.Add(new MenuTemplateNode("IMShapeAdd", LocalizationHolder.rm.GetString("ImShape.ContextMenu.Add"), -1, 1, 0));
          node.Nodes.Add(new MenuTemplateNode("IMShapeSearch", LocalizationHolder.rm.GetString("ImShape.ContextMenu.Search"), -1, 1, 1));
        }
        finally
        {
          contextMenuTemplate.EndUpdate();
        }
      }
      ServiceUtils.GetService<INotificationService>((object) ServicesManager.ServiceContainer, false)?.Subscribe(IMShapeEventArgs.UpdateDB, new NotificationEventHandler(this.UpdateImShapeDBAfterDocumentChanges));
      ImShapeCom.Init();
    }
    catch
    {
    }
  }

  public void Unload()
  {
  }

  public string Name => "IMShape Client";

  private bool IsIMShapeInstalled()
  {
    try
    {
      return Plugin.shapeProvider.IsRegistered();
    }
    catch
    {
      return false;
    }
  }

  private void UpdateImShapeDBAfterDocumentChanges(object sender, NotificationEventArgs e)
  {
    if (!(e is IMShapeEventArgs imShapeEventArgs))
      return;
    ImShapeCom.AddDoc(imShapeEventArgs.Integrator, imShapeEventArgs.Documents);
  }
}

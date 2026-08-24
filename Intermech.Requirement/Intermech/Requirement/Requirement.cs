// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.Requirement
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using Intermech.Commands;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Plugins;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Protection;
using System;

#nullable disable
namespace Intermech.Requirement;

public class Requirement : IPackage
{
  public void Unload()
  {
    ObjectCommandEvents.SaveChanges.Before -= new EventHandler<BeforeObjectCommandArgs>(new RequirementContextMenuProvider().SaveChanges_Before);
  }

  public void Load(IServiceProvider serviceProvider)
  {
    if (!(serviceProvider.GetService(typeof (ILicenser)) is ILicenser service1))
      throw new ProtectionException("ILicenser not found");
    service1.AllocateLicense(344);
    if (!(serviceProvider.GetService(typeof (IPluginManager)) is IPluginManager service2))
      return;
    if (service2.IsLoadComplete)
      Intermech.Requirement.Requirement.RegisterContextMenuProvider();
    else
      service2.LoadComplete += new EventHandler(this.pluginManager_LoadComplete);
  }

  public string Name => "Модуль управления требованиями";

  private void pluginManager_LoadComplete(object sender, EventArgs e)
  {
    Intermech.Requirement.Requirement.RegisterContextMenuProvider();
  }

  private static void RegisterContextMenuProvider()
  {
    ObjectCommandEvents.SaveChanges.Before += new EventHandler<BeforeObjectCommandArgs>(new RequirementContextMenuProvider().SaveChanges_Before);
    if (!(ServicesManager.GetService(typeof (IFactory)) is IFactory service))
      return;
    IMSObjectType objectType = MetaDataHelper.GetObjectType(RequirementConst.SpecificationGuid);
    if (objectType == null)
      return;
    RequirementConst.SpecificationID = (long) objectType.ObjectTypeID;
    foreach (int typeID in MetaDataHelper.GetObjectTypeChildrenIDRecursive((int) RequirementConst.SpecificationID))
      service.AddCommandsProvider(1, typeID, (ICommandsProvider) new RequirementContextMenuProvider());
    MenuTemplate contextMenuTemplate = service.ContextMenuTemplate;
    contextMenuTemplate.BeginUpdate();
    try
    {
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode("CreateObjectTreeReq", "Создать дерево объектов ТЗ", -1, 19, 31 /*0x1F*/));
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode("CheckTZforCompleted", "Проверить ТЗ на завершение", -1, 19, 32 /*0x20*/));
    }
    finally
    {
      contextMenuTemplate.EndUpdate();
    }
  }
}

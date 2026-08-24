// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.ConfigurationCodeProvider
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.PdmConfigurator;

public sealed class ConfigurationCodeProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (((viewServices.GetService(typeof (IViewState)) is IViewState service ? (long) service.ViewState : 0L) & 2L) != 0L || items.Count != 1 || !(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || !MetaDataHelper.IsPdmConfigurableObjectType(itemData.ObjectType))
      return CommandsInfo.Empty;
    CommandsInfo groupCommands = new CommandsInfo();
    groupCommands.Add("CreateConfigurationCode", new CommandInfo(0, new ClickEventHandler(this.CreateConfigurationCode)));
    return groupCommands;
  }

  private void CreateConfigurationCode(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    using (ConfigurationCodeForm configurationCodeForm = new ConfigurationCodeForm(items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID))
    {
      int num = (int) configurationCodeForm.ShowDialog();
    }
  }
}

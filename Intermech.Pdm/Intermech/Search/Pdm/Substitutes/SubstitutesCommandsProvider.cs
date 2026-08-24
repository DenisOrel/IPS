// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.Substitutes.SubstitutesCommandsProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Pdm;
using Intermech.Pdm.Substitutes;
using Intermech.Search.Utilities;
using System;

#nullable disable
namespace Intermech.Search.Pdm.Substitutes;

public sealed class SubstitutesCommandsProvider : ICommandsProvider
{
  private LazyService<ISubstitutesClientService> _substitutesClientService = new LazyService<ISubstitutesClientService>();

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    CommandsInfo groupCommands = new CommandsInfo();
    if (this.CheckSelectedItems(items))
      groupCommands.Add("PDM.CreateSubstitutesGroup", new CommandInfo(0, new ClickEventHandler(this.CreateSubstitutes)));
    if (this.CheckParamsForActualizeSubstitute(items, viewServices))
      groupCommands.Add("PDM.MakeActualSubstitute", new CommandInfo(0, new ClickEventHandler(this.ActualizeSubstitute)));
    if (this.CheckSelectedItems(items))
    {
      groupCommands.Add("PDM.EditSubstitutesGroup", new CommandInfo(0, new ClickEventHandler(this.EditSubstitutes)));
      groupCommands.Add("PDM.DeleteSubstitutesGroup", new CommandInfo(0, new ClickEventHandler(this.RemoveSubstitutes)));
    }
    return groupCommands;
  }

  private void CreateSubstitutes(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (!this.CheckSelectedItems(items))
      throw new ArgumentException();
    int num = (int) new ArtSubstitutionsEditor(string.Empty, items, viewServices, SubstitutesEditorCommand.CreateGroup).ShowDialog();
  }

  private void ActualizeSubstitute(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (!this.CheckParamsForActualizeSubstitute(items, viewServices))
      throw new ArgumentException();
    this._substitutesClientService.Value.ActualizeSubstitute((items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID).Value);
  }

  private void EditSubstitutes(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (!this.CheckSelectedItems(items))
      throw new ArgumentException();
    int num = (int) new ArtSubstitutionsEditor(string.Empty, items, viewServices, SubstitutesEditorCommand.EditSubstitutes).ShowDialog();
  }

  private void RemoveSubstitutes(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    IDBRelationID dbRelationId = this.CheckSelectedItems(items) ? items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID : throw new ArgumentException();
    this._substitutesClientService.Value.RemoveSubstitutes(dbRelationId.ProjID, dbRelationId.RelationType);
  }

  private bool CheckSelectedItems(ISelectedItems selectedItems)
  {
    return !ObjectTypeHelper.IsUnknownObjectTypeID(SelectedItemsHelper.GetProjectTypeID(selectedItems)) && selectedItems.GetItemData(0, typeof (IDBRelationID)) is IDBRelationID itemData && !ObjectHelper.IsUnknownObjectVersionID(itemData.ProjID) && itemData.RelationType != -1 && SubstitutesHelper.IsSuitableForSubstitutesRelationType(itemData.RelationType);
  }

  private bool CheckParamsForActualizeSubstitute(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider)
  {
    return this.CheckSelectedItems(selectedItems) && !RelationHelper.IsUnknownRelationID((selectedItems.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID).Value);
  }
}

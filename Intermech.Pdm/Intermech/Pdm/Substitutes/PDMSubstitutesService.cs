// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Substitutes.PDMSubstitutesService
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator.Interfaces;
using Intermech.Search;
using Intermech.Search.Pdm.Substitutes;
using Intermech.Search.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Intermech.Pdm.Substitutes;

internal class PDMSubstitutesService : IPDMSubstitutesService
{
  private INotificationService _notificationService;

  public PDMSubstitutesService(IServiceProvider serviceProvider)
  {
    this.CheckArgumentNotNull((object) serviceProvider, nameof (serviceProvider));
    this._notificationService = serviceProvider.GetService<INotificationService>();
  }

  public bool CanUseRussianFeatures
  {
    get => Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName == "ru";
  }

  public void CreateSubstitutesGroup(
    object items,
    IServiceProvider serviceProvider,
    object additionalInfo,
    long desiredGroupNumber = -1)
  {
    this.CheckArgumentNotNull(items, nameof (items));
    this.CheckArgumentCastToType<ISelectedItems>(items, nameof (items));
    this.ExecuteSubstitutesEditor((ISelectedItems) items, serviceProvider, additionalInfo, SubstitutesEditorCommand.CreateGroup, desiredGroupNumber);
  }

  public void MakeActualSubstitute(
    object items,
    IServiceProvider serviceProvider,
    object additionalInfo)
  {
    this.CheckArgumentNotNull(items, nameof (items));
    this.CheckArgumentCastToType<ISelectedItems>(items, nameof (items));
    this.ExecuteSubstitutesEditor((ISelectedItems) items, serviceProvider, additionalInfo, SubstitutesEditorCommand.ActualizeSubstitute);
  }

  public void EditSubstitutesGroup(
    object items,
    IServiceProvider serviceProvider,
    object additionalInfo)
  {
    this.CheckArgumentNotNull(items, nameof (items));
    this.CheckArgumentCastToType<ISelectedItems>(items, nameof (items));
    this.ExecuteSubstitutesEditor((ISelectedItems) items, serviceProvider, additionalInfo, SubstitutesEditorCommand.EditSubstitutes);
  }

  public void DeleteSubstitutesGroup(
    object items,
    IServiceProvider serviceProvider,
    object additionalInfo)
  {
    this.CheckArgumentNotNull(items, nameof (items));
    this.CheckArgumentCastToType<ISelectedItems>(items, nameof (items));
    this.DeleteSubstitutesGroup((ISelectedItems) items, serviceProvider, additionalInfo);
  }

  public PDMSubstitutesCommands GetEnabledSubstitutesCommands(
    object items,
    IServiceProvider serviceProvider)
  {
    this.CheckArgumentNotNull(items, nameof (items));
    this.CheckArgumentCastToType<ISelectedItems>(items, nameof (items));
    int num = PDMPlugin.CheckSelectedItems((ISelectedItems) items, serviceProvider);
    PDMSubstitutesCommands substitutesCommands = PDMSubstitutesCommands.None;
    if (num == 1 || num == 2)
      substitutesCommands |= PDMSubstitutesCommands.CreateSubstitutesGroup;
    if (num > 0)
      substitutesCommands = substitutesCommands | PDMSubstitutesCommands.MakeActualSubstitute | PDMSubstitutesCommands.EditSubstitutesGroup | PDMSubstitutesCommands.DeleteSubstitutesGroup;
    return substitutesCommands;
  }

  public PDMSubstitutesCommands GetEnabledSubstitutesCommands(int parObjectType, List<int> relTypes)
  {
    int num = this.CheckRelTypesFull(parObjectType, relTypes);
    PDMSubstitutesCommands substitutesCommands = PDMSubstitutesCommands.None;
    if (num > 0)
      substitutesCommands |= PDMSubstitutesCommands.CreateSubstitutesGroup;
    if (num > 0)
      substitutesCommands = substitutesCommands | PDMSubstitutesCommands.MakeActualSubstitute | PDMSubstitutesCommands.EditSubstitutesGroup | PDMSubstitutesCommands.DeleteSubstitutesGroup;
    return substitutesCommands;
  }

  private void CheckArgumentNotNull(object argument, string argumentName)
  {
    if (argument == null)
      throw new ArgumentNullException(argumentName);
  }

  private void CheckArgumentCastToType<T>(object argument, string argumentName)
  {
    if (!(argument is T))
      throw new ArgumentException($"Argument '{argumentName}' must be {typeof (T).ToString()}");
  }

  private int CheckRelTypesFull(int parObjectType, List<int> items)
  {
    int num = -1;
    if (items == null || items.Count <= 0)
      return -4;
    if (!MetaDataHelper.HasObjectTypeSubstRelTypes(parObjectType))
      return -2;
    string empty = string.Empty;
    for (int index = 0; index < items.Count; ++index)
    {
      int relTypeID = items[index];
      if (relTypeID == -1 || !MetaDataHelper.HasRelationTypeSubstitutes(relTypeID))
        return -1;
      if (num == -1)
        num = relTypeID;
      if (num != relTypeID)
        return 0;
    }
    return num >= 0 ? 3 : 2;
  }

  private void ExecuteSubstitutesEditor(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    object additionalInfo,
    SubstitutesEditorCommand substitutesEditorCommand,
    long desiredGroupNumber = -1)
  {
    this.CheckArgumentNotNull((object) selectedItems, nameof (selectedItems));
    if (!(selectedItems.GetParentData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID parentData))
      return;
    long[] numArray = new long[selectedItems.Count];
    for (int index = 0; index < selectedItems.Count; ++index)
      numArray[index] = (selectedItems.GetItemData(index, typeof (IDBTypedObjectID)) as IDBTypedObjectID).ObjectID;
    long[] NewRels = (long[]) null;
    long[] DelRels = (long[]) null;
    long[] ChRels = (long[]) null;
    long[] SubstRels = (long[]) null;
    Dictionary<long, long> ChkOuts = (Dictionary<long, long>) null;
    int num = (int) ArtSubstitutionsEditor.Execute(string.Empty, selectedItems, serviceProvider, substitutesEditorCommand, desiredGroupNumber, out NewRels, out DelRels, out ChRels, out SubstRels, out ChkOuts);
    if (NewRels.Length != 0)
      this._notificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", (IList<long>) NewRels));
    if (DelRels.Length != 0)
      this._notificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsRemoved", (IList<long>) DelRels));
    if (ChRels.Length != 0)
      this._notificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsChanged", (IList<long>) ChRels));
    if (SubstRels.Length != 0)
      this._notificationService.FireEvent((object) null, (NotificationEventArgs) new DBSubstitutesEventArgs("SubstitutesChanged", (IList<long>) SubstRels, parentData.ObjectID));
    if (ChkOuts.Count <= 0)
      return;
    List<long> objectIDs = new List<long>(ChkOuts.Count);
    List<long> newObjectIDs = new List<long>(ChkOuts.Count);
    foreach (KeyValuePair<long, long> keyValuePair in ChkOuts)
    {
      objectIDs.Add(keyValuePair.Key);
      newObjectIDs.Add(keyValuePair.Value);
    }
    this._notificationService.FireEvent((object) null, (NotificationEventArgs) new DBObjectsCheckOutEventArgs("ObjectsCheckedOut", (IList<long>) objectIDs, (IList<long>) newObjectIDs, false));
  }

  private void DeleteSubstitutesGroup(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    object additionalInfo)
  {
    ISubstitutesClientService substitutesClientService = ServiceLocator.Get<ISubstitutesClientService>();
    if (!(selectedItems.GetParentData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID parentData))
      return;
    long objectId = parentData.ObjectID;
    if (ObjectHelper.IsUnknownObjectVersionID(objectId))
      return;
    List<int> intList = new List<int>();
    int index = 0;
    for (int count = selectedItems.Count; index < count; ++index)
    {
      if (selectedItems.GetItemData(index, typeof (IDBRelationID)) is IDBRelationID itemData && itemData.RelationType != -1 && !intList.Contains(itemData.RelationType))
        intList.Add(itemData.RelationType);
    }
    foreach (int relationTypeID in intList)
      substitutesClientService.RemoveSubstitutes(objectId, relationTypeID);
  }
}

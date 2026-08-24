// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Mbom.MbomCommandsProvider
// Assembly: Intermech.Mbom, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 13559C9A-4DBC-479B-BA71-AFEA0247DEC7
// Assembly location: D:\IPS\Client\Intermech.Mbom.dll
// XML documentation location: D:\IPS\Client\Intermech.Mbom.xml

using Intermech.DataFormats;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Search.Mbom;

public sealed class MbomCommandsProvider : ICommandsProvider
{
  private IMbomClientService _mbomClientService;

  public MbomCommandsProvider(IMbomClientService mbomClientService)
  {
    this._mbomClientService = mbomClientService != null ? mbomClientService : throw new ArgumentNullException(nameof (mbomClientService));
  }

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    CommandsInfo mergedCommands = new CommandsInfo();
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (this.CheckParamsForCreateMbom(items, viewServices, out typedObjectID))
      mergedCommands.Add("CreateMbom", new CommandInfo(0, new ClickEventHandler(this.CreateMbom)));
    if (this.CheckParamsForEditMbom(items, viewServices, out typedObjectID))
      mergedCommands.Add("EditDocument", new CommandInfo(0, new ClickEventHandler(this.EditMbom)));
    return mergedCommands;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  private bool CheckParamsForCreateMbom(
    ISelectedItems items,
    IServiceProvider viewServices,
    out IDBTypedObjectID typedObjectID)
  {
    typedObjectID = (IDBTypedObjectID) null;
    INodeID itemId = items.GetItemID(0);
    return (itemId == null || itemId.CategoryID == 1) && SelectedItemsHelper.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeID(items, out typedObjectID) && typedObjectID.ObjectType == MbomConstants.AssemblyUnitObjectTypeID && (!(viewServices.GetService(typeof (IViewState)) is IViewState service) || !service.ViewState.HasFlag((Enum) ViewStateFlags.NodeInViews));
  }

  private void CreateMbom(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (!this.CheckParamsForCreateMbom(items, viewServices, out typedObjectID))
      throw new ArgumentException();
    this._mbomClientService.CreateMbom(typedObjectID.ObjectID);
  }

  private bool CheckParamsForEditMbom(
    ISelectedItems items,
    IServiceProvider viewServices,
    out IDBTypedObjectID typedObjectID)
  {
    return SelectedItemsHelper.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeID(items, out typedObjectID) && typedObjectID.ObjectType == MbomConstants.MbomObjectTypeID;
  }

  private void EditMbom(ISelectedItems items, IServiceProvider viewServices, object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (!this.CheckParamsForEditMbom(items, viewServices, out typedObjectID))
      throw new ArgumentException();
    this._mbomClientService.EditMbom(typedObjectID.ObjectID);
  }
}

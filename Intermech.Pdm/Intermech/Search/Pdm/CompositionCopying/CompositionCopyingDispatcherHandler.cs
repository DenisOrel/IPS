// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionCopying.CompositionCopyingDispatcherHandler
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Search.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Search.Pdm.CompositionCopying;

internal sealed class CompositionCopyingDispatcherHandler
{
  private ICompositionCopyingClientService _compositionCopyingClientService;

  public CompositionCopyingDispatcherHandler(
    ICompositionCopyingClientService compositionCopyingClientService)
  {
    this._compositionCopyingClientService = compositionCopyingClientService != null ? compositionCopyingClientService : throw new ArgumentNullException(nameof (compositionCopyingClientService));
  }

  public void FindHandlerBySelectedItems(object sender, FindCompositionCopyingHandlerEventArgs e)
  {
    if (e == null)
      throw new ArgumentNullException(nameof (e));
    if (e.Handler != null)
      return;
    int[] allowableForCreateCopyObjectTypes = ObjectTypeHelper.GetDescendantsAndSelf(new int[3]
    {
      Constants.AssemblyUnitObjectTypeID,
      CompositionCopyingConstants.ComplexObjectTypeID,
      CompositionCopyingConstants.SetOfProductsObjectTypeID
    });
    NodeID objectNodeID;
    if (!this.CheckSelectedItemsForCreateCompositionByPrototype(e.Items, allowableForCreateCopyObjectTypes, out objectNodeID))
      return;
    e.Handler = (Action) (() => this._compositionCopyingClientService.CreateCompositionByPrototype(objectNodeID.ObjectID, allowableForCreateCopyObjectTypes, new int[1]
    {
      CompositionCopyingConstants.ProductCompositionRelationTypeID
    }));
  }

  private bool CheckSelectedItemsForCreateCompositionByPrototype(
    ISelectedItems selectedItems,
    int[] allowableForCreateCopyObjectTypes,
    out NodeID objectNodeID)
  {
    if (SelectedItemsHelper.TryGetSingleObjectNodeIDWithObjectVersionIDObjectTypeID(selectedItems, out objectNodeID) && ((IEnumerable<int>) allowableForCreateCopyObjectTypes).Contains<int>(objectNodeID.ObjectTypeID))
      return true;
    objectNodeID = (NodeID) null;
    return false;
  }
}

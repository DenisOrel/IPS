// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.MRP2SelectedItemsAnalyzer
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.MRP2;

internal class MRP2SelectedItemsAnalyzer : SelectedItemsAnalyzer
{
  private List<int> _rootTypeIDs;

  public MRP2SelectedItemsAnalyzer()
  {
    this._rootTypeIDs = new List<int>();
    this._rootTypeIDs.Add(MetaDataHelper.GetObjectTypeID("cad00268-306c-11d8-b4e9-00304f19f545"));
    this._rootTypeIDs.Add(MRP2Consts.objtypeIdProductionCopy);
  }

  public override SelectedItemsAnalyzerResult Analyze(
    ISelectionWindow sender,
    ISelectedItemsHost itemsHost)
  {
    SelectedItemsAnalyzerResult itemsAnalyzerResult = base.Analyze(sender, itemsHost);
    if (itemsAnalyzerResult == SelectedItemsAnalyzerResult.Enabled)
    {
      if (itemsHost.SelectedItems == null)
        return SelectedItemsAnalyzerResult.Disabled;
      INodeID itemId = itemsHost.SelectedItems.GetItemID(0);
      itemsAnalyzerResult = itemId == null || !this.IsAllowedType(itemId.TypeID) || itemId.CategoryID != 1 ? SelectedItemsAnalyzerResult.Disabled : SelectedItemsAnalyzerResult.Enabled;
    }
    return itemsAnalyzerResult;
  }

  private bool IsAllowedType(int typeID)
  {
    if (this._rootTypeIDs == null)
      return false;
    return typeID == -1 || this._rootTypeIDs.Contains(typeID) || this._rootTypeIDs.Any<int>((Func<int, bool>) (x => MetaDataHelper.IsObjectTypeChildOf(typeID, x)));
  }
}

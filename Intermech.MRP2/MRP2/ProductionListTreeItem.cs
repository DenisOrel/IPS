// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionListTreeItem
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.MRP2;

internal class ProductionListTreeItem : List<ProductionListTreeItem>
{
  internal long _object_id;
  internal int _object_type;
  internal long _checkout_by;
  internal Guid _prjGuid;
  internal ProductionListTreeItem _parent;
  internal DataRow _row;
  private ProductionListTree _tree;

  public ProductionListTreeItem(ProductionListTree parent, long object_id)
  {
    this._parent = (ProductionListTreeItem) null;
    this._row = (DataRow) null;
    this._tree = parent;
    this._object_id = object_id;
    this._prjGuid = Guid.Empty;
    this._object_type = 0;
    this._checkout_by = 0L;
  }

  public ProductionListTreeItem(ProductionListTreeItem parent, DataRow row)
  {
    this._parent = parent;
    this._row = row;
    this._tree = parent._tree;
    this._object_id = DataSetProcessor.GetInt64Value(row, $"{-2}", 0L);
    this._object_type = DataSetProcessor.GetInt32Value(row, $"{-7}", -1);
    this._prjGuid = DataSetProcessor.GetGuidValue(row, $"{-26}", Guid.Empty);
    this._checkout_by = DataSetProcessor.GetInt64Value(row, $"{-6}", 0L);
    if (this._tree.target.IndexOf(this._prjGuid) < 0)
      return;
    this._tree.targetItems.Add(this);
  }

  public long ObjectID => this._object_id;
}

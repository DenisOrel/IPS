// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImCatalogRecItemFactory
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal class ImCatalogRecItemFactory : ImDataTableItemFactory
{
  private int idxLEVEL = -1;

  public ImCatalogRecItemFactory(
    IImportingData cacheData,
    string tabName,
    IDataReader idr,
    IAppManager appMgr,
    ICollection<GroupAttribute> fieldsCollection)
    : base(cacheData, tabName, idr, appMgr, fieldsCollection, DataTableItemOptions.None)
  {
    this.idxLEVEL = this.getFieldIndex("F_LEVEL");
  }

  public override object NewItem(IDataReader idr)
  {
    ImCatalogRecItem record = new ImCatalogRecItem();
    this.addFields((ImDataTableItem) record, idr);
    record.RecLevel = this.getInt32(idr, this.idxLEVEL);
    return (object) record;
  }
}

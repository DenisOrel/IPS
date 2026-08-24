// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ItemFactories.ProductionListItemAttributeFactory
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData.ItemFactories;

internal sealed class ProductionListItemAttributeFactory : PumpItemFactory
{
  private readonly int _idxID;
  private readonly int _idxLabel;
  private readonly int _idxField;
  private readonly ITableFieldInfo[] _dataTableFields;
  public static string TableName = "ZPC_PARAMS_CFG";
  public static string TableColumns = "PARAM_ID, P_LABEL, P_FIELD";

  public ProductionListItemAttributeFactory(
    string tableName,
    IDataReader dataReader,
    ITableFieldInfo[] dataTableFields,
    IAppManager appManager)
    : base(tableName, dataReader, appManager)
  {
    this._idxID = this.getFieldIndex("PARAM_ID");
    this._idxLabel = this.getFieldIndex("P_LABEL");
    this._idxField = this.getFieldIndex("P_FIELD");
    this._dataTableFields = dataTableFields;
  }

  public ProductionListItemAttribute NewItem(IDataReader idr)
  {
    ProductionListItemAttribute item = new ProductionListItemAttribute()
    {
      ParamID = this.getInt32(idr, this._idxID),
      AttributeName = this.getString(idr, this._idxLabel),
      DBFieldName = this.getString(idr, this._idxField)
    };
    ITableFieldInfo tableFieldInfo = Array.Find<ITableFieldInfo>(this._dataTableFields, (Predicate<ITableFieldInfo>) (x => x.ColumnName.Equals(item.DBFieldName)));
    item.AttributeType = Helper.GetFieldType(tableFieldInfo.DataType.FullName, tableFieldInfo.NumericScale, tableFieldInfo.IsLong);
    item.AttributeSize = tableFieldInfo.ColumnSize;
    return item;
  }
}

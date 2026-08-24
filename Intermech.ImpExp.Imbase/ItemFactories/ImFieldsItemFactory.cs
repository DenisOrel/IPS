// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImFieldsItemFactory
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal sealed class ImFieldsItemFactory : PumpItemFactory
{
  public static string TableName = "IM_FIELDS";
  private static int idxKey = -1;
  private static int idxTableId = -1;
  private static int idxField = -1;
  private static int idxLongName = -1;
  private static int idxShortName = -1;
  private static int idxUnits = -1;
  private static int idxSort = -1;
  private static int idxWidth = -1;
  private static int idxFlags = -1;
  private static int idxType = -1;
  private static int idxRequired = -1;
  private static int idxDataType = -1;
  private static int idxEnterMode = -1;
  private static int idxData = -1;

  public ImFieldsItemFactory(IDataReader dataReader, IAppManager appManager)
    : base(ImFieldsItemFactory.TableName, dataReader, appManager)
  {
    string fieldName1 = "F_KEY";
    string fieldName2 = "F_TABLE_ID";
    string fieldName3 = "F_FIELD";
    string fieldName4 = "F_LONGNAME";
    string fieldName5 = "F_SHORTNAME";
    string fieldName6 = "F_UNITS";
    string fieldName7 = "F_SORT";
    string fieldName8 = "F_WIDTH";
    string fieldName9 = "F_FLAGS";
    string fieldName10 = "F_TYPE";
    string fieldName11 = "F_REQUIRED";
    string fieldName12 = "F_DATATYPE";
    string fieldName13 = "F_ENTERMODE";
    string fieldName14 = "F_DATA";
    ImFieldsItemFactory.idxKey = this.getFieldIndex(fieldName1);
    ImFieldsItemFactory.idxTableId = this.getFieldIndex(fieldName2);
    ImFieldsItemFactory.idxField = this.getFieldIndex(fieldName3);
    ImFieldsItemFactory.idxLongName = this.getFieldIndex(fieldName4);
    ImFieldsItemFactory.idxShortName = this.getFieldIndex(fieldName5);
    ImFieldsItemFactory.idxUnits = this.getFieldIndex(fieldName6);
    ImFieldsItemFactory.idxSort = this.getFieldIndex(fieldName7);
    ImFieldsItemFactory.idxWidth = this.getFieldIndex(fieldName8);
    ImFieldsItemFactory.idxFlags = this.getFieldIndex(fieldName9);
    ImFieldsItemFactory.idxType = this.getFieldIndex(fieldName10);
    ImFieldsItemFactory.idxRequired = this.getFieldIndex(fieldName11);
    ImFieldsItemFactory.idxDataType = this.getFieldIndex(fieldName12);
    ImFieldsItemFactory.idxEnterMode = this.getFieldIndex(fieldName13);
    ImFieldsItemFactory.idxData = this.getFieldIndex(fieldName14);
  }

  public override object NewItem(IDataReader idr)
  {
    return (object) new ImFieldsItem(this.getInt32(idr, ImFieldsItemFactory.idxKey), this.getInt32(idr, ImFieldsItemFactory.idxTableId), this.getString(idr, ImFieldsItemFactory.idxField), this.getString(idr, ImFieldsItemFactory.idxLongName), this.getString(idr, ImFieldsItemFactory.idxShortName), this.getString(idr, ImFieldsItemFactory.idxUnits), this.getInt32(idr, ImFieldsItemFactory.idxSort), this.getInt32(idr, ImFieldsItemFactory.idxFlags), (ImDataMode) this.getInt32(idr, ImFieldsItemFactory.idxType), this.getInt32(idr, ImFieldsItemFactory.idxRequired), (ImDataTypeEx) this.getInt32(idr, ImFieldsItemFactory.idxDataType), (long) this.getInt32(idr, ImFieldsItemFactory.idxWidth), (ImEnterMode) this.getInt32(idr, ImFieldsItemFactory.idxEnterMode), this.getString(idr, ImFieldsItemFactory.idxData));
  }
}

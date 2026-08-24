// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImCatalogItemFactory
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal class ImCatalogItemFactory : PumpItemFactory
{
  private static int idxKEY = -1;
  private static int idxOWNER = -1;
  private static int idxLEVEL = -1;
  private static int idxNAME = -1;
  private static int idxSORT = -1;
  private static int idxMASK = -1;
  private static int idxTAG1 = -1;
  private static int idxTAG2 = -1;
  private static int idxTEXTID = -1;
  private static int idxGRAPHID = -1;
  private static int idxCREATED = -1;
  private static int idxUSER = -1;
  private static int idxTAG3 = -1;
  private static int idxTAG4 = -1;

  public ImCatalogItemFactory(string tabName, IDataReader idr, IAppManager appMgr)
    : base(tabName, idr, appMgr)
  {
    string fieldName1 = "F_KEY";
    string fieldName2 = "F_OWNER";
    string fieldName3 = "F_LEVEL";
    string fieldName4 = "F_NAME";
    string fieldName5 = "F_SORT";
    string fieldName6 = "F_MASK";
    string fieldName7 = "F_TAG1";
    string fieldName8 = "F_TAG2";
    string fieldName9 = "F_TEXTID";
    string fieldName10 = "F_GRAPHID";
    string fieldName11 = "F_CREATED";
    string fieldName12 = "F_USER";
    string fieldName13 = "F_TAG3";
    string fieldName14 = "F_TAG4";
    ImCatalogItemFactory.idxKEY = this.getFieldIndex(fieldName1);
    ImCatalogItemFactory.idxOWNER = this.getFieldIndex(fieldName2);
    ImCatalogItemFactory.idxLEVEL = this.getFieldIndex(fieldName3);
    ImCatalogItemFactory.idxNAME = this.getFieldIndex(fieldName4);
    ImCatalogItemFactory.idxSORT = this.getFieldIndex(fieldName5);
    ImCatalogItemFactory.idxMASK = this.getFieldIndex(fieldName6);
    ImCatalogItemFactory.idxTAG1 = this.getFieldIndex(fieldName7);
    ImCatalogItemFactory.idxTAG2 = this.getFieldIndex(fieldName8);
    ImCatalogItemFactory.idxTEXTID = this.getFieldIndex(fieldName9);
    ImCatalogItemFactory.idxGRAPHID = this.getFieldIndex(fieldName10);
    ImCatalogItemFactory.idxCREATED = this.getFieldIndex(fieldName11);
    ImCatalogItemFactory.idxUSER = this.getFieldIndex(fieldName12);
    ImCatalogItemFactory.idxTAG3 = this.getFieldIndex(fieldName13);
    ImCatalogItemFactory.idxTAG4 = this.getFieldIndex(fieldName14);
  }

  public override object NewItem(IDataReader idr)
  {
    return (object) new ImCatalogItem()
    {
      RecKey = this.getInt32(idr, ImCatalogItemFactory.idxKEY),
      RecOwner = this.getInt32(idr, ImCatalogItemFactory.idxOWNER),
      RecLevel = this.getInt32(idr, ImCatalogItemFactory.idxLEVEL),
      RecNAME = this.getString(idr, ImCatalogItemFactory.idxNAME).Trim(),
      RecSORT = this.getInt32(idr, ImCatalogItemFactory.idxSORT),
      RecMASK = this.getInt32(idr, ImCatalogItemFactory.idxMASK),
      RecTag1 = this.getInt32(idr, ImCatalogItemFactory.idxTAG1),
      RecTag2 = this.getInt32(idr, ImCatalogItemFactory.idxTAG2),
      RecTextID = this.getInt32(idr, ImCatalogItemFactory.idxTEXTID),
      RecGraphID = this.getInt32(idr, ImCatalogItemFactory.idxGRAPHID),
      RecCreated = this.getDateTime(idr, ImCatalogItemFactory.idxCREATED),
      RecUser = this.getString(idr, ImCatalogItemFactory.idxUSER).Trim(),
      RecTag3 = this.getInt32(idr, ImCatalogItemFactory.idxTAG3),
      RecTag4 = this.getInt32(idr, ImCatalogItemFactory.idxTAG4)
    };
  }
}

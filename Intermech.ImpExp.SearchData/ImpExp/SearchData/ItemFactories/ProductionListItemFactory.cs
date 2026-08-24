// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ItemFactories.ProductionListItemFactory
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData.ItemFactories;

internal sealed class ProductionListItemFactory : PumpItemFactory
{
  private readonly int _idxID;
  private readonly int _idxZakazID;
  private readonly int _idxZRecID;
  private readonly int _idxZParentRecID;
  private readonly int _idxPartID;
  private readonly int _idxPartVer;
  private readonly int _idxCountPC;
  private readonly int _idxMUShortName;
  private readonly int _idxRazdel;
  private readonly int _idxPositio;
  private readonly int _idxNote;
  private readonly int _idxLinkType;
  private readonly int _idxFormat;
  private readonly int _idxMaterial;
  private readonly int _idxZVer;
  private readonly int _idxZVer2;
  private readonly int _idxZVer3;
  private readonly int _idxChgCode;
  private readonly int _idxOPCode;
  private readonly int _idxOPVars;
  private readonly int _idxZFrom;
  private readonly int _idxZTill;
  private readonly int _idxActualVersionID;
  private readonly Dictionary<string, int> _additionalFields;
  public static string TableName = "ZPC";

  public ProductionListItemFactory(
    IDataReader dataReader,
    List<string> additionalFields,
    IAppManager appManager)
    : base(ProductionListItemFactory.TableName, dataReader, appManager)
  {
    this._idxID = this.getFieldIndex("prjlink_id");
    this._idxZakazID = this.getFieldIndex("zakaz_id");
    this._idxZRecID = this.getFieldIndex("zrec_id");
    this._idxZParentRecID = this.getFieldIndex("parent_zrec_id");
    this._idxPartID = this.getFieldIndex("part_aid");
    this._idxPartVer = this.getFieldIndex("part_ver");
    this._idxCountPC = this.getFieldIndex("count_pc");
    this._idxMUShortName = this.getFieldIndex("mu_short_name");
    this._idxRazdel = this.getFieldIndex("razdel");
    this._idxPositio = this.getFieldIndex("positio");
    this._idxNote = this.getFieldIndex("note");
    this._idxLinkType = this.getFieldIndex("link_type");
    this._idxFormat = this.getFieldIndex("format");
    this._idxMaterial = this.getFieldIndex("z_material");
    this._idxZVer = this.getFieldIndex("z_ver");
    this._idxZVer2 = this.getFieldIndex("z_ver2");
    this._idxZVer3 = this.getFieldIndex("z_ver3");
    this._idxChgCode = this.getFieldIndex("chg_code");
    this._idxOPCode = this.getFieldIndex("opcode");
    this._idxOPVars = this.getFieldIndex("opvars");
    this._idxActualVersionID = this.getFieldIndex("art_ver_id");
    this._idxZFrom = this.getFieldIndex("z_from");
    this._idxZTill = this.getFieldIndex("z_till");
    if (additionalFields == null || additionalFields.Count <= 0)
      return;
    this._additionalFields = new Dictionary<string, int>(additionalFields.Count);
    foreach (string additionalField in additionalFields)
      this._additionalFields.Add(additionalField, this.getFieldIndex(additionalField));
  }

  public ProductionListItem NewItem(IDataReader idr)
  {
    ProductionListItem productionListItem = new ProductionListItem()
    {
      ID = this.getInt32(idr, this._idxID),
      ZakazID = this.getInt32(idr, this._idxZakazID),
      ZakazVer = this.getInt32(idr, this._idxZVer),
      ZRecID = this.getInt32(idr, this._idxZRecID),
      ZParentRecID = this.getInt32(idr, this._idxZParentRecID),
      PartArticleID = this.getInt32(idr, this._idxPartID),
      PartArticleVer = ProductionListsCache.GetVersionNo(this.getInt32(idr, this._idxPartVer), this.getInt32(idr, this._idxActualVersionID)),
      CountPC = this.getDouble(idr, this._idxCountPC),
      MUShortName = this.getString(idr, this._idxMUShortName),
      Razdel = this.getInt32(idr, this._idxRazdel),
      Positio = this.getString(idr, this._idxPositio),
      Note = this.getString(idr, this._idxNote),
      LinkType = this.getString(idr, this._idxLinkType),
      Format = this.getString(idr, this._idxFormat),
      Material = this.getString(idr, this._idxMaterial),
      ZVer2 = this.getInt32(idr, this._idxZVer2),
      ZVer3 = this.getInt32(idr, this._idxZVer3),
      ChgCode = this.getInt32(idr, this._idxChgCode),
      ZFrom = this.getInt32(idr, this._idxZFrom),
      ZTill = this.getInt32(idr, this._idxZTill),
      OPCode = this.getInt32(idr, this._idxOPCode),
      OPVars = this.getInt32(idr, this._idxOPVars)
    };
    if (this._additionalFields != null && this._additionalFields.Count > 0)
    {
      productionListItem.AdditionalItems = new Dictionary<string, object>(this._additionalFields.Count);
      foreach (KeyValuePair<string, int> additionalField in this._additionalFields)
        productionListItem.AdditionalItems.Add(additionalField.Key, this.getObject(idr, additionalField.Value));
    }
    return productionListItem;
  }
}

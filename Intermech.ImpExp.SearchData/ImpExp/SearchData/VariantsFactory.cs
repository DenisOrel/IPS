// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.VariantsFactory
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal class VariantsFactory : PumpItemFactory
{
  public static string TableName = "VARIANTS";
  public static string TableNameVersions = "V_VARIANTS";
  private bool _isVersions;

  public VariantsFactory(IDataReader dataReader, IAppManager appManager, bool isVersions)
    : base(VariantsFactory.TableName, dataReader, appManager)
  {
    this._isVersions = isVersions;
  }

  public IVariantsItem NewItem(IDataReader idr)
  {
    VariantsFactory.VariantsItem variantsItem = new VariantsFactory.VariantsItem();
    variantsItem.prjLinkID = this.getInt32(idr, 0);
    variantsItem.projAID = this.getInt32(idr, 1);
    variantsItem.partAID = this.getInt32(idr, 2);
    variantsItem.countPC = this.getDouble(idr, 3);
    variantsItem.muID = this.getInt32(idr, 4);
    variantsItem.razdel = this.getInt32(idr, 5);
    variantsItem.positio = this.getString(idr, 6);
    variantsItem.note = this.getString(idr, 7);
    variantsItem.varMode = this.getString(idr, 8);
    variantsItem.varNo = this.getInt32(idr, 9);
    variantsItem.format = this.getString(idr, 10);
    variantsItem.prID = this.getInt32(idr, 11);
    variantsItem.ctxID = this.getInt32(idr, 12);
    variantsItem.ctxFL = this.getInt32(idr, 13);
    if (this._isVersions)
      variantsItem.artInfo = new ArtIDInfo(this.getInt32(idr, 14), this.getInt32(idr, 15));
    return (IVariantsItem) variantsItem;
  }

  private class VariantsItem : IVariantsItem
  {
    internal int prjLinkID = -1;
    internal int projAID = -1;
    internal int partAID = -1;
    internal double countPC = -1.0;
    internal int muID = -1;
    internal int razdel = -1;
    internal string positio = string.Empty;
    internal string note = string.Empty;
    internal string varMode = string.Empty;
    internal int varNo = -1;
    internal string format = string.Empty;
    internal int prID = -1;
    internal int ctxID = -1;
    internal int ctxFL = -1;
    internal ArtIDInfo artInfo;

    public int PrjLinkID => this.prjLinkID;

    public int ProjAID => this.projAID;

    public int PartAID => this.partAID;

    public double CountPC => this.countPC;

    public int MuID => this.muID;

    public int Razdel => this.razdel;

    public string Positio => this.positio;

    public string Note => this.note;

    public string Format => this.format;

    public int PrID => this.prID;

    public int CtxID => this.ctxID;

    public int CtxFL => this.ctxFL;

    public string VarMode => this.varMode;

    public int VarNo => this.varNo;

    public ArtIDInfo ArtInfo => this.artInfo;
  }
}

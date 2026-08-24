// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.BoundPosFactory
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal class BoundPosFactory : PumpItemFactory
{
  public static string TableName = "BOUNDPOS";
  public static string TableNameVersions = "V_BOUNDPOS";
  private bool _isVersions;

  public BoundPosFactory(IDataReader dataReader, IAppManager appManager, bool isVersions)
    : base(BoundPosFactory.TableName, dataReader, appManager)
  {
    this._isVersions = isVersions;
  }

  public IBoundPosItem NewItem(IDataReader idr)
  {
    BoundPosFactory.BoundPosItem boundPosItem = new BoundPosFactory.BoundPosItem();
    boundPosItem.prjLinkID = this.getInt32(idr, 0);
    boundPosItem.projAID = this.getInt32(idr, 1);
    boundPosItem.partAID = this.getInt32(idr, 2);
    boundPosItem.countPC = BasePumpHelper.dbType != BasePumpHelper.DBType.Interbase ? this.getDouble(idr, 3) : (double) this.getFloat(idr, 3);
    boundPosItem.muID = this.getInt32(idr, 4);
    boundPosItem.razdel = this.getInt32(idr, 5);
    boundPosItem.positio = this.getString(idr, 6);
    boundPosItem.note = this.getString(idr, 7);
    boundPosItem.format = this.getString(idr, 8);
    boundPosItem.prID = this.getInt32(idr, 9);
    boundPosItem.ctxID = this.getInt32(idr, 10);
    boundPosItem.ctxFL = this.getInt32(idr, 11);
    if (this._isVersions)
      boundPosItem.artInfo = new ArtIDInfo(this.getInt32(idr, 12), this.getInt32(idr, 13));
    return (IBoundPosItem) boundPosItem;
  }

  private class BoundPosItem : IBoundPosItem
  {
    internal int prjLinkID = -1;
    internal int projAID = -1;
    internal int partAID = -1;
    internal double countPC = -1.0;
    internal int muID = -1;
    internal int razdel = -1;
    internal string positio = string.Empty;
    internal string note = string.Empty;
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

    public ArtIDInfo ArtInfo => this.artInfo;
  }
}

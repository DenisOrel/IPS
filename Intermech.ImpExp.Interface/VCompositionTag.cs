// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.VCompositionTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.IO;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class VCompositionTag : CompositionTag, ITagImportObject
{
  public int PrjArtID;
  public int PrjArtVerID;
  public int PartArtID;
  public int PartArtVerID;

  /// <summary>Constructor</summary>
  /// <param name="prjArtID">ArtID базового объекта projectа в базе SEARCH</param>
  /// <param name="prjArtVerID">версия базового объекта projectа</param>
  /// <param name="partArtID">ArtID базового объекта partа в базе SEARCH</param>
  /// <param name="partArtVerID">версия базового объекта parta</param>
  public VCompositionTag(
    int projAID,
    int partAID,
    int prjArtID,
    int prjArtVerID,
    int partArtID,
    int partArtVerID)
    : base(projAID, partAID)
  {
    this.PrjArtID = prjArtID;
    this.PrjArtVerID = prjArtVerID;
    this.PartArtID = partArtID;
    this.PartArtVerID = partArtVerID;
  }

  public VCompositionTag()
  {
    this.PrjArtID = 0;
    this.PrjArtVerID = 0;
    this.PartArtID = 0;
    this.PartArtVerID = 0;
  }

  protected override void InternalSave(BinaryWriter bw)
  {
    base.InternalSave(bw);
    bw.Write(this.PrjArtID);
    bw.Write(this.PrjArtVerID);
    bw.Write(this.PartArtID);
    bw.Write(this.PartArtVerID);
  }

  protected override void InternalLoad(BinaryReader br)
  {
    base.InternalLoad(br);
    this.PrjArtID = br.ReadInt32();
    this.PrjArtVerID = br.ReadInt32();
    this.PartArtID = br.ReadInt32();
    this.PartArtVerID = br.ReadInt32();
  }

  public override short ClassID => 16 /*0x10*/;
}

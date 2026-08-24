// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.DocTypeItem
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using System;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class DocTypeItem : IDocTypeItem
{
  internal int docType;
  internal string docCode;
  internal string docName;
  internal string docExt;
  internal string bitmap;
  internal int docColor;
  internal int drawStamp;
  internal int suffix;
  internal string linkedExt;
  internal int refSetup;
  internal byte[] fileBody;
  internal string protoName;
  internal string classif;
  internal string dTName;
  internal string dTCode;
  internal int strongSign;
  internal string signStamp;
  private Guid guid;
  private Guid parentID;
  private Guid defRelation;
  private Guid lcScheme;
  private ObjectVersionModes versionMode = ObjectVersionModes.MultiVersion;
  private bool anyAttribute = true;
  private byte[] icon;

  public DocTypeItem()
  {
  }

  public DocTypeItem(
    int DocType,
    string DocCode,
    string DocName,
    string DocExt,
    string Bitmap,
    int DocColor,
    int DrawStamp,
    int Suffix,
    string LinkedExt,
    int RefSetup,
    byte[] FileBody,
    string ProtoName,
    string Classif,
    string DTName,
    string DTCode,
    int StrongSign,
    string SignStamp)
  {
    this.docType = DocType;
    this.docCode = DocCode;
    this.docName = DocName;
    this.docExt = DocExt;
    this.bitmap = Bitmap;
    this.docColor = DocColor;
    this.drawStamp = DrawStamp;
    this.suffix = Suffix;
    this.linkedExt = LinkedExt;
    this.refSetup = RefSetup;
    this.fileBody = FileBody;
    this.protoName = ProtoName;
    this.classif = Classif;
    this.dTName = DTName;
    this.dTCode = DTCode;
    this.strongSign = StrongSign;
    this.signStamp = SignStamp;
  }

  public int DocType => this.docType;

  public string DocCode
  {
    get => this.docCode;
    set => this.docCode = value;
  }

  public string DocName
  {
    get => this.docName;
    set => this.docName = value;
  }

  public string DocExt
  {
    get => this.docExt;
    set => this.docExt = value;
  }

  public string Bitmap => this.bitmap;

  public int DocColor => this.docColor;

  public int DrawStamp => this.drawStamp;

  public int Suffix => this.suffix;

  public string LinkedExt
  {
    get => this.linkedExt;
    set => this.linkedExt = value;
  }

  public int RefSetup => this.refSetup;

  public byte[] FileBody => this.fileBody;

  public string ProtoName => this.protoName;

  public string Classif => this.classif;

  public string DTName => this.dTName;

  public string DTCode => this.dTCode;

  public int StrongSign => this.strongSign;

  public string SignStamp => this.signStamp;

  public Guid Guid
  {
    get => this.guid;
    set => this.guid = value;
  }

  public Guid ParentID
  {
    get => this.parentID;
    set => this.parentID = value;
  }

  public Guid DefRelation
  {
    get => this.defRelation;
    set => this.defRelation = value;
  }

  public Guid LCScheme
  {
    get => this.lcScheme;
    set => this.lcScheme = value;
  }

  public ObjectVersionModes VersionMode
  {
    get => this.versionMode;
    set => this.versionMode = value;
  }

  public bool AnyAttribute
  {
    get => this.anyAttribute;
    set => this.anyAttribute = value;
  }

  public byte[] Icon
  {
    get => this.icon;
    set => this.icon = value;
  }
}

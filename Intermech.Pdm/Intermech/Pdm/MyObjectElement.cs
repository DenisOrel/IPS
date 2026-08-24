// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.MyObjectElement
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Collections;

#nullable disable
namespace Intermech.Pdm;

[Serializable]
public sealed class MyObjectElement : ICloneable
{
  public long ID;
  public long ObjectID;
  public int ObjectType;
  public long PrjLinkID;
  public int RelationType = -1;
  public string Caption = string.Empty;
  public bool ObjectBool;
  public Guid ObjectGuid = Guid.Empty;
  public long Version;
  public long BaseVersion;
  public ArrayList Tags = new ArrayList(0);

  public MyObjectElement()
  {
  }

  public MyObjectElement(
    long AnID,
    long AnObjectID,
    int AnObjectType,
    long APrjLinkID,
    int ARelationType,
    string ACaption,
    bool AnObjectBool,
    Guid AnObjectGuid,
    long AVersion,
    long ABaseVersion,
    params object[] ATags)
  {
    this.ID = AnID;
    this.ObjectID = AnObjectID;
    this.ObjectType = AnObjectType;
    this.PrjLinkID = APrjLinkID;
    this.RelationType = ARelationType;
    this.Caption = ACaption;
    this.ObjectBool = AnObjectBool;
    this.ObjectGuid = AnObjectGuid;
    this.Version = AVersion;
    this.BaseVersion = ABaseVersion;
    if (this.Tags == null)
      this.Tags = new ArrayList(0);
    this.Tags.Clear();
    if (ATags == null || ATags.Length == 0)
      return;
    for (int index = 0; index < ATags.Length; ++index)
      this.Tags.Add(ATags[index]);
  }

  public void Clear()
  {
    this.ID = 0L;
    this.ObjectID = 0L;
    this.ObjectType = 0;
    this.PrjLinkID = 0L;
    this.RelationType = -1;
    this.Caption = string.Empty;
    this.ObjectBool = false;
    this.ObjectGuid = Guid.Empty;
    this.Version = 0L;
    this.BaseVersion = 0L;
  }

  public override string ToString()
  {
    return $"[{this.ObjectID}.{this.ID}] {this.Caption} ({this.ObjectGuid})";
  }

  public object Clone()
  {
    object[] objArray = (object[]) null;
    if (this.Tags.Count > 0)
    {
      objArray = new object[this.Tags.Count];
      this.Tags.CopyTo((Array) objArray);
    }
    return (object) new MyObjectElement(this.ID, this.ObjectID, this.ObjectType, this.PrjLinkID, this.RelationType, this.Caption, this.ObjectBool, this.ObjectGuid, this.Version, this.BaseVersion, objArray);
  }
}

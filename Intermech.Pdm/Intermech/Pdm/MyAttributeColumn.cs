// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.MyAttributeColumn
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;

#nullable disable
namespace Intermech.Pdm;

[Serializable]
public sealed class MyAttributeColumn : ICloneable
{
  public int AttrID;
  public int Width;
  public int Order = -1;
  public bool Visible;
  [NonSerialized]
  public FieldTypes AttrType;
  [NonSerialized]
  public string Caption = string.Empty;
  [NonSerialized]
  public bool IsRelationAttr = true;
  public object Tag;
  public object[] Tags;

  public MyAttributeColumn()
  {
  }

  public MyAttributeColumn(
    int AnAttrID,
    int AWidth,
    int AnOrder,
    bool AVisible,
    FieldTypes AnAttrType,
    string ACaption,
    bool AnIsRelationAttr,
    object ATag,
    params object[] ATags)
  {
    this.AttrID = AnAttrID;
    this.Width = AWidth;
    this.Order = AnOrder;
    this.Visible = AVisible;
    this.AttrType = AnAttrType;
    this.IsRelationAttr = AnIsRelationAttr;
    this.Caption = ACaption;
    this.Tag = ATag;
    this.Tags = ATags;
  }

  public override string ToString() => this.Caption;

  public object Clone()
  {
    return (object) new MyAttributeColumn(this.AttrID, this.Width, this.Order, this.Visible, this.AttrType, this.Caption, this.IsRelationAttr, this.Tag, this.Tags);
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.InseparableObjectTypesItem
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Tools.Settings.PropertyEditors;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class InseparableObjectTypesItem : ICloneable
{
  public InseparableObjectTypesItem()
  {
    this.LeftTypeId = new LocalId<int>(-1, "Тип не указан");
    this.RightTypeId = new LocalId<int>(-1, "Тип не указан");
  }

  public InseparableObjectTypesItem(LocalId<int> leftTypeID, LocalId<int> rightTypeId)
  {
    this.LeftTypeId = leftTypeID;
    this.RightTypeId = rightTypeId;
  }

  [DisplayName("Тип объекта 1")]
  [Editor(typeof (SelectObjectTypeUIEditor), typeof (UITypeEditor))]
  public LocalId<int> LeftTypeId { get; set; }

  [DisplayName("Тип объекта 2")]
  [Editor(typeof (SelectObjectTypeUIEditor), typeof (UITypeEditor))]
  public LocalId<int> RightTypeId { get; set; }

  [Browsable(false)]
  public string Name => $"{this.LeftTypeId.Name} => {this.RightTypeId.Name}";

  public InseparableObjectTypesItem Clone()
  {
    return new InseparableObjectTypesItem()
    {
      LeftTypeId = (LocalId<int>) this.LeftTypeId.Clone(),
      RightTypeId = (LocalId<int>) this.RightTypeId.Clone()
    };
  }

  object ICloneable.Clone() => (object) this.Clone();

  public override int GetHashCode()
  {
    int id = this.LeftTypeId.Id;
    int hashCode1 = id.GetHashCode();
    id = this.RightTypeId.Id;
    int hashCode2 = id.GetHashCode();
    return hashCode1 ^ hashCode2;
  }

  public override bool Equals(object obj)
  {
    if (!(obj is InseparableObjectTypesItem inseparableObjectTypesItem))
      return base.Equals(obj);
    return inseparableObjectTypesItem.LeftTypeId.Id == this.LeftTypeId.Id && inseparableObjectTypesItem.RightTypeId.Id == this.RightTypeId.Id;
  }
}

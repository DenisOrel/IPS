// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ElementListItemSurrogate
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.MG.Integrator;

[DefaultProperty("ObjectType")]
internal sealed class ElementListItemSurrogate : ICloneable
{
  private GlobalId<int> _objectType;
  private string _suffix;

  [DisplayName("Тип перечня элементов")]
  [Description("Глобальный идентификатор типа перечня элементов")]
  [Editor(typeof (ElementListTypeMarkerUIEditor), typeof (UITypeEditor))]
  public GlobalId<int> ObjectType
  {
    get => this._objectType;
    set => this._objectType = value;
  }

  [DisplayName("Суфикс обозначения")]
  [Description("Суфикс в обозначении соотвествующий этому типу")]
  public string Suffix
  {
    get => this._suffix;
    set => this._suffix = value;
  }

  public ElementListItemSurrogate Clone()
  {
    return new ElementListItemSurrogate()
    {
      _objectType = this._objectType,
      _suffix = this._suffix
    };
  }

  object ICloneable.Clone() => (object) this.Clone();

  public override string ToString() => "Тип перечня элементов";

  public override int GetHashCode()
  {
    int hashCode = 0;
    if (this._objectType != null)
      hashCode ^= this._objectType.GetHashCode();
    return hashCode;
  }

  public override bool Equals(object obj)
  {
    return obj is ElementListItemSurrogate listItemSurrogate && listItemSurrogate._objectType != null ? listItemSurrogate._objectType.Equals((LocalId<int>) this._objectType) : base.Equals(obj);
  }
}

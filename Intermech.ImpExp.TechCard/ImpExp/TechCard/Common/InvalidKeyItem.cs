// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.InvalidKeyItem
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

internal struct InvalidKeyItem(ImportingCategory category, object key) : IEquatable<InvalidKeyItem>
{
  public ImportingCategory Category = category;
  public object Key = key;

  public bool Equals(InvalidKeyItem other)
  {
    return this.Category.Equals((object) other.Category) && this.Key.Equals(other.Key);
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Configurations.ItemWithDescription
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Statistics.Interfaces;
using System;
using System.ComponentModel;
using System.Reflection;

#nullable disable
namespace Intermech.Statistics.Configurations;

internal class ItemWithDescription
{
  public string Description { get; }

  public CollectPeriodsEnum Value { get; }

  public ItemWithDescription(CollectPeriodsEnum value)
  {
    this.Description = !(Attribute.GetCustomAttribute((MemberInfo) value.GetType().GetField(value.ToString()), typeof (DescriptionAttribute)) is DescriptionAttribute customAttribute) ? value.ToString() : customAttribute.Description;
    this.Value = value;
  }

  public override string ToString() => this.Description;
}

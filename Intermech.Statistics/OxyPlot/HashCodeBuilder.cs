// Decompiled with JetBrains decompiler
// Type: OxyPlot.HashCodeBuilder
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot;

public static class HashCodeBuilder
{
  public static int GetHashCode(IEnumerable<object> items)
  {
    return items.Where<object>((Func<object, bool>) (item => item != null)).Aggregate<object, int>(17, (Func<int, object, int>) ((current, item) => current * 23 + item.GetHashCode()));
  }
}

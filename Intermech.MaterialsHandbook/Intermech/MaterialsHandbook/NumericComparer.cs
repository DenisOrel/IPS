// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.NumericComparer
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class NumericComparer
{
  internal static int Compare(object x, object y)
  {
    double result1;
    double result2;
    return !double.TryParse(Convert.ToString(x), out result1) ? (double.TryParse(Convert.ToString(y), out double _) ? -1 : 0) : (double.TryParse(Convert.ToString(y), out result2) ? result1.CompareTo(result2) : 1);
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.DateTimeComparer
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class DateTimeComparer
{
  internal static int Compare(object x, object y)
  {
    DateTime result1;
    DateTime result2;
    return !DateTime.TryParse(Convert.ToString(x), out result1) ? (DateTime.TryParse(Convert.ToString(y), out DateTime _) ? -1 : 0) : (DateTime.TryParse(Convert.ToString(y), out result2) ? result1.CompareTo(result2) : 1);
  }
}

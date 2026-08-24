// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.StringComparer
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class StringComparer
{
  internal static int Compare(object x, object y)
  {
    return string.Compare(Convert.ToString(x), Convert.ToString(y), StringComparison.Ordinal);
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ListInt32Comparer
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm;

internal class ListInt32Comparer : IComparer<int>
{
  public int Compare(int x, int y) => x.CompareTo(y);
}

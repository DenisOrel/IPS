// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.GraphComparer
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Client;

internal class GraphComparer : IComparer<string[]>
{
  public int Compare(string[] x, string[] y) => x[0].CompareTo(y[0]);
}

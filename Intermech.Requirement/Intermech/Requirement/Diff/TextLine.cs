// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.Diff.TextLine
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System;

#nullable disable
namespace Intermech.Requirement.Diff;

public class TextLine : IComparable
{
  public string Line;
  public int _hash;

  public TextLine(string str)
  {
    this.Line = str.Replace("\t", "    ");
    this._hash = str.GetHashCode();
  }

  public int CompareTo(object obj) => this._hash.CompareTo(((TextLine) obj)._hash);
}

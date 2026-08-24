// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.Diff.DiffList_TextFile
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System;
using System.Collections;

#nullable disable
namespace Intermech.Requirement.Diff;

public class DiffList_TextFile : IDiffList
{
  private const int MaxLineLength = 1024 /*0x0400*/;
  private ArrayList _lines;

  public DiffList_TextFile(string text)
  {
    string[] strArray = text.Split('\r');
    this._lines = new ArrayList();
    for (int index = 0; index < strArray.Length; ++index)
    {
      if (strArray[index].Length > 1024 /*0x0400*/)
        throw new InvalidOperationException($"File contains a line greater than {1024 /*0x0400*/.ToString()} characters.");
      if (!string.IsNullOrEmpty(strArray[index]))
        this._lines.Add((object) new TextLine(strArray[index]));
    }
  }

  public int Count() => this._lines.Count;

  public IComparable GetByIndex(int index) => (IComparable) this._lines[index];
}

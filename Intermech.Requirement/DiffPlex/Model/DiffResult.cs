// Decompiled with JetBrains decompiler
// Type: DiffPlex.Model.DiffResult
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System.Collections.Generic;

#nullable disable
namespace DiffPlex.Model;

public class DiffResult
{
  public string[] PiecesOld { get; private set; }

  public string[] PiecesNew { get; private set; }

  public IList<DiffBlock> DiffBlocks { get; private set; }

  public DiffResult(string[] peicesOld, string[] piecesNew, IList<DiffBlock> blocks)
  {
    this.PiecesOld = peicesOld;
    this.PiecesNew = piecesNew;
    this.DiffBlocks = blocks;
  }
}

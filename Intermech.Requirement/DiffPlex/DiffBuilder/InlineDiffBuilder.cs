// Decompiled with JetBrains decompiler
// Type: DiffPlex.DiffBuilder.InlineDiffBuilder
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using DiffPlex.DiffBuilder.Model;
using DiffPlex.Model;
using System;
using System.Collections.Generic;

#nullable disable
namespace DiffPlex.DiffBuilder;

public class InlineDiffBuilder : IInlineDiffBuilder
{
  private readonly IDiffer differ;

  public InlineDiffBuilder(IDiffer differ)
  {
    this.differ = differ != null ? differ : throw new ArgumentNullException(nameof (differ));
  }

  public DiffPaneModel BuildDiffModel(string oldText, string newText)
  {
    if (oldText == null)
      throw new ArgumentNullException(nameof (oldText));
    if (newText == null)
      throw new ArgumentNullException(nameof (newText));
    DiffPaneModel diffPaneModel = new DiffPaneModel();
    InlineDiffBuilder.BuildDiffPieces(this.differ.CreateLineDiffs(oldText, newText, true), diffPaneModel.Lines);
    return diffPaneModel;
  }

  private static void BuildDiffPieces(DiffResult diffResult, List<DiffPiece> pieces)
  {
    int index1 = 0;
    foreach (DiffBlock diffBlock in (IEnumerable<DiffBlock>) diffResult.DiffBlocks)
    {
      for (; index1 < diffBlock.InsertStartB; ++index1)
        pieces.Add(new DiffPiece(diffResult.PiecesNew[index1], ChangeType.Unchanged, new int?(index1 + 1)));
      for (int index2 = 0; index2 < Math.Min(diffBlock.DeleteCountA, diffBlock.InsertCountB); ++index2)
        pieces.Add(new DiffPiece(diffResult.PiecesOld[index2 + diffBlock.DeleteStartA], ChangeType.Deleted));
      int num;
      for (num = 0; num < Math.Min(diffBlock.DeleteCountA, diffBlock.InsertCountB); ++num)
      {
        pieces.Add(new DiffPiece(diffResult.PiecesNew[num + diffBlock.InsertStartB], ChangeType.Inserted, new int?(index1 + 1)));
        ++index1;
      }
      if (diffBlock.DeleteCountA > diffBlock.InsertCountB)
      {
        for (; num < diffBlock.DeleteCountA; ++num)
          pieces.Add(new DiffPiece(diffResult.PiecesOld[num + diffBlock.DeleteStartA], ChangeType.Deleted));
      }
      else
      {
        for (; num < diffBlock.InsertCountB; ++num)
        {
          pieces.Add(new DiffPiece(diffResult.PiecesNew[num + diffBlock.InsertStartB], ChangeType.Inserted, new int?(index1 + 1)));
          ++index1;
        }
      }
    }
    for (; index1 < diffResult.PiecesNew.Length; ++index1)
      pieces.Add(new DiffPiece(diffResult.PiecesNew[index1], ChangeType.Unchanged, new int?(index1 + 1)));
  }
}

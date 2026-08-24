// Decompiled with JetBrains decompiler
// Type: DiffPlex.DiffBuilder.SideBySideDiffBuilder
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using DiffPlex.DiffBuilder.Model;
using DiffPlex.Model;
using System;
using System.Collections.Generic;

#nullable disable
namespace DiffPlex.DiffBuilder;

public class SideBySideDiffBuilder : ISideBySideDiffBuilder
{
  private readonly IDiffer differ;
  public static readonly char[] WordSeparaters = new char[8]
  {
    ' ',
    '\t',
    '.',
    '(',
    ')',
    '{',
    '}',
    ','
  };

  public SideBySideDiffBuilder(IDiffer differ)
  {
    this.differ = differ != null ? differ : throw new ArgumentNullException(nameof (differ));
  }

  public SideBySideDiffModel BuildDiffModel(string oldText, string newText)
  {
    if (oldText == null)
      throw new ArgumentNullException(nameof (oldText));
    return newText != null ? this.BuildLineDiff(oldText, newText) : throw new ArgumentNullException(nameof (newText));
  }

  private SideBySideDiffModel BuildLineDiff(string oldText, string newText)
  {
    SideBySideDiffModel sideBySideDiffModel = new SideBySideDiffModel();
    SideBySideDiffBuilder.BuildDiffPieces(this.differ.CreateLineDiffs(oldText, newText, true), sideBySideDiffModel.OldText.Lines, sideBySideDiffModel.NewText.Lines, new SideBySideDiffBuilder.PieceBuilder(this.BuildWordDiffPieces));
    return sideBySideDiffModel;
  }

  private void BuildWordDiffPieces(
    string oldText,
    string newText,
    List<DiffPiece> oldPieces,
    List<DiffPiece> newPieces)
  {
    SideBySideDiffBuilder.BuildDiffPieces(this.differ.CreateWordDiffs(oldText, newText, false, SideBySideDiffBuilder.WordSeparaters), oldPieces, newPieces, (SideBySideDiffBuilder.PieceBuilder) null);
  }

  private static void BuildDiffPieces(
    DiffResult diffResult,
    List<DiffPiece> oldPieces,
    List<DiffPiece> newPieces,
    SideBySideDiffBuilder.PieceBuilder subPieceBuilder)
  {
    int index1 = 0;
    int index2 = 0;
    foreach (DiffBlock diffBlock in (IEnumerable<DiffBlock>) diffResult.DiffBlocks)
    {
      for (; index2 < diffBlock.InsertStartB && index1 < diffBlock.DeleteStartA; ++index2)
      {
        oldPieces.Add(new DiffPiece(diffResult.PiecesOld[index1], ChangeType.Unchanged, new int?(index1 + 1)));
        newPieces.Add(new DiffPiece(diffResult.PiecesNew[index2], ChangeType.Unchanged, new int?(index2 + 1)));
        ++index1;
      }
      int num;
      for (num = 0; num < Math.Min(diffBlock.DeleteCountA, diffBlock.InsertCountB); ++num)
      {
        DiffPiece diffPiece1 = new DiffPiece(diffResult.PiecesOld[num + diffBlock.DeleteStartA], ChangeType.Deleted, new int?(index1 + 1));
        DiffPiece diffPiece2 = new DiffPiece(diffResult.PiecesNew[num + diffBlock.InsertStartB], ChangeType.Inserted, new int?(index2 + 1));
        if (subPieceBuilder != null)
        {
          subPieceBuilder(diffResult.PiecesOld[index1], diffResult.PiecesNew[index2], diffPiece1.SubPieces, diffPiece2.SubPieces);
          diffPiece2.Type = diffPiece1.Type = ChangeType.Modified;
        }
        oldPieces.Add(diffPiece1);
        newPieces.Add(diffPiece2);
        ++index1;
        ++index2;
      }
      if (diffBlock.DeleteCountA > diffBlock.InsertCountB)
      {
        for (; num < diffBlock.DeleteCountA; ++num)
        {
          oldPieces.Add(new DiffPiece(diffResult.PiecesOld[num + diffBlock.DeleteStartA], ChangeType.Deleted, new int?(index1 + 1)));
          newPieces.Add(new DiffPiece());
          ++index1;
        }
      }
      else
      {
        for (; num < diffBlock.InsertCountB; ++num)
        {
          newPieces.Add(new DiffPiece(diffResult.PiecesNew[num + diffBlock.InsertStartB], ChangeType.Inserted, new int?(index2 + 1)));
          oldPieces.Add(new DiffPiece());
          ++index2;
        }
      }
    }
    for (; index2 < diffResult.PiecesNew.Length && index1 < diffResult.PiecesOld.Length; ++index2)
    {
      oldPieces.Add(new DiffPiece(diffResult.PiecesOld[index1], ChangeType.Unchanged, new int?(index1 + 1)));
      newPieces.Add(new DiffPiece(diffResult.PiecesNew[index2], ChangeType.Unchanged, new int?(index2 + 1)));
      ++index1;
    }
  }

  private delegate void PieceBuilder(
    string oldText,
    string newText,
    List<DiffPiece> oldPieces,
    List<DiffPiece> newPieces);
}

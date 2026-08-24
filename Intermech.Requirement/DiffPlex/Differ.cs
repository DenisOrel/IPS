// Decompiled with JetBrains decompiler
// Type: DiffPlex.Differ
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using DiffPlex.Model;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace DiffPlex;

public class Differ : IDiffer
{
  public DiffResult CreateLineDiffs(string oldText, string newText, bool ignoreWhitespace)
  {
    return this.CreateLineDiffs(oldText, newText, ignoreWhitespace, false);
  }

  public DiffResult CreateLineDiffs(
    string oldText,
    string newText,
    bool ignoreWhitespace,
    bool ignoreCase)
  {
    if (oldText == null)
      throw new ArgumentNullException(nameof (oldText));
    if (newText == null)
      throw new ArgumentNullException(nameof (newText));
    return this.CreateCustomDiffs(oldText, newText, ignoreWhitespace, ignoreCase, (Func<string, string[]>) (str => Differ.NormalizeNewlines(str).Split('\n')));
  }

  public DiffResult CreateCharacterDiffs(string oldText, string newText, bool ignoreWhitespace)
  {
    return this.CreateCharacterDiffs(oldText, newText, ignoreWhitespace, false);
  }

  public DiffResult CreateCharacterDiffs(
    string oldText,
    string newText,
    bool ignoreWhitespace,
    bool ignoreCase)
  {
    if (oldText == null)
      throw new ArgumentNullException(nameof (oldText));
    if (newText == null)
      throw new ArgumentNullException(nameof (newText));
    return this.CreateCustomDiffs(oldText, newText, ignoreWhitespace, ignoreCase, (Func<string, string[]>) (str =>
    {
      string[] characterDiffs = new string[str.Length];
      for (int index = 0; index < str.Length; ++index)
        characterDiffs[index] = str[index].ToString();
      return characterDiffs;
    }));
  }

  public DiffResult CreateWordDiffs(
    string oldText,
    string newText,
    bool ignoreWhitespace,
    char[] separators)
  {
    return this.CreateWordDiffs(oldText, newText, ignoreWhitespace, false, separators);
  }

  public DiffResult CreateWordDiffs(
    string oldText,
    string newText,
    bool ignoreWhitespace,
    bool ignoreCase,
    char[] separators)
  {
    if (oldText == null)
      throw new ArgumentNullException(nameof (oldText));
    if (newText == null)
      throw new ArgumentNullException(nameof (newText));
    return this.CreateCustomDiffs(oldText, newText, ignoreWhitespace, ignoreCase, (Func<string, string[]>) (str => Differ.SmartSplit(str, (IEnumerable<char>) separators)));
  }

  public DiffResult CreateCustomDiffs(
    string oldText,
    string newText,
    bool ignoreWhiteSpace,
    Func<string, string[]> chunker)
  {
    return this.CreateCustomDiffs(oldText, newText, ignoreWhiteSpace, false, chunker);
  }

  public DiffResult CreateCustomDiffs(
    string oldText,
    string newText,
    bool ignoreWhiteSpace,
    bool ignoreCase,
    Func<string, string[]> chunker)
  {
    if (oldText == null)
      throw new ArgumentNullException(nameof (oldText));
    if (newText == null)
      throw new ArgumentNullException(nameof (newText));
    if (chunker == null)
      throw new ArgumentNullException(nameof (chunker));
    Dictionary<string, int> pieceHash = new Dictionary<string, int>();
    List<DiffBlock> blocks = new List<DiffBlock>();
    ModificationData modificationData1 = new ModificationData(oldText);
    ModificationData modificationData2 = new ModificationData(newText);
    Differ.BuildPieceHashes((IDictionary<string, int>) pieceHash, modificationData1, ignoreWhiteSpace, ignoreCase, chunker);
    Differ.BuildPieceHashes((IDictionary<string, int>) pieceHash, modificationData2, ignoreWhiteSpace, ignoreCase, chunker);
    Differ.BuildModificationData(modificationData1, modificationData2);
    int length1 = modificationData1.HashedPieces.Length;
    int length2 = modificationData2.HashedPieces.Length;
    int index1 = 0;
    int index2 = 0;
    while (true)
    {
      while (index1 >= length1 || index2 >= length2 || modificationData1.Modifications[index1] || modificationData2.Modifications[index2])
      {
        int deleteStartA = index1;
        int insertStartB = index2;
        while (index1 < length1 && modificationData1.Modifications[index1])
          ++index1;
        while (index2 < length2 && modificationData2.Modifications[index2])
          ++index2;
        int deleteCountA = index1 - deleteStartA;
        int insertCountB = index2 - insertStartB;
        if (deleteCountA > 0 || insertCountB > 0)
          blocks.Add(new DiffBlock(deleteStartA, deleteCountA, insertStartB, insertCountB));
        if (index1 >= length1 || index2 >= length2)
          return new DiffResult(modificationData1.Pieces, modificationData2.Pieces, (IList<DiffBlock>) blocks);
      }
      ++index1;
      ++index2;
    }
  }

  private static string NormalizeNewlines(string str)
  {
    return str.Replace("\r\n", "\n").Replace("\r", "\n");
  }

  private static string[] SmartSplit(string str, IEnumerable<char> delims)
  {
    List<string> stringList = new List<string>();
    int startIndex = 0;
    for (int index = 0; index < str.Length; ++index)
    {
      if (delims.Contains<char>(str[index]))
      {
        stringList.Add(str.Substring(startIndex, index + 1 - startIndex));
        startIndex = index + 1;
      }
      else if (index >= str.Length - 1)
        stringList.Add(str.Substring(startIndex, index + 1 - startIndex));
    }
    return stringList.ToArray();
  }

  protected static EditLengthResult CalculateEditLength(
    int[] A,
    int startA,
    int endA,
    int[] B,
    int startB,
    int endB)
  {
    int num1 = endA - startA;
    int num2 = endB - startB + num1 + 1;
    int[] forwardDiagonal = new int[num2 + 1];
    int[] reverseDiagonal = new int[num2 + 1];
    return Differ.CalculateEditLength(A, startA, endA, B, startB, endB, forwardDiagonal, reverseDiagonal);
  }

  private static EditLengthResult CalculateEditLength(
    int[] A,
    int startA,
    int endA,
    int[] B,
    int startB,
    int endB,
    int[] forwardDiagonal,
    int[] reverseDiagonal)
  {
    if (A == null)
      throw new ArgumentNullException(nameof (A));
    if (B == null)
      throw new ArgumentNullException(nameof (B));
    if (A.Length == 0 && B.Length == 0)
      return new EditLengthResult();
    int num1 = endA - startA;
    int num2 = endB - startB;
    int num3 = (num2 + num1 + 1) / 2;
    int num4 = num1 - num2;
    bool flag = num4 % 2 == 0;
    forwardDiagonal[1 + num3] = 0;
    reverseDiagonal[1 + num3] = num1 + 1;
    for (int index1 = 0; index1 <= num3; ++index1)
    {
      for (int index2 = -index1; index2 <= index1; index2 += 2)
      {
        int index3 = index2 + num3;
        int num5;
        Edit edit;
        if (index2 == -index1 || index2 != index1 && forwardDiagonal[index3 - 1] < forwardDiagonal[index3 + 1])
        {
          num5 = forwardDiagonal[index3 + 1];
          edit = Edit.InsertDown;
        }
        else
        {
          num5 = forwardDiagonal[index3 - 1] + 1;
          edit = Edit.DeleteRight;
        }
        int num6 = num5 - index2;
        int num7 = num5;
        int num8 = num6;
        for (; num5 < num1 && num6 < num2 && A[num5 + startA] == B[num6 + startB]; ++num6)
          ++num5;
        forwardDiagonal[index3] = num5;
        if (!flag && index2 - num4 >= -index1 + 1 && index2 - num4 <= index1 - 1)
        {
          int index4 = index2 - num4 + num3;
          int num9 = reverseDiagonal[index4];
          int num10 = num9 - index2;
          if (num9 <= num5 && num10 <= num6)
            return new EditLengthResult()
            {
              EditLength = 2 * index1 - 1,
              StartX = num7 + startA,
              StartY = num8 + startB,
              EndX = num5 + startA,
              EndY = num6 + startB,
              LastEdit = edit
            };
        }
      }
      for (int index5 = -index1; index5 <= index1; index5 += 2)
      {
        int index6 = index5 + num3;
        int num11;
        Edit edit;
        if (index5 == -index1 || index5 != index1 && reverseDiagonal[index6 + 1] <= reverseDiagonal[index6 - 1])
        {
          num11 = reverseDiagonal[index6 + 1] - 1;
          edit = Edit.DeleteLeft;
        }
        else
        {
          num11 = reverseDiagonal[index6 - 1];
          edit = Edit.InsertUp;
        }
        int num12 = num11 - (index5 + num4);
        int num13 = num11;
        int num14 = num12;
        for (; num11 > 0 && num12 > 0 && A[startA + num11 - 1] == B[startB + num12 - 1]; --num12)
          --num11;
        reverseDiagonal[index6] = num11;
        if (flag && index5 + num4 >= -index1 && index5 + num4 <= index1)
        {
          int index7 = index5 + num4 + num3;
          int num15 = forwardDiagonal[index7];
          int num16 = num15 - (index5 + num4);
          if (num15 >= num11 && num16 >= num12)
            return new EditLengthResult()
            {
              EditLength = 2 * index1,
              StartX = num11 + startA,
              StartY = num12 + startB,
              EndX = num13 + startA,
              EndY = num14 + startB,
              LastEdit = edit
            };
        }
      }
    }
    throw new Exception("Should never get here");
  }

  protected static void BuildModificationData(ModificationData A, ModificationData B)
  {
    int length1 = A.HashedPieces.Length;
    int length2 = B.HashedPieces.Length;
    int num = length2 + length1 + 1;
    int[] forwardDiagonal = new int[num + 1];
    int[] reverseDiagonal = new int[num + 1];
    Differ.BuildModificationData(A, 0, length1, B, 0, length2, forwardDiagonal, reverseDiagonal);
  }

  private static void BuildModificationData(
    ModificationData A,
    int startA,
    int endA,
    ModificationData B,
    int startB,
    int endB,
    int[] forwardDiagonal,
    int[] reverseDiagonal)
  {
    for (; startA < endA && startB < endB && A.HashedPieces[startA].Equals(B.HashedPieces[startB]); ++startB)
      ++startA;
    for (; startA < endA && startB < endB && A.HashedPieces[endA - 1].Equals(B.HashedPieces[endB - 1]); --endB)
      --endA;
    int num1 = endA - startA;
    int num2 = endB - startB;
    if (num1 > 0 && num2 > 0)
    {
      EditLengthResult editLength = Differ.CalculateEditLength(A.HashedPieces, startA, endA, B.HashedPieces, startB, endB, forwardDiagonal, reverseDiagonal);
      if (editLength.EditLength <= 0)
        return;
      if (editLength.LastEdit == Edit.DeleteRight && editLength.StartX - 1 > startA)
        A.Modifications[--editLength.StartX] = true;
      else if (editLength.LastEdit == Edit.InsertDown && editLength.StartY - 1 > startB)
        B.Modifications[--editLength.StartY] = true;
      else if (editLength.LastEdit == Edit.DeleteLeft && editLength.EndX < endA)
        A.Modifications[editLength.EndX++] = true;
      else if (editLength.LastEdit == Edit.InsertUp && editLength.EndY < endB)
        B.Modifications[editLength.EndY++] = true;
      Differ.BuildModificationData(A, startA, editLength.StartX, B, startB, editLength.StartY, forwardDiagonal, reverseDiagonal);
      Differ.BuildModificationData(A, editLength.EndX, endA, B, editLength.EndY, endB, forwardDiagonal, reverseDiagonal);
    }
    else if (num1 > 0)
    {
      for (int index = startA; index < endA; ++index)
        A.Modifications[index] = true;
    }
    else
    {
      if (num2 <= 0)
        return;
      for (int index = startB; index < endB; ++index)
        B.Modifications[index] = true;
    }
  }

  private static void BuildPieceHashes(
    IDictionary<string, int> pieceHash,
    ModificationData data,
    bool ignoreWhitespace,
    bool ignoreCase,
    Func<string, string[]> chunker)
  {
    string[] strArray = !string.IsNullOrEmpty(data.RawData) ? chunker(data.RawData) : new string[0];
    data.Pieces = strArray;
    data.HashedPieces = new int[strArray.Length];
    data.Modifications = new bool[strArray.Length];
    for (int index = 0; index < strArray.Length; ++index)
    {
      string key = strArray[index];
      if (ignoreWhitespace)
        key = key.Trim();
      if (ignoreCase)
        key = key.ToUpperInvariant();
      if (pieceHash.ContainsKey(key))
      {
        data.HashedPieces[index] = pieceHash[key];
      }
      else
      {
        data.HashedPieces[index] = pieceHash.Count;
        pieceHash[key] = pieceHash.Count;
      }
    }
  }
}

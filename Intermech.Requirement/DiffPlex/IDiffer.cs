// Decompiled with JetBrains decompiler
// Type: DiffPlex.IDiffer
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using DiffPlex.Model;
using System;

#nullable disable
namespace DiffPlex;

public interface IDiffer
{
  DiffResult CreateLineDiffs(string oldText, string newText, bool ignoreWhitespace);

  DiffResult CreateLineDiffs(
    string oldText,
    string newText,
    bool ignoreWhitespace,
    bool ignoreCase);

  DiffResult CreateCharacterDiffs(string oldText, string newText, bool ignoreWhitespace);

  DiffResult CreateCharacterDiffs(
    string oldText,
    string newText,
    bool ignoreWhitespace,
    bool ignoreCase);

  DiffResult CreateWordDiffs(
    string oldText,
    string newText,
    bool ignoreWhitespace,
    char[] separators);

  DiffResult CreateWordDiffs(
    string oldText,
    string newText,
    bool ignoreWhitespace,
    bool ignoreCase,
    char[] separators);

  DiffResult CreateCustomDiffs(
    string oldText,
    string newText,
    bool ignoreWhiteSpace,
    Func<string, string[]> chunker);

  DiffResult CreateCustomDiffs(
    string oldText,
    string newText,
    bool ignoreWhiteSpace,
    bool ignoreCase,
    Func<string, string[]> chunker);
}

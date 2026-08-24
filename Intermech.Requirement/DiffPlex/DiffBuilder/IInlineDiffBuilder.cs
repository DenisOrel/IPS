// Decompiled with JetBrains decompiler
// Type: DiffPlex.DiffBuilder.IInlineDiffBuilder
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using DiffPlex.DiffBuilder.Model;

#nullable disable
namespace DiffPlex.DiffBuilder;

public interface IInlineDiffBuilder
{
  DiffPaneModel BuildDiffModel(string oldText, string newText);
}

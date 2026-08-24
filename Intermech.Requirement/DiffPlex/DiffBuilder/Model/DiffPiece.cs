// Decompiled with JetBrains decompiler
// Type: DiffPlex.DiffBuilder.Model.DiffPiece
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System.Collections.Generic;

#nullable disable
namespace DiffPlex.DiffBuilder.Model;

public class DiffPiece
{
  public ChangeType Type { get; set; }

  public int? Position { get; set; }

  public string Text { get; set; }

  public List<DiffPiece> SubPieces { get; set; }

  public DiffPiece(string text, ChangeType type, int? position)
  {
    this.Text = text;
    this.Position = position;
    this.Type = type;
    this.SubPieces = new List<DiffPiece>();
  }

  public DiffPiece(string text, ChangeType type)
    : this(text, type, new int?())
  {
  }

  public DiffPiece()
    : this((string) null, ChangeType.Imaginary)
  {
  }
}

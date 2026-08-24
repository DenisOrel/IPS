// Decompiled with JetBrains decompiler
// Type: DiffPlex.Model.EditLengthResult
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

#nullable disable
namespace DiffPlex.Model;

public class EditLengthResult
{
  public int EditLength { get; set; }

  public int StartX { get; set; }

  public int EndX { get; set; }

  public int StartY { get; set; }

  public int EndY { get; set; }

  public Edit LastEdit { get; set; }
}

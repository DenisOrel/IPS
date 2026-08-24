// Decompiled with JetBrains decompiler
// Type: DiffPlex.Model.ModificationData
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

#nullable disable
namespace DiffPlex.Model;

public class ModificationData
{
  public int[] HashedPieces { get; set; }

  public string RawData { get; private set; }

  public bool[] Modifications { get; set; }

  public string[] Pieces { get; set; }

  public ModificationData(string str) => this.RawData = str;
}

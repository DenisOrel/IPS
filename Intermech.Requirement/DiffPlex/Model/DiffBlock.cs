// Decompiled with JetBrains decompiler
// Type: DiffPlex.Model.DiffBlock
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

#nullable disable
namespace DiffPlex.Model;

public class DiffBlock
{
  public int DeleteStartA { get; private set; }

  public int DeleteCountA { get; private set; }

  public int InsertStartB { get; private set; }

  public int InsertCountB { get; private set; }

  public DiffBlock(int deleteStartA, int deleteCountA, int insertStartB, int insertCountB)
  {
    this.DeleteStartA = deleteStartA;
    this.DeleteCountA = deleteCountA;
    this.InsertStartB = insertStartB;
    this.InsertCountB = insertCountB;
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.ChangesNodesRelation
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

#nullable disable
namespace Intermech.Requirement;

public class ChangesNodesRelation
{
  public string New { get; set; }

  public string Old { get; set; }

  public int IndexParentNodes { get; set; }

  public int IndexNewParentNodes { get; set; }

  public int IndexEntry { get; set; }
}

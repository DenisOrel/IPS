// Decompiled with JetBrains decompiler
// Type: DiffPlex.Model.Edit
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

#nullable disable
namespace DiffPlex.Model;

public enum Edit
{
  None,
  DeleteRight,
  DeleteLeft,
  InsertDown,
  InsertUp,
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.WordTTDictionary
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.Requirement;

public class WordTTDictionary
{
  public string TTName { get; set; }

  public string TTDescription { get; set; }

  public string TTLevel { get; set; }

  public string TTLevelHierarhi { get; set; }

  public bool IsChecked { get; set; }

  public string TTIndexInDocument { get; set; }

  public List<WordTTDictionary> Children { get; set; }
}

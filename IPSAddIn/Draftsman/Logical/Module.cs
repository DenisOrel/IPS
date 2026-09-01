// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Draftsman.Logical.Module
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace CSharpPlugin.Draftsman.Logical;

public class Module
{
  [JsonProperty("designator")]
  public string Designator { get; set; }

  [JsonProperty("source")]
  public Source Source { get; set; }

  [JsonProperty("functionalBlocks")]
  public List<object> FunctionalBlocks { get; set; }

  [JsonProperty("components")]
  public List<Component> Components { get; set; }

  [JsonProperty("entries")]
  public List<Entry> Entries { get; set; }

  [JsonProperty("entryPinMap")]
  public EntryPinMap EntryPinMap { get; set; }

  [JsonProperty("parameters")]
  public List<Parameter> Parameters { get; set; }

  [JsonProperty("id")]
  public long Id { get; set; }
}

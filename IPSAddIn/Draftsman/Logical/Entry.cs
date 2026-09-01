// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Draftsman.Logical.Entry
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace CSharpPlugin.Draftsman.Logical;

public class Entry
{
  [JsonProperty("logicalConnection", NullValueHandling = NullValueHandling.Ignore)]
  public long? LogicalConnection { get; set; }

  [JsonProperty("calculatedDesignator")]
  public string CalculatedDesignator { get; set; }

  [JsonProperty("parameters")]
  public List<object> Parameters { get; set; }

  [JsonProperty("id")]
  public long Id { get; set; }

  [JsonProperty("definedDesignator", NullValueHandling = NullValueHandling.Ignore)]
  public string DefinedDesignator { get; set; }
}

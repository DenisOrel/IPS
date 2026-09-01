// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Draftsman.Logical.Connection
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace CSharpPlugin.Draftsman.Logical;

public class Connection
{
  [JsonProperty("$type")]
  public string Type { get; set; }

  [JsonProperty("pinPair", NullValueHandling = NullValueHandling.Ignore)]
  public PinPair PinPair { get; set; }

  [JsonProperty("entriesConnection", NullValueHandling = NullValueHandling.Ignore)]
  public EntriesConnection EntriesConnection { get; set; }

  [JsonProperty("designator")]
  public string Designator { get; set; }

  [JsonProperty("parameters")]
  public List<object> Parameters { get; set; }

  [JsonProperty("id")]
  public long Id { get; set; }
}

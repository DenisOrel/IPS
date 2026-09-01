// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Draftsman.Logical.Pin
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace CSharpPlugin.Draftsman.Logical;

public class Pin
{
  [JsonProperty("pinId")]
  public string PinId { get; set; }

  [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
  public string Name { get; set; }

  [JsonProperty("number")]
  public string Number { get; set; }

  [JsonProperty("parameters")]
  public List<Parameter> Parameters { get; set; }

  [JsonProperty("id")]
  public long Id { get; set; }
}

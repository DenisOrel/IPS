// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Draftsman.Logical.PinPair
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace CSharpPlugin.Draftsman.Logical;

public class PinPair
{
  [JsonProperty("pin1")]
  public long Pin1 { get; set; }

  [JsonProperty("pin2")]
  public long Pin2 { get; set; }

  [JsonProperty("parameters")]
  public List<object> Parameters { get; set; }

  [JsonProperty("id")]
  public long Id { get; set; }
}

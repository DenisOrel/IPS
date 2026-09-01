// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Draftsman.Parameters.ParameterRecord
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Newtonsoft.Json;

#nullable disable
namespace CSharpPlugin.Draftsman.Parameters;

internal class ParameterRecord
{
  [JsonProperty("name")]
  public string Name { get; set; }

  [JsonProperty("value")]
  public string Value { get; set; }
}

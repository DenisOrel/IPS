// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Draftsman.Logical.EntryPinMap
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace CSharpPlugin.Draftsman.Logical;

public class EntryPinMap
{
  [JsonProperty("map")]
  public List<CSharpPlugin.Draftsman.Logical.Map> Map { get; set; }
}

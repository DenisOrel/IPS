// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Draftsman.Logical.EntriesConnection
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace CSharpPlugin.Draftsman.Logical;

public class EntriesConnection
{
  [JsonProperty("entry1")]
  public long Entry1 { get; set; }

  [JsonProperty("entry2")]
  public long Entry2 { get; set; }

  [JsonProperty("physicalConnections")]
  public List<Connection> PhysicalConnections { get; set; }

  [JsonProperty("id")]
  public long Id { get; set; }
}

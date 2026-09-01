// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Draftsman.Logical.Root
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace CSharpPlugin.Draftsman.Logical;

public class Root
{
  [JsonProperty("uniqueId")]
  public Guid UniqueId { get; set; }

  [JsonProperty("modules")]
  public List<Module> Modules { get; set; }

  [JsonProperty("powerPorts")]
  public List<object> PowerPorts { get; set; }

  [JsonProperty("connections")]
  public List<Connection> Connections { get; set; }

  [JsonProperty("conflicts")]
  public List<object> Conflicts { get; set; }

  [JsonProperty("parameters")]
  public List<object> Parameters { get; set; }
}

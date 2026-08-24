// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.VirtualSearchSchemeID
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class VirtualSearchSchemeID : SearchSchemeID
{
  public List<int> RelTypes { get; private set; }

  public List<int> Types { get; private set; }

  public ContainsMode ContainsMode { get; private set; }

  public VirtualSearchSchemeID(
    string name,
    ContainsMode containsMode,
    int relTypeID,
    List<int> types)
    : this(name, containsMode, new List<int>((IEnumerable<int>) new int[1]
    {
      relTypeID
    }), types)
  {
  }

  public VirtualSearchSchemeID(
    string name,
    ContainsMode containsMode,
    List<int> relTypes,
    List<int> types)
    : base(name, -1L)
  {
    if (relTypes.Count > 0)
      this.SchemeID = (long) (-relTypes[0] - 1);
    this.ContainsMode = containsMode;
    this.RelTypes = relTypes;
    this.Types = types;
  }
}

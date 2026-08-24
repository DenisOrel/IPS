// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.IDAttributesSyncronizer.IDAttributeInfo
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;

#nullable disable
namespace Intermech.Pdm.IDAttributesSyncronizer;

internal sealed class IDAttributeInfo
{
  public bool Changed { get; private set; }

  public AttributeValues OrigValue { get; set; }

  public string NewValue { get; set; }

  public IDAttributeInfo()
  {
    this.Changed = false;
    this.NewValue = string.Empty;
  }

  public IDAttributeInfo(bool changed, string newValue)
  {
    this.Changed = changed;
    this.NewValue = newValue;
  }
}

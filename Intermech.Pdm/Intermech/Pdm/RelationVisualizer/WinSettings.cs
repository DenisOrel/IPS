// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.WinSettings
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm.RelationVisualizer;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class WinSettings
{
  public RelVisPred.NoCaptionFormula NoCaptionFormula;

  public WinSettings()
  {
    this.ShowLifecycleLevel = false;
    this.ShowStatuses = false;
    this.MaxCaptionLength = 10;
  }

  public WinSettings(WinSettings other)
  {
    this.ShowLifecycleLevel = other.ShowLifecycleLevel;
    this.ShowStatuses = other.ShowStatuses;
    this.MaxCaptionLength = other.MaxCaptionLength;
  }

  public bool ShowLifecycleLevel { get; set; }

  public bool ShowStatuses { get; set; }

  public int MaxCaptionLength { get; set; }
}

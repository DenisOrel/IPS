// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.DrawSettings
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm.RelationVisualizer;
using Intermech.Map;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class DrawSettings : IDrawSettings
{
  public long ObjId = -1;

  public DrawSettings()
  {
    this.ShowLifecycleLevel = false;
    this.ShowStatuses = false;
    this.MaxCaptionLength = 10;
    this.NoCaptionFormula = RelVisPred.NoCaptionFormula.Nom;
    this.PickedObj = (MapObject) null;
  }

  public DrawSettings(bool showLL, bool showSt, int maxCaptLength)
  {
    this.ShowLifecycleLevel = showLL;
    this.ShowStatuses = showSt;
    this.MaxCaptionLength = maxCaptLength;
    this.NoCaptionFormula = RelVisPred.NoCaptionFormula.Nom;
    this.PickedObj = (MapObject) null;
  }

  public DrawSettings(
    bool showLL,
    bool showSt,
    int maxCaptLength,
    RelVisPred.NoCaptionFormula ncForm)
  {
    this.ShowLifecycleLevel = showLL;
    this.ShowStatuses = showSt;
    this.MaxCaptionLength = maxCaptLength;
    this.NoCaptionFormula = ncForm;
    this.PickedObj = (MapObject) null;
  }

  public bool ShowLifecycleLevel { get; set; }

  public bool ShowStatuses { get; set; }

  public int MaxCaptionLength { get; set; }

  public bool DrawLinkArrow { get; set; }

  public RelVisPred.NoCaptionFormula NoCaptionFormula { get; set; }

  public MapObject PickedObj { get; set; }
}

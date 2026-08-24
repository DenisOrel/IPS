// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.IDrawSettings
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm.RelationVisualizer;
using Intermech.Map;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public interface IDrawSettings
{
  bool ShowLifecycleLevel { get; set; }

  bool ShowStatuses { get; set; }

  int MaxCaptionLength { get; set; }

  bool DrawLinkArrow { get; set; }

  RelVisPred.NoCaptionFormula NoCaptionFormula { get; set; }

  MapObject PickedObj { get; set; }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.LoadSettings
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public struct LoadSettings
{
  public RelationFlags RelsLoaded { get; internal set; }

  public bool ParentsLoaded { get; internal set; }

  public bool ChildsLoaded { get; internal set; }

  public bool ShowStructLinks
  {
    get => (this.RelsLoaded & RelationFlags.StructLinks) != 0;
    set => this.RelsLoaded |= RelationFlags.StructLinks;
  }

  public bool ShowAssocLinks
  {
    get => (this.RelsLoaded & RelationFlags.AssocLinks) != 0;
    set => this.RelsLoaded |= RelationFlags.AssocLinks;
  }

  public void UpdateSettings(
    bool parentsLoaded,
    bool childsLoaded,
    bool showStructLinks,
    bool showAssocLinks)
  {
    if (childsLoaded)
      this.ChildsLoaded = true;
    if (parentsLoaded)
      this.ParentsLoaded = true;
    if (showStructLinks)
      this.RelsLoaded |= RelationFlags.StructLinks;
    if (!showAssocLinks)
      return;
    this.RelsLoaded |= RelationFlags.AssocLinks;
  }
}

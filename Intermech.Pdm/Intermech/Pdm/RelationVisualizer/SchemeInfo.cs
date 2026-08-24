// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.SchemeInfo
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public struct SchemeInfo
{
  public static readonly SchemeInfo Empty;

  public LayoutKind SchemeLayoutKind { get; set; }

  public DateTime DateOfLastCheck { get; set; }

  public long ObjectVersionId { get; set; }

  public int ObjectTypeId { get; set; }

  public long ID { get; set; }

  public string Caption { get; set; }

  public DrawSettings UserSettings { get; set; }

  public bool IsEmpty => this.Caption == null || this.Caption == "";

  public ReloadDecision NeedUpdateTree(
    LoadSettings ls,
    bool needParents,
    bool needChilds,
    bool structLinks,
    bool assocLinks)
  {
    if (structLinks && !ls.ShowStructLinks || assocLinks && !ls.ShowAssocLinks)
      return ReloadDecision.FullReload;
    return needParents && !ls.ParentsLoaded || needChilds && !ls.ChildsLoaded ? ReloadDecision.PartReload : ReloadDecision.NoReload;
  }

  public SchemeInfo(
    long objVerId,
    long ID,
    string capt,
    int objtypeId,
    LayoutKind lKind,
    DrawSettings settings)
    : this()
  {
    this.ObjectVersionId = objVerId;
    this.Caption = capt;
    this.ObjectTypeId = objtypeId;
    this.SchemeLayoutKind = lKind;
    this.ID = ID;
    this.DateOfLastCheck = DateTime.Now;
    this.UserSettings = settings;
  }

  public SchemeInfo(SchemeInfo parentScheme, LayoutKind lKind)
    : this()
  {
    this.ObjectVersionId = parentScheme.ObjectVersionId;
    this.Caption = parentScheme.Caption;
    this.ObjectTypeId = parentScheme.ObjectTypeId;
    this.SchemeLayoutKind = lKind;
    this.ID = parentScheme.ID;
    this.DateOfLastCheck = DateTime.Now;
    this.UserSettings = parentScheme.UserSettings;
  }

  public override bool Equals(object obj)
  {
    return obj is SchemeInfo schemeInfo && this.ObjectVersionId == schemeInfo.ObjectVersionId && this.Caption == schemeInfo.Caption && this.ObjectTypeId == schemeInfo.ObjectTypeId && this.SchemeLayoutKind == schemeInfo.SchemeLayoutKind;
  }

  public override int GetHashCode() => this.ToString().GetHashCode();

  public override string ToString()
  {
    string description = VisLayout.GetDescription(this.SchemeLayoutKind);
    return !this.Caption.Equals(string.Empty) ? this.Caption + description : $"{this.ObjectVersionId}" + description;
  }
}

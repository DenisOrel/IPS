// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.ArcArtPump.ArcArtsObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common.ArcArtPump;

[Serializable]
internal struct ArcArtsObject(int artId, int artVer, string name, string designation)
{
  private readonly int _artId = artId;
  private readonly int _artVer = artVer;
  private string _name = name;
  private string _designation = designation;
  [OptionalField]
  private Guid _portalVerGuid = Guid.Empty;

  public int ArtId => this._artId;

  public int ArtVer => this._artVer;

  public string Name => this._name;

  public string Designation => this._designation;

  public string Caption
  {
    get
    {
      string caption = string.Empty;
      if (this._name != string.Empty)
        caption = $"({this._name})";
      if (this._designation != string.Empty)
        caption = $"{this._designation} {caption}";
      return caption;
    }
  }

  public Guid PortalVerGuid
  {
    get => this._portalVerGuid;
    set => this._portalVerGuid = value;
  }
}

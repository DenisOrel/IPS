// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisStatus
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class VisStatus : IEquatable<VisStatus>
{
  private Image _icon;
  private string _capt;

  public VisStatusKey Key { get; private set; }

  public Image Icon
  {
    get
    {
      this._initImage();
      return this._icon;
    }
  }

  public string Caption
  {
    get
    {
      this._initCaption();
      return this._capt;
    }
  }

  public VisStatus(Guid g, int i) => this.Key = new VisStatusKey(g, i);

  private bool _initImage()
  {
    if (this._icon != null)
      return true;
    this._icon = VisStatusKeeper.GetImage(this.Key);
    return this._icon != null;
  }

  private bool _initCaption()
  {
    if (this._capt != null)
      return true;
    this._capt = VisStatusKeeper.GetCapt(this.Key);
    return this._capt != null;
  }

  public bool Equals(VisStatus other) => this.Key._Equals(other.Key);

  public override bool Equals(object obj)
  {
    switch (obj)
    {
      case VisStatus _:
        return this.Key._Equals(((VisStatus) obj).Key);
      case VisStatusKey other:
        return this.Key._Equals(other);
      default:
        return false;
    }
  }

  public override int GetHashCode() => this.Key.GetHashCode();

  public override string ToString() => this.Caption ?? "<?>";
}

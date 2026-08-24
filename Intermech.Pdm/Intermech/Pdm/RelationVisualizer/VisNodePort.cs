// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisNodePort
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

internal class VisNodePort : MapGeneralNodePort
{
  private bool? _open;

  public VisObject Obj { get; set; }

  public bool? Open
  {
    get => this._open;
    set
    {
      this._open = value;
      this.UpdateStyle(this._open);
    }
  }

  public override bool OnDoubleClick(MapInputEventArgs evt, MapView view)
  {
    if (!this.Open.HasValue || !(view is VisControl))
      return false;
    ((VisControl) view).OnPortDoubleClicked(this, evt);
    return true;
  }

  internal void UpdateStyle(bool? openState)
  {
    MapPortStyle mapPortStyle = MapPortStyle.None;
    bool? nullable = openState;
    if (nullable.HasValue)
    {
      bool valueOrDefault = nullable.GetValueOrDefault();
      if (valueOrDefault)
      {
        if (valueOrDefault)
          mapPortStyle = MapPortStyle.Ellipse;
      }
      else
        mapPortStyle = MapPortStyle.Ellipse;
    }
    if (this.Style == mapPortStyle)
      return;
    this.Style = mapPortStyle;
  }

  public override void Paint(Graphics g, MapView view)
  {
    base.Paint(g, view);
    if (this.PaintGreek(g, view))
      return;
    RectangleF bounds = this.Bounds;
    if (!this._open.HasValue)
      return;
    PointF pointF = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
    float num = bounds.Height / 3f;
    MapShape.DrawLine(g, view, this.Pen, pointF.X - num, pointF.Y, pointF.X + num, pointF.Y);
    bool? open = this._open;
    bool flag = false;
    if (!(open.GetValueOrDefault() == flag & open.HasValue))
      return;
    MapShape.DrawLine(g, view, this.Pen, pointF.X, pointF.Y - num, pointF.X, pointF.Y + num);
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisLink
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Map;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

internal class VisLink : MapLabeledLink
{
  public VisRelation Rel { get; protected set; }

  public long RelId
  {
    get => this.Rel.VisRelData.RelationId;
    set => this.Rel.VisRelData.RelationId = value;
  }

  public VisLink(VisRelation rel, MapLayer layer)
  {
    this.Rel = rel;
    rel.Link = this;
    if (!(layer.Document.UserObject is IDrawSettings userObject))
      return;
    this.ToArrow = userObject.DrawLinkArrow;
    this.ToArrowShaftLength = 1f;
    this.FromArrowLength = 1f;
    this.Deletable = false;
    this.Movable = false;
    this.Relinkable = false;
    this.Copyable = false;
    this.Resizable = false;
    this.Pen = new Pen(this.Rel.LineColor);
    this.PenWidth = 0.5f;
    this.Pen.DashStyle = this.Rel.DStyle;
    this.HighlightPen = new Pen(this.Rel.HighlightColor);
    this.HighlightPenWidth = 2f;
    this.MidLabelCentered = true;
    this.SetPorts();
    layer.Add((MapObject) this);
    this.DrawCount();
  }

  internal void DrawCount()
  {
    if (this.Rel.LineText != "")
    {
      MapText mapText = new MapText();
      mapText.Alignment = 1;
      mapText.Selectable = false;
      mapText.Text = this.Rel.LineText;
      mapText.TextColor = Color.Red;
      this.MidLabel = (MapObject) mapText;
    }
    else
      this.MidLabel = (MapObject) null;
  }

  public void SetCount(MeasuredValue mv)
  {
    this.Rel.VisRelData.Quantity = mv;
    this.DrawCount();
  }

  internal void SetPorts()
  {
    if (this.Rel.Parent.Node.RightPortsCount == 0)
      this.Rel.Parent.Node.AddRightPort((MapGeneralNodePort) this.Rel.Parent.Node._VisMakePort(false));
    if (this.Rel.Child.Node.LeftPortsCount == 0)
      this.Rel.Child.Node.AddLeftPort((MapGeneralNodePort) this.Rel.Child.Node._VisMakePort(true));
    this.FromPort = (IMapPort) this.Rel.Parent.Node.RightPorts[0];
    this.ToPort = (IMapPort) this.Rel.Child.Node.LeftPorts[0];
    this.PenWidth = 0.2f;
  }

  public override MapObject Pick(PointF p, bool selectableOnly)
  {
    if (this.CanView())
    {
      foreach (MapObject backward in this.Backwards)
      {
        if (backward is MapStroke mapStroke)
        {
          int segmentNearPoint = mapStroke.GetSegmentNearPoint(p);
          if (segmentNearPoint > 0 && segmentNearPoint < mapStroke.PointsCount - 2)
            return (MapObject) this;
        }
        else
        {
          MapObject mapObject = backward.Pick(p, selectableOnly);
          if (mapObject != null)
            return mapObject;
        }
      }
    }
    return (MapObject) null;
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisPrevNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

internal class VisPrevNode(VisObject obj, MapLayer layer) : VisNode(obj, layer)
{
  public override void InitializeUncommon(
    System.Drawing.Image typeImg,
    string top,
    string bottom,
    bool needStatusSpace)
  {
    this.Initializing = true;
    try
    {
      MapNodeIcon mapNodeIcon = new MapNodeIcon();
      mapNodeIcon.MinimumIconSize = new SizeF((float) VisObject.PreviewWid, (float) VisObject.PreviewHei);
      mapNodeIcon.MaximumIconSize = new SizeF((float) VisObject.PreviewWid, (float) VisObject.PreviewHei);
      mapNodeIcon.AutoResizes = false;
      mapNodeIcon.Image = this.Obj.Preview;
      if (VisNode.Vertical)
        this.Orientation = Orientation.Vertical;
      this.Icon = (MapObject) mapNodeIcon;
      if (this.Obj.Level == 0)
        this.TopLabel = this.CreateLabel(true, top);
      this.BottomLabel = this.CreateLabel(false, bottom);
      if (this.Obj.ParentRels.Count > 0)
        this.AddLeftPort((MapGeneralNodePort) this._VisMakePort(true));
      if (this.Obj.ChildRels.Count > 0)
        this.AddRightPort((MapGeneralNodePort) this._VisMakePort(false));
      this.PropertiesDelegated = true;
    }
    finally
    {
      this.Initializing = false;
    }
  }

  public override void UpdateSettings(
    out string topLabel,
    out string bottomLabel,
    out bool needSpace4Statuses)
  {
    if (!(this.Document.UserObject is IDrawSettings userObject))
    {
      topLabel = "";
      bottomLabel = "";
      needSpace4Statuses = false;
    }
    else
    {
      topLabel = this.Obj.UpperHint;
      bottomLabel = this.Obj.LowerHint;
      needSpace4Statuses = userObject.ShowLifecycleLevel || userObject.ShowStatuses;
      this.UpdateDrawSettings(userObject);
    }
  }

  public override void UpdateDrawSettings(IDrawSettings ids)
  {
    this.ShowLifecycleLevel = ids.ShowLifecycleLevel;
    this.ShowStatuses = ids.ShowStatuses;
    this.StatusList = this.Obj.StatusList;
    this.LifecycleLevelImg = this.Obj.LCLevelImage;
  }

  public override void UpdateAllSettings(IDrawSettings ids) => this.UpdateDrawSettings(ids);

  protected override void DrawLifecycleLevel(Graphics g)
  {
    if (this.LifecycleLevelImg == null || !this.ShowLifecycleLevel)
      return;
    Graphics graphics = g;
    System.Drawing.Image lifecycleLevelImg = this.LifecycleLevelImg;
    PointF location = this.Location;
    double x = (double) location.X - 49.0;
    location = this.Location;
    double y = (double) location.Y + 5.0;
    RectangleF rect = new RectangleF(new PointF((float) x, (float) y), new SizeF(16f, 8f));
    graphics.DrawImage(lifecycleLevelImg, rect);
  }

  protected override void DrawStatuses(Graphics g)
  {
    if (this.StatusList == null || !this.ShowStatuses)
      return;
    for (int index = 0; index < this.StatusList.Count; ++index)
    {
      System.Drawing.Image icon = this.StatusList[index].Icon;
      if (icon != null)
      {
        Graphics graphics = g;
        System.Drawing.Image image = icon;
        PointF location = this.Location;
        double x = (double) location.X - 49.0;
        location = this.Location;
        double y = (double) location.Y - 15.0 - (double) (8 * index);
        RectangleF rect = new RectangleF(new PointF((float) x, (float) y), new SizeF(8f, 8f));
        graphics.DrawImage(image, rect);
      }
    }
  }

  private int GetStatusMaxElementsCount()
  {
    return !this.ShowStatuses || this.StatusList == null ? 0 : this.StatusList.Count;
  }

  private string GetLCLevelName()
  {
    return this.Obj.LCLevelId == 0 ? (string) null : LCLevelInfoKeeper.GetLCName(this.Obj.LCLevelId);
  }

  private string GetStatusName(int i)
  {
    return this.StatusList == null || this.StatusList.Count < i + 1 ? (string) null : this.StatusList[i].Caption;
  }

  protected override string GetToolTipText(PointF p)
  {
    if (this.ShowLifecycleLevel && new RectangleF(new PointF(this.Location.X - 49f, this.Location.Y + 5f), new SizeF(8f, 8f)).Contains(p))
      return this.GetLCLevelName();
    if (this.ShowStatuses)
    {
      for (int i = 0; i < this.GetStatusMaxElementsCount() + 1; ++i)
      {
        if (new RectangleF(new PointF(this.Location.X - 49f, this.Location.Y - 15f - (float) (8 * i)), new SizeF(8f, 8f)).Contains(p))
          return this.GetStatusName(i);
      }
    }
    return (string) null;
  }
}

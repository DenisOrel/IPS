// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

internal class VisNode : MapGeneralNode
{
  public static bool Vertical;
  public static MapBasicNode tooltipProxy = new MapBasicNode();
  private int XOffset = 12;
  protected bool ShowLifecycleLevel;
  protected bool ShowStatuses;
  protected List<VisStatus> StatusList;
  protected System.Drawing.Image LifecycleLevelImg;

  public VisObject Obj { get; protected set; }

  public long ObjId => this.Obj.VisObjectData.ObjVerId;

  public int ObjTypeId => this.Obj.VisObjectData.ObjTypeId;

  internal bool MarkForDelete { get; set; }

  static VisNode()
  {
    VisNode.tooltipProxy.Selectable = false;
    VisNode.tooltipProxy.Movable = false;
    VisNode.tooltipProxy.Copyable = false;
    VisNode.tooltipProxy.Deletable = false;
    VisNode.tooltipProxy.DragsNode = false;
    VisNode.tooltipProxy.Editable = false;
  }

  public VisNode(VisObject obj, MapLayer layer)
  {
    this.Obj = obj;
    obj.Node = this;
    this.Text = "";
    this.MarkForDelete = false;
    this.Deletable = false;
    layer.Add((MapObject) this);
    this.Init();
  }

  public void Init()
  {
    string topLabel;
    string bottomLabel;
    bool needSpace4Statuses;
    this.UpdateSettings(out topLabel, out bottomLabel, out needSpace4Statuses);
    this.InitializeUncommon(this.Obj.TypeImage, topLabel, bottomLabel, needSpace4Statuses);
    this.Location = (PointF) this.Obj.Org;
    this.LayoutChildren((MapObject) null);
    if (this.TopLabel != null)
    {
      this.TopLabel.Selectable = false;
      this.TopLabel.Resizable = false;
      this.TopLabel.Editable = false;
      this.TopLabel.ToolTipText = this.Obj.UpperHint;
      this.TopLabel.Bold = this.Obj.Level == 0;
    }
    if (this.BottomLabel != null)
    {
      this.BottomLabel.Selectable = false;
      this.BottomLabel.Resizable = false;
      this.BottomLabel.Editable = false;
      this.BottomLabel.ToolTipText = this.Obj.LowerHint;
      this.BottomLabel.Bold = this.Obj.Level == 0;
    }
    if (this.LeftPorts.Count > 0)
    {
      VisNodePort leftPort = (VisNodePort) this.LeftPorts[0];
      leftPort.Label = (MapGeneralNodePortLabel) null;
      leftPort.Movable = false;
      leftPort.Selectable = false;
      leftPort.Name = "";
      if (this.Obj.Level <= 0)
        leftPort.Open = new bool?(this.Obj.ParentsOpen);
    }
    if (this.RightPorts.Count > 0)
    {
      VisNodePort rightPort = (VisNodePort) this.RightPorts[0];
      rightPort.Label = (MapGeneralNodePortLabel) null;
      rightPort.Movable = false;
      rightPort.Selectable = false;
      rightPort.Name = "";
      if (this.Obj.Level >= 0)
        rightPort.Open = new bool?(this.Obj.ChildsOpen);
    }
    this.ToolTipText = this.Obj.MainHint;
  }

  public virtual void InitializeUncommon(
    System.Drawing.Image typeImg,
    string top,
    string bottom,
    bool needStatusSpace)
  {
    this.Initializing = true;
    try
    {
      MapNodeIcon mapNodeIcon = new MapNodeIcon();
      mapNodeIcon.MinimumIconSize = new SizeF(16f, 16f);
      mapNodeIcon.MaximumIconSize = new SizeF(40f, 32f);
      mapNodeIcon.Image = typeImg;
      if (VisNode.Vertical)
        this.Orientation = Orientation.Vertical;
      this.Icon = (MapObject) mapNodeIcon;
      if (typeImg.Width == 40)
        this.XOffset = 20;
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

  public virtual VisNodePort _VisMakePort(bool input)
  {
    VisNodePort visPort = this.CreateVisPort(input);
    if (visPort != null)
    {
      visPort.Name = "1";
      PointF position;
      if (this.Icon != null)
      {
        position = this.Icon.Position;
        if (VisNode.Vertical)
        {
          visPort.ToSpot = 1;
          visPort.FromSpot = 1;
          if (input)
          {
            visPort.Style = MapPortStyle.None;
            position.Y -= visPort.Height;
          }
          else
          {
            visPort.Style = MapPortStyle.None;
            position.Y = this.Icon.Bottom;
          }
        }
        else
        {
          visPort.ToSpot = 1;
          visPort.FromSpot = 1;
          if (input)
          {
            visPort.Style = MapPortStyle.None;
            position.X -= visPort.Width;
          }
          else
          {
            visPort.Style = MapPortStyle.None;
            position.X = this.Icon.Right;
          }
        }
      }
      else
        position = this.Position;
      visPort.Position = position;
    }
    return visPort;
  }

  private VisNodePort CreateVisPort(bool input)
  {
    VisNodePort visPort = new VisNodePort();
    visPort.LeftSide = input;
    visPort.IsValidFrom = !input;
    visPort.IsValidTo = input;
    visPort.Obj = this.Obj;
    visPort.Width += 2f;
    visPort.Height += 2f;
    return visPort;
  }

  public override void LayoutChildren(MapObject childchanged)
  {
    base.LayoutChildren(childchanged);
    if (this.Orientation == Orientation.Horizontal || this.Icon == null || this.Initializing)
      return;
    this.Initializing = true;
    for (int i = 0; i < this.LeftPortsCount; ++i)
    {
      MapGeneralNodePort leftPort = this.GetLeftPort(i);
      if (leftPort.Visible)
      {
        leftPort.SetSpotLocation(128 /*0x80*/, this.Icon, 32 /*0x20*/);
        leftPort.Left += 3f;
        leftPort.LayoutLabel();
      }
    }
    for (int i = 0; i < this.RightPortsCount; ++i)
    {
      MapGeneralNodePort rightPort = this.GetRightPort(i);
      if (rightPort.Visible)
      {
        rightPort.SetSpotLocation(32 /*0x20*/, this.Icon, 128 /*0x80*/);
        rightPort.Left += 3f;
        rightPort.LayoutLabel();
      }
    }
    this.TopLabel?.SetSpotLocation(128 /*0x80*/, this.Icon, 32 /*0x20*/);
    this.BottomLabel?.SetSpotLocation(32 /*0x20*/, this.Icon, 128 /*0x80*/);
    this.Initializing = false;
  }

  public virtual void UpdateSettings(
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
      topLabel = this.Obj.UpperStr;
      bottomLabel = this.Obj.LowerStr;
      needSpace4Statuses = userObject.ShowLifecycleLevel || userObject.ShowStatuses;
      this.UpdateDrawSettings(userObject);
    }
  }

  public virtual void UpdateDrawSettings(IDrawSettings ids)
  {
    this.ShowLifecycleLevel = ids.ShowLifecycleLevel;
    this.ShowStatuses = ids.ShowStatuses;
    this.StatusList = this.Obj.StatusList;
    this.LifecycleLevelImg = this.Obj.LCLevelImage;
  }

  public virtual void UpdateAllSettings(IDrawSettings ids) => this.UpdateDrawSettings(ids);

  public override void Paint(Graphics g, MapView view)
  {
    this.DrawLifecycleLevel(g);
    this.DrawStatuses(g);
    base.Paint(g, view);
  }

  protected virtual void DrawLifecycleLevel(Graphics g)
  {
    if (this.LifecycleLevelImg == null || !this.ShowLifecycleLevel)
      return;
    Graphics graphics = g;
    System.Drawing.Image lifecycleLevelImg = this.LifecycleLevelImg;
    PointF location = this.Location;
    double x = (double) location.X - (double) this.XOffset;
    location = this.Location;
    double y = (double) location.Y;
    RectangleF rect = new RectangleF(new PointF((float) x, (float) y), new SizeF(16f, 8f));
    graphics.DrawImage(lifecycleLevelImg, rect);
  }

  protected virtual void DrawStatuses(Graphics g)
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
        double x = (double) location.X - (double) this.XOffset;
        location = this.Location;
        double y = (double) location.Y - (double) (8 * (index + 1));
        RectangleF rect = new RectangleF(new PointF((float) x, (float) y), new SizeF(8f, 8f));
        graphics.DrawImage(image, rect);
      }
    }
  }

  public override MapObject Pick(PointF p, bool selectableOnly)
  {
    if (!selectableOnly)
    {
      string toolTipText = this.GetToolTipText(p);
      if (toolTipText != null && !toolTipText.Equals(string.Empty))
      {
        VisNode.tooltipProxy.ToolTipText = toolTipText;
        return (MapObject) VisNode.tooltipProxy;
      }
    }
    MapObject pickedObj = base.Pick(p, selectableOnly);
    this.ProcessHighlight(pickedObj);
    return pickedObj;
  }

  private void ProcessPort(MapGeneralNodePort port, bool hl)
  {
    if (port == null)
      return;
    foreach (MapLabeledLink link in port.Links)
    {
      if (link.Highlight != hl)
        link.Highlight = hl;
    }
  }

  private MapObject ProcessHighlight(MapObject pickedObj)
  {
    if (!(this.Document.UserObject is DrawSettings userObject))
      return (MapObject) null;
    switch (pickedObj)
    {
      case MapGeneralNodePort _:
        if (userObject.ObjId != this.Obj.ObjVerId)
          this.Deselect(userObject);
        MapGeneralNodePort port = (MapGeneralNodePort) pickedObj;
        if (userObject.PickedObj != port)
        {
          userObject.PickedObj = (MapObject) port;
          this.ProcessPort(port, true);
        }
        userObject.ObjId = this.Obj.ObjVerId;
        break;
      case MapNodeIcon _:
      case MapText _:
        if (userObject.ObjId != this.Obj.ObjVerId)
          this.Deselect(userObject);
        if (pickedObj.ParentNode is VisNode parentNode)
        {
          this.ProcessPort(parentNode.GetLeftPort(0), true);
          this.ProcessPort(parentNode.GetRightPort(0), true);
          userObject.PickedObj = (MapObject) parentNode;
        }
        userObject.ObjId = this.Obj.ObjVerId;
        break;
      default:
        if (userObject.ObjId != this.Obj.ObjVerId)
          return (MapObject) null;
        this.Deselect(userObject);
        break;
    }
    return pickedObj;
  }

  private void Deselect(DrawSettings ds)
  {
    if (ds.PickedObj is MapGeneralNodePort pickedObj1)
      this.ProcessPort(pickedObj1, false);
    if (ds.PickedObj is VisNode pickedObj2)
    {
      this.ProcessPort(pickedObj2.GetLeftPort(0), false);
      this.ProcessPort(pickedObj2.GetRightPort(0), false);
    }
    ds.PickedObj = (MapObject) null;
    ds.ObjId = -1L;
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

  protected virtual string GetToolTipText(PointF p)
  {
    for (int index = 0; index < this.GetStatusMaxElementsCount() + 1; ++index)
    {
      if (index != 0 || this.ShowLifecycleLevel)
      {
        RectangleF rectangleF;
        ref RectangleF local = ref rectangleF;
        PointF location1 = this.Location;
        double x = (double) location1.X - (double) this.XOffset;
        location1 = this.Location;
        double y = (double) location1.Y - (double) (8 * index);
        PointF location2 = new PointF((float) x, (float) y);
        SizeF size = new SizeF(8f, 8f);
        local = new RectangleF(location2, size);
        if (rectangleF.Contains(p))
          return index == 0 ? this.GetLCLevelName() : this.GetStatusName(index - 1);
      }
    }
    return (string) null;
  }

  internal void ReflectCheckInOrOut()
  {
    this.Obj.VisObjectData.ObjVerId = -this.Obj.VisObjectData.ObjVerId;
  }
}

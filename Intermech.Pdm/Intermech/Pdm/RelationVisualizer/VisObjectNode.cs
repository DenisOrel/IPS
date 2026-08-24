// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisObjectNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Map;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

[Serializable]
public class VisObjectNode : MapIconicNode
{
  private WinSettings settings;
  private List<Status> statusImg;
  private System.Drawing.Image lifecycleLevelImg;
  private int lifecycleLevelId;
  private long objectVerId;
  private int objectTypeId = -1;
  private bool useF = true;
  public VisObjectNode ParentShape;
  private long linkId;
  private int level = int.MinValue;

  private VisObjectNode() => this.Deletable = false;

  public VisObjectNode(int objectTypeId, long objectVerId, long linkid, WinSettings sett)
    : this()
  {
    this.objectVerId = objectVerId;
    this.objectTypeId = objectTypeId;
    this.linkId = linkid;
    this.settings = sett;
    int indexByObjectType = Observer.GetImageIndexByObjectType(objectTypeId);
    this.Initialize(Observer.GetObjectTypeImageList(), indexByObjectType, objectTypeId.ToString());
  }

  public void CheckInOrOut() => this.objectVerId = -this.objectVerId;

  public override void Paint(Graphics g, MapView view)
  {
    this.DrawLifecycleLevel(g);
    this.DrawStatuss(g);
    base.Paint(g, view);
  }

  private void DrawStatuss(Graphics g)
  {
    List<Status> statusImg = this.statusImg;
    if (this.statusImg == null || !this.settings.ShowStatuses)
      return;
    for (int index = 0; index < this.statusImg.Count; ++index)
    {
      System.Drawing.Image img = this.statusImg[index].Img;
      if (img != null)
      {
        Graphics graphics = g;
        System.Drawing.Image image = img;
        PointF location = this.Location;
        double x = (double) location.X - 24.0;
        location = this.Location;
        double y = (double) location.Y - (double) (8 * (index + 1));
        RectangleF rect = new RectangleF(new PointF((float) x, (float) y), new SizeF(8f, 8f));
        graphics.DrawImage(image, rect);
      }
    }
  }

  public override MapObject Pick(PointF p, bool selectableOnly)
  {
    MapObject mapObject = base.Pick(p, selectableOnly);
    if (mapObject == null)
    {
      string toolTipText = this.GetToolTipText(p);
      if (toolTipText != null && !toolTipText.Equals(string.Empty))
      {
        MapBasicNode mapBasicNode = new MapBasicNode();
        mapBasicNode.Selectable = false;
        mapBasicNode.Movable = false;
        mapBasicNode.Copyable = false;
        mapBasicNode.Deletable = false;
        mapBasicNode.DragsNode = false;
        mapBasicNode.Editable = false;
        mapBasicNode.ToolTipText = toolTipText;
        return (MapObject) mapBasicNode;
      }
    }
    return mapObject;
  }

  private int GetStatusMaxElementsCount()
  {
    return this.statusImg == null || !this.settings.ShowStatuses ? 0 : this.statusImg.Count;
  }

  private string GetLCLevelName()
  {
    return this.lifecycleLevelId == 0 ? (string) null : LifecycleLevelInfo.GetLCLevelName(this.lifecycleLevelId);
  }

  private string GetStatusName(int i)
  {
    return this.statusImg == null || this.statusImg.Count < i + 1 ? (string) null : this.statusImg[i].Caption;
  }

  private string GetToolTipText(PointF p)
  {
    for (int index = 0; index < this.GetStatusMaxElementsCount() + 1; ++index)
    {
      if (index != 0 || this.settings.ShowLifecycleLevel)
      {
        RectangleF rectangleF;
        ref RectangleF local = ref rectangleF;
        PointF location1 = this.Location;
        double x = (double) location1.X - 24.0;
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

  private void DrawLifecycleLevel(Graphics g)
  {
    if (this.lifecycleLevelImg == null || !this.settings.ShowLifecycleLevel)
      return;
    Graphics graphics = g;
    System.Drawing.Image lifecycleLevelImg = this.lifecycleLevelImg;
    PointF location = this.Location;
    double x = (double) location.X - 24.0;
    location = this.Location;
    double y = (double) location.Y;
    RectangleF rect = new RectangleF(new PointF((float) x, (float) y), new SizeF(16f, 8f));
    graphics.DrawImage(lifecycleLevelImg, rect);
  }

  public void SetLifecycleLevel(int levelId)
  {
    if (levelId == 0)
      return;
    this.lifecycleLevelId = levelId;
    int index = Observer.objectTypeImageService.IndexOf(8, levelId);
    this.lifecycleLevelImg = Observer.GetObjectTypeImageList().Images[index];
  }

  public void SetStatus(byte[] status, IElementStatusesClientService svc)
  {
    if (status == null)
      return;
    this.statusImg = StatusCollection.GetStatus(status, svc);
  }

  public long LinkId => this.linkId;

  public bool UseF
  {
    get => this.useF;
    set => this.useF = value;
  }

  public int ObjectTypeId => this.objectTypeId;

  public long ObjectVerId => this.objectVerId;

  public int Level
  {
    get => this.level;
    set => this.level = value;
  }

  public override bool Equals(object obj) => base.Equals(obj);

  public bool IsEquals(object obj)
  {
    bool flag = base.Equals(obj);
    if (obj == null || !(obj is VisObjectNode))
      return false;
    VisObjectNode visObjectNode = obj as VisObjectNode;
    if (this.objectTypeId != visObjectNode.objectTypeId)
      return false;
    if (this.objectVerId != visObjectNode.objectVerId && this.objectVerId != -visObjectNode.objectVerId)
      return base.Equals(obj);
    int num = flag ? 1 : 0;
    return true;
  }

  public override int GetHashCode() => base.GetHashCode();
}

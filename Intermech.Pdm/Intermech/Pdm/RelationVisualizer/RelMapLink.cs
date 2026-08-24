// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.RelMapLink
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map;
using System;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

[Serializable]
public class RelMapLink : MapLabeledLink
{
  private long relId;
  private long relTypeId = -1;
  private bool useF = true;

  public RelMapLink(double Count, long relationId, long relationTypeId, bool isShowToArrow)
    : this(isShowToArrow)
  {
    this.SetCount(Count);
    this.relId = relationId;
    this.relTypeId = relationTypeId;
  }

  public RelMapLink(bool isShowToArrow)
    : this()
  {
    this.ToArrow = isShowToArrow;
    this.ToArrowShaftLength = 1f;
    this.FromArrowLength = 1f;
    this.Deletable = false;
    this.Movable = false;
    this.Relinkable = false;
    this.Copyable = false;
    this.Resizable = false;
    this.Pen = Pens.Gray;
  }

  public RelMapLink()
  {
  }

  public long RelTypeId
  {
    get => this.relTypeId;
    set => this.relTypeId = value;
  }

  public bool UseF
  {
    get => this.useF;
    set => this.useF = value;
  }

  public long RelId
  {
    get => this.relId;
    set => this.relId = value;
  }

  public void SetCount(double Count)
  {
    if (Count != 0.0 && Count != 1.0)
    {
      MapText mapText = new MapText();
      mapText.Alignment = 1;
      mapText.Selectable = false;
      mapText.Text = Count.ToString();
      this.MidLabel = (MapObject) mapText;
    }
    else
      this.MidLabel = (MapObject) null;
  }
}

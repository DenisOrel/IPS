// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.ObjectShape
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Pdm.RelationVisualizer;
using Intermech.Map;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

[Serializable]
public class ObjectShape
{
  private VisObjectNode node;
  private long projID;
  private long partID;

  public ObjectShape()
  {
  }

  public ObjectShape(long projID, long partID)
  {
    this.partID = partID;
    this.projID = projID;
  }

  public ObjectShape(VisObjectNode vobject)
  {
    this.node = vobject;
    this.partID = vobject.ObjectVerId;
  }

  public static string GetShortCaption(
    string capt,
    WinSettings setts,
    int objTypeId,
    long partId,
    int width)
  {
    if (capt.Equals(string.Empty))
      return ObjectShape.GetNameIfNoCaption(setts, objTypeId, partId);
    int length = width / 5 / 4;
    if (length > setts.MaxCaptionLength)
      length = setts.MaxCaptionLength;
    return capt.Length <= length ? capt : capt.Substring(0, length) + "..";
  }

  private static string GetNameIfNoCaption(WinSettings setts, int objTypeId, long objVerId)
  {
    switch (setts.NoCaptionFormula)
    {
      case RelVisPred.NoCaptionFormula.Nom:
        return objVerId.ToString();
      case RelVisPred.NoCaptionFormula.ObjType_Nom:
        return $"{MetaDataHelper.GetObjectTypeName(objTypeId)} №{objVerId.ToString()}";
      case RelVisPred.NoCaptionFormula.St_ObjType_St_Nom:
        return $"[{MetaDataHelper.GetObjectTypeName(objTypeId)}]{objVerId.ToString()}";
      case RelVisPred.NoCaptionFormula.St_Nom_St_ObjType:
        return $"[{objVerId.ToString()}]{MetaDataHelper.GetObjectTypeName(objTypeId)}";
      default:
        return objVerId.ToString();
    }
  }

  private bool isShowArrow(ILayoutAlgorithm LayoutAlgoritm) => LayoutAlgoritm.isShowArrow();

  public VisObjectNode Node
  {
    set => this.node = value;
    get => this.node;
  }

  public long ProjID
  {
    get => this.projID;
    set => this.projID = value;
  }

  public long PartID
  {
    get => this.partID;
    set => this.partID = value;
  }

  public void UpdateCaption(string caption, WinSettings setts, int objTypeId)
  {
    this.Node.Text = ObjectShape.GetShortCaption(caption, setts, objTypeId, this.PartID, 100000);
  }

  public void CreateObject(
    MapDocument document,
    PointF point,
    int objectTypeId,
    RelVisPred.RelVisLayers layerFlag,
    string caption,
    long linkId,
    int levelId,
    byte[] statuses,
    IElementStatusesClientService svc,
    WinSettings sett,
    Statistic statistic)
  {
    this.Node = new VisObjectNode(objectTypeId, this.partID, linkId, sett);
    this.Node.Location = point;
    this.Node.Text = caption;
    this.Node.ToolTipText = caption;
    this.Node.SetLifecycleLevel(levelId);
    this.Node.SetStatus(statuses, svc);
    document.Layers.Find((object) (int) layerFlag).Add((MapObject) this.Node);
    ++statistic.selectedObjectsCount;
  }

  public RelMapLink CreateRelation(
    MapDocument document,
    ObjectShape parentShape,
    double relCount,
    long relId,
    int relTypeId,
    object cadRelationType,
    RelVisPred.RelVisLayers layerFlag,
    ILayoutAlgorithm LayoutAlgoritm)
  {
    if (parentShape == null || parentShape.Node == null || parentShape.Node.Port == null)
      return (RelMapLink) null;
    bool isShowToArrow = this.isShowArrow(LayoutAlgoritm);
    RelMapLink relation = new RelMapLink(relCount, relId, (long) relTypeId, isShowToArrow);
    relation.ToArrowShaftLength = 1f;
    relation.MidLabelCentered = true;
    if (cadRelationType != null && cadRelationType != DBNull.Value && Convert.ToInt32(cadRelationType) == 1)
    {
      Pen pen = relation.Pen.Clone() as Pen;
      pen.DashStyle = DashStyle.Dash;
      pen.DashPattern = new float[4]{ 10f, 10f, 10f, 10f };
      pen.DashOffset = 20f;
      relation.Pen = pen;
    }
    if (layerFlag == RelVisPred.RelVisLayers.ParentTree)
    {
      relation.ToPort = (IMapPort) this.Node.Port;
      relation.FromPort = (IMapPort) parentShape.Node.Port;
      relation.PenWidth = 0.01f;
    }
    else
    {
      relation.ToPort = (IMapPort) parentShape.Node.Port;
      relation.FromPort = (IMapPort) this.Node.Port;
      relation.PenWidth = 0.2f;
    }
    document.Layers.Find((object) (int) layerFlag).Add((MapObject) relation);
    return relation;
  }
}

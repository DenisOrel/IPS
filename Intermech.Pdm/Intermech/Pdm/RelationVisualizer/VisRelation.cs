// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisRelation
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Expert;
using Intermech.Pdm.VisDialogs;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class VisRelation
{
  public bool Disabled;
  private HybridRowExp dr;

  public IVisRelationData VisRelData { get; internal set; }

  public VisObject Parent { get; internal set; }

  public VisObject Child { get; internal set; }

  internal VisLink Link { get; set; }

  public int ChildLevelNum
  {
    get
    {
      VisLevel parentLevel = this.Child.ParentLevel;
      return parentLevel == null ? 0 : parentLevel.LevelNum;
    }
  }

  public int ParentLevelNum
  {
    get
    {
      VisLevel parentLevel = this.Parent.ParentLevel;
      return parentLevel == null ? 0 : parentLevel.LevelNum;
    }
  }

  public Color LineColor { get; set; }

  public Color HighlightColor { get; set; }

  public DashStyle DStyle { get; set; }

  public string LineText { get; set; }

  public void ProcessStyle(HybridRowExp row)
  {
    this.dr = row;
    this.UpdateStyle();
  }

  public void UpdateStyle()
  {
    LinkNodeData linkStyle = this.Child.ParentScheme.StyleData.GetLinkStyle(this.VisRelData.RelType);
    this.LineColor = linkStyle.LineColor;
    this.HighlightColor = linkStyle.HighlightColor;
    this.DStyle = linkStyle.DStyle;
    this.LineText = "";
    int attributeId = MetaDataHelper.GetAttributeID((object) linkStyle.AttrName);
    if (attributeId < 0)
      return;
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attributeId);
    int indexByName = this.dr.Columns.GetIndexByName(attributeType.AttributeGuid.ToString().ToLower());
    if (indexByName < 0)
      return;
    if (attributeType.FieldType == FieldTypes.ftMeasured)
    {
      if (this.dr[indexByName] == null || this.dr[indexByName] == DBNull.Value)
        return;
      long measureId = MeasureHelper.GetMeasureID(SystemGUIDs.objectShtukiGuid);
      MeasuredValue measuredValue = MeasureHelper.ConvertToMeasuredValue(this.dr[indexByName].ToString());
      if (measuredValue.MeasureID == 0L || measuredValue.MeasureID == measureId)
      {
        if (Math.Abs(measuredValue.Value - 1.0) <= 1E-05)
          return;
        this.LineText = Convert.ToString(measuredValue.Value);
      }
      else
        this.LineText = measuredValue.ToString();
    }
    else
      this.LineText = this.dr[indexByName].ToString();
  }

  public VisRelation(IVisRelationData iRelData) => this.VisRelData = iRelData;

  public VisRelation(IVisRelationData iRelData, VisObject parent, VisObject child)
  {
    this.VisRelData = iRelData;
    this.Parent = parent;
    this.Child = child;
  }

  public static int GetRelTypeBetweenObjTypes(
    int parentTypeId,
    int childTypeId,
    IUserSession session)
  {
    int typeBetweenObjTypes = -1;
    IDBRelationsApplicabilityCollection applicabilityCollection = session.GetRelationsApplicabilityCollection();
    if (applicabilityCollection != null)
    {
      DataTable applicabilitiesList = applicabilityCollection.GetApplicabilitiesList(-1, parentTypeId, childTypeId);
      if (applicabilitiesList != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) applicabilitiesList.Rows)
        {
          int int32 = Convert.ToInt32(row["F_RELATION_TYPE"]);
          if (int32 != -1)
          {
            typeBetweenObjTypes = int32;
            break;
          }
        }
      }
    }
    return typeBetweenObjTypes;
  }

  public bool IsLinkEnabled(bool structLinksEnabled, bool assocLinksEnabled)
  {
    if (this.Disabled)
      return false;
    switch (this.VisRelData.CADType)
    {
      case CADRelType.Structural:
        return structLinksEnabled;
      case CADRelType.Associative:
        return assocLinksEnabled;
      default:
        return true;
    }
  }
}

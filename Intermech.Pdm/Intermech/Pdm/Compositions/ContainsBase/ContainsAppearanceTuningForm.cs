// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ContainsBase.ContainsAppearanceTuningForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions.ContainsBase;

internal class ContainsAppearanceTuningForm : AppearanceTuningForm
{
  private int[] _objectTypeIDs;
  private int[] _relationTypeIDs;

  public ContainsAppearanceTuningForm(
    INode node,
    ContentType content,
    NodeColumnCollection supportedColumns,
    NodeColumnCollection columns,
    bool fullFormReset,
    int[] objectTypeIDs,
    int[] relationTypeIDs,
    params object[] nodeIDs)
  {
    this._objectTypeIDs = objectTypeIDs;
    this._relationTypeIDs = relationTypeIDs;
    this.Init(node, content, supportedColumns, columns, fullFormReset, nodeIDs);
  }

  protected override void Init(
    INode node,
    ContentType content,
    NodeColumnCollection supportedColumns,
    NodeColumnCollection columns,
    bool fullFormReset,
    params object[] nodeIDs)
  {
    this.InitVariables(node, content, supportedColumns, columns, fullFormReset);
    this.objectTypeAttrs = new List<IMSAttribute4ObjectType>();
    if (this._objectTypeIDs != null && this._objectTypeIDs.Length != 0)
    {
      for (int index1 = 0; index1 < this._objectTypeIDs.Length; ++index1)
      {
        List<IMSAttribute4ObjectType> attribute4ObjectTypeList = MetaDataHelper.GetAttribute4ObjectTypeList(this._objectTypeIDs[index1]);
        for (int index2 = 0; index2 < attribute4ObjectTypeList.Count; ++index2)
        {
          if (attribute4ObjectTypeList[index2].OptimizationMode != OptimizationModes.Write)
            this.objectTypeAttrs.Add(attribute4ObjectTypeList[index2]);
        }
      }
    }
    this.relationsTypeAttrs = new List<IMSAttribute4RelationType>();
    if (this._relationTypeIDs != null && this._relationTypeIDs.Length != 0)
    {
      for (int index3 = 0; index3 < this._relationTypeIDs.Length; ++index3)
      {
        List<IMSAttribute4RelationType> relationTypeList = MetaDataHelper.GetAttribute4RelationTypeList(this._relationTypeIDs[index3]);
        for (int index4 = 0; index4 < relationTypeList.Count; ++index4)
        {
          if (relationTypeList[index4].OptimizationMode != OptimizationModes.Write)
            this.relationsTypeAttrs.Add(relationTypeList[index4]);
        }
      }
    }
    this.InitControls();
  }
}

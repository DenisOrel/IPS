// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.VirtualNodeObjectPart
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class VirtualNodeObjectPart : RelatedObjectsPart
{
  public int CategoryID { get; }

  public int ParentCategoryID { get; }

  public VirtualNodeObjectPart(
    int parentCategoryID,
    int categoryID,
    int objTypeID,
    long objID,
    RelatedObjectsRole role,
    int relTypeID,
    IServiceProvider services)
    : base(objTypeID, objID, role, relTypeID, services)
  {
    this.ParentCategoryID = parentCategoryID;
    this.CategoryID = categoryID;
  }

  public override INodeID CreateNodeId(object[] fieldValues, RecordAdapter adapter)
  {
    INodeID nodeId = (INodeID) null;
    if (fieldValues != null && adapter != null && (this.ParentCategoryID == Consts.IMHStandardNodeCategoryID || this.CategoryID == Consts.IMHDetailsMaterialNodeCategoryID))
    {
      long objectId = -1;
      int fieldIndex1 = adapter.GetFieldIndex((object) ObjectsPartBase.ncF_OBJECT_ID);
      if (fieldIndex1 > -1 && fieldIndex1 < fieldValues.Length)
      {
        object fieldValue = fieldValues[fieldIndex1];
        if (fieldValue != null && fieldValue != DBNull.Value)
          objectId = Convert.ToInt64(fieldValue);
      }
      NodeColumnID field1 = new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_NAME, AttributeSourceTypes.Object);
      int fieldIndex2 = adapter.GetFieldIndex((object) field1);
      string empty1 = string.Empty;
      if (fieldIndex2 > -1 && fieldIndex2 < fieldValues.Length)
      {
        object fieldValue = fieldValues[fieldIndex2];
        if (fieldValue != null && fieldValue != DBNull.Value)
          empty1 = Convert.ToString(fieldValue);
      }
      int fieldIndex3 = adapter.GetFieldIndex((object) ObjectsPartBase.ncF_OBJECT_TYPE);
      int num1 = fieldIndex3 <= -1 || fieldIndex3 >= fieldValues.Length ? -1 : Convert.ToInt32(fieldValues[fieldIndex3]);
      int fieldIndex4 = adapter.GetFieldIndex((object) ObjectsPartBase.ncF_OBJECT_ID);
      long num2 = 0;
      if (fieldIndex4 > -1 && fieldIndex4 < fieldValues.Length)
      {
        object fieldValue = fieldValues[fieldIndex4];
        if (fieldValue != null && fieldValue != DBNull.Value)
          num2 = Convert.ToInt64(fieldValue);
      }
      int fieldIndex5 = adapter.GetFieldIndex((object) ObjectsPartBase.ncCAPTION);
      string str = fieldIndex5 <= -1 || fieldIndex5 >= fieldValues.Length ? string.Empty : Convert.ToString(fieldValues[fieldIndex5]);
      NodeColumnID field2 = new NodeColumnID((object) (this.CategoryID == Consts.IMHMaterialsNodeCategoryID ? Intermech.Imbase.Consts.StandartAttrID : Intermech.Imbase.Consts.StandartAssortmentAttrID), AttributeSourceTypes.Object);
      int fieldIndex6 = adapter.GetFieldIndex((object) field2);
      string empty2 = string.Empty;
      if (fieldIndex6 > -1 && fieldIndex6 < fieldValues.Length)
      {
        object fieldValue = fieldValues[fieldIndex6];
        if (fieldValue != null && fieldValue != DBNull.Value)
          empty2 = Convert.ToString(fieldValue);
      }
      nodeId = (INodeID) new StandartFolderNodeID(new CreateObjectNodeParams()
      {
        ObjectTypeID = num1,
        ObjectID = num2,
        Caption = str
      }, empty1, empty2, objectId);
    }
    return nodeId ?? base.CreateNodeId(fieldValues, adapter);
  }

  protected override INodeQuery GetQuery(ConditionStructure[] conditions)
  {
    return (INodeQuery) new VirtualNodeQuery((INodeQuerySupport) this, this._objTypeID, this._objID, this._role, this._relTypeID, conditions);
  }

  public override List<object> GetSpecialFields()
  {
    List<object> specialFields;
    if (this.ParentCategoryID == Consts.IMHStandardNodeCategoryID)
    {
      if (this.CategoryID == Consts.IMHMaterialsNodeCategoryID)
      {
        NodeColumnID nodeColumnId = new NodeColumnID((object) Intermech.Imbase.Consts.StandartAttrID, AttributeSourceTypes.Object);
        specialFields = new List<object>((IEnumerable<object>) new object[4]
        {
          (object) ObjectsPartBase.ncF_OBJECT_TYPE,
          (object) ObjectsPartBase.ncF_OBJECT_ID,
          (object) ObjectsPartBase.ncCAPTION,
          (object) nodeColumnId
        });
      }
      else
      {
        NodeColumnID nodeColumnId1 = new NodeColumnID((object) Intermech.Imbase.Consts.StandartAssortmentAttrID, AttributeSourceTypes.Object);
        NodeColumnID nodeColumnId2 = new NodeColumnID((object) Intermech.Imbase.Consts.ClassAttrID, AttributeSourceTypes.Object);
        specialFields = new List<object>((IEnumerable<object>) new object[5]
        {
          (object) ObjectsPartBase.ncF_OBJECT_TYPE,
          (object) ObjectsPartBase.ncF_OBJECT_ID,
          (object) ObjectsPartBase.ncCAPTION,
          (object) nodeColumnId1,
          (object) nodeColumnId2
        });
      }
    }
    else if (this.CategoryID == Consts.IMHDetailsMaterialNodeCategoryID)
      specialFields = new List<object>((IEnumerable<object>) new object[4]
      {
        (object) new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_NAME, AttributeSourceTypes.Object),
        (object) ObjectsPartBase.ncF_OBJECT_TYPE,
        (object) ObjectsPartBase.ncF_OBJECT_ID,
        (object) ObjectsPartBase.ncCAPTION
      });
    else
      specialFields = base.GetSpecialFields() ?? new List<object>();
    return specialFields;
  }
}

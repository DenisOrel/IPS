// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.Observer
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Interfaces.Pdm.RelationVisualizer;
using Intermech.Localization;
using Intermech.Map;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public sealed class Observer
{
  private const string CadRelationTypeGUID = "cadd94da-306c-11d8-b4e9-00304f19f545";
  public static ICategoryTypeIconService objectTypeImageService;
  private static int attrCount = -1;

  public static int GetImageIndexByObjectType(int objTypeId)
  {
    return Observer.objectTypeImageService.IndexOf(4, objTypeId);
  }

  public static ImageList GetObjectTypeImageList() => Observer.objectTypeImageService.ImageList;

  public static ObjectShape BuildCentralObject(
    long projID,
    Size winSize,
    string caption,
    MapDocument document,
    int objectTypeId,
    ILayoutAlgorithm LayoutAlgoritm,
    IElementStatusesClientService svc,
    WinSettings setts,
    Statistic statistic,
    int levelId)
  {
    ObjectShape objectShape = new ObjectShape(0L, projID);
    objectShape.CreateObject(document, new PointF(0.0f, 0.0f), objectTypeId, RelVisPred.RelVisLayers.GeneralTree, caption, 0L, levelId, (byte[]) null, svc, setts, statistic);
    objectShape.Node.Label.TextColor = Color.DarkGoldenrod;
    objectShape.Node.Label.Bordered = true;
    objectShape.Node.Label.Bold = true;
    objectShape.Node.Level = 0;
    return objectShape;
  }

  public static void BuildParent(
    long objVerId,
    long objId,
    int objType,
    WinSettings setts,
    Statistic statistic,
    IUserSession userSession,
    MapDocument document,
    Size winSize,
    ObjectShape centralShape,
    string filtrationOwnerId,
    Observer.BuildFlags bfl,
    ILayoutAlgorithm LayoutAlgoritm,
    IElementStatusesClientService svc,
    IRelVisObserverService serverService,
    ICompositionsAutosortRule rule)
  {
    if (objVerId == 0L)
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_28"));
    if (userSession == null)
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_29"));
    if (serverService == null)
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_30"));
    int attributeTypeId = MetaDataHelper.GetAttributeTypeID(new Guid("cad00267-306c-11d8-b4e9-00304f19f545"));
    MapLayerCollectionObjectEnumerator enumerator1 = new MapLayerCollectionObjectEnumerator();
    if (bfl == Observer.BuildFlags.UpdateTree)
      enumerator1 = document.GetEnumerator();
    DataTable[] parentTree = serverService.GetParentTree(objVerId, objId, filtrationOwnerId, rule, objType, userSession.SessionGUID, new HybridDictionary()
    {
      [(object) "RELVISSHOWSTRUCTURELINKS"] = (object) RelationVisualiserWindow.ShowStructLinks,
      [(object) "RELVISSHOWASSOCIATIVELINKS"] = (object) RelationVisualiserWindow.ShowAssociativeLinks
    });
    if (parentTree == null)
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_27"));
    List<ObjectShape> projList_input = new List<ObjectShape>();
    projList_input.Add(centralShape);
    for (int index1 = 0; index1 < parentTree.Length; ++index1)
    {
      DataTable dataTable = parentTree[index1];
      List<ObjectShape> objectShapeList = new List<ObjectShape>();
      LayoutAlgoritm.NextLayer();
      for (int index2 = 0; index2 < dataTable.Rows.Count; ++index2)
      {
        DataRow row = dataTable.Rows[index2];
        long int64_1 = Convert.ToInt64(row["F_PRJLINK_ID"]);
        int int32_1 = Convert.ToInt32(row["F_RELATION_TYPE"]);
        object cadRelationType = row["cadd94da-306c-11d8-b4e9-00304f19f545"];
        long int64_2 = Convert.ToInt64(row["F_OBJECT_ID"]);
        long int64_3 = Convert.ToInt64(row["F_PROJ_ID"]);
        if (!row.IsNull("F_OBJECT_TYPE"))
        {
          int int32_2 = Convert.ToInt32(row["F_OBJECT_TYPE"]);
          string caption = Convert.ToString(row["CAPTION"]);
          int int32_3 = Convert.ToInt32(row["F_LEVEL_ID"]);
          byte[] statuses = row["cad005f1-306c-11d8-b4e9-00304f19f545"] as byte[];
          double num = 0.0;
          bool flag = true;
          IMSRelationType relationType = MetaDataHelper.GetRelationType(int32_1);
          if (relationType != null && !relationType.AnyAttributes && MetaDataHelper.GetAttribute4RelationType(int32_1, attributeTypeId) == null)
            flag = false;
          if (flag)
          {
            try
            {
              num = row[Observer.attrCount.ToString()].Equals((object) DBNull.Value) ? 0.0 : Convert.ToDouble(row[Observer.attrCount.ToString()]);
            }
            catch (Exception ex)
            {
            }
          }
          List<ObjectShape> objectShapeByProjId = Observer.GetObjectShapeByProjId(projList_input, int64_2);
          if (objectShapeByProjId.Count > 1)
            statistic.isMultiContainsMode = true;
          switch (bfl)
          {
            case Observer.BuildFlags.CreateTree:
              using (List<ObjectShape>.Enumerator enumerator2 = objectShapeByProjId.GetEnumerator())
              {
                while (enumerator2.MoveNext())
                {
                  ObjectShape current = enumerator2.Current;
                  ObjectShape objectShape = new ObjectShape(int64_2, int64_3);
                  objectShape.CreateObject(document, Observer.GenerateNodePosition(winSize, new Random(), RelVisPred.RelVisLayers.ParentTree, LayoutAlgoritm), int32_2, RelVisPred.RelVisLayers.ParentTree, caption, int64_1, int32_3, statuses, svc, setts, statistic);
                  objectShape.CreateRelation(document, current, num, int64_1, int32_1, cadRelationType, RelVisPred.RelVisLayers.ParentTree, LayoutAlgoritm);
                  objectShape.Node.ParentShape = current.Node;
                  objectShapeList.Add(objectShape);
                }
                continue;
              }
            case Observer.BuildFlags.UpdateTree:
              using (Dictionary<ObjectShape, VisObjectNode>.Enumerator enumerator3 = Observer.FindExistObjectInTheDocument(enumerator1, int64_3, objectShapeByProjId, int64_1).GetEnumerator())
              {
                while (enumerator3.MoveNext())
                {
                  KeyValuePair<ObjectShape, VisObjectNode> current = enumerator3.Current;
                  ObjectShape key = current.Key;
                  VisObjectNode visObjectNode = current.Value;
                  ObjectShape objectShape;
                  if (visObjectNode == null)
                  {
                    objectShape = new ObjectShape(int64_2, int64_3);
                    objectShape.CreateObject(document, Observer.GenerateNodePosition(winSize, new Random(), RelVisPred.RelVisLayers.ParentTree, LayoutAlgoritm), int32_2, RelVisPred.RelVisLayers.ParentTree, caption, int64_1, int32_3, statuses, svc, setts, statistic);
                    objectShape.Node.UseF = false;
                    objectShape.Node.ParentShape = key.Node;
                    objectShape.Node.Level = -(index1 + 1);
                    objectShape.CreateRelation(document, key, num, int64_1, int32_1, cadRelationType, RelVisPred.RelVisLayers.ParentTree, LayoutAlgoritm);
                  }
                  else
                  {
                    objectShape = new ObjectShape(visObjectNode);
                    objectShape.ProjID = int64_2;
                    objectShape.Node.Level = -(index1 + 1);
                    objectShape.UpdateCaption(caption, setts, int32_2);
                    Observer.UpdateShape(visObjectNode, int64_1, num, svc, statuses, LayoutAlgoritm, statistic);
                  }
                  if (objectShape != null)
                    objectShapeList.Add(objectShape);
                }
                continue;
              }
            default:
              continue;
          }
        }
      }
      projList_input.Clear();
      projList_input = objectShapeList;
    }
    statistic.isReadParentTree = true;
  }

  public static void BuildChild(
    long objVerId,
    int objType,
    WinSettings setts,
    Statistic statistic,
    IUserSession userSession,
    MapDocument document,
    Size winSize,
    ObjectShape centralShape,
    string filtrationOwnerId,
    Observer.BuildFlags bfl,
    ILayoutAlgorithm LayoutAlgoritm,
    IElementStatusesClientService svc,
    IRelVisObserverService serverService,
    ICompositionsAutosortRule rule)
  {
    if (objVerId == 0L)
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_28"));
    if (userSession == null)
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_29"));
    if (serverService == null)
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_30"));
    int attributeTypeId = MetaDataHelper.GetAttributeTypeID(new Guid("cad00267-306c-11d8-b4e9-00304f19f545"));
    Observer.attrCount = MetaDataHelper.GetAttributeTypeID(new Guid("cad00267-306c-11d8-b4e9-00304f19f545"));
    HybridDictionary PluginsData = new HybridDictionary();
    if (ServicesManager.GetService(typeof (IClientPluginsService)) is IClientPluginsService service)
      service.GetClientPluginsData(ref PluginsData);
    PluginsData[(object) "RELVISSHOWSTRUCTURELINKS"] = (object) RelationVisualiserWindow.ShowStructLinks;
    PluginsData[(object) "RELVISSHOWASSOCIATIVELINKS"] = (object) RelationVisualiserWindow.ShowAssociativeLinks;
    DataTable[] childTree = serverService.GetChildTree(objVerId, filtrationOwnerId, rule, objType, userSession.SessionGUID, true, true, PluginsData);
    if (childTree == null)
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_26"));
    MapLayerCollectionObjectEnumerator enumerator1 = new MapLayerCollectionObjectEnumerator();
    if (bfl == Observer.BuildFlags.UpdateTree)
      enumerator1 = document.GetEnumerator();
    List<ObjectShape> projList_input = new List<ObjectShape>();
    projList_input.Add(centralShape);
    for (int index1 = 0; index1 < childTree.Length; ++index1)
    {
      DataTable dataTable = childTree[index1];
      List<ObjectShape> objectShapeList = new List<ObjectShape>();
      LayoutAlgoritm.NextLayer();
      for (int index2 = 0; index2 < dataTable.Rows.Count; ++index2)
      {
        DataRow row = dataTable.Rows[index2];
        object cadRelationType = row["cadd94da-306c-11d8-b4e9-00304f19f545"];
        long int64_1 = Convert.ToInt64(row["F_PRJLINK_ID"]);
        int int32_1 = Convert.ToInt32(row["F_RELATION_TYPE"]);
        long int64_2 = Convert.ToInt64(row["F_PROJ_ID"]);
        long int64_3 = Convert.ToInt64(row["F_OBJECT_ID"]);
        int int32_2 = Convert.ToInt32(row["F_OBJECT_TYPE"]);
        string caption = Convert.ToString(row["CAPTION"]);
        int int32_3 = Convert.ToInt32(row["F_LEVEL_ID"]);
        byte[] statuses = row["cad005f1-306c-11d8-b4e9-00304f19f545"] as byte[];
        double num = 0.0;
        bool flag = true;
        IMSRelationType relationType = MetaDataHelper.GetRelationType(int32_1);
        if (relationType != null && !relationType.AnyAttributes && MetaDataHelper.GetAttribute4RelationType(int32_1, attributeTypeId) == null)
          flag = false;
        if (flag)
        {
          try
          {
            num = row[Observer.attrCount.ToString()].Equals((object) DBNull.Value) ? 0.0 : Convert.ToDouble(row[Observer.attrCount.ToString()]);
          }
          catch (Exception ex)
          {
            if (!(ex is InvalidCastException))
              throw ex;
          }
        }
        switch (bfl)
        {
          case Observer.BuildFlags.CreateTree:
            List<ObjectShape> objectShapeByProjId1 = Observer.GetObjectShapeByProjId(projList_input, int64_2);
            if (objectShapeByProjId1.Count > 1)
              statistic.isMultiContainsMode = true;
            using (List<ObjectShape>.Enumerator enumerator2 = objectShapeByProjId1.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                ObjectShape current = enumerator2.Current;
                ObjectShape objectShape = new ObjectShape(int64_2, int64_3);
                objectShape.CreateObject(document, Observer.GenerateNodePosition(winSize, new Random(), RelVisPred.RelVisLayers.ChildTree, LayoutAlgoritm), int32_2, RelVisPred.RelVisLayers.ChildTree, caption, int64_1, int32_3, statuses, svc, setts, statistic);
                objectShape.Node.ParentShape = current.Node;
                objectShape.Node.Level = index1 + 1;
                objectShape.CreateRelation(document, current, num, int64_1, int32_1, cadRelationType, RelVisPred.RelVisLayers.ChildTree, LayoutAlgoritm);
                objectShapeList.Add(objectShape);
              }
              break;
            }
          case Observer.BuildFlags.UpdateTree:
            List<ObjectShape> objectShapeByProjId2 = Observer.GetObjectShapeByProjId(projList_input, int64_2);
            if (objectShapeByProjId2.Count > 1)
              statistic.isMultiContainsMode = true;
            using (Dictionary<ObjectShape, VisObjectNode>.Enumerator enumerator3 = Observer.FindExistObjectInTheDocument(enumerator1, int64_3, objectShapeByProjId2, int64_1).GetEnumerator())
            {
              while (enumerator3.MoveNext())
              {
                KeyValuePair<ObjectShape, VisObjectNode> current = enumerator3.Current;
                ObjectShape key = current.Key;
                VisObjectNode visObjectNode = current.Value;
                ObjectShape objectShape;
                if (visObjectNode == null)
                {
                  objectShape = new ObjectShape(int64_2, int64_3);
                  objectShape.CreateObject(document, Observer.GenerateNodePosition(winSize, new Random(), RelVisPred.RelVisLayers.ChildTree, LayoutAlgoritm), int32_2, RelVisPred.RelVisLayers.ChildTree, caption, int64_1, int32_3, statuses, svc, setts, statistic);
                  objectShape.Node.UseF = false;
                  objectShape.Node.ParentShape = key.Node;
                  objectShape.CreateRelation(document, key, num, int64_1, int32_1, cadRelationType, RelVisPred.RelVisLayers.ChildTree, LayoutAlgoritm);
                }
                else
                {
                  objectShape = new ObjectShape(visObjectNode);
                  objectShape.ProjID = int64_2;
                  objectShape.UpdateCaption(caption, setts, int32_2);
                  Observer.UpdateShape(visObjectNode, int64_1, num, svc, statuses, LayoutAlgoritm, statistic);
                }
                objectShapeList.Add(objectShape);
              }
              break;
            }
        }
      }
      projList_input.Clear();
      projList_input = objectShapeList;
    }
    statistic.isReadChildTree = true;
  }

  public static List<ObjectShape> GetObjectShapeByProjId(
    List<ObjectShape> projList_input,
    long projId)
  {
    List<ObjectShape> objectShapeByProjId = new List<ObjectShape>();
    foreach (ObjectShape objectShape in projList_input)
    {
      if (objectShape.PartID == projId)
        objectShapeByProjId.Add(objectShape);
    }
    return objectShapeByProjId;
  }

  public static int GetRelationsTypeByObjTypes(
    int objectOutTypeId,
    int objectInTypeId,
    IUserSession session)
  {
    int relationsTypeByObjTypes = -1;
    IDBRelationsApplicabilityCollection applicabilityCollection = session.GetRelationsApplicabilityCollection();
    if (applicabilityCollection != null)
    {
      DataTable applicabilitiesList = applicabilityCollection.GetApplicabilitiesList(-1, objectOutTypeId, objectInTypeId);
      if (applicabilitiesList != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) applicabilitiesList.Rows)
        {
          int int32 = Convert.ToInt32(row["F_RELATION_TYPE"]);
          if (int32 != -1)
          {
            relationsTypeByObjTypes = int32;
            break;
          }
        }
      }
    }
    return relationsTypeByObjTypes;
  }

  private static void UpdateShape(
    VisObjectNode vShape,
    long relId,
    double vCount,
    IElementStatusesClientService svc,
    byte[] statuses,
    ILayoutAlgorithm layoutAlg,
    Statistic statistic)
  {
    vShape.SetStatus(statuses, svc);
    layoutAlg.RegistExistObject();
    MapNodeLinkEnumerator nodeLinkEnumerator = vShape.Links;
    nodeLinkEnumerator = nodeLinkEnumerator.GetEnumerator();
    MapNodeLinkEnumerator enumerator = nodeLinkEnumerator.GetEnumerator();
    if (!enumerator.MoveNext())
      return;
    IMapLink current = enumerator.Current;
    vShape.UseF = false;
    ++statistic.selectedObjectsCount;
    if (!(current is RelMapLink))
      return;
    RelMapLink relMapLink = current as RelMapLink;
    if (relMapLink.RelId != relId)
      return;
    relMapLink.SetCount(vCount);
  }

  public static PointF GenerateNodePosition(
    Size winSize,
    Random rnd,
    RelVisPred.RelVisLayers layer,
    ILayoutAlgorithm LayoutAlgoritm)
  {
    return new PointF(0.0f, 0.0f);
  }

  public static List<VisObjectNode> GetObjectByObjVerId(
    MapLayerCollectionObjectEnumerator enumerator,
    long objId)
  {
    List<VisObjectNode> objectByObjVerId = new List<VisObjectNode>();
    foreach (MapObject mapObject in enumerator)
    {
      if (mapObject is VisObjectNode visObjectNode && Math.Abs(visObjectNode.ObjectVerId) == Math.Abs(objId))
        objectByObjVerId.Add(visObjectNode);
    }
    return objectByObjVerId;
  }

  public static Dictionary<ObjectShape, VisObjectNode> FindExistObjectInTheDocument(
    MapLayerCollectionObjectEnumerator enumerator,
    long objId,
    List<ObjectShape> parentst,
    long linkid)
  {
    Dictionary<ObjectShape, VisObjectNode> objectInTheDocument = new Dictionary<ObjectShape, VisObjectNode>();
    foreach (ObjectShape key in parentst)
      objectInTheDocument.Add(key, (VisObjectNode) null);
    int count = parentst.Count;
    int num = 0;
    foreach (MapObject mapObject in enumerator)
    {
      if (mapObject is VisObjectNode)
      {
        VisObjectNode visObjectNode = mapObject as VisObjectNode;
        if (visObjectNode.ObjectVerId == objId && visObjectNode.LinkId == linkid && visObjectNode.UseF)
        {
          foreach (ObjectShape key in parentst)
          {
            if (key.Node.IsEquals((object) visObjectNode.ParentShape))
            {
              objectInTheDocument[key] = visObjectNode;
              ++num;
              break;
            }
          }
          if (num == count)
            break;
        }
      }
    }
    return objectInTheDocument;
  }

  [Serializable]
  public enum BuildFlags
  {
    CreateTree,
    UpdateTree,
  }
}

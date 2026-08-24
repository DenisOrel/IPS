// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.PdmComposition
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Pdm.Substitutes;
using Intermech.PropertyEditors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm;

public static class PdmComposition
{
  public static readonly string ErrorMsg0 = LocalizationHolder.rm.GetString("Pdm_316");
  public static readonly string ErrorMsg1 = LocalizationHolder.rm.GetString("Pdm_317");
  public static readonly string ErrorMsg2 = LocalizationHolder.rm.GetString("Pdm_318") + LocalizationHolder.rm.GetString("Pdm_319") + LocalizationHolder.rm.GetString("Pdm_320");
  public static readonly string ErrorMsg3 = LocalizationHolder.rm.GetString("Pdm_321");
  public static readonly string ErrorMsg4 = $"{LocalizationHolder.rm.GetString("Pdm_322")}\n{LocalizationHolder.rm.GetString("Pdm_323")}";
  public static readonly string ErrorMsg5 = LocalizationHolder.rm.GetString("Pdm_324") + LocalizationHolder.rm.GetString("Pdm_325");
  public static readonly string ErrorMsg6 = LocalizationHolder.rm.GetString("Pdm_326") + LocalizationHolder.rm.GetString("Pdm_327");
  public static readonly string ErrorMsg7 = LocalizationHolder.rm.GetString("Pdm_328") + LocalizationHolder.rm.GetString("Pdm_329");
  public static readonly string ErrorMsg8 = LocalizationHolder.rm.GetString("Pdm_330") + LocalizationHolder.rm.GetString("Pdm_331") + LocalizationHolder.rm.GetString("Pdm_332");
  public static readonly string ErrorMsg9 = LocalizationHolder.rm.GetString("Pdm_333") + LocalizationHolder.rm.GetString("Pdm_334");
  public static readonly string ErrorMsg10 = LocalizationHolder.rm.GetString("Pdm_556");
  public static readonly string SubstitutesGroupNoValues = "{7C6DC585-1812-4D59-9F79-21878EA5C996}";
  private static readonly string Exception1 = LocalizationHolder.rm.GetString("Pdm_335");
  private static readonly string Exception2 = LocalizationHolder.rm.GetString("Pdm_336");
  private static readonly string Exception3 = LocalizationHolder.rm.GetString("Pdm_337");
  private static readonly string Exception4 = LocalizationHolder.rm.GetString("Pdm_338");

  public static void BlockPluginFiltrations(ref DBRecordSetParams paramsSet)
  {
    IFiltrationService service = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
    paramsSet.Tags = service == null || service.Filtration.Tags == null ? new HybridDictionary(0, true) : service.Filtration.Tags;
    paramsSet.Tags[(object) "{2FACA180-73B8-4F24-9928-5623661BBBE6}"] = (object) true;
    paramsSet.Tags[(object) "{325F5CDB-8B8E-4B2D-9AA9-5624A0A64D7E}"] = (object) true;
    paramsSet.Tags[(object) "{529FFE92-FDA7-48B8-AADF-ADB1EE6EF584}"] = (object) true;
    paramsSet.Tags[(object) "{0422E069-0A1D-4235-85E8-C52C3516CFC1}"] = (object) true;
  }

  public static ArrayList DefaultColumnNames(IUserSession session)
  {
    return new ArrayList(6)
    {
      (object) -20,
      (object) -7,
      (object) -2,
      (object) -50,
      (object) -4,
      (object) session.IdentHelper.SubstitutesGroupNoID,
      (object) session.IdentHelper.SubstituteInGroup
    };
  }

  public static List<ColumnDescriptor> DefaultColumns(IUserSession session)
  {
    return new List<ColumnDescriptor>(6)
    {
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PRJLINK_ID, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_TYPE, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.CAPTION, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_LC_STEP, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) session.IdentHelper.SubstitutesGroupNoID, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.ASC, 0),
      new ColumnDescriptor((object) session.IdentHelper.SubstituteInGroup, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.ASC, 0)
    };
  }

  public static ArrayList GetColumnNames(
    IUserSession session,
    List<int> relationAttrs,
    List<int> objectAttrs,
    List<int> requiredAttrs,
    HybridDictionary allAttributes)
  {
    ArrayList columnNames = new ArrayList(0);
    if (relationAttrs != null)
    {
      for (int index = 0; index < relationAttrs.Count; ++index)
      {
        if ((objectAttrs == null || objectAttrs.IndexOf(relationAttrs[index]) < 0) && (requiredAttrs == null || requiredAttrs.IndexOf(relationAttrs[index]) < 0) && allAttributes[(object) relationAttrs[index]] is MyAttributeColumn allAttribute && allAttribute.Visible)
          columnNames.Add((object) relationAttrs[index]);
      }
    }
    if (objectAttrs != null)
    {
      for (int index = 0; index < objectAttrs.Count; ++index)
      {
        if ((requiredAttrs == null || requiredAttrs.IndexOf(objectAttrs[index]) < 0) && allAttributes[(object) objectAttrs[index]] is MyAttributeColumn allAttribute && allAttribute.Visible)
          columnNames.Add((object) objectAttrs[index]);
      }
    }
    if (requiredAttrs != null)
    {
      for (int index = 0; index < requiredAttrs.Count; ++index)
        columnNames.Add((object) requiredAttrs[index]);
    }
    return columnNames;
  }

  public static List<ColumnDescriptor> GetColumns(
    IUserSession session,
    List<int> relationAttrs,
    List<int> objectAttrs,
    List<int> requiredAttrs,
    Dictionary<int, AttributeSourceTypes> attrSources,
    HybridDictionary allAttributes)
  {
    int capacity = 0;
    if (relationAttrs != null)
      capacity = relationAttrs.Count;
    if (objectAttrs != null)
      capacity += objectAttrs.Count;
    List<ColumnDescriptor> columns = new List<ColumnDescriptor>(capacity);
    if (relationAttrs != null)
    {
      for (int index = 0; index < relationAttrs.Count; ++index)
      {
        if ((objectAttrs == null || objectAttrs.IndexOf(relationAttrs[index]) < 0) && (requiredAttrs == null || requiredAttrs.IndexOf(relationAttrs[index]) < 0) && allAttributes[(object) relationAttrs[index]] is MyAttributeColumn allAttribute && allAttribute.Visible)
        {
          AttributeSourceTypes attributeSource = attrSources.ContainsKey(relationAttrs[index]) ? attrSources[relationAttrs[index]] : AttributeSourceTypes.Relation;
          columns.Add(new ColumnDescriptor((object) relationAttrs[index], attributeSource, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0));
        }
      }
    }
    if (objectAttrs != null)
    {
      for (int index = 0; index < objectAttrs.Count; ++index)
      {
        if ((requiredAttrs == null || requiredAttrs.IndexOf(objectAttrs[index]) < 0) && allAttributes[(object) objectAttrs[index]] is MyAttributeColumn allAttribute && allAttribute.Visible)
        {
          AttributeSourceTypes attributeSource = attrSources.ContainsKey(objectAttrs[index]) ? attrSources[objectAttrs[index]] : AttributeSourceTypes.Object;
          columns.Add(new ColumnDescriptor((object) objectAttrs[index], attributeSource, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0));
        }
      }
    }
    if (requiredAttrs != null)
    {
      for (int index = 0; index < requiredAttrs.Count; ++index)
      {
        AttributeSourceTypes attributeSource = attrSources.ContainsKey(requiredAttrs[index]) ? attrSources[requiredAttrs[index]] : AttributeSourceTypes.Auto;
        columns.Add(new ColumnDescriptor((object) requiredAttrs[index], attributeSource, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0));
      }
    }
    return columns;
  }

  private static List<int> GetChildObjectTypes(
    IUserSession session,
    int parentObjTypeID,
    int relTypeID)
  {
    List<int> childObjectTypes = new List<int>();
    bool flag1 = false;
    bool flag2 = false;
    CompositionsAutosortRule autosortRule = session.GetCustomService(typeof (ICompositionsAutomaticSortingService)) is ICompositionsAutomaticSortingService customService ? customService.GetAutosortRule((object) session.SessionGUID, false) : (CompositionsAutosortRule) null;
    if (autosortRule != null)
    {
      int index1 = autosortRule.IndexOfParentObjectType(parentObjTypeID, true);
      if (index1 >= 0)
      {
        ChildRelationType childRelationType = autosortRule.ParentObjectTypes[index1][relTypeID];
        if (childRelationType != null)
        {
          for (int index2 = 0; index2 < childRelationType.ChildObjectTypes.Count; ++index2)
          {
            List<int> childrenIdRecursive = MetaDataHelper.GetLocalObjectTypeChildrenIDRecursive(childRelationType.ChildObjectTypes[index2].ObjectTypeID);
            if (!flag1)
            {
              for (int index3 = 0; index3 < childrenIdRecursive.Count; ++index3)
              {
                IMSObjectType objectType = MetaDataHelper.GetObjectType(childrenIdRecursive[index3]);
                flag1 = objectType != null && objectType.IsLocalType;
                if (flag1)
                  break;
              }
            }
            for (int index4 = 0; index4 < childrenIdRecursive.Count; ++index4)
            {
              if (childObjectTypes.IndexOf(childrenIdRecursive[index4]) < 0)
                childObjectTypes.Add(childrenIdRecursive[index4]);
            }
          }
        }
        if (!flag1)
          childObjectTypes.Clear();
        flag2 = childObjectTypes.Count > 0;
      }
      if (!flag2)
      {
        DataTable applicabilitiesList = session.GetRelationsApplicabilityCollection().GetApplicabilitiesList(relTypeID, -1, parentObjTypeID);
        int num1 = -1;
        if (applicabilitiesList != null && applicabilitiesList.Rows.Count > 0)
        {
          List<int> intList1 = new List<int>(applicabilitiesList.Rows.Count);
          foreach (DataRow row in (InternalDataCollectionBase) applicabilitiesList.Rows)
          {
            int int32 = Convert.ToInt32(row["F_OBJECT_TYPE"]);
            if (!intList1.Contains(int32))
              intList1.Add(int32);
          }
          if (intList1.Count == 1)
          {
            num1 = intList1[0];
          }
          else
          {
            List<List<int>> intListList = new List<List<int>>(intList1.Count);
            for (int index5 = 0; index5 < intList1.Count; ++index5)
            {
              List<int> parentsIdReverse = MetaDataHelper.GetObjectTypeParentsIDReverse(intList1[index5]);
              List<int> intList2 = new List<int>(parentsIdReverse.Count);
              for (int index6 = parentsIdReverse.Count - 1; index6 >= 0 && !MetaDataHelper.IsLocalObjectType(parentsIdReverse[index6]); --index6)
                intList2.Insert(0, parentsIdReverse[index6]);
              intListList.Add(intList2);
            }
            int index7 = 0;
            bool flag3 = false;
            while (true)
            {
              int num2 = -1;
              for (int index8 = 0; index8 < intListList.Count; ++index8)
              {
                if (intListList[index8].Count <= index7)
                {
                  flag3 = true;
                  break;
                }
                if (!flag3)
                {
                  num2 = intListList[0][index7];
                  if (num2 != intListList[index8][index7])
                  {
                    flag3 = true;
                    break;
                  }
                  if (flag3)
                    break;
                }
                else
                  break;
              }
              if (!flag3)
              {
                if (num2 != -1)
                  num1 = num2;
                ++index7;
              }
              else
                break;
            }
          }
        }
        applicabilitiesList?.Dispose();
        childObjectTypes.Add(num1);
      }
    }
    return childObjectTypes;
  }

  public static DataTable LoadComposition(
    long ProjID,
    int RelationTypeID,
    List<ColumnDescriptor> Columns,
    string FiltrationOwnerID)
  {
    DataTable dataTable = (DataTable) null;
    if (ProjID == 0L || RelationTypeID == 0 || Columns == null || Columns.Count <= 0)
      return dataTable;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ColumnDescriptor[] array = Columns.ToArray();
      object[] objArray = new object[0];
      SortOrders[] sortOrdersArray = new SortOrders[0];
      DBRecordSetParams paramsSet = new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(-21, RelationalOperators.Equal, (object) ProjID, LogicalOperators.NONE, 0, true)
      }, array);
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(RelationTypeID, FiltrationOwnerID);
      PdmComposition.BlockPluginFiltrations(ref paramsSet);
      if (!(paramsSet.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] is List<long>))
        paramsSet.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) new List<long>(2)
        {
          0L,
          1L
        };
      try
      {
        if (relationCollection != null)
        {
          QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(ProjID);
          relationCollection.ChildObjectTypes = (IList<int>) PdmComposition.GetChildObjectTypes(sessionKeeper.Session, objectInfo.ObjectTypeID, RelationTypeID);
          dataTable = relationCollection.Select(paramsSet);
        }
      }
      catch
      {
      }
    }
    return dataTable;
  }

  private static bool FoundInList(List<MyCompositionObject> objects, long prjLinkID, long objectID)
  {
    if (objects == null || objects.Count <= 0)
      return false;
    for (int index = 0; index < objects.Count; ++index)
    {
      MyCompositionObject compositionObject = objects[index];
      if (compositionObject.ObjectID == objectID && compositionObject.PrjLinkID == prjLinkID)
        return true;
    }
    return false;
  }

  private static List<long> GetGroupsList(IUserSession session, long prjLinkID, long groupNo)
  {
    List<long> groupsList = new List<long>(0);
    if (groupNo >= 0L)
    {
      groupsList.Add(groupNo);
      return groupsList;
    }
    IDBAttribute attributeById = (session.GetRelation(prjLinkID, false) ?? throw new ApplicationException(string.Format(PdmComposition.Exception1, (object) prjLinkID))).GetAttributeByID(session.IdentHelper.SubstitutesGroupNoID);
    object obj = attributeById != null ? attributeById.Value : throw new ApplicationException(string.Format(PdmComposition.Exception2, (object) prjLinkID));
    long result = 0;
    if (!long.TryParse(obj.ToString(), out result))
      throw new ApplicationException(PdmComposition.Exception4);
    if (result >= 0L)
    {
      groupsList.Add(result);
      return groupsList;
    }
    object[] values = attributeById.Values;
    if (values == null)
      throw new ApplicationException(string.Format(PdmComposition.Exception3, (object) prjLinkID));
    if (values.Length <= 1)
    {
      attributeById.ClearValues();
      attributeById.Values = new object[1]{ (object) 0 };
      values = attributeById.Values;
    }
    for (int index = 1; index < values.Length; ++index)
    {
      long num = (long) values[index];
      if (num >= 0L)
        groupsList.Add(num);
    }
    groupsList.Sort();
    if (groupsList.Count <= 0)
      groupsList.Add(0L);
    return groupsList;
  }

  public static void LoadSubstitutionGroups(
    DataTable source,
    int groupNoColumn,
    int substColumn,
    int prjLinkIDColumn,
    int objectIDColumn,
    ref HybridDictionary masterGroups,
    ref HybridDictionary compositionPK)
  {
    if (masterGroups == null)
      masterGroups = new HybridDictionary(0, true);
    if (compositionPK == null)
      compositionPK = new HybridDictionary(0, true);
    compositionPK.Clear();
    if (source == null || source.Columns.Count <= 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      HybridDictionary hybridDictionary1 = new HybridDictionary(0, true);
      foreach (DataRow row in (InternalDataCollectionBase) source.Rows)
      {
        long result1 = 0;
        if (long.TryParse(row[prjLinkIDColumn].ToString(), out result1))
        {
          long result2 = 0;
          if (long.TryParse(row[objectIDColumn].ToString(), out result2))
          {
            long result3 = 0;
            if (!long.TryParse(row[groupNoColumn].ToString(), out result3))
              result3 = 0L;
            MyCompositionObject key1 = new MyCompositionObject(result1, result2);
            compositionPK[(object) key1] = (object) row;
            if (!(hybridDictionary1[(object) result1] is List<long> groupsList))
            {
              groupsList = PdmComposition.GetGroupsList(sessionKeeper.Session, result1, result3);
              hybridDictionary1[(object) result1] = (object) groupsList;
            }
            long result4 = 0;
            if (long.TryParse(row[substColumn].ToString(), out result4))
            {
              for (int index = 0; index < groupsList.Count; ++index)
              {
                long key2 = groupsList[index];
                if (key2 > 0L)
                {
                  if (!(masterGroups[(object) key2] is HybridDictionary hybridDictionary2))
                  {
                    hybridDictionary2 = new HybridDictionary(1, true);
                    masterGroups[(object) key2] = (object) hybridDictionary2;
                  }
                  if (!(hybridDictionary2[(object) 0L] is List<MyCompositionObject>))
                  {
                    List<MyCompositionObject> compositionObjectList = new List<MyCompositionObject>(0);
                    hybridDictionary2[(object) 0L] = (object) compositionObjectList;
                  }
                  if (!(hybridDictionary2[(object) result4] is List<MyCompositionObject> objects))
                  {
                    objects = new List<MyCompositionObject>(1);
                    hybridDictionary2[(object) result4] = (object) objects;
                  }
                  if (!PdmComposition.FoundInList(objects, result1, result2))
                    objects.Add(key1);
                }
              }
            }
          }
        }
      }
    }
  }

  public static DataTable LoadCompositionSelected(
    long ProjID,
    int RelationTypeID,
    string FiltrationOwnerID,
    ISelectedItems items)
  {
    DataTable dataTable = (DataTable) null;
    if (items == null || ProjID == 0L || RelationTypeID == 0)
      return dataTable;
    object[] conditionValue = new object[items.Count];
    for (int index = 0; index < items.Count; ++index)
    {
      IDBRelationID itemData = items.GetItemData(index, typeof (IDBRelationID)) as IDBRelationID;
      conditionValue[index] = (object) itemData.Value;
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ColumnDescriptor[] array = PdmComposition.DefaultColumns(sessionKeeper.Session).ToArray();
      object[] objArray = new object[0];
      SortOrders[] sortOrdersArray = new SortOrders[0];
      DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[2]
      {
        new ConditionStructure(-21, RelationalOperators.Equal, (object) ProjID, LogicalOperators.AND, 0, true),
        new ConditionStructure(-20, RelationalOperators.In, (object) conditionValue, LogicalOperators.NONE, 0, false)
      }, array);
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(RelationTypeID, FiltrationOwnerID);
      try
      {
        if (relationCollection != null)
        {
          QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(ProjID);
          relationCollection.ChildObjectTypes = (IList<int>) PdmComposition.GetChildObjectTypes(sessionKeeper.Session, objectInfo.ObjectTypeID, RelationTypeID);
          dataTable = relationCollection.Select(paramSet);
        }
      }
      catch
      {
      }
    }
    return dataTable;
  }

  public static DataTable LoadCompositionGroup(
    long ProjID,
    long GroupID,
    int RelationTypeID,
    string FiltrationOwnerID)
  {
    DataTable dataTable = (DataTable) null;
    if (GroupID <= 0L || ProjID == 0L || RelationTypeID == 0)
      return dataTable;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ColumnDescriptor[] array = PdmComposition.DefaultColumns(sessionKeeper.Session).ToArray();
      object[] objArray = new object[0];
      SortOrders[] sortOrdersArray = new SortOrders[0];
      DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[2]
      {
        new ConditionStructure(-21, RelationalOperators.Equal, (object) ProjID, LogicalOperators.AND, 0, true),
        new ConditionStructure(sessionKeeper.Session.IdentHelper.SubstitutesGroupNoID, RelationalOperators.Equal, (object) GroupID, LogicalOperators.NONE, 0, false)
      }, array);
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(RelationTypeID, FiltrationOwnerID);
      try
      {
        if (relationCollection != null)
        {
          QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(ProjID);
          relationCollection.ChildObjectTypes = (IList<int>) PdmComposition.GetChildObjectTypes(sessionKeeper.Session, objectInfo.ObjectTypeID, RelationTypeID);
          dataTable = relationCollection.Select(paramSet);
        }
      }
      catch
      {
      }
    }
    return dataTable;
  }

  public static DataTable LoadRelation(
    long PrjLinkID,
    int RelationTypeID,
    List<ColumnDescriptor> Columns,
    string FiltrationOwnerID)
  {
    DataTable dataTable = (DataTable) null;
    if (PrjLinkID == 0L || RelationTypeID == 0 || Columns == null || Columns.Count <= 0)
      return dataTable;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ColumnDescriptor[] array = Columns.ToArray();
      object[] objArray = new object[0];
      SortOrders[] sortOrdersArray = new SortOrders[0];
      DBRecordSetParams paramsSet = new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(-20, RelationalOperators.Equal, (object) PrjLinkID, LogicalOperators.NONE, 0, true)
      }, array);
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(RelationTypeID, FiltrationOwnerID);
      PdmComposition.BlockPluginFiltrations(ref paramsSet);
      if (!(paramsSet.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] is List<long>))
        paramsSet.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) new List<long>(2)
        {
          0L,
          1L
        };
      try
      {
        if (relationCollection != null)
        {
          relationCollection.LocalTypesMode = true;
          dataTable = relationCollection.Select(paramsSet);
        }
      }
      catch
      {
      }
    }
    return dataTable;
  }

  public static bool ListContains(List<MyCompositionObject> list, MyCompositionObject item)
  {
    if (list == null || list.Count <= 0 || item == null)
      return false;
    for (int index = 0; index < list.Count; ++index)
    {
      if (list[index].Equals((object) item))
        return true;
    }
    return false;
  }

  public static bool ListContainsRelation(List<MyCompositionObject> list, MyCompositionObject item)
  {
    if (list == null || list.Count <= 0 || item == null)
      return false;
    for (int index = 0; index < list.Count; ++index)
    {
      if (list[index].PrjLinkID == item.PrjLinkID)
        return true;
    }
    return false;
  }

  public static bool ListRemove(List<MyCompositionObject> list, MyCompositionObject item)
  {
    if (list == null || list.Count <= 0 || item == null)
      return false;
    for (int index = 0; index < list.Count; ++index)
    {
      if (list[index].Equals((object) item))
      {
        list.RemoveAt(index);
        return true;
      }
    }
    return false;
  }

  public static bool ClearSubstsNumber(
    IUserSession userSession,
    int relationTypeID,
    string filtrationOwnerID,
    long projectVersionID,
    ref List<long> changedRelationIds,
    out List<long> deletedRelationIds,
    bool needDeleteAuxiliaryPositionRelations = false)
  {
    deletedRelationIds = new List<long>();
    if (userSession == null || relationTypeID == -1 || projectVersionID == 0L)
      return false;
    ConditionStructure[] conditions = new ConditionStructure[1]
    {
      new ConditionStructure(-21, RelationalOperators.Equal, (object) projectVersionID, LogicalOperators.AND, 0, true)
    };
    ColumnDescriptor[] columnDescriptorArray = new ColumnDescriptor[3];
    ColumnDescriptor columnDescriptor = new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PRJLINK_ID);
    columnDescriptor.ColumnName = ColumnNameMapping.ID;
    columnDescriptorArray[0] = columnDescriptor;
    columnDescriptor = new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PART_ID);
    columnDescriptor.ColumnName = ColumnNameMapping.ID;
    columnDescriptorArray[1] = columnDescriptor;
    columnDescriptor = new ColumnDescriptor((object) Constants.SubstituteGroupNumberAttributeTypeID);
    columnDescriptor.AttributeSource = AttributeSourceTypes.Relation;
    columnDescriptor.ColumnName = ColumnNameMapping.ID;
    columnDescriptorArray[2] = columnDescriptor;
    ColumnDescriptor[] columns = columnDescriptorArray;
    DBRecordSetParams paramsSet = new DBRecordSetParams(conditions, columns);
    PdmComposition.BlockPluginFiltrations(ref paramsSet);
    IDBRelationCollection relationCollection = userSession.GetRelationCollection(relationTypeID, filtrationOwnerID);
    QuickObjectInfo objectInfo = userSession.GetObjectInfo(projectVersionID);
    relationCollection.ChildObjectTypes = (IList<int>) PdmComposition.GetChildObjectTypes(userSession, objectInfo.ObjectTypeID, relationTypeID);
    DataTable dataTable = relationCollection.Select(paramsSet);
    if (dataTable == null)
      return false;
    List<long> longList = new List<long>();
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      long int64Value1 = DataSetProcessor.GetInt64Value(row, -20.ToString(), 0L);
      int num = -22;
      long int64Value2 = DataSetProcessor.GetInt64Value(row, num.ToString(), 0L);
      num = Constants.SubstituteGroupNumberAttributeTypeID;
      long int64Value3 = DataSetProcessor.GetInt64Value(row, num.ToString(), -1L);
      IDBRelation relation = userSession.GetRelation(int64Value1, false);
      if (relation != null)
      {
        try
        {
          relation.Attributes.FindByID(Constants.SubstituteGroupNumberAttributeTypeID)?.Delete(0L);
          relation.Attributes.FindByID(Constants.SubstituteNumberAttributeTypeID)?.Delete(0L);
          relation.Attributes.FindByID(Constants.SubstituteGroupNameAttributeTypeID)?.Delete(0L);
          relation.Attributes.FindByID(Constants.SubstituteNameAttributeTypeID)?.Delete(0L);
          relation.Attributes.FindByID(Constants.DesingActualVariantAttributeTypeID)?.Delete(0L);
        }
        catch
        {
        }
        if ((!needDeleteAuxiliaryPositionRelations || !longList.Contains(int64Value2) ? 0 : (int64Value3 >= 0L ? 1 : 0)) != 0)
        {
          relation.Delete(0L);
          deletedRelationIds.Add(int64Value1);
        }
        else
        {
          if (!longList.Contains(int64Value2) && int64Value3 >= 0L)
            longList.Add(int64Value2);
          if (changedRelationIds != null && !changedRelationIds.Contains(int64Value1))
            changedRelationIds.Add(int64Value1);
        }
      }
    }
    dataTable?.Dispose();
    return true;
  }

  public static int RemoveSubsts(
    ISelectedItems items,
    IServiceProvider viewServices,
    string FiltrationOwnerID,
    out string message,
    out long[] relChIDs,
    out List<long> deletedRelationIds,
    ref Dictionary<long, long> chkOuts)
  {
    deletedRelationIds = new List<long>();
    bool needDeleteAuxiliaryPositionRelations = false;
    if (MessageBox.Show(Strings.NeedDeleteAuxiliaryPositionsWithRelations, Strings.Confirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      needDeleteAuxiliaryPositionRelations = true;
    message = string.Empty;
    relChIDs = new long[0];
    if (chkOuts == null)
      chkOuts = new Dictionary<long, long>();
    chkOuts.Clear();
    List<int> intList = new List<int>(0);
    for (int index = 0; index < items.Count; ++index)
    {
      if (items.GetItemData(index, typeof (IDBRelationID)) is IDBRelationID itemData && !intList.Contains(itemData.RelationType))
        intList.Add(itemData.RelationType);
    }
    if (intList.Count == 0)
      return 0;
    List<long> articles = new List<long>();
    if (items == null || items.Count < 0 || !(items.GetParentData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID parentData))
      return 0;
    string empty = string.Empty;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(parentData.ObjectID, false);
      if (dbObject == null)
      {
        message = string.Format(PdmComposition.ErrorMsg10, (object) parentData.Caption, (object) parentData.ObjectID);
        return -1;
      }
      if (dbObject.CheckoutBy != 0L && dbObject.CheckoutBy != sessionKeeper.Session.UserID)
      {
        message = string.Format(PdmComposition.ErrorMsg7, (object) parentData.Caption);
        return -1;
      }
      if (dbObject.CheckoutBy == 0L && dbObject.ObjectModifyMode == ObjectModifyModes.Checkout)
      {
        message = string.Format(PdmComposition.ErrorMsg8, (object) parentData.Caption);
        return -1;
      }
      string caption = dbObject.Caption;
      articles = (sessionKeeper.Session.GetCustomService(typeof (IArticleService)) as IArticleService).GetListInstances(parentData.ObjectID, (object) sessionKeeper.Session.SessionGUID);
    }
    if (!(viewServices.GetService(typeof (PDMSubstitutesEditorOptionsHolder)) is PDMSubstitutesEditorOptionsHolder editorOptionsHolder))
      editorOptionsHolder = new PDMSubstitutesEditorOptionsHolder(PDMSubstitutesEditorMode.DialogMultiInstances, AVSSpecificationForm.A, articles);
    for (int index = intList.Count - 1; index >= 0; --index)
    {
      if (!MetaDataHelper.HasRelationTypeSubstitutes(intList[index]))
        intList.RemoveAt(index);
    }
    if (intList.Count == 0)
    {
      message = PdmComposition.ErrorMsg6;
      return -1;
    }
    if (intList.Count != 1)
    {
      message = PdmComposition.ErrorMsg9;
      return -1;
    }
    List<long> longList = new List<long>();
    ProgressForm progressForm = (ProgressForm) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBTransactions dbTransactions = sessionKeeper.Session.GetCustomService(typeof (IDBTransactions)) as IDBTransactions;
      bool flag = false;
      try
      {
        progressForm = ProgressForm.Execute(LocalizationHolder.rm.GetString("Pdm_549"), LocalizationHolder.rm.GetString("Pdm_540"), 0, editorOptionsHolder.Articles.Count, false, string.Empty, (EventHandler) null);
        progressForm.SetProgressValue(0);
        try
        {
          dbTransactions?.StartTransaction();
          List<long> changedRelationIds = new List<long>();
          for (int index = 0; index < editorOptionsHolder.Articles.Count; ++index)
          {
            IDBObject dbObject = sessionKeeper.Session.GetObject(editorOptionsHolder.Articles[index]);
            if (dbObject.CheckoutBy != sessionKeeper.Session.UserID && dbObject.CheckoutBy != 0L && dbObject.ObjectModifyMode == ObjectModifyModes.Checkout)
              dbObject = dbObject.CheckOut(true);
            long objectId = dbObject.ObjectID;
            if (objectId != editorOptionsHolder.Articles[index])
              chkOuts.Add(editorOptionsHolder.Articles[index], objectId);
            List<long> deletedRelationIds1 = (List<long>) null;
            PdmComposition.ClearSubstsNumber(sessionKeeper.Session, intList[0], FiltrationOwnerID, objectId, ref changedRelationIds, out deletedRelationIds1, needDeleteAuxiliaryPositionRelations);
            deletedRelationIds.AddRange((IEnumerable<long>) deletedRelationIds1);
            longList.AddRange((IEnumerable<long>) changedRelationIds);
            changedRelationIds.Clear();
            progressForm.SetProgressValue(index + 1);
          }
          flag = true;
        }
        catch
        {
          if (dbTransactions != null && dbTransactions.InTransaction)
          {
            dbTransactions.Rollback();
            dbTransactions = (IDBTransactions) null;
          }
          throw;
        }
      }
      finally
      {
        if (progressForm != null)
        {
          progressForm.SetProgressValue(5);
          progressForm.CanCloseForm = true;
          progressForm.Close();
          progressForm.Dispose();
        }
        if (dbTransactions != null && dbTransactions.InTransaction)
        {
          if (flag)
            dbTransactions.Commit();
          else
            dbTransactions.Rollback();
        }
      }
    }
    relChIDs = longList.ToArray();
    return 0;
  }

  [Obsolete]
  private static int RemoveSubstsEx(
    ISelectedItems items,
    IServiceProvider viewServices,
    string FiltrationOwnerID,
    out string message,
    out long[] relChIDs,
    ref Dictionary<long, long> chkOuts)
  {
    if (chkOuts == null)
      chkOuts = new Dictionary<long, long>();
    if (!(viewServices.GetService(typeof (PDMSubstitutesEditorOptionsHolder)) is PDMSubstitutesEditorOptionsHolder editorOptionsHolder))
      editorOptionsHolder = new PDMSubstitutesEditorOptionsHolder(PDMSubstitutesEditorMode.Default, AVSSpecificationForm.Single, (List<long>) null);
    chkOuts.Clear();
    bool flag = false;
    long num = 0;
    List<int> intList = new List<int>(0);
    ProgressForm progressForm = (ProgressForm) null;
    List<long> changedRelationIds = new List<long>();
    Dictionary<long, RelationAttributesPackage> newGroups;
    try
    {
      progressForm = ProgressForm.Execute(LocalizationHolder.rm.GetString("Pdm_549"), LocalizationHolder.rm.GetString("Pdm_548"), 0, 3, false, string.Empty, (EventHandler) null);
      progressForm.SetProgressValue(0);
      relChIDs = new long[0];
      message = string.Empty;
      if (items == null || items.Count < 0 || !(items.GetParentData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID parentData))
        return 0;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(parentData.ObjectID);
        if (dbObject.CheckoutBy > 0L && dbObject.CheckoutBy != sessionKeeper.Session.UserID)
        {
          message = string.Format(PdmComposition.ErrorMsg7, (object) dbObject.Caption);
          return -1;
        }
        if (dbObject.CheckoutBy == 0L)
        {
          if (dbObject.ObjectModifyMode == ObjectModifyModes.Checkout)
          {
            message = string.Format(PdmComposition.ErrorMsg8, (object) dbObject.Caption);
            return -1;
          }
        }
      }
      for (int index = 0; index < items.Count; ++index)
      {
        if (items.GetItemData(index, typeof (IDBRelationID)) is IDBRelationID itemData && !intList.Contains(itemData.RelationType))
          intList.Add(itemData.RelationType);
      }
      if (intList.Count <= 0)
        return 0;
      progressForm.SetProgressValue(1);
      for (int index = intList.Count - 1; index >= 0; --index)
      {
        if (!MetaDataHelper.HasRelationTypeSubstitutes(intList[index]))
          intList.RemoveAt(index);
      }
      if (intList.Count == 0)
      {
        message = PdmComposition.ErrorMsg6;
        return -1;
      }
      if (intList.Count != 1)
      {
        message = PdmComposition.ErrorMsg9;
        return -1;
      }
      progressForm.SetProgressValue(2);
      num = parentData.ObjectID;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        SubstituteObjects.InitStaticFields(sessionKeeper.Session);
        flag = (sessionKeeper.Session.GetCustomService(typeof (ISubstitutesService)) as ISubstitutesService).FindCommonArticles(sessionKeeper.Session.SessionGUID, FiltrationOwnerID, (List<long>) null, num, intList[0], (List<ColumnDescriptor>) null, new SubstituteObjects(sessionKeeper.Session), true, editorOptionsHolder.Form, out newGroups);
      }
    }
    finally
    {
      progressForm.SetProgressValue(3);
      progressForm.CanCloseForm = true;
      progressForm.Close();
      progressForm.Dispose();
      progressForm = (ProgressForm) null;
    }
    try
    {
      if (newGroups.Count == 0)
        return 0;
      progressForm = ProgressForm.Execute(LocalizationHolder.rm.GetString("Pdm_549"), LocalizationHolder.rm.GetString("Pdm_540"), 5, newGroups.Count, false, string.Empty, (EventHandler) null);
      progressForm.SetProgressValue(1);
      progressForm.SetProgressValue(2);
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        ISubstitutesService customService = sessionKeeper.Session.GetCustomService(typeof (ISubstitutesService)) as ISubstitutesService;
        if (flag)
        {
          if (newGroups.Count > 0)
          {
            foreach (KeyValuePair<long, RelationAttributesPackage> keyValuePair in newGroups)
            {
              long objectID = keyValuePair.Key;
              if (objectID > 0L)
              {
                IDBObject dbObject = sessionKeeper.Session.GetObject(objectID);
                if (dbObject.CheckoutBy != sessionKeeper.Session.UserID)
                  dbObject = dbObject.CheckOut(true);
                objectID = dbObject.ObjectID;
              }
              if (objectID != keyValuePair.Key)
                chkOuts.Add(keyValuePair.Key, objectID);
            }
            if (chkOuts.Count > 0)
              customService.FindCommonArticles(sessionKeeper.Session.SessionGUID, FiltrationOwnerID, (List<long>) null, num, intList[0], (List<ColumnDescriptor>) null, new SubstituteObjects(sessionKeeper.Session), true, editorOptionsHolder.Form, out newGroups);
          }
        }
      }
      progressForm.SetProgressValue(3);
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        List<long> chRels;
        (sessionKeeper.Session.GetCustomService(typeof (ISubstitutesService)) as ISubstitutesService).WriteRelationAttributesPackages(sessionKeeper.Session.SessionGUID, newGroups, out chRels);
        if (chRels != null)
        {
          for (int index = 0; index < chRels.Count; ++index)
          {
            if (!changedRelationIds.Contains(chRels[index]))
              changedRelationIds.Add(chRels[index]);
          }
        }
      }
      progressForm.SetProgressValue(4);
    }
    finally
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        List<long> deletedRelationIds = (List<long>) null;
        PdmComposition.ClearSubstsNumber(sessionKeeper.Session, intList[0], FiltrationOwnerID, num, ref changedRelationIds, out deletedRelationIds);
      }
      relChIDs = changedRelationIds.ToArray();
      if (progressForm != null)
      {
        progressForm.SetProgressValue(5);
        progressForm.CanCloseForm = true;
        progressForm.Close();
        progressForm.Dispose();
      }
    }
    return 0;
  }
}

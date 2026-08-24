// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.AutoSel.AutoSelCondPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.AutoSelection.Client;
using Intermech.AutoSelection.Client.AutoSelectionNode;
using Intermech.Expert;
using Intermech.Expert.Table;
using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.AutoSel;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.Common;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using Intermech.Interfaces.AutoSelection;
using Intermech.Interfaces.AutoSelection.AutoSelectionCache;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.AutoSel;

[TaskDescription("Инициализация данных для перекачки - Условия автоподбора", "Перекачка данных - Условия автоподбора")]
[TaskType(PumperType.MetaData)]
internal class AutoSelCondPump : TechPumpBase
{
  private readonly Guid _guid = new Guid("{37523270-8ED0-4592-B045-BACD649900E2}");
  protected int atGlobalObjectTypeId;
  protected int atGlobalAttributeTypeId;

  protected override void InitData() => this._tableName = "TC_OSNCOND";

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
      this._dataSource = new TechDataSource((ITechDataBuilder) new AutoSelCondDataBuilder<TechPumpBase>((TechPumpBase) this));
    return this._dataSource;
  }

  private Dictionary<int, List<long>> ReadTableRowsWithDb()
  {
    Dictionary<int, List<long>> dictionary = new Dictionary<int, List<long>>();
    try
    {
      using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
      {
        command.CommandText = $"SELECT * FROM {"TC_OSNANK_ROWS"} ORDER BY {"F_ID"}";
        using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
        {
          int ordinal1 = dataReader.GetOrdinal("F_ID");
          int ordinal2 = dataReader.GetOrdinal("F_ROW_KEY");
          while (dataReader.Read())
          {
            int int32_1 = BasePumpHelper.ToInt32(dataReader[ordinal1]);
            int int32_2 = BasePumpHelper.ToInt32(dataReader[ordinal2]);
            if (dictionary.ContainsKey(int32_1))
            {
              dictionary[int32_1].Add((long) int32_2);
            }
            else
            {
              List<long> longList = new List<long>()
              {
                (long) int32_2
              };
              dictionary.Add(int32_1, longList);
            }
          }
          dataReader.Close();
        }
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Невозможно прочитать информацию о записях по умолчанию таблицы IMBASE из таблицы {"TC_OSNANK_ROWS"} по причине: {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    return dictionary;
  }

  private Dictionary<int, List<AnketaRow>> ReadAnketaRowsWithDb()
  {
    Dictionary<int, List<AnketaRow>> dictionary = new Dictionary<int, List<AnketaRow>>();
    try
    {
      using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
      {
        command.CommandText = string.Format("SELECT \r\n                                                        a.{0}, \r\n                                                        a.{1}, \r\n                                                        a.{2}, \r\n                                                        b.{3}, \r\n                                                        c.{4} \r\n                                                      FROM \r\n                                                        {5} a, \r\n                                                        {6} b, \r\n                                                        {7} c \r\n                                                      WHERE \r\n                                                        a.{8} = b.{9} AND \r\n                                                        c.{9} = b.{10} ", (object) "F_ID", (object) "F_CTLCONDKEY", (object) "F_FLAGS", (object) "F_FIELD", (object) "F_TABLE", (object) "TC_OSNFLDCOND", (object) "IM_FIELDS", (object) "IM_TABLES", (object) "F_FLDKEY", (object) "F_KEY", (object) "F_TABLE_ID");
        using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
        {
          int ordinal1 = dataReader.GetOrdinal("F_ID");
          int ordinal2 = dataReader.GetOrdinal("F_CTLCONDKEY");
          int ordinal3 = dataReader.GetOrdinal("F_FLAGS");
          int ordinal4 = dataReader.GetOrdinal("F_FIELD");
          int ordinal5 = dataReader.GetOrdinal("F_TABLE");
          while (dataReader.Read())
          {
            int int32_1 = BasePumpHelper.ToInt32(dataReader[ordinal1]);
            int int32_2 = BasePumpHelper.ToInt32(dataReader[ordinal2]);
            int int32_3 = BasePumpHelper.ToInt32(dataReader[ordinal3]);
            string fieldName = dataReader.GetString(ordinal4);
            string tableName = dataReader.GetString(ordinal5);
            AnketaRow anketaRow = new AnketaRow(int32_1, tableName, fieldName, int32_2, int32_3);
            if (dictionary.ContainsKey(int32_1))
            {
              if (!dictionary[int32_1].Contains(anketaRow))
                dictionary[int32_1].Add(anketaRow);
            }
            else
            {
              List<AnketaRow> anketaRowList = new List<AnketaRow>()
              {
                anketaRow
              };
              dictionary.Add(int32_1, anketaRowList);
            }
          }
          dataReader.Close();
        }
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage(string.Format("Невозможно прочитать информацию о анкетах (таблица {1}): {0}", (object) ex.Message, (object) "TC_OSNFLDCOND"));
      if (ex is OutOfMemoryException)
        throw;
    }
    return dictionary;
  }

  private Dictionary<int, AutoSelectProcRec> ReadProcListWithDb()
  {
    Dictionary<int, AutoSelectProcRec> dictionary = new Dictionary<int, AutoSelectProcRec>();
    try
    {
      using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
      {
        command.CommandText = $"select * from {"TC_AOPROCS"}";
        using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
        {
          int ordinal1 = dataReader.GetOrdinal("F_KEY");
          int ordinal2 = dataReader.GetOrdinal("F_GROUP");
          int ordinal3 = dataReader.GetOrdinal("F_NAME");
          int ordinal4 = dataReader.GetOrdinal("F_ROOTKEY");
          int ordinal5 = dataReader.GetOrdinal("F_WORKTYPE");
          while (dataReader.Read())
          {
            int int32_1 = BasePumpHelper.ToInt32(dataReader[ordinal1]);
            int int32_2 = BasePumpHelper.ToInt32(dataReader[ordinal2]);
            string str = dataReader.GetString(ordinal3);
            int int32_3 = BasePumpHelper.ToInt32(dataReader[ordinal4]);
            int int32_4 = BasePumpHelper.ToInt32(dataReader[ordinal5]);
            int _groupId = int32_2;
            string _name = str;
            int _rootkey = int32_3;
            int _workType = int32_4;
            AutoSelectProcRec autoSelectProcRec = new AutoSelectProcRec(int32_1, _groupId, _name, _rootkey, _workType);
            dictionary.Add(int32_3, autoSelectProcRec);
          }
          dataReader.Close();
        }
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Невозможно прочитать информацию о процедурах, табл. {"TC_AOPROCS"}: {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    return dictionary;
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechAutoSelCondPump;

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[5]
    {
      ImportingCategory.ImbaseFolders,
      ImportingCategory.TechCeh,
      ImportingCategory.TechExpTables,
      ImportingCategory.TechExpObjStruct,
      ImportingCategory.ImbaseTableLinks
    };
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    record.RecMode = TechObjectRecord.PumpMode.ObjectAndLinks;
    return string.Empty;
  }

  protected override void LoadMetaData4Pump()
  {
    this.atGlobalObjectTypeId = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atGlobalObjectTypeGuid).ID;
    this.atGlobalAttributeTypeId = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atGlobalAttributeTypeGuid).ID;
    base.LoadMetaData4Pump();
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
  }

  private long GetIpsImbaseId(int level, int ctlKey)
  {
    long num = 0;
    if (level >= 1)
      ;
    return level == 0 && ctlKey == 0 ? num : ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.ImbaseFolders, (object) TechcardConsts.Utils.CodeHashCode(ctlKey, level));
  }

  private Dictionary<int, IObjectTypeItem> GetImbaisKey_IpsObjTypeIdpairList()
  {
    Dictionary<int, IObjectTypeItem> objTypeIdpairList = new Dictionary<int, IObjectTypeItem>();
    ImTableInfo tableInfo1 = TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.Perehod);
    IObjectTypeItem byGuid1 = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechPerehodObjTypeGuid);
    if (tableInfo1 != null && byGuid1 != null)
      objTypeIdpairList.Add(tableInfo1.TableKey, byGuid1);
    ImTableInfo tableInfo2 = TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.Operations);
    IObjectTypeItem byGuid2 = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otOperationObjTypeGuid);
    if (tableInfo2 != null && byGuid2 != null)
      objTypeIdpairList.Add(tableInfo2.TableKey, byGuid2);
    ImTableInfo tableInfo3 = TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.Tool);
    IObjectTypeItem byGuid3 = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otRiggingObjTypeGuid);
    if (tableInfo3 != null && byGuid3 != null)
      objTypeIdpairList.Add(tableInfo3.TableKey, byGuid3);
    ImTableInfo tableInfo4 = TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.Oborud);
    IObjectTypeItem byGuid4 = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otInstrumentationObjTypeGuid);
    if (tableInfo4 != null && byGuid4 != null)
      objTypeIdpairList.Add(tableInfo4.TableKey, byGuid4);
    ImTableInfo tableInfo5 = TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.VidIzd);
    IObjectTypeItem byGuid5 = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otArticleTypes);
    if (tableInfo5 != null && byGuid5 != null)
      objTypeIdpairList.Add(tableInfo5.TableKey, byGuid5);
    return objTypeIdpairList;
  }

  private int GetIMBASECatalogIdByWorkType(int worktype)
  {
    switch (worktype)
    {
      case 0:
        return TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.Tool).TableKey;
      case 1:
        return TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.Oborud).TableKey;
      case 2:
        return TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.SpecialWorks).TableKey;
      case 3:
        return TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.SupportMater).TableKey;
      case 4:
        return TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.Operations).TableKey;
      case 5:
        return TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.Perehod).TableKey;
      case 6:
        return TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.Sortament).TableKey;
      default:
        return 0;
    }
  }

  private long GetImbaseTable(int level, int ctlkey, int recKey)
  {
    long imbaseTable = 0;
    if (ctlkey == 0)
      return imbaseTable;
    try
    {
      imbaseTable = this.GetIpsImbaseId(level, ctlkey);
      if (recKey == 0)
        return imbaseTable;
      DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.ImbaseTableLinks, (object) TechcardConsts.Utils.CodeHashCode(ctlkey, recKey)) ?? this._import_data_main.GetValue(ImportingCategory.ImbaseTableLinks, (object) recKey);
      if (dictionaryValue == null || dictionaryValue.NewObjectID == 0L || !(dictionaryValue.Tag is TableLinkTag))
        return imbaseTable;
      TableLinkTag tag = (TableLinkTag) dictionaryValue.Tag;
      if (tag.CatalogKey != ctlkey || tag.Level != level)
        return imbaseTable;
      imbaseTable = dictionaryValue.NewObjectID;
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Невозможно найти в кэше папку или таблицу IMBASE. (F_LEVEL={level}; F_CTLKEY={ctlkey}; F_RECKEY={recKey}): {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    return imbaseTable;
  }

  private Guid GetObjectTypeGuidByWorkType(int worktype)
  {
    switch (worktype)
    {
      case 0:
        return TechcardConsts.TypeConsts.otRiggingObjTypeGuid;
      case 1:
        return TechcardConsts.TypeConsts.otInstrumentationObjTypeGuid;
      case 2:
        return TechcardConsts.TypeConsts.otPersonalObjTypeGuid;
      case 3:
        return TechcardConsts.TypeConsts.otMaterialsObjTypeGuid;
      case 4:
        return TechcardConsts.TypeConsts.otOperationObjTypeGuid;
      case 5:
        return TechcardConsts.TypeConsts.otTechPerehodObjTypeGuid;
      case 6:
        return TechcardConsts.TypeConsts.otZagotGUID;
      default:
        return Guid.Empty;
    }
  }

  private void Add2Cache(AutoSelectNode rule, long objectId)
  {
    this._import_data_main.AddValue(this.GetTechCategory(), (object) rule.Key, objectId);
  }

  private void FillObjectRuleParams(AutoSelectNode node, IDBObject dbRule)
  {
    dbRule.Caption = node.Name;
  }

  private bool CheckRoleWithPump(AutoSelectNode rule)
  {
    bool flag = false;
    int key = rule.Key;
    if (this._import_data_main.GetNewKey(this.GetTechCategory(), (object) key) != 0L)
      flag = true;
    return flag;
  }

  private AutoSelectionMode GetAutoSelectionModeByRule(AutoSelectNode node)
  {
    AutoSelectionMode selectionModeByRule = AutoSelectionMode.Manual;
    if (node.AutoSelect)
    {
      selectionModeByRule = AutoSelectionMode.AutoObject;
      node.IpsObjectTypeGuid.Equals(TechcardConsts.TypeConsts.otTechPerehodObjTypeGuid);
      node.IpsObjectTypeGuid.Equals(TechcardConsts.TypeConsts.otOperationObjTypeGuid);
      if (node.IpsObjectTypeGuid.Equals(TechcardConsts.TypeConsts.otRiggingObjTypeGuid))
        selectionModeByRule = AutoSelectionMode.AutoRelation;
      if (node.IpsObjectTypeGuid.Equals(TechcardConsts.TypeConsts.otInstrumentationObjTypeGuid))
        selectionModeByRule = AutoSelectionMode.AutoRelation;
      node.IpsObjectTypeGuid.Equals(TechcardConsts.TypeConsts.otArticleTypes);
    }
    return selectionModeByRule;
  }

  protected AutoSelectionNodeCondList ConvertRowList2AutoSelectionNodeCondList(
    List<AnketaRow> rows,
    Dictionary<string, Guid> fieldsAttr2GuidList)
  {
    AutoSelectionNodeCondList selectionNodeCondList = new AutoSelectionNodeCondList();
    if (rows != null)
    {
      if (fieldsAttr2GuidList != null)
      {
        try
        {
          foreach (AnketaRow row in rows)
          {
            int ctlCondKey = row.CtlCondKey;
            if (ctlCondKey != 0 && this._import_data_main.GetTag(ImportingCategory.TechExpObjStruct, (object) ctlCondKey) is TechObjectTag tag)
            {
              if (tag.Object is TempFormula condition)
              {
                string key = $"{row.TableName}.{row.FieldName}";
                Guid attributeGuid;
                if (!fieldsAttr2GuidList.TryGetValue(key, out attributeGuid))
                {
                  this.plugin.appManager.AddWarningMessage($"Не найден атрибут соответствующий полю \"{row.FieldName}\" таблицы \"{row.TableName}\" справочника");
                }
                else
                {
                  AutoSelectionNodeCondition selectionNodeCondition = new AutoSelectionNodeCondition(attributeGuid, condition, row.Flag);
                  selectionNodeCondList.Add(selectionNodeCondition);
                }
              }
              else
                this.plugin.appManager.AddWarningMessage($"Условие {ctlCondKey} не найдено в кэше \"{(Enum) ImportingCategory.TechExpObjStruct}\"");
            }
          }
        }
        catch (Exception ex)
        {
          this.plugin.appManager.AddWarningMessage($"Невозможно обработать список записей таблицы {"TC_OSNFLDCOND"}: {ex.Message}");
          if (ex is OutOfMemoryException)
            throw;
        }
        return selectionNodeCondList;
      }
    }
    this.plugin.appManager.AddWarningMessage("Невозможно сформировать список анкет");
    return selectionNodeCondList;
  }

  private bool RegisterRule(
    IAutoSelectionRuleCacheService autoSelRuleService,
    Intermech.AutoSelection.Client.AutoSelectionRule.AutoSelectionRule rule,
    AutoSelectNode node,
    IUserSession session)
  {
    if (autoSelRuleService == null)
    {
      string Message = "Нет службы IAutoSelectionRuleCacheService";
      TechcardConsts.Plugin.appManager.AddErrorMessage(Message);
      return false;
    }
    if (rule == null || node == null)
      return false;
    long objectId = 0;
    if (node.ForCtl != 0L)
      objectId = node.ForCtl;
    List<long> ruleIdList = new List<long>() { rule.RuleID };
    autoSelRuleService.RulesRegister(ruleIdList, objectId, AutoSelectionLinkMode.asotImbaseObject, session.SessionGUID);
    return true;
  }

  private void FillAutoSel3(Intermech.AutoSelection.Client.AutoSelectionRule.AutoSelectionRule rule, AutoSelectNode node)
  {
    this.FillChilds((AutoSelectionNodeBase) rule, node.Childs);
    rule.Mode = this.GetAutoSelectionModeByRule(node);
    rule.Name = node.Name;
  }

  private void FillChilds(AutoSelectionNodeBase root, List<AutoSelectNode> childs)
  {
    foreach (AutoSelectNode child in childs)
    {
      AutoSelectionNodeCommon selectionNodeCommon = (AutoSelectionNodeCommon) null;
      switch (child.NodeFlag)
      {
        case AutoSelectNode.NodeFlags.Normal:
        case AutoSelectNode.NodeFlags.Mandatory:
          selectionNodeCommon = (AutoSelectionNodeCommon) new AutoSelectionNodeItemImbase(root, child.Name);
          this.FillImbaseNode((AutoSelectionNodeItemImbase) selectionNodeCommon, child);
          break;
        case AutoSelectNode.NodeFlags.Select:
        case AutoSelectNode.NodeFlags.Folder:
        case AutoSelectNode.NodeFlags.Dialog:
        case AutoSelectNode.NodeFlags.Slide:
        case AutoSelectNode.NodeFlags.MultiDial:
          selectionNodeCommon = (AutoSelectionNodeCommon) new AutoSelectionNodeFolder(root, child.Name);
          this.FillFolderNode((AutoSelectionNodeFolder) selectionNodeCommon, child);
          break;
        case AutoSelectNode.NodeFlags.Proc:
          selectionNodeCommon = (AutoSelectionNodeCommon) new AutoSelectionNodeProc(root, child.Name);
          this.FillProcNode((AutoSelectionNodeProc) selectionNodeCommon, child);
          break;
        case AutoSelectNode.NodeFlags.Confirm:
          selectionNodeCommon = (AutoSelectionNodeCommon) new AutoSelectionNodeQuest(root, child.Name);
          this.FillQuestNode((AutoSelectionNodeQuest) selectionNodeCommon, child);
          break;
      }
      root.ChildsNodes.Add(selectionNodeCommon);
      this.FillAutoSel3((AutoSelectionNodeBase) selectionNodeCommon, child);
    }
  }

  private void FillAutoSel3(AutoSelectionNodeBase basenode, AutoSelectNode node)
  {
    this.FillChilds(basenode, node.Childs);
    this.FillCondition(basenode as AutoSelectionNodeCommon, node);
  }

  private void FillFolderNode(AutoSelectionNodeFolder folder, AutoSelectNode node)
  {
    switch (node.NodeFlag)
    {
      case AutoSelectNode.NodeFlags.Normal:
        folder.FolderType = AutoSelectionFolderType.SimpleFolder;
        break;
      case AutoSelectNode.NodeFlags.Select:
        folder.FolderType = AutoSelectionFolderType.SelectFolder;
        if (node.TableId != 0)
        {
          long tableId = (long) node.TableId;
          eTable[] eTableArray = (this._import_data_main.GetTag(ImportingCategory.TechExpTables, (object) tableId) ?? this._import_data_main.GetTag(ImportingCategory.TechExpTables, (object) TechExpKeyConverter.ConvertTo(tableId).Value)) is TechObjectTag techObjectTag ? techObjectTag.Object as eTable[] : (eTable[]) null;
          if (eTableArray != null)
          {
            if (eTableArray.Length != 0)
            {
              folder.ExpTables = eTableArray;
              break;
            }
            break;
          }
          this.plugin.appManager.AddWarningMessage($"Таблица экспертной системы не найдена в кэше. F_TABLEFILE={node.TableId}");
          break;
        }
        break;
      case AutoSelectNode.NodeFlags.Folder:
        folder.FolderType = AutoSelectionFolderType.SimpleFolder;
        break;
      case AutoSelectNode.NodeFlags.Dialog:
        folder.FolderType = AutoSelectionFolderType.DialogFolder;
        break;
      case AutoSelectNode.NodeFlags.Slide:
        folder.FolderType = AutoSelectionFolderType.SlideFolder;
        break;
      case AutoSelectNode.NodeFlags.MultiDial:
        folder.FolderType = AutoSelectionFolderType.MultiSelectFolder;
        break;
    }
    folder.Order = node.Order;
  }

  private void FillProcNode(AutoSelectionNodeProc proc, AutoSelectNode node)
  {
    proc.Order = node.Order;
    proc.SetExtProcGuid(new AS_Guid(node.Procedure), false);
  }

  private void FillImbaseNode(AutoSelectionNodeItemImbase imbaseNode, AutoSelectNode node)
  {
    imbaseNode.Order = node.Order;
    if (node.FromCtl != 0L)
      imbaseNode.SetImbaseObjectID(new AS_Long(node.FromCtl), false);
    if (!node.IpsObjectTypeGuid.Equals(Guid.Empty))
      imbaseNode.ObjTypeGuid = new AS_Guid(node.IpsObjectTypeGuid);
    imbaseNode.RelTypeGuid = new AS_Guid(TechcardConsts.TypeConsts.rtTechRelationGuid);
    if (node.Anketa != null && node.Anketa.Count > 0)
      imbaseNode.TableInfo.CondList.AddRange((IEnumerable<AutoSelectionNodeCondition>) node.Anketa);
    foreach (long anketaRow in node.AnketaRows)
    {
      AutoSelectionDefRow autoSelectionDefRow = new AutoSelectionDefRow(anketaRow);
      imbaseNode.TableInfo.RowList.Add(autoSelectionDefRow);
    }
    AutoSelectNode root = node.GetRoot<AutoSelectNode>((System.Func<AutoSelectNode, AutoSelectNode>) (a => a.Parent));
    if (root == null || root == node || !(root.IpsObjectTypeGuid == node.IpsObjectTypeGuid))
      return;
    imbaseNode.ExecObjMode = AutoSelectionExecObjMode.ParentObject;
  }

  private void FillQuestNode(AutoSelectionNodeQuest quest, AutoSelectNode node)
  {
    quest.Order = node.Order;
    quest.Question = node.Name;
  }

  private void FillOneNodeParams(AutoSelectionNodeCommon root, AutoSelectNode node)
  {
    switch (node.NodeFlag)
    {
    }
  }

  private void FillCondition(AutoSelectionNodeCommon autoSelNode, AutoSelectNode basenode)
  {
    if (basenode.CtlCondKey == 0)
      return;
    int ctlCondKey = basenode.CtlCondKey;
    TempFormula tempFormula = this._import_data_main.GetTag(ImportingCategory.TechExpObjStruct, (object) ctlCondKey) is TechObjectTag tag ? tag.Object as TempFormula : (TempFormula) null;
    if (tempFormula != null)
      autoSelNode.Condition = tempFormula;
    else
      this.plugin.appManager.AddWarningMessage($"Условие автоподбора F_KEY={ctlCondKey} не найдено в кэше.");
  }

  private long CreateRule(
    AutoSelectNode node,
    IAutoSelectionRuleCacheService autoSelRuleService,
    IDBObjectCollection dbRuleColl,
    IUserSession session,
    Dictionary<int, AutoSelectProcRec> procRecs)
  {
    long rule1 = 0;
    if (node != null && autoSelRuleService != null)
    {
      if (session != null)
      {
        long rule2;
        try
        {
          IDBObject dbObject = dbRuleColl.Create();
          long objectId = dbObject.ObjectID;
          Intermech.AutoSelection.Client.AutoSelectionRule.AutoSelectionRule rule3 = new Intermech.AutoSelection.Client.AutoSelectionRule.AutoSelectionRule(node.IpsObjectTypeGuid);
          this.FillObjectRuleParams(node, dbObject);
          this.FillAutoSel3(rule3, node);
          rule3.Save(dbObject, session);
          rule3.AttributeType = TechcardConsts.TypeConsts.atImbaseObjectAttrGuid;
          List<AttributeValues> attributeValuesList = new List<AttributeValues>();
          AttributeValues attributeValues1 = new AttributeValues(this.atGlobalObjectTypeId, (object) node.IpsObjectTypeGuid);
          AttributeValues attributeValues2 = new AttributeValues(this.atGlobalAttributeTypeId, (object) TechcardConsts.TypeConsts.atImbaseObjectAttrGuid);
          attributeValuesList.Add(attributeValues1);
          attributeValuesList.Add(attributeValues2);
          dbObject.SetAttributesValues(attributeValuesList.ToArray());
          dbObject.CommitCreation(false);
          rule3.RuleID = dbObject.ObjectID;
          this.RegisterRule(autoSelRuleService, rule3, node, session);
          if (procRecs.ContainsKey(node.Key))
            procRecs[node.Key].Proc = dbObject.ObjectGUID;
          rule2 = objectId;
        }
        catch (Exception ex)
        {
          this.plugin.appManager.AddWarningMessage($"Невозможно импортировать правило автоподбора \"{node}\": {ex.Message}");
          rule2 = 0L;
          if (ex is OutOfMemoryException)
            throw;
        }
        return rule2;
      }
    }
    return rule1;
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic("TC_OSNCOND");
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    return new List<TechRelParam>();
  }

  public override void Exam() => base.Exam();

  public override void Pump()
  {
    if (!TechSettingsHelper.PumpMetaDataType.HasFlag((Enum) TechPumpMetaDataType.AutoSelection))
    {
      this.plugin.appManager.AddInfoMessage("Перекачка условий автоподбора отключена в настройках");
      this.PumpCheckPoint("Перекачка данных отключена", 0);
    }
    else
    {
      this.LoadMetaData4Pump();
      this.LoadImportingCategoryData();
      using (TechDataSource dataSource = this.GetDataSource())
      {
        TechDataReaderInfo dataReaderInfo = dataSource.GetDataReaderInfo(string.Empty);
        this.plugin.appManager.AddInfoMessage($"Количество записей источника данных: {dataReaderInfo.RecordCount}");
        if (ServicesManager.GetService(typeof (ICache)) is ICache)
        {
          if (this._import_data_main != null)
          {
            try
            {
              TechObjectRecordBase tpObjRec1 = (TechObjectRecordBase) this.GetTpObjRec();
              using (IDataReader dataReader = dataReaderInfo.DataReader)
              {
                IUserSession userSession = this.plugin.Idw.GetUserSession();
                IAutoSelectionRuleCacheService autoSelRuleService;
                try
                {
                  autoSelRuleService = userSession.GetCustomService(typeof (IAutoSelectionRuleCacheService)) as IAutoSelectionRuleCacheService;
                }
                catch
                {
                  autoSelRuleService = (IAutoSelectionRuleCacheService) null;
                }
                if (autoSelRuleService == null)
                {
                  this.plugin.appManager.AddWarningMessage("Перекачка условий автоподбора прекращена т.к. не удалось получить соответствующую службу");
                  return;
                }
                IDBObjectCollection objectCollection = userSession.GetObjectCollection(TechcardConsts.TypeConsts.otAutoSelectionTreeObjTypeGuid);
                Dictionary<int, IObjectTypeItem> objTypeIdpairList = this.GetImbaisKey_IpsObjTypeIdpairList();
                if (objTypeIdpairList == null || objTypeIdpairList.Count == 0)
                {
                  this.plugin.appManager.AddWarningMessage("Перекачка условий автоподбора прекращена т.к. не удалось получить соответствия справочников IMBASE типам объектов");
                  return;
                }
                Dictionary<int, AutoSelectProcRec> procRecs = this.ReadProcListWithDb();
                Dictionary<int, List<AnketaRow>> dictionary1 = this.ReadAnketaRowsWithDb();
                Dictionary<int, List<long>> dictionary2 = this.ReadTableRowsWithDb();
                Dictionary<string, Guid> attributeGuidDictionary = this.GetAttributeGuidDictionary(ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings);
                this.PumpCheckPoint("Закачка правил автоподбора", 0);
                tpObjRec1.ParseSchema((IDictionary<string, int>) this.GetTableColumns(dataReader));
                int index = 0;
                AutoSelectNode autoSelectNode1 = (AutoSelectNode) null;
                int num = 0;
                int worktype = 0;
                while (dataReader.Read())
                {
                  TechObjectRecord tpObjRec2 = this.GetTpObjRec();
                  tpObjRec2.Parse(dataReader);
                  int key = tpObjRec2.Key;
                  string name = Convert.ToString(tpObjRec2.Fields["F_NAME"]);
                  int int32_1 = Convert.ToInt32(tpObjRec2.Fields["F_OWNER_KEY"]);
                  int int32_2 = Convert.ToInt32(tpObjRec2.Fields["F_NODEFLAG"]);
                  int int32_3 = Convert.ToInt32(tpObjRec2.Fields["F_LEVEL"]);
                  int int32_4 = Convert.ToInt32(tpObjRec2.Fields["F_RECKEY"]);
                  int int32_5 = Convert.ToInt32(tpObjRec2.Fields["F_CTLKEY"]);
                  int int32_6 = Convert.ToInt32(tpObjRec2.Fields["F_LEVEL1"]);
                  bool boolean = Convert.ToBoolean(tpObjRec2.Fields["F_AUTOSELECT"]);
                  int int32_7 = Convert.ToInt32(tpObjRec2.Fields["F_TABLEFILE"]);
                  int int32_8 = Convert.ToInt32(tpObjRec2.Fields["F_WORKTYPE"]);
                  int int32_9 = Convert.ToInt32(tpObjRec2.Fields["F_CTLCONDKEY"]);
                  bool flag1 = dataReaderInfo.RecordCount == index + 1;
                  AutoSelectNode.NodeFlags flag2 = (AutoSelectNode.NodeFlags) int32_2;
                  if (int32_1 == 0)
                    worktype = 0;
                  if (int32_1 == 0)
                    num = 0;
                  else
                    ++num;
                  int order = num;
                  AutoSelectNode autoSelectNode2 = new AutoSelectNode(name, key, flag2, order, int32_5);
                  List<long> longList;
                  if (dictionary2.TryGetValue(autoSelectNode2.Key, out longList))
                    autoSelectNode2.AnketaRows = longList;
                  autoSelectNode2.TableId = int32_7;
                  autoSelectNode2.CtlCondKey = int32_9;
                  List<AnketaRow> rows;
                  if (dictionary1.TryGetValue(autoSelectNode2.Key, out rows))
                  {
                    autoSelectNode2.Anketa = this.ConvertRowList2AutoSelectionNodeCondList(rows, attributeGuidDictionary);
                    dictionary1.Remove(autoSelectNode2.Key);
                  }
                  if (autoSelectNode2.NodeFlag != AutoSelectNode.NodeFlags.Proc)
                  {
                    autoSelectNode2.ForCtl = this.GetIpsImbaseId(int32_6, int32_5);
                    int catalogIdByWorkType = this.GetIMBASECatalogIdByWorkType(worktype);
                    autoSelectNode2.FromCtl = this.GetImbaseTable(int32_3, catalogIdByWorkType, int32_4);
                  }
                  else
                  {
                    AutoSelectProcRec autoSelectProcRec;
                    if (procRecs.TryGetValue(int32_4, out autoSelectProcRec) && autoSelectProcRec.Proc != Guid.Empty)
                      autoSelectNode2.Procedure = autoSelectProcRec.Proc;
                    else
                      this.plugin.appManager.AddWarningMessage($"Процедура \"{autoSelectNode2.Name}\"KEY={int32_4} не найдена");
                  }
                  if (int32_1 != 0)
                    autoSelectNode2.IpsObjectTypeGuid = this.GetObjectTypeGuidByWorkType(worktype);
                  autoSelectNode2.AutoSelect = boolean;
                  if (int32_1 == 0 | flag1)
                  {
                    worktype = int32_8;
                    try
                    {
                      if (autoSelectNode1 != null)
                      {
                        if (flag1)
                        {
                          AutoSelectNode chNode = autoSelectNode2;
                          autoSelectNode1.AddNode(chNode, int32_1);
                        }
                        if (!this.CheckRoleWithPump(autoSelectNode1))
                        {
                          if (!procRecs.ContainsKey(autoSelectNode1.Key))
                          {
                            IObjectTypeItem objectTypeItem;
                            if (objTypeIdpairList.TryGetValue(autoSelectNode1.CtlKey, out objectTypeItem))
                            {
                              if (autoSelectNode1.IpsObjectTypeGuid.Equals(Guid.Empty))
                              {
                                autoSelectNode1.IpsObjectTypeGuid = objectTypeItem.GUID;
                                if (autoSelectNode1.NodeFlag != AutoSelectNode.NodeFlags.Proc)
                                  autoSelectNode1.Name = objectTypeItem.Name;
                              }
                            }
                            else
                              this.plugin.appManager.AddWarningMessage($"Миграция правила автоподбора \"{autoSelectNode1}\" : тип объекта соответствующий справочнику {autoSelectNode1.CtlKey} не найден, привязка к типу объекта невозможна");
                          }
                          long rule = this.CreateRule(autoSelectNode1, autoSelRuleService, objectCollection, userSession, procRecs);
                          if (rule != 0L)
                            this.Add2Cache(autoSelectNode1, rule);
                        }
                        else
                          continue;
                      }
                    }
                    finally
                    {
                      autoSelectNode1 = autoSelectNode2;
                    }
                  }
                  else
                  {
                    AutoSelectNode chNode = autoSelectNode2;
                    autoSelectNode1?.AddNode(chNode, int32_1);
                  }
                  ++index;
                  if (index % this.CheckCount == 0 || index == dataReaderInfo.RecordCount - 1)
                    this.PumpCheckPoint($"Закачка узлов правил автоподбора ({index} из {dataReaderInfo.RecordCount})", this.CalculatePercent(dataReaderInfo.RecordCount, index, 0, 100));
                }
                dataReader.Close();
                goto label_60;
              }
            }
            catch (Exception ex)
            {
              this.plugin.appManager.AddWarningMessage($"Ошибка закачки правил автоподбора: {ex.Message}");
              if (ex is OutOfMemoryException)
                throw;
              goto label_60;
            }
          }
        }
        this.plugin.appManager.AddWarningMessage($"Интерфейс для работы с закэшированными импортированными данными для \"{(object) this.GetTechCategory()}\" не получен");
        return;
      }
label_60:
      this.PumpCheckPoint("Закачка правил автоподбора завершена", 100);
    }
  }

  public AutoSelCondPump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = false;
    this.taskPump.Repumpble = false;
  }

  protected override Guid GUID => this._guid;
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpDocRefsClass
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для перекачки связей между документами", "Перекачка связей между документами")]
public class PumpDocRefsClass : PumpClass
{
  protected SearchDataPlugin plugin;
  private CacheCategory _docsCache;
  private CacheCategory _docRefsCache;
  private CacheCategory _ecoRefsCache;
  private CacheCategory _docLinksCache;

  protected override Guid GUID => new Guid("{82AC82CA-F612-49cc-9E15-C130EC9B6C72}");

  public PumpDocRefsClass(SearchDataPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  public override void Exam() => this.ExamCheckPoint("Проверка данных успешно завершена", 100);

  public override void Pump()
  {
    SimpleLogger logger = BasePumpHelper.Logger;
    this._docsCache = PumpCache.Category[ImportingCategory.Documents];
    this._docRefsCache = PumpCache.Category[ImportingCategory.DocRefs];
    this._ecoRefsCache = PumpCache.Category[ImportingCategory.ECORefs];
    this._docLinksCache = PumpCache.Category[ImportingCategory.DocLinksCache];
    bool flag = PumpHelper.DBVersion >= 1700;
    try
    {
      using (IDbCommand command = this.plugin.idb2.CreateCommand())
      {
        this.PumpCheckPoint("Определение количества связей для перекачки", 0);
        string str1 = "where dr.doc_id > 0";
        command.CommandText = "select count(*) from docsrefs dr " + str1;
        int int32_1 = Convert.ToInt32(command.ExecuteScalar());
        logger.Write($"{command.CommandText}: {int32_1} result(s)");
        string str2 = "";
        if (flag)
        {
          str2 = " ,dr.VERSION_ID, dr.TOVERSION_ID, d.version_id as aver";
          str1 = $", doclist d {str1} and dr.todoc_id = d.doc_id ";
        }
        command.CommandText = $"select dr.LINK_ID, dr.DOC_ID, dr.TODOC_ID, dr.LINK_TYPE{str2} from DOCSREFS dr {str1} order by dr.DOC_ID";
        IDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
        try
        {
          int index = 1;
          string format = "Перекачка связей документов ({0} из {1})";
          int num1 = 0;
          DocumentTag documentTag = (DocumentTag) null;
          IImportedRelationList importedRelationList = this.plugin.Idw.CreateImportedRelationList();
          while (dataReader.Read())
          {
            this.PumpCheckPoint(string.Format(format, (object) index, (object) int32_1), this.CalculatePercent(int32_1, index, 1, 99));
            logger.Flush();
            try
            {
              int int32_2 = BasePumpHelper.ToInt32(dataReader[0]);
              int int32_3 = BasePumpHelper.ToInt32(dataReader[1]);
              int int32_4 = BasePumpHelper.ToInt32(dataReader[2]);
              if (!dataReader.IsDBNull(3))
                BasePumpHelper.ToInt32(dataReader[3]);
              int key1 = -1;
              int key2 = -1;
              int num2 = -1;
              if (flag)
              {
                key1 = BasePumpHelper.ToInt32(dataReader[4]);
                key2 = BasePumpHelper.ToInt32(dataReader[5]);
                num2 = Convert.ToInt32(dataReader["aver"]);
              }
              if (this._docRefsCache.GetNewKey((object) int32_2) == 0L)
              {
                if (num1 != int32_3)
                {
                  documentTag = (DocumentTag) null;
                  DictionaryValue dictionaryValue = this._docsCache.GetValue((object) int32_3);
                  if (dictionaryValue != null)
                  {
                    documentTag = dictionaryValue.Tag as DocumentTag;
                    num1 = int32_3;
                  }
                  else
                    continue;
                }
                DictionaryValue dictionaryValue1 = this._docsCache.GetValue((object) int32_4);
                if (dictionaryValue1 != null)
                {
                  long newObjectId = dictionaryValue1.NewObjectID;
                  if (newObjectId != 0L)
                  {
                    DocumentTag tag = dictionaryValue1.Tag as DocumentTag;
                    if (flag)
                    {
                      long projId = 0;
                      long num3 = 0;
                      if (documentTag.Versions.TryGetValue(key1, out projId))
                      {
                        if (key2 == -77)
                          key2 = num2;
                        if (tag.Versions.TryGetValue(key2, out num3))
                        {
                          importedRelationList.AddRelationFromID(projId, newObjectId, PumpHelper.RelTypeDocRefID, PumpHelper.MinDBDateTime);
                          importedRelationList.AddAttributeInt(PumpHelper.AttrVerLinkID, num3);
                        }
                        else
                          BasePumpHelper.AppManager.AddWarningMessage($"Версия документа ({int32_4},{key2}) не найдена, невозможно восстановить ссылку на документ (DRT: {int32_2})!");
                      }
                      else
                        BasePumpHelper.AppManager.AddWarningMessage($"Версия документа ({int32_3},{key1}) не найдена, невозможно восстановить ссылку на документ (DRF: {int32_2})!");
                    }
                    else
                    {
                      foreach (KeyValuePair<int, long> version in documentTag.Versions)
                        importedRelationList.AddRelationFromID(version.Value, newObjectId, PumpHelper.RelTypeDocRefID, PumpHelper.MinDBDateTime);
                    }
                    this._docRefsCache.AddValue((object) int32_2, 1L);
                  }
                }
              }
            }
            finally
            {
              ++index;
            }
          }
          importedRelationList.Import();
        }
        finally
        {
          dataReader.Close();
          BlobHelper.Clear();
        }
      }
      using (IDbCommand command = this.plugin.idb2.CreateCommand())
      {
        this.PumpCheckPoint("Определение количества связей ИИ для перекачки", 0);
        string str = "where doc_id > 0 and revdoc_id <> doc_id";
        command.CommandText = "select count(*) from RC " + str;
        int int32_5 = Convert.ToInt32(command.ExecuteScalar());
        logger.Write($"{command.CommandText}: {int32_5} result(s)");
        command.CommandText = $"select DOC_ID, REVDOC_ID, NLIST_1, NLIST_2, NLIST_3, NLIST_4, NLIST_5, SOPROVDOC, PODPIS, DATA_IZM, VERSION_ID, REC_ID from RC {str} order by REVDOC_ID";
        IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
        try
        {
          int index = 1;
          string format = "Перекачка связей документов ИИ ({0} из {1})";
          int num4 = 0;
          List<long> longList = new List<long>();
          IImportedRelationList importedRelationList = this.plugin.Idw.CreateImportedRelationList();
          while (dataReader.Read())
          {
            this.PumpCheckPoint(string.Format(format, (object) index, (object) int32_5), this.CalculatePercent(int32_5, index, 1, 99));
            logger.Flush();
            try
            {
              int int32_6 = BasePumpHelper.ToInt32(dataReader[11]);
              int int32_7 = BasePumpHelper.ToInt32(dataReader[0]);
              int int32_8 = BasePumpHelper.ToInt32(dataReader[10]);
              int int32_9 = BasePumpHelper.ToInt32(dataReader[1]);
              if (this._ecoRefsCache.GetNewKey((object) int32_6) == 0L)
              {
                if (num4 != int32_9)
                {
                  longList.Clear();
                  DictionaryValue dictionaryValue = this._docsCache.GetValue((object) int32_9);
                  if (dictionaryValue != null)
                  {
                    foreach (KeyValuePair<int, long> version in (dictionaryValue.Tag as DocumentTag).Versions)
                      longList.Add(version.Value);
                    num4 = int32_9;
                  }
                  else
                    continue;
                }
                DictionaryValue dictionaryValue2 = this._docsCache.GetValue((object) int32_7);
                if (dictionaryValue2 == null)
                {
                  BasePumpHelper.AppManager.AddWarningMessage($"Документ (DOC_ID={int32_7}) не закачан, невозможно связать с ИИ!");
                }
                else
                {
                  long newObjectId = dictionaryValue2.NewObjectID;
                  long num5 = 0;
                  (dictionaryValue2.Tag as DocumentTag).Versions.TryGetValue(int32_8, out num5);
                  if (newObjectId != 0L)
                  {
                    foreach (long projId in longList)
                    {
                      importedRelationList.AddRelationFromID(projId, newObjectId, PumpHelper.RelTypeECOID, PumpHelper.MinDBDateTime);
                      this._ecoRefsCache.AddValue((object) int32_6, 1L);
                      importedRelationList.AddAttributeInt(PumpHelper.AttrVerLinkID, num5);
                      importedRelationList.AddAttribute(PumpHelper.AttrLRINList1, AttrValueType.stringVal, dataReader[2], 0);
                      importedRelationList.AddAttribute(PumpHelper.AttrLRINList2, AttrValueType.stringVal, dataReader[3], 0);
                      importedRelationList.AddAttribute(PumpHelper.AttrLRINList3, AttrValueType.stringVal, dataReader[4], 0);
                      importedRelationList.AddAttribute(PumpHelper.AttrLRINList4, AttrValueType.stringVal, dataReader[5], 0);
                      importedRelationList.AddAttribute(PumpHelper.AttrLRINList5, AttrValueType.stringVal, dataReader[6], 0);
                      importedRelationList.AddAttribute(PumpHelper.AttrLRISoprovDoc, AttrValueType.stringVal, dataReader[7], 0);
                      importedRelationList.AddAttribute(PumpHelper.AttrLRIPodpis, AttrValueType.stringVal, dataReader[8], 0);
                      object fldvalue = dataReader[9];
                      if (fldvalue is DateTime)
                        BasePumpHelper.FixDateTimeField(ref fldvalue);
                      importedRelationList.AddAttribute(PumpHelper.AttrLRIDate, AttrValueType.datetimeVal, PumpHelper.ToDateTime(fldvalue), 0);
                    }
                    importedRelationList.Import();
                    importedRelationList.Items.Clear();
                  }
                }
              }
            }
            finally
            {
              ++index;
            }
          }
        }
        finally
        {
          dataReader.Close();
          BlobHelper.Clear();
        }
      }
      if (this._docLinksCache.Items != null && this._docLinksCache.Items.Count > 0)
      {
        this.PumpCheckPoint("Определение количества связей ИИ для перекачки", 0);
        int index = 1;
        int count = this._docLinksCache.Items.Count;
        string format = "Перекачка ссылок документов на ИИ ({0} из {1})";
        List<object> packetKeys = new List<object>();
        IImportedObjectList importedObjectList = this.plugin.Idw.CreateImportedObjectList();
        importedObjectList.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
        {
          try
          {
            foreach (object oldKey in packetKeys)
              this._docLinksCache.SetNewKey(oldKey, -1L);
          }
          catch (Exception ex)
          {
            this.plugin.appManager.AddWarningMessage($"Ошибка во время записи результатов импорта выборок в кэш: {ex.Message} StackTrace: {ex.StackTrace}");
          }
          finally
          {
            packetKeys.Clear();
          }
        });
        foreach (KeyValuePair<object, DictionaryValue> keyValuePair in this._docLinksCache.Items)
        {
          this.PumpCheckPoint(string.Format(format, (object) index, (object) count), this.CalculatePercent(count, index, 1, 99));
          int int32 = Convert.ToInt32(keyValuePair.Value.NewObjectID);
          if (int32 != -1)
          {
            int oldKey = (int) ((long) keyValuePair.Key >> 32 /*0x20*/ & (long) uint.MaxValue);
            int key = (int) ((long) keyValuePair.Key & (long) uint.MaxValue);
            DictionaryValue dictionaryValue3 = this._docsCache.GetValue((object) oldKey);
            if (dictionaryValue3 == null)
            {
              BasePumpHelper.AppManager.AddWarningMessage($"Документ (DOC_ID={oldKey}) не закачан, невозможно восстановить ссылку на ИИ!");
            }
            else
            {
              DocumentTag tag = dictionaryValue3.Tag as DocumentTag;
              if (!tag.Versions.ContainsKey(key))
              {
                BasePumpHelper.AppManager.AddWarningMessage($"Версия {key} документа (DOC_ID={oldKey}) не закачана, невозможно восстановить ссылку на ИИ!");
              }
              else
              {
                long version = tag.Versions[key];
                DictionaryValue dictionaryValue4 = this._docsCache.GetValue((object) int32);
                if (dictionaryValue4 == null)
                {
                  BasePumpHelper.AppManager.AddWarningMessage($"ИИ (DOC_ID={int32}) не закачан, невозможно восстановить на него ссылку в документе (DOC_ID={oldKey})!");
                }
                else
                {
                  if (dictionaryValue4.Tag is DocumentTag && (dictionaryValue4.Tag as DocumentTag).Versions.Count > 0)
                  {
                    long num = Math.Abs((dictionaryValue4.Tag as DocumentTag).Versions.First<KeyValuePair<int, long>>().Value);
                    importedObjectList.UseObject(version);
                    importedObjectList.AddAttributeLink(PumpHelper.AttrECOLinkID, num, dictionaryValue4.Caption);
                    importedObjectList.AddAttributeInt(PumpHelper.AttrModificationID, num);
                    packetKeys.Add(keyValuePair.Key);
                  }
                  ++index;
                }
              }
            }
          }
        }
        importedObjectList.Import();
      }
      this.PumpCheckPoint("Перекачка связей документов успешно завершена", 100);
      logger.Write("=========Pump end\r\n\r\n");
    }
    catch (Exception ex)
    {
      logger.Write($"=========Pump abort ({ex.Message})\r\n\r\n");
      throw;
    }
    finally
    {
      this._docsCache.Release();
      this._docRefsCache.Release();
      this._ecoRefsCache.Release();
      this._docLinksCache.Release();
    }
  }
}

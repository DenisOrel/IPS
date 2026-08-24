// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpOtdClass
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.Archives.Common;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces.Copies;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для перекачки листов рассылки ОТД", "Перекачка листов рассылки ОТД")]
public class PumpOtdClass : PumpClass
{
  protected SearchDataPlugin plugin;
  private CacheCategory _docTypes;
  private CacheCategory _docsCache;
  private CacheCategory _listsCache;
  private IImportedObjectList _iol;
  private List<long> _processedCacheKeys = new List<long>();
  private Dictionary<long, int> _pendingSubscribers = new Dictionary<long, int>();
  private int _pendingDT;
  private ICopiesService _copiesService;

  internal static Guid PumperGUID => new Guid("{CBF125DF-ECB1-4DD6-8793-278932C069D4}");

  protected override Guid GUID => PumpOtdClass.PumperGUID;

  public PumpOtdClass(SearchDataPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  public override void Exam() => this.ExamCheckPoint("Проверка данных успешно завершена", 100);

  protected IImportedObjectList Iol
  {
    get
    {
      if (this._iol == null)
      {
        this._iol = this.plugin.Idw.CreateImportedObjectList();
        this._iol.AfterImportEvent += new AfterImportEventDelegate(this._iol_AfterImportEvent);
      }
      return this._iol;
    }
  }

  private void _iol_AfterImportEvent(object sender, EventArgs e)
  {
    IImportedObjectList importedObjectList = (IImportedObjectList) sender;
    for (int index = 0; index < importedObjectList.Items.Count; ++index)
      this._listsCache.AddValue((object) this._processedCacheKeys[index], importedObjectList.Items[index].Object.Object_id);
    this._processedCacheKeys.Clear();
  }

  private void CheckPendingDT()
  {
    if (this._pendingDT == 0)
      return;
    this.CopiesService.ChangeSubscribers(this._pendingDT, this._pendingSubscribers, (object) BasePumpHelper.Session.SessionGUID);
    this._pendingDT = 0;
    this._pendingSubscribers.Clear();
  }

  internal ICopiesService CopiesService
  {
    get
    {
      if (this._copiesService == null)
        this._copiesService = BasePumpHelper.Session.GetCustomService(typeof (ICopiesService)) as ICopiesService;
      return this._copiesService;
    }
  }

  public override void Pump()
  {
    SimpleLogger logger = BasePumpHelper.Logger;
    this._docTypes = PumpCache.Category[ImportingCategory.DocTypes];
    this._docsCache = PumpCache.Category[ImportingCategory.Documents];
    this._listsCache = PumpCache.Category[ImportingCategory.OTDLists];
    try
    {
      using (IDbCommand command = this.plugin.idb.DbConnection.CreateCommand())
      {
        this.PumpCheckPoint("Определение количества листов рассылки ОТД для перекачки", 0);
        command.CommandText = "select count(*) from delivery_list";
        int int32_1 = Convert.ToInt32(command.ExecuteScalar());
        logger.Write($"{command.CommandText}: {int32_1} result(s)");
        command.CommandText = "select d.*, g.user_id from delivery_list d, groups g where d.group_id = g.group_id order by doc_id, doc_type";
        IDataReader dataReader = command.ExecuteReader();
        try
        {
          int index = 1;
          string format = "Перекачка листов рассылки ОТД ({0} из {1})";
          int num1 = 0;
          int num2 = 0;
          int num3 = 0;
          CacheCategory cacheCategory1 = (CacheCategory) null;
          while (dataReader.Read())
          {
            this.PumpCheckPoint(string.Format(format, (object) index, (object) int32_1), this.CalculatePercent(int32_1, index, 1, 99));
            ++index;
            int int32_2 = Convert.ToInt32(dataReader["doc_id"]);
            int int32_3 = Convert.ToInt32(dataReader["doc_type"]);
            long oldKey = BasePumpHelper.MakeCacheKey(int32_2, int32_3);
            if (this._listsCache.GetNewKey((object) oldKey) <= 0L)
            {
              int int32_4 = Convert.ToInt32(dataReader["user_id"]);
              cacheCategory1 = BasePumpHelper.UsersCache;
              CacheCategory cacheCategory2;
              if (int32_4 != 0)
              {
                cacheCategory2 = BasePumpHelper.UsersCache;
              }
              else
              {
                int32_4 = Convert.ToInt32(dataReader["group_id"]);
                cacheCategory2 = BasePumpHelper.GroupsCache;
              }
              if (int32_3 != -2)
              {
                if (num3 != int32_3)
                {
                  this.CheckPendingDT();
                  num3 = int32_3;
                  this._pendingDT = Convert.ToInt32(this._docTypes.GetNewKey((object) int32_3));
                }
                DictionaryValue dictionaryValue = cacheCategory2.GetValue((object) int32_4);
                if (dictionaryValue != null)
                  this._pendingSubscribers.Add(dictionaryValue.NewObjectID, Convert.ToInt32(dataReader["copy_cnt"]));
              }
              else
              {
                this.CheckPendingDT();
                num3 = 0;
                DictionaryValue dictionaryValue1 = this._docsCache.GetValue((object) int32_2);
                if (dictionaryValue1 == null)
                {
                  BasePumpHelper.AppManager.AddWarningMessage($"Документ (DOC_ID={int32_2}) не закачан, невозможно перекачать рабочую копию!");
                }
                else
                {
                  IImportedObjectList iol = this.Iol;
                  if (num2 != int32_2)
                  {
                    string str = dictionaryValue1.Caption;
                    if (str == "")
                      str = "N" + dictionaryValue1.NewObjectID.ToString();
                    string caption = "Лист рассылки для документа " + str;
                    iol.AddObject(ConstsHolder.DeliveryListID, 0, caption);
                    num1 = 0;
                    num2 = int32_2;
                    this._processedCacheKeys.Add(oldKey);
                    iol.AddAttributeInt(ConstsHolder.OriginalObjectID, dictionaryValue1.NewObjectID);
                  }
                  DictionaryValue dictionaryValue2 = BasePumpHelper.UsersCache.GetValue((object) Convert.ToInt32(dataReader["assigned_by"]));
                  if (dictionaryValue2 != null)
                    iol.AddAttributeLink(ConstsHolder.ListOwnerID, dictionaryValue2.NewObjectID, dictionaryValue2.Caption, num1);
                  else
                    iol.AddAttributeNull(ConstsHolder.ListOwnerID, num1);
                  DictionaryValue dictionaryValue3 = cacheCategory2.GetValue((object) int32_4);
                  if (dictionaryValue3 != null)
                    iol.AddAttributeLink(ConstsHolder.SubscribersID, dictionaryValue3.NewObjectID, dictionaryValue3.Caption, num1);
                  else
                    iol.AddAttributeNull(ConstsHolder.SubscribersID, num1);
                  object obj = dataReader["assigned_date"];
                  if (!DBNull.Value.Equals(obj))
                    iol.AddAttribute(ConstsHolder.SubscribersDateID, AttrValueType.datetimeVal, (object) Convert.ToDateTime(obj), num1);
                  else
                    iol.AddAttributeNull(ConstsHolder.SubscribersDateID, num1);
                  iol.AddAttribute(ConstsHolder.NumberOfCopiesID, AttrValueType.integerVal, (object) Convert.ToInt32(dataReader["copy_cnt"]), num1);
                  iol.AddAttributeNull(ConstsHolder.ActualCopyID, num1);
                  ++num1;
                }
              }
            }
          }
          this.CheckPendingDT();
          this.Iol.Import();
        }
        finally
        {
          dataReader.Close();
        }
        this.PumpCheckPoint("Перекачка листов рассылки успешно завершена", 100);
        logger.Write("=========Pump end\r\n\r\n");
      }
    }
    catch (Exception ex)
    {
      logger.Write($"=========Pump abort ({ex.Message}\r\n{ex.StackTrace})\r\n\r\n");
      throw;
    }
    finally
    {
      this._listsCache.Release();
      this._docsCache.Release();
      this._docTypes.Release();
    }
  }

  private FileTypes LinkTypeToFileTypes(int linkType)
  {
    return linkType == -1 ? FileTypes.ftRedlining : (FileTypes) linkType;
  }

  private class PackFlag
  {
    public const int NotPacked = 0;
    public const int MinZLIBMethodID = 1;
    public const int MaxZLIBMethodID = 3;
  }
}

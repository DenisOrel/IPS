// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.ImbaseSync.ImbaseFolderCathegoryUpdatePump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Imbase;
using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.ImbaseSync;

[TaskDescription("Инициализация данных для перекачки - Обновление кэшей папок Imbase существующих в IPS", "Перекачка данных - Импортированные через ImbaseSync данные")]
internal class ImbaseFolderCathegoryUpdatePump : PumpClass
{
  private static long GetCatalogCacheKey(int parentKey, int level)
  {
    return ((long) parentKey << 32 /*0x20*/) + (long) level;
  }

  private DataTable SelectAllFoldersTable(IUserSession session)
  {
    IDBObjectCollection objectCollection = session.GetObjectCollection(ImbaseIDHelper.ObjTypeIdImFolder);
    ColumnDescriptor[] columns = new ColumnDescriptor[3]
    {
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ImbaseIDHelper.AttrIdClassifierKey, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.ASC, 0),
      new ColumnDescriptor((object) ImbaseIDHelper.AttrIdImCode, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0)
    };
    DBRecordSetParams paramSet = new DBRecordSetParams(Array.Empty<ConditionStructure>(), columns);
    return objectCollection.Select(paramSet);
  }

  private Dictionary<string, int> SelectAllCatalogsCache(IUserSession session)
  {
    IDBObjectCollection objectCollection = session.GetObjectCollection(ImbaseIDHelper.ObjTypeIdImCtl);
    ColumnDescriptor[] columns = new ColumnDescriptor[3]
    {
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ImbaseIDHelper.AttrIdClassifierKey, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.ASC, 0),
      new ColumnDescriptor((object) ImbaseIDHelper.AttrIdImCode, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0)
    };
    DBRecordSetParams paramSet = new DBRecordSetParams(Array.Empty<ConditionStructure>(), columns);
    DataTable dataTable = objectCollection.Select(paramSet);
    Dictionary<string, int> dictionary = new Dictionary<string, int>(dataTable.Rows.Count);
    Dictionary<object, DictionaryValue> category = ImportingCategoryDataCache.Instance.GetCache(new ImportingCategory[1]
    {
      ImportingCategory.ImbaseCatalogsCreated
    })?.GetCategory(ImportingCategory.ImbaseCatalogsCreated);
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      long catalogObjVerId = DataSetProcessor.GetInt64Value(row, 0, 0L);
      string stringValue = DataSetProcessor.GetStringValue(row, 1, string.Empty);
      int int32Value = DataSetProcessor.GetInt32Value(row, 2, 0);
      int num = category != null ? category.Where<KeyValuePair<object, DictionaryValue>>((System.Func<KeyValuePair<object, DictionaryValue>, bool>) (cachedValue => cachedValue.Value.NewObjectID == catalogObjVerId)).Select<KeyValuePair<object, DictionaryValue>, int>((System.Func<KeyValuePair<object, DictionaryValue>, int>) (cachedValue => Convert.ToInt32(cachedValue.Key))).FirstOrDefault<int>() : 0;
      if (!string.IsNullOrEmpty(stringValue))
        dictionary[stringValue] = num != 0 ? num : int32Value;
    }
    return dictionary;
  }

  private Dictionary<long, long> LoadIpsImbaseFoldersCache(IUserSession session)
  {
    Dictionary<string, int> dictionary1 = this.SelectAllCatalogsCache(session);
    if (dictionary1 == null || dictionary1.Count == 0)
      return (Dictionary<long, long>) null;
    DataTable dataTable = this.SelectAllFoldersTable(session);
    if (dataTable == null || dataTable.Rows.Count == 0)
      return (Dictionary<long, long>) null;
    Dictionary<long, long> dictionary2 = new Dictionary<long, long>();
    string key = string.Empty;
    int parentKey = 0;
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      long int64Value = DataSetProcessor.GetInt64Value(row, 0, 0L);
      string stringValue = DataSetProcessor.GetStringValue(row, 1, string.Empty);
      int int32Value = DataSetProcessor.GetInt32Value(row, 2, 0);
      if (!string.IsNullOrEmpty(stringValue))
      {
        if (string.IsNullOrEmpty(key) || !stringValue.StartsWith(key))
        {
          key = stringValue.Substring(0, 2);
          if (!dictionary1.TryGetValue(key, out parentKey))
          {
            key = string.Empty;
            continue;
          }
        }
        if (parentKey != 0)
        {
          long catalogCacheKey = ImbaseFolderCathegoryUpdatePump.GetCatalogCacheKey(parentKey, int32Value);
          dictionary2[catalogCacheKey] = int64Value;
        }
      }
    }
    return dictionary2.Count <= 0 ? (Dictionary<long, long>) null : dictionary2;
  }

  private void SyncImbaseFoldersCache(IUserSession session)
  {
    IImportingData cache = ImportingCategoryDataCache.Instance.GetCache(new ImportingCategory[1]
    {
      ImportingCategory.ImbaseFolders
    });
    Dictionary<long, long> dictionary = this.LoadIpsImbaseFoldersCache(session);
    foreach (long key in dictionary.Keys)
    {
      if (cache.GetValue(ImportingCategory.ImbaseFolders, (object) key) == null)
        cache.AddValue(ImportingCategory.ImbaseFolders, (object) key, dictionary[key]);
    }
  }

  protected override Guid GUID => new Guid("C23F7960-5593-46B7-97D6-114A4F7F4620");

  public override void Exam()
  {
    base.Exam();
    ImbaseIDHelper.Initialize(this.Plugin.Imdi);
    this.SyncImbaseFoldersCache(this.plugin.Idw.GetUserSession());
  }

  public ImbaseFolderCathegoryUpdatePump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
  }
}

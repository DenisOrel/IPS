// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.PumpImbaseLookup
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Interface;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Imbase;

[TaskDescription("Получение информации о наборах Imbase", "Перекачка наборов Imbase")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpImbaseLookup(ImbasePlugin plugin) : PumpImbaseClass(plugin)
{
  internal static Guid guid = new Guid("{4F057F99-11F1-4e7d-B908-0EA1ACDA3382}");
  private Dictionary<int, List<IImLookupItem>> ownersList;

  protected override Guid GUID => PumpImbaseLookup.guid;

  public List<IImLookupItem> GetListById(int ownerId)
  {
    return this.ownersList == null || !this.ownersList.ContainsKey(ownerId) ? new List<IImLookupItem>() : this.ownersList[ownerId];
  }

  public object GetLookupValue(IImLookupItem item, ImLookupDataType dataType)
  {
    object lookupValue = (object) null;
    if (dataType == ImLookupDataType.ldtNone)
      dataType = !(item.ValueStr != string.Empty) ? (item.ValueDbl == 0.0 ? (item.ValueInt == 0 ? ImLookupDataType.ldtNmd : ImLookupDataType.ldtInt) : ImLookupDataType.ldtDbl) : ImLookupDataType.ldtStr;
    if ((dataType & ImLookupDataType.ldtStr) > ImLookupDataType.ldtNone)
      lookupValue = (object) item.ValueStr;
    else if ((dataType & ImLookupDataType.ldtInt) > ImLookupDataType.ldtNone)
      lookupValue = (object) item.ValueInt;
    else if ((dataType & ImLookupDataType.ldtDbl) > ImLookupDataType.ldtNone)
      lookupValue = (object) item.ValueDbl;
    else if ((dataType & ImLookupDataType.ldtNmd) > ImLookupDataType.ldtNone)
      lookupValue = (object) item.Name;
    return lookupValue;
  }

  public override void Exam()
  {
    this.ExamCheckPoint("Определение количества записей", 0);
    int tableRecordsCount = this.GetTableRecordsCount(ImLookupItemFactory.TableName);
    int index = 0;
    this.ExamCheckPoint("Получение данных из таблицы " + ImLookupItemFactory.TableName, 1);
    IDataReader defaultDataReader = this.GetDefaultDataReader(ImLookupItemFactory.TableName);
    try
    {
      string format = $"Импорт записи из таблицы {ImLookupItemFactory.TableName} ({{0}} из {{1}})";
      ImLookupItemFactory lookupItemFactory = new ImLookupItemFactory(defaultDataReader, this.plugin.Idw.AppManager);
      this.ownersList = new Dictionary<int, List<IImLookupItem>>(tableRecordsCount);
      while (defaultDataReader.Read())
      {
        ++index;
        this.ExamCheckPoint(string.Format(format, (object) index, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index, 2, 99));
        IImLookupItem imLookupItem = lookupItemFactory.NewItem(defaultDataReader);
        if (imLookupItem != null)
        {
          if (!this.ownersList.ContainsKey(imLookupItem.Owner))
            this.ownersList.Add(imLookupItem.Owner, new List<IImLookupItem>());
          this.ownersList[imLookupItem.Owner].Add(imLookupItem);
        }
      }
    }
    finally
    {
      defaultDataReader.Close();
    }
    this.ExamCheckPoint($"Обработка данных из таблицы {ImLookupItemFactory.TableName} успешно завершена", 100);
  }

  public void Clear()
  {
    if (this.ownersList == null)
      return;
    this.ownersList.Clear();
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.PumpImbaseDataTables
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Imbase;

[TaskDescription("Получение информации о таблицах Imbase", "Перекачка таблиц Imbase")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpImbaseDataTables(ImbasePlugin plugin) : PumpImbaseClass(plugin)
{
  internal static Guid _guid = new Guid("{C5BE11DA-70EC-4108-821F-03560A449648}");

  protected override Guid GUID => PumpImbaseDataTables._guid;

  public override void Exam()
  {
  }

  public override void Pump()
  {
    this.PumpCheckPoint("Определение количества записей для закачки информации о таблицах IMBASE", 0);
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service.GetCache(ImportingCategory.ImbaseGroups, ImportingCategory.ImbaseTablesAttributes, ImportingCategory.ImbaseTables, ImportingCategory.ImbaseGroupsAttributes, ImportingCategory.ImbaseBlobs);
    ArrayList deleteFiles = new ArrayList(cache.GetCategory(ImportingCategory.ImbaseGroups).Count);
    IUserSession userSession = this.plugin.Imdi.UserSession;
    try
    {
      List<ImbaseGroup> tablesList = PumpImbaseTablesHelper.GetTablesList(cache, ImportingCategory.ImbaseTables, 0);
      int count = tablesList.Count;
      this.SetCountPumpRecords(count);
      int index = 0;
      string format = "Закачка таблиц данных IMBASE ({0} из {1})";
      foreach (ImbaseGroup table in tablesList)
      {
        ++index;
        this.PumpCheckPoint(string.Format(format, (object) index, (object) count), this.CalculatePercent(count, index, 2, 99));
        this.PumpTable(userSession, table, cache, deleteFiles);
      }
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ImbaseGroups, ImportingCategory.ImbaseTablesAttributes, ImportingCategory.ImbaseTables, ImportingCategory.ImbaseGroupsAttributes, ImportingCategory.ImbaseBlobs);
      long num = 0;
      string str1 = string.Empty;
      foreach (string str2 in deleteFiles)
      {
        FileInfo fileInfo = new FileInfo(str2);
        if (fileInfo.Exists)
        {
          if (num < fileInfo.Length)
          {
            num = fileInfo.Length;
            str1 = fileInfo.Name;
          }
          File.Delete(str2);
        }
      }
      FileInfo fileInfo1 = new FileInfo(Path.Combine(Path.GetTempPath(), "sizeInfo.txt"));
      if (fileInfo1.Exists)
        File.Delete(fileInfo1.FullName);
      StreamWriter text = fileInfo1.CreateText();
      try
      {
        text.Write($"Максимальный размер блоба: {num} байт, файло {str1}");
      }
      finally
      {
        text.Flush();
        text.Close();
      }
    }
    this.PumpCheckPoint("Создание таблиц данных IMBASE успешно завершено", 100);
  }

  private void PumpTable(
    IUserSession session,
    ImbaseGroup table,
    IImportingData cacheData,
    ArrayList deleteFiles)
  {
    int ownerUser = 0;
    IImportedObjectList importedTableObjectList = PumpImbaseTablesHelper.GetImportedTableObjectList(this.plugin, table, cacheData, ownerUser, ImbaseIDHelper.ObjTypeIdImTab);
    if (table.TextID != 0)
    {
      IDataReader dataReader = this.GetDataReader($"SELECT * FROM IM_BLOBS WHERE F_KEY={table.TextID}");
      try
      {
        if (dataReader.Read())
          ComentTextAttribute.Create(importedTableObjectList, dataReader, this.plugin.Idw.AppManager);
      }
      finally
      {
        dataReader.Close();
      }
    }
    Dictionary<Guid, int> dictionary = new Dictionary<Guid, int>();
    DictionaryValue dictionaryValue1 = cacheData.GetValue(ImportingCategory.ImbaseGroupsAttributes, (object) table.Key);
    if (dictionaryValue1 != null && dictionaryValue1.Tag is ImbaseGroupAttributes tag && tag.Attributes != null)
    {
      bool flag = false;
      foreach (GroupAttribute attribute in tag.Attributes)
      {
        if (!dictionary.ContainsKey(attribute.AttrGuid))
          dictionary.Add(attribute.AttrGuid, attribute.Sort);
        else
          this.plugin.appManager.AddWarningMessage($"В таблице {table.TableName} присутствуют одинаковые поля {attribute.LongName}. Возможен сбой сортировки полей в таблице.");
        if (attribute.EnterMode == 9 && attribute.LongName.ToUpper().Equals("ШАБЛОН") && !string.IsNullOrEmpty(attribute.Data))
        {
          string s = attribute.Data.Substring(0, attribute.Data.IndexOf('|'));
          int result;
          if (!string.IsNullOrEmpty(s) && int.TryParse(s, out result))
          {
            DictionaryValue dictionaryValue2 = cacheData.GetValue(ImportingCategory.ImbaseBlobs, (object) result);
            if (dictionaryValue2 != null)
              importedTableObjectList.AddAttributeLink(ImbaseIDHelper.AttrIdTemplateRef, dictionaryValue2.NewObjectID, dictionaryValue2.Caption);
          }
        }
        if (attribute.EnterMode == 15 || attribute.EnterMode == 16 /*0x10*/)
          flag = true;
      }
      if (flag)
        importedTableObjectList.AddAttributeInt(ImbaseIDHelper.AttrIdNeedHandle, 1L);
    }
    AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Idw.GetUserSession(), importedTableObjectList);
    importedTableObjectList.Import();
    table.ObjectID = importedTableObjectList.Items[0].Object.Object_id;
    cacheData.AddValue(ImportingCategory.ImbaseTables, (object) table.TableName, table.ObjectID, table.Description);
    importedTableObjectList.Items.Clear();
  }
}

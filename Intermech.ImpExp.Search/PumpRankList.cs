// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpRankList
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskDescription("Инициализация данных для перекачки должностей", "Перекачка данных о должностях")]
[TaskType(PumperType.MetaData)]
internal class PumpRankList(SearchPlugin plugin) : PumpSearchClass(plugin)
{
  private const string tableNameRankList = "RANKLIST";
  private const string fieldNameRankId = "RANK_ID";
  private const string fieldNameRankName = "RANK_NAME";
  private const string fieldNameSignLabel = "SIGN_LABEL";
  private const string fieldNameRankCode = "RANK_CODE";

  protected override Guid GUID => new Guid("ADDE5F8C-0BE3-4f94-8B17-33263759023C");

  public override void Exam()
  {
    this.plugin.CheckIdAttribute(this.plugin.NameSearchIdRankList, this.plugin.GuidSearchIdRankList, FieldTypes.ftString);
    IAttributeTypeItem byGuid = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00141-306c-11d8-b4e9-00304f19f545"));
    IAttributePossibleValue[] possibleValues = byGuid.GetPossibleValues();
    List<string> stringList1 = possibleValues != null ? new List<string>(possibleValues.Length) : new List<string>(1);
    List<string> stringList2 = possibleValues != null ? new List<string>(possibleValues.Length) : new List<string>(1);
    if (possibleValues != null)
    {
      foreach (IAttributePossibleValue attributePossibleValue in possibleValues)
      {
        stringList1.Add(attributePossibleValue.Description);
        stringList2.Add(attributePossibleValue.ValueString);
      }
    }
    IDataReader defaultDataReader = this.GetDefaultDataReader("RANKLIST", "SIGN_LABEL, RANK_NAME");
    try
    {
      while (defaultDataReader.Read())
      {
        string description = defaultDataReader.IsDBNull(0) ? string.Empty : defaultDataReader.GetString(0).Trim();
        if (description != string.Empty)
        {
          if (!stringList1.Contains(description))
          {
            int num = stringList2.Count + 1;
            string str;
            for (str = Convert.ToString(num); stringList2.Contains(str); str = Convert.ToString(num))
              ++num;
            stringList2.Add(str);
            stringList1.Add(description);
            byGuid.AddPossibleValue(stringList2.Count, (object) str, description);
          }
        }
        else
          this.plugin.appManager.AddWarningMessage($"Для перекачки данных в IPS графа у должности '{defaultDataReader.GetString(1)}' должна быть заполнена");
      }
    }
    finally
    {
      defaultDataReader.Close();
    }
    this.ExamCheckPoint("Проверка данных успешно завершена", 100);
  }

  public override void Pump()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    service.GetHashCode();
    IImportingData cacheData = service.GetCache(ImportingCategory.RankList);
    try
    {
      this.PumpCheckPoint("Перекачка информации для предопределенных объектов", 0);
      IUserSession userSession = this.plugin.Idw.GetUserSession();
      DataTable possibleValues = userSession.GetAttributeType(new Guid("cad00141-306c-11d8-b4e9-00304f19f545")).GetPossibleValues();
      int id1 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00147-306c-11d8-b4e9-00304f19f545")).ID;
      int id2 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00070-306c-11d8-b4e9-00304f19f545")).ID;
      int id3 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).ID;
      int id4 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00148-306c-11d8-b4e9-00304f19f545")).ID;
      int id5 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid(this.plugin.GuidSearchIdRankList)).ID;
      this.PumpCheckPoint("Определение количества записей для закачки информации о должностях", 1);
      int tableRecordsCount = this.GetTableRecordsCount("RANKLIST");
      int index1 = 0;
      string format = "Закачка данных о должностях ({0} из {1})";
      IDataReader sequentialDataReader = this.GetSequentialDataReader("RANKLIST");
      int num = 0;
      List<string> stringList = new List<string>();
      try
      {
        Dictionary<char, string> rankCodeList = new Dictionary<char, string>(1);
        IImportedObjectList iolIm = this.plugin.Idw.CreateImportedObjectList();
        iolIm.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
        {
          int index2 = 0;
          foreach (KeyValuePair<char, string> keyValuePair in rankCodeList)
          {
            if (iolIm.Items[index2].Object.Object_id != 0L && iolIm.Items[index2].Object.Object_id != -1L)
              cacheData.AddValue(ImportingCategory.RankList, (object) keyValuePair.Key, iolIm.Items[index2].Object.Object_id, keyValuePair.Value);
            else
              this.plugin.appManager.AddWarningMessage($"Должность {keyValuePair.Key} не импортирована. См. серверный лог.");
            ++index2;
          }
          rankCodeList.Clear();
        });
        Dictionary<string, int> tableColumns = this.GetTableColumns(sequentialDataReader);
        int i1 = tableColumns["RANK_ID"];
        int i2 = tableColumns["RANK_NAME"];
        int i3 = tableColumns["SIGN_LABEL"];
        int i4 = tableColumns["RANK_CODE"];
        while (sequentialDataReader.Read())
        {
          ++index1;
          if (!sequentialDataReader.IsDBNull(i1))
            BasePumpHelper.ToInt32(sequentialDataReader[i1]);
          string caption = sequentialDataReader.IsDBNull(i2) ? string.Empty : sequentialDataReader.GetString(i2).Trim();
          string aValue = sequentialDataReader.IsDBNull(i3) ? string.Empty : sequentialDataReader.GetString(i3).Trim();
          string str1 = sequentialDataReader.IsDBNull(i4) ? string.Empty : sequentialDataReader.GetString(i4).Trim();
          this.PumpCheckPoint(string.Format(format, (object) index1, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index1, 2));
          if (string.IsNullOrEmpty(aValue))
          {
            this.plugin.appManager.AddWarningMessage($"Должность '{caption}' не импортирована, так как для перекачки данных в IPS графа у должности должна быть заполнена");
          }
          else
          {
            if (cacheData.GetNewKey(ImportingCategory.RankList, (object) str1[0]) == 0L)
            {
              rankCodeList.Add(str1[0], aValue);
              iolIm.AddObject(id1, 0, caption);
              iolIm.AddAttributeStr(id3, caption);
              iolIm.AddAttributeStr(id5, str1);
              string empty = string.Empty;
              DataRow[] dataRowArray = possibleValues.Select("F_DESCRIPTION = " + DataSetProcessor.QString(aValue));
              if (dataRowArray != null && dataRowArray.Length != 0)
              {
                Graphs4Type graphs4Type = new Graphs4Type((Dictionary<string, string>) null);
                graphs4Type.Add(id2, Convert.ToString(dataRowArray[0]["F_STRING_VALUE"]));
                int fileSize = 0;
                string str2 = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.tmp");
                stringList.Add(str2);
                FileStream destStream = File.OpenWrite(str2);
                try
                {
                  graphs4Type.Save((Stream) destStream, userSession);
                  fileSize = Convert.ToInt32(destStream.Length);
                }
                finally
                {
                  destStream.Flush();
                  destStream.Close();
                }
                iolIm.AddAttributeBlob(id4, str2, (long) fileSize, $"Настройка подписей для должности \"{caption}\"", ArcMethods.NotPacked);
              }
              AttributesHelper.AddObligatoryObjectAttributes(userSession, iolIm);
            }
            ++num;
          }
        }
        iolIm.Import();
      }
      finally
      {
        sequentialDataReader.Close();
        foreach (string path in stringList)
          File.Delete(path);
      }
      this.plugin.appManager.AddInfoMessage("Добавлено должностей: " + num.ToString());
      this.PumpCheckPoint("Перекачка данных успешно завершена", 100);
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.RankList);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpUserGroups
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskDescription("Инициализация данных для перекачки групп пользователей", "Перекачка данных о группах пользователей")]
[TaskType(PumperType.MetaData)]
public class PumpUserGroups(SearchPlugin plugin) : PumpSearchClass(plugin)
{
  private const string tableNameGroups = "GROUPS";
  private const string fieldNameGroupId = "GROUP_ID";
  private const string fieldNameGroupName = "NAME_GROUP";
  private const string fieldNameS4Resource = "S4RESOURCE";
  private const string fieldNameUserId = "USER_ID";
  private const string fieldNameNotes = "NOTES";
  private const string fieldNameChiefId = "CHIEF_ID";
  private const string fieldNameCalendarId = "CALENDAR_ID";
  private const string fieldNameDeloId = "DELO_ID";
  private const string tableNameGroupsTree = "GRPINGRP";
  private const string fieldNameGroupIdLink = "GROUP_ID";
  private const string fieldNameInGroupId = "INGROUP_ID";

  public override void Exam()
  {
    this.plugin.CheckIdAttribute(this.plugin.NameSearchIdUserGroup, this.plugin.GuidSearchIdUserGroup, FieldTypes.ftInteger);
    this.ExamCheckPoint("Проверка данных успешно завершена", 100);
  }

  protected override Guid GUID => new Guid("B8D9AF83-4831-4f0e-BB45-1346A1A2A6AB");

  public override void Pump()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cacheData = service.GetCache(ImportingCategory.UsersToGroups, ImportingCategory.UserGroups);
    Dictionary<int, long> dictionary = (Dictionary<int, long>) null;
    try
    {
      this.PumpCheckPoint("Перекачка информации для предопределенных объектов", 0);
      long newKey1 = this.plugin.Idw.AddSysObject(new Guid("cad00059-306c-11d8-b4e9-00304f19f545"));
      long newKey2 = this.plugin.Idw.AddSysObject(new Guid("cad00017-306c-11d8-b4e9-00304f19f545"));
      int objectTypeId = MetaDataHelper.GetObjectTypeID("cad00003-306c-11d8-b4e9-00304f19f545");
      int attributeTypeId1 = MetaDataHelper.GetAttributeTypeID("cad00020-306c-11d8-b4e9-00304f19f545");
      int attributeTypeId2 = MetaDataHelper.GetAttributeTypeID("cad0001c-306c-11d8-b4e9-00304f19f545");
      int attributeByTypeNameId = MetaDataHelper.GetAttributeByTypeNameID(this.plugin.NameSearchIdUserGroup);
      int relationTypeId1 = MetaDataHelper.GetRelationTypeID("cad00022-306c-11d8-b4e9-00304f19f545");
      int relationTypeId2 = MetaDataHelper.GetRelationTypeID("cad00022-306c-11d8-b4e9-00304f19f545");
      this.PumpCheckPoint("Определение количества записей для закачки информации о группах пользователей", 1);
      int tableRecordsCount1 = this.GetTableRecordsCount("GROUPS");
      int index1 = 0;
      string empty = string.Empty;
      dictionary = new Dictionary<int, long>(tableRecordsCount1);
      IDataReader sequentialDataReader1 = this.GetSequentialDataReader("GROUPS");
      IUserSession userSession = this.plugin.Idw.GetUserSession();
      int num1 = 0;
      int num2 = 0;
      IImportedObjectList iolIm = this.plugin.Idw.CreateImportedObjectList();
      List<Tuple<int, string>> groupIDs = new List<Tuple<int, string>>();
      try
      {
        Dictionary<string, int> tableColumns = this.GetTableColumns(sequentialDataReader1);
        int i1 = tableColumns["GROUP_ID"];
        int i2 = tableColumns["NAME_GROUP"];
        int num3 = tableColumns["S4RESOURCE"];
        int i3 = tableColumns["USER_ID"];
        int i4 = tableColumns["NOTES"];
        int i5 = tableColumns["CHIEF_ID"];
        int i6 = tableColumns["CALENDAR_ID"];
        int i7 = tableColumns["DELO_ID"];
        string format = "Закачка данных о группах пользователей ({0} из {1})";
        iolIm.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
        {
          int index2 = 0;
          foreach (Tuple<int, string> tuple in groupIDs)
          {
            if (iolIm.Items[index2].Object.Object_id != 0L && iolIm.Items[index2].Object.Object_id != -1L)
              cacheData.AddValue(ImportingCategory.UserGroups, (object) tuple.Item1, iolIm.Items[index2].Object.Object_id, tuple.Item2);
            else
              this.plugin.appManager.AddWarningMessage($"Группа пользователей {tuple.Item1} не импортирована. См. серверный лог.");
            ++index2;
          }
          groupIDs.Clear();
        });
        while (sequentialDataReader1.Read())
        {
          ++index1;
          this.PumpCheckPoint(string.Format(format, (object) index1, (object) tableRecordsCount1), this.CalculatePercent(tableRecordsCount1, index1, 2, 50));
          int int32_1 = sequentialDataReader1.IsDBNull(i1) ? 0 : BasePumpHelper.ToInt32(sequentialDataReader1[i1]);
          string str = sequentialDataReader1.IsDBNull(i2) ? string.Empty : sequentialDataReader1.GetString(i2).Trim();
          int int32_2 = sequentialDataReader1.IsDBNull(i3) ? 0 : BasePumpHelper.ToInt32(sequentialDataReader1[i3]);
          string attrVal = sequentialDataReader1.IsDBNull(i4) ? string.Empty : sequentialDataReader1.GetString(i4).Trim();
          if (!sequentialDataReader1.IsDBNull(i5))
            BasePumpHelper.ToInt32(sequentialDataReader1[i5]);
          if (!sequentialDataReader1.IsDBNull(i6))
            BasePumpHelper.ToInt32(sequentialDataReader1[i6]);
          if (!sequentialDataReader1.IsDBNull(i7))
            BasePumpHelper.ToInt32(sequentialDataReader1[i7]);
          try
          {
            if (int32_2 != 0)
            {
              dictionary.Add(int32_1, this.plugin.Imdi.ImportedUsers.GetNewKey(int32_2));
              ++num2;
            }
            else if (cacheData.GetNewKey(ImportingCategory.UserGroups, (object) int32_1) == 0L)
            {
              string upper = str.ToUpper();
              if (int32_1 == -1 || upper.Equals("ВЛАДЕЛЕЦ_ОБЪЕКТА"))
                cacheData.AddValue(ImportingCategory.UserGroups, (object) int32_1, newKey1, str);
              else if (int32_1 == 999999999 || upper.Equals("ВСЕ_ПОЛЬЗОВАТЕЛИ"))
              {
                cacheData.AddValue(ImportingCategory.UserGroups, (object) int32_1, newKey2, str);
              }
              else
              {
                iolIm.AddObject(objectTypeId, 0, str);
                iolIm.AddAttribute(attributeTypeId1, AttrValueType.stringVal, (object) str, 0);
                iolIm.AddAttribute(attributeTypeId2, AttrValueType.stringVal, (object) attrVal, 0);
                iolIm.AddAttribute(attributeByTypeNameId, AttrValueType.integerVal, (object) int32_1, 0);
                AttributesHelper.AddObligatoryObjectAttributes(userSession, iolIm);
                groupIDs.Add(new Tuple<int, string>(int32_1, str));
              }
              ++num1;
            }
          }
          catch (Exception ex)
          {
            this.plugin.appManager.AddWarningMessage($"Ошибка при создании группы пользователей {str}: {ex.Message}");
          }
        }
        iolIm.Import();
      }
      finally
      {
        sequentialDataReader1.Close();
      }
      this.plugin.appManager.AddInfoMessage("Добавлено групп пользователей: " + num1.ToString());
      this.plugin.appManager.AddInfoMessage("Найдено скрытых групп пользователей: " + num2.ToString());
      this.PumpCheckPoint("Определение количества записей для закачки информации о связях", 51);
      int tableRecordsCount2 = this.GetTableRecordsCount("GRPINGRP");
      IDataReader sequentialDataReader2 = this.GetSequentialDataReader("GRPINGRP");
      int num4 = 0;
      try
      {
        Dictionary<string, int> tableColumns = this.GetTableColumns(sequentialDataReader2);
        int i8 = tableColumns["GROUP_ID"];
        int i9 = tableColumns["INGROUP_ID"];
        string format = "Закачка данных о связях между группами пользователей и пользователями ({0} из {1})";
        int index3 = 0;
        List<long> codes = new List<long>((ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.PacketSize);
        IImportedRelationList irl = this.plugin.Idw.CreateImportedRelationList();
        irl.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
        {
          for (int index4 = 0; index4 < irl.Items.Count; ++index4)
          {
            if (irl.Items[index4].Relation.PrjLinkId != -1L && irl.Items[index4].Relation.PrjLinkId != 0L)
              cacheData.AddValue(ImportingCategory.UsersToGroups, (object) codes[index4], irl.Items[index4].Relation.PrjLinkId);
            else
              this.plugin.appManager.AddWarningMessage($"Связь между группой пользователей {irl.Items[index4].Relation.ProjId} и пользователем {irl.Items[index4].Relation.PartId} не импортирован");
          }
          codes.Clear();
        });
        while (sequentialDataReader2.Read())
        {
          ++index3;
          this.PumpCheckPoint(string.Format(format, (object) index3, (object) tableRecordsCount2), this.CalculatePercent(tableRecordsCount2, index3, 51));
          int int32_3 = sequentialDataReader2.IsDBNull(i8) ? 0 : BasePumpHelper.ToInt32(sequentialDataReader2[i8]);
          int int32_4 = sequentialDataReader2.IsDBNull(i9) ? 0 : BasePumpHelper.ToInt32(sequentialDataReader2[i9]);
          if (int32_3 != 0 && int32_4 != 0)
          {
            long oldKey = Convert.ToInt64(int32_3) << 32 /*0x20*/ | (long) int32_4;
            if (cacheData.GetNewKey(ImportingCategory.UsersToGroups, (object) oldKey) == 0L)
            {
              try
              {
                long newKey3 = cacheData.GetNewKey(ImportingCategory.UserGroups, (object) int32_4);
                int relType;
                long newKey4;
                if (dictionary.ContainsKey(int32_3))
                {
                  relType = relationTypeId1;
                  newKey4 = dictionary[int32_3];
                }
                else
                {
                  relType = relationTypeId2;
                  newKey4 = cacheData.GetNewKey(ImportingCategory.UserGroups, (object) int32_3);
                }
                if (newKey4 != 0L)
                {
                  irl.AddRelation(newKey3, newKey4, relType);
                  AttributesHelper.AddObligatoryRelationAttributes(this.plugin.Idw, irl);
                  codes.Add(oldKey);
                }
              }
              catch (Exception ex)
              {
                this.plugin.appManager.AddWarningMessage($"Ошибка при добавлении связей между группами пользователей {int32_3} и {int32_4}: {ex.Message}");
              }
            }
          }
        }
        irl.Import();
      }
      finally
      {
        sequentialDataReader2.Close();
      }
      this.plugin.appManager.AddInfoMessage("Создано связей между группами: " + num4.ToString());
      this.PumpCheckPoint("Перекачка данных успешно завершена", 100);
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.UsersToGroups, ImportingCategory.UserGroups);
      dictionary?.Clear();
    }
  }
}

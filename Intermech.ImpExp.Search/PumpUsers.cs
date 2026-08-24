// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpUsers
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.SelectionService;
using Intermech.Kernel.Search;
using Intermech.Protection;
using ntermech.ImpExp.Interface.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskDescription("Инициализация данных для перекачки пользователей", "Перекачка данных о пользователях")]
[TaskType(PumperType.MetaData)]
public class PumpUsers(SearchPlugin plugin) : PumpSearchClass(plugin)
{
  private const string tableNameUsers = "USERS";
  private const string fieldNameUserId = "USER_ID";
  private const string fieldNameLoginName = "LOGINNAME";
  private const string fieldNameFullName = "FULLNAME";
  private const string fieldNamePassword = "PASSW";
  private const string fieldNameUserDir = "USER_DIR";
  private const string fieldNameMailDir = "MAIL_DIR";
  private const string fieldNameRankString = "RANKSTRING";
  private const string fieldNameCfgData = "CFG_DATA";
  private const string fieldNameUserStatus = "USERSTATUS";
  private const string fieldNamePasswControl = "PSWCONTROL";
  private const string fieldNameSignName = "SIGNNAME";
  private const string fieldNameIsBlocked = "ISBLOCKED";
  private const string fieldNameAccessLevel = "ACCESS_LEVEL";
  private const string tableNameUsersInfo = "USERS_INFO";
  private const string fieldNameUserIdInfo = "USER_ID";
  private const string fieldNameFirstName = "FIRSTNAME";
  private const string fieldNameLastName = "LASTNAME";
  private const string fieldNameFio = "FIO";
  private const string fieldNameWorkPhone = "WORKPHONE";
  private const string fieldNameHomePhone = "HOMEPHONE";
  private const string fieldNameRoomNumber = "ROOMNUMBER";
  private const string fieldNameAddress = "ADDRESS";
  private const string fieldNameEmail = "EMAIL";
  private const string fieldNameNote = "NOTE";
  private const string fieldNameGuid = "F_GUID";

  protected override Guid GUID => new Guid("626800C3-F1CD-4403-934C-ADE56ABF39AF");

  public override void Exam()
  {
    this.plugin.CheckIdAttribute(this.plugin.NameSearchIdUser, this.plugin.GuidSearchIdUser, FieldTypes.ftInteger);
    this.ExamCheckPoint("Проверка данных успешно завершена", 100);
  }

  public override void Pump()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cacheData = service.GetCache(ImportingCategory.RankList, ImportingCategory.Workspaces, ImportingCategory.CheckOutSelections, ImportingCategory.TrashSelections, ImportingCategory.WorkspaceToCheckOut, ImportingCategory.WorkspaceToMyTrash);
    Dictionary<int, UserInfo> dictionary = (Dictionary<int, UserInfo>) null;
    try
    {
      this.PumpCheckPoint("Добавление информации для предопределенных объектов в общий кэш", 0);
      long num1 = this.plugin.Idw.AddSysObject(new Guid("cad00016-306c-11d8-b4e9-00304f19f545"));
      long num2 = this.plugin.Idw.AddSysObject(new Guid("cad0000d-306c-11d8-b4e9-00304f19f545"));
      int id1 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00002-306c-11d8-b4e9-00304f19f545")).ID;
      int id2 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00018-306c-11d8-b4e9-00304f19f545")).ID;
      int id3 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00816-306c-11d8-b4e9-00304f19f545")).ID;
      int id4 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0001d-306c-11d8-b4e9-00304f19f545")).ID;
      int id5 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00024-306c-11d8-b4e9-00304f19f545")).ID;
      int id6 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0001b-306c-11d8-b4e9-00304f19f545")).ID;
      int id7 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0001a-306c-11d8-b4e9-00304f19f545")).ID;
      int id8 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0001c-306c-11d8-b4e9-00304f19f545")).ID;
      int id9 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00021-306c-11d8-b4e9-00304f19f545")).ID;
      int id10 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad002dc-306c-11d8-b4e9-00304f19f545")).ID;
      int id11 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad002dd-306c-11d8-b4e9-00304f19f545")).ID;
      int id12 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad002da-306c-11d8-b4e9-00304f19f545")).ID;
      int id13 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00019-306c-11d8-b4e9-00304f19f545")).ID;
      int id14 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad002db-306c-11d8-b4e9-00304f19f545")).ID;
      int id15 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad002de-306c-11d8-b4e9-00304f19f545")).ID;
      int id16 = this.plugin.Imdi.AttributeTypes.GetByName(this.plugin.NameSearchIdUser).ID;
      int id17 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00142-306c-11d8-b4e9-00304f19f545")).ID;
      int id18 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0036e-306c-11d8-b4e9-00304f19f545")).ID;
      int id19 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cadd99fb-306c-11d8-b4e9-00304f19f545")).ID;
      this.PumpCheckPoint("Определение количества записей для закачки дополнительной информации ", 1);
      int tableRecordsCount1 = this.GetTableRecordsCount("USERS_INFO");
      this.SetCountPumpRecords(tableRecordsCount1);
      int index1 = 0;
      string empty = string.Empty;
      dictionary = new Dictionary<int, UserInfo>(tableRecordsCount1);
      IDataReader sequentialDataReader1 = this.GetSequentialDataReader("USERS_INFO");
      try
      {
        Dictionary<string, int> tableColumns = this.GetTableColumns(sequentialDataReader1);
        int i1 = tableColumns["USER_ID"];
        int i2 = tableColumns["FIRSTNAME"];
        int i3 = tableColumns["LASTNAME"];
        int i4 = tableColumns["FIO"];
        int i5 = tableColumns["WORKPHONE"];
        int i6 = tableColumns["HOMEPHONE"];
        int i7 = tableColumns["ROOMNUMBER"];
        int i8 = tableColumns["ADDRESS"];
        int i9 = tableColumns["EMAIL"];
        int i10 = tableColumns["NOTE"];
        int i11 = tableColumns.ContainsKey("F_GUID") ? tableColumns["F_GUID"] : -1;
        string format = "Закачка дополнительной информации о пользователях ({0} из {1})";
        while (sequentialDataReader1.Read())
        {
          ++index1;
          this.PumpCheckPoint(string.Format(format, (object) index1, (object) tableRecordsCount1), this.CalculatePercent(tableRecordsCount1, index1, 2, 25));
          int int32 = sequentialDataReader1.IsDBNull(i1) ? 0 : BasePumpHelper.ToInt32(sequentialDataReader1[i1]);
          string firstName = sequentialDataReader1.IsDBNull(i2) ? string.Empty : sequentialDataReader1.GetString(i2).Trim();
          string lastName = sequentialDataReader1.IsDBNull(i3) ? string.Empty : sequentialDataReader1.GetString(i3).Trim();
          string fio = sequentialDataReader1.IsDBNull(i4) ? string.Empty : sequentialDataReader1.GetString(i4).Trim();
          string workPhone = sequentialDataReader1.IsDBNull(i5) ? string.Empty : sequentialDataReader1.GetString(i5).Trim();
          string homePhone = sequentialDataReader1.IsDBNull(i6) ? string.Empty : sequentialDataReader1.GetString(i6).Trim();
          string roomNumber = sequentialDataReader1.IsDBNull(i7) ? string.Empty : sequentialDataReader1.GetString(i7).Trim();
          string address = sequentialDataReader1.IsDBNull(i8) ? string.Empty : sequentialDataReader1.GetString(i8).Trim();
          string email = sequentialDataReader1.IsDBNull(i9) ? string.Empty : sequentialDataReader1.GetString(i9).Trim();
          string note = sequentialDataReader1.IsDBNull(i10) ? string.Empty : sequentialDataReader1.GetString(i10).Trim();
          string guid = i11 == -1 || sequentialDataReader1.IsDBNull(i11) ? string.Empty : sequentialDataReader1.GetString(i11).Trim();
          dictionary.Add(int32, new UserInfo(firstName, lastName, fio, workPhone, homePhone, roomNumber, address, email, note, guid));
        }
      }
      finally
      {
        sequentialDataReader1.Close();
      }
      this.PumpCheckPoint("Определение количества записей для закачки информации ", 26);
      int tableRecordsCount2 = this.GetTableRecordsCount("USERS");
      IDataReader sequentialDataReader2 = this.GetSequentialDataReader("USERS");
      int num3 = 0;
      IUserSession userSession = this.plugin.Idw.GetUserSession();
      IDBLifecycleStep lifecycleStep = userSession.GetLifecycleStep(new Guid("cadd9504-306c-11d8-b4e9-00304f19f545"));
      int lcStep = lifecycleStep.LCStep;
      int levelId = lifecycleStep.LevelID;
      char cryptMethod = Convert.ToChar(userSession.Configurations.ReadString("KERNEL", "SECURITY", "CRYPTO_METHOD", Convert.ToString(CryptHelper.SHA1Crypt), DBConfigMode.GlobalOnly));
      try
      {
        Dictionary<string, int> tableColumns = this.GetTableColumns(sequentialDataReader2);
        int i12 = tableColumns["USER_ID"];
        int i13 = tableColumns["LOGINNAME"];
        int i14 = tableColumns["FULLNAME"];
        int i15 = tableColumns["PASSW"];
        int num4 = tableColumns["USER_DIR"];
        int num5 = tableColumns["MAIL_DIR"];
        int i16 = tableColumns["RANKSTRING"];
        int num6 = tableColumns["CFG_DATA"];
        int i17 = tableColumns["USERSTATUS"];
        int i18 = tableColumns["SIGNNAME"];
        int i19 = tableColumns["ISBLOCKED"];
        int i20 = tableColumns.ContainsKey("ACCESS_LEVEL") ? tableColumns["ACCESS_LEVEL"] : -1;
        string format = "Закачка информации о пользователях ({0} из {1})";
        int index2 = 0;
        IImportedObjectList iolIm = this.plugin.Idw.CreateImportedObjectList();
        List<PumpUsers.QuickUserInfo> importedUsers = new List<PumpUsers.QuickUserInfo>(SearchHelper.PacketSize);
        iolIm.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
        {
          for (int index3 = 0; index3 < iolIm.Items.Count; ++index3)
          {
            if (iolIm.Items[index3].Object.Object_id != 0L)
              this.plugin.Imdi.ImportedUsers.AddValue(importedUsers[index3].UserID, iolIm.Items[index3].Object.Object_id, importedUsers[index3].UserName, (Guid) iolIm.Items[index3].Object.ObjectGuid);
            else
              this.plugin.appManager.AddWarningMessage($"Пользователь {importedUsers[index3].UserID} не импортирован. См. серверный лог.");
          }
          importedUsers.Clear();
        });
        while (sequentialDataReader2.Read())
        {
          ++index2;
          this.PumpCheckPoint(string.Format(format, (object) index2, (object) tableRecordsCount2), this.CalculatePercent(tableRecordsCount2, index2, 27, 60));
          int int32_1 = sequentialDataReader2.IsDBNull(i12) ? 0 : BasePumpHelper.ToInt32(sequentialDataReader2[i12]);
          string attrVal1 = sequentialDataReader2.IsDBNull(i13) ? string.Empty : sequentialDataReader2.GetString(i13).Trim();
          string str1 = sequentialDataReader2.IsDBNull(i14) ? string.Empty : sequentialDataReader2.GetString(i14).Trim();
          string codestr = sequentialDataReader2.IsDBNull(i15) ? string.Empty : sequentialDataReader2.GetString(i15).Trim();
          string str2 = sequentialDataReader2.IsDBNull(i16) ? string.Empty : sequentialDataReader2.GetString(i16).Trim();
          string str3 = sequentialDataReader2.IsDBNull(i17) ? "" : sequentialDataReader2.GetString(i17).Trim();
          string attrVal2 = sequentialDataReader2.IsDBNull(i18) ? string.Empty : sequentialDataReader2.GetString(i18).Trim();
          int int32_2 = sequentialDataReader2.IsDBNull(i19) ? 0 : BasePumpHelper.ToInt32(sequentialDataReader2[i19]);
          int num7 = i20 == -1 || sequentialDataReader2.IsDBNull(i20) ? -1 : BasePumpHelper.ToInt32(sequentialDataReader2[i20]);
          string upper1 = attrVal1.ToUpper();
          string upper2 = str1.ToUpper();
          if (this.plugin.Imdi.ImportedUsers.GetNewKey(int32_1) == 0L)
          {
            long num8 = 0;
            if (int32_1 == -1 || upper1 == "SYSDBA" || upper2 == "СИСТЕМНЫЙ АДМИНИСТРАТОР")
            {
              long objectID = num1;
              this.plugin.Imdi.ImportedUsers.AddValue(int32_1, objectID, str1, new Guid("cad00016-306c-11d8-b4e9-00304f19f545"));
            }
            else if (int32_1 == -2 || upper1 == "SYSTEM$USER" || upper2 == "СИСТЕМА")
            {
              long objectID = num2;
              this.plugin.Imdi.ImportedUsers.AddValue(int32_1, objectID, str1, new Guid("cad0000d-306c-11d8-b4e9-00304f19f545"));
            }
            else
            {
              ObjectRecord objectRecord = iolIm.AddObject(id1, 0, str1);
              if (str3.Equals("D"))
              {
                objectRecord.Lc_step = lcStep;
                objectRecord.LevelId = levelId;
              }
              if (num7 != -1)
                iolIm.AddAttributeInt(id3, (long) num7);
              iolIm.AddAttribute(id16, AttrValueType.integerVal, (object) int32_1, 0);
              if (num8 != num1 && num8 != num2)
              {
                iolIm.AddAttribute(id2, AttrValueType.stringVal, (object) attrVal1, 0);
                string str4 = string.Empty;
                try
                {
                  str4 = this.DecodeIt(codestr);
                }
                catch
                {
                  this.plugin.appManager.AddWarningMessage($"Невозможно восстановить пароль для пользователя {attrVal1}");
                }
                iolIm.AddAttribute(id13, AttrValueType.stringVal, str4 != string.Empty ? (object) CryptHelper.CryptPassword(str4.ToLower(), cryptMethod) : (object) string.Empty, 0);
                iolIm.AddAttribute(id4, AttrValueType.stringVal, (object) str1, 0);
                iolIm.AddAttribute(id18, AttrValueType.stringVal, (object) attrVal2, 0);
                if (int32_2 == 1)
                  iolIm.AddAttributeInt(id19, (long) int32_2);
              }
              if (dictionary.ContainsKey(int32_1))
              {
                UserInfo userInfo = dictionary[int32_1];
                if (userInfo.Guid == Guid.Empty)
                {
                  userInfo.Guid = this.plugin.Imdi.NewPumpGuid();
                  IDbCommand command = this.plugin.idb2.DbConnection.CreateCommand();
                  command.CommandText = $"UPDATE USERS_INFO SET F_GUID='{userInfo.Guid.ToString("B").ToUpper()}' WHERE USER_ID={int32_1}";
                  command.ExecuteNonQuery();
                }
                objectRecord.ObjectGuid = (object) userInfo.Guid;
                objectRecord.IdGuid = (object) userInfo.Guid;
                iolIm.AddAttribute(id5, AttrValueType.stringVal, (object) userInfo.FirstName, 0);
                iolIm.AddAttribute(id6, AttrValueType.stringVal, (object) userInfo.LastName, 0);
                iolIm.AddAttribute(id7, AttrValueType.stringVal, (object) userInfo.Fio, 0);
                iolIm.AddAttribute(id8, AttrValueType.stringVal, (object) userInfo.Note, 0);
                iolIm.AddAttribute(id9, AttrValueType.stringVal, (object) userInfo.Note, 0);
                iolIm.AddAttribute(id10, AttrValueType.stringVal, (object) userInfo.Address, 0);
                iolIm.AddAttribute(id11, AttrValueType.stringVal, (object) userInfo.HomePhone, 0);
                iolIm.AddAttribute(id12, AttrValueType.stringVal, (object) userInfo.WorkPhone, 0);
                iolIm.AddAttribute(id14, AttrValueType.stringVal, (object) userInfo.RoomNumber, 0);
                iolIm.AddAttribute(id15, AttrValueType.stringVal, (object) userInfo.Email, 0);
              }
              try
              {
                List<long> longList = new List<long>(str2.Length);
                int inListID = 0;
                foreach (char oldKey in str2)
                {
                  if (oldKey != ' ')
                  {
                    DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.RankList, (object) oldKey);
                    if (dictionaryValue != null)
                    {
                      iolIm.AddAttributeLink(id17, dictionaryValue.NewObjectID, dictionaryValue.Caption, inListID);
                      ++inListID;
                    }
                  }
                }
              }
              catch (Exception ex)
              {
                this.plugin.appManager.AddWarningMessage($"Ошибка при добавлении пользователю должности {str2}: {ex.Message}");
              }
              AttributesHelper.AddObligatoryObjectAttributes(userSession, iolIm);
              importedUsers.Add(new PumpUsers.QuickUserInfo(int32_1, str1));
            }
          }
          ++num3;
        }
        iolIm.Import();
      }
      finally
      {
        sequentialDataReader2.Close();
      }
      Dictionary<object, DictionaryValue> category = this.plugin.Imdi.ImportedUsers.Category;
      if (category != null && category.Count > 0)
      {
        int index4 = 0;
        int count = category.Count;
        string format1 = "Закачка рабочего стола для пользователя ({0} из {1})";
        IImportedObjectList iolWs = this.plugin.Idw.CreateImportedObjectList();
        List<AdditionalUserInfo> importingList = new List<AdditionalUserInfo>(SearchHelper.PacketSize);
        List<string> stringList = new List<string>(SearchHelper.PacketSize);
        try
        {
          iolWs.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
          {
            for (int index5 = 0; index5 < iolWs.Items.Count; ++index5)
            {
              if (iolWs.Items[index5].Object.Object_id != 0L)
                cacheData.AddValue(importingList[index5].Category, (object) importingList[index5].UserID, iolWs.Items[index5].Object.Object_id);
              else
                this.plugin.appManager.AddWarningMessage($"Рабочий стол для пользователя {importingList[index5].UserID} не импортирован. См. серверный лог.");
            }
            importingList.Clear();
          });
          int workspaceTypeId = userSession.IdentHelper.WorkspaceTypeID;
          int objectType1 = userSession.GetObjectType(new Guid("cad0005d-306c-11d8-b4e9-00304f19f545")).ObjectType;
          int objectType2 = userSession.GetObjectType(new Guid("cad00123-306c-11d8-b4e9-00304f19f545")).ObjectType;
          int nameId = userSession.IdentHelper.NameID;
          int attributeId1 = userSession.GetAttributeType(new Guid("cad00155-306c-11d8-b4e9-00304f19f545")).AttributeID;
          int attributeId2 = userSession.GetAttributeType(new Guid("cad00158-306c-11d8-b4e9-00304f19f545")).AttributeID;
          int attributeId3 = userSession.GetAttributeType(new Guid("cad00345-306c-11d8-b4e9-00304f19f545")).AttributeID;
          int attributeId4 = userSession.GetAttributeType(new Guid("cad0069b-306c-11d8-b4e9-00304f19f545")).AttributeID;
          int deletedId = userSession.IdentHelper.DeletedID;
          string caption1 = "Рабочий стол";
          string caption2 = "Взятые на изменение объекты";
          string caption3 = "Моя корзина";
          string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
          string format2 = Path.Combine(folderPath, "selectionTrash4User{0}.tmp");
          string format3 = Path.Combine(folderPath, "selectionCheckOut4User{0}.tmp");
          string fileNote = "Selection Structure";
          SelectionWrapper selectionWrapper = new SelectionWrapper(false);
          foreach (KeyValuePair<object, DictionaryValue> keyValuePair in category)
          {
            ++index4;
            this.PumpCheckPoint(string.Format(format1, (object) index4, (object) count), this.CalculatePercent(count, index4, 61, 85));
            if (keyValuePair.Value.NewObjectID != num2 && keyValuePair.Value.NewObjectID != num1)
            {
              if (cacheData.GetNewKey(ImportingCategory.Workspaces, (object) keyValuePair.Value.NewObjectID) == 0L)
              {
                iolWs.AddObject(workspaceTypeId, Convert.ToInt32(keyValuePair.Key), caption1);
                iolWs.AddAttributeStr(nameId, caption1);
                AttributesHelper.AddObligatoryObjectAttributes(userSession, iolWs);
                importingList.Add(new AdditionalUserInfo(ImportingCategory.Workspaces, keyValuePair.Value.NewObjectID));
              }
              if (cacheData.GetNewKey(ImportingCategory.CheckOutSelections, (object) keyValuePair.Value.NewObjectID) == 0L)
              {
                iolWs.AddObject(objectType2, Convert.ToInt32(keyValuePair.Key), caption2);
                iolWs.AddAttributeStr(nameId, caption2);
                iolWs.AddAttributeInt(attributeId1, 0L);
                iolWs.AddAttributeInt(attributeId2, 0L);
                iolWs.AddAttributeInt(attributeId3, 1L);
                ConditionStructure[] conditionStructures = new ConditionStructure[1]
                {
                  new ConditionStructure(-6, RelationalOperators.Equal, (object) keyValuePair.Value.NewObjectID, (object) null, LogicalOperators.AND, 0, true)
                };
                XmlDocument xml = selectionWrapper.SaveToXML(userSession, conditionStructures);
                string str = string.Format(format3, (object) keyValuePair.Value.NewObjectID);
                string filename = str;
                xml.Save(filename);
                FileInfo fileInfo = new FileInfo(str);
                stringList.Add(str);
                iolWs.AddAttributeBlob(attributeId4, str, fileInfo.Length, fileNote, ArcMethods.NotPacked);
                AttributesHelper.AddObligatoryObjectAttributes(userSession, iolWs);
                importingList.Add(new AdditionalUserInfo(ImportingCategory.CheckOutSelections, keyValuePair.Value.NewObjectID));
              }
              if (cacheData.GetNewKey(ImportingCategory.TrashSelections, (object) keyValuePair.Value.NewObjectID) == 0L)
              {
                iolWs.AddObject(objectType2, Convert.ToInt32(keyValuePair.Key), caption3);
                iolWs.AddAttributeStr(nameId, caption3);
                iolWs.AddAttributeInt(attributeId1, 0L);
                iolWs.AddAttributeInt(attributeId2, 0L);
                iolWs.AddAttributeInt(attributeId3, 2L);
                ConditionStructure[] conditionStructures = new ConditionStructure[2]
                {
                  new ConditionStructure(-8, RelationalOperators.Equal, (object) keyValuePair.Value.NewObjectID, (object) null, LogicalOperators.AND, 0, true),
                  new ConditionStructure(-9, RelationalOperators.Equal, (object) deletedId, (object) null, LogicalOperators.NONE, 0, true)
                };
                XmlDocument xml = selectionWrapper.SaveToXML(userSession, conditionStructures);
                string str = string.Format(format2, (object) keyValuePair.Value.NewObjectID);
                string filename = str;
                xml.Save(filename);
                FileInfo fileInfo = new FileInfo(str);
                stringList.Add(str);
                iolWs.AddAttributeBlob(attributeId4, str, fileInfo.Length, fileNote, ArcMethods.NotPacked);
                AttributesHelper.AddObligatoryObjectAttributes(userSession, iolWs);
                importingList.Add(new AdditionalUserInfo(ImportingCategory.TrashSelections, keyValuePair.Value.NewObjectID));
              }
            }
          }
          iolWs.Import();
        }
        finally
        {
          for (int index6 = 0; index6 < stringList.Count; ++index6)
            File.Delete(stringList[index6]);
          stringList.Clear();
        }
        int relationType = userSession.GetRelationType(new Guid("cad0005e-306c-11d8-b4e9-00304f19f545")).RelationType;
        importingList.Clear();
        IImportedRelationList irlWs = this.plugin.Idw.CreateImportedRelationList();
        irlWs.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
        {
          for (int index7 = 0; index7 < irlWs.Items.Count; ++index7)
          {
            if (irlWs.Items[index7].Relation.PrjLinkId != 0L)
              cacheData.AddValue(importingList[index7].Category, (object) importingList[index7].UserID, irlWs.Items[index7].Relation.PrjLinkId);
            else
              this.plugin.appManager.AddWarningMessage($"Связь между рабочим столом {importingList[index7].ProjectID} и {$"{(importingList[index7].Category == ImportingCategory.WorkspaceToCheckOut ? (object) "выборкой \"Взятые на изменение объекты\"" : (object) "выборкой \"Моя корзина\"")} {importingList[index7].PartID}"} для пользователя {importingList[index7].UserID} не импортирована. См. серверный лог.");
          }
          importingList.Clear();
        });
        int index8 = 0;
        string format4 = "Закачка связей рабочего стола для пользователя ({0} из {1})";
        foreach (KeyValuePair<object, DictionaryValue> keyValuePair in category)
        {
          ++index8;
          this.PumpCheckPoint(string.Format(format4, (object) index8, (object) count), this.CalculatePercent(count, index8, 86, 99));
          if (keyValuePair.Value.NewObjectID != num2 && keyValuePair.Value.NewObjectID != num1)
          {
            if (cacheData.GetNewKey(ImportingCategory.WorkspaceToCheckOut, (object) keyValuePair.Value.NewObjectID) == 0L)
            {
              long newKey1 = cacheData.GetNewKey(ImportingCategory.Workspaces, (object) keyValuePair.Value.NewObjectID);
              long newKey2 = cacheData.GetNewKey(ImportingCategory.CheckOutSelections, (object) keyValuePair.Value.NewObjectID);
              irlWs.AddRelation(newKey1, newKey2, relationType);
              importingList.Add(new AdditionalUserInfo(ImportingCategory.WorkspaceToCheckOut, keyValuePair.Value.NewObjectID, newKey1, newKey2));
            }
            if (cacheData.GetNewKey(ImportingCategory.WorkspaceToMyTrash, (object) keyValuePair.Value.NewObjectID) == 0L)
            {
              long newKey3 = cacheData.GetNewKey(ImportingCategory.Workspaces, (object) keyValuePair.Value.NewObjectID);
              long newKey4 = cacheData.GetNewKey(ImportingCategory.TrashSelections, (object) keyValuePair.Value.NewObjectID);
              irlWs.AddRelation(newKey3, newKey4, relationType);
              importingList.Add(new AdditionalUserInfo(ImportingCategory.WorkspaceToMyTrash, keyValuePair.Value.NewObjectID, newKey3, newKey4));
            }
          }
        }
        irlWs.Import();
      }
      this.plugin.appManager.AddInfoMessage("Добавлено пользователей: " + num3.ToString());
      this.PumpCheckPoint("Перекачка данных успешно завершена", 100);
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.RankList, ImportingCategory.Workspaces, ImportingCategory.CheckOutSelections, ImportingCategory.TrashSelections, ImportingCategory.WorkspaceToCheckOut, ImportingCategory.WorkspaceToMyTrash);
      dictionary?.Clear();
    }
  }

  private string DecodeIt(string codestr)
  {
    if (codestr == null || codestr.Length == 0)
      return string.Empty;
    string empty = string.Empty;
    int index = 0;
    do
    {
      char ch1 = codestr[index];
      int num = (int) codestr[index + 1];
      char ch2 = codestr[index + 2];
      if (Convert.ToInt32((char) num) > Convert.ToInt32(ch1) + 3)
        ch2 = Convert.ToChar((byte) (Convert.ToInt32(ch2) - 100));
      if (Convert.ToInt32(ch1) >= 60)
      {
        ch1 = Convert.ToChar(Convert.ToInt32(ch1) - 60);
        ch2 = Convert.ToChar(Convert.ToInt32(ch2) + 128 /*0x80*/);
      }
      ch2 = Convert.ToChar(Convert.ToInt32(ch2) ^ Convert.ToInt32(ch1));
      empty += ch2.ToString();
      index += 3;
    }
    while (index < codestr.Length - 2);
    return empty;
  }

  private struct QuickUserInfo(int id, string name)
  {
    public int UserID = id;
    public string UserName = name;
  }
}

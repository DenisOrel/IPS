// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Security.PumpAccessRights
// Assembly: Intermech.ImpExp.Security, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B4185E78-CFCB-46F6-B1BC-486522A5A9AE
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Security.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Security;

[TaskDescription("Инициализация данных для перекачки прав доступ", "Перекачка прав доступа")]
public class PumpAccessRights : PumpClass
{
  protected PumpSecurityPlugin plugin;
  private CacheCategory _rightsCache;
  private CacheCategory _archivesCache;
  private CacheCategory _documentsCache;
  private CacheCategory _articlesCache;
  private CacheCategory _artTypes;
  private CacheCategory _themeParams;
  private CacheCategory _classifCache;
  private CacheCategory _processesCache;
  private AccessRightsMapper rightsMapper = new AccessRightsMapper();
  private List<SecurityRecord> _secRecords = new List<SecurityRecord>();
  private List<int> _secRecordIds = new List<int>();
  private int ObjTypeClassifiersRootID;
  public static readonly Guid SchemesGuid = new Guid("cad002ac-306c-11d8-b4e9-00304f19f545");
  public static int SchemesTypeID = 0;
  public static readonly Guid SchemeCategoriesGuid = new Guid("cad002ab-306c-11d8-b4e9-00304f19f545");
  public static int SchemeCategoriesID;
  private Dictionary<int, string> _classifKeys;
  private List<BOType> KnownBOTypes = new List<BOType>((IEnumerable<BOType>) new BOType[13]
  {
    BOType.Document,
    BOType.Archive,
    BOType.Object,
    BOType.ArticleType,
    BOType.ThemeParamsGroup,
    BOType.ThemeParameter,
    BOType.Articles,
    BOType.Classificator,
    BOType.ClassificatorsList,
    BOType.User,
    BOType.UserGroup,
    BOType.Scheme,
    BOType.SchemesRoot
  });
  private const int _packetSize = 50;
  private Dictionary<int, int> _lcStepsCache = new Dictionary<int, int>();
  private Dictionary<int, List<int>> _themeGroupParams = new Dictionary<int, List<int>>();

  protected override Guid GUID => new Guid("{D85147D1-0206-4326-996D-B4CDAA76EC76}");

  public PumpAccessRights(PumpSecurityPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  public override void Exam() => this.ExamCheckPoint("Проверка данных успешно завершена", 100);

  private AccessType ToAccessType(string denyIt, string grantIt)
  {
    if (grantIt == "Y")
      return AccessType.Grant;
    return denyIt == "Y" ? AccessType.Deny : AccessType.NoGrant;
  }

  private void LoadClassifierKeys()
  {
    if (this._classifKeys != null)
      return;
    this._classifKeys = new Dictionary<int, string>();
    using (IDataReader dataReader = BasePumpHelper.S4Query("select c.class_id,c.folder_key from class_list c where exists (select * from bo_rights r where r.bo_id = 39 and c.class_id = r.instanc_id)"))
    {
      while (dataReader.Read())
        this._classifKeys.Add(BasePumpHelper.ToInt32(dataReader[0]), dataReader.GetString(1));
    }
  }

  private long ClassifIDToNewID(int id)
  {
    this.LoadClassifierKeys();
    string oldKey = "";
    return this._classifKeys.TryGetValue(id, out oldKey) ? this._classifCache.GetNewKey((object) oldKey) : 0L;
  }

  private bool ProcessRow(IDataReader reader)
  {
    BOType int32_1 = (BOType) BasePumpHelper.ToInt32(reader[3]);
    if (this.KnownBOTypes.Contains(int32_1))
    {
      int int32_2 = BasePumpHelper.ToInt32(reader[0]);
      int int32_3 = BasePumpHelper.ToInt32(reader[1]);
      long num1 = int32_3 == 0 ? BasePumpHelper.GetNewGroupID(int32_2) : BasePumpHelper.GetNewUserID(int32_3);
      RightType int32_4 = (RightType) BasePumpHelper.ToInt32(reader[4]);
      int int32_5 = BasePumpHelper.ToInt32(reader[5]);
      RightInfo rightInfo = this.rightsMapper.Map(int32_1, int32_4);
      if (rightInfo != null && num1 != 0L)
      {
        List<long> longList = new List<long>();
        switch (int32_1)
        {
          case BOType.Document:
            DictionaryValue dictionaryValue1 = this._documentsCache.GetValue((object) int32_5);
            if (dictionaryValue1 != null)
            {
              using (Dictionary<int, long>.Enumerator enumerator = (dictionaryValue1.Tag as DocumentTag).Versions.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  KeyValuePair<int, long> current = enumerator.Current;
                  longList.Add(current.Value);
                }
                break;
              }
            }
            break;
          case BOType.Articles:
            if (int32_5 == 0)
            {
              longList.Add(0L);
              break;
            }
            break;
          case BOType.Archive:
            long newKey1 = this._archivesCache.GetNewKey((object) int32_5);
            if (newKey1 != 0L)
            {
              longList.Add(newKey1);
              break;
            }
            break;
          case BOType.UserGroup:
            long newGroupId = BasePumpHelper.GetNewGroupID(int32_5);
            if (newGroupId != 0L)
            {
              longList.Add(newGroupId);
              break;
            }
            break;
          case BOType.ThemeParamsGroup:
            List<int> intList = new List<int>();
            if (this._themeGroupParams.TryGetValue(int32_5, out intList))
            {
              using (List<int>.Enumerator enumerator = intList.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  long newKey2 = this._themeParams.GetNewKey((object) enumerator.Current);
                  if (newKey2 != 0L)
                    longList.Add(newKey2);
                }
                break;
              }
            }
            break;
          case BOType.ThemeParameter:
            long newKey3 = this._themeParams.GetNewKey((object) int32_5);
            if (newKey3 != 0L)
            {
              longList.Add(newKey3);
              break;
            }
            break;
          case BOType.User:
            long newUserId = BasePumpHelper.GetNewUserID(int32_5);
            if (newUserId != 0L)
            {
              longList.Add(newUserId);
              break;
            }
            break;
          case BOType.Scheme:
            long newKey4 = this._processesCache.GetNewKey((object) int32_5);
            if (newKey4 != 0L)
            {
              longList.Add(newKey4);
              break;
            }
            break;
          case BOType.SchemesRoot:
            longList.Add((long) PumpAccessRights.SchemesTypeID);
            longList.Add((long) PumpAccessRights.SchemeCategoriesID);
            break;
          case BOType.ArticleType:
            long newKey5 = this._artTypes.GetNewKey((object) int32_5);
            if (newKey5 > 0L)
            {
              longList.Add(newKey5);
              break;
            }
            break;
          case BOType.Object:
            DictionaryValue dictionaryValue2 = this._articlesCache.GetValue((object) int32_5);
            if (dictionaryValue2 != null)
            {
              using (Dictionary<int, long>.Enumerator enumerator = (dictionaryValue2.Tag as ArticleTag).Versions.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  KeyValuePair<int, long> current = enumerator.Current;
                  longList.Add(current.Value);
                }
                break;
              }
            }
            break;
          case BOType.ClassificatorsList:
            if (int32_5 == 0)
            {
              longList.Add((long) this.ObjTypeClassifiersRootID);
              break;
            }
            break;
          case BOType.Classificator:
            long newId = this.ClassifIDToNewID(int32_5);
            if (newId != 0L)
            {
              longList.Add(newId);
              break;
            }
            break;
        }
        if (longList.Count > 0)
        {
          DateTime dateTime1 = DateTime.MinValue;
          DateTime dateTime2 = DateTime.MinValue;
          object fldvalue1 = reader[10];
          BasePumpHelper.FixDateTimeField(ref fldvalue1);
          if (fldvalue1 != null && !DBNull.Value.Equals(fldvalue1))
          {
            object fldvalue2 = reader[11];
            BasePumpHelper.FixDateTimeField(ref fldvalue2);
            if (fldvalue2 != null && !DBNull.Value.Equals(fldvalue2))
            {
              dateTime1 = Convert.ToDateTime(fldvalue1);
              dateTime2 = Convert.ToDateTime(fldvalue2);
            }
          }
          long num2 = BasePumpHelper.GetNewUserID(BasePumpHelper.ToInt32(reader[8]));
          if (num2 == 0L)
            num2 = BasePumpHelper.SessionUserID;
          int accessType = (int) this.ToAccessType(reader.GetString(6), reader.GetString(7));
          foreach (long num3 in longList)
          {
            long num4 = num3;
            foreach (ActionType type in rightInfo.Types)
            {
              if (rightInfo.Category == 7)
                num4 = this.GetLCStepCategoryID(Convert.ToInt32(num3));
              SecurityRecord securityRecord = new SecurityRecord();
              securityRecord.CategoryID = num4;
              securityRecord.CategoryType = rightInfo.Category;
              securityRecord.UserId = (object) num1;
              securityRecord.RightId = (int) type;
              securityRecord.RightType = accessType;
              securityRecord.OwnerId = (object) num2;
              if (dateTime1 != DateTime.MinValue)
              {
                securityRecord.BeginDate = (object) dateTime1;
                securityRecord.EndDate = (object) dateTime2;
              }
              this._secRecords.Add(securityRecord);
            }
          }
          return true;
        }
      }
    }
    return false;
  }

  private void CheckDataPacket(bool ForcePump)
  {
    if (!ForcePump && this._secRecords.Count < 50)
      return;
    this.plugin.Idw.ImportSequrity(this._secRecords.ToArray());
    this._secRecords.Clear();
    foreach (int secRecordId in this._secRecordIds)
      this._rightsCache.AddValue((object) secRecordId, 1L);
    this._secRecordIds.Clear();
  }

  private int GetLCStepForType(int typeID)
  {
    int lcStepForType = 0;
    if (!this._lcStepsCache.TryGetValue(typeID, out lcStepForType))
    {
      lcStepForType = BasePumpHelper.Session.GetLifecycleStepCollection(typeID).GetFirstStep();
      this._lcStepsCache.Add(typeID, lcStepForType);
    }
    return lcStepForType;
  }

  private long GetLCStepCategoryID(int objTypeID)
  {
    return Convert.ToInt64(objTypeID) << 32 /*0x20*/ | (long) (uint) this.GetLCStepForType(objTypeID);
  }

  public override void Pump()
  {
    SimpleLogger logger = BasePumpHelper.Logger;
    this._rightsCache = PumpCache.Category[ImportingCategory.AccessRights];
    this._archivesCache = PumpCache.Category[ImportingCategory.Archives];
    this._documentsCache = PumpCache.Category[ImportingCategory.Documents];
    this._articlesCache = PumpCache.Category[ImportingCategory.Articles];
    this._artTypes = PumpCache.Category[ImportingCategory.ArticleTypes];
    this._themeParams = PumpCache.Category[ImportingCategory.ThematicParams];
    this._classifCache = PumpCache.Category[ImportingCategory.Classificators];
    this._processesCache = PumpCache.Category[ImportingCategory.Processes];
    IMetadataInfo imdi = this.plugin.Imdi;
    this.ObjTypeClassifiersRootID = imdi.ObjectTypes.GetByGuid(new Guid("cad00157-306c-11d8-b4e9-00304f19f545")).ID;
    PumpAccessRights.SchemesTypeID = imdi.ObjectTypes.GetByGuid(PumpAccessRights.SchemesGuid).ID;
    PumpAccessRights.SchemeCategoriesID = imdi.ObjectTypes.GetByGuid(PumpAccessRights.SchemeCategoriesGuid).ID;
    using (IDataReader dataReader = BasePumpHelper.S4Query("select group_id, param_id from paramstbl"))
    {
      while (dataReader.Read())
      {
        List<int> intList = (List<int>) null;
        int int32 = BasePumpHelper.ToInt32(dataReader[0]);
        if (!this._themeGroupParams.TryGetValue(int32, out intList))
        {
          intList = new List<int>();
          this._themeGroupParams.Add(int32, intList);
        }
        intList.Add(BasePumpHelper.ToInt32(dataReader[1]));
      }
    }
    try
    {
      using (IDbCommand command = this.plugin.idb2.DbConnection.CreateCommand())
      {
        this.PumpCheckPoint("Определение количества прав доступа для перекачки", 0);
        string str1 = "where r.bo_id <> 16";
        command.CommandText = "select count(*) from bo_rights r " + str1;
        int int32_1 = Convert.ToInt32(command.ExecuteScalar());
        logger.Write($"{command.CommandText}: {int32_1} result(s)");
        string format1 = "select {0} from bo_rights r, groups g {1} and g.group_id = r.group_id order by instanc_id, right_id";
        string str2 = "g.group_id, g.user_id, r.record_id, r.bo_id, r.right_id, r.instanc_id, r.deny_it, r.grant_it, r.user_id as owner, r.date_type, r.begin_date, r.end_date";
        command.CommandText = string.Format(format1, (object) str2, (object) str1);
        IDataReader reader = command.ExecuteReader();
        try
        {
          int index = 1;
          string format2 = "Перекачка прав доступа ({0} из {1})";
          while (reader.Read())
          {
            this.PumpCheckPoint(string.Format(format2, (object) index, (object) int32_1), this.CalculatePercent(int32_1, index, 1, 99));
            logger.Flush();
            try
            {
              int int32_2 = BasePumpHelper.ToInt32(reader[2]);
              if (this._rightsCache.GetNewKey((object) int32_2) <= 0L)
              {
                this.CheckDataPacket(false);
                if (this.ProcessRow(reader))
                  this._secRecordIds.Add(int32_2);
              }
            }
            finally
            {
              ++index;
            }
          }
          this.CheckDataPacket(true);
        }
        finally
        {
          reader.Close();
        }
      }
      this.PumpCheckPoint("Перекачка прав доступа успешно завершена", 100);
      logger.Write("=========Pump end\r\n\r\n");
    }
    catch (Exception ex)
    {
      logger.Write($"=========Pump abort ({ex.Message})\r\n\r\n");
      throw;
    }
    finally
    {
      this._rightsCache.Release();
      this._archivesCache.Release();
      this._documentsCache.Release();
      this._articlesCache.Release();
      this._artTypes.Release();
      this._themeParams.Release();
      this._classifCache.Release();
      this._processesCache.Release();
    }
  }
}

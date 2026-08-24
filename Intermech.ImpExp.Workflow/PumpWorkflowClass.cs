// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.PumpWorkflowClass
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using Intermech.Expert;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Workflow;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Workflow;

[TaskDescription("Инициализация перекачки данных маршрутизатора", "Перекачка данных маршрутизатора")]
[TaskType(PumperType.MetaData)]
public class PumpWorkflowClass : PumpClass
{
  protected WorkflowPlugin plugin;
  private Guid _guid = new Guid("{566C5694-96C3-4f24-95BD-EC1C77956EEA}");
  private IImportedObjectList _iol;
  private IImportedObjectList _iol2;
  private IImportedRelationList _rwriter;
  private CacheCategory _docsCache;
  private CacheCategory _articlesCache;
  private CacheCategory _processesCache;
  private CacheCategory _processesToSkip;
  private CacheCategory _formsCache;
  private CacheCategory _scriptsCache;
  private CacheCategory _schemeCategoriesCache;
  private CacheCategory _subprocessesCache;

  protected override Guid GUID => this._guid;

  public PumpWorkflowClass(WorkflowPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  public override void Exam() => this.ExamCheckPoint("Проверка данных успешно завершена", 100);

  public override void Pump()
  {
    this.DoPump();
    this.DoPumpReferences();
  }

  private DataReadResult ReadWorkflowScheme(IDataReader wfSchemesReader, WorkflowScheme scheme)
  {
    scheme.Clear();
    bool flag = wfSchemesReader.Read();
    if (flag)
    {
      for (int i = 0; i < wfSchemesReader.FieldCount; ++i)
        BasePumpHelper.AddDBValueToDictionary(scheme.Data, wfSchemesReader.GetName(i).ToLower(), wfSchemesReader[i]);
      if (this._processesCache.GetNewKey((object) scheme.SchemeID) > 0L)
      {
        BasePumpHelper.Logger.Write($"Skipped ID={scheme.SchemeID}, rule=exists", false);
        return DataReadResult.Skipped;
      }
      if (scheme.IsProcess)
      {
        long newKey = this._processesToSkip.GetNewKey((object) scheme.SchemeID);
        if (!PumpWorkflowSettings.HasOption(WFOptions.PumpBig) && newKey >= (long) PumpWorkflowSettings.UnrealBigSchemeActivitiesCount)
        {
          BasePumpHelper.Logger.Write($"Skipped ID={scheme.SchemeID}, rule=big", false);
          return DataReadResult.Skipped;
        }
      }
      BasePumpHelper.Logger.Write($"Read scheme ID={scheme.SchemeID}", true);
      IDataReader dataReader1 = BasePumpHelper.S4Query("select * from ActivitiesTable where SchemeID=@p1 order by ActivityID", (object) scheme.SchemeID);
      try
      {
        while (dataReader1.Read())
        {
          ActInfo actInfo = new ActInfo(scheme);
          for (int i = 0; i < dataReader1.FieldCount; ++i)
            BasePumpHelper.AddDBValueToDictionary(actInfo.Data, dataReader1.GetName(i).ToLower(), dataReader1[i]);
          scheme.Activities.Add(actInfo);
        }
      }
      finally
      {
        dataReader1.Close();
        BlobHelper.Clear();
      }
      IDbCommand lastS4Query = BasePumpHelper.LastS4Query;
      lastS4Query.CommandText = !scheme.IsProcess ? "select * from ActivityLinksTable where SchemeID=@p1 order by ActivityID, LinkID" : "select l.* from ActivityLinksTable l where l.schemeid=@p1 and exists (select * from ActivitiesTable a where a.SchemeID=l.SchemeID and a.ActivityID=l.ActivityID and a.ParentActivityID=-1) order by ActivityID, LinkID";
      BasePumpHelper.NormalizeCommandText(lastS4Query);
      IDataReader reader = lastS4Query.ExecuteReader(CommandBehavior.Default);
      try
      {
        while (reader.Read())
        {
          LinkInfo linkInfo = new LinkInfo(reader);
          scheme.Links.Add(linkInfo);
        }
      }
      finally
      {
        reader.Close();
      }
      BasePumpHelper.AddQueryParameter(lastS4Query, (object) 0);
      for (int index1 = -1; index1 < scheme.Activities.Count; ++index1)
      {
        ActInfo activity = index1 <= -1 ? (ActInfo) null : scheme.Activities[index1];
        if (activity != null && scheme.IsProcess)
        {
          lastS4Query.CommandText = "select * from usertaskstable where processid=@p1 and activityid=@p2 order by activityid";
          (lastS4Query.Parameters[1] as IDbDataParameter).Value = (object) activity.ID;
          BasePumpHelper.NormalizeCommandText(lastS4Query);
          IDataReader dataReader2 = lastS4Query.ExecuteReader(CommandBehavior.Default);
          try
          {
            activity.Tasks = new List<S4Table>();
            while (dataReader2.Read())
            {
              S4Table dict = new S4Table();
              for (int i = 0; i < dataReader2.FieldCount; ++i)
                BasePumpHelper.AddDBValueToDictionary((Dictionary<string, object>) dict, dataReader2.GetName(i).ToLower(), dataReader2[i]);
              activity.Tasks.Add(dict);
            }
          }
          finally
          {
            dataReader2.Close();
          }
          IDbCommand dbCommand = (IDbCommand) null;
          foreach (S4Table task in activity.Tasks)
          {
            if (task["kind"].ToString() == "1")
            {
              int int32 = Convert.ToInt32(task["messageid"]);
              if (int32 > 0)
              {
                IDataReader dataReader3;
                if (dbCommand == null)
                {
                  dataReader3 = BasePumpHelper.S4Query("select * from UserMessagesTable where id = @p1", (object) int32);
                  dbCommand = BasePumpHelper.LastS4Query;
                }
                else
                {
                  (dbCommand.Parameters[0] as IDbDataParameter).Value = (object) int32;
                  dataReader3 = dbCommand.ExecuteReader(CommandBehavior.Default);
                }
                try
                {
                  while (dataReader3.Read())
                  {
                    for (int i = 0; i < dataReader3.FieldCount; ++i)
                      BasePumpHelper.AddDBValueToDictionary((Dictionary<string, object>) task, "msg." + dataReader3.GetName(i).ToLower(), dataReader3[i]);
                  }
                }
                finally
                {
                  dataReader3.Close();
                }
              }
            }
          }
        }
        for (int index2 = 0; index2 < wfTables.SchemeID_Linked.Length; ++index2)
        {
          lastS4Query.CommandText = $"select * from {wfTables.SchemeID_Linked[index2].ToUpper()} where SchemeID=@p1 and ActivityID=@p2";
          int num = -1;
          if (index1 != -1)
            num = activity.ID;
          (lastS4Query.Parameters[1] as IDbDataParameter).Value = (object) num;
          BasePumpHelper.NormalizeCommandText(lastS4Query);
          string lower1 = wfTables.SchemeID_Linked_InternalIDs[index2].ToLower();
          if (lower1 != "")
          {
            IDbCommand dbCommand = lastS4Query;
            dbCommand.CommandText = $"{dbCommand.CommandText} order by {lower1}";
          }
          IDataReader dataReader4 = lastS4Query.ExecuteReader();
          try
          {
            S4Table s4Table = new S4Table();
            if (index1 == -1)
              scheme.Table.Add(wfTables.SchemeID_Linked_Alias[index2], s4Table);
            else
              activity.Add(wfTables.SchemeID_Linked_Alias[index2], s4Table);
            while (dataReader4.Read())
            {
              Dictionary<string, object> dict = (Dictionary<string, object>) null;
              for (int i = 0; i < dataReader4.FieldCount; ++i)
              {
                string lower2 = dataReader4.GetName(i).ToLower();
                if (dict == null)
                {
                  if (lower1 != "")
                  {
                    dict = new Dictionary<string, object>();
                    string key = dataReader4[lower1].ToString();
                    if (s4Table.ContainsKey(key))
                    {
                      List<Dictionary<string, object>> dictionaryList;
                      if (s4Table[key] is List<Dictionary<string, object>>)
                      {
                        dictionaryList = (List<Dictionary<string, object>>) s4Table[key];
                      }
                      else
                      {
                        dictionaryList = new List<Dictionary<string, object>>();
                        dictionaryList.Add((Dictionary<string, object>) s4Table[key]);
                        s4Table[key] = (object) dictionaryList;
                      }
                      dictionaryList.Add(dict);
                    }
                    else
                      s4Table.Add(key, (object) dict);
                  }
                  else
                    dict = (Dictionary<string, object>) s4Table;
                }
                if ((!(lower1 != "") || !(lower1 == lower2)) && !(lower2 == "schemeid") && dict != null)
                  BasePumpHelper.AddDBValueToDictionary(dict, lower2, dataReader4[i]);
              }
            }
          }
          finally
          {
            dataReader4.Close();
          }
        }
      }
    }
    return flag ? DataReadResult.OK : DataReadResult.NoData;
  }

  private void PumpVariables(S4Table vars)
  {
    VarList varList = new VarList(BasePumpHelper.Session, true, true);
    Dictionary<Variable, string> dictionary1 = new Dictionary<Variable, string>();
    foreach (KeyValuePair<string, object> var in (Dictionary<string, object>) vars)
    {
      string key1 = var.Key;
      if (var.Value is Dictionary<string, object>)
      {
        Dictionary<string, object> dictionary2 = (Dictionary<string, object>) var.Value;
        string str1 = dictionary2["name"].ToString();
        int int32_1 = Convert.ToInt32(dictionary2["vartype"]);
        string str2 = BasePumpHelper.BlobToString(dictionary2["varvalue"]);
        object[] addInfo = (object[]) null;
        VarType varType = VarsPump.IntToVarType(int32_1);
        if (varType == VarType.StringList)
          addInfo = new object[1]{ (object) str2 };
        Variable key2 = varList.AddVariable(0, str1, varType, addInfo);
        string possibleValues = (string) null;
        if (varType == VarType.StringList)
          possibleValues = key2.ValuesList.Text;
        int newVarTypeId = VarsPump.GetNewVarTypeID(str1, int32_1, possibleValues);
        key2.VariableType = VarsPump.LastVarType;
        key2.Name = VarsPump.LastVarName;
        key2.AttrTypeID = newVarTypeId;
        dictionary2.Add("newid", (object) newVarTypeId);
        switch (key2.VariableType)
        {
          case VarType.StringList:
            continue;
          case VarType.ParticipantList:
            dictionary1.Add(key2, str2);
            continue;
          case VarType.Archive:
            try
            {
              int int32_2 = Convert.ToInt32(str2);
              key2.StoredValue = PumpHelper.GetNewArchiveGuid(int32_2).ToString();
              continue;
            }
            catch
            {
              continue;
            }
          default:
            key2.StoredValue = str2;
            continue;
        }
      }
    }
    foreach (KeyValuePair<Variable, string> keyValuePair in dictionary1)
    {
      ParticipantList pl = new ParticipantList();
      foreach (string str in (List<string>) new StringList()
      {
        Text = keyValuePair.Value
      })
      {
        char[] chArray = new char[1]{ '=' };
        string[] strArray = str.Split(chArray);
        ParticipantKind Kind = ParticipantKind.Variable;
        if (!strArray[0].Equals("0"))
          Kind = (ParticipantKind) Convert.ToInt32(strArray[1]);
        switch (Kind)
        {
          case ParticipantKind.User:
            long newUserId = BasePumpHelper.GetNewUserID(Convert.ToInt32(strArray[0]));
            if (newUserId > 0L)
            {
              pl.AddParticipant(Kind, newUserId);
              continue;
            }
            continue;
          case ParticipantKind.Group:
            int int32 = Convert.ToInt32(strArray[0]);
            PumpHelper.AddGroup(pl, int32);
            continue;
          case ParticipantKind.Variable:
            int newVarTypeId = VarsPump.GetNewVarTypeID(strArray[1].ToString(), -1);
            if (newVarTypeId > 0)
            {
              pl.AddParticipant(ParticipantKind.Variable, (long) newVarTypeId);
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      keyValuePair.Key.StoredValue = pl.AsString;
    }
    if (varList.Count <= 0)
      return;
    BlobHelper.ReserveBlob(new SaveToStreamDelegate(varList.SaveToStream));
    this.Iol.AddAttributeBlob(wfConsts.AttrVariablesID, BlobHelper.TempFileName, BlobHelper.FileSize, "", ArcMethods.NotPacked);
  }

  private void PumpNotifications(ActInfo ai)
  {
    S4Table s4Table1 = ai["notifs"];
    if (s4Table1.Count > 0)
    {
      S4Table s4Table2 = ai["messages"];
      S4Table s4Table3 = ai["msgrecips"];
      Notifications notifications = new Notifications(BasePumpHelper.Session);
      List<Notification> notificationList = new List<Notification>();
      notificationList.Add(notifications.StartNotify);
      notificationList.Add((Notification) notifications.PeriodNotify);
      notificationList.Add(notifications.StopNotify);
      string[] strArray = new string[3]
      {
        "startmessageenabled",
        "periodmessageenabled",
        "stopmessageenabled"
      };
      for (int index = 0; index < notificationList.Count; ++index)
      {
        string key = index.ToString();
        Notification notification = notificationList[index];
        notification.Enabled = Convert.ToInt32(s4Table1[strArray[index]]) == 1;
        if (notification.Enabled && s4Table2.ContainsKey(key))
        {
          Dictionary<string, object> dictionary1 = s4Table2[key] as Dictionary<string, object>;
          notification.Subject = dictionary1["subject"].ToString();
          notification.Text = BasePumpHelper.BlobToString(dictionary1["text"]);
          if (s4Table3.ContainsKey(key))
          {
            ParticipantList participantList = new ParticipantList();
            object obj = s4Table3[key];
            List<Dictionary<string, object>> dictionaryList;
            if (obj is List<Dictionary<string, object>>)
            {
              dictionaryList = (List<Dictionary<string, object>>) obj;
            }
            else
            {
              dictionaryList = new List<Dictionary<string, object>>();
              dictionaryList.Add(obj as Dictionary<string, object>);
            }
            foreach (Dictionary<string, object> dictionary2 in dictionaryList)
            {
              ParticipantKind int32_1 = (ParticipantKind) Convert.ToInt32(dictionary2["recipkind"]);
              int int32_2 = Convert.ToInt32(dictionary2["recipid"]);
              switch (int32_1)
              {
                case ParticipantKind.User:
                  long newUserId = BasePumpHelper.GetNewUserID(int32_2);
                  if (newUserId > 0L)
                  {
                    participantList.AddParticipant(int32_1, newUserId);
                    continue;
                  }
                  continue;
                case ParticipantKind.Group:
                  long newGroupId = BasePumpHelper.GetNewGroupID(int32_2);
                  if (newGroupId > 0L)
                  {
                    participantList.AddParticipant(int32_1, newGroupId);
                    continue;
                  }
                  continue;
                case ParticipantKind.Variable:
                  int newVarId = ai.VarIDToNewVarID(int32_2);
                  if (newVarId > 0)
                  {
                    participantList.AddParticipant(ParticipantKind.Variable, (long) newVarId);
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
            notification.Recips = participantList;
          }
          if (notification is PeriodNotification && notification.Enabled)
          {
            PeriodInformation period = ((PeriodNotification) notification).Period;
            period.Units = (TimeUnits) Convert.ToInt32(s4Table1["unitskind"]);
            period.UnitsCount = Convert.ToInt32(s4Table1["unitscount"]);
            string Name = s4Table1["datevarname"].ToString();
            if (Name.Trim() != "")
              period.VarTypeID = VarsPump.GetNewVarTypeID(Name, -1);
          }
        }
      }
      BlobHelper.ReserveBlob(new SaveToStreamDelegate(notifications.SaveToStream));
      this.Iol.AddAttributeBlob(wfConsts.AttrNotificationsID, BlobHelper.TempFileName, BlobHelper.FileSize, notifications.BriefString, ArcMethods.NotPacked);
    }
    else
      this.Iol.AddAttributeBlob(wfConsts.AttrNotificationsID, "", 0L, "", ArcMethods.NotPacked);
  }

  private Guid DBArcIDToNewArcID(string dbArcID)
  {
    if (dbArcID.Length > 0 && dbArcID[0] == ' ')
    {
      dbArcID = dbArcID.Trim();
      try
      {
        return PumpHelper.GetNewArchiveGuid(Convert.ToInt32(dbArcID));
      }
      catch
      {
      }
    }
    else if (VarsPump.GetNewVarTypeID(dbArcID, -1) > 0)
      return VarsPump.LastVarInfo.AttrGuid;
    return Guid.Empty;
  }

  private int FormStringHashCode(string value)
  {
    int num1 = 5381;
    int num2 = num1;
    for (int index = 0; index < value.Length; index += 2)
    {
      num1 = (num1 << 5) + num1 ^ (int) value[index];
      if (index + 1 < value.Length)
        num2 = (num2 << 5) + num2 ^ (int) value[index + 1];
    }
    return num1 + num2 * 1566083941;
  }

  private void PumpAdditional(ActInfo ai, StringList data)
  {
    ActivityKind kind = ai.Kind;
    WorkflowScheme scheme = ai.Scheme;
    int num1 = wfConsts.IsParticipantActivity(kind) ? 1 : 0;
    IImportedObjectList iol = this.Iol;
    IDataWriter idw = this.plugin.Idw;
    if (num1 != 0 || kind == ActivityKind.Register)
    {
      string str = data.Values["ROLLBACKKIND"];
      int num2 = 0;
      if (str != null)
        num2 = Convert.ToInt32(str);
      iol.AddAttributeInt(wfConsts.AttrRollbackKindID, (long) num2);
    }
    long num3 = 0;
    if (num1 != 0)
    {
      string str1 = data.Values["ALLOWADDATTACHS"];
      if (str1 != null && Convert.ToInt32(str1) == 0)
        num3 |= 2L;
      string str2 = data.Values["ALLOWDELATTACHS"];
      if (str2 != null && Convert.ToInt32(str2) == 0)
        num3 |= 4L;
    }
    object obj = ai.Data["denydel"];
    if (!obj.Equals((object) DBNull.Value) && Convert.ToInt32(obj) == 1)
      num3 |= 1L;
    bool strongSign = true;
    string str3 = scheme.AddData.Values["STRONGSIGN"];
    if (str3 != null)
      strongSign = Convert.ToInt32(str3) == 1;
    S4Table s4Table = ai["form"];
    if (s4Table.Count > 0 && wfConsts.IsParticipantActivity(ai.Kind))
    {
      string text = BasePumpHelper.BlobToString(s4Table[nameof (data)]);
      int oldKey = this.FormStringHashCode(text);
      long newKey = this._formsCache.GetNewKey((object) oldKey);
      if (newKey == 0L)
      {
        FormConverter formConverter = new FormConverter(text);
        formConverter.OnConvertVarValue += new FormConverter.ConvertVarValue(this.FormConverter_OnConvertVarValue);
        if (!formConverter.Empty)
        {
          this.Iol2.Items.Clear();
          this.Iol2.AddObject(wfConsts.FormsTypeID, 0, "Форма");
          this.Iol2.AddAttributeStr(wfConsts.AttrFormNameID, "Форма");
          BlobHelper.ReserveZBlob(new SaveToStreamDelegate(formConverter.SaveToStream));
          this.Iol2.AddAttributeBlob(wfConsts.AttrFormBodyID, BlobHelper.TempFileName, BlobHelper.FileSize, "", ArcMethods.ZLibPacked);
          this.Iol2.Import();
          newKey = this.Iol2.Items[0].Object.Object_id;
          this._formsCache.AddValue((object) oldKey, newKey);
        }
      }
      if (newKey > 0L)
        iol.AddAttributeInt(wfConsts.AttrFormID, newKey);
    }
    Terms terms = new Terms(BasePumpHelper.Session);
    string str4 = data.Values["TERM"];
    if (str4 != null && str4 != "")
      terms.AsList[0].Period = PumpHelper.TermToPeriodInformation(str4);
    if (data.Values["ARTERM"] == "1")
      terms.AsList[0].Enabled = true;
    string str5 = data.Values["RTERM"];
    if (str5 != null && str5 != "")
    {
      terms.AsList[1].Period = PumpHelper.TermToPeriodInformation(str5);
      terms.AsList[1].Enabled = true;
    }
    if (terms.BriefString != "")
    {
      BlobHelper.ReserveBlob(new SaveToStreamDelegate(terms.SaveToStream));
      iol.AddAttributeBlob(wfConsts.AttrTermsID, BlobHelper.TempFileName, BlobHelper.FileSize, terms.BriefString, ArcMethods.NotPacked);
    }
    switch (kind)
    {
      case ActivityKind.Approve:
        string str6 = data.Values["SignAs"];
        if (str6 != null && str6.Length > 0)
        {
          RequiredSigns requiredSigns = new RequiredSigns();
          requiredSigns.Add("1", strongSign, 0);
          BlobHelper.ReserveBlob(new SaveToStreamDelegate(requiredSigns.GraphsSet.Save));
          iol.AddAttributeBlob(wfConsts.AttrRequiredSignsID, BlobHelper.TempFileName, BlobHelper.FileSize, "", ArcMethods.NotPacked);
          break;
        }
        iol.AddAttributeBlob(wfConsts.AttrRequiredSignsID, "", 0L, "", ArcMethods.NotPacked);
        break;
      case ActivityKind.SubProcess:
        int result1 = 0;
        int result2 = 0;
        string s1 = data.Values["SCHEMEID"];
        if (s1 != null)
          int.TryParse(s1, out result1);
        if (result1 > 0)
        {
          string s2 = data.Values["PSCHEMEID"];
          if (s2 != null)
            int.TryParse(s2, out result2);
          if (result2 == -1)
            result2 = 0;
          ai.SubProcessInfo = BasePumpHelper.MakeCacheKey(result1, result2);
          break;
        }
        break;
      case ActivityKind.Condition:
        string expr1 = BasePumpHelper.CommaStringToString(data.Values["COND"]);
        if (!string.IsNullOrEmpty(expr1))
        {
          BlobHelper.ReserveBlob(new SaveToStreamDelegate(new TempFormulaWriter(PumpHelper.CreateTempFormula(expr1)).SaveToStream));
          iol.AddAttributeBlob(wfConsts.AttrConditionID, BlobHelper.TempFileName, BlobHelper.FileSize, "", ArcMethods.NotPacked);
          break;
        }
        break;
      case ActivityKind.Case:
        string str7 = data.Values["CONDITIONS"];
        if (str7 != null && str7 != "")
        {
          StringList stringList = new StringList();
          stringList.CommaText = str7;
          ai.ExpertConditions = new ConditionList();
          for (int index = 0; index < stringList.Count / 2; ++index)
          {
            long result3;
            if (!long.TryParse(stringList[index * 2], out result3))
            {
              BasePumpHelper.Logger.Write($"Ошибка при чтении настроек для действия множественного условного выбора для схемы {scheme.Name}");
            }
            else
            {
              string expr2 = stringList[index * 2 + 1].Trim();
              TempFormula tf = (TempFormula) null;
              if (expr2.ToUpper() != "ИНАЧЕ")
                tf = PumpHelper.CreateTempFormula(expr2);
              ai.ExpertConditions.Add(result3, tf);
            }
          }
          break;
        }
        break;
      case ActivityKind.Timer:
        PeriodInformation periodInformation = new PeriodInformation(BasePumpHelper.Session);
        string str8 = data.Values["EXECAFTER"];
        if (str8 == "-1")
        {
          string Name = data.Values["DATEVARNAME"] ?? data.Values["TIMEVARNAME"];
          periodInformation.VarTypeID = VarsPump.GetNewVarTypeID(Name, -1);
        }
        else
        {
          periodInformation.UnitsCount = Convert.ToInt32(str8);
          string str9 = data.Values["TIMEUNITS"];
          if (str9 != null)
            periodInformation.Units = (TimeUnits) Convert.ToInt32(str9);
        }
        iol.AddAttributeStr(wfConsts.AttrAddInfoID, periodInformation.AsString);
        break;
      case ActivityKind.Register:
        Guid empty = Guid.Empty;
        string dbArcID1 = data.Values["VARNAME"];
        if (dbArcID1 != null)
        {
          Guid newArcId = this.DBArcIDToNewArcID(dbArcID1);
          iol.AddAttributeStr(wfConsts.AttrDocArchiveID, Guid.Empty.Equals(newArcId) ? "" : newArcId.ToString());
        }
        string dbArcID2 = data.Values["REVVARNAME"];
        if (dbArcID2 != null)
        {
          Guid newArcId = this.DBArcIDToNewArcID(dbArcID2);
          iol.AddAttributeStr(wfConsts.AttrRevArchiveID, Guid.Empty.Equals(newArcId) ? "" : newArcId.ToString());
        }
        string str10 = data.Values["DETACHREGISTERED"];
        if (str10 == null || str10 != "0")
        {
          num3 |= 8L;
          break;
        }
        break;
      case ActivityKind.Script:
        string s3 = data.Values["CODETEXT"];
        if (s3 != null)
        {
          string s4 = $"/*\r\n Данный код сценария был импортирован из предыдущей версии системы\r\n и больше не поддерживается.\r\n Для корректной работы сценария его нужно переписать на языке C#.\r\n*/\r\n\r\n/*\r\n{BasePumpHelper.CommaStringToString(s3)}\r\n*/";
          string oldKey = s4.GetHashCode().ToString();
          long num4 = this._scriptsCache.GetNewKey((object) oldKey);
          if (num4 == 0L)
          {
            this.Iol2.Items.Clear();
            this.Iol2.AddObject(wfConsts.ScriptsTypeID, 0, $"Импортированный сценарий {scheme.SchemeID}");
            BlobHelper.ReserveBlob(s4);
            this.Iol2.AddAttributeBlob(wfConsts.AttrScriptTextID, BlobHelper.TempFileName, BlobHelper.FileSize, "", ArcMethods.NotPacked);
            this.Iol2.Import();
            num4 = this.Iol2.Items[0].Object.Object_id;
            this._scriptsCache.AddValue((object) oldKey, num4);
          }
          ai.ScriptData = new DeferredObjectData(num4);
          ScriptExecSide scriptExecSide = ai["parts"].Count > 0 ? ScriptExecSide.Client : ScriptExecSide.Server;
          ai.ScriptData.Tag = (object) scriptExecSide;
          break;
        }
        break;
    }
    if (scheme.IsProcess && ai.Status != ActivityStatus.OnApproach)
    {
      int num5 = 0;
      string str11 = data.Values["MRESULT"];
      if (str11 != null)
      {
        if (str11 != "")
        {
          try
          {
            num5 = Convert.ToInt32(str11);
          }
          catch
          {
          }
        }
      }
      iol.AddAttributeInt(wfConsts.AttrActivityResultID, (long) num5);
      string s5 = data.Values["SENDERACTIVITYID"];
      if (s5 != null && s5 != "")
      {
        int intDef = BasePumpHelper.StrToIntDef(s5, -1);
        ai.SenderActivityID = intDef;
      }
      string s6 = data.Values["SENDERID"];
      if (s6 != null && s6 != "")
      {
        int intDef = BasePumpHelper.StrToIntDef(s6, 0);
        if (intDef != 0)
          PumpHelper.MakeUserLink(iol, intDef, wfConsts.AttrSenderID);
      }
      string str12 = data.Values["PARTHISTORY"];
      if (str12 != null && str12 != "" && str12 != "-1")
        ai.ExecHistory = str12.Split(',');
    }
    if (num3 == 0L)
      return;
    iol.AddAttributeInt(wfConsts.AttrAddIDID, num3);
  }

  private bool FormConverter_OnConvertVarValue(string Name, ref string Value, ref ComponentInfo ci)
  {
    if (VarsPump.GetNewVarTypeID(Value, -1) <= 0)
      return false;
    Value = VarsPump.LastVarInfo.AttrGuid.ToString();
    return true;
  }

  protected IImportedObjectList Iol
  {
    get
    {
      if (this._iol == null)
        this._iol = this.plugin.Idw.CreateImportedObjectList(0);
      return this._iol;
    }
  }

  protected IImportedObjectList Iol2
  {
    get
    {
      if (this._iol2 == null)
        this._iol2 = this.plugin.Idw.CreateImportedObjectList(0);
      return this._iol2;
    }
  }

  protected IImportedRelationList RWriter
  {
    get
    {
      if (this._rwriter == null)
        this._rwriter = this.plugin.Idw.CreateImportedRelationList();
      return this._rwriter;
    }
  }

  private void PumpWorkflowScheme(WorkflowScheme scheme)
  {
    BasePumpHelper.Logger.Write($"Pump scheme ID={scheme.SchemeID}", true);
    BasePumpHelper.CurrentObjectID = scheme.SchemeID;
    BlobHelper.Reset();
    IImportedObjectList iol = this.Iol;
    iol.Items.Clear();
    string name = scheme.Name;
    iol.AddObject(scheme.TypeID, 0, name);
    iol.AddAttributeStr(wfConsts.AttrNameID, name);
    iol.AddAttributeInt(BasePumpHelper.AttrSearchID, scheme.SchemeID);
    int num1 = Convert.ToInt32(scheme.Data["status"]);
    if (!scheme.IsProcess)
      num1 = num1 != 0 ? -1 : -2;
    iol.AddAttributeInt(wfConsts.AttrActivityStatusID, (long) num1);
    VarsPump.ClearRenamedCache();
    this.PumpVariables(scheme.Table["vars"]);
    DateTime universalTime = DateTime.Now.ToUniversalTime();
    AttributesHelper.AddObligatoryObjectAttributes(BasePumpHelper.Session, iol);
    this.Iol.Import();
    scheme.ObjectID = this.Iol.Items[0].Object.Object_id;
    this.Iol.Items.Clear();
    ActivityList activities = scheme.Activities;
    foreach (ActInfo ai in (List<ActInfo>) activities)
    {
      iol.AddObject(ai.TypeID, 0, scheme.ActivitiesLCStep, 0, 0, 0, universalTime, 0, universalTime, ai.Name);
      ai.ListIndex = iol.Items.Count - 1;
      iol.AddAttributeStr(wfConsts.AttrNameID, ai.Name);
      iol.AddAttributeStr(wfConsts.AttrDescriptionID, ai.Data["description"].ToString());
      iol.AddAttributeInt(wfConsts.AttrCollectorID, (long) Convert.ToInt32(ai.Data["collector"]));
      iol.AddAttributeLink(wfConsts.AttrProcessID, scheme.ObjectID, name);
      iol.AddAttributeStr(wfConsts.AttrGraphDataID, $"X={ai.Data["boxx"].ToString()},Y={ai.Data["boxy"].ToString()}");
      if (scheme.IsProcess)
      {
        iol.AddAttributeInt(wfConsts.AttrActivityStatusID, (long) Convert.ToInt32(ai.Data["status"]));
        object obj1 = ai.Data["startedtime"];
        if (obj1 != null && obj1 != DBNull.Value)
          iol.AddAttributeDate(wfConsts.AttrStartedID, Convert.ToDateTime(obj1));
        object obj2 = ai.Data["completedtime"];
        if (obj2 != null && obj2 != DBNull.Value)
          iol.AddAttributeDate(wfConsts.AttrCompletedID, Convert.ToDateTime(obj2));
      }
      if (scheme.IsProcess)
        this.PumpVariables(ai["vars"]);
      if (wfConsts.IsParticipantActivity(ai.Kind) && (scheme.IsProcess || ai.Kind != ActivityKind.Start))
      {
        BlobHelper.ReserveBlob(new SaveToStreamDelegate(ai.SaveParticipantsToStream));
        iol.AddAttributeBlob(wfConsts.AttrParticipantsID, BlobHelper.TempFileName, BlobHelper.FileSize, "", ArcMethods.NotPacked);
      }
      this.PumpNotifications(ai);
      StringList data = new StringList();
      S4Table s4Table = ai["data"];
      if (s4Table.Count > 0)
        data.Text = BasePumpHelper.BlobToString(s4Table["blobdata"]);
      this.PumpAdditional(ai, data);
      if (scheme.IsProcess && ai["amessage"].Count != 0)
      {
        string s = BasePumpHelper.BlobToString(ai["amessage"]["msgtext"]);
        BlobHelper.ReserveBlob(s);
        iol.AddAttributeBlob(wfConsts.AttrActivityMessageID, BlobHelper.TempFileName, BlobHelper.FileSize, s.Length > wfConsts.MaxStoredTextLength ? s.Substring(0, wfConsts.MaxStoredTextLength) : s, ArcMethods.NotPacked);
      }
      if (ai.Tasks != null && ai.Tasks.Count > 0)
      {
        foreach (S4Table task in ai.Tasks)
        {
          int int32_1 = Convert.ToInt32(task["kind"]);
          switch (int32_1)
          {
            case 0:
              int int32_2 = Convert.ToInt32(task["userid"]);
              PumpHelper.MakeUserLink(iol, int32_2, wfConsts.AttrRecipID);
              if (int32_1 != 0)
              {
                int int32_3 = Convert.ToInt32(task["fromuserid"]);
                PumpHelper.MakeUserLink(iol, int32_3, wfConsts.AttrSenderID);
              }
              int num2 = Convert.ToInt32(task["recip_status"]);
              if (num2 > 3)
                num2 = 0;
              iol.AddAttributeInt(wfConsts.AttrRecipStatusID, (long) num2);
              int int32_4 = Convert.ToInt32(task["sender_status"]);
              iol.AddAttributeInt(wfConsts.AttrSenderStatusID, (long) int32_4);
              int int32_5 = Convert.ToInt32(task["sender_del"]);
              iol.AddAttributeInt(wfConsts.AttrSenderDeletionID, (long) int32_5);
              int int32_6 = Convert.ToInt32(task["recip_del"]);
              iol.AddAttributeInt(wfConsts.AttrRecipDeletionID, (long) int32_6);
              continue;
            case 1:
              if (task.ContainsKey("msg.id"))
              {
                string subject = task["msg.subject"].ToString();
                string text = BasePumpHelper.BlobToString(task["msg.text"]);
                DateTime dateTime = Convert.ToDateTime(task["msg.startedtime"]);
                this.CreateMessage(iol, scheme, subject, text, dateTime);
                if (ai.MessageIndexes == null)
                  ai.MessageIndexes = new List<int>();
                ai.MessageIndexes.Add(iol.Items.Count - 1);
                goto case 0;
              }
              goto case 0;
            default:
              continue;
          }
        }
      }
      AttributesHelper.AddObligatoryObjectAttributes(BasePumpHelper.Session, iol);
    }
    foreach (ActInfo actInfo in (List<ActInfo>) activities)
    {
      if (actInfo.FormData != null && actInfo.FormData.ID == -1L)
      {
        iol.AddObject(wfConsts.FormsTypeID, 0);
        actInfo.FormData.Index = (long) (iol.Items.Count - 1);
        iol.AddAttributeStr(wfConsts.AttrFormNameID, actInfo.FormData.Name);
        iol.AddAttributeBlob(wfConsts.AttrFormBodyID, actInfo.FormData.BlobFileName, actInfo.FormData.BlobFileSize, "", ArcMethods.ZLibPacked);
      }
      if (actInfo.ScriptData != null && actInfo.ScriptData.ID == -1L)
      {
        iol.AddObject(wfConsts.ScriptsTypeID, 0, actInfo.ScriptData.Name);
        actInfo.ScriptData.Index = (long) (iol.Items.Count - 1);
        iol.AddAttributeBlob(wfConsts.AttrScriptTextID, actInfo.ScriptData.BlobFileName, actInfo.ScriptData.BlobFileSize, "", ArcMethods.NotPacked);
      }
    }
    this.Iol.Import();
    this.Iol2.Items.Clear();
    foreach (ActInfo ai in (List<ActInfo>) activities)
    {
      int listIndex = ai.ListIndex;
      ai.ObjectRecord = this.Iol.Items[listIndex].Object;
      this.Iol2.UseObject(ai.ObjectRecord);
      if (ai.FormData != null)
      {
        if (ai.FormData.ID == -1L)
          ai.FormData.ID = this.Iol.Items[(int) ai.FormData.Index].Object.Object_id;
        this.Iol2.AddAttributeInt(wfConsts.AttrFormID, ai.FormData.ID);
      }
      if (ai.ScriptData != null)
      {
        if (ai.ScriptData.ID == -1L)
          ai.ScriptData.ID = this.Iol.Items[(int) ai.ScriptData.Index].Object.Object_id;
        this.RWriter.AddRelation(ai.ObjectID, ai.ScriptData.ID, wfConsts.ScriptRelationTypeID);
        this.RWriter.AddAttributeInt(wfConsts.AttrScriptKindID, 0L);
        this.RWriter.AddAttributeInt(wfConsts.AttrScriptExecSideID, (long) (int) ai.ScriptData.Tag);
      }
      if (ai.MessageIndexes != null)
      {
        foreach (int messageIndex in ai.MessageIndexes)
          this.Iol2.AddAttributeLink(wfConsts.AttrActivityID, ai.ObjectID, ai.Data["name"].ToString());
      }
      this.PumpAttachments(ai);
      if (ai.SubProcessInfo != 0L)
        this._subprocessesCache.AddValue((object) ai.ObjectID, ai.SubProcessInfo);
    }
    this.Iol.Items.Clear();
    List<LinkInfo> linkInfoList = new List<LinkInfo>();
    foreach (LinkInfo link in scheme.Links)
    {
      ActInfo byOldId1 = activities.FindByOldID(link.ActivityID);
      ActInfo byOldId2 = activities.FindByOldID(link.LinkTo);
      if (byOldId1 != null && byOldId2 != null)
      {
        linkInfoList.Add(link);
        iol.AddObject(wfConsts.LinksTypeID, 0, scheme.ActivitiesLCStep, 0, 0, 0, universalTime, 0, universalTime, "");
        int num3 = iol.Items.Count - 1;
        iol.AddAttributeLink(wfConsts.AttrProcessID, scheme.ObjectID, name);
        iol.AddAttributeInt(wfConsts.AttrFromActivityID, byOldId1.ObjectID);
        iol.AddAttributeInt(wfConsts.AttrToActivityID, byOldId2.ObjectID);
        int num4 = link.LinkKind;
        if (num4 > 1)
        {
          if (byOldId2.ResetTimerLinks == null)
            byOldId2.ResetTimerLinks = new List<long>();
          byOldId2.ResetTimerLinks.Add((long) num3);
          num4 = num4 == 2 ? 0 : 1;
        }
        if (link.LinkCondition == 1)
          num4 = 2;
        else if (link.LinkCondition == 2)
          num4 = 3;
        if (byOldId1.ExpertConditions != null)
        {
          ConditionInfo conditionInfo = byOldId1.ExpertConditions.Find((long) link.LinkID);
          if (conditionInfo != null)
            num4 = conditionInfo.ExpertFormula != null ? 2 : 3;
        }
        iol.AddAttributeInt(wfConsts.AttrLinkKindID, (long) num4);
        if (byOldId1.Links == null)
          byOldId1.Links = new List<LinkInfo>();
        byOldId1.Links.Add(link);
        AttributesHelper.AddObligatoryObjectAttributes(BasePumpHelper.Session, iol);
      }
    }
    this.Iol.Import();
    for (int index = 0; index < linkInfoList.Count; ++index)
      linkInfoList[index].NewLinkID = this.Iol.Items[index].Object.Object_id;
    foreach (ActInfo actInfo in (List<ActInfo>) activities)
    {
      this.Iol2.UseObject(actInfo.ObjectRecord);
      if (actInfo.ResetTimerLinks != null)
      {
        for (int index = 0; index < actInfo.ResetTimerLinks.Count; ++index)
        {
          long objectId = iol.Items[(int) actInfo.ResetTimerLinks[index]].Object.Object_id;
          this.Iol2.AddAttribute(wfConsts.AttrObjectListID, AttrValueType.integerVal, (object) objectId, index);
        }
      }
      if (actInfo.ExpertConditions != null && actInfo.Links != null)
      {
        ConditionList expertConditions = actInfo.ExpertConditions;
        for (int index = 0; index < expertConditions.Count; ++index)
        {
          ConditionInfo conditionInfo = expertConditions[index];
          conditionInfo.LinkID = actInfo.Links[Convert.ToInt32(conditionInfo.LinkID)].NewLinkID;
        }
        BlobHelper.ReserveBlob(new SaveToStreamDelegate(actInfo.ExpertConditions.SaveToStream));
        this.Iol2.AddAttributeBlob(wfConsts.AttrConditionID, BlobHelper.TempFileName, BlobHelper.FileSize, "", ArcMethods.NotPacked).IsNew = false;
      }
      if (actInfo.SenderActivityID != -1)
      {
        ActInfo byOldId = scheme.Activities.FindByOldID(actInfo.SenderActivityID);
        if (byOldId != null)
          this.Iol2.AddAttributeLink(wfConsts.AttrSenderActivityID, byOldId.ObjectID, byOldId.Name);
      }
      if (actInfo.ExecHistory != null)
      {
        int numInList = 0;
        foreach (string s in actInfo.ExecHistory)
        {
          if (!(s == "-1"))
          {
            long intDef = (long) BasePumpHelper.StrToIntDef(s, -2);
            if (intDef != -2L)
            {
              ActInfo byOldId = scheme.Activities.FindByOldID((int) intDef);
              if (byOldId != null)
              {
                this.Iol2.AddAttribute(wfConsts.AttrExecHistoryID, AttrValueType.integerVal, (object) byOldId.ObjectID, numInList);
                ++numInList;
              }
            }
          }
        }
      }
      if (actInfo.ParentActivityID != -1)
      {
        ActInfo byOldId = activities.FindByOldID(actInfo.ParentActivityID);
        if (byOldId != null)
          this.Iol2.AddAttributeLink(wfConsts.AttrParentActivityID, byOldId.ObjectID, byOldId.Name);
      }
    }
    this.Iol2.Import();
    this._processesCache.AddValue((object) scheme.SchemeID, scheme.ObjectID, scheme.Name);
    if (!scheme.IsProcess)
    {
      object obj = scheme.Data["incategory"];
      int oldKey = 0;
      if (!DBNull.Value.Equals(obj))
        oldKey = Convert.ToInt32(obj);
      if (oldKey > 0)
      {
        long newKey = this._schemeCategoriesCache.GetNewKey((object) oldKey);
        if (newKey > 0L)
          this.RWriter.AddRelation(newKey, scheme.ObjectID, wfConsts.SimpleLinkTypeID);
      }
    }
    this.RWriter.Import();
  }

  private void PumpAttachments(ActInfo ai)
  {
    S4Table s4Table = ai["att"];
    long num = 0;
    foreach (KeyValuePair<string, object> keyValuePair in (Dictionary<string, object>) s4Table)
    {
      string key = keyValuePair.Key;
      if (keyValuePair.Value is Dictionary<string, object>)
      {
        Dictionary<string, object> dictionary = (Dictionary<string, object>) keyValuePair.Value;
        switch (Convert.ToInt32(dictionary["attachmentkind"]))
        {
          case 0:
          case 1:
            int int32_1 = Convert.ToInt32(dictionary["id"]);
            long newKey1 = this._docsCache.GetNewKey((object) int32_1);
            if (newKey1 != 0L)
            {
              this.RWriter.AddRelationFromID(ai.ObjectID, newKey1, wfConsts.AttachmentRelationTypeID);
              num = 1L;
              continue;
            }
            BasePumpHelper.AddWarning(BasePumpHelper.WarningType.Document, $"[AID={ai.ID}] " + "Невозможно восстановить вложение. Документ (ID={0}) не найден", (long) int32_1);
            continue;
          case 4:
            int int32_2 = Convert.ToInt32(dictionary["id"]);
            long newKey2 = this._articlesCache.GetNewKey((object) int32_2);
            if (newKey2 != 0L)
            {
              this.RWriter.AddRelationFromID(ai.ObjectID, newKey2, wfConsts.AttachmentRelationTypeID);
              num = 1L;
              continue;
            }
            BasePumpHelper.AddWarning(BasePumpHelper.WarningType.Article, $"[AID={ai.ID}] " + "Невозможно восстановить вложение. Изделие (ID={0}) не найден", (long) int32_2);
            continue;
          default:
            continue;
        }
      }
    }
    this.Iol2.AddAttributeInt(wfConsts.AttrAttachmentsID, num).IsNew = false;
  }

  private void CreateMessage(
    IImportedObjectList writer,
    WorkflowScheme process,
    string subject,
    string text,
    DateTime startedTime)
  {
    writer.AddObject(wfConsts.MessageTypeID, 0, subject);
    writer.AddAttributeLink(wfConsts.AttrProcessID, process.ObjectID, process.Name);
    writer.AddAttributeStr(wfConsts.AttrSubjectID, subject);
    BlobHelper.ReserveBlob(text);
    string fileNote = text.Length <= wfConsts.MaxStoredTextLength ? text : text.Substring(0, wfConsts.MaxStoredTextLength);
    writer.AddAttributeBlob(wfConsts.AttrActivityMessageID, BlobHelper.TempFileName, BlobHelper.FileSize, fileNote, ArcMethods.NotPacked);
    writer.AddAttributeDate(wfConsts.AttrStartedID, startedTime);
  }

  private void DoPump()
  {
    SimpleLogger logger = BasePumpHelper.Logger;
    logger.Write("=========Pump start");
    logger.Write("Options: " + PumpWorkflowSettings.ToString());
    this._docsCache = PumpCache.Category[ImportingCategory.Documents];
    this._articlesCache = PumpCache.Category[ImportingCategory.Articles];
    this._processesCache = PumpCache.Category[ImportingCategory.Processes];
    this._processesToSkip = PumpCache.Category[ImportingCategory.ProcessesToSkip];
    this._formsCache = PumpCache.Category[ImportingCategory.Forms];
    this._scriptsCache = PumpCache.Category[ImportingCategory.Scripts];
    this._schemeCategoriesCache = PumpCache.Category[ImportingCategory.SchemeCategories];
    this._subprocessesCache = PumpCache.Category[ImportingCategory.Subprocesses];
    try
    {
      WorkflowScheme scheme = new WorkflowScheme();
      IDbCommand command = this.plugin.idb2.DbConnection.CreateCommand();
      string str1 = "";
      string str2 = "";
      if (!PumpWorkflowSettings.HasOption(WFOptions.PumpTerminated))
        str2 += " Status <> 5";
      if (!PumpWorkflowSettings.HasOption(WFOptions.PumpCompleted))
      {
        if (str2 != "")
          str2 += " and ";
        str2 += "Status <> 6";
      }
      if (PumpWorkflowSettings.HasOption(WFOptions.PumpByDateTime))
      {
        if (str2 != "")
          str2 += " and ";
        str2 += "(CreationTime between @p1 and @p2)";
      }
      if (PumpWorkflowSettings.HasOption(WFOptions.PumpSchemes))
        str1 = "(Kind='S') ";
      if (PumpWorkflowSettings.HasOption(WFOptions.PumpProcesses))
      {
        if (str1 != "")
          str1 += " or ";
        string str3 = str1 + "(Kind='P'";
        if (str2 != "")
          str3 = $"{str3} and {str2}";
        str1 = str3 + ")";
      }
      if (str1 != "")
        str1 = " where " + str1;
      this.PumpCheckPoint("Определение количества шаблонов для перекачки", 0);
      command.CommandText = $"select count(*) from {wfTables.Schemes.ToUpper()}{str1}";
      if (PumpWorkflowSettings.HasOption(WFOptions.PumpByDateTime))
        BasePumpHelper.FillQueryParameters(command, (object) PumpWorkflowSettings.StartDT, (object) PumpWorkflowSettings.EndDT);
      int int32 = Convert.ToInt32(command.ExecuteScalar());
      logger.Write($"{command.CommandText}: {int32} result(s)");
      command.CommandText = $"select * from {wfTables.Schemes.ToUpper()}{str1} order by schemeid";
      IDataReader wfSchemesReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
      try
      {
        int index = 1;
        string format = "Перекачка шаблонов ({0} из {1})";
        DataReadResult dataReadResult;
        while ((dataReadResult = this.ReadWorkflowScheme(wfSchemesReader, scheme)) != DataReadResult.NoData)
        {
          this.PumpCheckPoint(string.Format(format, (object) index, (object) int32), this.CalculatePercent(int32, index, 1, 99));
          if (dataReadResult == DataReadResult.OK)
            this.PumpWorkflowScheme(scheme);
          logger.Flush();
          ++index;
        }
      }
      finally
      {
        wfSchemesReader.Close();
      }
      this.PumpCheckPoint("Перекачка данных маршрутизатора успешно завершена", 100);
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
      this._articlesCache.Release();
      this._processesCache.Release();
      this._processesToSkip.Release();
      this._formsCache.Release();
      this._scriptsCache.Release();
      this._schemeCategoriesCache.Release();
      this._subprocessesCache.Release();
    }
  }

  private void DoPumpReferences()
  {
    IImportedObjectList importedObjectList = this.plugin.Idw.CreateImportedObjectList(0);
    this._subprocessesCache = PumpCache.Category[ImportingCategory.Subprocesses];
    this._processesCache = PumpCache.Category[ImportingCategory.Processes];
    try
    {
      this.PumpCheckPoint("Перекачка связей подпроцессов", 0);
      string format = "Перекачка связей подпроцессов ({0} из {1})";
      int index = 1;
      int count = this._subprocessesCache.Items.Count;
      foreach (KeyValuePair<object, DictionaryValue> keyValuePair in this._subprocessesCache.Items)
      {
        if (keyValuePair.Value.NewObjectID != 0L)
        {
          int Hi = 0;
          int Lo = 0;
          BasePumpHelper.ExtractCacheKey(keyValuePair.Value.NewObjectID, out Hi, out Lo);
          long int64 = Convert.ToInt64(keyValuePair.Key);
          long num1 = 0;
          string caption1 = "";
          importedObjectList.UseObject(int64);
          if (Hi > 0)
          {
            DictionaryValue dictionaryValue = this._processesCache.GetValue((object) Hi);
            if (dictionaryValue != null)
            {
              num1 = dictionaryValue.NewObjectID;
              caption1 = dictionaryValue.Caption;
            }
          }
          if (num1 > 0L)
            importedObjectList.AddAttributeLink(wfConsts.AttrSubprocessSchemeID, num1, caption1).IsNew = false;
          long num2 = 0;
          string caption2 = "";
          if (Lo > 0)
          {
            DictionaryValue dictionaryValue = this._processesCache.GetValue((object) Lo);
            if (dictionaryValue != null)
            {
              num2 = dictionaryValue.NewObjectID;
              caption2 = dictionaryValue.Caption;
            }
          }
          if (num2 > 0L)
            importedObjectList.AddAttributeLink(wfConsts.AttrSubprocessID, num2, caption2);
          this._subprocessesCache.SetNewKey(keyValuePair.Key, 0L);
        }
        ++index;
        this.PumpCheckPoint(string.Format(format, (object) index, (object) count), this.CalculatePercent(count, index, 1, 99));
      }
      importedObjectList.Import();
      this.PumpCheckPoint("Перекачка связей подпроцессов", 100);
    }
    finally
    {
      this._subprocessesCache.Release();
      this._processesCache.Release();
    }
  }
}

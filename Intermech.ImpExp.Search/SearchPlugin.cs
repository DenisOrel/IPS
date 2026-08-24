// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.SearchPlugin
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Search;

public class SearchPlugin : PluginClass
{
  internal bool isPump = true;
  public IDataBase idb2;
  internal PumpRankList rankList;
  internal PumpUsers users;
  internal PumpUserGroups userGroups;
  internal PumpArchives archives;
  internal ArchiveSettingsControl archSettings;
  internal ThematicParamsSettingsControl themParmSettings;
  internal PumpMeasures measures;
  internal PumpArticleTypes articleTypes;
  internal PumpThematicParams thematicParams;
  internal PumpThematicParamsGroups thematicParamsGroups;
  internal PumpCompositionAttributes compositionAttributes;
  internal PumpArticleAttributes articleAttributes;
  internal PumpDocTypes docTypes;
  internal PumpExternalProgs externalProgs;
  internal PumpArticleCommonParameters articleCommonParameters;
  internal PumpDocumentsCommonParameters documentsCommonParameters;
  internal string NameSearchIdUser = "SEARCH_ID_USER";
  internal string NameSearchIdUserGroup = "SEARCH_ID_USER_GROUP";
  internal string NameSearchIdArchive = "SEARCH_ID_ARCHIVE";
  internal string NameSearchIdRankList = "SEARCH_ID_RANK_LIST";
  internal string GuidSearchIdUser = "cad00d1a-306c-11d8-b4e9-00304f19f545";
  internal string GuidSearchIdUserGroup = "cad00d1b-306c-11d8-b4e9-00304f19f545";
  internal string GuidSearchIdArchive = "cad00d1c-306c-11d8-b4e9-00304f19f545";
  internal string GuidSearchIdRankList = "cad00d1d-306c-11d8-b4e9-00304f19f545";
  private Dictionary<string, string> _columnsDefValues;
  private string[] _connectInfo;

  public override Guid GUID => new Guid("A7A45959-A30F-4d08-8A1B-3D5D50249A3C");

  public SearchPlugin(IAppManager manager)
    : base(manager)
  {
    this.aliasString = "SEARCH PLUGIN CONNECTION";
    this.rankList = new PumpRankList(this);
    this.users = new PumpUsers(this);
    this.userGroups = new PumpUserGroups(this);
    this.archives = new PumpArchives(this);
    this.measures = new PumpMeasures(this);
    this.articleAttributes = new PumpArticleAttributes(this);
    this.articleTypes = new PumpArticleTypes(this);
    this.thematicParamsGroups = new PumpThematicParamsGroups(this);
    this.thematicParams = new PumpThematicParams(this);
    this.docTypes = new PumpDocTypes(this);
    this.externalProgs = new PumpExternalProgs(this);
    this.compositionAttributes = new PumpCompositionAttributes(this);
    this.articleCommonParameters = new PumpArticleCommonParameters(this);
    this.documentsCommonParameters = new PumpDocumentsCommonParameters(this);
    this.archSettings = new ArchiveSettingsControl(this);
    this.themParmSettings = new ThematicParamsSettingsControl();
  }

  private void AddTasks()
  {
    this.verificationsList.Add(this.rankList.TaskExam);
    this.verificationsList.Add(this.users.TaskExam);
    this.verificationsList.Add(this.userGroups.TaskExam);
    this.verificationsList.Add(this.measures.TaskExam);
    this.verificationsList.Add(this.articleAttributes.TaskExam);
    this.verificationsList.Add(this.articleCommonParameters.TaskExam);
    this.verificationsList.Add(this.documentsCommonParameters.TaskExam);
    this.verificationsList.Add(this.articleTypes.TaskExam);
    this.verificationsList.Add(this.thematicParamsGroups.TaskExam);
    this.verificationsList.Add(this.thematicParams.TaskExam);
    this.verificationsList.Add(this.docTypes.TaskExam);
    this.verificationsList.Add(this.archives.TaskExam);
    this.verificationsList.Add(this.compositionAttributes.TaskExam);
    this.pumpsList.Add(this.rankList.TaskPump);
    this.pumpsList.Add(this.users.TaskPump);
    this.pumpsList.Add(this.userGroups.TaskPump);
    this.pumpsList.Add(this.archives.TaskPump);
    this.pumpsList.Add(this.measures.TaskPump);
    this.pumpsList.Add(this.articleAttributes.TaskPump);
    this.pumpsList.Add(this.articleTypes.TaskPump);
    this.pumpsList.Add(this.docTypes.TaskPump);
    this.pumpsList.Add(this.articleCommonParameters.TaskPump);
    this.pumpsList.Add(this.documentsCommonParameters.TaskPump);
    this.pumpsList.Add(this.externalProgs.TaskPump);
    string str1 = "'PC_PARAMS'";
    string str2 = string.Empty;
    switch (this.imConnection.DataBaseType)
    {
      case "IntermechConnection.MsSQL":
        str2 = $" SELECT O.name AS TABLE_NAME, C.name AS FIELD_NAME, M.text AS DEF_VALUE FROM  syscomments M, syscolumns C, sysobjects O WHERE C.id= O.id AND M.id= C.cdefault AND O.name in ({str1})";
        break;
      case "IntermechConnection.Oracle":
        str2 = $"SELECT A.TABLE_NAME AS TABLE_NAME, A.COLUMN_NAME AS FIELD_NAME, A.DATA_DEFAULT AS DEF_VALUE FROM SYS.ALL_TAB_COLUMNS A WHERE A.TABLE_NAME IN ({str1})";
        break;
      case "IntermechConnection.Interbase":
        str2 = $"SELECT A.RDB$RELATION_NAME AS TABLE_NAME, B.RDB$FIELD_NAME AS FIELD_NAME, B.RDB$DEFAULT_VALUE AS DEF_VALUE FROM RDB$RELATION_FIELDS A, RDB$FIELDS B  WHERE A.RDB$FIELD_SOURCE = B.RDB$FIELD_NAME AND A.RDB$RELATION_NAME IN ({str1})";
        break;
    }
    DataSet dataSet = new DataSet();
    IDataAdapter dataAdapter = (IDataAdapter) this.idb.GetDataAdapter(str2.ToUpper());
    dataAdapter.Fill(dataSet);
    if (dataSet.Tables.Count == 1)
      this._columnsDefValues = new Dictionary<string, string>(dataSet.Tables[0].Rows.Count);
    foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
    {
      string key = $"{Convert.ToString(row["TABLE_NAME"])}.{Convert.ToString(row["FIELD_NAME"])}";
      string str3 = Convert.ToString(row["DEF_VALUE"]);
      if (this.imConnection.DataBaseType == "IntermechConnection.MsSQL" && str3.Length > 2)
      {
        str3 = str3.Substring(1, str3.Length - 2);
        if (str3[0] == '\'')
          str3 = str3.Remove(0, 1);
        if (str3[str3.Length - 1] == '\'')
          str3 = str3.Remove(str3.Length - 1, 1);
      }
      this._columnsDefValues.Add(key, str3);
    }
    if (!(dataAdapter is IDisposable))
      return;
    ((IDisposable) dataAdapter).Dispose();
  }

  public string GetDefaultValue(string tableName, string DBField)
  {
    if (this._columnsDefValues == null || this._columnsDefValues.Count == 0)
      return string.Empty;
    string empty = string.Empty;
    return !this._columnsDefValues.TryGetValue($"{tableName}.{DBField}", out empty) ? string.Empty : empty;
  }

  internal void CheckIdAttribute(string attrName, string attrGuid, FieldTypes fieldType)
  {
    if (this.Imdi.AttributeTypes.ExistsByName(attrName))
      return;
    this.Imdi.AttributeTypes.Add(attrName, attrGuid, fieldType, 0L);
  }

  public override string Name => "INTERMECH Search Plugin";

  public override string Description
  {
    get => "Модуль расширения для перекачки данных из базы INTERMECH Search";
  }

  public override bool BaseConnect()
  {
    this.baseConnect = this.OpenDbConnection(ConnStrType.Search);
    if (this.IsConnected())
    {
      this.idb2 = this.appManager.DBManager.CreateDBConnection(this.idbType, this.aliasString + "_Search");
      IDbConnection dbConnection = this.idb2.DbConnection;
      dbConnection.ConnectionString = SavedConnectionStrings.Items["SEARCH4"].ConnectionString;
      dbConnection.Open();
      this.AddTasks();
    }
    return this.IsConnected();
  }

  public override bool BaseDisconnect()
  {
    bool flag = base.BaseDisconnect();
    if (this.idb2 != null && this.idb2.DbConnection != null)
    {
      if (this.idb2.DbConnection.State != ConnectionState.Closed)
      {
        try
        {
          this.idb2.DbConnection.Close();
        }
        catch
        {
        }
      }
    }
    return flag;
  }

  public override StepControl[] GetSettingsControls()
  {
    return new StepControl[1]
    {
      (StepControl) this.archSettings
    };
  }

  public override string[] ConnectInfo
  {
    get
    {
      if (this._connectInfo == null)
        this._connectInfo = new string[1]
        {
          this.baseConnect.ConnectionString
        };
      return this._connectInfo;
    }
  }
}

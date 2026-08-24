// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.BasePumpHelper
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.SafeDataProxy;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp;

public class BasePumpHelper
{
  protected static IUserSession _session = (IUserSession) null;
  protected static IDbConnection _connection = (IDbConnection) null;
  protected static SimpleLogger _logger = (SimpleLogger) null;
  public static int S4Precision = 3;
  public static IAppManager AppManager = (IAppManager) null;
  protected static CacheCategory _usersCache = (CacheCategory) null;
  protected static CacheCategory _groupsCache = (CacheCategory) null;
  public static readonly Guid AttrSearchIDGuid = new Guid("cad0132b-306c-11d8-b4e9-00304f19f545");
  public static int AttrSearchID;
  public static readonly Guid AttrSearchVersionIDGuid = new Guid("cad007a4-306c-11d8-b4e9-00304f19f545");
  public static int AttrSearchVersionID;
  public static BasePumpHelper.DBType dbType;
  private static Dictionary<int, char> _rankCodes = new Dictionary<int, char>();
  private static CacheCategory _ranksCache = (CacheCategory) null;
  public static Encoding Encoding = Encoding.GetEncoding(1251);
  private static Dictionary<TypeCode, SqlDbType> SQLTypeMapper = new Dictionary<TypeCode, SqlDbType>();
  private static Dictionary<TypeCode, DbType> OracleTypeMapper = new Dictionary<TypeCode, DbType>();
  private static BasePumpHelper.GetParameterDelegate _getParameter = (BasePumpHelper.GetParameterDelegate) null;
  private static Regex _IBRegex = new Regex("@p\\d+", RegexOptions.Compiled);
  public static IDbCommand LastS4Query = (IDbCommand) null;
  private static Dictionary<long, string> _rankCaptionsCache = (Dictionary<long, string>) null;
  public static long CurrentObjectID = 0;
  private static Dictionary<BasePumpHelper.WarningType, List<long>> _warningsDone = new Dictionary<BasePumpHelper.WarningType, List<long>>();
  private static long _lastSchemeID = 0;
  private static long _sessionUserID = 0;
  private static int _packetSize = -2;

  public static SimpleLogger Logger => BasePumpHelper._logger;

  public static CacheCategory UsersCache => BasePumpHelper._usersCache;

  public static CacheCategory GroupsCache => BasePumpHelper._groupsCache;

  public static void Init(PluginClass plugin)
  {
    if (BasePumpHelper._session != null)
      return;
    BasePumpHelper._logger = new SimpleLogger(Path.Combine(Application.StartupPath, "searchDataImport.log"));
    switch (plugin.idbType.DataBaseType())
    {
      case "IntermechConnection.Oracle":
        BasePumpHelper.dbType = BasePumpHelper.DBType.Oracle;
        break;
      case "IntermechConnection.Interbase":
        BasePumpHelper.dbType = BasePumpHelper.DBType.Interbase;
        break;
      default:
        BasePumpHelper.dbType = BasePumpHelper.DBType.MSSQL;
        break;
    }
    switch (BasePumpHelper.dbType)
    {
      case BasePumpHelper.DBType.Oracle:
        BasePumpHelper._getParameter = new BasePumpHelper.GetParameterDelegate(BasePumpHelper.GetOracleParameter);
        break;
      case BasePumpHelper.DBType.MSSQL:
        BasePumpHelper._getParameter = new BasePumpHelper.GetParameterDelegate(BasePumpHelper.GetMSSQLParameter);
        break;
      case BasePumpHelper.DBType.Interbase:
        BasePumpHelper._getParameter = new BasePumpHelper.GetParameterDelegate(BasePumpHelper.GetInterbaseParameter);
        break;
    }
    BasePumpHelper._session = plugin.Idw.GetUserSession();
    BasePumpHelper._connection = plugin.idb.DbConnection;
    BasePumpHelper.AttrSearchID = plugin.Imdi.AttributeTypes.GetByGuid(BasePumpHelper.AttrSearchIDGuid).ID;
    BasePumpHelper.AttrSearchVersionID = plugin.Imdi.AttributeTypes.GetByGuid(BasePumpHelper.AttrSearchVersionIDGuid).ID;
    BasePumpHelper.SQLTypeMapper.Add(TypeCode.Int32, SqlDbType.Int);
    BasePumpHelper.SQLTypeMapper.Add(TypeCode.Int64, SqlDbType.BigInt);
    BasePumpHelper.SQLTypeMapper.Add(TypeCode.String, SqlDbType.NVarChar);
    BasePumpHelper.SQLTypeMapper.Add(TypeCode.DateTime, SqlDbType.DateTime);
    BasePumpHelper.OracleTypeMapper.Add(TypeCode.Int32, DbType.Int32);
    BasePumpHelper.OracleTypeMapper.Add(TypeCode.Int64, DbType.Int64);
    BasePumpHelper.OracleTypeMapper.Add(TypeCode.String, DbType.AnsiString);
    BasePumpHelper.OracleTypeMapper.Add(TypeCode.DateTime, DbType.DateTime);
    BasePumpHelper._ranksCache = PumpCache.Category[ImportingCategory.RankList];
    IDataReader dataReader = BasePumpHelper.S4Query("select RANK_ID,RANK_CODE from RANKLIST");
    try
    {
      while (dataReader.Read())
        BasePumpHelper._rankCodes.Add(Convert.ToInt32(dataReader[0]), Convert.ToChar(dataReader[1]));
    }
    finally
    {
      dataReader.Close();
    }
    BasePumpHelper._usersCache = PumpCache.Category[ImportingCategory.Users];
    BasePumpHelper._groupsCache = PumpCache.Category[ImportingCategory.UserGroups];
    BasePumpHelper._sessionUserID = BasePumpHelper._session.UserID;
  }

  public static IUserSession Session => BasePumpHelper._session;

  public static object SimpleQuery(int objType, int AttrID, object AttrVal)
  {
    return BasePumpHelper.SimpleQuery(objType, AttrID, AttrVal, (object) ObligatoryObjectAttributes.F_OBJECT_ID);
  }

  public static object SimpleQuery(int objType, int AttrID, object AttrVal, object Column)
  {
    DataTable dataTable = BasePumpHelper._session.GetObjectCollection(objType).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(AttrID, RelationalOperators.Equal, AttrVal, LogicalOperators.AND, 0, false)
    }, new object[1]{ Column }, (object[]) null, (SortOrders[]) null));
    return dataTable.Rows.Count > 0 ? dataTable.Rows[0][0] : (object) DBNull.Value;
  }

  public static long SimpleIntQuery(int objType, int AttrID, object AttrVal)
  {
    object obj = BasePumpHelper.SimpleQuery(objType, AttrID, AttrVal);
    return !DBNull.Value.Equals(obj) ? Convert.ToInt64(obj) : 0L;
  }

  public static DataTable SimpleQuery(int objType, int AttrID, object AttrVal, object[] Columns)
  {
    return BasePumpHelper._session.GetObjectCollection(objType).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(AttrID, RelationalOperators.Equal, AttrVal, LogicalOperators.AND, 0, false)
    }, Columns, (object[]) null, (SortOrders[]) null));
  }

  public static string BlobToString(object value)
  {
    return value is byte[] bytes && (bytes.Length != 1 || bytes[0] != (byte) 0) ? BasePumpHelper.Encoding.GetString(bytes) : "";
  }

  public static string CommaStringToString(string s)
  {
    if (s == null)
      return "";
    return new StringList() { CommaText = s }.Text;
  }

  public static int StrToIntDef(string s, int def)
  {
    try
    {
      return Convert.ToInt32(s);
    }
    catch
    {
      return def;
    }
  }

  public static string ToString(object obj)
  {
    return DBNull.Value.Equals(obj) ? string.Empty : Convert.ToString(obj);
  }

  public static int ToInt32(object obj) => DBNull.Value.Equals(obj) ? 0 : Convert.ToInt32(obj);

  public static double ToDouble(object obj)
  {
    return DBNull.Value.Equals(obj) ? 0.0 : Convert.ToDouble(obj);
  }

  public static void FixDateTimeField(ref object fldvalue)
  {
    if (!(fldvalue is DateTime))
      return;
    if (((DateTime) fldvalue).Year == 1899)
      fldvalue = (object) null;
    else
      fldvalue = (object) ((DateTime) fldvalue).ToUniversalTime();
  }

  public static void AddDBValueToDictionary(
    Dictionary<string, object> dict,
    string name,
    object value)
  {
    if (value is DateTime dateTime && dateTime.TimeOfDay != new TimeSpan(0L))
      BasePumpHelper.FixDateTimeField(ref value);
    dict.Add(name, value);
  }

  public static void AddDBValueToDictionary(
    Dictionary<string, object> dict,
    IDataReader reader,
    int fieldIndex)
  {
    Type fieldType = reader.GetFieldType(fieldIndex);
    object obj = (object) null;
    if (Type.GetTypeCode(fieldType) == TypeCode.Double)
    {
      if (!reader.IsDBNull(fieldIndex))
        obj = (object) Math.Round(BasePumpHelper.ToDouble(reader[fieldIndex]), BasePumpHelper.S4Precision);
    }
    else
      obj = reader.GetValue(fieldIndex);
    BasePumpHelper.AddDBValueToDictionary(dict, reader.GetName(fieldIndex).ToLower(), obj);
  }

  public static void ReaderRowToS4Table(IDataReader reader, S4Table tab, bool juggleTypes)
  {
    BasePumpHelper.ReaderRowToS4Table(reader, tab, (List<string>) null, juggleTypes);
  }

  public static void ReaderRowToS4Table(
    IDataReader reader,
    S4Table tab,
    List<string> exceptFieldNames,
    bool juggleTypes)
  {
    if (juggleTypes)
      reader.GetSchemaTable();
    for (int index = 0; index < reader.FieldCount; ++index)
    {
      string lower = reader.GetName(index).ToLower();
      if (exceptFieldNames == null || !exceptFieldNames.Contains(lower))
      {
        try
        {
          if (juggleTypes)
            BasePumpHelper.AddDBValueToDictionary((Dictionary<string, object>) tab, reader, index);
          else
            BasePumpHelper.AddDBValueToDictionary((Dictionary<string, object>) tab, lower, reader[index]);
        }
        catch (Exception ex)
        {
          if (BasePumpHelper._logger != null)
            BasePumpHelper._logger.Write($"Exception in ReaderRowToS4Table [{lower}]: {ex.Message}\r\n{BasePumpHelper.LastS4Query.CommandText}\r\n");
          tab.Add(lower, (object) null);
        }
      }
    }
  }

  public static long MakeCacheKey(int ProjID, int PartID)
  {
    return ((long) ProjID << 32 /*0x20*/) + (long) PartID;
  }

  public static string MakeCacheKey(long i, long j) => $"{i:x},{j:x}";

  public static void ExtractCacheKey(long Key, out int Hi, out int Lo)
  {
    Hi = (int) (Key >> 32 /*0x20*/);
    Lo = (int) (Key & (long) uint.MaxValue);
  }

  public static long MakeCacheKey2(int hi, int lo, int short_lo)
  {
    return (long) hi << 32 /*0x20*/ ^ (long) (lo << 8) ^ (long) short_lo;
  }

  public static void ExtractCacheKey2(long key, out int hi, out int lo, out int short_lo)
  {
    hi = (int) (key >> 32 /*0x20*/);
    int num = (int) (key & (long) uint.MaxValue);
    lo = num >> 8;
    short_lo = num & (int) byte.MaxValue;
  }

  private static IDbDataParameter GetMSSQLParameter(object o)
  {
    TypeCode typeCode = Type.GetTypeCode(o.GetType());
    SqlDbType sqlDbType = SqlDbType.Variant;
    if (!BasePumpHelper.SQLTypeMapper.TryGetValue(typeCode, out sqlDbType))
      return (IDbDataParameter) null;
    SqlParameter mssqlParameter = new SqlParameter();
    mssqlParameter.SqlDbType = sqlDbType;
    mssqlParameter.Value = o;
    return (IDbDataParameter) mssqlParameter;
  }

  private static IDbDataParameter GetOracleParameter(object o)
  {
    TypeCode typeCode = Type.GetTypeCode(o.GetType());
    DbType dbType = DbType.String;
    if (!BasePumpHelper.OracleTypeMapper.TryGetValue(typeCode, out dbType))
      return (IDbDataParameter) null;
    OracleParameter oracleParameter = new OracleParameter();
    oracleParameter.DbType = dbType;
    oracleParameter.Value = o;
    return (IDbDataParameter) oracleParameter;
  }

  private static IDbDataParameter GetInterbaseParameter(object o)
  {
    TypeCode typeCode = Type.GetTypeCode(o.GetType());
    DbType dbType = DbType.String;
    if (!BasePumpHelper.OracleTypeMapper.TryGetValue(typeCode, out dbType))
      return (IDbDataParameter) null;
    OleDbParameter interbaseParameter = new OleDbParameter();
    interbaseParameter.DbType = dbType;
    interbaseParameter.Value = o;
    interbaseParameter.SourceColumn = (string) null;
    return (IDbDataParameter) interbaseParameter;
  }

  private static IDbDataParameter GetSQLParameter(object o)
  {
    return BasePumpHelper._getParameter != null ? BasePumpHelper._getParameter(o) : (IDbDataParameter) null;
  }

  public static void NormalizeCommandText(IDbCommand cmd)
  {
    switch (BasePumpHelper.dbType)
    {
      case BasePumpHelper.DBType.Oracle:
        cmd.CommandText = cmd.CommandText.Replace("@", ":");
        break;
      case BasePumpHelper.DBType.Interbase:
        cmd.CommandText = BasePumpHelper._IBRegex.Replace(cmd.CommandText, "?");
        break;
    }
  }

  public static void FillQueryParameters(IDbCommand cmd, params object[] parameters)
  {
    if (parameters != null && parameters.Length != 0)
      BasePumpHelper.NormalizeCommandText(cmd);
    if (parameters == null)
      return;
    for (int index = 0; index < parameters.Length; ++index)
      BasePumpHelper.AddQueryParameter(cmd, parameters[index]);
  }

  public static void AddQueryParameter(IDbCommand cmd, object value)
  {
    IDbDataParameter sqlParameter = BasePumpHelper.GetSQLParameter(value);
    if (sqlParameter == null)
      return;
    int num = cmd.Parameters.Add((object) sqlParameter);
    sqlParameter.ParameterName = "p" + (num + 1).ToString();
  }

  public static IDataReader S4Query(IDbConnection conn, string cmdtext, params object[] parameters)
  {
    return BasePumpHelper.S4Query(conn, cmdtext, CommandBehavior.Default, parameters);
  }

  public static IDataReader S4Query(
    IDbConnection conn,
    string cmdtext,
    CommandBehavior behaviour,
    params object[] parameters)
  {
    IDbCommand command = conn.CreateCommand();
    if ((command is IDbCommandProxy dbCommandProxy ? dbCommandProxy.Target : command) is OracleCommand oracleCommand)
    {
      oracleCommand.BindByName = true;
      oracleCommand.FetchSize = 262144L /*0x040000*/;
      oracleCommand.InitialLOBFetchSize = 262144 /*0x040000*/;
      oracleCommand.InitialLONGFetchSize = -1;
    }
    command.CommandText = cmdtext;
    BasePumpHelper.FillQueryParameters(command, parameters);
    IDataReader dataReader = command.ExecuteReader(behaviour);
    BasePumpHelper.LastS4Query = command;
    return dataReader;
  }

  public static IDataReader S4Query(string cmdtext)
  {
    return BasePumpHelper.S4Query(BasePumpHelper._connection, cmdtext, (object[]) null);
  }

  public static int S4NonQuery(string cmdtext, params object[] parameters)
  {
    IDbCommand command = BasePumpHelper._connection.CreateCommand();
    command.CommandText = cmdtext;
    command.CommandTimeout = (ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.CommandTimeout;
    BasePumpHelper.FillQueryParameters(command, parameters);
    int num = command.ExecuteNonQuery();
    BasePumpHelper.LastS4Query = command;
    return num;
  }

  public static IDataReader S4Query(string cmdtext, params object[] parameters)
  {
    return BasePumpHelper.S4Query(BasePumpHelper._connection, cmdtext, parameters);
  }

  public static int S4IntQuery(string cmdtext)
  {
    using (IDataReader dataReader = BasePumpHelper.S4Query(cmdtext))
      return dataReader.Read() ? Convert.ToInt32(dataReader[0]) : 0;
  }

  public static object S4ObjectQuery(string cmdtext, params object[] parameters)
  {
    using (IDataReader dataReader = BasePumpHelper.S4Query(cmdtext, parameters))
      return dataReader.Read() ? dataReader[0] : (object) null;
  }

  public static char GetRankCode(int rankID)
  {
    char rankCode = Convert.ToChar(0);
    BasePumpHelper._rankCodes.TryGetValue(rankID, out rankCode);
    return rankCode;
  }

  public static CacheCategory RanksCache => BasePumpHelper._ranksCache;

  public static long GetNewRankID(char rankCode)
  {
    return BasePumpHelper._ranksCache.GetNewKey((object) rankCode);
  }

  public static long GetNewRankID(int oldRankID)
  {
    char rankCode = BasePumpHelper.GetRankCode(oldRankID);
    return rankCode > char.MinValue ? BasePumpHelper.GetNewRankID(rankCode) : 0L;
  }

  public static string GetNewRankCaption(long rankID)
  {
    if (BasePumpHelper._rankCaptionsCache == null)
    {
      BasePumpHelper._rankCaptionsCache = new Dictionary<long, string>();
      IDBObjectCollection objectCollection = BasePumpHelper._session.GetObjectCollection(BasePumpHelper._session.IdentHelper.RanksTypeID);
      DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new object[2]
      {
        (object) -2,
        (object) -50
      });
      foreach (DataRow row in (InternalDataCollectionBase) objectCollection.Select(paramSet).Rows)
      {
        string str = "";
        if (!DBNull.Value.Equals(row[1]))
          str = row[1].ToString();
        BasePumpHelper._rankCaptionsCache.Add(Convert.ToInt64(row[0]), str);
      }
    }
    string newRankCaption;
    BasePumpHelper._rankCaptionsCache.TryGetValue(rankID, out newRankCaption);
    return newRankCaption;
  }

  public static long GetNewUserID(int oldID)
  {
    long newKey = BasePumpHelper._usersCache.GetNewKey((object) oldID);
    if (newKey > 0L)
      return newKey;
    BasePumpHelper.AddWarning(BasePumpHelper.WarningType.User, "Пользователь с идентификатором \"{0}\" в БД Search не найден", (long) oldID);
    return 0;
  }

  public static long GetNewGroupID(int oldID)
  {
    long newKey = BasePumpHelper._groupsCache.GetNewKey((object) oldID);
    if (newKey > 0L)
      return newKey;
    BasePumpHelper.AddWarning(BasePumpHelper.WarningType.Group, "Группа пользователей с идентификатором \"{0}\" в БД Search не найдена", (long) oldID);
    return 0;
  }

  public static void AddWarning(BasePumpHelper.WarningType type, string warning, long id)
  {
    List<long> longList = (List<long>) null;
    if (BasePumpHelper.CurrentObjectID != BasePumpHelper._lastSchemeID)
    {
      BasePumpHelper._warningsDone.Clear();
      BasePumpHelper._lastSchemeID = BasePumpHelper.CurrentObjectID;
    }
    else if (BasePumpHelper._warningsDone.TryGetValue(type, out longList) && longList.Contains(id))
      return;
    if (longList == null)
    {
      longList = new List<long>();
      BasePumpHelper._warningsDone.Add(type, longList);
    }
    longList.Add(id);
    BasePumpHelper.AppManager.AddWarningMessage(string.Format("[PID={0}] " + string.Format(warning, (object) id), (object) BasePumpHelper.CurrentObjectID));
  }

  public static long SessionUserID => BasePumpHelper._sessionUserID;

  public static int PacketSize
  {
    get
    {
      if (BasePumpHelper._packetSize == -2)
        BasePumpHelper._packetSize = (ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.PacketSize;
      return BasePumpHelper._packetSize;
    }
  }

  public enum DBType
  {
    Unknown,
    Oracle,
    MSSQL,
    Interbase,
  }

  private delegate IDbDataParameter GetParameterDelegate(object o);

  public enum WarningType
  {
    Document,
    Article,
    User,
    Group,
    Attribute,
    Archive,
  }
}

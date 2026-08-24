// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IMConnection
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Microsoft.Win32;
using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Сервисный класс для облегчения работы по подключению к старым базам INTERMECH
/// </summary>
public class IMConnection
{
  /// <summary>Псевдоним для типа подключения к СУБД Interbase</summary>
  public const string Interbase = "IntermechConnection.Interbase";
  /// <summary>Псевдоним для типа подключения к СУБД MSSQL</summary>
  public const string MsSQL = "IntermechConnection.MsSQL";
  /// <summary>Псевдоним для типа подключения к СУБД Oracle</summary>
  public const string Oracle = "IntermechConnection.Oracle";
  /// <summary>Псевдоним типа подключения запрошенной базы</summary>
  public string DataBaseType = "";
  /// <summary>
  /// Строка соединения, используемая для создания подключения к запрошенной базе
  /// (имя пользователя и пароль заданы как позиции для замены функцией
  /// String.Format(), т.е. как "{0}" и "{1}" соответственнно )
  /// </summary>
  internal string ConnectionString = "";
  internal string DBName = "";
  /// <summary>Псевдоним подключения к базе</summary>
  protected string dbAlias = "";

  /// <summary>Вызов формы для ввода имени пользоваиеля и его пароля</summary>
  /// <param name="userName">Имя пользователя</param>
  /// <param name="password">Пароль для подключения</param>
  /// <param name="caption">Заголовок формы</param>
  /// <returns>true - если ввод информации подтвержден пользователем,
  /// false - если пользователь отменил ввод инфомации</returns>
  public static bool Login(ref string userName, ref string password, string caption)
  {
    return LoginForm.ShowLogin(ref userName, ref password, caption);
  }

  /// <summary>
  /// Формирование строки подключения к базе данных, используя заданное
  /// имя пользователя и его пароль
  /// </summary>
  /// <param name="userName">Имя пльзователя</param>
  /// <param name="password">Пароль пользователя</param>
  /// <returns>строка для подключения к базе</returns>
  public string GetConnectionString(string userName, string password)
  {
    return string.Format(this.ConnectionString, (object) userName, (object) password);
  }

  public string Alias => this.dbAlias;

  /// <summary>Имя базы, выводимое пользователю</summary>
  public string Name
  {
    get
    {
      string alias = this.Alias;
      if (this.DBName != "")
        alias += $" ({this.DBName})";
      return alias;
    }
  }

  /// <summary>Конструктор</summary>
  /// <param name="strType">один из допустимых типов подключения к базам ИНТЕРМЕХ</param>
  public IMConnection(ConnStrType strType)
    : this(strType, (ReadIniDelegate) null)
  {
  }

  public IMConnection(string Alias, string dbName, string dsString, string aliasType)
  {
    this.Init(dbName, dsString, aliasType, Alias);
  }

  public void Init(string dbName, string dsString, string aliasType, string newAlias)
  {
    switch (aliasType.ToUpper())
    {
      case "INTRBASE":
        string str = "LCPI.IBProvider";
        DataTable elements = new OleDbEnumerator().GetElements();
        bool flag = false;
        foreach (DataRow row in (InternalDataCollectionBase) elements.Rows)
        {
          if (Convert.ToString(row["SOURCES_NAME"]).IndexOf(str) >= 0)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          throw new Exception("В системе не найден провайдер IBProvider для работы с БД Interbase. Для дальнейшей работы необходимо установить IBProvider из папки Files дистрибутива IPS.");
        this.DataBaseType = "IntermechConnection.Interbase";
        OleDbConnectionStringBuilder connectionStringBuilder = new OleDbConnectionStringBuilder();
        connectionStringBuilder.Provider = str;
        connectionStringBuilder.Add("Location", (object) dsString);
        connectionStringBuilder.Add("User ID", (object) "{0}");
        connectionStringBuilder.Add("Password", (object) "{1}");
        connectionStringBuilder.Add("ctype", (object) "win1251");
        this.ConnectionString = connectionStringBuilder.ToString();
        this.ConnectionString += ";auto_commit=true";
        if (TraceSupport.PluginConnections.Enabled)
        {
          Trace.WriteLine("IMConnection: Init(): switch = INTRBASE");
          break;
        }
        break;
      case "ORACLE":
        this.DataBaseType = "IntermechConnection.Oracle";
        this.ConnectionString = $"Data Source={dsString};User ID={{0}};Password={{1}};Min Pool Size=5";
        if (TraceSupport.PluginConnections.Enabled)
        {
          Trace.WriteLine("IMConnection: Init(): switch = ORACLE");
          break;
        }
        break;
      case "MSSQL":
        this.DataBaseType = "IntermechConnection.MsSQL";
        this.ConnectionString = $"Server={dsString};Database={dbName};User ID={{0}};Password={{1}};";
        if (TraceSupport.PluginConnections.Enabled)
        {
          Trace.WriteLine("IMConnection: Init(): switch = MSSQL");
          break;
        }
        break;
      default:
        this.DataBaseType = "";
        this.ConnectionString = "";
        if (TraceSupport.PluginConnections.Enabled)
        {
          Trace.WriteLine("IMConnection: Init(): switch = default");
          break;
        }
        break;
    }
    this.DBName = dbName;
    Trace.WriteLine($"IMConnection: Init(): ConnectionString = {this.ConnectionString}");
    if (!(newAlias != ""))
      return;
    this.dbAlias = newAlias;
  }

  /// <summary>
  /// Поиск ini-файла.
  /// Сначала производится поиск файла в папке с программой перекачки и если не находим, то
  /// производится поиск через реестр
  /// </summary>
  /// <param name="strType">один из допустимых типов подключения к базам ИНТЕРМЕХ</param>
  /// <returns></returns>
  private string FindIniFile(ConnStrType strType, out bool fromRegistry)
  {
    string path1 = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), strType == ConnStrType.Imbase ? "IMBASE.INI" : "SEARCH4.INI");
    if (File.Exists(path1))
    {
      fromRegistry = false;
      return path1;
    }
    RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Intermech");
    string str = registryKey != null ? (string) registryKey.GetValue("IM_Dir") : (string) null;
    if (str == null)
      throw new Exception("В реестре отсутствует параметр HKEY_LOCAL_MACHINE\\SOFTWARE\\Intermech\\IM_Dir");
    string path2 = str + (strType == ConnStrType.Imbase ? "\\IM-BASE\\IMBASE.INI" : "\\SEARCH\\SEARCH4.INI");
    if (!File.Exists(path2))
      throw new Exception($"Не найден ini-файл {path2}");
    fromRegistry = true;
    return path2;
  }

  /// <summary>Конструктор</summary>
  /// <param name="strType">один из допустимых типов подключения к базам ИНТЕРМЕХ</param>
  /// <param name="readIniFunc">Функция для зачитки доп. параметров из INI файла</param>
  public IMConnection(ConnStrType strType, ReadIniDelegate readIniFunc)
  {
    bool fromRegistry = false;
    IniFile ini = new IniFile(this.FindIniFile(strType, out fromRegistry));
    switch (strType)
    {
      case ConnStrType.Search:
        this.dbAlias = "SEARCH4";
        break;
      case ConnStrType.Docums:
        this.dbAlias = "DOCUMS4";
        break;
      case ConnStrType.Imbase:
        if (fromRegistry)
        {
          RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Intermech\\Imbase\\3.0\\BDE");
          this.dbAlias = registryKey != null ? (string) registryKey.GetValue(nameof (Alias)) : "IMBASE";
          break;
        }
        this.dbAlias = "IMBASE";
        break;
    }
    string dbName = ini.IniReadValue("DATABASE NAME", this.dbAlias);
    string str = ini.IniReadValue("ALIASES", this.dbAlias);
    string aliasType = ini.IniReadValue("ALIAS_TYPES", this.dbAlias);
    if (aliasType.ToUpper() == "INTRBASE" && str.IndexOf(":") == -1 && File.Exists(str))
      str = "localhost:" + str;
    if (TraceSupport.PluginConnections.Enabled)
    {
      Trace.WriteLine($"new IMConnection(): iniFile = {ini.Path}");
      Trace.WriteLine($"new IMConnection(): dbName = {dbName}");
      Trace.WriteLine($"new IMConnection(): dsString = {str}");
      Trace.WriteLine($"new IMConnection(): aliasType = {aliasType}");
    }
    this.Init(dbName, str, aliasType, "");
    if (readIniFunc == null)
      return;
    if (TraceSupport.PluginConnections.Enabled)
      Trace.WriteLine($"new IMConnection(): before readIniFunc. ConnectionString = {this.ConnectionString}");
    readIniFunc(ini);
    if (!TraceSupport.PluginConnections.Enabled)
      return;
    Trace.WriteLine($"new IMConnection(): after readIniFunc. ConnectionString = {this.ConnectionString}");
  }
}

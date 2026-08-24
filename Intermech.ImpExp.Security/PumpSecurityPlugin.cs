// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Security.PumpSecurityPlugin
// Assembly: Intermech.ImpExp.Security, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B4185E78-CFCB-46F6-B1BC-486522A5A9AE
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Security.dll

using Intermech.ImpExp.Interface;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Security;

public class PumpSecurityPlugin : PluginClass
{
  internal IDataBase idb2;

  public PumpSecurityPlugin(IAppManager manager)
    : base(manager)
  {
    this.aliasString = "SEARCH PLUGIN CONNECTION";
    BasePumpHelper.AppManager = manager;
  }

  public override string Name => "INTERMECH Search Data Pump Plugin";

  public override string Description
  {
    get => "Модуль расширения для перекачки прав доступа из базы INTERMECH Search";
  }

  public override bool BaseConnect()
  {
    int num = base.BaseConnect() ? 1 : 0;
    if (num == 0)
      return num != 0;
    this.idb2 = this.appManager.DBManager.CreateDBConnection(this.idbType, this.aliasString + "_Security");
    IDbConnection dbConnection = this.idb2.DbConnection;
    dbConnection.ConnectionString = SavedConnectionStrings.Items["SEARCH4"].ConnectionString;
    dbConnection.Open();
    BasePumpHelper.Init((PluginClass) this);
    this.pumpsList.Add(new PumpAccessRights(this).TaskPump);
    return num != 0;
  }

  public override bool BaseDisconnect()
  {
    int num = base.BaseDisconnect() ? 1 : 0;
    if (this.idb2 == null)
      return num != 0;
    if (this.idb2.DbConnection == null)
      return num != 0;
    if (this.idb2.DbConnection.State == ConnectionState.Closed)
      return num != 0;
    this.idb2.DbConnection.Close();
    return num != 0;
  }
}

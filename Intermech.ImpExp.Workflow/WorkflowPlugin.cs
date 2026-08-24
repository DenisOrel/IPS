// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.WorkflowPlugin
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using Intermech.Expert;
using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using Intermech.Workflow;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Workflow;

public class WorkflowPlugin : PluginClass
{
  private PumpWorkflowClass _pump;
  private PumpSchemeCategories _cpump;
  internal IDataBase idb2;

  public WorkflowPlugin(IAppManager manager)
    : base(manager)
  {
    this.aliasString = "SEARCH PLUGIN CONNECTION";
    BasePumpHelper.AppManager = manager;
    this._cpump = new PumpSchemeCategories(this);
    this.verificationsList.Add(this._cpump.TaskExam);
    this.pumpsList.Add(this._cpump.TaskPump);
    this._pump = new PumpWorkflowClass(this);
    this.verificationsList.Add(this._pump.TaskExam);
    this.pumpsList.Add(this._pump.TaskPump);
    if (ExpertConsts.Consts != null)
      return;
    ExpertConsts.Init(this.Idw.GetUserSession());
  }

  public override string Name => "INTERMECH Workflow Pump Plugin";

  public override string Description
  {
    get => "Модуль расширения для перекачки данных маршрутизатора из базы INTERMECH Search";
  }

  public override bool BaseConnect()
  {
    int num = base.BaseConnect() ? 1 : 0;
    if (num != 0)
    {
      this.idb2 = this.appManager.DBManager.CreateDBConnection(this.idbType, this.aliasString + "_WF");
      IDbConnection dbConnection = this.idb2.DbConnection;
      dbConnection.ConnectionString = SavedConnectionStrings.Items["SEARCH4"].ConnectionString;
      dbConnection.Open();
    }
    IUserSession userSession = this.Idw.GetUserSession();
    if (userSession == null)
      return num != 0;
    wfConsts.Init(userSession);
    wfTables.Init(userSession);
    PumpHelper.Init(this);
    VarsPump.Init(this, userSession);
    return num != 0;
  }
}

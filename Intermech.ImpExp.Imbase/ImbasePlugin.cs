// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ImbasePlugin
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.Controls;
using Intermech.ImpExp.Imbase.ImbaseMeasuresSettings;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace Intermech.ImpExp.Imbase;

internal sealed class ImbasePlugin : PluginClass
{
  public ImbasePumpServiceImpl imService = ImbasePumpServiceImpl.ImbasePumpService;
  public PumpImbaseLookup imbaseLookups;
  public PumpImbaseFields imbaseFields;
  public PumpImbaseTables imbaseTables;
  public PumpImbaseCatalogs imbaseCatalogs;
  public PumpImbaseDataTables imbaseDataTables;
  public PumpObjectPoperties imbaseObjectPoperties;
  public PumpRebuildKeys rebuildKeys;
  public CatalogBindingControl catalogBindingControl;
  public ImbaseSettingsMeasures imbaseSettingsMeasures;
  public ImbaseAttributesControl attributesControl;
  public PumpImbaseBlobs imbaseBlobs;
  private PumpFolderFilters folderFilters;
  private PumpImbaseLinks imbaseLinks;
  private PumpDataTableBlobs tableBlobs;
  private PumpTablesMixData tablesMixData;
  public static SelectCatalogForm selectCatalogsForm = new SelectCatalogForm();
  public static List<int> EnableBlobs;
  private static List<string> EnableTables;
  private static List<string> EnableCatalogs;
  private static List<int> EnableFields;
  public IDataBase idb2;
  private string[] _connectInfo;

  public static bool IsBlobToPump(int blobID)
  {
    return ImbasePlugin.EnableBlobs == null || ImbasePlugin.EnableBlobs.IndexOf(blobID) >= 0;
  }

  public static bool IsTableToPump(string tableName)
  {
    return ImbasePlugin.EnableTables == null || ImbasePlugin.EnableTables.IndexOf(tableName) >= 0;
  }

  public static bool IsCatalogToPump(string tableName)
  {
    if (tableName.ToLower() == "ctl000054")
      return false;
    return ImbasePlugin.EnableCatalogs == null || ImbasePlugin.EnableCatalogs.IndexOf(tableName) >= 0;
  }

  public static bool IsFieldPump(int fieldID)
  {
    return ImbasePlugin.EnableFields == null || ImbasePlugin.EnableFields.IndexOf(fieldID) >= 0;
  }

  public override Guid GUID => new Guid("F0B080F4-E23F-495e-A9B7-6087FDD5BBFE");

  public ImbasePlugin(IAppManager manager)
    : base(manager)
  {
    ImbasePumpServiceImpl.imPlugin = this;
    ImbaseIDHelper.Initialize(this.Imdi);
    this.aliasString = "IMBASE PLUGIN CONNECTION";
    this.imbaseLookups = new PumpImbaseLookup(this);
    this.imbaseTables = new PumpImbaseTables(this);
    this.imbaseObjectPoperties = new PumpObjectPoperties(this);
    this.rebuildKeys = new PumpRebuildKeys(this);
    this.imbaseFields = new PumpImbaseFields(this);
    this.imbaseDataTables = new PumpImbaseDataTables(this);
    this.tablesMixData = new PumpTablesMixData(this);
    this.imbaseCatalogs = new PumpImbaseCatalogs(this);
    this.folderFilters = new PumpFolderFilters(this);
    this.imbaseLinks = new PumpImbaseLinks(this);
    this.imbaseBlobs = new PumpImbaseBlobs(this);
    this.tableBlobs = new PumpDataTableBlobs(this);
    this.imbaseSettingsMeasures = new ImbaseSettingsMeasures(this);
    this.catalogBindingControl = new CatalogBindingControl();
    this.attributesControl = new ImbaseAttributesControl(this);
  }

  private void SetMigrateCatalogs()
  {
    MigrateCatalogFilter migrateCatalogFilter = new MigrateCatalogFilter();
    if (!migrateCatalogFilter.ReadMigrateCatalogsList(this.baseConnect))
      return;
    ImbasePlugin.EnableCatalogs = migrateCatalogFilter.EnableCatalogs;
    ImbasePlugin.EnableTables = migrateCatalogFilter.EnableTables;
    ImbasePlugin.EnableFields = migrateCatalogFilter.EnableFields;
  }

  private void AddTasks()
  {
    this.verificationsList.Add(this.imbaseLookups.TaskExam);
    this.verificationsList.Add(this.imbaseTables.TaskExam);
    this.verificationsList.Add(this.imbaseFields.TaskExam);
    this.verificationsList.Add(this.imbaseObjectPoperties.TaskExam);
    this.pumpsList.Add(this.imbaseBlobs.TaskPump);
    this.pumpsList.Add(this.imbaseDataTables.TaskPump);
    this.pumpsList.Add(this.tablesMixData.TaskPump);
    this.pumpsList.Add(this.imbaseObjectPoperties.TaskPump);
    this.pumpsList.Add(this.imbaseCatalogs.TaskPump);
    this.pumpsList.Add(this.imbaseLinks.TaskPump);
    this.pumpsList.Add(this.tableBlobs.TaskPump);
    this.pumpsList.Add(this.folderFilters.TaskPump);
    this.finalPumpsList.Add(this.rebuildKeys.TaskPump);
    this.SetMigrateCatalogs();
  }

  public override string Name => "INTERMECH Imbase Plugin";

  public override string Description
  {
    get => "Модуль расширения для перекачки данных из базы INTERMECH  Imbase";
  }

  public override StepControl[] GetSettingsControls()
  {
    return new StepControl[3]
    {
      (StepControl) this.imbaseSettingsMeasures,
      (StepControl) this.catalogBindingControl,
      (StepControl) this.attributesControl
    };
  }

  public override bool BaseConnect()
  {
    if (TraceSupport.PluginConnections.Enabled)
      Trace.WriteLine("ImbasePlugin.BaseConnect(): start");
    this.baseConnect = this.OpenDbConnection(ConnStrType.Imbase);
    if (TraceSupport.PluginConnections.Enabled)
      Trace.WriteLine($"ImbasePlugin.BaseConnect(): baseConnect {(this.baseConnect != null ? (object) "not null" : (object) "is null")}");
    if (TraceSupport.PluginConnections.Enabled && this.baseConnect != null)
      Trace.WriteLine($"ImbasePlugin.BaseConnect(): baseConnect.State = {this.baseConnect.State}");
    if (this.IsConnected())
    {
      this.idb2 = this.appManager.DBManager.CreateDBConnection(this.idbType, this.aliasString + "_Imbase");
      IDbConnection dbConnection = this.idb2.DbConnection;
      dbConnection.ConnectionString = SavedConnectionStrings.Items["IMBASE"].ConnectionString;
      dbConnection.Open();
      this.AddTasks();
    }
    return this.IsConnected();
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

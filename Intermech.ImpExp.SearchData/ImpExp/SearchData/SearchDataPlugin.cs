// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.SearchDataPlugin
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace Intermech.ImpExp.SearchData;

public class SearchDataPlugin : PluginClass
{
  internal SearchDataSettingsControl _settingsControl;
  internal ProductionListsSettingsControl ProductionListsSettingsControl;
  internal IDataBase idb2;
  internal IDataBase idb3;
  internal Dictionary<string, Intermech.ImpExp.SearchData.AliasInfo> AliasInfo = new Dictionary<string, Intermech.ImpExp.SearchData.AliasInfo>();
  internal bool PumpBlobsInParallel = true;
  private static Bitmap _infoImage;
  private static Bitmap _warningImage;
  private string _blobPath;
  private BinaryFormatter _formatter;
  internal BlobThread BlobThread;

  public SearchDataPlugin(IAppManager manager)
    : base(manager)
  {
    this.aliasString = "SEARCH PLUGIN CONNECTION";
    BasePumpHelper.AppManager = manager;
  }

  public override string Name => "INTERMECH Search Data Pump Plugin";

  public override string Description
  {
    get => "Модуль расширения для перекачки изделий и документов из базы INTERMECH Search";
  }

  private IDataBase CreateAdditionalConnection(string dbAliasSuffix)
  {
    IDataBase dbConnection1 = this.appManager.DBManager.CreateDBConnection(this.idbType, this.aliasString + dbAliasSuffix);
    IDbConnection dbConnection2 = dbConnection1.DbConnection;
    dbConnection2.ConnectionString = SavedConnectionStrings.Items["SEARCH4"].ConnectionString;
    dbConnection2.Open();
    return dbConnection1;
  }

  private void ReadIni(IniFile ini)
  {
    BasePumpHelper.S4Precision = Convert.ToInt32(ini.IniReadValue("COMMON", "Precision", "3"));
    foreach (string Key in ini.ReadSection("ALIASES"))
    {
      if (!this.AliasInfo.ContainsKey(Key.ToLower()))
      {
        Intermech.ImpExp.SearchData.AliasInfo aliasInfo1 = new Intermech.ImpExp.SearchData.AliasInfo();
        aliasInfo1[AliasData.Alias] = Key;
        aliasInfo1[AliasData.DBString] = ini.IniReadValue("ALIASES", Key);
        aliasInfo1[AliasData.Type] = ini.IniReadValue("ALIAS_TYPES", Key);
        aliasInfo1[AliasData.DBName] = ini.IniReadValue("Database Name", Key);
        aliasInfo1[AliasData.FilePath] = "";
        Intermech.ImpExp.SearchData.AliasInfo aliasInfo2 = aliasInfo1;
        this.AliasInfo.Add(Key.ToLower(), aliasInfo2);
      }
    }
  }

  public override bool BaseConnect()
  {
    this.baseConnect = this.OpenDbConnection(ConnStrType.Search, new ReadIniDelegate(this.ReadIni));
    int num1 = this.IsConnected() ? 1 : 0;
    if (num1 == 0)
      return num1 != 0;
    this.idb2 = this.CreateAdditionalConnection("_SearchData");
    this.idb3 = this.CreateAdditionalConnection("_SearchData2");
    PumpHelper.Init(this);
    ISavePoint service1 = ServicesManager.GetService(typeof (ISavePoint)) as ISavePoint;
    int num2 = service1.IsResumeMode(service1.GetSavePoint()) ? 1 : 0;
    PumpFormsClass pumpFormsClass = new PumpFormsClass(this);
    this.verificationsList.Add(pumpFormsClass.TaskExam);
    this.pumpsList.Add(pumpFormsClass.TaskPump);
    PumpAVSSettings pumpAvsSettings = new PumpAVSSettings(this);
    this.verificationsList.Add(pumpAvsSettings.TaskExam);
    this.pumpsList.Add(pumpAvsSettings.TaskPump);
    PumpDocumentsClass pumpDocumentsClass = new PumpDocumentsClass(this);
    this.verificationsList.Add(pumpDocumentsClass.TaskExam);
    this.pumpsList.Add(pumpDocumentsClass.TaskPump);
    PumpDocRefsClass pumpDocRefsClass = new PumpDocRefsClass(this);
    this.verificationsList.Add(pumpDocRefsClass.TaskExam);
    this.pumpsList.Add(pumpDocRefsClass.TaskPump);
    PumpClass pumpClass1 = PumpArticlesClass.GetPumpClass(num2 != 0, this);
    this.verificationsList.Add(pumpClass1.TaskExam);
    this.pumpsList.Add(pumpClass1.TaskPump);
    PumpSeriesClass pumpSeriesClass = new PumpSeriesClass(this);
    this.verificationsList.Add(pumpSeriesClass.TaskExam);
    this.pumpsList.Add(pumpSeriesClass.TaskPump);
    PumpCompositionClass compositionClass = new PumpCompositionClass(this);
    this.verificationsList.Add(compositionClass.TaskExam);
    this.pumpsList.Add(compositionClass.TaskPump);
    if (PumpHelper.IsVariantsExists)
      this.pumpsList.Add(new PumpSubstitutesClass(this).TaskPump);
    PumpOtdClass pumpOtdClass = new PumpOtdClass(this);
    this.verificationsList.Add(pumpOtdClass.TaskExam);
    this.pumpsList.Add(pumpOtdClass.TaskPump);
    PumpConfiguratorOptions configuratorOptions = new PumpConfiguratorOptions(this);
    this.verificationsList.Add(configuratorOptions.TaskExam);
    this.pumpsList.Add(configuratorOptions.TaskPump);
    this.pumpsList.Add(new PumpOptionsToArticles(this).TaskPump);
    this.pumpsList.Add(new PumpSelections(this).TaskPump);
    this.pumpsList.Add(new PumpClassificators(this).TaskPump);
    PumpSignsClass pumpSignsClass = new PumpSignsClass(this);
    this.verificationsList.Add(pumpSignsClass.TaskExam);
    this.pumpsList.Add(pumpSignsClass.TaskPump);
    IConfigurationService service2 = ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService;
    bool specialModePL = num2 != 0 || service2.Configuration.PLPumpingResume;
    this.ProductionListsSettingsControl = new ProductionListsSettingsControl(specialModePL);
    PumpClass pumpClass2 = PumpProductionLists.GetPumpClass(specialModePL, this);
    this.verificationsList.Add(pumpClass2.TaskExam);
    this.pumpsList.Add(pumpClass2.TaskPump);
    this.finalPumpsList.Add(new PumpFinalizer(this).TaskPump);
    this._settingsControl = new SearchDataSettingsControl(this);
    BlobHelper.Clear();
    PumpEvents.OnStartPump += new OnStartPumpDelegate(this.OnStartPump);
    return num1 != 0;
  }

  public void CheckVersionedTypes()
  {
    if (!PluginSettings.PumpArtVersions)
      return;
    bool flag = false;
    CacheCategory cacheCategory = PumpCache.Category[ImportingCategory.ArticleTypes];
    try
    {
      using (IDbCommand command = this.idb2.CreateCommand())
      {
        do
        {
          command.CommandText = "select distinct(section_id) from v_articles where art_ver_id > 0";
          IDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
          try
          {
            string str = "";
            while (dataReader.Read())
            {
              IMSObjectType imsObjectType = (IMSObjectType) null;
              int int32_1 = BasePumpHelper.ToInt32(dataReader[0]);
              int int32_2 = Convert.ToInt32(cacheCategory.GetNewKey((object) int32_1));
              if (int32_2 != 0)
                imsObjectType = MetaDataHelper.GetObjectType(int32_2);
              if (imsObjectType != null && imsObjectType.VersionsMode != ObjectVersionModes.MultiVersion)
              {
                if (str != "")
                  str += ", ";
                str += $"{imsObjectType.ObjectTypeName} (ID={imsObjectType.ObjectTypeID})";
              }
            }
            if (str != "")
              throw new NotificationException($"В базе Search найдены версии изделий, которые соответствуют неверсионным типам в IPS. Для успешной перекачки следующие типы объектов IPS должны быть версионными: {str}. Устраните несоответствия и перезапустите программу перекачки.");
          }
          finally
          {
            dataReader.Close();
          }
        }
        while (flag);
      }
    }
    finally
    {
      cacheCategory.Release();
    }
  }

  private void OnStartPump(List<IPumpTask> pumpers)
  {
    bool flag = false;
    foreach (IPumpTask pumper in pumpers)
    {
      if (pumper.GUID == PumpDocumentsClass.PumperGUID)
      {
        flag = true;
        break;
      }
    }
    if (flag || this.BlobThread != null)
      return;
    this.StartBlobThread(true);
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
    if (this._settingsControl == null)
      return base.GetSettingsControls();
    List<StepControl> stepControlList = new List<StepControl>()
    {
      (StepControl) this._settingsControl
    };
    stepControlList.Add((StepControl) new SuffixSettingsControl());
    LiteraSettingsControl literaSettingsControl = LiteraSettingsControl.InitControl();
    if (literaSettingsControl != null)
      stepControlList.Add((StepControl) literaSettingsControl);
    stepControlList.Add((StepControl) this.ProductionListsSettingsControl);
    return stepControlList.ToArray();
  }

  protected static Bitmap LoadResImage(Assembly assembly, string name)
  {
    Stream manifestResourceStream = assembly.GetManifestResourceStream(assembly.GetName().Name + name);
    if (manifestResourceStream == null)
      return (Bitmap) null;
    if (Image.FromStream(manifestResourceStream) is Bitmap bitmap && bitmap.RawFormat.Guid != ImageFormat.Icon.Guid)
      bitmap.MakeTransparent();
    return bitmap;
  }

  public static Bitmap InfoImage
  {
    get
    {
      if (SearchDataPlugin._infoImage == null)
        SearchDataPlugin._infoImage = SearchDataPlugin.LoadResImage(typeof (SearchDataPlugin).Assembly, ".Resources.info.png");
      return SearchDataPlugin._infoImage;
    }
  }

  public static Bitmap WarningImage
  {
    get
    {
      if (SearchDataPlugin._warningImage == null)
        SearchDataPlugin._warningImage = SearchDataPlugin.LoadResImage(typeof (SearchDataPlugin).Assembly, ".Resources.warning.png");
      return SearchDataPlugin._warningImage;
    }
  }

  internal string BlobsPath
  {
    get
    {
      if (this._blobPath == null)
      {
        this._blobPath = BlobHelper.TempPath + "_IPSBlobs\\";
        if (!Directory.Exists(this._blobPath))
          Directory.CreateDirectory(this._blobPath);
      }
      return this._blobPath;
    }
  }

  internal string BlobsIndexFileName => this.BlobsPath + "blobs.dat";

  internal BinaryFormatter Formatter
  {
    get
    {
      if (this._formatter == null)
        this._formatter = new BinaryFormatter()
        {
          AssemblyFormat = FormatterAssemblyStyle.Simple
        };
      return this._formatter;
    }
  }

  internal void StartBlobThread(bool runOnce = false)
  {
    if (this.BlobThread != null)
      return;
    this.BlobThread = new BlobThread(this);
    this.BlobThread.Start(runOnce);
  }
}

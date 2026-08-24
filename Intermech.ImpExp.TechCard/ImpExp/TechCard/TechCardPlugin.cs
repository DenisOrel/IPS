// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechCardPlugin
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Expert;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.ImpExp.Interface.Techcard;
using Intermech.ImpExp.TechCard.AutoSel;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.ImpExp.TechCard.Common.TC_CEH;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Common.VidIzd;
using Intermech.ImpExp.TechCard.Common.VidZag;
using Intermech.ImpExp.TechCard.DraftPump;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.ImbaseSync;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TC_INVNOM;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_AGREE;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_LINKS;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.MaterialGroup;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.MaterialGroupSubstitute;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_SKETCH;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_ZAG;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.Vtd;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ZPCEntryPump;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2Tp;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2TpOld;
using Intermech.ImpExp.TechCard.Pumpers.Data.Tp2LinkPump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.ArcArtPump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.ImTablesPump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TC_Configs;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TechcardDocsPumper;
using Intermech.ImpExp.TechCard.TechExpPump.CondsPump;
using Intermech.ImpExp.TechCard.TechExpPump.TablesPump;
using Intermech.ImpExp.TechCard.TechProcPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common.TechDiff;
using Intermech.ImpExp.TechCard.TechProcPump.Common.TechIZW;
using Intermech.ImpExp.TechCard.TechProcPump.TC_INVNOM;
using Intermech.ImpExp.TechCard.TechProcPump.TP_ART;
using Intermech.ImpExp.TechCard.TechProcPump.TP_CHN;
using Intermech.ImpExp.TechCard.TechProcPump.TP_COM;
using Intermech.ImpExp.TechCard.TechProcPump.TP_MAT;
using Intermech.ImpExp.TechCard.TechProcPump.TP_NPER;
using Intermech.ImpExp.TechCard.TechProcPump.TP_OB;
using Intermech.ImpExp.TechCard.TechProcPump.TP_OPER;
using Intermech.ImpExp.TechCard.TechProcPump.TP_OSNPOS;
using Intermech.ImpExp.TechCard.TechProcPump.TP_PER;
using Intermech.ImpExp.TechCard.TechProcPump.TP_REZ;
using Intermech.ImpExp.TechCard.TechProcPump.TP_TOOL;
using Intermech.ImpExp.TechCard.TechProcPump.TP_WRK;
using Intermech.ImpExp.TechCard.TechProcPump.TP_ZAG;
using Intermech.ImpExp.TechCard.TechRoutePump;
using Intermech.ImpExp.TechCard.TechRoutePump.TechRoute_Links.Elem2Oper;
using Intermech.ImpExp.TechCard.TechRoutePump.TechRoute_Links.Routes2Art;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.ImpExp.TechCardEx.Pumpers.Data.TechProcPump.Tp2ZagLink;
using Intermech.ImpExp.TechCardEx.Pumpers.Data.TechProcPump.TpDwgDraft;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.TechCard.Document.Interfaces.Configs.Serialization.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard;

public class TechCardPlugin : PluginClass, IConfigurable
{
  private readonly Dictionary<string, StepControl> _controls = new Dictionary<string, StepControl>();
  internal static readonly List<Conflict> InitializationConflictList = new List<Conflict>();

  private bool CheckContention() => TechCardPlugin.InitializationConflictList.Count > 0;

  private bool CheckBaseVersion()
  {
    if (!this.IsConnected() && !this.BaseConnect())
      return false;
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      command.CommandText = " SELECT *  FROM   " + "IM_VERSION".ToUpper();
      IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior);
      try
      {
        if (!dataReader.Read())
        {
          int num = (int) MessageBox.Show("Невозможно получить информацию о версии базы", "Ошибка", MessageBoxButtons.OK);
          return false;
        }
        try
        {
          TechVersionInfo.DataBase.CurrentVersion = Convert.ToInt32(dataReader["F_VERSION"]);
        }
        catch (InvalidCastException ex)
        {
          TechVersionInfo.DataBase.CurrentVersion = -1;
        }
        if (TechVersionInfo.DataBase.CurrentVersion == -1)
        {
          int num = (int) MessageBox.Show("Невозможно получить информацию о версии базы", "Ошибка загрузки плагина", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        int.TryParse(Convert.ToString(dataReader["F_PRODUCT"]).ToUpper().Replace("TECHCARD", ""), out TechVersionInfo.Program.CurrentVersion);
        if (TechVersionInfo.Program.CurrentVersion == 0)
        {
          int num = (int) MessageBox.Show("В таблице IM_VERSION отсутствует информация о версии TechCard", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        string[] strArray1 = Convert.ToString(dataReader["F_NOTE"]).Split(',');
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        for (int index = 0; index < strArray1.Length; ++index)
        {
          string[] strArray2 = strArray1[index].Split('=');
          dictionary.Add(strArray2[0], strArray2[1]);
        }
        if (!dictionary.ContainsKey("TRUNID"))
        {
          int num = (int) MessageBox.Show("В таблице IM_VERSION отсутствует информация о ревизии TechCard", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        int.TryParse(Convert.ToString(dictionary["TRUNID"]).ToUpper().Replace("T", ""), out TechVersionInfo.Program.CurrentRevision);
        if (TechVersionInfo.Program.CurrentVersion >= 9 && (TechVersionInfo.Program.CurrentVersion != 9 || TechVersionInfo.Program.CurrentRevision >= 8))
          return true;
        int num1 = (int) MessageBox.Show($"Версия 'TECHCARD{TechVersionInfo.Program.CurrentVersion}' (T{TechVersionInfo.Program.CurrentRevision}) не поддерживается. Обновите базу до версии 'TECHCARD{9} (T{8}).", "Ошибка загрузки плагина", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      finally
      {
        dataReader.Close();
      }
    }
  }

  private void RegisterCustomServices()
  {
    ApplicationServices.Container.AddService(typeof (ITechcardCommon), (object) TechcardConsts.TechcardCommon);
    ApplicationServices.Container.AddService<TechCardDocumentConfigSerializeService>(new TechCardDocumentConfigSerializeService());
    ApplicationServices.Container.AddService<TechCardDocumentConfigLoadService>(new TechCardDocumentConfigLoadService());
    PortalImportedObjectsPump.RegisterPortalServices();
  }

  private void InitPumperType()
  {
    TechTypeConversionPredefinedRules.InitializeSettings();
    UpdateIpsSettingsPump updateIpsSettingsPump = new UpdateIpsSettingsPump((PluginClass) this);
    this.verificationsList.Add(updateIpsSettingsPump.TaskExam);
    this.pumpsList.Add(updateIpsSettingsPump.TaskPump);
    Intermech.ImpExp.TechCard.ProductionsPump.ProductionsPump productionsPump = new Intermech.ImpExp.TechCard.ProductionsPump.ProductionsPump((PluginClass) this);
    this.verificationsList.Add(productionsPump.TaskExam);
    this.pumpsList.Add(productionsPump.TaskPump);
    this.verificationsList.Add(new TechTypePumper((PluginClass) this).TaskExam);
    this.verificationsList.Add(new Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntitiesPump((PluginClass) this).TaskExam);
    this.verificationsList.Add(new MetaDataPump((PluginClass) this).TaskExam);
  }

  private void InitPumperMeta()
  {
    TechPumpData.EntFixList = new TechEntFixList();
    this.verificationsList.Add(new TechEntityFixPump((PluginClass) this).TaskExam);
    TechPumpData.Tables.ImTablesData = new ImTableInfoCache();
    this.verificationsList.Add(new TechImTablesPump((PluginClass) this).TaskExam);
    TechPumpData.Tables.ImFieldsData = new ImFieldInfoCache();
    this.verificationsList.Add(new TechImFieldsPump((PluginClass) this).TaskExam);
    TechPumpData.Configs.Cache = new TechConfigCache();
    this.verificationsList.Add(new TechConfigPump((PluginClass) this).TaskExam);
    PumpClass pumpClass1 = (PumpClass) new VidZagPump((PluginClass) this);
    this.verificationsList.Add(pumpClass1.TaskExam);
    this.pumpsList.Add(pumpClass1.TaskPump);
    PumpClass pumpClass2 = (PumpClass) new VidIzdPump((PluginClass) this);
    this.verificationsList.Add(pumpClass2.TaskExam);
    this.pumpsList.Add(pumpClass2.TaskPump);
    PumpClass pumpClass3 = (PumpClass) new ArtArtsPump((PluginClass) this);
    this.verificationsList.Add(pumpClass3.TaskExam);
    this.pumpsList.Add(pumpClass3.TaskPump);
    PumpClass pumpClass4 = (PumpClass) new Intermech.ImpExp.TechCard.WorkTypePump.WorkTypePump((PluginClass) this);
    this.verificationsList.Add(pumpClass4.TaskExam);
    this.pumpsList.Add(pumpClass4.TaskPump);
    PumpClass pumpClass5 = (PumpClass) new TechCehPump((PluginClass) this);
    this.verificationsList.Add(pumpClass5.TaskExam);
    this.pumpsList.Add(pumpClass5.TaskPump);
    TechPumpData.EntTypeList = new EntityTypeList();
    this.verificationsList.Add(new TechEntityTypePump((PluginClass) this).TaskExam);
    PumpClass pumpClass6 = (PumpClass) new Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump.ScenarioPump((PluginClass) this);
    this.verificationsList.Add(pumpClass6.TaskExam);
    this.pumpsList.Add(pumpClass6.TaskPump);
    PumpClass pumpClass7 = (PumpClass) new TablePump((PluginClass) this);
    this.verificationsList.Add(pumpClass7.TaskExam);
    this.pumpsList.Add(pumpClass7.TaskPump);
    PumpClass pumpClass8 = (PumpClass) new Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.FormulaPump((PluginClass) this);
    this.verificationsList.Add(pumpClass8.TaskExam);
    this.pumpsList.Add(pumpClass8.TaskPump);
    PumpClass pumpClass9 = (PumpClass) new CondPump((PluginClass) this);
    this.verificationsList.Add(pumpClass9.TaskExam);
    this.pumpsList.Add(pumpClass9.TaskPump);
    PumpClass pumpClass10 = (PumpClass) new AutoSelCondPump((PluginClass) this);
    this.verificationsList.Add(pumpClass10.TaskExam);
    this.pumpsList.Add(pumpClass10.TaskPump);
    TcBlankProperties tcBlankProperties = new TcBlankProperties((PluginClass) this);
    this.verificationsList.Add(tcBlankProperties.TaskExam);
    this.pumpsList.Add(tcBlankProperties.TaskPump);
    PumpClass pumpClass11 = (PumpClass) new TechInvNomTablePump((PluginClass) this);
    this.verificationsList.Add(pumpClass11.TaskExam);
    this.pumpsList.Add(pumpClass11.TaskPump);
    PumpClass pumpClass12 = (PumpClass) new TechImCatalogUpdatePump((PluginClass) this);
    this.verificationsList.Add(pumpClass12.TaskExam);
    this.pumpsList.Add(pumpClass12.TaskPump);
  }

  private void InitPumperData()
  {
    PumpClass pumpClass1 = (PumpClass) new EntitiesSettingsPump((PluginClass) this);
    this.verificationsList.Add(pumpClass1.TaskExam);
    this.pumpsList.Add(pumpClass1.TaskPump);
    PumpClass pumpClass2 = (PumpClass) new Intermech.ImpExp.TechCard.Common.DataManager.DataManager((PluginClass) this);
    this.verificationsList.Add(pumpClass2.TaskExam);
    this.pumpsList.Add(pumpClass2.TaskPump);
    this.verificationsList.Add(new PortalImportedObjectsPump((PluginClass) this).TaskExam);
    this.verificationsList.Add(new ImbaseFolderCathegoryUpdatePump((PluginClass) this).TaskExam);
    PumpClass pumpClass3 = (PumpClass) new Tp2Obj2LinkPump((PluginClass) this);
    this.verificationsList.Add(pumpClass3.TaskExam);
    this.pumpsList.Add(pumpClass3.TaskPump);
    PumpClass pumpClass4 = (PumpClass) new TechZagGroupPump((PluginClass) this);
    this.verificationsList.Add(pumpClass4.TaskExam);
    this.pumpsList.Add(pumpClass4.TaskPump);
    PumpClass pumpClass5 = (PumpClass) new TechZagPump((PluginClass) this);
    this.verificationsList.Add(pumpClass5.TaskExam);
    this.pumpsList.Add(pumpClass5.TaskPump);
    this.verificationsList.Add(new TechDiffPump((PluginClass) this).TaskExam);
    new TechRoutesInitClass((PluginClass) this).Load(this.verificationsList, this.pumpsList);
    PumpClass pumpClass6 = (PumpClass) new Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump.TechProcessPump((PluginClass) this);
    this.verificationsList.Add(pumpClass6.TaskExam);
    this.pumpsList.Add(pumpClass6.TaskPump);
    PumpClass pumpClass7 = (PumpClass) new ProcRouteEntryPump((PluginClass) this);
    this.verificationsList.Add(pumpClass7.TaskExam);
    this.pumpsList.Add(pumpClass7.TaskPump);
    PumpClass pumpClass8 = (PumpClass) new ProcRouteZPCEntryPump((PluginClass) this);
    this.verificationsList.Add(pumpClass8.TaskExam);
    this.pumpsList.Add(pumpClass8.TaskPump);
    PumpClass pumpClass9 = (PumpClass) new ProcRoutePump((PluginClass) this);
    this.verificationsList.Add(pumpClass9.TaskExam);
    this.pumpsList.Add(pumpClass9.TaskPump);
    PumpClass pumpClass10 = (PumpClass) new Intermech.ImpExp.TechCard.TechProcPump.Common.TechCehZahodPump.TechCehZahodPump((PluginClass) this);
    this.verificationsList.Add(pumpClass10.TaskExam);
    this.pumpsList.Add(pumpClass10.TaskPump);
    PumpClass pumpClass11 = (PumpClass) new TechOperPump((PluginClass) this);
    this.verificationsList.Add(pumpClass11.TaskExam);
    this.pumpsList.Add(pumpClass11.TaskPump);
    PumpClass pumpClass12 = (PumpClass) new TechRouteElem2OperPump((PluginClass) this);
    this.verificationsList.Add(pumpClass12.TaskExam);
    this.pumpsList.Add(pumpClass12.TaskPump);
    PumpClass pumpClass13 = (PumpClass) new TechPerehodPump((PluginClass) this);
    this.verificationsList.Add(pumpClass13.TaskExam);
    this.pumpsList.Add(pumpClass13.TaskPump);
    PumpClass pumpClass14 = (PumpClass) new TechMaterialGroupSubstitutePump((PluginClass) this);
    this.verificationsList.Add(pumpClass14.TaskExam);
    this.pumpsList.Add(pumpClass14.TaskPump);
    PumpClass pumpClass15 = (PumpClass) new TechMaterialGroupPump((PluginClass) this);
    this.verificationsList.Add(pumpClass15.TaskExam);
    this.pumpsList.Add(pumpClass15.TaskPump);
    PumpClass pumpClass16 = (PumpClass) new TechMaterialPump((PluginClass) this);
    this.verificationsList.Add(pumpClass16.TaskExam);
    this.pumpsList.Add(pumpClass16.TaskPump);
    PumpClass pumpClass17 = (PumpClass) new TechMaterialLinksPump((PluginClass) this);
    this.verificationsList.Add(pumpClass17.TaskExam);
    this.pumpsList.Add(pumpClass17.TaskPump);
    PumpClass pumpClass18 = (PumpClass) new VtdPump((PluginClass) this);
    this.verificationsList.Add(pumpClass18.TaskExam);
    this.pumpsList.Add(pumpClass18.TaskPump);
    PumpClass pumpClass19 = (PumpClass) new TechRoute2ArtLinks((PluginClass) this);
    this.verificationsList.Add(pumpClass19.TaskExam);
    this.pumpsList.Add(pumpClass19.TaskPump);
    PumpClass pumpClass20 = (PumpClass) new TechArtPump((PluginClass) this);
    this.verificationsList.Add(pumpClass20.TaskExam);
    this.pumpsList.Add(pumpClass20.TaskPump);
    PumpClass pumpClass21 = (PumpClass) new TechAddMovementPump((PluginClass) this);
    this.verificationsList.Add(pumpClass21.TaskExam);
    this.pumpsList.Add(pumpClass21.TaskPump);
    PumpClass pumpClass22 = (PumpClass) new TechOsnPosPump((PluginClass) this);
    this.verificationsList.Add(pumpClass22.TaskExam);
    this.pumpsList.Add(pumpClass22.TaskPump);
    PumpClass pumpClass23 = (PumpClass) new TechOsnPosInstrumLinksPump((PluginClass) this);
    this.verificationsList.Add(pumpClass23.TaskExam);
    this.pumpsList.Add(pumpClass23.TaskPump);
    PumpClass pumpClass24 = (PumpClass) new TechRezPump((PluginClass) this);
    this.verificationsList.Add(pumpClass24.TaskExam);
    this.pumpsList.Add(pumpClass24.TaskPump);
    PumpClass pumpClass25 = (PumpClass) new TechCommentPump((PluginClass) this);
    this.verificationsList.Add(pumpClass25.TaskExam);
    this.pumpsList.Add(pumpClass25.TaskPump);
    PumpClass pumpClass26 = (PumpClass) new TechToolsPump((PluginClass) this);
    this.verificationsList.Add(pumpClass26.TaskExam);
    this.pumpsList.Add(pumpClass26.TaskPump);
    PumpClass pumpClass27 = (PumpClass) new TechOutfitPump((PluginClass) this);
    this.verificationsList.Add(pumpClass27.TaskExam);
    this.pumpsList.Add(pumpClass27.TaskPump);
    PumpClass pumpClass28 = (PumpClass) new TechInvNomPump((PluginClass) this);
    this.verificationsList.Add(pumpClass28.TaskExam);
    this.pumpsList.Add(pumpClass28.TaskPump);
    PumpClass pumpClass29 = (PumpClass) new TechOutfitInvNomLinksPump((PluginClass) this);
    this.verificationsList.Add(pumpClass29.TaskExam);
    this.pumpsList.Add(pumpClass29.TaskPump);
    PumpClass pumpClass30 = (PumpClass) new TechPersonalPump((PluginClass) this);
    this.verificationsList.Add(pumpClass30.TaskExam);
    this.pumpsList.Add(pumpClass30.TaskPump);
    PumpClass pumpClass31 = (PumpClass) new TechTPOverpatchingPump((PluginClass) this);
    this.verificationsList.Add(pumpClass31.TaskExam);
    this.pumpsList.Add(pumpClass31.TaskPump);
    PumpClass pumpClass32 = (PumpClass) new TechIzwPump((PluginClass) this);
    this.verificationsList.Add(pumpClass32.TaskExam);
    this.pumpsList.Add(pumpClass32.TaskPump);
    PumpClass pumpClass33 = (PumpClass) new DraftOLEPump((PluginClass) this);
    this.verificationsList.Add(pumpClass33.TaskExam);
    this.pumpsList.Add(pumpClass33.TaskPump);
    PumpClass pumpClass34 = (PumpClass) new DraftDwgDataPump((PluginClass) this);
    this.verificationsList.Add(pumpClass34.TaskExam);
    this.pumpsList.Add(pumpClass34.TaskPump);
    PumpClass pumpClass35 = (PumpClass) new TechSketchPump((PluginClass) this);
    this.verificationsList.Add(pumpClass35.TaskExam);
    this.pumpsList.Add(pumpClass35.TaskPump);
    PumpClass pumpClass36 = (PumpClass) new TechSketchDwgPump((PluginClass) this);
    this.verificationsList.Add(pumpClass36.TaskExam);
    this.pumpsList.Add(pumpClass36.TaskPump);
    PumpClass pumpClass37 = (PumpClass) new TechTp2ZagLinkPump((PluginClass) this);
    this.verificationsList.Add(pumpClass37.TaskExam);
    this.pumpsList.Add(pumpClass37.TaskPump);
    PumpClass pumpClass38 = (PumpClass) new TechAgreePump((PluginClass) this);
    this.verificationsList.Add(pumpClass38.TaskExam);
    this.pumpsList.Add(pumpClass38.TaskPump);
    PumpClass pumpClass39 = (PumpClass) new TechTpLinkPump((PluginClass) this);
    this.verificationsList.Add(pumpClass39.TaskExam);
    this.pumpsList.Add(pumpClass39.TaskPump);
    PumpClass pumpClass40 = (PumpClass) new TechRoute2TpOldPump((PluginClass) this);
    this.verificationsList.Add(pumpClass40.TaskExam);
    this.pumpsList.Add(pumpClass40.TaskPump);
    PumpClass pumpClass41 = (PumpClass) new TechRoute2TpPump((PluginClass) this);
    this.verificationsList.Add(pumpClass41.TaskExam);
    this.pumpsList.Add(pumpClass41.TaskPump);
  }

  private void InitCustomData()
  {
    if (MeasureHelper.Measures == null)
      MeasureHelper.Init(this.Idw.GetUserSession().GetMeasuresList());
    Intermech.Imbase.Consts.Initialize(this.Idw.GetUserSession(), (IMetaDataHelper) MetaDataHelperService.Instance);
    if (ExpertConsts.Consts != null)
      return;
    ExpertConsts.Init(this.Idw.GetUserSession());
  }

  private void IntegrityTest(IAppManager manager)
  {
    if (TechCache.isResumeMode || !this.CheckContention())
      return;
    using (ConflictManager conflictManager = new ConflictManager())
    {
      switch (conflictManager.ShowDialog())
      {
        case DialogResult.Cancel:
          if (MessageBox.Show("Вы уверены, что желаете закрыть приложение, тем самым завершив перекачку? \n\n Для завершения перекачки нажмите \"Yes/Да\" ", "Завершение перекачки", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            break;
          manager.CloseManager();
          break;
      }
    }
  }

  private void InitSettingsUpdate(IAppManager manager)
  {
    UpdateControlDataPump updateControlDataPump = new UpdateControlDataPump((PluginClass) this, manager);
    this.verificationsList.Add(updateControlDataPump.TaskExam);
    foreach (StepControl control in updateControlDataPump.Controls)
      this._controls.Add(control.Name, control);
    TechSettingsControl techSettingsControl = new TechSettingsControl((object) manager);
    this._controls.Add(techSettingsControl.Name, (StepControl) techSettingsControl);
  }

  private void ResumePumpingData()
  {
    SavePoint savePoint = ServicesManager.GetService(typeof (ISavePoint)) is ISavePoint service ? service.GetSavePoint() : (SavePoint) null;
    if (savePoint == null)
      return;
    if (!TechCache.ReadAllLists(service))
    {
      int num = (int) MessageBox.Show("Ошибка чтения выгружаемых списков! \n Дальнейшая закачка невозможна!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      this.appManager.AddErrorMessage("Ошибка чтения выгружаемых списков TechCard. Дальнейшая закачка невозможна!");
      this.pumpsList.Clear();
      this.verificationsList.Clear();
    }
    else
    {
      TechCache.isResumeMode = true;
      TechCache.SavePoint = savePoint.Clone() as SavePoint;
    }
  }

  public TechCardPlugin(IAppManager manager)
    : base(manager)
  {
    TechcardConsts.Plugin = (PluginClass) this;
    TechcardConsts.TechcardCommon = (ITechcardCommon) new TechcardConsts();
    TechcardConsts.ConnectionManager = new TechConnectionsManager((PluginClass) this);
    if (!this.CheckBaseVersion())
      return;
    MetaDataHelper.SyncMetadata((manager.DataWriter.GetUserSession() as IUserSessionCacheDataSet).CacheDataSet);
    this.RegisterCustomServices();
    this.InitPumperType();
    this.InitPumperMeta();
    this.InitPumperData();
    this.InitCustomData();
    this.ResumePumpingData();
    this.IntegrityTest(manager);
    this.InitSettingsUpdate(manager);
  }

  public override string Name => "INTERMECH TechCard Plugin";

  public override string Description
  {
    get => "Модуль расширения для перекачки данных INTERMECH TechCard из базы";
  }

  public override StepControl[] GetSettingsControls()
  {
    StepControl[] array = new StepControl[this._controls.Values.Count];
    this._controls.Values.CopyTo(array, 0);
    return array;
  }

  public override bool BaseConnect()
  {
    this.baseConnect = this.OpenDbConnection(ConnStrType.Imbase);
    return this.IsConnected() && SearchConnectionsManager.GetConnection() != null;
  }

  public void LoadConfiguration()
  {
    TechCardPlugin.Configuration = new TechConfiguration(this.appManager);
  }

  public void LoadConfiguration(IConfiguration cfg)
  {
  }

  public void SaveConfiguration()
  {
  }

  public void SaveConfiguration(IConfiguration cfg)
  {
  }

  internal static TechConfiguration Configuration { get; private set; }
}

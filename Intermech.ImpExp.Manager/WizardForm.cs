// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.WizardForm
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using DevExpress.IM.Utils;
using Intermech.Client.Core;
using Intermech.Controls;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.Controls;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Interface.PumpStatistics;
using Intermech.ImpExp.Interface.Techcard;
using Intermech.ImpExp.Manager.Caches;
using Intermech.ImpExp.Manager.CommonData;
using Intermech.ImpExp.Manager.DataWriter;
using Intermech.ImpExp.Manager.Properties;
using Intermech.ImpExp.Manager.StepControls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.ImpExp.Manager;

internal sealed class WizardForm : Form, IAppManager
{
  private ILogFile log;
  internal PluginsManager _plugins;
  internal DataBaseManager _databases;
  internal DataWriterImpl _dataWriter;
  internal MetadataInfo _meatadataInf;
  private SaveSettings _saveSettings;
  private CacheManager _cache;
  internal SavePointManager _savePointManager;
  private List<StepControl> _stepControls = new List<StepControl>();
  private StepControl _currentStepControl;
  private int _stepIndex;
  internal List<IPumpTask> initsCollection;
  internal List<IPumpTask> pumpsCollection;
  private ImportTimer _importTimer;
  private const int _messageLogLimit = 1000;
  private StepControlSplash splashControl;
  private StepControlInit initControl;
  private StepControlSettings attrSetControl;
  private StepControlMetadata metadataControl;
  private StepControlPump pumpControl;
  private StepControlResult finalControl;
  private List<string> _listSteps = new List<string>(1);
  private bool _exit;
  private StepSettingsSaver _saverSteps;
  private Dictionary<WizardForm.MessageType, int> _countMessages;
  private Dictionary<WizardForm.MessageType, bool> _showMessages;
  internal string nameSettingsStep = "Настройка метаданных";
  private IContainer components;
  private ImageList imageList1;
  private Panel panel4;
  private PictureBox pictureBox1;
  private Label lText;
  private Panel panel2;
  private Button buttonNext;
  private Button buttonExit;
  private Button buttonPrev;
  private Panel panel3;
  private Panel panelSteps;
  private Label lErrorsCount;
  private Label lInfosCount;
  private Label label1;
  private Label label6;
  private Label label4;
  private Label lWarningsCount;
  private SplitContainer splitContainer1;
  private Label lTime;
  private ImageList imageList2;
  private iGrid iGrid1;
  private iGCellStyle iGrid1Col0CellStyle;
  private iGColHdrStyle iGrid1Col0ColHdrStyle;
  private iGCellStyle iGrid1DefaultCellStyle;
  private iGColHdrStyle iGrid1DefaultColHdrStyle;
  private iGCellStyle iGrid1RowTextColCellStyle;
  private Button bBasesInfo;
  private ToolTipController toolTipController1;
  private ContextMenuStrip contextMenuStrip1;
  private ToolStripMenuItem miCopy;

  public WizardForm()
  {
    try
    {
      this.InitializeComponent();
      this.log = (ILogFile) new LogFile(Path.Combine(Application.StartupPath, "import.log"), false, true, true);
      this.log.WriteMessage(this.GetType().Assembly.FullName);
      this.InitializeServices();
      this.lTime.Text = string.Empty;
      Array values = Enum.GetValues(typeof (WizardForm.MessageType));
      this._countMessages = new Dictionary<WizardForm.MessageType, int>(values.Length);
      this._showMessages = new Dictionary<WizardForm.MessageType, bool>(values.Length);
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      string attribute1 = FormStorageEx.GetAttribute((Control) this, "ShowErrors");
      if (attribute1 != string.Empty)
        flag1 = Convert.ToBoolean(attribute1);
      string attribute2 = FormStorageEx.GetAttribute((Control) this, "ShowWarnings");
      if (attribute2 != string.Empty)
        flag2 = Convert.ToBoolean(attribute2);
      string attribute3 = FormStorageEx.GetAttribute((Control) this, "ShowInfos");
      if (attribute3 != string.Empty)
        flag3 = Convert.ToBoolean(attribute3);
      foreach (WizardForm.MessageType key in values)
      {
        switch (key)
        {
          case WizardForm.MessageType.Info:
            this._showMessages.Add(key, flag3);
            break;
          case WizardForm.MessageType.Warning:
            this._showMessages.Add(key, flag2);
            break;
          case WizardForm.MessageType.Error:
            this._showMessages.Add(key, flag1);
            break;
        }
        this._countMessages.Add(key, 0);
      }
      this.label1.ImageIndex = flag1 ? 5 : 2;
      this.label4.ImageIndex = flag2 ? 4 : 1;
      this.label6.ImageIndex = flag3 ? 3 : 0;
      this.ShowMessages_CheckedChanged((object) this, new EventArgs());
      bool newImport = true;
      SavePoint savePoint = this._savePointManager.GetSavePoint();
      if (savePoint != null)
      {
        if (savePoint.OperationTerminateType != TerminateType.None && savePoint.OperationTerminateType != TerminateType.Complete)
        {
          if (MessageBox.Show("Предыдущая закачка была не завершена. Продолжить ?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          {
            this._savePointManager.RemoveSavePoint();
            this.ClearStatistics();
          }
          else
            newImport = false;
        }
        else if (savePoint.OperationTerminateType == TerminateType.Complete)
        {
          if (IMMessageBox.Show("Внимание", "Предыдущая закачка была завершена. Начать закачку заново или произвести докачку?", new IMMessageBoxButton[2]
          {
            new IMMessageBoxButton("Заново", DialogResult.OK),
            new IMMessageBoxButton("Докачка", DialogResult.No)
          }, IMMessageBoxImage.Question) == DialogResult.OK)
          {
            this._savePointManager.RemoveSavePoint();
          }
          else
          {
            savePoint.RePumpMode = true;
            savePoint.PumpCompleted?.Clear();
            savePoint.PumpGuid = Guid.Empty;
            this._savePointManager.SetSavePoint(savePoint);
          }
          this.ClearStatistics();
        }
      }
      else
      {
        foreach (string settingsFile in SettingsHelper.GetSettingsFiles())
        {
          if (File.Exists(settingsFile))
          {
            string str = settingsFile + ".bak";
            if (!File.Exists(str))
              File.Copy(settingsFile, str, true);
          }
        }
      }
      this.initsCollection = new List<IPumpTask>();
      this.pumpsCollection = new List<IPumpTask>();
      this.splashControl = new StepControlSplash((object) this);
      this.AddSettings((StepControl) this.splashControl);
      this.OnStepUpdate();
      this._importTimer = new ImportTimer((ICache) this._cache, newImport);
      this._importTimer.OnTickImportTimer += new OnTickImportTimerHandler(this._importTimer_OnTickImportTimer);
      this._importTimer.Start();
    }
    catch (Exception ex)
    {
      ExceptionHelper.ExceptionService.ShowException(ex);
    }
  }

  private void _importTimer_OnTickImportTimer(object sender, OnTickImportEventArgs e)
  {
    if (!this.Visible)
      return;
    if (this.InvokeRequired)
      this.BeginInvoke((Delegate) new OnTickImportTimerHandler(this.SetTime), sender, (object) e);
    else
      this.SetTime(sender, e);
  }

  private void SetTime(object sender, OnTickImportEventArgs e)
  {
    DateTime dateTime = new DateTime(e.Ticks);
    int num = dateTime.Day - 1;
    this.lTime.Text = $"Время импорта: {(num > 0 ? (object) $"{num} д " : (object) string.Empty)}{dateTime.ToString("HH:mm:ss")}";
  }

  public void LoadSettings() => this._saveSettings.Load();

  private void InitializeServices()
  {
    this.LoadSettings(Path.Combine(Application.StartupPath, "Intermech.ImpExp.Manager.cfg"));
    this._plugins = new PluginsManager((IAppManager) this);
    this._databases = new DataBaseManager((IAppManager) this);
    this._dataWriter = new DataWriterImpl((IAppManager) this);
    this._meatadataInf = new MetadataInfo((IAppManager) this);
    this._cache = new CacheManager();
    this._savePointManager = new SavePointManager();
    this._saveSettings = new SaveSettings();
    BigImageList serviceInstance = new BigImageList();
    new ImageList().ColorDepth = ColorDepth.Depth24Bit;
    this.AddBigImages((IBigImageList) serviceInstance);
    ApplicationServices.Container.AddService<ILogFile>(this.log);
    ServicesManager.ServiceContainer.AddService(typeof (IImpExpPluginsManager), (object) this._plugins);
    ServicesManager.ServiceContainer.AddService(typeof (INotificationService), (object) new NotificationService());
    ServicesManager.ServiceContainer.AddService(typeof (IAttributeImageList), (object) new AttributeImageList());
    ServicesManager.ServiceContainer.AddService(typeof (IBigImageList), (object) serviceInstance);
    ServicesManager.ServiceContainer.AddService(typeof (IDataWriter), (object) this._dataWriter);
    ServicesManager.ServiceContainer.AddService(typeof (ISettingsGroupService), (object) new SettingsGroupService());
    ServicesManager.ServiceContainer.AddService(typeof (IMetadataInfo), (object) this._meatadataInf);
    ServicesManager.ServiceContainer.AddService(typeof (ICache), (object) this._cache);
    ServicesManager.ServiceContainer.AddService(typeof (ISavePoint), (object) this._savePointManager);
    ServicesManager.ServiceContainer.AddService(typeof (ISaveSettings), (object) this._saveSettings);
    ServicesManager.ServiceContainer.AddService(typeof (ITechCardTypeService), (object) new TechCardTypeService());
    ServicesManager.ServiceContainer.AddService(typeof (IPackedStream), (object) new PackedStreamService());
    ApplicationServices.Container.AddService<PumpStatisticsService>(new PumpStatisticsService());
  }

  private int StepIndex
  {
    get => this._stepIndex;
    set
    {
      if (this._stepIndex == value || value <= -1 || value >= this._stepControls.Count)
        return;
      this._stepIndex = value;
      this.OnStepUpdate();
    }
  }

  private void StepsInit()
  {
    StepControlInit stepControlInit = new StepControlInit((object) this);
    stepControlInit.LogFile = this.log;
    this.initControl = stepControlInit;
    StepControlSettings stepControlSettings = new StepControlSettings((object) this);
    stepControlSettings.LogFile = this.log;
    this.attrSetControl = stepControlSettings;
    this.metadataControl = new StepControlMetadata((object) this);
    StepControlPump stepControlPump = new StepControlPump((object) this);
    stepControlPump.LogFile = this.log;
    this.pumpControl = stepControlPump;
    this.finalControl = new StepControlResult((object) this);
    SavePoint savePoint = this._savePointManager.GetSavePoint();
    if (savePoint == null || savePoint.OperationTerminateType == TerminateType.None)
    {
      if (this.initsCollection.Count > 0)
        this.AddSettings((StepControl) this.initControl, 1);
      this._listSteps.Add(this.nameSettingsStep);
      this.AddSettings((StepControl) this.attrSetControl);
      this.AddSettings((StepControl) this.metadataControl);
    }
    else if (savePoint.OperationTerminateType == TerminateType.Complete && this.initsCollection.Count > 0)
    {
      this.AddSettings((StepControl) this.initControl, 1);
      this.AddSettings((StepControl) this.metadataControl);
    }
    if (this.pumpsCollection.Count > 0)
      this.AddSettings((StepControl) this.pumpControl);
    this.AddSettings((StepControl) this.finalControl);
  }

  private void StepsFinalize()
  {
    foreach (IPlugin plugins in this._plugins.PluginsList)
      plugins?.BaseDisconnect();
  }

  private void OnStepUpdate()
  {
    if (this._exit)
      return;
    if (this.InvokeRequired)
      this.BeginInvoke((Delegate) new WizardForm.stepUpdateDelegate(this._stepUpdate));
    else
      this._stepUpdate();
  }

  private void _stepUpdate()
  {
    if (this._currentStepControl != null)
    {
      if (this._currentStepControl is IConfigurable)
        (this._currentStepControl as IConfigurable).SaveConfiguration();
      this._currentStepControl.Parent = (Control) null;
    }
    this._currentStepControl = this._stepControls[this._stepIndex];
    this.buttonPrev.Visible = this._stepIndex > 0;
    if (this._stepIndex > 0)
      this.buttonPrev.Enabled = this._stepControls[this._stepIndex - 1].StepPrevAllowed;
    this._currentStepControl.Parent = (Control) this.panelSteps;
    if (this._currentStepControl is IConfigurable)
      (this._currentStepControl as IConfigurable).LoadConfiguration();
    this._currentStepControl.RefreshControl();
    this.buttonNext.Enabled = this._stepIndex == 0 || this._stepIndex < this._stepControls.Count - 1;
    if (this._stepIndex == 0)
      this.buttonExit.Text = "Выход";
    else if (this._stepIndex == this._stepControls.Count - 1)
      this.buttonExit.Text = "Завершить";
    else
      this.buttonExit.Text = "Прервать";
    this.lText.Text = this._stepIndex > 0 ? $"{this._stepControls[this._stepIndex].Caption} (шаг {this._stepIndex + 1} из {this._stepControls.Count})" : $"{this._stepControls[this._stepIndex].Caption} (шаг {this._stepIndex + 1})";
    this.pictureBox1.Image = this._stepControls[this._stepIndex].Image;
  }

  private void OnStepNext()
  {
    bool flag = false;
    try
    {
      if (!this._stepControls[this._stepIndex].LeaveControl())
        return;
      if (this._stepIndex > 0 && this._stepControls[this._stepIndex + 1] is StepControlMetadata)
      {
        if (this._saverSteps == null)
        {
          List<StepControl> controls = new List<StepControl>(this._stepIndex);
          for (int index = 1; index <= this._stepIndex - 1; ++index)
            controls.Add(this._stepControls[index]);
          this._saverSteps = new StepSettingsSaver(controls);
        }
        SaveSettingsResult saveSettingsResult = this._saverSteps.Save();
        if (saveSettingsResult != SaveSettingsResult.ssrOk)
        {
          this.buttonNext.Enabled = saveSettingsResult == SaveSettingsResult.ssrRetry;
          return;
        }
        this._currentStepControl.StepPrevAllowed = false;
        if (this._saveSettings != null)
          this._saveSettings.Save();
      }
      if (this._currentStepControl is ThreadedStepControl)
        (this._currentStepControl as ThreadedStepControl).OnEndSaveSettings += new OnEndEventHandler(this.OnSaveSettings);
      if (!this._currentStepControl.StepPrevAllowed)
      {
        SaveSettingsResult saveSettingsResult = this._currentStepControl.SaveSettings();
        if (saveSettingsResult == SaveSettingsResult.ssrOk)
          ++this.StepIndex;
        else
          this.buttonNext.Enabled = saveSettingsResult == SaveSettingsResult.ssrRetry;
      }
      else
        ++this.StepIndex;
    }
    finally
    {
      if (flag)
        this._stepControls[this.StepIndex - 1].StepPrevAllowed = false;
    }
  }

  private void SetButton() => this.buttonNext.Enabled = true;

  private void OnSaveSettings(object obj, OnEndEventArgs ea)
  {
    if (ea.Result == SaveSettingsResult.ssrOk)
    {
      if (!this.Visible)
        return;
      if (this.InvokeRequired)
        this.Invoke((Delegate) new MethodInvoker(this.SetButton));
      else
        this.SetButton();
      ++this.StepIndex;
    }
    else if (ea.Result == SaveSettingsResult.ssrError)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new MethodInvoker(this.TerminateProgram));
      else
        this.TerminateProgram();
    }
    else
    {
      if (ea.Result != SaveSettingsResult.ssrMetadataTerminate)
        return;
      this.ResultType |= ResultTypes.MetadataTerminate;
      ++this.StepIndex;
    }
  }

  internal ResultTypes ResultType { get; private set; }

  private void TerminateProgram() => this.StepsFinalize();

  private void StepPrev() => --this.StepIndex;

  public void LoadSettings(string configFileName)
  {
    try
    {
      object serviceInstance = (object) ConfigurationLoader.Load(configFileName);
      ServicesManager.ServiceContainer.AddService(typeof (IConfiguration), serviceInstance);
      ServicesManager.ServiceContainer.AddService(typeof (IConfigurationService), serviceInstance);
    }
    catch (FileNotFoundException ex)
    {
      this.AddErrorMessage("Ошибка чтения файла установок:\r\n" + ex.Message);
    }
  }

  public void LoadPlugins()
  {
    IConfiguration service = ServicesManager.GetService(typeof (IConfiguration)) as IConfiguration;
    try
    {
      if (service == null)
        return;
      this._plugins.LoadPlugins(service);
    }
    catch (XmlException ex)
    {
      this.AddErrorMessage("Ошибка чтения файла установок:\r\n" + ex.Message);
    }
  }

  public void AddInfoMessage(string str) => this.AddMessageEvent(str, WizardForm.MessageType.Info);

  public void AddErrorMessage(string str)
  {
    this.AddMessageEvent(str, WizardForm.MessageType.Error);
    if ((this.ResultType & ResultTypes.ErrorsPresent) != ResultTypes.None)
      return;
    this.ResultType |= ResultTypes.ErrorsPresent;
  }

  public void AddExceptionToLog(Exception ex)
  {
    this.AddMessageEvent(ex, WizardForm.MessageType.Error);
    if ((this.ResultType & ResultTypes.ErrorsPresent) != ResultTypes.None)
      return;
    this.ResultType |= ResultTypes.ErrorsPresent;
  }

  public void AddWarningMessage(string str)
  {
    this.AddMessageEvent(str, WizardForm.MessageType.Warning);
    if ((this.ResultType & ResultTypes.WarningPresent) != ResultTypes.None)
      return;
    this.ResultType |= ResultTypes.WarningPresent;
  }

  private void AddMessageEvent(string str, WizardForm.MessageType type)
  {
    if (this.IsDisposed)
      return;
    this.AppendMessageLog(str, type);
    if (this.InvokeRequired)
      this.BeginInvoke((Delegate) new WizardForm.ShowMessageDelegate(this.ShowMessage), (object) str, (object) type);
    else
      this.ShowMessage(str, type);
  }

  private void AddMessageEvent(Exception ex, WizardForm.MessageType type)
  {
    if (this.IsDisposed)
      return;
    this.AppendExceptionLog(ex, type);
  }

  private void ShowsClick(object sender, EventArgs e)
  {
    if (sender is Label label)
    {
      WizardForm.MessageType int32 = (WizardForm.MessageType) Convert.ToInt32(label.Tag);
      this._showMessages[int32] = !this._showMessages[int32];
      label.ImageIndex = this._showMessages[int32] ? (int) (int32 + 3) : (int) int32;
    }
    this.ShowMessages_CheckedChanged((object) this, new EventArgs());
    this.RebuildListView();
  }

  private void RebuildListView()
  {
    for (int index = 0; index < this.iGrid1.Rows.Count; ++index)
    {
      iGRow row = this.iGrid1.Rows[index];
      row.Visible = this._showMessages[(row.Tag as WizardForm.RowTag).Type];
    }
  }

  private void AppendExceptionLog(Exception ex, WizardForm.MessageType type)
  {
    this.log.WriteMessage($"{EnumDescConverter.GetEnumDescription((Enum) type)} : [{DateTime.Now.ToString("dd.MM.yy HH:mm:ss", (IFormatProvider) DateTimeFormatInfo.InvariantInfo)}] {Helper.GetExceptionMessage(ex)}");
    this.log.WriteMessage($"StackTrace: {ex.StackTrace}");
    if (ex.InnerException == null)
      return;
    this.log.WriteMessage($"> InnerException: {Helper.GetExceptionMessage(ex.InnerException)}");
    this.log.WriteMessage($"> InnerStackTrace: {ex.InnerException.StackTrace}");
  }

  private void AppendMessageLog(string str, WizardForm.MessageType type)
  {
    str = $"[{DateTime.Now.ToString("dd.MM.yy HH:mm:ss", (IFormatProvider) DateTimeFormatInfo.InvariantInfo)}] {str}";
    this.log.WriteMessage($"{EnumDescConverter.GetEnumDescription((Enum) type)} : {str}");
  }

  private void ShowMessage(string str, WizardForm.MessageType type)
  {
    int hashCode = str.GetHashCode();
    str = $"[{DateTime.Now.ToString("dd.MM.yy HH:mm:ss", (IFormatProvider) DateTimeFormatInfo.InvariantInfo)}] {str}";
    this._countMessages[type]++;
    if (type == WizardForm.MessageType.Error)
      this.lErrorsCount.Text = $"Ошибок: {this._countMessages[type]}";
    if (type == WizardForm.MessageType.Warning)
      this.lWarningsCount.Text = $"Предупреждений: {this._countMessages[type]}";
    if (type == WizardForm.MessageType.Info)
      this.lInfosCount.Text = $"Сообщений: {this._countMessages[type]}";
    this.iGrid1.BeginUpdate();
    try
    {
      if (this.iGrid1.Rows.Count > 0)
      {
        iGRow row = this.iGrid1.Rows[0];
        WizardForm.RowTag tag = row.Tag as WizardForm.RowTag;
        if (tag.MessageHash == hashCode)
        {
          string str1 = Convert.ToString(row.Cells[0].Value);
          ++tag.Count;
          if (str1[str1.Length - 1] == ']')
            str1 = str1.Remove(str1.LastIndexOf('['));
          row.Cells[0].Value = (object) $"{str1} [{tag.Count}]";
          return;
        }
        for (int rowIndex = this.iGrid1.Rows.Count - 1; rowIndex >= 1000; --rowIndex)
          this.iGrid1.Rows.RemoveAt(rowIndex);
      }
      iGRow iGrow = this.iGrid1.Rows.Insert(0);
      iGrow.Cells[0].ImageIndex = (int) type;
      iGrow.Cells[0].Value = (object) str;
      iGrow.Tag = (object) new WizardForm.RowTag(type, hashCode);
      bool showMessage = this._showMessages[type];
      if (iGrow.Visible == showMessage)
        return;
      iGrow.Visible = showMessage;
    }
    finally
    {
      this.iGrid1.EndUpdate();
    }
  }

  public IDataBaseManager DBManager => (IDataBaseManager) this._databases;

  public IDataWriter DataWriter => (IDataWriter) this._dataWriter;

  public void AddSettings(StepControl settingControl) => this.AddSettings(settingControl, -1);

  public void AddSettings(StepControl settingControl, int index)
  {
    if (settingControl == null)
      return;
    if (index > -1)
      this._stepControls.Insert(index, settingControl);
    else
      this._stepControls.Add(settingControl);
    settingControl.Dock = DockStyle.Fill;
    string str = settingControl.Caption;
    if (str == string.Empty)
      str = this.nameSettingsStep;
    foreach (string listStep in this._listSteps)
    {
      if (listStep == str)
        return;
    }
    this._listSteps.Add(str);
  }

  private void Exit_Click(object sender, EventArgs e)
  {
    if (this._stepIndex == 0 || this._stepIndex == this._stepControls.Count - 1)
    {
      this.StepsFinalize();
      this.Close();
    }
    else
    {
      if (MessageBox.Show("Вы действительно желаете прервать закачку ?", "Прервать закачку", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this._exit = true;
      if (this._currentStepControl != null)
        this._currentStepControl.Cancel();
      this.StepsFinalize();
      this.Close();
    }
  }

  private void Prev_Click(object sender, EventArgs e) => this.StepPrev();

  private void Next_Click(object sender, EventArgs e) => this.OnStepNext();

  private void WizardForm_Load(object sender, EventArgs e)
  {
    try
    {
      FormStorageEx.LoadSettings((Control) this);
      string attribute = FormStorageEx.GetAttribute((Control) this, "SplitterDistance");
      if (!(attribute != string.Empty))
        return;
      this.splitContainer1.SplitterDistance = Convert.ToInt32(attribute);
    }
    catch (Exception ex)
    {
      ExceptionHelper.ExceptionService.ShowException(ex);
    }
  }

  public void StopTimer()
  {
    if (this.InvokeRequired)
      this.BeginInvoke((Delegate) new MethodInvoker(this.CloseTimer));
    else
      this.CloseTimer();
  }

  private void CloseTimer()
  {
    if (!this._importTimer.Started)
      return;
    this._importTimer.OnTickImportTimer -= new OnTickImportTimerHandler(this._importTimer_OnTickImportTimer);
    this._importTimer.Stop();
  }

  private void WizardForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    if (this._importTimer != null && this._importTimer.Started)
      this.StopTimer();
    if (this._cache != null)
      this._cache.Close();
    if (this.log != null)
      this.log.Close();
    if (ServicesManager.ServiceContainer.GetService(typeof (IMetadataInfo)) is IMetadataInfo service1)
      service1.Close();
    if (this.Visible && this.WindowState != FormWindowState.Minimized)
    {
      FormStorageEx.SaveSettings((Control) this);
      FormStorageEx.AddAttribute((Control) this, "SplitterDistance", Convert.ToString(this.splitContainer1.SplitterDistance));
    }
    FormStorageEx.AddAttribute((Control) this, "ShowErrors", Convert.ToString(this._showMessages[WizardForm.MessageType.Error]));
    FormStorageEx.AddAttribute((Control) this, "ShowWarnings", Convert.ToString(this._showMessages[WizardForm.MessageType.Warning]));
    FormStorageEx.AddAttribute((Control) this, "ShowInfos", Convert.ToString(this._showMessages[WizardForm.MessageType.Info]));
    if (!(ServicesManager.GetService(typeof (IConfigurationService)) is IConfigurationService service2))
      return;
    service2.Save(Path.Combine(Application.StartupPath, "Intermech.ImpExp.Manager.cfg"));
  }

  private void AddBigImages(IBigImageList bigImageList)
  {
    Bitmap bigImages = Resources.BigImages;
    bigImages.MakeTransparent();
    bigImageList.AddStrip((Image) bigImages, new string[21]
    {
      "imgOK",
      "imgCancel",
      "imgCut",
      "imgConvert",
      "imgLostData",
      "imgConnect",
      "imgReadData",
      "imgMeasure",
      "imgImbaseCatalogsBinding",
      "imgImbaseFieldsBinding",
      "imgArchiveParams",
      "imgSearchData",
      "imgRouterParams",
      "imgTechObjTypes",
      "imgTechParams",
      "imgSearchMetadata",
      "imgImportMetadata",
      "imgImportData",
      "imgComplited",
      "imgEmpty",
      "imgPhysError"
    });
  }

  private bool LogPanelVisible
  {
    get
    {
      foreach (WizardForm.MessageType key in Enum.GetValues(typeof (WizardForm.MessageType)))
      {
        if (this._showMessages[key])
          return true;
      }
      return false;
    }
  }

  private void ShowMessages_CheckedChanged(object sender, EventArgs e)
  {
    this.splitContainer1.Panel2Collapsed = !this.LogPanelVisible;
  }

  private void BasesInfo_Click(object sender, EventArgs e)
  {
    using (BasesInfo basesInfo = new BasesInfo(this._plugins))
    {
      int num = (int) basesInfo.ShowDialog();
    }
  }

  public void SetSteps()
  {
    this.Invoke((Delegate) new MethodInvoker(this.StepsInit));
    this.Invoke((Delegate) new MethodInvoker(this.OnStepUpdate));
  }

  internal void SetButtonBasesInfoEnabled()
  {
    this.Invoke((Delegate) (() => this.bBasesInfo.Enabled = true));
  }

  private void CopySelectedToClipboard()
  {
    if (this.iGrid1.SelectedCells.Count <= 0)
      return;
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < this.iGrid1.SelectedCells.Count; ++index)
    {
      iGCell selectedCell = this.iGrid1.SelectedCells[index];
      stringBuilder.AppendLine(selectedCell.Text);
    }
    if (stringBuilder.Length <= 0)
      return;
    Clipboard.SetText(stringBuilder.ToString());
  }

  private void Grid_KeyDown(object sender, KeyEventArgs e)
  {
    if ((!e.Control || e.KeyCode != Keys.C) && (!e.Control || e.KeyCode != Keys.Insert))
      return;
    this.CopySelectedToClipboard();
  }

  private void Copy_Click(object sender, EventArgs e) => this.CopySelectedToClipboard();

  public void ClearStatistics()
  {
    ApplicationServices.Container.GetService<PumpStatisticsService>()?.Clear();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  public void CloseManager() => this.Close();

  public void AddEventOnSaveMetadata(EventHandler handler)
  {
    this.metadataControl.OnEndSaveMetadata += handler;
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (WizardForm));
    iGColPattern iGcolPattern = new iGColPattern();
    this.iGrid1Col0CellStyle = new iGCellStyle(true);
    this.iGrid1Col0ColHdrStyle = new iGColHdrStyle(true);
    this.splitContainer1 = new SplitContainer();
    this.panel3 = new Panel();
    this.panelSteps = new Panel();
    this.panel2 = new Panel();
    this.lErrorsCount = new Label();
    this.buttonNext = new Button();
    this.lInfosCount = new Label();
    this.buttonExit = new Button();
    this.label1 = new Label();
    this.imageList1 = new ImageList(this.components);
    this.buttonPrev = new Button();
    this.label6 = new Label();
    this.label4 = new Label();
    this.lWarningsCount = new Label();
    this.iGrid1 = new iGrid();
    this.contextMenuStrip1 = new ContextMenuStrip(this.components);
    this.miCopy = new ToolStripMenuItem();
    this.iGrid1DefaultCellStyle = new iGCellStyle(true);
    this.iGrid1DefaultColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1RowTextColCellStyle = new iGCellStyle(true);
    this.panel4 = new Panel();
    this.bBasesInfo = new Button();
    this.lTime = new Label();
    this.lText = new Label();
    this.pictureBox1 = new PictureBox();
    this.imageList2 = new ImageList(this.components);
    this.toolTipController1 = new ToolTipController(this.components);
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.panel3.SuspendLayout();
    this.panel2.SuspendLayout();
    ((ISupportInitialize) this.iGrid1).BeginInit();
    this.contextMenuStrip1.SuspendLayout();
    this.panel4.SuspendLayout();
    ((ISupportInitialize) this.pictureBox1).BeginInit();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.panel3);
    this.splitContainer1.Panel1.Controls.Add((Control) this.panel2);
    this.splitContainer1.Panel2.Controls.Add((Control) this.iGrid1);
    this.panel3.Controls.Add((Control) this.panelSteps);
    componentResourceManager.ApplyResources((object) this.panel3, "panel3");
    this.panel3.Name = "panel3";
    this.panelSteps.BackColor = SystemColors.Control;
    componentResourceManager.ApplyResources((object) this.panelSteps, "panelSteps");
    this.panelSteps.Name = "panelSteps";
    this.panel2.Controls.Add((Control) this.lErrorsCount);
    this.panel2.Controls.Add((Control) this.buttonNext);
    this.panel2.Controls.Add((Control) this.lInfosCount);
    this.panel2.Controls.Add((Control) this.buttonExit);
    this.panel2.Controls.Add((Control) this.label1);
    this.panel2.Controls.Add((Control) this.buttonPrev);
    this.panel2.Controls.Add((Control) this.label6);
    this.panel2.Controls.Add((Control) this.label4);
    this.panel2.Controls.Add((Control) this.lWarningsCount);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    componentResourceManager.ApplyResources((object) this.lErrorsCount, "lErrorsCount");
    this.lErrorsCount.Name = "lErrorsCount";
    componentResourceManager.ApplyResources((object) this.buttonNext, "buttonNext");
    this.buttonNext.Name = "buttonNext";
    this.buttonNext.UseVisualStyleBackColor = true;
    this.buttonNext.Click += new EventHandler(this.Next_Click);
    componentResourceManager.ApplyResources((object) this.lInfosCount, "lInfosCount");
    this.lInfosCount.Name = "lInfosCount";
    componentResourceManager.ApplyResources((object) this.buttonExit, "buttonExit");
    this.buttonExit.Name = "buttonExit";
    this.buttonExit.UseVisualStyleBackColor = true;
    this.buttonExit.Click += new EventHandler(this.Exit_Click);
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.ImageList = this.imageList1;
    this.label1.Name = "label1";
    this.label1.Tag = (object) "2";
    this.label1.Click += new EventHandler(this.ShowsClick);
    this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
    this.imageList1.TransparentColor = Color.Fuchsia;
    this.imageList1.Images.SetKeyName(0, "ok.ico");
    this.imageList1.Images.SetKeyName(1, "warn.ico");
    this.imageList1.Images.SetKeyName(2, "error.ico");
    this.imageList1.Images.SetKeyName(3, "ok_act.ico");
    this.imageList1.Images.SetKeyName(4, "warn_act.ico");
    this.imageList1.Images.SetKeyName(5, "error_act.ico");
    componentResourceManager.ApplyResources((object) this.buttonPrev, "buttonPrev");
    this.buttonPrev.Name = "buttonPrev";
    this.buttonPrev.UseVisualStyleBackColor = true;
    this.buttonPrev.Click += new EventHandler(this.Prev_Click);
    componentResourceManager.ApplyResources((object) this.label6, "label6");
    this.label6.ImageList = this.imageList1;
    this.label6.Name = "label6";
    this.label6.Tag = (object) "0";
    this.label6.Click += new EventHandler(this.ShowsClick);
    componentResourceManager.ApplyResources((object) this.label4, "label4");
    this.label4.ImageList = this.imageList1;
    this.label4.Name = "label4";
    this.label4.Tag = (object) "1";
    this.label4.Click += new EventHandler(this.ShowsClick);
    componentResourceManager.ApplyResources((object) this.lWarningsCount, "lWarningsCount");
    this.lWarningsCount.Name = "lWarningsCount";
    this.iGrid1.AutoResizeCols = true;
    iGcolPattern.CellStyle = this.iGrid1Col0CellStyle;
    iGcolPattern.ColHdrStyle = this.iGrid1Col0ColHdrStyle;
    iGcolPattern.SortOrder = iGSortOrder.None;
    componentResourceManager.ApplyResources((object) iGcolPattern, "iGColPattern1");
    this.iGrid1.Cols.AddRange(new iGColPattern[1]
    {
      iGcolPattern
    });
    this.iGrid1.ContextMenuStrip = this.contextMenuStrip1;
    this.iGrid1.DefaultCol.CellStyle = this.iGrid1DefaultCellStyle;
    this.iGrid1.DefaultCol.ColHdrStyle = this.iGrid1DefaultColHdrStyle;
    componentResourceManager.ApplyResources((object) this.iGrid1, "iGrid1");
    this.iGrid1.Header.Height = (int) componentResourceManager.GetObject("iGrid1.Header.Height");
    this.iGrid1.ImageList = this.imageList1;
    this.iGrid1.Name = "iGrid1";
    this.iGrid1.ReadOnly = true;
    this.iGrid1.RowTextCol.CellStyle = this.iGrid1RowTextColCellStyle;
    this.iGrid1.SelectionMode = iGSelectionMode.MultiExtended;
    this.iGrid1.KeyDown += new KeyEventHandler(this.Grid_KeyDown);
    this.contextMenuStrip1.Items.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this.miCopy
    });
    this.contextMenuStrip1.Name = "contextMenuStrip1";
    componentResourceManager.ApplyResources((object) this.contextMenuStrip1, "contextMenuStrip1");
    this.miCopy.Name = "miCopy";
    componentResourceManager.ApplyResources((object) this.miCopy, "miCopy");
    this.miCopy.Click += new EventHandler(this.Copy_Click);
    this.panel4.BackColor = Color.PowderBlue;
    componentResourceManager.ApplyResources((object) this.panel4, "panel4");
    this.panel4.Controls.Add((Control) this.bBasesInfo);
    this.panel4.Controls.Add((Control) this.lTime);
    this.panel4.Controls.Add((Control) this.lText);
    this.panel4.Controls.Add((Control) this.pictureBox1);
    this.panel4.Name = "panel4";
    componentResourceManager.ApplyResources((object) this.bBasesInfo, "bBasesInfo");
    this.bBasesInfo.Image = (Image) Resources.data_information1;
    this.bBasesInfo.Name = "bBasesInfo";
    this.toolTipController1.SetToolTip((Control) this.bBasesInfo, "Информация о базах");
    this.bBasesInfo.UseVisualStyleBackColor = true;
    this.bBasesInfo.Click += new EventHandler(this.BasesInfo_Click);
    componentResourceManager.ApplyResources((object) this.lTime, "lTime");
    this.lTime.BackColor = Color.Transparent;
    this.lTime.ForeColor = Color.DimGray;
    this.lTime.Name = "lTime";
    componentResourceManager.ApplyResources((object) this.lText, "lText");
    this.lText.BackColor = Color.PowderBlue;
    this.lText.ForeColor = Color.DimGray;
    this.lText.Name = "lText";
    this.pictureBox1.BackColor = Color.Transparent;
    this.pictureBox1.Image = (Image) Resources.data_next;
    componentResourceManager.ApplyResources((object) this.pictureBox1, "pictureBox1");
    this.pictureBox1.Name = "pictureBox1";
    this.pictureBox1.TabStop = false;
    this.imageList2.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList2.ImageStream");
    this.imageList2.TransparentColor = Color.Transparent;
    this.imageList2.Images.SetKeyName(0, "check2.png");
    this.imageList2.Images.SetKeyName(1, "registry.png");
    this.imageList2.Images.SetKeyName(2, "transform.png");
    this.imageList2.Images.SetKeyName(3, "delete.png");
    this.toolTipController1.Style = new ViewStyle("ToolTip style");
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.splitContainer1);
    this.Controls.Add((Control) this.panel4);
    this.Name = nameof (WizardForm);
    this.FormClosing += new FormClosingEventHandler(this.WizardForm_FormClosing);
    this.Load += new EventHandler(this.WizardForm_Load);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.panel2.PerformLayout();
    ((ISupportInitialize) this.iGrid1).EndInit();
    this.contextMenuStrip1.ResumeLayout(false);
    this.panel4.ResumeLayout(false);
    this.panel4.PerformLayout();
    ((ISupportInitialize) this.pictureBox1).EndInit();
    this.ResumeLayout(false);
  }

  private delegate void stepUpdateDelegate();

  private delegate void setButtonEnable();

  [TypeConverter(typeof (EnumDescConverter))]
  [Description("Типы сообщений")]
  private enum MessageType
  {
    [Description("Информация")] Info,
    [Description("Предупреждение")] Warning,
    [Description("Ошибка")] Error,
  }

  private delegate void ShowMessageDelegate(string str, WizardForm.MessageType type);

  private class RowTag
  {
    public WizardForm.MessageType Type;
    public int MessageHash;
    public int Count;

    public RowTag(WizardForm.MessageType type, int hash)
    {
      this.Type = type;
      this.MessageHash = hash;
      this.Count = 1;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.StepControls.StepControlSplash
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.ImpExp.Manager.Caches;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager.StepControls;

public class StepControlSplash : StepControl
{
  private Image _image;
  private IContainer components;
  private Panel panel1;
  private LinkLabel llClearCache;
  private LinkLabel llClearSettings;
  private Label label3;
  private Label label2;
  private Panel panel2;
  private ToolTip toolTip1;
  private Button bSettings;

  public StepControlSplash(object owner)
    : base(owner)
  {
    this.InitializeComponent();
    this.stepPrevAllowed = false;
    SavePoint savePoint = (ServicesManager.GetService(typeof (ISavePoint)) as ISavePoint).GetSavePoint();
    if (savePoint == null || savePoint.OperationTerminateType == TerminateType.None)
    {
      this.llClearCache.Visible = this.label2.Visible = this.CacheFilesExists(CacheHelper.GetCacheFiles());
      this.llClearSettings.Visible = this.label3.Visible = this.CacheFilesExists(SettingsHelper.GetSettingsFiles());
    }
    else
      this.llClearCache.Visible = this.label2.Visible = this.llClearSettings.Visible = this.label3.Visible = false;
  }

  private bool CacheFilesExists(List<string> filesSet)
  {
    foreach (string files in filesSet)
    {
      FileInfo fileInfo = new FileInfo(files);
      if (fileInfo.Exists && fileInfo.Length > 0L)
        return true;
    }
    return false;
  }

  protected override string getCaption() => "Подключение к базам и получение метаданных";

  private bool PumpIsComplete(SavePoint sp, Guid pumpGuid)
  {
    return sp.PumpCompleted != null && sp.PumpCompleted.Contains(pumpGuid);
  }

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
      this._image = service.ImageList.Images[service.ImageIndex("imgConnect")];
    return this._image;
  }

  public override SaveSettingsResult SaveSettings()
  {
    IMetadataInfo service1 = ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    WizardForm owner = this.owner as WizardForm;
    if (!service1.Login())
    {
      int num = (int) MessageBox.Show("Не удалось подключиться к серверу приложений IPS", "Подключение к серверу", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return SaveSettingsResult.ssrRetry;
    }
    SplashLoadingForm splashLoadingForm = new SplashLoadingForm();
    splashLoadingForm.Start();
    while (!splashLoadingForm.Visible)
      Thread.Sleep(100);
    splashLoadingForm.SetProgressText("Получение метаданных из сервера приложений IPS");
    if (!service1.MetadataLoadFromServer())
    {
      splashLoadingForm.CloseForm();
      string str = "Ошибка при получении метаданных с сервера. Дальнейшая работа невозможна.";
      int num = (int) MessageBox.Show(str, "Ошибка получения метаданных", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      owner.AddErrorMessage(str);
      return SaveSettingsResult.ssrError;
    }
    splashLoadingForm.SetProgressText("Загрузка сохраненных настроек");
    owner.LoadSettings();
    splashLoadingForm.SetProgressText("Загрузка плагинов");
    owner.LoadPlugins();
    if (splashLoadingForm != null && splashLoadingForm.Visible)
      splashLoadingForm.CloseForm();
    ISavePoint service2 = ServicesManager.GetService(typeof (ISavePoint)) as ISavePoint;
    int num1 = 1;
    SavePoint savePoint = service2.GetSavePoint();
    bool flag1 = savePoint != null && savePoint.OperationTerminateType == TerminateType.Pump;
    bool flag2 = service2.IsResumeMode(savePoint);
    IConfigurationService service3 = ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService;
    List<IPumpTask> pumpTaskList1 = new List<IPumpTask>();
    List<IPumpTask> pumpTaskList2 = new List<IPumpTask>();
    List<IPumpTask> pumpTaskList3 = new List<IPumpTask>();
    foreach (IPlugin plugins in owner._plugins.PluginsList)
    {
      if (plugins != null)
      {
        if (!plugins.IsConnected() && !plugins.BaseConnect())
        {
          int num2 = (int) MessageBox.Show($"Не удалось произвести соединение для плагина {plugins.Name}");
          return SaveSettingsResult.ssrError;
        }
        if (savePoint == null || savePoint != null && (savePoint.OperationTerminateType == TerminateType.None || savePoint.OperationTerminateType == TerminateType.Complete))
        {
          foreach (StepControl settingsControl in plugins.GetSettingsControls())
          {
            if (settingsControl != null)
            {
              bool flag3 = false;
              if (savePoint != null && savePoint.OperationTerminateType == TerminateType.Complete)
              {
                if (settingsControl.StepRepumpble)
                  flag3 = true;
              }
              else
                flag3 = true;
              if (flag3 && (settingsControl.isMetadataSettingsStep || service3.Configuration.DataMigrate && !settingsControl.isMetadataSettingsStep))
                owner.AddSettings(settingsControl, num1++);
            }
          }
          foreach (IPumpTask verification in plugins.GetVerifications())
          {
            bool flag4 = false;
            if (savePoint != null && (savePoint.OperationTerminateType == TerminateType.Complete || savePoint.OperationTerminateType == TerminateType.Pump && savePoint.RePumpMode))
            {
              if (verification.Repumpble)
                flag4 = true;
            }
            else
              flag4 = true;
            if (flag4)
            {
              if (verification.Type == PumpTaskType.ExamMetadata)
                owner.initsCollection.Add(verification);
              else if (service3.Configuration.DataMigrate && verification.Type == PumpTaskType.ExamData)
                owner.initsCollection.Add(verification);
            }
          }
        }
        foreach (IPumpTask pump in plugins.GetPumps())
        {
          if (flag2)
          {
            if (pump.Repumpble && !this.PumpIsComplete(savePoint, pump.GUID))
            {
              if (pump.Type == PumpTaskType.PumpData)
                pumpTaskList2.Add(pump);
              else if (pump.Type == PumpTaskType.PumpMetadata)
                pumpTaskList1.Add(pump);
            }
          }
          else
          {
            bool flag5 = true;
            if (flag1 && this.PumpIsComplete(savePoint, pump.GUID))
              flag5 = false;
            if (flag5)
            {
              if (pump.Type == PumpTaskType.PumpData)
                pumpTaskList2.Add(pump);
              else if (pump.Type == PumpTaskType.PumpMetadata)
                pumpTaskList1.Add(pump);
            }
          }
        }
        IPumpTask[] finalPumps = plugins.GetFinalPumps();
        if (finalPumps != null)
        {
          foreach (IPumpTask pumpTask in finalPumps)
          {
            if (flag2)
            {
              if (pumpTask.Repumpble && !this.PumpIsComplete(savePoint, pumpTask.GUID))
                pumpTaskList3.Add(pumpTask);
            }
            else
            {
              bool flag6 = true;
              if (flag1 && this.PumpIsComplete(savePoint, pumpTask.GUID))
                flag6 = false;
              if (flag6)
                pumpTaskList3.Add(pumpTask);
            }
          }
        }
      }
    }
    if (TraceSupport.PluginConnections.Enabled)
      Trace.WriteLine("StepControlSplash.SaveSettings(): end plugins foreach");
    IPumpTask taskPump1 = new DropIndexesPumper().TaskPump;
    if (service3.Configuration.DropIndexes)
    {
      bool flag7 = true;
      if (flag1 && savePoint.PumpCompleted != null && savePoint.PumpCompleted.Contains(taskPump1.GUID))
        flag7 = false;
      if (flag7)
        owner.pumpsCollection.Add(taskPump1);
    }
    foreach (IPumpTask pumpTask in pumpTaskList1)
      owner.pumpsCollection.Add(pumpTask);
    if (service3.Configuration.DataMigrate)
    {
      foreach (IPumpTask pumpTask in pumpTaskList2)
        owner.pumpsCollection.Add(pumpTask);
    }
    if (pumpTaskList3.Count > 0)
    {
      foreach (IPumpTask pumpTask in pumpTaskList3)
      {
        if (pumpTask.Type == PumpTaskType.PumpMetadata || service3.Configuration.DataMigrate && pumpTask.Type == PumpTaskType.PumpData)
          owner.pumpsCollection.Add(pumpTask);
      }
    }
    IPumpTask taskPump2 = new CreateIndexesPumper().TaskPump;
    if (service3.Configuration.DropIndexes)
      owner.pumpsCollection.Add(taskPump2);
    MeasureHelper.Init(owner.DataWriter.GetUserSession().GetMeasuresList());
    owner.SetSteps();
    owner.SetButtonBasesInfoEnabled();
    return SaveSettingsResult.ssrOk;
  }

  private void llClearCache_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
  {
    foreach (string cacheFile in CacheHelper.GetCacheFiles())
    {
      if (new FileInfo(cacheFile).Exists)
        File.Delete(cacheFile);
    }
    this.llClearCache.Enabled = false;
    (this.owner as WizardForm).AddInfoMessage("Произведена очистка кэша импорта предыдущей закачки");
    (this.owner as WizardForm).ClearStatistics();
  }

  private void llClearSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
  {
    foreach (string settingsFile in SettingsHelper.GetSettingsFiles())
    {
      if (File.Exists(settingsFile))
        File.Delete(settingsFile);
    }
    this.llClearSettings.Enabled = false;
    string[] files = Directory.GetFiles(CacheHelper.CacheFolder);
    if (files != null)
    {
      foreach (string str in files)
      {
        if (new FileInfo(str).Extension.EndsWith(".bak"))
          File.Copy(str, str.Remove(str.Length - ".bak".Length));
      }
    }
    (this.owner as WizardForm).AddInfoMessage("Произведена очистка настроек предыдущей закачки");
  }

  private void bSettings_Click(object sender, EventArgs e)
  {
    IConfigurationService service = ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService;
    using (MainConfigurationEditor configurationEditor = new MainConfigurationEditor())
    {
      configurationEditor.Initialize(service.Configuration);
      int num = (int) configurationEditor.ShowDialog();
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this.panel1 = new Panel();
    this.bSettings = new Button();
    this.llClearCache = new LinkLabel();
    this.label2 = new Label();
    this.label3 = new Label();
    this.llClearSettings = new LinkLabel();
    this.panel2 = new Panel();
    this.toolTip1 = new ToolTip(this.components);
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.llClearCache);
    this.panel1.Controls.Add((Control) this.label2);
    this.panel1.Controls.Add((Control) this.label3);
    this.panel1.Controls.Add((Control) this.llClearSettings);
    this.panel1.Dock = DockStyle.Fill;
    this.panel1.Location = new Point(0, 74);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(524, 232);
    this.panel1.TabIndex = 1;
    this.bSettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bSettings.Location = new Point(356, 25);
    this.bSettings.Name = "bSettings";
    this.bSettings.Size = new Size(143, 27);
    this.bSettings.TabIndex = 7;
    this.bSettings.Text = "Настройки миграции";
    this.bSettings.UseVisualStyleBackColor = true;
    this.bSettings.Click += new EventHandler(this.bSettings_Click);
    this.llClearCache.AutoSize = true;
    this.llClearCache.Location = new Point(42, 44);
    this.llClearCache.Name = "llClearCache";
    this.llClearCache.Size = new Size(78, 13);
    this.llClearCache.TabIndex = 0;
    this.llClearCache.TabStop = true;
    this.llClearCache.Text = "Очистка кэша";
    this.llClearCache.LinkClicked += new LinkLabelLinkClickedEventHandler(this.llClearCache_LinkClicked);
    this.label2.AutoSize = true;
    this.label2.Location = new Point(42, 31 /*0x1F*/);
    this.label2.Name = "label2";
    this.label2.Size = new Size(368, 13);
    this.label2.TabIndex = 2;
    this.label2.Text = "Внимание, обнаружены файлы кэша предыдущего процесса миграции";
    this.label3.AutoSize = true;
    this.label3.Location = new Point(42, 68);
    this.label3.Name = "label3";
    this.label3.Size = new Size(423, 13);
    this.label3.TabIndex = 3;
    this.label3.Text = "Внимание, обнаружены настройки метаданных предыдущего процесса миграции";
    this.llClearSettings.AutoSize = true;
    this.llClearSettings.Location = new Point(42, 84);
    this.llClearSettings.Name = "llClearSettings";
    this.llClearSettings.Size = new Size(99, 13);
    this.llClearSettings.TabIndex = 1;
    this.llClearSettings.TabStop = true;
    this.llClearSettings.Text = "Очистка настроек";
    this.llClearSettings.LinkClicked += new LinkLabelLinkClickedEventHandler(this.llClearSettings_LinkClicked);
    this.panel2.Controls.Add((Control) this.bSettings);
    this.panel2.Dock = DockStyle.Top;
    this.panel2.Location = new Point(0, 0);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(524, 74);
    this.panel2.TabIndex = 2;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.panel2);
    this.Name = nameof (StepControlSplash);
    this.Size = new Size(524, 306);
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}

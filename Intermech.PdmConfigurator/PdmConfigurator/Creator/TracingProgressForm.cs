// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.Creator.TracingProgressForm
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.PdmConfigurator;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.PdmConfigurator.Creator;

public sealed class TracingProgressForm : Form
{
  private Dictionary<long, ObjectVersionDescription> _dictionary = new Dictionary<long, ObjectVersionDescription>();
  private long _orderObjectID;
  private int _orderTypeID = MetaDataHelper.GetObjectTypeID("cad00580-306c-11d8-b4e9-00304f19f545");
  private Thread _thread;
  private Guid _browserID;
  private PdmCompositionBrowserJobStatus _pdmCompositionBrowserJobStatus;
  private SortedDictionary<RelationPath, TraceEntry> _errors = new SortedDictionary<RelationPath, TraceEntry>();
  private object _syncObject = new object();
  private IFiltrationService _filtrationService;
  private IContainer components;
  private Label labelInfo;
  private Button _cancelButton;
  private PictureBox pictureInfo6;
  private System.Windows.Forms.Timer _refreshTimer;

  public TracingProgressForm(
    Dictionary<long, ObjectVersionDescription> dictionary,
    long orderObjectID)
  {
    this.InitializeComponent();
    this._dictionary = dictionary;
    this._orderObjectID = orderObjectID;
    this._filtrationService = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
    this.StartThread();
  }

  public static SortedDictionary<RelationPath, TraceEntry> Execute(
    Dictionary<long, ObjectVersionDescription> dictionary,
    long orderObjectID)
  {
    using (TracingProgressForm tracingProgressForm = new TracingProgressForm(dictionary, orderObjectID))
    {
      int num = (int) tracingProgressForm.ShowDialog();
      return tracingProgressForm._errors;
    }
  }

  private void TracingProgressForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    if (this.DialogResult == DialogResult.OK)
      return;
    this.CancelButton_Click(sender, (EventArgs) null);
  }

  private void CancelButton_Click(object sender, EventArgs e) => this.StopThread();

  private void RefreshTimer_Tick(object sender, EventArgs e)
  {
    this._refreshTimer.Enabled = false;
    lock (this._syncObject)
    {
      if (this._thread != null)
      {
        if (this._pdmCompositionBrowserJobStatus != null)
        {
          if (this._pdmCompositionBrowserJobStatus.Progress != PdmCompositionBrowserJobProgress.NotStarted)
          {
            if (this._pdmCompositionBrowserJobStatus.Progress == PdmCompositionBrowserJobProgress.Working)
              goto label_9;
          }
          else
            goto label_9;
        }
        else
          goto label_9;
      }
      this.StopThread();
      this.DialogResult = DialogResult.OK;
      return;
    }
label_9:
    this._refreshTimer.Enabled = true;
  }

  private void StartThread()
  {
    this.StopThread();
    using (FixEditingContext fixEditingContext = new FixEditingContext())
    {
      this._thread = new Thread(fixEditingContext.SendEditingContextToThread(new ThreadStart(this.ThreadMethod)));
      this._thread.IsBackground = true;
      this._thread.Name = "PdmConfigurator.CompositionTracing";
      this._thread.Start();
    }
    this._refreshTimer.Enabled = true;
  }

  private void ThreadMethod()
  {
    lock (this._syncObject)
      this._pdmCompositionBrowserJobStatus = (PdmCompositionBrowserJobStatus) null;
    this.Browse();
  }

  private void Browse()
  {
    PdmCompositionBrowserJobStatus browserJobStatus1 = (PdmCompositionBrowserJobStatus) null;
    this._errors.Clear();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IPdmConfiguratorService)) is IPdmConfiguratorService customService))
      {
        lock (this._syncObject)
          ;
        this._thread = (Thread) null;
      }
      else
      {
        RelationPair rootObject = new RelationPair(sessionKeeper.Session.ClientConnectionID, this._orderObjectID, this._orderTypeID, 0L, sessionKeeper.Session.UserID, this._orderObjectID, -1, this._orderTypeID);
        RelationPath rootObjectPath = new RelationPath();
        CompositionObjects objs = new CompositionObjects();
        foreach (int key in this._dictionary.Keys)
        {
          ObjectVersionDescription versionDescription = this._dictionary[(long) key];
          IDBRelation relation = sessionKeeper.Session.GetRelation((long) key, false);
          if (relation != null)
          {
            rootObjectPath.Items.Add(new SimpleRelationPair((long) key, relation.RelationType, rootObject.TOP_OBJECT_ID, rootObject.TOP_OBJECT_TYPE));
            CompositionObject compositionObject = new CompositionObject(versionDescription.F_ID, versionDescription.F_OBJECT_ID, versionDescription.F_OBJECT_TYPE, versionDescription.F_LCSTEP_ID, versionDescription.F_OWNER_ID, versionDescription.F_CHKOUT_BY, versionDescription.CAPTION, versionDescription.F_VERSION_ID, versionDescription.F_MODIFICATION_ID, versionDescription.F_BASE_VERSION, versionDescription.Options, (CompositionObjects) null, (long) key, this._orderObjectID, relation.RelationType, string.Empty);
            objs.Add(compositionObject);
          }
        }
        this._browserID = customService.Browse(sessionKeeper.Session.SessionGUID, rootObject, rootObjectPath, objs, new PdmCompositionBrowserEventArgs(-1, this._filtrationService.FiltrationServiceOwnerID, (VersionsRule) null, (HybridDictionary) null, true, false));
        while (!(this._browserID == Guid.Empty))
        {
          PdmCompositionBrowserJobStatus browserJobStatus2 = customService.QueryBrowserStatus(this._browserID);
          lock (this._syncObject)
            browserJobStatus1 = browserJobStatus2;
          if (browserJobStatus2 != null && (browserJobStatus2.Progress == PdmCompositionBrowserJobProgress.NotStarted || browserJobStatus2.Progress == PdmCompositionBrowserJobProgress.Working))
            Thread.Sleep(1000);
          else
            break;
        }
        if (browserJobStatus1 != null)
        {
          SortedDictionary<RelationPath, TraceEntry> items = browserJobStatus1.Trace.Items;
          foreach (RelationPath key in items.Keys)
          {
            TraceEntry traceEntry = items[key];
            if (traceEntry.Flags != PdmConfiguratorResult.False && traceEntry.Flags != PdmConfiguratorResult.True)
              this._errors.Add(key, traceEntry);
          }
        }
        this._thread = (Thread) null;
      }
    }
  }

  private void StopThread()
  {
    if (this._browserID != Guid.Empty)
      return;
    ((ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IPdmConfiguratorService)) as IPdmConfiguratorService).CancelBrowse(this._browserID);
    this._browserID = Guid.Empty;
    if (this._thread != null)
      this._thread.Abort();
    this._thread = (Thread) null;
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TracingProgressForm));
    this.labelInfo = new Label();
    this._cancelButton = new Button();
    this.pictureInfo6 = new PictureBox();
    this._refreshTimer = new System.Windows.Forms.Timer(this.components);
    ((ISupportInitialize) this.pictureInfo6).BeginInit();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.labelInfo, "labelInfo");
    this.labelInfo.Name = "labelInfo";
    this._cancelButton.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this._cancelButton, "_cancelButton");
    this._cancelButton.Name = "_cancelButton";
    this._cancelButton.UseVisualStyleBackColor = true;
    this._cancelButton.Click += new EventHandler(this.CancelButton_Click);
    componentResourceManager.ApplyResources((object) this.pictureInfo6, "pictureInfo6");
    this.pictureInfo6.Name = "pictureInfo6";
    this.pictureInfo6.TabStop = false;
    this._refreshTimer.Interval = 500;
    this._refreshTimer.Tick += new EventHandler(this.RefreshTimer_Tick);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.pictureInfo6);
    this.Controls.Add((Control) this._cancelButton);
    this.Controls.Add((Control) this.labelInfo);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (TracingProgressForm);
    this.FormClosing += new FormClosingEventHandler(this.TracingProgressForm_FormClosing);
    ((ISupportInitialize) this.pictureInfo6).EndInit();
    this.ResumeLayout(false);
  }
}

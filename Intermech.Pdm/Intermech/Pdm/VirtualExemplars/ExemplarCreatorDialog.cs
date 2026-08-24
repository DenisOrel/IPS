// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VirtualExemplars.ExemplarCreatorDialog
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using ImSSP;
using Intermech.Client.Core.FormDesigner.Navigator;
using Intermech.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.VirtualExemplars;

internal class ExemplarCreatorDialog : Form
{
  private ArrayList CreatorSteps = new ArrayList();
  private int _currentStep;
  private long _newInstanceID;
  private bool _isNew = true;
  private long _articleID;
  private ExemplarsTreeCreator _tree;
  private IContainer components;
  private Panel panel1;
  private Panel panel2;
  private Button bCancel;
  private Button bNext;
  private Button bPreview;
  private Panel pSteps;
  private Button bOK;
  private Label label2;
  private PictureBox pictureBox1;

  public ExemplarCreatorDialog()
  {
    this.InitializeComponent();
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1487);
  }

  public bool SetFormData(
    SessionKeeper sk,
    long articleID,
    int articleType,
    ArticlesInManufacture attrInManuf)
  {
    this._articleID = articleID;
    IDBObjectType instanceObjectType = PDMHelper.GetInstanceObjectType(sk.Session, articleType, attrInManuf);
    this.Text = string.Format(LocalizationHolder.rm.GetString("Pdm_404"));
    if (instanceObjectType == null)
    {
      int num = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString("Pdm_550"), string.Format(LocalizationHolder.rm.GetString("Pdm_551"), (object) MetaDataHelper.GetObjectTypeName(articleType)), MessageBoxButtons.OK, IMMessageBoxImage.Error);
      return false;
    }
    int objectType = instanceObjectType.ObjectType;
    ICategoryTypeIconService service = (ICategoryTypeIconService) ServicesManager.GetService(typeof (ICategoryTypeIconService));
    if (service != null)
    {
      this.label2.Text = instanceObjectType.ObjectInstanceName;
      this.pictureBox1.Image = service.BigImageList.Images[service.IndexOf(4, instanceObjectType.ObjectType)];
    }
    this._tree = new ExemplarsTreeCreator(this._articleID);
    if (!this._tree.CheckTree(sk))
      return false;
    this._newInstanceID = sk.Session.GetObjectCollection(objectType).Create(articleID).ObjectID;
    ICollection<FormInformation> formInformations = (ICollection<FormInformation>) null;
    IFormDesignerService customService = (IFormDesignerService) sk.Session.GetCustomService(typeof (IFormDesignerService));
    if (customService != null)
      formInformations = customService.GetFormsForObject(this._newInstanceID, sk.Session.SessionGUID);
    if (formInformations == null || formInformations.Count == 0)
      throw new Exception(string.Format(LocalizationHolder.rm.GetString(sc_16950.ssp_pdm_16951()), (object) instanceObjectType.ObjectTypeName));
    int index = 0;
    foreach (FormInformation formInformation in (IEnumerable<FormInformation>) formInformations)
    {
      FormDesignerView formDesignerView = new FormDesignerView(this._newInstanceID, formInformation.ID);
      string errorMsg = string.Empty;
      if (!formDesignerView.LoadForm(sk.Session.GetObject(formInformation.ID), out errorMsg))
      {
        int num = (int) MessageBox.Show(errorMsg, LocalizationHolder.rm.GetString(sc_16950.ssp_pdm_16952()));
      }
      else
        formDesignerView.ButtonsVisible(false);
      this.CreatorSteps.Insert(index, (object) formDesignerView);
      ++index;
    }
    this._currentStep = 0;
    this.SetCurrentPage(this._currentStep, -1);
    return true;
  }

  private void SetCurrentPage(int newPageID, int oldPageID)
  {
    if (oldPageID != -1 && newPageID >= oldPageID)
      this.AfterPageAction(oldPageID);
    this.pSteps.Controls.Clear();
    UserControl creatorStep = this.CreatorSteps[newPageID] as UserControl;
    this.pSteps.Controls.Add((Control) creatorStep);
    creatorStep.Dock = DockStyle.Fill;
    this.BeforePageAction(newPageID);
  }

  private void BeforePageAction(int pageID)
  {
    this.bNext.Visible = pageID < this.CreatorSteps.Count - 1;
    this.bPreview.Visible = pageID > 0;
    this.bOK.Visible = true;
  }

  private void AfterPageAction(int pageID)
  {
    UserControl creatorStep = (UserControl) this.CreatorSteps[pageID];
    if (!(creatorStep is FormDesignerView))
      return;
    FormDesignerView formDesignerView = creatorStep as FormDesignerView;
    if (!formDesignerView.FormChanged)
      return;
    string errorMsg = string.Empty;
    if (formDesignerView.SaveForm(out errorMsg))
      return;
    int num = (int) MessageBox.Show(errorMsg, LocalizationHolder.rm.GetString(sc_16950.ssp_pdm_16953()));
  }

  private void Cancel()
  {
    if (this._newInstanceID == 0L || !this._isNew)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      sessionKeeper.Session.GetObject(this._newInstanceID)?.Delete(0L);
      this._newInstanceID = 0L;
    }
  }

  private void bCancel_Click(object sender, EventArgs e)
  {
    this.Cancel();
    this.DialogResult = DialogResult.Cancel;
    this.Close();
  }

  private void bPreview_Click(object sender, EventArgs e)
  {
    --this._currentStep;
    this.SetCurrentPage(this._currentStep, this._currentStep + 1);
  }

  private void bNext_Click(object sender, EventArgs e)
  {
    ++this._currentStep;
    this.SetCurrentPage(this._currentStep, this._currentStep - 1);
  }

  private void bOK_Click(object sender, EventArgs e)
  {
    this.SetCurrentPage(this._currentStep, this._currentStep);
    if (this._newInstanceID == 0L)
      return;
    INotificationService service = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    List<NotificationEventArgs> notificationEventArgsList = new List<NotificationEventArgs>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBTransactions customService = (IDBTransactions) sessionKeeper.Session.GetCustomService(typeof (IDBTransactions));
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._newInstanceID);
      long objectId = dbObject.ObjectID;
      try
      {
        customService.StartTransaction();
        IDBAttributeType attributeType1 = sessionKeeper.Session.GetAttributeType(new Guid("cad00622-306c-11d8-b4e9-00304f19f545"));
        dbObject.Attributes.AddAttribute(attributeType1.AttributeID, false).Value = (object) Math.Abs(this._articleID);
        dbObject.CommitCreation(false);
        this._newInstanceID = dbObject.CheckOut(false).ObjectID;
        notificationEventArgsList.Add((NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", this._newInstanceID));
        IDBRelationType relationType = sessionKeeper.Session.GetRelationType(new Guid("cad00154-306c-11d8-b4e9-00304f19f545"));
        IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(relationType.RelationType);
        DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new object[3]
        {
          (object) -20,
          (object) -22,
          (object) -2
        });
        IDBAttributeType attributeType2 = sessionKeeper.Session.GetAttributeType(new Guid("cad001c2-306c-11d8-b4e9-00304f19f545"));
        foreach (DataRow row in (InternalDataCollectionBase) relationCollection.ConsistFrom(paramSet, this._articleID).Rows)
        {
          IDBRelation dbRelation = relationCollection.Create(new NewRelationProperties(Convert.ToInt64(row[0]), this._newInstanceID, Convert.ToInt64(row[1])));
          dbRelation.Attributes.AddAttribute(attributeType2.AttributeID, false, new object[1]
          {
            (object) Math.Abs(Convert.ToInt64(row[2]))
          });
          notificationEventArgsList.Add((NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID, dbRelation.ProjID, dbRelation.RelationType));
        }
        if (this._tree.TreePresent)
        {
          this._tree.CreateTree(sessionKeeper.Session, this._newInstanceID);
          if (this._tree.NotifAfterCreateTree != null && this._tree.NotifAfterCreateTree.Count > 0)
            notificationEventArgsList.AddRange((IEnumerable<NotificationEventArgs>) this._tree.NotifAfterCreateTree);
        }
        customService.Commit();
        if (service != null)
        {
          foreach (NotificationEventArgs e1 in notificationEventArgsList)
            service.FireEvent((object) this, e1);
        }
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
      catch
      {
        customService.Rollback();
        if (this._tree != null)
          this._tree.RollbackTree();
        this._newInstanceID = objectId;
        throw;
      }
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ExemplarCreatorDialog));
    this.panel1 = new Panel();
    this.bOK = new Button();
    this.bCancel = new Button();
    this.bNext = new Button();
    this.bPreview = new Button();
    this.panel2 = new Panel();
    this.label2 = new Label();
    this.pictureBox1 = new PictureBox();
    this.pSteps = new Panel();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    ((ISupportInitialize) this.pictureBox1).BeginInit();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.bOK);
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bNext);
    this.panel1.Controls.Add((Control) this.bPreview);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.bOK, "bOK");
    this.bOK.Name = "bOK";
    this.bOK.UseVisualStyleBackColor = true;
    this.bOK.Click += new EventHandler(this.bOK_Click);
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bCancel.Click += new EventHandler(this.bCancel_Click);
    componentResourceManager.ApplyResources((object) this.bNext, "bNext");
    this.bNext.Name = "bNext";
    this.bNext.UseVisualStyleBackColor = true;
    this.bNext.Click += new EventHandler(this.bNext_Click);
    componentResourceManager.ApplyResources((object) this.bPreview, "bPreview");
    this.bPreview.Name = "bPreview";
    this.bPreview.UseVisualStyleBackColor = true;
    this.bPreview.Click += new EventHandler(this.bPreview_Click);
    this.panel2.Controls.Add((Control) this.label2);
    this.panel2.Controls.Add((Control) this.pictureBox1);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    componentResourceManager.ApplyResources((object) this.label2, "label2");
    this.label2.ForeColor = SystemColors.GrayText;
    this.label2.Name = "label2";
    componentResourceManager.ApplyResources((object) this.pictureBox1, "pictureBox1");
    this.pictureBox1.Name = "pictureBox1";
    this.pictureBox1.TabStop = false;
    componentResourceManager.ApplyResources((object) this.pSteps, "pSteps");
    this.pSteps.Name = "pSteps";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.pSteps);
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (ExemplarCreatorDialog);
    this.ShowInTaskbar = false;
    this.Tag = (object) "";
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.panel2.PerformLayout();
    ((ISupportInitialize) this.pictureBox1).EndInit();
    this.ResumeLayout(false);
  }
}

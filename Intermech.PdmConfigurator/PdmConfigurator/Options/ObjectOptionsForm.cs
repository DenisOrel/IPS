// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.Options.ObjectOptionsForm
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Navigator.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.PdmConfigurator.Options;

public class ObjectOptionsForm : Form
{
  internal IServiceProvider _services;
  private ISelectedItems _items;
  private IDBObject obj;
  private IDBObject parentObject;
  private IDBRelation relation;
  private IContainer components;
  private Panel panel1;
  private Button btnCancel;
  private Button btnOk;
  private ObjectOptionsEditor editor;

  public ObjectOptionsForm(IServiceProvider provider, ISelectedItems items)
  {
    this.InitializeComponent();
    this._services = provider;
    this._items = items;
    IViewState service = this._services != null ? this._services.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    this.editor.ReadOnly = ((service != null ? (long) service.ViewState : 0L) & 2L) == 2L;
    this.LoadViewData();
  }

  public ObjectOptionsForm(
    IDBObject obj,
    IDBObject parentObj,
    IDBRelation relation,
    IServiceProvider provider,
    bool IgnoreNullRelation,
    string pdmCriterion,
    string pdmContext)
  {
    this.InitializeComponent();
    this._services = provider;
    this.obj = obj;
    this.parentObject = parentObj;
    this.relation = relation;
    IViewState service = this._services != null ? this._services.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    this.editor.ReadOnly = ((service != null ? (long) service.ViewState : 0L) & 2L) == 2L;
    this.editor._ignoreNullRelation = IgnoreNullRelation;
    this.editor._pdmContext = pdmContext;
    this.editor._pdmCriterion = pdmCriterion;
    this.LoadViewData();
  }

  public ObjectOptionsForm(
    IDBObject obj,
    IDBObject parentObj,
    IDBRelation relation,
    IServiceProvider provider)
    : this(obj, parentObj, relation, provider, false, "", "")
  {
  }

  public static DialogResult Execute(
    IDBObject obj,
    IDBObject parentObj,
    IDBRelation relation,
    IServiceProvider provider,
    ref string pdmCriterion,
    ref string pdmContext)
  {
    using (ObjectOptionsForm objectOptionsForm = new ObjectOptionsForm(obj, parentObj, relation, provider, true, pdmCriterion, pdmContext))
    {
      int num = (int) objectOptionsForm.ShowDialog();
      if (num == 1)
      {
        ObjectsApplicabilitiesCriterionsCollection criterionsCollection = new ObjectsApplicabilitiesCriterionsCollection();
        criterionsCollection.Assign((object) objectOptionsForm.editor.appEditor.PdmCriterionCollection);
        pdmCriterion = criterionsCollection.ToXMLString();
        pdmContext = objectOptionsForm.editor.contextEditor.Context.ToString();
      }
      return (DialogResult) num;
    }
  }

  private void btnCancel_Click(object sender, EventArgs e)
  {
    if (!this.editor.IsChanged)
      return;
    this.editor.Undo();
  }

  private void btnOk_Click(object sender, EventArgs e)
  {
    if (!this.editor.IsChanged)
      return;
    this.editor.Save();
  }

  internal void Clear()
  {
    this.editor.Clear();
    this.UpdateControls();
  }

  internal void LoadViewData()
  {
    this.Clear();
    if (!MetaDataHelper.IsPdmConfigurableObjectType(this.obj.ObjectType))
      return;
    this.editor.LoadInfo(this._services, this.relation != null ? PdmConfiguratorHelper.CreateKey(this.parentObject.ObjectID, this.parentObject.ObjectType, this.relation.RelationID, this.relation.RelationType, this.obj.ObjectID, this.obj.ObjectType) : PdmConfiguratorHelper.CreateKey(this.parentObject.ObjectID, this.parentObject.ObjectType, 0L, -1, this.obj.ObjectID, this.obj.ObjectType), PdmConfiguratorHelper.CreateKey(0L, -1, 0L, -1, this.parentObject.ObjectID, this.parentObject.ObjectType));
  }

  private void editor_OnChanged(object sender, EventArgs e) => this.UpdateControls();

  public void UpdateControls()
  {
    this.btnOk.Enabled = this.editor.IsChanged;
    this.btnCancel.Enabled = this.editor.IsChanged;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ObjectOptionsForm));
    this.panel1 = new Panel();
    this.btnCancel = new Button();
    this.btnOk = new Button();
    this.editor = new ObjectOptionsEditor();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.BorderStyle = BorderStyle.Fixed3D;
    this.panel1.Controls.Add((Control) this.btnCancel);
    this.panel1.Controls.Add((Control) this.btnOk);
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    componentResourceManager.ApplyResources((object) this.btnOk, "btnOk");
    this.btnOk.DialogResult = DialogResult.OK;
    this.btnOk.Name = "btnOk";
    this.btnOk.UseVisualStyleBackColor = true;
    this.btnOk.Click += new EventHandler(this.btnOk_Click);
    componentResourceManager.ApplyResources((object) this.editor, "editor");
    this.editor.DisableHeader = false;
    this.editor.IsChanged = false;
    this.editor.IsInternalChanged = false;
    this.editor.MinimumSize = new Size(600, 245);
    this.editor.Name = "editor";
    this.editor.OnChanged += new ObjectOptionsEditor.ObjectOptionsChangedEventHandler(this.editor_OnChanged);
    this.AcceptButton = (IButtonControl) this.btnOk;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.Controls.Add((Control) this.editor);
    this.Controls.Add((Control) this.panel1);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ObjectOptionsForm);
    this.ShowInTaskbar = false;
    this.panel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.ConfigurationCodeForm
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Client.Core;
using Intermech.Controls;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Localization;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.PdmConfigurator;

public sealed class ConfigurationCodeForm : Form
{
  private IDBTypedObjectID _typedObjectID;
  private bool _isReadOnly;
  private IContainer components;
  private Panel panel1;
  private Button _cancelButton;
  private Button _acceptButton;
  private ConfigurationCodeEditor _configurationCodeEditor;
  private Button _refreshButton;
  private ImageList imageList1;

  public ConfigurationCodeForm(IDBTypedObjectID objID)
  {
    this.InitializeComponent();
    this._typedObjectID = objID;
  }

  public void IsReadOnly()
  {
    this._isReadOnly = true;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._typedObjectID.ObjectID);
      if (dbObject is IDBSecurity dbSecurity && dbSecurity.CheckAccess(ActionType.Edit, true, false))
        this._isReadOnly = false;
      ObjectModifyModes objectModifyMode = dbObject.ObjectModifyMode;
      long checkoutBy = dbObject.CheckoutBy;
      if (objectModifyMode != ObjectModifyModes.CantModify && (objectModifyMode != ObjectModifyModes.Checkout || checkoutBy == sessionKeeper.Session.UserID) && (objectModifyMode != ObjectModifyModes.CreateVersion || checkoutBy == sessionKeeper.Session.UserID))
        return;
      this._isReadOnly = true;
    }
  }

  private void ConfigurationCodeForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void ConfigurationCodeForm_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void ConfigurationCodeEditor_Changed(object sender, EventArgs e) => this.UpdateControls();

  private void AcceptButton_Click(object sender, EventArgs e)
  {
    try
    {
      DialogResult dialogResult = IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_7"), LocalizationHolder.rm.GetString("PdmConfigurator_27"), MessageBoxButtons.YesNoCancel, IMMessageBoxImage.Question);
      if (dialogResult == DialogResult.Yes)
        this._configurationCodeEditor.Save();
      this.DialogResult = dialogResult == DialogResult.Cancel ? DialogResult.None : dialogResult;
    }
    catch (PdmConfiguratorExeption ex)
    {
      this.DialogResult = DialogResult.None;
      if (ex != null)
      {
        int num1 = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_7"), ex.Message, MessageBoxButtons.OK, IMMessageBoxImage.Information);
      }
      else if (ex.InnerException is PdmConfiguratorExeption)
      {
        int num2 = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_7"), ex.InnerException.Message, MessageBoxButtons.OK, IMMessageBoxImage.Information);
      }
      else
        throw;
    }
  }

  private void RefreshButton_Click(object sender, EventArgs e)
  {
  }

  private void UpdateControls()
  {
    this._acceptButton.Enabled = this._refreshButton.Enabled = this._configurationCodeEditor.IsChanged;
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ConfigurationCodeForm));
    this.panel1 = new Panel();
    this._refreshButton = new Button();
    this._cancelButton = new Button();
    this._acceptButton = new Button();
    this.imageList1 = new ImageList(this.components);
    this._configurationCodeEditor = new ConfigurationCodeEditor();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this._refreshButton);
    this.panel1.Controls.Add((Control) this._cancelButton);
    this.panel1.Controls.Add((Control) this._acceptButton);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this._refreshButton, "_refreshButton");
    this._refreshButton.Name = "_refreshButton";
    this._refreshButton.UseVisualStyleBackColor = true;
    this._refreshButton.Click += new EventHandler(this.RefreshButton_Click);
    componentResourceManager.ApplyResources((object) this._cancelButton, "_cancelButton");
    this._cancelButton.DialogResult = DialogResult.Cancel;
    this._cancelButton.Name = "_cancelButton";
    this._cancelButton.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._acceptButton, "_acceptButton");
    this._acceptButton.Name = "_acceptButton";
    this._acceptButton.UseVisualStyleBackColor = true;
    this._acceptButton.Click += new EventHandler(this.AcceptButton_Click);
    this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
    this.imageList1.TransparentColor = Color.Transparent;
    this.imageList1.Images.SetKeyName(0, "warning.png");
    componentResourceManager.ApplyResources((object) this._configurationCodeEditor, "_configurationCodeEditor");
    this._configurationCodeEditor.IsChanged = false;
    this._configurationCodeEditor.Name = "_configurationCodeEditor";
    this._configurationCodeEditor.OnChanged += new ConfigurationCodeEditor.ObjectOptionsChangedEventHandler(this.ConfigurationCodeEditor_Changed);
    this.AcceptButton = (IButtonControl) this._acceptButton;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._cancelButton;
    this.Controls.Add((Control) this._configurationCodeEditor);
    this.Controls.Add((Control) this.panel1);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ConfigurationCodeForm);
    this.ShowInTaskbar = false;
    this.FormClosing += new FormClosingEventHandler(this.ConfigurationCodeForm_FormClosing);
    this.Load += new EventHandler(this.ConfigurationCodeForm_Load);
    this.panel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}

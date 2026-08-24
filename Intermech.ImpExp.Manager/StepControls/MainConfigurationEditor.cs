// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.StepControls.MainConfigurationEditor
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.Interfaces.Client;
using Intermech.PropertyEditors.ChangeHighlighting;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager.StepControls;

public class MainConfigurationEditor : Form
{
  private bool _changed;
  private IContainer components;
  private PropertyGrid pgSettings;
  private Button bOK;
  private Button bClose;
  private Button bCancel;

  public MainConfigurationEditor()
  {
    this.InitializeComponent();
    this.RefreshControls();
  }

  public void Initialize(MainConfiguration configuration)
  {
    this.pgSettings.SelectedObject = (object) new EditableObjectChangeHighlighter((ICloneable) configuration);
    this.pgSettings.ExpandAllGridItems();
    this.ActiveControl = (Control) this.pgSettings;
  }

  private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
  {
    this._changed = true;
    this.RefreshControls();
  }

  private void bOK_Click(object sender, EventArgs e)
  {
    IConfigurationService service = ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService;
    this.Save(service);
    this.Initialize(service.Configuration);
    this._changed = false;
    this.RefreshControls();
  }

  private void bCancel_Click(object sender, EventArgs e)
  {
    this.Initialize((MainConfiguration) ((EditableObjectChangeHighlighter) this.pgSettings.SelectedObject).OriginalObject);
    this._changed = false;
    this.RefreshControls();
  }

  private void bClose_Click(object sender, EventArgs e)
  {
    if (this._changed && MessageBox.Show("На форме остались несохраненные данные. Сохранить?", "Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      this.Save(ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService);
    this.Close();
  }

  private void RefreshControls()
  {
    this.bOK.Enabled = this._changed;
    this.bCancel.Enabled = this._changed;
  }

  private void Save(IConfigurationService cfg)
  {
    EditableObjectChangeHighlighter selectedObject = (EditableObjectChangeHighlighter) this.pgSettings.SelectedObject;
    cfg.Configuration = (MainConfiguration) selectedObject.EditableObject;
    if (!(ServicesManager.GetService(typeof (IConfigurationService)) is IConfigurationService service))
      return;
    service.Save(Path.Combine(Application.StartupPath, "Intermech.ImpExp.Manager.cfg"));
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.pgSettings = new PropertyGrid();
    this.bOK = new Button();
    this.bClose = new Button();
    this.bCancel = new Button();
    this.SuspendLayout();
    this.pgSettings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.pgSettings.Location = new Point(12, 12);
    this.pgSettings.Name = "pgSettings";
    this.pgSettings.Size = new Size(567, 497);
    this.pgSettings.TabIndex = 0;
    this.pgSettings.PropertyValueChanged += new PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
    this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bOK.Location = new Point(187, 515);
    this.bOK.Name = "bOK";
    this.bOK.Size = new Size(121, 27);
    this.bOK.TabIndex = 1;
    this.bOK.Text = "Применить";
    this.bOK.UseVisualStyleBackColor = true;
    this.bOK.Click += new EventHandler(this.bOK_Click);
    this.bClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bClose.DialogResult = DialogResult.Cancel;
    this.bClose.Location = new Point(459, 515);
    this.bClose.Name = "bClose";
    this.bClose.Size = new Size(121, 27);
    this.bClose.TabIndex = 2;
    this.bClose.Text = "Закрыть";
    this.bClose.UseVisualStyleBackColor = true;
    this.bClose.Click += new EventHandler(this.bClose_Click);
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.Location = new Point(314, 515);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(121, 27);
    this.bCancel.TabIndex = 3;
    this.bCancel.Text = "Отмена";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bCancel.Click += new EventHandler(this.bCancel_Click);
    this.AcceptButton = (IButtonControl) this.bOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bClose;
    this.ClientSize = new Size(591, 558);
    this.Controls.Add((Control) this.bCancel);
    this.Controls.Add((Control) this.bClose);
    this.Controls.Add((Control) this.bOK);
    this.Controls.Add((Control) this.pgSettings);
    this.Name = nameof (MainConfigurationEditor);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Настройки миграции";
    this.ResumeLayout(false);
  }
}

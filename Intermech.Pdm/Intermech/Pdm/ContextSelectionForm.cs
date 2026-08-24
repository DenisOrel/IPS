// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ContextSelectionForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm;

public class ContextSelectionForm : Form
{
  private MyAttributeMetadata _contextAttr = new MyAttributeMetadata();
  private List<long> _selectedContexts;
  private IContainer components;
  private Button btnCancel;
  private Label lbPromt;
  private CheckedListBox cbContexts;
  private Button btnOK;

  public ContextSelectionForm(ref List<long> selectedContexts, Rectangle formBounds)
  {
    this.InitializeComponent();
    this.Bounds = formBounds;
    if (selectedContexts == null)
      selectedContexts = new List<long>(1);
    this._selectedContexts = new List<long>(selectedContexts.Count);
    for (int index = 0; index < selectedContexts.Count; ++index)
      this._selectedContexts.Add(selectedContexts[index]);
    this.FillContextsList();
    this.btnOK.Top = this.ClientSize.Height - this.btnOK.Height - 10;
    this.btnCancel.Top = this.btnOK.Top;
    this.cbContexts.Height = this.ClientSize.Height - this.cbContexts.Top - this.btnOK.Height - 20;
  }

  public static DialogResult Execute(ref List<long> selectedContexts, Rectangle formBounds)
  {
    using (ContextSelectionForm contextSelectionForm = new ContextSelectionForm(ref selectedContexts, formBounds))
    {
      int num = (int) contextSelectionForm.ShowDialog();
      if (num == 1)
        selectedContexts = contextSelectionForm._selectedContexts;
      return (DialogResult) num;
    }
  }

  private void FillContextsList()
  {
    this._contextAttr.SetByGUID("cad00651-306c-11d8-b4e9-00304f19f545");
    this.cbContexts.Items.Clear();
    if (this._contextAttr.AttrPossibleValues != null)
    {
      for (int index = 0; index < this._contextAttr.AttrPossibleValues.Count; ++index)
      {
        MyElement attrPossibleValue = this._contextAttr.AttrPossibleValues[index] as MyElement;
        this.cbContexts.Items.Add((object) attrPossibleValue, this._selectedContexts.Contains(Convert.ToInt64(attrPossibleValue.Value)));
      }
    }
    this.UpdateControls();
  }

  private void UpdateControls()
  {
    this.btnOK.Enabled = this._selectedContexts != null && this._selectedContexts.Count > 0;
    this.btnCancel.Enabled = true;
  }

  private void cbContexts_ItemCheck(object sender, ItemCheckEventArgs e)
  {
    if (this._contextAttr.AttrPossibleValues == null || this._contextAttr.AttrPossibleValues.Count <= e.Index)
      return;
    long int64 = Convert.ToInt64((this._contextAttr.AttrPossibleValues[e.Index] as MyElement).Value);
    this._selectedContexts.Remove(int64);
    if (e.NewValue == CheckState.Checked)
      this._selectedContexts.Add(int64);
    this.UpdateControls();
  }

  private void ContextSelectionForm_KeyUp(object sender, KeyEventArgs e)
  {
    if (e.KeyData == Keys.Escape)
      this.DialogResult = DialogResult.Cancel;
    if (e.KeyData != Keys.Return)
      return;
    this.DialogResult = DialogResult.OK;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ContextSelectionForm));
    this.btnCancel = new Button();
    this.lbPromt = new Label();
    this.cbContexts = new CheckedListBox();
    this.btnOK = new Button();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.Cursor = Cursors.Hand;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Name = "btnCancel";
    componentResourceManager.ApplyResources((object) this.lbPromt, "lbPromt");
    this.lbPromt.Name = "lbPromt";
    componentResourceManager.ApplyResources((object) this.cbContexts, "cbContexts");
    this.cbContexts.CheckOnClick = true;
    this.cbContexts.FormattingEnabled = true;
    this.cbContexts.Name = "cbContexts";
    this.cbContexts.ItemCheck += new ItemCheckEventHandler(this.cbContexts_ItemCheck);
    componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
    this.btnOK.Cursor = Cursors.Hand;
    this.btnOK.DialogResult = DialogResult.OK;
    this.btnOK.Name = "btnOK";
    this.AcceptButton = (IButtonControl) this.btnOK;
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.CancelButton = (IButtonControl) this.btnCancel;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.ControlBox = false;
    this.Controls.Add((Control) this.btnOK);
    this.Controls.Add((Control) this.cbContexts);
    this.Controls.Add((Control) this.lbPromt);
    this.Controls.Add((Control) this.btnCancel);
    this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
    this.KeyPreview = true;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ContextSelectionForm);
    this.ShowIcon = false;
    this.ShowInTaskbar = false;
    this.SizeGripStyle = SizeGripStyle.Show;
    this.Tag = (object) " ";
    this.KeyUp += new KeyEventHandler(this.ContextSelectionForm_KeyUp);
    this.ResumeLayout(false);
  }
}

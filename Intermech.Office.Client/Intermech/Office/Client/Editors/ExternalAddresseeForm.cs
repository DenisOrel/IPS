// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.Editors.ExternalAddresseeForm
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Client.Core;
using Intermech.Client.Core.History;
using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Interfaces.Client;
using Intermech.Navigator;
using Intermech.Navigator.DBObjectTypes;
using Intermech.Navigator.Interfaces;
using Intermech.Office.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client.Editors;

internal class ExternalAddresseeForm : Form
{
  private readonly long _unitID;
  private readonly bool _multiValue;
  private IContainer components;
  private Panel panel1;
  private Button bCancel;
  private Button bOK;
  private Panel panel2;
  private Label label4;
  private TextBox tbRegNum;
  private Label label3;
  private Label label2;
  private TextBox tbDocRecipients;
  private Label label1;
  private Button bAddressee;
  private TextBox tbAddressee;
  private Button bRegNum;
  private Button bDocRecipients;
  private DateTimePicker dtpRegDate;

  internal long AddresseeID => (long?) this.tbAddressee.Tag ?? 0L;

  [NotNull]
  internal string AddresseeCaption => this.tbAddressee.Text;

  [NotNull]
  internal string DocRecipients => this.tbDocRecipients.Text;

  [NotNull]
  internal string RegNum => this.tbRegNum.Text;

  internal DateTime RegDate => this.dtpRegDate.Value;

  public ExternalAddresseeForm([NotNull] string formText, long unitID, bool multiValue)
  {
    this.InitializeComponent();
    this.Text = formText;
    this._unitID = unitID;
    this._multiValue = multiValue;
  }

  internal void Init(
    long addresseeID,
    [NotNull] string addresseeCaption,
    [NotNull] string docRecipients,
    [NotNull] string regNum,
    DateTime regDate)
  {
    this.tbAddressee.Tag = (object) addresseeID;
    this.tbAddressee.Text = addresseeCaption;
    this.tbDocRecipients.Text = docRecipients;
    this.tbRegNum.Text = regNum;
    this.dtpRegDate.Value = regDate;
  }

  private void bAddressee_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    object[] objArray = SelectionWindow.Select(Localization.GetString("Office.Client_11"), (IDescriptor) new Descriptor(OfficeConsts.ObjtypeOrganizationUnitsID), typeof (IDBTypedObjectID), SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect);
    if (objArray == null || objArray.Length == 0)
      return;
    this.tbAddressee.Text = ((IDBObjectID) objArray[0]).Caption;
    this.tbAddressee.Tag = (object) ((IDBTypedObjectID) objArray[0]).ObjectID;
  }

  private void bDocRecipients_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    using (ObjectsHistory objectsHistory = new ObjectsHistory((object) this._unitID, AttributableElements.Object, (object) (this._multiValue ? OfficeConsts.AttrDocRecipientsID : OfficeConsts.AttrDocRecipientID)))
    {
      objectsHistory.SelectedValue = (object) this.tbDocRecipients.Text;
      if (objectsHistory.ShowDialog() != DialogResult.OK)
        return;
      this.tbDocRecipients.Text = objectsHistory.SelectedValue != null ? (string) objectsHistory.SelectedValue : string.Empty;
    }
  }

  private void bRegNum_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    using (ObjectsHistory objectsHistory = new ObjectsHistory((object) this._unitID, AttributableElements.Object, (object) (this._multiValue ? OfficeConsts.AttrInputRegNumsID : OfficeConsts.AttrInputRegNumID)))
    {
      objectsHistory.SelectedValue = (object) this.tbDocRecipients.Text;
      if (objectsHistory.ShowDialog() != DialogResult.OK)
        return;
      this.tbRegNum.Text = objectsHistory.SelectedValue != null ? (string) objectsHistory.SelectedValue : string.Empty;
    }
  }

  private void ExternalAddresseeForm_Load([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void ExternalAddresseeForm_FormClosing([CanBeNull] object sender, [NotNull] FormClosingEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ExternalAddresseeForm));
    this.panel1 = new Panel();
    this.bCancel = new Button();
    this.bOK = new Button();
    this.panel2 = new Panel();
    this.bRegNum = new Button();
    this.bDocRecipients = new Button();
    this.dtpRegDate = new DateTimePicker();
    this.label4 = new Label();
    this.tbRegNum = new TextBox();
    this.label3 = new Label();
    this.label2 = new Label();
    this.tbDocRecipients = new TextBox();
    this.label1 = new Label();
    this.bAddressee = new Button();
    this.tbAddressee = new TextBox();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOK);
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.bOK, "bOK");
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Name = "bOK";
    this.bOK.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Controls.Add((Control) this.bRegNum);
    this.panel2.Controls.Add((Control) this.bDocRecipients);
    this.panel2.Controls.Add((Control) this.dtpRegDate);
    this.panel2.Controls.Add((Control) this.label4);
    this.panel2.Controls.Add((Control) this.tbRegNum);
    this.panel2.Controls.Add((Control) this.label3);
    this.panel2.Controls.Add((Control) this.label2);
    this.panel2.Controls.Add((Control) this.tbDocRecipients);
    this.panel2.Controls.Add((Control) this.label1);
    this.panel2.Controls.Add((Control) this.bAddressee);
    this.panel2.Controls.Add((Control) this.tbAddressee);
    this.panel2.Name = "panel2";
    componentResourceManager.ApplyResources((object) this.bRegNum, "bRegNum");
    this.bRegNum.Name = "bRegNum";
    this.bRegNum.UseVisualStyleBackColor = true;
    this.bRegNum.Click += new EventHandler(this.bRegNum_Click);
    componentResourceManager.ApplyResources((object) this.bDocRecipients, "bDocRecipients");
    this.bDocRecipients.Name = "bDocRecipients";
    this.bDocRecipients.UseVisualStyleBackColor = true;
    this.bDocRecipients.Click += new EventHandler(this.bDocRecipients_Click);
    componentResourceManager.ApplyResources((object) this.dtpRegDate, "dtpRegDate");
    this.dtpRegDate.Name = "dtpRegDate";
    componentResourceManager.ApplyResources((object) this.label4, "label4");
    this.label4.Name = "label4";
    componentResourceManager.ApplyResources((object) this.tbRegNum, "tbRegNum");
    this.tbRegNum.Name = "tbRegNum";
    componentResourceManager.ApplyResources((object) this.label3, "label3");
    this.label3.Name = "label3";
    componentResourceManager.ApplyResources((object) this.label2, "label2");
    this.label2.Name = "label2";
    componentResourceManager.ApplyResources((object) this.tbDocRecipients, "tbDocRecipients");
    this.tbDocRecipients.AccessibleRole = AccessibleRole.None;
    this.tbDocRecipients.Name = "tbDocRecipients";
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.bAddressee, "bAddressee");
    this.bAddressee.Name = "bAddressee";
    this.bAddressee.UseVisualStyleBackColor = true;
    this.bAddressee.Click += new EventHandler(this.bAddressee_Click);
    componentResourceManager.ApplyResources((object) this.tbAddressee, "tbAddressee");
    this.tbAddressee.BackColor = SystemColors.Window;
    this.tbAddressee.Name = "tbAddressee";
    this.tbAddressee.ReadOnly = true;
    this.AcceptButton = (IButtonControl) this.bOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.Name = nameof (ExternalAddresseeForm);
    this.FormClosing += new FormClosingEventHandler(this.ExternalAddresseeForm_FormClosing);
    this.Load += new EventHandler(this.ExternalAddresseeForm_Load);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.panel2.PerformLayout();
    this.ResumeLayout(false);
  }
}

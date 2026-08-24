// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ManualRegNumberForm
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Navigator.Interfaces;
using Intermech.Office.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class ManualRegNumberForm : Form
{
  private bool _emptyNumberPossible;
  [NotNull]
  private readonly Dictionary<int, string> _historyList;
  private int _documentType;
  private long _documentID;
  [CanBeNull]
  private string _defaultLabel;
  private IContainer components;
  private Label label1;
  private Button bOK;
  private Button bCancel;
  private Button bClassify;
  private ToolTip toolTip1;
  private ComboBox cbRegNumber;

  public ManualRegNumberForm(bool privateMode)
  {
    this.InitializeComponent();
    this._historyList = new Dictionary<int, string>();
    bool flag = FormStorage.LoadLayout((Control) this, (IDictionary) this._historyList);
    for (int key = 0; key < this._historyList.Count; ++key)
      this.cbRegNumber.Items.Insert(0, (object) this._historyList[key]);
    if (!flag)
      this.StartPosition = FormStartPosition.CenterParent;
    this.Text = privateMode ? "Внутренний регистрационный номер документа" : "Регистрационный номер документа";
  }

  public void Initialize(
    long documentID,
    int documentType,
    [CanBeNull] string regNumber,
    bool readOnly,
    bool privateNumber)
  {
    this._documentID = documentID;
    this._documentType = documentType;
    this.cbRegNumber.Text = regNumber;
    if (this._defaultLabel == null)
      this._defaultLabel = this.label1.Text;
    if (!privateNumber)
    {
      this.label1.Text = this._defaultLabel;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        this._emptyNumberPossible = (sessionKeeper.Session.GetObjectType(documentType, true).Attributes.GetAttributeByID(OfficeConsts.AttrRegNumberID, true).Options & AttributeOptions.DisableNulls) == AttributeOptions.None;
    }
    else
    {
      this.label1.Text = "Регистрационный номер внутренней канцелярии:";
      this._emptyNumberPossible = false;
    }
    if (this._emptyNumberPossible)
      this.bOK.Enabled = true;
    if (!readOnly)
      return;
    this.cbRegNumber.Enabled = this.bClassify.Enabled = this.bOK.Enabled = false;
  }

  [NotNull]
  public string Template => this.cbRegNumber.Text;

  private void PrivateRegNumberForm_FormClosing([CanBeNull] object sender, [NotNull] FormClosingEventArgs e)
  {
    if (this.DialogResult == DialogResult.OK && !this._historyList.ContainsValue(this.cbRegNumber.Text))
      this._historyList.Add(this._historyList.Count, this.cbRegNumber.Text);
    FormStorage.SaveLayout((Control) this, (IDictionary) this._historyList);
  }

  private void bClassify_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long[] classifierForObjType = sessionKeeper.Session.GetCustomService<ISelectionsService>().GetClassifierForObjType((object) sessionKeeper.Session.SessionGUID, this._documentType);
      long classifierID = 0;
      string text = Localization.GetString("Office.Client_62");
      using (ClassifySelectionForm classifySelectionForm = new ClassifySelectionForm(classifierForObjType, text))
      {
        if (!classifySelectionForm.ShowDialog().Equals((object) DialogResult.OK))
          return;
        IDBObjectID itemData = classifySelectionForm.SelectedItems.GetItemData<IDBObjectID>(0, false);
        if (itemData != null)
          classifierID = itemData.Value;
      }
      if (classifierID == 0L)
        return;
      IObjectClassificator objectClassificator = sessionKeeper.Session.GetCustomService<ISelectionsService>().GetObjectClassificator((object) sessionKeeper.Session.SessionGUID, classifierID);
      if (objectClassificator == null)
        return;
      AttributeValues[] clasificatorAttributes = objectClassificator.GetClasificatorAttributes(this._documentID);
      if (clasificatorAttributes == null || clasificatorAttributes.Length == 0 || clasificatorAttributes[0].Values == null || clasificatorAttributes[0].Values.Length == 0)
        return;
      this.cbRegNumber.Text = Convert.ToString(clasificatorAttributes[0].Values[0]);
    }
  }

  private void tbTemplateString_TextChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.bOK.Enabled = this._emptyNumberPossible || this.cbRegNumber.Text != string.Empty;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ManualRegNumberForm));
    this.label1 = new Label();
    this.bOK = new Button();
    this.bCancel = new Button();
    this.bClassify = new Button();
    this.toolTip1 = new ToolTip();
    this.cbRegNumber = new ComboBox();
    this.SuspendLayout();
    this.label1.AutoSize = true;
    this.label1.ForeColor = SystemColors.ControlText;
    this.label1.ImeMode = ImeMode.NoControl;
    this.label1.Location = new Point(24, 22);
    this.label1.Name = "label1";
    this.label1.Size = new Size(136, 13);
    this.label1.TabIndex = 19;
    this.label1.Text = "Регистрационный номер:";
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Enabled = false;
    this.bOK.Location = new Point(181, 76);
    this.bOK.Name = "bOK";
    this.bOK.Size = new Size(121, 27);
    this.bOK.TabIndex = 2;
    this.bOK.Text = "ОК";
    this.bOK.UseVisualStyleBackColor = true;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(308, 76);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(121, 27);
    this.bCancel.TabIndex = 3;
    this.bCancel.Text = "Отмена";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bClassify.FlatStyle = FlatStyle.Popup;
    this.bClassify.Image = (Image) componentResourceManager.GetObject("bClassify.Image");
    this.bClassify.Location = new Point(27, 77);
    this.bClassify.Name = "bClassify";
    this.bClassify.Size = new Size(24, 24);
    this.bClassify.TabIndex = 1;
    this.toolTip1.SetToolTip((Control) this.bClassify, "Классификатор");
    this.bClassify.UseVisualStyleBackColor = true;
    this.bClassify.Click += new EventHandler(this.bClassify_Click);
    this.toolTip1.BackColor = SystemColors.Window;
    this.toolTip1.ForeColor = SystemColors.WindowText;
    this.cbRegNumber.FormattingEnabled = true;
    this.cbRegNumber.Location = new Point(27, 38);
    this.cbRegNumber.Name = "cbRegNumber";
    this.cbRegNumber.Size = new Size(402, 21);
    this.cbRegNumber.TabIndex = 0;
    this.cbRegNumber.SelectedIndexChanged += new EventHandler(this.tbTemplateString_TextChanged);
    this.cbRegNumber.TextChanged += new EventHandler(this.tbTemplateString_TextChanged);
    this.AcceptButton = (IButtonControl) this.bOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.ClientSize = new Size(452, 124);
    this.Controls.Add((Control) this.cbRegNumber);
    this.Controls.Add((Control) this.bClassify);
    this.Controls.Add((Control) this.bCancel);
    this.Controls.Add((Control) this.bOK);
    this.Controls.Add((Control) this.label1);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.Name = nameof (ManualRegNumberForm);
    this.StartPosition = FormStartPosition.Manual;
    this.FormClosing += new FormClosingEventHandler(this.PrivateRegNumberForm_FormClosing);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}

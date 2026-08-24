// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.SMDOEmailForm
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class SMDOEmailForm : Form
{
  private SMDOEmailDataSettings _smdoEmailDataSettings = new SMDOEmailDataSettings();
  private IContainer components;
  private Label label1;
  private GroupBox groupBox1;
  private RadioButton rbConfident;
  private RadioButton rbGeneral;
  private Button btnOK;
  private Button btnCancel;
  private CheckedComboBox ccbReceiver;
  private ComboBox cbCert;
  private Label lblCert;

  public SMDOEmailForm(DataRowCollection organizations)
  {
    this.InitializeComponent();
    this._smdoEmailDataSettings.ConfValue = 0;
    this._smdoEmailDataSettings.ConfName = this.rbGeneral.Text;
    this._smdoEmailDataSettings.Organizations = new Dictionary<string, string>();
    foreach (X509Certificate2 allCertificate in this.GetAllCertificates(X509FindType.FindByIssuerName))
    {
      Dictionary<string, string> dictionary = this.X509Parse(allCertificate.Subject);
      string empty = string.Empty;
      this.cbCert.Items.Add((object) new ComboBoxCertItem(!dictionary.ContainsKey("CN") ? (!dictionary.ContainsKey("E") ? dictionary["OE"] : dictionary["E"]) : dictionary["CN"], allCertificate));
    }
    for (int index = 0; index < organizations.Count; ++index)
    {
      this.ccbReceiver.Items.Add((object) new CCBoxItem(organizations[index].ItemArray[1].ToString(), index, organizations[index].ItemArray[2].ToString()));
      this.ccbReceiver.MaxDropDownItems = 5;
      this.ccbReceiver.DisplayMember = "Name";
      this.ccbReceiver.ValueSeparator = ", ";
    }
  }

  public Dictionary<string, string> X509Parse(string X509Value)
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    string str1 = X509Value;
    string[] separator = new string[1]{ ", " };
    foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
    {
      int length = str2.IndexOf('=');
      if (length != -1)
      {
        string key = str2.Substring(0, length);
        string str3 = str2.Remove(0, length + 1).TrimStart('"').TrimEnd('"');
        if (!dictionary.ContainsKey(key))
          dictionary[key] = str3;
      }
    }
    return dictionary;
  }

  public SMDOEmailDataSettings SmdoEmailDataSettings => this._smdoEmailDataSettings;

  private void confidentially_CheckedChanged(object sender, EventArgs e)
  {
    if (!(sender is RadioButton radioButton))
      throw new KernelException("Данные о конфидециальности  получить не удалось");
    switch (radioButton.Text)
    {
      case "Общий":
        this._smdoEmailDataSettings.ConfValue = 0;
        this._smdoEmailDataSettings.ConfName = radioButton.Text;
        break;
      case "Ограниченный":
        this._smdoEmailDataSettings.ConfValue = 1;
        this._smdoEmailDataSettings.ConfName = radioButton.Text;
        break;
    }
  }

  private void btnOK_Click(object sender, EventArgs e)
  {
    foreach (CCBoxItem checkedItem in this.ccbReceiver.CheckedItems)
    {
      if (!this._smdoEmailDataSettings.Organizations.ContainsKey(checkedItem.SMDOID))
        this._smdoEmailDataSettings.Organizations.Add(checkedItem.SMDOID, checkedItem.Name);
    }
    if (this._smdoEmailDataSettings.Organizations.Count == 0)
    {
      int num1 = (int) MessageBox.Show("Выберите хотя бы одного получателя", "Внимание");
    }
    else if (this.cbCert.SelectedIndex > -1)
    {
      this._smdoEmailDataSettings.Certificate = (this.cbCert.SelectedItem as ComboBoxCertItem).Value;
      this._smdoEmailDataSettings.IsHavePrivateKey = (this.cbCert.SelectedItem as ComboBoxCertItem).Value.HasPrivateKey;
      foreach (X509Extension x509Extension in (this.cbCert.SelectedItem as ComboBoxCertItem).Value.Extensions)
      {
        if (x509Extension is X509SubjectKeyIdentifierExtension identifierExtension)
          this._smdoEmailDataSettings.OpenKeyID = identifierExtension.SubjectKeyIdentifier;
      }
      this._smdoEmailDataSettings.IsHaveSigns = true;
      this.Close();
      this.DialogResult = DialogResult.OK;
    }
    else
    {
      int num2 = (int) MessageBox.Show("Выберите сертификат для подписи", "Внимание");
    }
  }

  public X509Certificate2Collection GetAllCertificates(X509FindType findType)
  {
    X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
    x509Store.Open(OpenFlags.OpenExistingOnly);
    return x509Store.Certificates.Find(findType, (object) string.Empty, false);
  }

  private void ccbReceiver_ItemCheck(object sender, ItemCheckEventArgs e)
  {
    if (e.NewValue == CheckState.Checked && this.ccbReceiver.CheckedItems.Count > 100)
      throw new Exception("Максимальное число получателей не должно превышать 100.");
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.label1 = new Label();
    this.groupBox1 = new GroupBox();
    this.rbConfident = new RadioButton();
    this.rbGeneral = new RadioButton();
    this.btnOK = new Button();
    this.btnCancel = new Button();
    this.ccbReceiver = new CheckedComboBox();
    this.cbCert = new ComboBox();
    this.lblCert = new Label();
    this.groupBox1.SuspendLayout();
    this.SuspendLayout();
    this.label1.AutoSize = true;
    this.label1.Location = new Point(13, 9);
    this.label1.Name = "label1";
    this.label1.Size = new Size(66, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "Получатель";
    this.groupBox1.Controls.Add((Control) this.rbConfident);
    this.groupBox1.Controls.Add((Control) this.rbGeneral);
    this.groupBox1.Location = new Point(16 /*0x10*/, 104);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(336, 53);
    this.groupBox1.TabIndex = 5;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Тип ограничений файла";
    this.rbConfident.AutoSize = true;
    this.rbConfident.Location = new Point(103, 22);
    this.rbConfident.Name = "rbConfident";
    this.rbConfident.Size = new Size(99, 17);
    this.rbConfident.TabIndex = 1;
    this.rbConfident.Text = "Ограниченный";
    this.rbConfident.UseVisualStyleBackColor = true;
    this.rbConfident.CheckedChanged += new EventHandler(this.confidentially_CheckedChanged);
    this.rbGeneral.AutoSize = true;
    this.rbGeneral.Checked = true;
    this.rbGeneral.Location = new Point(11, 22);
    this.rbGeneral.Name = "rbGeneral";
    this.rbGeneral.Size = new Size(60, 17);
    this.rbGeneral.TabIndex = 0;
    this.rbGeneral.TabStop = true;
    this.rbGeneral.Text = "Общий";
    this.rbGeneral.UseVisualStyleBackColor = true;
    this.rbGeneral.CheckedChanged += new EventHandler(this.confidentially_CheckedChanged);
    this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnOK.Location = new Point(196, 169);
    this.btnOK.Name = "btnOK";
    this.btnOK.Size = new Size(75, 25);
    this.btnOK.TabIndex = 6;
    this.btnOK.Text = "Отправить";
    this.btnOK.UseVisualStyleBackColor = true;
    this.btnOK.Click += new EventHandler(this.btnOK_Click);
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(277, 169);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(75, 25);
    this.btnCancel.TabIndex = 7;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.ccbReceiver.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.ccbReceiver.CheckOnClick = true;
    this.ccbReceiver.DrawMode = DrawMode.OwnerDrawVariable;
    this.ccbReceiver.DropDownHeight = 1;
    this.ccbReceiver.FormattingEnabled = true;
    this.ccbReceiver.IntegralHeight = false;
    this.ccbReceiver.Location = new Point(16 /*0x10*/, 27);
    this.ccbReceiver.Name = "ccbReceiver";
    this.ccbReceiver.Size = new Size(336, 21);
    this.ccbReceiver.TabIndex = 8;
    this.ccbReceiver.ValueSeparator = ", ";
    this.ccbReceiver.ItemCheck += new ItemCheckEventHandler(this.ccbReceiver_ItemCheck);
    this.cbCert.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.cbCert.FormattingEnabled = true;
    this.cbCert.Location = new Point(16 /*0x10*/, 71);
    this.cbCert.Name = "cbCert";
    this.cbCert.Size = new Size(336, 21);
    this.cbCert.TabIndex = 12;
    this.lblCert.AutoSize = true;
    this.lblCert.Location = new Point(13, 53);
    this.lblCert.Name = "lblCert";
    this.lblCert.Size = new Size(68, 13);
    this.lblCert.TabIndex = 13;
    this.lblCert.Text = "Сертификат";
    this.AcceptButton = (IButtonControl) this.btnOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(364, 203);
    this.Controls.Add((Control) this.lblCert);
    this.Controls.Add((Control) this.cbCert);
    this.Controls.Add((Control) this.ccbReceiver);
    this.Controls.Add((Control) this.btnCancel);
    this.Controls.Add((Control) this.btnOK);
    this.Controls.Add((Control) this.groupBox1);
    this.Controls.Add((Control) this.label1);
    this.MinimumSize = new Size(380, 225);
    this.Name = nameof (SMDOEmailForm);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Отправка в СМДО";
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}

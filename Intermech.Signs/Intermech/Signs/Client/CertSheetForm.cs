// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetForm
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Client.Core;
using Intermech.Controls;
using Intermech.Interfaces;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

public class CertSheetForm : Form
{
  private List<long> objIdList;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private Button btnOK;
  private Button btnCancel;
  private CertSheetControl certSheetControl;
  private Panel panel1;
  private Panel panel2;

  public CertSheetControl CertSheetControl => this.certSheetControl;

  public CertSheetForm() => this.InitializeComponent();

  public DialogResult ShowDialog(List<long> objIdList)
  {
    this.objIdList = objIdList;
    this.certSheetControl.InitControl(this.objIdList);
    return this.ShowDialog();
  }

  private void CertSheetForm_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void btnOK_Click(object sender, EventArgs e)
  {
    string empty = string.Empty;
    bool flag = false;
    CertSheetOptions certSheetOptions = this.CertSheetControl.GetCertSheetOptions();
    if (certSheetOptions.Graphs.Count == 0 && certSheetOptions.EmptyGraphs.Count == 0)
    {
      empty = LocalizationHolder.rm.GetString("CertSheetGraphsNotSelected");
      flag = true;
    }
    if (certSheetOptions.Extensions.Count == 0)
    {
      if (empty != string.Empty)
        empty += "\n";
      empty += LocalizationHolder.rm.GetString("CertSheetExtensionsNotSelected");
      flag = true;
    }
    if (!flag)
      return;
    IMMessageBox.ShowEx(MessageDialogs.msgWarning, empty, new IMMessageBoxButton[1]
    {
      new IMMessageBoxButton("OK", DialogResult.OK, (object) DialogResult.OK)
    }, IMMessageBoxImage.Warning);
    this.DialogResult = DialogResult.None;
  }

  private void CertSheetForm_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.btnOK = new Button();
    this.btnCancel = new Button();
    this.panel1 = new Panel();
    this.panel2 = new Panel();
    this.certSheetControl = new CertSheetControl();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.btnOK.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.btnOK.DialogResult = DialogResult.OK;
    this.btnOK.Location = new Point(273, 6);
    this.btnOK.Name = "btnOK";
    this.btnOK.Size = new Size(75, 23);
    this.btnOK.TabIndex = 0;
    this.btnOK.Text = "OK";
    this.btnOK.UseVisualStyleBackColor = true;
    this.btnOK.Click += new EventHandler(this.btnOK_Click);
    this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(354, 6);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(75, 23);
    this.btnCancel.TabIndex = 1;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.panel1.Controls.Add((Control) this.btnCancel);
    this.panel1.Controls.Add((Control) this.btnOK);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 370);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(451, 42);
    this.panel1.TabIndex = 3;
    this.panel2.Controls.Add((Control) this.certSheetControl);
    this.panel2.Dock = DockStyle.Fill;
    this.panel2.Location = new Point(0, 0);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(451, 370);
    this.panel2.TabIndex = 4;
    this.certSheetControl.Dock = DockStyle.Fill;
    this.certSheetControl.Location = new Point(0, 0);
    this.certSheetControl.Name = "certSheetControl";
    this.certSheetControl.SaveToDiskInterfaceFlag = false;
    this.certSheetControl.Size = new Size(451, 370);
    this.certSheetControl.TabIndex = 2;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(451, 412);
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.MinimumSize = new Size(460, 360);
    this.Name = nameof (CertSheetForm);
    this.Text = "Информационно-удостоверяющие листы";
    this.FormClosed += new FormClosedEventHandler(this.CertSheetForm_FormClosed);
    this.Load += new EventHandler(this.CertSheetForm_Load);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}

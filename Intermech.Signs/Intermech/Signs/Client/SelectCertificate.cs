// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SelectCertificate
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Client.Core;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

public class SelectCertificate : Form
{
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private ListView LV;
  private Button bCancel;
  private Button bOk;
  private ColumnHeader columnHeader1;
  private ColumnHeader columnHeader2;
  private ColumnHeader columnHeader3;
  private ColumnHeader columnHeader4;
  private ColumnHeader columnHeader5;
  private Button bView;

  public SelectCertificate()
  {
    this.InitializeComponent();
    this.bOk.Enabled = this.bView.Enabled = false;
    this.LoadCertificates();
  }

  private void SelectCertificate_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void SelectCertificate_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  public X509Certificate2 Certificate => this.LV.SelectedItems[0].Tag as X509Certificate2;

  private void LoadCertificates()
  {
    X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
    x509Store.Open(OpenFlags.ReadOnly);
    try
    {
      X509Certificate2Enumerator enumerator = x509Store.Certificates.GetEnumerator();
      while (enumerator.MoveNext())
      {
        X509Certificate2 current = enumerator.Current;
        Dictionary<string, string> dictionary1 = this.X509Parse(current.Subject);
        string empty1 = string.Empty;
        string text1 = !dictionary1.ContainsKey("CN") ? (!dictionary1.ContainsKey("E") ? dictionary1["OE"] : dictionary1["E"]) : dictionary1["CN"];
        Dictionary<string, string> dictionary2 = this.X509Parse(current.Issuer);
        string empty2 = string.Empty;
        string text2 = !dictionary2.ContainsKey("CN") ? dictionary2["OU"] : dictionary2["CN"];
        ListViewItem listViewItem = this.LV.Items.Add(text1);
        listViewItem.SubItems.Add(text2);
        listViewItem.SubItems.Add(current.NotBefore.ToString());
        listViewItem.SubItems.Add(current.NotAfter.ToString());
        listViewItem.SubItems.Add(current.FriendlyName);
        listViewItem.Tag = (object) current;
      }
    }
    finally
    {
      x509Store.Close();
    }
    this.LV.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
  }

  private Dictionary<string, string> X509Parse(string X509Value)
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    string str1 = X509Value;
    string[] separator = new string[1]{ ", " };
    foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
    {
      int length = str2.IndexOf('=');
      string key = str2.Substring(0, length);
      string str3 = str2.Remove(0, length + 1).TrimStart('"').TrimEnd('"');
      dictionary[key] = str3;
    }
    return dictionary;
  }

  private void LV_SelectedIndexChanged(object sender, EventArgs e)
  {
    bool flag1 = this.LV.SelectedIndices.Count.Equals(1) && this.LV.SelectedIndices[0] >= 0;
    bool flag2 = false;
    if (flag1)
    {
      X509Certificate2 tag = this.LV.SelectedItems[0].Tag as X509Certificate2;
      flag2 = DateTime.Now >= tag.NotBefore && DateTime.Now <= tag.NotAfter;
    }
    this.bView.Enabled = flag1;
    this.bOk.Enabled = flag1 & flag2;
  }

  private void bView_Click(object sender, EventArgs e)
  {
    X509Certificate2UI.DisplayCertificate(this.LV.SelectedItems[0].Tag as X509Certificate2, this.Handle);
  }

  public static X509Certificate2 SelectCertificateDlg()
  {
    X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
    x509Store.Open(OpenFlags.ReadOnly);
    try
    {
      X509Certificate2Collection certificate2Collection = X509Certificate2UI.SelectFromCollection(x509Store.Certificates, LocalizationHolder.rm.GetString("Signs_86"), LocalizationHolder.rm.GetString("Signs_87"), X509SelectionFlag.SingleSelection);
      if (certificate2Collection.Count.Equals(1))
        return certificate2Collection[0];
    }
    finally
    {
      x509Store.Close();
    }
    return (X509Certificate2) null;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectCertificate));
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.LV = new ListView();
    this.columnHeader1 = new ColumnHeader();
    this.columnHeader2 = new ColumnHeader();
    this.columnHeader5 = new ColumnHeader();
    this.columnHeader3 = new ColumnHeader();
    this.columnHeader4 = new ColumnHeader();
    this.bView = new Button();
    this.bCancel = new Button();
    this.bOk = new Button();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
    this.tableLayoutPanel1.Controls.Add((Control) this.LV, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.bView, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.bCancel, 3, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.bOk, 2, 1);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.LV.Columns.AddRange(new ColumnHeader[5]
    {
      this.columnHeader1,
      this.columnHeader2,
      this.columnHeader5,
      this.columnHeader3,
      this.columnHeader4
    });
    this.tableLayoutPanel1.SetColumnSpan((Control) this.LV, 4);
    componentResourceManager.ApplyResources((object) this.LV, "LV");
    this.LV.FullRowSelect = true;
    this.LV.GridLines = true;
    this.LV.Name = "LV";
    this.LV.UseCompatibleStateImageBehavior = false;
    this.LV.View = View.Details;
    this.LV.SelectedIndexChanged += new EventHandler(this.LV_SelectedIndexChanged);
    componentResourceManager.ApplyResources((object) this.columnHeader1, "columnHeader1");
    componentResourceManager.ApplyResources((object) this.columnHeader2, "columnHeader2");
    componentResourceManager.ApplyResources((object) this.columnHeader5, "columnHeader5");
    componentResourceManager.ApplyResources((object) this.columnHeader3, "columnHeader3");
    componentResourceManager.ApplyResources((object) this.columnHeader4, "columnHeader4");
    componentResourceManager.ApplyResources((object) this.bView, "bView");
    this.bView.Name = "bView";
    this.bView.UseVisualStyleBackColor = true;
    this.bView.Click += new EventHandler(this.bView_Click);
    this.bCancel.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bOk.DialogResult = DialogResult.OK;
    componentResourceManager.ApplyResources((object) this.bOk, "bOk");
    this.bOk.Name = "bOk";
    this.bOk.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (SelectCertificate);
    this.ShowInTaskbar = false;
    this.Load += new EventHandler(this.SelectCertificate_Load);
    this.FormClosed += new FormClosedEventHandler(this.SelectCertificate_FormClosed);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}

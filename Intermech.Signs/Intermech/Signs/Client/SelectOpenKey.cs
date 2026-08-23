// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SelectOpenKey
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Localization;
using Intermech.Signs.Interfaces;
using System;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

public class SelectOpenKey : Form
{
  private System.ComponentModel.Container components;
  private Button button1;
  private Button button2;
  private PropertyGrid _property;
  private TableLayoutPanel tableLayoutPanel1;
  private Button bCertificate;
  private object _value;
  private SelectOpenKeyValueType _valueType;

  public SelectOpenKey(OpenKeysCollection collection)
  {
    this.InitializeComponent();
    if (collection.Count > 0)
      this.button1.Enabled = true;
    else
      this.button1.Enabled = false;
    this._property.SelectedObject = (object) new OpenKeyClassWrapper(collection.Values);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectOpenKey));
    this.button2 = new Button();
    this.button1 = new Button();
    this._property = new PropertyGrid();
    this.bCertificate = new Button();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.button2.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this.button2, "button2");
    this.button2.Name = "button2";
    this.button1.DialogResult = DialogResult.OK;
    componentResourceManager.ApplyResources((object) this.button1, "button1");
    this.button1.Name = "button1";
    this.button1.Click += new EventHandler(this.button1_Click);
    this.tableLayoutPanel1.SetColumnSpan((Control) this._property, 4);
    componentResourceManager.ApplyResources((object) this._property, "_property");
    this._property.Name = "_property";
    this._property.SelectedGridItemChanged += new SelectedGridItemChangedEventHandler(this._property_SelectedGridItemChanged);
    componentResourceManager.ApplyResources((object) this.bCertificate, "bCertificate");
    this.bCertificate.Name = "bCertificate";
    this.bCertificate.UseVisualStyleBackColor = true;
    this.bCertificate.Click += new EventHandler(this.bCertificate_Click);
    componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
    this.tableLayoutPanel1.Controls.Add((Control) this.bCertificate, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this._property, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.button1, 2, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.button2, 3, 1);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (SelectOpenKey);
    this.ShowInTaskbar = false;
    this.Closed += new EventHandler(this.SelectOpenKey_Closed);
    this.Load += new EventHandler(this.SelectOpenKey_Load);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private void SelectOpenKey_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void SelectOpenKey_Closed(object sender, EventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void _property_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
  {
    this._value = (object) (e.NewSelection.PropertyDescriptor as OpenKeyPropertyDescriptor).Parent;
    this._valueType = SelectOpenKeyValueType.OpenKey;
  }

  private void bCertificate_Click(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      X509Certificate2Collection possibleCertificates = CertProcs.GetPossibleCertificates(sessionKeeper.Session);
      if (possibleCertificates == null || possibleCertificates.Count <= 0)
        return;
      X509Certificate2 x509Certificate2 = CertProcs.SelectCertificate(possibleCertificates, LocalizationHolder.rm.GetString("Signs_86"), LocalizationHolder.rm.GetString("Signs_87"));
      if (x509Certificate2 == null)
        return;
      this._valueType = SelectOpenKeyValueType.Certificate;
      this._value = (object) x509Certificate2;
      this.DialogResult = DialogResult.OK;
    }
  }

  public SelectOpenKeyValueType ValueType => this._valueType;

  public object Value => this._value;

  private void button1_Click(object sender, EventArgs e)
  {
  }
}

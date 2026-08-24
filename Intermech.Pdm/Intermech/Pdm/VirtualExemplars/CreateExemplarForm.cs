// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VirtualExemplars.CreateExemplarForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using Intermech.Localization;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.VirtualExemplars;

internal class CreateExemplarForm : Form
{
  private IContainer components;
  private Panel panel1;
  private Label lCount;
  private ProgressBar pbProcess;
  private Label lMain;
  private Panel panel2;
  private Panel panel3;
  private Button bOK;
  private Button bCancel;
  private ListBox listBox1;

  public event CancelCreateHandler CancelCreateEvent;

  public CreateExemplarForm()
  {
    this.InitializeComponent();
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1487);
  }

  public CreateExemplarForm(string Caption, string Text)
    : this()
  {
    this.Text = Caption;
    this.lMain.Text = Text;
  }

  public CreateExemplarForm(string Caption, string Text, int CountSteps)
    : this(Caption, Text)
  {
    this.Height = 85;
    this.pbProcess.Maximum = CountSteps;
    this.pbProcess.Minimum = 0;
    this.pbProcess.Step = 1;
    this.pbProcess.Value = 0;
    this.lCount.Text = string.Format(LocalizationHolder.rm.GetString("Pdm_402"), (object) this.pbProcess.Value, (object) this.pbProcess.Maximum);
  }

  public void OkEnable() => this.bOK.Invoke((Delegate) new MethodInvoker(this.OnOkEnable));

  public void AddCount() => this.pbProcess.Invoke((Delegate) new MethodInvoker(this.OnAddCount));

  public void AddString(string LineText)
  {
    this.listBox1.Invoke((Delegate) new AddStringHandler(this.OnAddString), (object) LineText);
  }

  private void OnOkEnable() => this.bOK.Enabled = true;

  private void OnAddCount()
  {
    ++this.pbProcess.Value;
    this.lCount.Text = string.Format(LocalizationHolder.rm.GetString("Pdm_403"), (object) this.pbProcess.Value, (object) this.pbProcess.Maximum);
  }

  private void OnAddString(string LineText) => this.listBox1.Items.Add((object) LineText);

  private void bCancel_Click(object sender, EventArgs e)
  {
    if (this.CancelCreateEvent == null)
      return;
    this.CancelCreateEvent();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CreateExemplarForm));
    this.panel1 = new Panel();
    this.lCount = new Label();
    this.pbProcess = new ProgressBar();
    this.lMain = new Label();
    this.panel2 = new Panel();
    this.bOK = new Button();
    this.bCancel = new Button();
    this.panel3 = new Panel();
    this.listBox1 = new ListBox();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.panel3.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.lCount);
    this.panel1.Controls.Add((Control) this.pbProcess);
    this.panel1.Controls.Add((Control) this.lMain);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.lCount, "lCount");
    this.lCount.Name = "lCount";
    componentResourceManager.ApplyResources((object) this.pbProcess, "pbProcess");
    this.pbProcess.Name = "pbProcess";
    componentResourceManager.ApplyResources((object) this.lMain, "lMain");
    this.lMain.Name = "lMain";
    this.panel2.Controls.Add((Control) this.bOK);
    this.panel2.Controls.Add((Control) this.bCancel);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    componentResourceManager.ApplyResources((object) this.bOK, "bOK");
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Name = "bOK";
    this.bOK.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bCancel.Click += new EventHandler(this.bCancel_Click);
    this.panel3.Controls.Add((Control) this.listBox1);
    componentResourceManager.ApplyResources((object) this.panel3, "panel3");
    this.panel3.Name = "panel3";
    componentResourceManager.ApplyResources((object) this.listBox1, "listBox1");
    this.listBox1.FormattingEnabled = true;
    this.listBox1.Name = "listBox1";
    this.AcceptButton = (IButtonControl) this.bOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.panel3);
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (CreateExemplarForm);
    this.Tag = (object) "";
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    this.panel2.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}

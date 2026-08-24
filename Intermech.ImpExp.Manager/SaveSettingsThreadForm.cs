// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.SaveSettingsThreadForm
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager;

internal class SaveSettingsThreadForm : Form
{
  private IContainer components;
  private Label label1;
  private Label lStepName;

  public SaveSettingsThreadForm() => this.InitializeComponent();

  public void SetStepName(string stepName)
  {
    this.Invoke((Delegate) new SaveSettingsThreadForm.SetStepNameDelegate(this._setStepName), (object) stepName);
  }

  private void _setStepName(string stepName) => this.lStepName.Text = stepName;

  private void _closeForm() => this.Close();

  internal void CloseForm() => this.Invoke((Delegate) new MethodInvoker(this._closeForm));

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.label1 = new Label();
    this.lStepName = new Label();
    this.SuspendLayout();
    this.label1.AutoSize = true;
    this.label1.Location = new Point(12, 9);
    this.label1.Name = "label1";
    this.label1.Size = new Size(148, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "Сохранение настроек шага:";
    this.lStepName.AutoSize = true;
    this.lStepName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
    this.lStepName.Location = new Point(12, 31 /*0x1F*/);
    this.lStepName.Name = "lStepName";
    this.lStepName.Size = new Size(41, 13);
    this.lStepName.TabIndex = 1;
    this.lStepName.Text = "label2";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(416, 73);
    this.Controls.Add((Control) this.lStepName);
    this.Controls.Add((Control) this.label1);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.Name = nameof (SaveSettingsThreadForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Сохранение настроек";
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private delegate void SetStepNameDelegate(string stepName);
}

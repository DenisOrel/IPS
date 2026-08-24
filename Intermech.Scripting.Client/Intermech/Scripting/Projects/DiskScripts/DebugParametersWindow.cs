// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Projects.DiskScripts.DebugParametersWindow
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Mvp;
using Intermech.Mvp.Components;
using Intermech.Mvp.Winforms;
using Intermech.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Scripting.Projects.DiskScripts;

internal class DebugParametersWindow : 
  MvpWindow,
  IDebugParametersView,
  IView,
  IOperationConfirmationView
{
  private IContainer components;
  private Button btOK;
  private Button btCancel;
  private TextBox tbRunArguments;
  private Label lbRunArguments;

  public DebugParametersWindow() => this.InitializeComponent();

  ICollection<string> IDebugParametersView.ScriptArguments
  {
    get
    {
      return (ICollection<string>) new List<string>((IEnumerable<string>) this.tbRunArguments.Text.Split(TextServices.TextLinesSplitPatterns, StringSplitOptions.None));
    }
    set
    {
      if (value == null)
        this.tbRunArguments.Text = string.Empty;
      else
        this.tbRunArguments.Text = string.Join(Environment.NewLine, (IEnumerable<string>) value);
    }
  }

  public event EventHandler OperationConfirmed;

  private void btOK_Click(object sender, EventArgs e)
  {
    if (this.OperationConfirmed == null)
      return;
    this.OperationConfirmed((object) this, EventArgs.Empty);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.btOK = new Button();
    this.btCancel = new Button();
    this.tbRunArguments = new TextBox();
    this.lbRunArguments = new Label();
    this.SuspendLayout();
    this.btOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btOK.DialogResult = DialogResult.OK;
    this.btOK.Location = new Point(396, 227);
    this.btOK.Name = "btOK";
    this.btOK.Size = new Size(75, 23);
    this.btOK.TabIndex = 2;
    this.btOK.Text = "OK";
    this.btOK.UseVisualStyleBackColor = true;
    this.btOK.Click += new EventHandler(this.btOK_Click);
    this.btCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btCancel.DialogResult = DialogResult.Cancel;
    this.btCancel.Location = new Point(477, 227);
    this.btCancel.Name = "btCancel";
    this.btCancel.Size = new Size(75, 23);
    this.btCancel.TabIndex = 3;
    this.btCancel.Text = "Отмена";
    this.btCancel.UseVisualStyleBackColor = true;
    this.tbRunArguments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.tbRunArguments.Location = new Point(12, 36);
    this.tbRunArguments.Multiline = true;
    this.tbRunArguments.Name = "tbRunArguments";
    this.tbRunArguments.Size = new Size(540, 138);
    this.tbRunArguments.TabIndex = 1;
    this.lbRunArguments.AutoSize = true;
    this.lbRunArguments.Location = new Point(9, 20);
    this.lbRunArguments.Name = "lbRunArguments";
    this.lbRunArguments.Size = new Size(317, 13);
    this.lbRunArguments.TabIndex = 0;
    this.lbRunArguments.Text = "Аргументы вызова сценария (один аргумент на одной строке)";
    this.AcceptButton = (IButtonControl) this.btOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btCancel;
    this.ClientSize = new Size(564, 262);
    this.Controls.Add((Control) this.lbRunArguments);
    this.Controls.Add((Control) this.tbRunArguments);
    this.Controls.Add((Control) this.btCancel);
    this.Controls.Add((Control) this.btOK);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.MinimumSize = new Size(580, 300);
    this.Name = nameof (DebugParametersWindow);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Параметры запуска сценария";
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}

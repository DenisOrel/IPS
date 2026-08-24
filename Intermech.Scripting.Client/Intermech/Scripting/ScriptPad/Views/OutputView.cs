// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.OutputView
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views;

internal class OutputView : DockContent, IScriptOutputStream
{
  private static readonly string newLine = Environment.NewLine;
  private object writeLineSyncRoot;
  private List<string> writeLineBuffer;
  private List<string> outputLineBuffer;
  private StringBuilder outputBuilder;
  private Action raiseHasWriteLinesAction;
  private IContainer components;
  private TextBox tbOutput;
  private ToolStrip tsOutputViewTools;
  private ToolStripButton tsbClearLog;
  private Timer tmShowOutput;

  public OutputView()
  {
    this.InitializeComponent();
    this.writeLineSyncRoot = new object();
    this.writeLineBuffer = new List<string>(1024 /*0x0400*/);
    this.outputLineBuffer = new List<string>(1024 /*0x0400*/);
    this.outputBuilder = new StringBuilder(8192 /*0x2000*/);
    this.raiseHasWriteLinesAction = new Action(this.RaiseHasWriteLines);
  }

  private void tsbClearLog_Click(object sender, EventArgs e) => this.tbOutput.Text = string.Empty;

  private void tmShowOutput_Tick(object sender, EventArgs e)
  {
    this.tmShowOutput.Enabled = false;
    this.ShowOutput();
  }

  private void ShowOutput()
  {
    bool flag = false;
    try
    {
      lock (this.writeLineSyncRoot)
      {
        int count = Math.Min(32768 /*0x8000*/, this.writeLineBuffer.Count);
        if (count != 0)
        {
          this.outputLineBuffer.AddRange((IEnumerable<string>) this.writeLineBuffer.GetRange(0, count));
          this.writeLineBuffer.RemoveRange(0, count);
          if (this.writeLineBuffer.Count != 0)
            flag = true;
        }
      }
      if (this.outputLineBuffer.Count == 0)
        return;
      int num = 0;
      foreach (string str in this.outputLineBuffer)
        num += str.Length;
      this.outputBuilder.Capacity = Math.Max(this.outputBuilder.Capacity, num + OutputView.newLine.Length * this.outputLineBuffer.Count);
      foreach (string str in this.outputLineBuffer)
      {
        this.outputBuilder.Append(str);
        if (!str.EndsWith(OutputView.newLine))
          this.outputBuilder.Append(OutputView.newLine);
      }
      this.tbOutput.AppendText(this.outputBuilder.ToString());
    }
    finally
    {
      this.outputLineBuffer.Clear();
      this.outputBuilder.Clear();
      if (flag)
        this.RaiseHasWriteLines();
    }
  }

  private void RaiseHasWriteLines()
  {
    if (this.tmShowOutput.Enabled)
      return;
    if (this.InvokeRequired)
      this.BeginInvoke((Delegate) this.raiseHasWriteLinesAction);
    else
      this.tmShowOutput.Enabled = true;
  }

  public void WriteLine(string line)
  {
    lock (this.writeLineSyncRoot)
    {
      this.ApplyWriteLineBufferOverflowProtection();
      this.writeLineBuffer.Add(line);
    }
    this.RaiseHasWriteLines();
  }

  private void ApplyWriteLineBufferOverflowProtection()
  {
    if (this.writeLineBuffer.Count <= 1048576 /*0x100000*/)
      return;
    this.writeLineBuffer.RemoveRange(1048575 /*0x0FFFFF*/, this.writeLineBuffer.Count - 1048576 /*0x100000*/ + 1);
    this.writeLineBuffer.Add("<some output skipped>");
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this.tbOutput = new TextBox();
    this.tsOutputViewTools = new ToolStrip();
    this.tsbClearLog = new ToolStripButton();
    this.tmShowOutput = new Timer(this.components);
    this.tsOutputViewTools.SuspendLayout();
    this.SuspendLayout();
    this.tbOutput.Dock = DockStyle.Fill;
    this.tbOutput.Location = new Point(0, 25);
    this.tbOutput.Multiline = true;
    this.tbOutput.Name = "tbOutput";
    this.tbOutput.ScrollBars = ScrollBars.Both;
    this.tbOutput.Size = new Size(284, 237);
    this.tbOutput.TabIndex = 1;
    this.tbOutput.TabStop = false;
    this.tbOutput.WordWrap = false;
    this.tsOutputViewTools.Items.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this.tsbClearLog
    });
    this.tsOutputViewTools.Location = new Point(0, 0);
    this.tsOutputViewTools.Name = "tsOutputViewTools";
    this.tsOutputViewTools.Size = new Size(284, 25);
    this.tsOutputViewTools.TabIndex = 0;
    this.tsOutputViewTools.Text = "toolStrip1";
    this.tsbClearLog.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.tsbClearLog.Image = (Image) IDEInternalResources.IR_ClearLog16;
    this.tsbClearLog.ImageTransparentColor = Color.Magenta;
    this.tsbClearLog.Name = "tsbClearLog";
    this.tsbClearLog.Size = new Size(23, 22);
    this.tsbClearLog.Text = "Очистить";
    this.tsbClearLog.Click += new EventHandler(this.tsbClearLog_Click);
    this.tmShowOutput.Interval = 150;
    this.tmShowOutput.Tick += new EventHandler(this.tmShowOutput_Tick);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(284, 262);
    this.CloseButton = false;
    this.CloseButtonVisible = false;
    this.Controls.Add((Control) this.tbOutput);
    this.Controls.Add((Control) this.tsOutputViewTools);
    this.DockAreas = DockAreas.Float | DockAreas.DockBottom | DockAreas.Document;
    this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.HideOnClose = true;
    this.Name = nameof (OutputView);
    this.TabText = "Вывод";
    this.Text = nameof (OutputView);
    this.tsOutputViewTools.ResumeLayout(false);
    this.tsOutputViewTools.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}

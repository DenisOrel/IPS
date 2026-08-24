// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.ErrorsView
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views;

internal class ErrorsView : DockContent, IErrorsView
{
  private IContainer components;
  private ListView lvErrors;
  private ColumnHeader chCode;
  private ColumnHeader chDescription;
  private ColumnHeader chLine;
  private ColumnHeader chColumn;
  private ColumnHeader chScriptName;
  private ToolStrip tsErrorsTools;
  private ToolStripButton tsbShowError;

  public ErrorsView() => this.InitializeComponent();

  private void lvErrors_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (this.lvErrors.SelectedItems.Count != 0)
      this.tsbShowError.Enabled = true;
    else
      this.tsbShowError.Enabled = false;
  }

  private void lvErrors_ShowSelectedError(object sender, EventArgs e)
  {
    if (this.lvErrors.SelectedItems.Count == 0 || this.ShowSelectedError == null)
      return;
    this.ShowSelectedError((object) this, EventArgs.Empty);
  }

  public void SetErrors(ICollection<ScriptProjectErrorRecord> errors)
  {
    if (errors == null)
      throw new ArgumentNullException(nameof (errors));
    this.lvErrors.BeginUpdate();
    try
    {
      this.lvErrors.Items.Clear();
      if (errors.Count == 0)
        return;
      foreach (ScriptProjectErrorRecord error in (IEnumerable<ScriptProjectErrorRecord>) errors)
        this.lvErrors.Items.Add(this.CreateErrorItem(error));
      this.chScriptName.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
      this.lvErrors.Items[0].Selected = true;
    }
    finally
    {
      this.lvErrors.EndUpdate();
    }
  }

  private ListViewItem CreateErrorItem(ScriptProjectErrorRecord errorRecord)
  {
    ScriptCompilationError error = errorRecord.Error;
    return new ListViewItem(error.ErrorNumber)
    {
      SubItems = {
        error.ErrorText,
        error.Line.ToString(),
        error.Column.ToString(),
        errorRecord.ScriptDisplayName
      },
      Tag = (object) errorRecord
    };
  }

  public ScriptProjectErrorRecord TryGetSelectedError()
  {
    return this.lvErrors.SelectedItems.Count != 0 ? (ScriptProjectErrorRecord) this.lvErrors.SelectedItems[0].Tag : (ScriptProjectErrorRecord) null;
  }

  public event EventHandler ShowSelectedError;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.lvErrors = new ListView();
    this.chCode = new ColumnHeader();
    this.chDescription = new ColumnHeader();
    this.chLine = new ColumnHeader();
    this.chColumn = new ColumnHeader();
    this.chScriptName = new ColumnHeader();
    this.tsErrorsTools = new ToolStrip();
    this.tsbShowError = new ToolStripButton();
    this.tsErrorsTools.SuspendLayout();
    this.SuspendLayout();
    this.lvErrors.Columns.AddRange(new ColumnHeader[5]
    {
      this.chCode,
      this.chDescription,
      this.chLine,
      this.chColumn,
      this.chScriptName
    });
    this.lvErrors.Dock = DockStyle.Fill;
    this.lvErrors.FullRowSelect = true;
    this.lvErrors.HideSelection = false;
    this.lvErrors.Location = new Point(0, 25);
    this.lvErrors.MultiSelect = false;
    this.lvErrors.Name = "lvErrors";
    this.lvErrors.Size = new Size(443, 219);
    this.lvErrors.TabIndex = 0;
    this.lvErrors.UseCompatibleStateImageBehavior = false;
    this.lvErrors.View = View.Details;
    this.lvErrors.SelectedIndexChanged += new EventHandler(this.lvErrors_SelectedIndexChanged);
    this.lvErrors.DoubleClick += new EventHandler(this.lvErrors_ShowSelectedError);
    this.chCode.Text = "Код";
    this.chCode.Width = 80 /*0x50*/;
    this.chDescription.Text = "Описание";
    this.chDescription.Width = 400;
    this.chLine.Text = "Строка";
    this.chLine.TextAlign = HorizontalAlignment.Right;
    this.chColumn.Text = "Столбец";
    this.chColumn.TextAlign = HorizontalAlignment.Right;
    this.chScriptName.Text = "Сценарий";
    this.chScriptName.Width = 120;
    this.tsErrorsTools.Items.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this.tsbShowError
    });
    this.tsErrorsTools.Location = new Point(0, 0);
    this.tsErrorsTools.Name = "tsErrorsTools";
    this.tsErrorsTools.Size = new Size(443, 25);
    this.tsErrorsTools.TabIndex = 1;
    this.tsErrorsTools.Text = "toolStrip1";
    this.tsbShowError.Enabled = false;
    this.tsbShowError.Image = (Image) IDEInternalResources.IR_Goto16;
    this.tsbShowError.ImageTransparentColor = Color.Magenta;
    this.tsbShowError.Name = "tsbShowError";
    this.tsbShowError.Size = new Size(77, 22);
    this.tsbShowError.Text = "Показать";
    this.tsbShowError.ToolTipText = "Показывает выбранную ошибку в коде сценария";
    this.tsbShowError.Click += new EventHandler(this.lvErrors_ShowSelectedError);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(443, 244);
    this.CloseButton = false;
    this.CloseButtonVisible = false;
    this.Controls.Add((Control) this.lvErrors);
    this.Controls.Add((Control) this.tsErrorsTools);
    this.DockAreas = DockAreas.Float | DockAreas.DockBottom | DockAreas.Document;
    this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.Name = nameof (ErrorsView);
    this.TabText = "Ошибки";
    this.Text = nameof (ErrorsView);
    this.tsErrorsTools.ResumeLayout(false);
    this.tsErrorsTools.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}

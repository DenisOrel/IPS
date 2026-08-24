// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ReportsViewControl
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Client.Editors;
using Intermech.Office.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class ReportsViewControl : ReportActionControl
{
  [NotNull]
  private readonly Report _report;
  private IContainer components;
  private SplitContainer splitContainer1;
  private ListView lvAuthors;
  private ColumnHeader columnHeader1;
  private ColumnHeader columnHeader2;
  private GroupBox groupBox1;
  private TextBox tbReport;

  public ReportsViewControl([NotNull] IDBResolution resolution)
    : base(resolution.ObjectID)
  {
    this.InitializeComponent();
    this._report = resolution.IsPrivate ? (Report) new ConfidentialReport(this._Resolution) : new Report(this._Resolution);
  }

  public override void OnLoadData([NotNull] IUserSession session, IDBResolution resolution)
  {
    ResolutionProgressReportRecord[] progressReportRecords = resolution.ProgressReportRecords;
    if (!((IEnumerable<ResolutionProgressReportRecord>) progressReportRecords).Any<ResolutionProgressReportRecord>())
      return;
    this.lvAuthors.BeginUpdate();
    try
    {
      int num = 0;
      foreach (ResolutionProgressReportRecord progressReportRecord in progressReportRecords)
        this.lvAuthors.Items.Add(new ListViewItem(new string[2]
        {
          session.GetObjectInfo(progressReportRecord.AuthorID).Caption,
          progressReportRecord.ReleaseDate.ToString((IFormatProvider) CultureInfo.CurrentCulture)
        })
        {
          Tag = (object) num++
        });
    }
    finally
    {
      this.lvAuthors.EndUpdate();
    }
    if (this.lvAuthors.Items.Count <= 0)
      return;
    this.lvAuthors.Items[0].Selected = true;
    this.SelectReport(session, (int) this.lvAuthors.Items[0].Tag);
  }

  private void SelectReport([NotNull] IUserSession session, int index)
  {
    this.tbReport.Text = this._report.Load(session, index);
  }

  private void lvAuthors_SelectedIndexChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (this.lvAuthors.FocusedItem?.Tag == null)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.SelectReport(sessionKeeper.Session, (int) this.lvAuthors.FocusedItem.Tag);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ReportsViewControl));
    this.splitContainer1 = new SplitContainer();
    this.lvAuthors = new ListView();
    this.columnHeader1 = new ColumnHeader();
    this.columnHeader2 = new ColumnHeader();
    this.groupBox1 = new GroupBox();
    this.tbReport = new TextBox();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
    this.splitContainer1.Name = "splitContainer1";
    componentResourceManager.ApplyResources((object) this.splitContainer1.Panel1, "splitContainer1.Panel1");
    this.splitContainer1.Panel1.Controls.Add((Control) this.lvAuthors);
    componentResourceManager.ApplyResources((object) this.splitContainer1.Panel2, "splitContainer1.Panel2");
    this.splitContainer1.Panel2.Controls.Add((Control) this.groupBox1);
    componentResourceManager.ApplyResources((object) this.lvAuthors, "lvAuthors");
    this.lvAuthors.Columns.AddRange(new ColumnHeader[2]
    {
      this.columnHeader1,
      this.columnHeader2
    });
    this.lvAuthors.GridLines = true;
    this.lvAuthors.MultiSelect = false;
    this.lvAuthors.Name = "lvAuthors";
    this.lvAuthors.UseCompatibleStateImageBehavior = false;
    this.lvAuthors.View = View.Details;
    this.lvAuthors.SelectedIndexChanged += new EventHandler(this.lvAuthors_SelectedIndexChanged);
    componentResourceManager.ApplyResources((object) this.columnHeader1, "columnHeader1");
    componentResourceManager.ApplyResources((object) this.columnHeader2, "columnHeader2");
    componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
    this.groupBox1.Controls.Add((Control) this.tbReport);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.TabStop = false;
    componentResourceManager.ApplyResources((object) this.tbReport, "tbReport");
    this.tbReport.BackColor = SystemColors.Window;
    this.tbReport.Name = "tbReport";
    this.tbReport.ReadOnly = true;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.splitContainer1);
    this.Name = nameof (ReportsViewControl);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.ResumeLayout(false);
  }
}

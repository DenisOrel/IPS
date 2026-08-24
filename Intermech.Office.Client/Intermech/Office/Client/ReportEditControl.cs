// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ReportEditControl
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Client.Editors;
using Intermech.Office.Interfaces;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class ReportEditControl : ReportActionControl
{
  [NotNull]
  private readonly Report _report;
  private IContainer components;
  private Panel panel2;
  private TextBox textBox1;

  public ReportEditControl([NotNull] IDBResolution resolution)
    : base(resolution.ObjectID)
  {
    this.InitializeComponent();
    this._report = resolution.IsPrivate ? (Report) new ConfidentialReport(this._Resolution) : new Report(this._Resolution);
  }

  public override bool OnSaveData(IUserSession session)
  {
    if (!this._Changed)
      return true;
    IDBObject dbObject = session.GetObject(this._Resolution);
    int index = this.GetIndex((IDBResolution) dbObject, session.UserID);
    IDBTransactions customService = (IDBTransactions) session.GetCustomService(typeof (IDBTransactions));
    customService.StartTransaction();
    try
    {
      this._report.Save(dbObject, index, this.textBox1.Text);
      if (index >= 0)
      {
        IDBAttribute dbAttribute = dbObject.AttributeByID(OfficeConsts.AttrReportDatesID);
        dbAttribute.Index = index;
        dbAttribute.Value = (object) Consts.CurrentDateFunction;
      }
      else
      {
        dbObject.AddValueToMultiObjLinkAttr(OfficeConsts.AttrReportAuthorsID, session.UserID);
        dbObject.AddValueToMultiStrAttr(OfficeConsts.AttrReportDatesID, Consts.CurrentDateFunction);
      }
      customService.Commit();
      return true;
    }
    catch
    {
      customService.Rollback();
      throw;
    }
  }

  public override void OnLoadData([NotNull] IUserSession session, IDBResolution resolution)
  {
    int index = this.GetIndex(resolution, session.UserID);
    if (index < 0)
      return;
    this.textBox1.Text = this._report.Load(session, index);
  }

  private void textBox1_TextChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.OnChanged();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ReportEditControl));
    this.panel2 = new Panel();
    this.textBox1 = new TextBox();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Controls.Add((Control) this.textBox1);
    this.panel2.Name = "panel2";
    componentResourceManager.ApplyResources((object) this.textBox1, "textBox1");
    this.textBox1.Name = "textBox1";
    this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.panel2);
    this.Name = nameof (ReportEditControl);
    this.panel2.ResumeLayout(false);
    this.panel2.PerformLayout();
    this.ResumeLayout(false);
  }
}

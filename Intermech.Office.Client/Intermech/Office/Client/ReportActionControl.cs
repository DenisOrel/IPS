// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ReportActionControl
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class ReportActionControl : UserControl, IReportEditorControl
{
  protected bool _Changed;
  protected long _Resolution;
  private IContainer components;

  public ReportActionControl()
  {
  }

  public ReportActionControl(long resolutionID) => this._Resolution = resolutionID;

  public event EventHandler Changed;

  public virtual bool OnSaveData(IUserSession session) => true;

  public virtual void OnLoadData(IUserSession session, IDBResolution resolution)
  {
  }

  protected void OnChanged()
  {
    if (this.Changed != null)
      this.Changed((object) this, new EventArgs());
    this._Changed = true;
  }

  [NotNull]
  protected string GetReportText([NotNull] IUserSession session, int index)
  {
    IDBAttribute attributeById = session.GetObject(this._Resolution).GetAttributeByID(OfficeConsts.AttrReportsID);
    if (attributeById == null || attributeById.ValuesCount < index)
      return string.Empty;
    attributeById.Index = index;
    return (string) attributeById.Value;
  }

  protected int GetIndex([NotNull] IDBResolution resolution, long userID)
  {
    return ((IEnumerable<ResolutionProgressReportRecord>) resolution.ProgressReportRecords).IndexOfFirst<ResolutionProgressReportRecord>((Predicate<ResolutionProgressReportRecord>) (progressReportRecord => progressReportRecord.AuthorID == userID));
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ReportActionControl));
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (ReportActionControl);
    this.ResumeLayout(false);
  }
}

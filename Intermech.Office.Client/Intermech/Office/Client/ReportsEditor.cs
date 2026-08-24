// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ReportsEditor
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

internal class ReportsEditor : Form
{
  private readonly long _resolutionID;
  private IContainer components;
  private Panel panel1;
  private Button bCancel;
  private Button bOK;
  private Panel pControl;

  public ReportsEditor(long resolutionID)
  {
    this._resolutionID = resolutionID;
    this.InitializeComponent();
  }

  internal void Init()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IReportEditorControl reportEditorControl = (IReportEditorControl) null;
      IDBResolution resolution = sessionKeeper.Session.GetResolution(this._resolutionID);
      if (resolution.IsUserAnyOfRoles(ResolutionUserRoles.Executor))
      {
        reportEditorControl = (IReportEditorControl) new ReportEditControl(resolution);
        this.AcceptButton = (IButtonControl) null;
      }
      else if (resolution.IsUserAnyOfRoles(ResolutionUserRoles.Admin | ResolutionUserRoles.Creator | ResolutionUserRoles.Author | ResolutionUserRoles.Controller))
      {
        reportEditorControl = (IReportEditorControl) new ReportsViewControl(resolution);
        this.AcceptButton = (IButtonControl) this.bOK;
      }
      if (reportEditorControl == null)
        return;
      reportEditorControl.Changed += new EventHandler(ReportsEditor.control_Changed);
      reportEditorControl.OnLoadData(sessionKeeper.Session, resolution);
      this.pControl.Controls.Add((Control) reportEditorControl);
      ((Control) reportEditorControl).Dock = DockStyle.Fill;
    }
  }

  private static void control_Changed([CanBeNull] object sender, [NotNull] EventArgs e)
  {
  }

  private void bOK_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      ((IReportEditorControl) this.pControl.Controls[0]).OnSaveData(sessionKeeper.Session);
    this.DialogResult = DialogResult.OK;
    this.Close();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ReportsEditor));
    this.panel1 = new Panel();
    this.bCancel = new Button();
    this.bOK = new Button();
    this.pControl = new Panel();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOK);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.bOK, "bOK");
    this.bOK.Name = "bOK";
    this.bOK.UseVisualStyleBackColor = true;
    this.bOK.Click += new EventHandler(this.bOK_Click);
    componentResourceManager.ApplyResources((object) this.pControl, "pControl");
    this.pControl.Name = "pControl";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.pControl);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (ReportsEditor);
    this.panel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}

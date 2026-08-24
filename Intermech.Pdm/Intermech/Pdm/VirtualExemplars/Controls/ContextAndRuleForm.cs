// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VirtualExemplars.Controls.ContextAndRuleForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.VirtualExemplars.Controls;

public class ContextAndRuleForm : Form
{
  private IContainer components;
  private Panel panelBottom;
  private Panel panelInfo;
  private Button btnNext;
  private Button btnCancel;
  private PictureBox pictureInfo;
  private Label labelTopic;

  public ContextAndRuleForm()
  {
    this.InitializeComponent();
    IFiltrationService service1 = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
    ICurrentUserAndRole service2 = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    string str1 = service1.RuleClass != null ? service1.RuleClass.RuleObjectCaption : service1.Filtration.Caption;
    string str2 = string.Empty;
    if (service2.CachedEditingContextID != 0L)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(service2.CachedEditingContextID);
        if (!objectInfo.Empty)
          str2 = objectInfo.Caption;
      }
    }
    this.labelTopic.Text = !string.IsNullOrEmpty(str2) ? string.Format(LocalizationHolder.rm.GetString("Pdm_ExemplarsInfo_1"), (object) str2, (object) str1) : string.Format(LocalizationHolder.rm.GetString("Pdm_ExemplarsInfo_2"), (object) str1);
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1487);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ContextAndRuleForm));
    this.panelBottom = new Panel();
    this.btnNext = new Button();
    this.btnCancel = new Button();
    this.panelInfo = new Panel();
    this.labelTopic = new Label();
    this.pictureInfo = new PictureBox();
    this.panelBottom.SuspendLayout();
    this.panelInfo.SuspendLayout();
    ((ISupportInitialize) this.pictureInfo).BeginInit();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.panelBottom, "panelBottom");
    this.panelBottom.Controls.Add((Control) this.btnNext);
    this.panelBottom.Controls.Add((Control) this.btnCancel);
    this.panelBottom.Name = "panelBottom";
    componentResourceManager.ApplyResources((object) this.btnNext, "btnNext");
    this.btnNext.DialogResult = DialogResult.OK;
    this.btnNext.Name = "btnNext";
    this.btnNext.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.panelInfo, "panelInfo");
    this.panelInfo.Controls.Add((Control) this.labelTopic);
    this.panelInfo.Controls.Add((Control) this.pictureInfo);
    this.panelInfo.Name = "panelInfo";
    componentResourceManager.ApplyResources((object) this.labelTopic, "labelTopic");
    this.labelTopic.Name = "labelTopic";
    componentResourceManager.ApplyResources((object) this.pictureInfo, "pictureInfo");
    this.pictureInfo.Name = "pictureInfo";
    this.pictureInfo.TabStop = false;
    this.AcceptButton = (IButtonControl) this.btnNext;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.Controls.Add((Control) this.panelInfo);
    this.Controls.Add((Control) this.panelBottom);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ContextAndRuleForm);
    this.ShowInTaskbar = false;
    this.SizeGripStyle = SizeGripStyle.Hide;
    this.panelBottom.ResumeLayout(false);
    this.panelInfo.ResumeLayout(false);
    ((ISupportInitialize) this.pictureInfo).EndInit();
    this.ResumeLayout(false);
  }
}

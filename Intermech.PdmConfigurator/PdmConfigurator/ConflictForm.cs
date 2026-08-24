// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.ConflictForm
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Interfaces;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.PdmConfigurator;

public class ConflictForm : Form
{
  private Button btnOK;
  private TextBox tbErrorMessage;
  private int _fullHeight;
  private Button button4;
  private SaveFileDialog sd;
  private string errorMessage = string.Empty;
  private System.ComponentModel.Container components;

  public ConflictForm()
  {
    this.InitializeComponent();
    this._fullHeight = this.Height;
  }

  public static DialogResult ShowErrorDialog(string errorMessage)
  {
    return new ConflictForm().ShowErrorMessage(errorMessage);
  }

  public DialogResult ShowErrorMessage(string errorMessage)
  {
    this.tbErrorMessage.Text = this.errorMessage = errorMessage;
    return this.ShowDialog();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ConflictForm));
    this.tbErrorMessage = new TextBox();
    this.btnOK = new Button();
    this.button4 = new Button();
    this.sd = new SaveFileDialog();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.tbErrorMessage, "tbErrorMessage");
    this.tbErrorMessage.Name = "tbErrorMessage";
    this.tbErrorMessage.ReadOnly = true;
    componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
    this.btnOK.DialogResult = DialogResult.Ignore;
    this.btnOK.Name = "btnOK";
    componentResourceManager.ApplyResources((object) this.button4, "button4");
    this.button4.Name = "button4";
    this.button4.Click += new EventHandler(this.button4_Click);
    this.sd.CheckPathExists = false;
    this.sd.DefaultExt = "xml";
    componentResourceManager.ApplyResources((object) this.sd, "sd");
    this.sd.RestoreDirectory = true;
    this.sd.SupportMultiDottedExtensions = true;
    this.AcceptButton = (IButtonControl) this.btnOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.CancelButton = (IButtonControl) this.btnOK;
    this.Controls.Add((Control) this.button4);
    this.Controls.Add((Control) this.btnOK);
    this.Controls.Add((Control) this.tbErrorMessage);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ConflictForm);
    this.SizeGripStyle = SizeGripStyle.Hide;
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private void button4_Click(object sender, EventArgs e)
  {
    this.sd.FileName = "PDM_Configurator.xml";
    if (this.sd.ShowDialog() != DialogResult.OK)
      return;
    XMLSettingsStorage xmlSettingsStorage = new XMLSettingsStorage();
    xmlSettingsStorage.document.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><IPS />");
    xmlSettingsStorage.AddNode((XmlNode) xmlSettingsStorage.document.DocumentElement, "linked_options_conflict").InnerText = this.errorMessage;
    xmlSettingsStorage.Save(this.sd.FileName);
  }
}

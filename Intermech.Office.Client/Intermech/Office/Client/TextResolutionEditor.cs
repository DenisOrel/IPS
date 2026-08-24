// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.TextResolutionEditor
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class TextResolutionEditor : Form
{
  private const string PassStr = "CF8FE109-CC13-48d0-BDEC-5D00FAAA75D4";
  private readonly long _unitID;
  [CanBeNull]
  private string _oldText;
  private IContainer components;
  private Panel panel2;
  private Button button2;
  private RichTextBox richTextBox1;

  public TextResolutionEditor([NotEmpty] long unitID)
  {
    this._unitID = unitID;
    this.InitializeComponent();
  }

  internal void Init()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBResolution resolution = sessionKeeper.Session.GetResolution(this._unitID);
      if (!resolution.IsUserAnyOfRoles(ResolutionUserRoles.Admin | ResolutionUserRoles.Creator | ResolutionUserRoles.Author))
        this.richTextBox1.ReadOnly = true;
      IDBAttribute attributeById = resolution.GetAttributeByID(OfficeConsts.AttrPrivacyTextID);
      if (attributeById != null)
      {
        IBlobReader blobReader = attributeById.As<IBlobReader>();
        BlobInformation blobInformation = blobReader.OpenBlob(0);
        if (blobInformation.RealFileSize > 0L)
        {
          byte[] cipherBytes = blobReader.ReadDataBlock((int) blobInformation.RealFileSize);
          if (cipherBytes.Length != 0)
            this.richTextBox1.Text = Cryptor.Decrypt(cipherBytes, "CF8FE109-CC13-48d0-BDEC-5D00FAAA75D4");
        }
      }
      this._oldText = this.richTextBox1.Text;
    }
  }

  private void button2_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (!this.richTextBox1.ReadOnly && this._oldText != this.richTextBox1.Text)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._unitID);
        IDBAttribute dbAttribute = dbObject.GetAttributeByID(OfficeConsts.AttrPrivacyTextID) ?? dbObject.Attributes.AddAttribute(OfficeConsts.AttrPrivacyTextID, false);
        if (this.richTextBox1.Text == string.Empty)
          dbAttribute.Clear();
        IBlobWriter blobWriter = dbAttribute.As<IBlobWriter>();
        byte[] data = Cryptor.EncryptEx(this.richTextBox1.Text, "CF8FE109-CC13-48d0-BDEC-5D00FAAA75D4");
        if (blobWriter.OpenBlob(new BlobInformation((long) data.Length, (long) data.Length, DateTime.Now, string.Empty, ArcMethods.NotPacked, string.Empty), false))
          blobWriter.WriteDataBlock(data);
      }
    }
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TextResolutionEditor));
    this.panel2 = new Panel();
    this.button2 = new Button();
    this.richTextBox1 = new RichTextBox();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Controls.Add((Control) this.button2);
    this.panel2.Name = "panel2";
    componentResourceManager.ApplyResources((object) this.button2, "button2");
    this.button2.DialogResult = DialogResult.OK;
    this.button2.Name = "button2";
    this.button2.UseVisualStyleBackColor = true;
    this.button2.Click += new EventHandler(this.button2_Click);
    componentResourceManager.ApplyResources((object) this.richTextBox1, "richTextBox1");
    this.richTextBox1.Name = "richTextBox1";
    this.AcceptButton = (IButtonControl) this.button2;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.richTextBox1);
    this.Controls.Add((Control) this.panel2);
    this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
    this.Name = nameof (TextResolutionEditor);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}

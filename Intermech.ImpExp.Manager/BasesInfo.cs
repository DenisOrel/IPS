// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.BasesInfo
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Remoting;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager;

public class BasesInfo : Form
{
  private IContainer components;
  private Button bOK;
  private RichTextBox richTextBox1;

  public BasesInfo(PluginsManager plugins)
  {
    this.InitializeComponent();
    string str = string.Empty;
    foreach (WellKnownClientTypeEntry wellKnownClientType in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
      str = wellKnownClientType.ObjectUrl;
    if (str != string.Empty)
      this.richTextBox1.Text = $"Сервер приложений IPS:{Environment.NewLine}{str}{Environment.NewLine}";
    foreach (IPlugin plugins1 in plugins.PluginsList)
    {
      string[] connectInfo = plugins1.ConnectInfo;
      if (connectInfo != null)
      {
        this.richTextBox1.AppendText($"{plugins1.Description}:" + Environment.NewLine);
        for (int index = 0; index < connectInfo.Length; ++index)
          this.richTextBox1.AppendText(connectInfo[index] + Environment.NewLine);
      }
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BasesInfo));
    this.bOK = new Button();
    this.richTextBox1 = new RichTextBox();
    this.SuspendLayout();
    this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bOK.DialogResult = DialogResult.Cancel;
    this.bOK.Location = new Point(383, 208 /*0xD0*/);
    this.bOK.Name = "bOK";
    this.bOK.Size = new Size(75, 23);
    this.bOK.TabIndex = 0;
    this.bOK.Text = "OK";
    this.bOK.UseVisualStyleBackColor = true;
    this.richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.richTextBox1.BackColor = SystemColors.Window;
    this.richTextBox1.Location = new Point(12, 12);
    this.richTextBox1.Name = "richTextBox1";
    this.richTextBox1.ReadOnly = true;
    this.richTextBox1.Size = new Size(446, 190);
    this.richTextBox1.TabIndex = 1;
    this.richTextBox1.Text = "";
    this.AcceptButton = (IButtonControl) this.bOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bOK;
    this.ClientSize = new Size(470, 243);
    this.Controls.Add((Control) this.richTextBox1);
    this.Controls.Add((Control) this.bOK);
    this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.MinimumSize = new Size(300, 250);
    this.Name = nameof (BasesInfo);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Информация о базах данных";
    this.ResumeLayout(false);
  }
}

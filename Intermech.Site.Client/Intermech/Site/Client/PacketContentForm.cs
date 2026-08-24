// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PacketContentForm
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Client.Core;
using Intermech.Document.Client;
using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Site.Client.PortalNavigator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace Intermech.Site.Client;

public class PacketContentForm : Form
{
  private IContainer components;
  private Button button1;
  private ComboBox comboBox1;
  private Label label1;
  private Panel panel1;

  public PacketContentForm() => this.InitializeComponent();

  public void Initialize(List<IPacketNodeID> packets)
  {
    foreach (PacketNodeID packet in packets)
      this.comboBox1.Items.Add((object) packet);
    this.comboBox1.SelectedIndex = 0;
  }

  private void LoadContent(IUserSession session, IPacketNodeID packet)
  {
    if (this.panel1.Controls.Count > 0)
    {
      foreach (Component control in (ArrangedElementCollection) this.panel1.Controls)
        control.Dispose();
    }
    Guid guid = Guid.Empty;
    IPortalConnector customService = (IPortalConnector) session.GetCustomService(typeof (IPortalConnector));
    try
    {
      guid = customService.Login(session.SessionGUID);
      DataTable packetContent = customService.GetPacketContent(guid, packet.ObjectID);
      if (packetContent == null)
        return;
      ImDocumentEditorForm form = ReceiptTableHelper.LoadDocumentToForm(session, packetContent, $"Содержимое пакета {packet.Caption}", packet.CreateDate);
      form.Dock = DockStyle.Fill;
      form.BorderStyle = Intermech.Docking.Rendering.BorderStyle.None;
      form.Parent = (Control) this.panel1;
      form.Visible = true;
    }
    finally
    {
      if (guid != Guid.Empty && customService != null)
        customService.Logout(guid);
    }
  }

  private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (!(this.comboBox1.SelectedItem is IPacketNodeID selectedItem))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.LoadContent(sessionKeeper.Session, selectedItem);
  }

  private void PacketContentForm_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void PacketContentForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.button1 = new Button();
    this.comboBox1 = new ComboBox();
    this.label1 = new Label();
    this.panel1 = new Panel();
    this.SuspendLayout();
    this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.button1.DialogResult = DialogResult.Cancel;
    this.button1.Location = new Point(892, 575);
    this.button1.Name = "button1";
    this.button1.Size = new Size(121, 27);
    this.button1.TabIndex = 1;
    this.button1.Text = "Закрыть";
    this.button1.UseVisualStyleBackColor = true;
    this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
    this.comboBox1.FormattingEnabled = true;
    this.comboBox1.Location = new Point(56, 6);
    this.comboBox1.Name = "comboBox1";
    this.comboBox1.Size = new Size(412, 21);
    this.comboBox1.TabIndex = 2;
    this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
    this.label1.AutoSize = true;
    this.label1.Location = new Point(12, 9);
    this.label1.Name = "label1";
    this.label1.Size = new Size(41, 13);
    this.label1.TabIndex = 3;
    this.label1.Text = "Пакет:";
    this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.panel1.Location = new Point(15, 33);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(996, 536);
    this.panel1.TabIndex = 4;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.button1;
    this.ClientSize = new Size(1023 /*0x03FF*/, 613);
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.comboBox1);
    this.Controls.Add((Control) this.button1);
    this.Name = nameof (PacketContentForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Содержимое импортируемого пакета";
    this.FormClosing += new FormClosingEventHandler(this.PacketContentForm_FormClosing);
    this.Load += new EventHandler(this.PacketContentForm_Load);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}

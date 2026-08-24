// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ImportPacketForm
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Interfaces;
using Intermech.Site.Client.PortalNavigator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

internal class ImportPacketForm : ImportForm
{
  private Label label1;
  private ComboBox cbVersionsMode;
  private List<IPacketNodeID> _packets;

  public ImportPacketForm() => this.InitializeComponent();

  public override void Initialize(ISelectedItems items, IServiceProvider viewServices)
  {
    this.groupBox2.Enabled = this.groupBox1.Enabled = this.cbOwners.Enabled = this.cbComposition.Enabled = this.bObjectTypesFilter.Enabled = this.cbAutoUpdate.Enabled = false;
    this.cbVersionsMode.SelectedIndex = 2;
    if (items.GetItemData(0, typeof (IPacketNodeID)) is IPacketNodeID)
      new PacketsListView(this.viewObjectsList).InitView(items, viewServices, out this.objectIDs);
    this._packets = new List<IPacketNodeID>(items.Count);
    for (int index = 0; index < items.Count; ++index)
      this._packets.Add(items.GetItemData(index, typeof (IPacketNodeID)) as IPacketNodeID);
  }

  public override object Options
  {
    get
    {
      return (object) new ImportPacketOptions()
      {
        ImportVersionsMode = (ImportVersionsModes) this.cbVersionsMode.SelectedIndex,
        StartImmediately = this.cbStartImmediately.Checked
      };
    }
  }

  protected override void ShowImportObjectList(
    IUserSession session,
    IPortalConnector connection,
    Guid connectGuid)
  {
    using (PacketContentForm packetContentForm = new PacketContentForm())
    {
      packetContentForm.Initialize(this._packets);
      int num = (int) packetContentForm.ShowDialog();
    }
  }

  private void InitializeComponent()
  {
    this.cbVersionsMode = new ComboBox();
    this.label1 = new Label();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.SuspendLayout();
    this.panel2.Controls.Add((Control) this.label1);
    this.panel2.Controls.Add((Control) this.cbVersionsMode);
    this.panel2.Controls.SetChildIndex((Control) this.groupBox2, 0);
    this.panel2.Controls.SetChildIndex((Control) this.groupBox1, 0);
    this.panel2.Controls.SetChildIndex((Control) this.bObjectTypesFilter, 0);
    this.panel2.Controls.SetChildIndex((Control) this.bObjectsList, 0);
    this.panel2.Controls.SetChildIndex((Control) this.cbAutoUpdate, 0);
    this.panel2.Controls.SetChildIndex((Control) this.cbStartImmediately, 0);
    this.panel2.Controls.SetChildIndex((Control) this.viewObjectsList, 0);
    this.panel2.Controls.SetChildIndex((Control) this.cbVersionsMode, 0);
    this.panel2.Controls.SetChildIndex((Control) this.label1, 0);
    this.bObjectTypesFilter.Location = new Point(409, 280);
    this.groupBox1.Location = new Point(367, 212);
    this.groupBox2.Location = new Point(14, 210);
    this.cbStartImmediately.Location = new Point(16 /*0x10*/, 298);
    this.cbAutoUpdate.Location = new Point(16 /*0x10*/, 275);
    this.bObjectsList.Location = new Point(525, 280);
    this.viewObjectsList.Size = new Size(616, 192 /*0xC0*/);
    this.cbVersionsMode.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.cbVersionsMode.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbVersionsMode.FormattingEnabled = true;
    this.cbVersionsMode.Items.AddRange(new object[3]
    {
      (object) "Перезаписывать все версии",
      (object) "Перезаписывать только более старые версии",
      (object) "Останавливать импорт если в пакете более старые версии"
    });
    this.cbVersionsMode.Location = new Point(16 /*0x10*/, 344);
    this.cbVersionsMode.Name = "cbVersionsMode";
    this.cbVersionsMode.Size = new Size(367, 21);
    this.cbVersionsMode.TabIndex = 15;
    this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(13, 327);
    this.label1.Name = "label1";
    this.label1.Size = new Size(231, 13);
    this.label1.TabIndex = 16 /*0x10*/;
    this.label1.Text = "Режим импорта версий объектов из пакета";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.ClientSize = new Size(644, 422);
    this.Name = nameof (ImportPacketForm);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.panel2.PerformLayout();
    this.groupBox1.ResumeLayout(false);
    this.groupBox2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.Settings.DemandAttributesForm
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client.Settings;

public class DemandAttributesForm : Form
{
  private IUserSession _UserSession;
  private ICategoryTypeIconService _categoryIcons;
  private IContainer components;
  private Panel pnlBottom;
  private Button btnCancel;
  private Button btnOK;
  private Label labelText;
  private ColumnHeader columnHeaderAttribute;
  public ListView lvAttributes;

  public DemandAttributesForm() => this.InitializeComponent();

  public DemandAttributesForm(IUserSession AUserSession)
    : this()
  {
    this._UserSession = AUserSession;
    this._categoryIcons = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this.lvAttributes.SmallImageList = this._categoryIcons.ImageList;
    IDBObjectType objectType = this._UserSession.GetObjectType(0);
    DataTable dataTable = objectType.Attributes.Select("F_ATTRIBUTE_ID", (object[]) null);
    this.Icon = this._categoryIcons.GetIcon(4, objectType.ObjectType);
    this.Text = objectType.ObjectTypeName;
    this.lvAttributes.Items.Clear();
    try
    {
      this.lvAttributes.BeginUpdate();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        IDBAttributeType attributeType = this._UserSession.GetAttributeType((int) row["F_ATTRIBUTE_ID"]);
        if (attributeType != null)
          this.lvAttributes.Items.Add(attributeType.Name, this._categoryIcons.IndexOf(3, -1, (object) attributeType.AttributeType)).Tag = (object) (attributeType as IDBGuid).GUID;
      }
    }
    finally
    {
      this.lvAttributes.EndUpdate();
    }
  }

  private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

  private void btnCancel_Click(object sender, EventArgs e)
  {
    this.DialogResult = DialogResult.Cancel;
  }

  private void lvAttributes_DoubleClick(object sender, EventArgs e)
  {
    this.DialogResult = DialogResult.OK;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.pnlBottom = new Panel();
    this.btnCancel = new Button();
    this.btnOK = new Button();
    this.lvAttributes = new ListView();
    this.columnHeaderAttribute = new ColumnHeader();
    this.labelText = new Label();
    this.pnlBottom.SuspendLayout();
    this.SuspendLayout();
    this.pnlBottom.BorderStyle = BorderStyle.FixedSingle;
    this.pnlBottom.Controls.Add((Control) this.btnCancel);
    this.pnlBottom.Controls.Add((Control) this.btnOK);
    this.pnlBottom.Dock = DockStyle.Bottom;
    this.pnlBottom.Location = new Point(0, 289);
    this.pnlBottom.Name = "pnlBottom";
    this.pnlBottom.Size = new Size(320, 45);
    this.pnlBottom.TabIndex = 0;
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.Location = new Point(232, 9);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(75, 23);
    this.btnCancel.TabIndex = 0;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnOK.Location = new Point(151, 9);
    this.btnOK.Name = "btnOK";
    this.btnOK.Size = new Size(75, 23);
    this.btnOK.TabIndex = 0;
    this.btnOK.Text = "ОК";
    this.btnOK.UseVisualStyleBackColor = true;
    this.btnOK.Click += new EventHandler(this.btnOK_Click);
    this.lvAttributes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.lvAttributes.Columns.AddRange(new ColumnHeader[1]
    {
      this.columnHeaderAttribute
    });
    this.lvAttributes.FullRowSelect = true;
    this.lvAttributes.HeaderStyle = ColumnHeaderStyle.None;
    this.lvAttributes.Location = new Point(12, 27);
    this.lvAttributes.MultiSelect = false;
    this.lvAttributes.Name = "lvAttributes";
    this.lvAttributes.Size = new Size(296, 256 /*0x0100*/);
    this.lvAttributes.TabIndex = 1;
    this.lvAttributes.UseCompatibleStateImageBehavior = false;
    this.lvAttributes.View = View.Details;
    this.lvAttributes.DoubleClick += new EventHandler(this.lvAttributes_DoubleClick);
    this.columnHeaderAttribute.Text = "Атрибут";
    this.columnHeaderAttribute.Width = 292;
    this.labelText.AutoSize = true;
    this.labelText.Location = new Point(12, 9);
    this.labelText.Name = "labelText";
    this.labelText.Size = new Size(159, 13);
    this.labelText.TabIndex = 2;
    this.labelText.Text = "Укажите атрибут для вставки";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(320, 334);
    this.Controls.Add((Control) this.labelText);
    this.Controls.Add((Control) this.lvAttributes);
    this.Controls.Add((Control) this.pnlBottom);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (DemandAttributesForm);
    this.ShowInTaskbar = false;
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = nameof (DemandAttributesForm);
    this.pnlBottom.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntityOkMessageDialog
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Advanced;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

public class EntityOkMessageDialog : ToolTipDialog
{
  private RichTextBox richTextBox_ErrorText;
  private Button button_OK;

  public string MessageText
  {
    get => this.richTextBox_ErrorText.Text;
    set => this.richTextBox_ErrorText.Text = value;
  }

  public EntityOkMessageDialog() => this.InitializeComponent();

  private void InitializeComponent()
  {
    this.button_OK = new Button();
    this.richTextBox_ErrorText = new RichTextBox();
    this.SuspendLayout();
    this.button_OK.BackColor = SystemColors.Control;
    this.button_OK.DialogResult = DialogResult.OK;
    this.button_OK.Location = new Point(283, 137);
    this.button_OK.Name = "button1";
    this.button_OK.Size = new Size(89, 23);
    this.button_OK.TabIndex = 0;
    this.button_OK.Text = "OK";
    this.button_OK.UseVisualStyleBackColor = false;
    this.richTextBox_ErrorText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.richTextBox_ErrorText.BackColor = this.BackColor;
    this.richTextBox_ErrorText.BorderStyle = System.Windows.Forms.BorderStyle.None;
    this.richTextBox_ErrorText.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
    this.richTextBox_ErrorText.Location = new Point(38, 38);
    this.richTextBox_ErrorText.Name = "richTextBox_ErrorText";
    this.richTextBox_ErrorText.ReadOnly = true;
    this.richTextBox_ErrorText.ShowSelectionMargin = true;
    this.richTextBox_ErrorText.Size = new Size(334, 98);
    this.richTextBox_ErrorText.TabIndex = 1;
    this.richTextBox_ErrorText.Text = "Неизвестная ошибка!";
    this.ClientSize = new Size(408, 190);
    this.Controls.Add((Control) this.richTextBox_ErrorText);
    this.Controls.Add((Control) this.button_OK);
    this.FrameBottomRight = SystemColors.ButtonFace;
    this.FrameTopLeft = SystemColors.ButtonShadow;
    this.Name = "OnEntityErrorDialog";
    this.ResumeLayout(false);
  }

  protected override void OnClick(EventArgs e)
  {
  }
}

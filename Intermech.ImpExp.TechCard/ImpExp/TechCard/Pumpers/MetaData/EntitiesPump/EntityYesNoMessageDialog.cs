// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntityYesNoMessageDialog
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Advanced;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

public class EntityYesNoMessageDialog : ToolTipDialog
{
  private Button buttonYes;
  private RichTextBox richTextBoxMessageText;
  private Button buttonNo;

  public EntityYesNoMessageDialog() => this.InitializeComponent();

  public string MessageText
  {
    set => this.richTextBoxMessageText.Text = value;
    get => this.richTextBoxMessageText.Text;
  }

  private void InitializeComponent()
  {
    this.buttonNo = new Button();
    this.buttonYes = new Button();
    this.richTextBoxMessageText = new RichTextBox();
    this.SuspendLayout();
    this.buttonNo.BackColor = SystemColors.Control;
    this.buttonNo.DialogResult = DialogResult.No;
    this.buttonNo.Location = new Point(190, 97);
    this.buttonNo.Name = "button1";
    this.buttonNo.Size = new Size(75, 23);
    this.buttonNo.TabIndex = 0;
    this.buttonNo.Text = "Нет";
    this.buttonNo.UseVisualStyleBackColor = false;
    this.buttonYes.BackColor = SystemColors.Control;
    this.buttonYes.DialogResult = DialogResult.Yes;
    this.buttonYes.Location = new Point(98, 97);
    this.buttonYes.Name = "button2";
    this.buttonYes.Size = new Size(75, 23);
    this.buttonYes.TabIndex = 2;
    this.buttonYes.Text = "Да";
    this.buttonYes.UseVisualStyleBackColor = false;
    this.richTextBoxMessageText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.richTextBoxMessageText.BackColor = this.BackColor;
    this.richTextBoxMessageText.BorderStyle = System.Windows.Forms.BorderStyle.None;
    this.richTextBoxMessageText.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
    this.richTextBoxMessageText.Location = new Point(35, 35);
    this.richTextBoxMessageText.Name = "richTextBox1";
    this.richTextBoxMessageText.ReadOnly = true;
    this.richTextBoxMessageText.Size = new Size(239, 61);
    this.richTextBoxMessageText.TabIndex = 3;
    this.richTextBoxMessageText.Text = "Неизвестная ошибка";
    this.ClientSize = new Size(312, 155);
    this.Controls.Add((Control) this.richTextBoxMessageText);
    this.Controls.Add((Control) this.buttonYes);
    this.Controls.Add((Control) this.buttonNo);
    this.FrameBottomRight = Color.Silver;
    this.FrameTopLeft = Color.DimGray;
    this.Name = "OnEntityAplyDialog";
    this.ResumeLayout(false);
  }

  protected override void OnClick(EventArgs e)
  {
  }
}

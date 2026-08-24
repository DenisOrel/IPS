// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ChoiceOfficeDocTypeForm
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces.Configuration;
using Intermech.Office.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class ChoiceOfficeDocTypeForm : Form
{
  private readonly OfficeDocumentTypes _saved;
  private readonly int _objectType;
  private IContainer components;
  private List<RadioButton> _radioButtons;
  private Button button1;

  public OfficeDocumentTypes OfficeDocumentType
  {
    get
    {
      foreach (RadioButton radioButton in this._radioButtons)
      {
        if (radioButton.Checked)
          return (OfficeDocumentTypes) radioButton.Tag;
      }
      return this._saved;
    }
  }

  public ChoiceOfficeDocTypeForm([NotNull] OfficeDocumentTypes[] enableTypes, int objectType)
  {
    this._objectType = objectType;
    if (Holder.ConfigurationManager != null)
    {
      IConfiguration configuration1 = Holder.ConfigurationManager.Open("FormStorage");
      if (configuration1 != null)
      {
        IConfiguration configuration2 = configuration1.Open($"{(object) this.GetType()}_{(object) objectType}");
        if (configuration2 != null && configuration2.HasProperty("selectedEnableType"))
          this._saved = (OfficeDocumentTypes) Convert.ToInt32(configuration2.GetProperty("selectedEnableType"));
      }
    }
    this.InitializeComponent(enableTypes, this._saved);
  }

  private void ChoiceOfficeDocTypeForm_FormClosed([CanBeNull] object sender, [NotNull] FormClosedEventArgs e)
  {
    if (Holder.ConfigurationManager == null)
      return;
    IConfiguration configuration = Holder.ConfigurationManager.Open("FormStorage") ?? Holder.ConfigurationManager.Create("FormStorage");
    string name = $"{(object) this.GetType()}_{(object) this._objectType}";
    (configuration.Open(name) ?? configuration.Add(name)).SetProperty("selectedEnableType", ((int) this.OfficeDocumentType).ToString());
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent(
    OfficeDocumentTypes[] enableTypes,
    OfficeDocumentTypes selectedType)
  {
    this.button1 = new Button();
    this.SuspendLayout();
    this._radioButtons = new List<RadioButton>(enableTypes.Length);
    bool flag = false;
    for (int index = 0; index < enableTypes.Length; ++index)
    {
      RadioButton radioButton = new RadioButton();
      radioButton.AutoSize = true;
      radioButton.Location = new Point(76, 30 + 23 * index);
      radioButton.Name = "radioButton" + index.ToString();
      radioButton.Size = new Size(90, 17);
      radioButton.TabIndex = index;
      radioButton.Text = EnumDescConverter.GetEnumDescription((Enum) enableTypes[index]);
      radioButton.Tag = (object) enableTypes[index];
      radioButton.UseVisualStyleBackColor = true;
      if (enableTypes[index] == selectedType)
      {
        radioButton.Checked = true;
        flag = true;
      }
      this._radioButtons.Add(radioButton);
      this.Controls.Add((Control) radioButton);
    }
    if (!flag)
      this._radioButtons[0].Checked = true;
    this.button1.DialogResult = DialogResult.OK;
    this.button1.Location = new Point(81, 124);
    this.button1.Name = "button1";
    this.button1.Size = new Size(75, 23);
    this.button1.TabIndex = 3;
    this.button1.Text = "ОК";
    this.button1.UseVisualStyleBackColor = true;
    this.AcceptButton = (IButtonControl) this.button1;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(243, 159);
    this.Controls.Add((Control) this.button1);
    this.StartPosition = FormStartPosition.CenterParent;
    this.FormClosed += new FormClosedEventHandler(this.ChoiceOfficeDocTypeForm_FormClosed);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.Name = nameof (ChoiceOfficeDocTypeForm);
    this.Text = "Выберите вид канцелярского документа";
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}

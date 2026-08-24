// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.CommonSettingsControl
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Client.Core;
using Intermech.ExternalSystemIntegration.Interfaces;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class CommonSettingsControl : UserControl, IPropertyPage
{
  private IContainer components;
  private ButtonedEdit tbErrorFiles;
  private ButtonedEdit tbDoneFiles;
  private ButtonedEdit tbInputFiles;
  private ButtonedEdit tbOutputFiles;

  public CommonSettingsControl()
  {
    this.InitializeComponent();
    this.ReadFromBase();
  }

  public event EventHandler Changed;

  public PropertyPageType Type => PropertyPageType.Control;

  public object Control => (object) this;

  public string PageName => Const.CommonSettingsName;

  public void Apply() => this.SaveToBase();

  private void SaveToBase()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (ICommonSettingsHolder)) is ICommonSettingsHolder customService))
        return;
      customService.InputFiles = this.tbInputFiles.Value;
      customService.OutputFiles = this.tbOutputFiles.Value;
      customService.DoneFiles = this.tbDoneFiles.Value;
      customService.ErrorFiles = this.tbErrorFiles.Value;
      customService.WriteSettings(sessionKeeper.Session.SessionGUID);
    }
  }

  public void Cancel() => this.ReadFromBase();

  private void ReadFromBase()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (ICommonSettingsHolder)) is ICommonSettingsHolder customService))
        return;
      this.tbInputFiles.Value = customService.InputFiles;
      this.tbOutputFiles.Value = customService.OutputFiles;
      this.tbDoneFiles.Value = customService.DoneFiles;
      this.tbErrorFiles.Value = customService.ErrorFiles;
    }
  }

  public string HelpTopicID => string.Empty;

  public string HeaderText => "Настройка директорий";

  private void SelectFolder(object sender, EventArgs e)
  {
    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
    if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
      return;
    ((ButtonedEdit) sender).Value = folderBrowserDialog.SelectedPath;
  }

  private void RaiseChangedEvent()
  {
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, new EventArgs());
  }

  private void tbEditTextChanged(object sender, EventArgs e) => this.RaiseChangedEvent();

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.tbErrorFiles = new ButtonedEdit();
    this.tbDoneFiles = new ButtonedEdit();
    this.tbInputFiles = new ButtonedEdit();
    this.tbOutputFiles = new ButtonedEdit();
    this.SuspendLayout();
    this.tbErrorFiles.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbErrorFiles.ButtonImage = (Image) null;
    this.tbErrorFiles.ButtonText = "...";
    this.tbErrorFiles.Caption = "Некорректные файлы:";
    this.tbErrorFiles.CaptionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.tbErrorFiles.Image = (Image) null;
    this.tbErrorFiles.Location = new Point(17, 146);
    this.tbErrorFiles.MinimumSize = new Size(40, 20);
    this.tbErrorFiles.Name = "tbErrorFiles";
    this.tbErrorFiles.Size = new Size(484, 38);
    this.tbErrorFiles.TabIndex = 68;
    this.tbErrorFiles.ButtonClick += new EventHandler(this.SelectFolder);
    this.tbErrorFiles.EditTextChanged += new EventHandler(this.tbEditTextChanged);
    this.tbDoneFiles.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbDoneFiles.ButtonImage = (Image) null;
    this.tbDoneFiles.ButtonText = "...";
    this.tbDoneFiles.Caption = "Обработанные файлы:";
    this.tbDoneFiles.CaptionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.tbDoneFiles.Image = (Image) null;
    this.tbDoneFiles.Location = new Point(17, 102);
    this.tbDoneFiles.MinimumSize = new Size(40, 20);
    this.tbDoneFiles.Name = "tbDoneFiles";
    this.tbDoneFiles.Size = new Size(484, 38);
    this.tbDoneFiles.TabIndex = 69;
    this.tbDoneFiles.ButtonClick += new EventHandler(this.SelectFolder);
    this.tbDoneFiles.EditTextChanged += new EventHandler(this.tbEditTextChanged);
    this.tbInputFiles.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbInputFiles.ButtonImage = (Image) null;
    this.tbInputFiles.ButtonText = "...";
    this.tbInputFiles.Caption = "Входящие файлы:";
    this.tbInputFiles.CaptionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.tbInputFiles.Image = (Image) null;
    this.tbInputFiles.Location = new Point(17, 58);
    this.tbInputFiles.MinimumSize = new Size(40, 20);
    this.tbInputFiles.Name = "tbInputFiles";
    this.tbInputFiles.Size = new Size(484, 38);
    this.tbInputFiles.TabIndex = 70;
    this.tbInputFiles.ButtonClick += new EventHandler(this.SelectFolder);
    this.tbInputFiles.EditTextChanged += new EventHandler(this.tbEditTextChanged);
    this.tbOutputFiles.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbOutputFiles.ButtonImage = (Image) null;
    this.tbOutputFiles.ButtonText = "...";
    this.tbOutputFiles.Caption = "Исходящие файлы:";
    this.tbOutputFiles.CaptionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.tbOutputFiles.Image = (Image) null;
    this.tbOutputFiles.Location = new Point(17, 14);
    this.tbOutputFiles.MinimumSize = new Size(40, 20);
    this.tbOutputFiles.Name = "tbOutputFiles";
    this.tbOutputFiles.Size = new Size(484, 38);
    this.tbOutputFiles.TabIndex = 71;
    this.tbOutputFiles.ButtonClick += new EventHandler(this.SelectFolder);
    this.tbOutputFiles.EditTextChanged += new EventHandler(this.tbEditTextChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((System.Windows.Forms.Control) this.tbErrorFiles);
    this.Controls.Add((System.Windows.Forms.Control) this.tbDoneFiles);
    this.Controls.Add((System.Windows.Forms.Control) this.tbInputFiles);
    this.Controls.Add((System.Windows.Forms.Control) this.tbOutputFiles);
    this.Name = nameof (CommonSettingsControl);
    this.Size = new Size(519, 351);
    this.ResumeLayout(false);
  }
}

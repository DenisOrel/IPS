// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.SuffixSettingsControl
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.Interfaces.Client;
using Intermech.Workflow.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.SearchData;

public class SuffixSettingsControl : StepControl
{
  private bool _loaded;
  private IContainer components;
  private GroupBox groupBox1;
  private AutoSizeLabel StatusLabel;
  private TextBox SuffixBox;
  private Label label2;
  private Button LoadSuffixesButton;

  public SuffixSettingsControl() => this.InitializeComponent();

  public override bool isMetadataSettingsStep => false;

  public void LoadSuffixesFromDB(bool forceLoad = false)
  {
    if (forceLoad || PluginSettings.ArtSuffixesToDelete == null)
    {
      PluginSettings.ArtSuffixesToDelete = new List<string>();
      using (IDataReader dataReader = BasePumpHelper.S4Query("select distinct dt_code from doctypes where dt_code <> '' and suffix=1 order by 1"))
      {
        while (dataReader.Read())
          PluginSettings.ArtSuffixesToDelete.Add(dataReader.GetString(0));
      }
    }
    this.SuffixBox.Lines = PluginSettings.ArtSuffixesToDelete.ToArray();
  }

  public override void RefreshControl()
  {
    base.RefreshControl();
    this.StatusLabel.StatusImage = (Image) SearchDataPlugin.WarningImage;
    if (this._loaded)
      return;
    this.LoadSuffixesFromDB();
    this._loaded = true;
  }

  public new SaveSettingsResult SaveSettings()
  {
    ISaveSettings service = ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings;
    Dictionary<string, SaveSettingsAttribute[]> settings = service.GetSettings("SEARCHDATA");
    List<SaveSettingsAttribute> settingsAttributeList = new List<SaveSettingsAttribute>();
    PluginSettings.ArtSuffixesToDelete.Clear();
    foreach (string line in this.SuffixBox.Lines)
    {
      string str = line.Trim();
      if (str != "")
      {
        string upper = str.ToUpper();
        if (!PluginSettings.ArtSuffixesToDelete.Contains(upper))
          PluginSettings.ArtSuffixesToDelete.Add(upper);
      }
    }
    settingsAttributeList.Add(new SaveSettingsAttribute("List", string.Join(",", PluginSettings.ArtSuffixesToDelete.ToArray())));
    settings["DelSuffixes"] = settingsAttributeList.ToArray();
    service.SetSettings("SEARCHDATA", settings);
    return base.SaveSettings();
  }

  public override bool LeaveControl()
  {
    base.LeaveControl();
    int num = (int) this.SaveSettings();
    return true;
  }

  protected override string getCaption() => "Настройка суффиксов";

  private void LoadSuffixesButton_Click(object sender, EventArgs e)
  {
    if (MessageBox.Show("Список возможных суффиксов документов будет загружен из БД Search, текущий список будет презаписан. Продолжить?", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) != DialogResult.Yes)
      return;
    this.LoadSuffixesFromDB(true);
  }

  private void SuffixBox_KeyPress(object sender, KeyPressEventArgs e)
  {
    if (Regex.IsMatch(e.KeyChar.ToString(), "^(\\w|[\\b]|[\\r]| |\\u001A)$"))
      return;
    e.Handled = true;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.groupBox1 = new GroupBox();
    this.SuffixBox = new TextBox();
    this.StatusLabel = new AutoSizeLabel();
    this.LoadSuffixesButton = new Button();
    this.label2 = new Label();
    this.groupBox1.SuspendLayout();
    this.SuspendLayout();
    this.groupBox1.Controls.Add((Control) this.SuffixBox);
    this.groupBox1.Controls.Add((Control) this.StatusLabel);
    this.groupBox1.Controls.Add((Control) this.LoadSuffixesButton);
    this.groupBox1.Controls.Add((Control) this.label2);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(10, 10);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Padding = new Padding(10, 10, 10, 2);
    this.groupBox1.Size = new Size(569, 460);
    this.groupBox1.TabIndex = 1;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Перекачка суффиксов";
    this.SuffixBox.BackColor = SystemColors.Window;
    this.SuffixBox.Dock = DockStyle.Fill;
    this.SuffixBox.Location = new Point(10, 51);
    this.SuffixBox.Multiline = true;
    this.SuffixBox.Name = "SuffixBox";
    this.SuffixBox.ScrollBars = ScrollBars.Vertical;
    this.SuffixBox.Size = new Size(549, 358);
    this.SuffixBox.TabIndex = 1;
    this.SuffixBox.KeyPress += new KeyPressEventHandler(this.SuffixBox_KeyPress);
    this.StatusLabel.Dock = DockStyle.Bottom;
    this.StatusLabel.ImageAlign = ContentAlignment.MiddleLeft;
    this.StatusLabel.Location = new Point(10, 409);
    this.StatusLabel.Name = "StatusLabel";
    this.StatusLabel.Padding = new Padding(0, 10, 0, 10);
    this.StatusLabel.Size = new Size(549, 49);
    this.StatusLabel.TabIndex = 3;
    this.StatusLabel.Text = "При наличи нескольких изделий, совпадающих по обозначению без учета суффиксов, изделия с суффиксами перекачиваться не будут.";
    this.LoadSuffixesButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.LoadSuffixesButton.Location = new Point(445, 22);
    this.LoadSuffixesButton.Name = "LoadSuffixesButton";
    this.LoadSuffixesButton.Size = new Size(114, 23);
    this.LoadSuffixesButton.TabIndex = 4;
    this.LoadSuffixesButton.Text = "Загрузить из БД";
    this.LoadSuffixesButton.UseVisualStyleBackColor = true;
    this.LoadSuffixesButton.Click += new EventHandler(this.LoadSuffixesButton_Click);
    this.label2.AutoSize = true;
    this.label2.Dock = DockStyle.Top;
    this.label2.Location = new Point(10, 23);
    this.label2.Name = "label2";
    this.label2.Padding = new Padding(0, 5, 0, 10);
    this.label2.Size = new Size(223, 28);
    this.label2.TabIndex = 1;
    this.label2.Text = "Удалять у изделий следующие суффиксы:";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.groupBox1);
    this.Name = nameof (SuffixSettingsControl);
    this.Padding = new Padding(10);
    this.Size = new Size(589, 480);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.ResumeLayout(false);
  }
}

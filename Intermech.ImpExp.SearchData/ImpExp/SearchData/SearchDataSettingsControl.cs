// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.SearchDataSettingsControl
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Workflow.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.SearchData;

public class SearchDataSettingsControl : StepControl
{
  private SearchDataPlugin _plugin;
  private List<ImStorageInfo> _paths;
  private int _pathColumnIndex;
  private bool _settingsLoaded;
  private Image _image;
  private IContainer components;
  private GroupBox groupBox1;
  private CheckBox AddArtIDCheckBox;
  private CheckBox AddDocIDCheckBox;
  private AutoSizeLabel HintLabel;
  private GroupBox ImdocsGroupBox;
  private DataGridView GridView;
  private DataGridViewTextBoxColumn Alias;
  private DataGridViewTextBoxColumn Path;
  private DataGridViewButtonColumn Button;
  private AutoSizeLabel label2;
  private GroupBox groupBox2;
  private GroupBox InvalidDataGroupBox;
  private ListView lvInvalidData;
  private Panel panel1;
  private Label label3;
  private ColumnHeader columnHeader1;
  private ColumnHeader columnHeader2;
  private ColumnHeader columnHeader3;
  private ColumnHeader columnHeader4;
  private GroupBox groupBox3;
  private CheckBox PumpArtVersionsCheckBox;
  private CheckBox PumpSysArtVersionsCheckBox;
  private CheckBox OptimizeReadTPСheckBox;

  public SearchDataSettingsControl() => this.InitializeComponent();

  public override bool isMetadataSettingsStep => false;

  public SearchDataSettingsControl(SearchDataPlugin plugin)
    : this()
  {
    this._plugin = plugin;
  }

  public override void RefreshControl()
  {
    base.RefreshControl();
    if (this._plugin == null || this._paths != null)
      return;
    this.LoadSettings();
    this._paths = new List<ImStorageInfo>();
    foreach (KeyValuePair<string, AliasInfo> keyValuePair in this._plugin.AliasInfo)
    {
      if (keyValuePair.Value[AliasData.Type].ToUpper() == "IMDOCS")
        this._paths.Add(new ImStorageInfo(keyValuePair.Key, $"{keyValuePair.Value[AliasData.DBString]}.{keyValuePair.Value[AliasData.Alias]}", keyValuePair.Value[AliasData.FilePath]));
    }
    this.GridView.DataSource = (object) this._paths;
    this._pathColumnIndex = this.GridView.Columns["Path"].Index;
    this.ImdocsGroupBox.Visible = this._paths.Count != 0;
    this.InvalidDataGroupBox.Visible = this.lvInvalidData.Items.Count > 0;
    this.HintLabel.StatusImage = (Image) SearchDataPlugin.InfoImage;
  }

  private void addInvalidObject(InvalidObject obj)
  {
    this.lvInvalidData.BeginUpdate();
    ListViewItem listViewItem = new ListViewItem(obj.Type);
    listViewItem.SubItems.Add(obj.ID.ToString());
    string text = obj.VerID == -1 ? "-" : obj.VerID.ToString();
    listViewItem.SubItems.Add(text);
    listViewItem.SubItems.Add(obj.InvalidObjectType.ToString(), Color.Red, this.lvInvalidData.BackColor, this.lvInvalidData.Font);
    this.lvInvalidData.Items.Add(listViewItem);
    this.lvInvalidData.EndUpdate();
  }

  internal void AddInvalidObject(InvalidObject obj)
  {
    this.Invoke((Delegate) new SearchDataSettingsControl.AddInvalidObjectDelegate(this.addInvalidObject), (object) obj);
  }

  private void LoadSettings()
  {
    if (this._settingsLoaded)
      return;
    this._settingsLoaded = true;
    this.AddDocIDCheckBox.Checked = PluginSettings.AddDocID;
    this.AddArtIDCheckBox.Checked = PluginSettings.AddArtID;
    this.OptimizeReadTPСheckBox.Checked = PluginSettings.OptimizeReadTParams;
    this.PumpArtVersionsCheckBox.Checked = PluginSettings.PumpArtVersions;
    this.PumpSysArtVersionsCheckBox.Checked = PluginSettings.PumpArtVersions && PluginSettings.PumpSysArtVersions;
    this.PumpArtVersionsCheckBox_CheckedChanged((object) null, (EventArgs) null);
  }

  private void GridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
  {
    if (!(sender is DataGridView) || !(((DataGridView) sender).Columns[e.ColumnIndex] is DataGridViewButtonColumn))
      return;
    using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
    {
      folderBrowserDialog.SelectedPath = this.GridView.Rows[e.RowIndex].Cells[this._pathColumnIndex].ToString();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.GridView.Rows[e.RowIndex].Cells[this._pathColumnIndex].Value = (object) folderBrowserDialog.SelectedPath;
    }
  }

  public new SaveSettingsResult SaveSettings()
  {
    ISaveSettings service = ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings;
    Dictionary<string, SaveSettingsAttribute[]> settings = new Dictionary<string, SaveSettingsAttribute[]>();
    List<SaveSettingsAttribute> settingsAttributeList = new List<SaveSettingsAttribute>();
    PluginSettings.AddDocID = this.AddDocIDCheckBox.Checked;
    PluginSettings.AddArtID = this.AddArtIDCheckBox.Checked;
    PluginSettings.PumpArtVersions = this.PumpArtVersionsCheckBox.Checked;
    PluginSettings.PumpSysArtVersions = this.PumpSysArtVersionsCheckBox.Checked;
    PluginSettings.OptimizeReadTParams = this.OptimizeReadTPСheckBox.Checked;
    settingsAttributeList.Add(new SaveSettingsAttribute("AddDocID", Convert.ToInt32(this.AddDocIDCheckBox.Checked).ToString()));
    settingsAttributeList.Add(new SaveSettingsAttribute("AddArtID", Convert.ToInt32(this.AddArtIDCheckBox.Checked).ToString()));
    settingsAttributeList.Add(new SaveSettingsAttribute("PumpArtVersions", Convert.ToInt32(PluginSettings.PumpArtVersions).ToString()));
    settingsAttributeList.Add(new SaveSettingsAttribute("PumpSysArtVersions", Convert.ToInt32(PluginSettings.PumpSysArtVersions).ToString()));
    settingsAttributeList.Add(new SaveSettingsAttribute("OptimizeReadTParams", Convert.ToInt32(PluginSettings.OptimizeReadTParams).ToString()));
    settings.Add("Common", settingsAttributeList.ToArray());
    settingsAttributeList.Clear();
    foreach (ImStorageInfo path in this._paths)
    {
      path.Path = Directory.Exists(path.Path) ? path.Path.Trim() : throw new Exception($"Директория для шкафа {path.Alias} не задана!");
      if (!path.Path.EndsWith("\\"))
        path.Path += "\\";
      this._plugin.AliasInfo[path.RawAlias][AliasData.FilePath] = path.Path;
      settingsAttributeList.Add(new SaveSettingsAttribute(path.RawAlias, path.Path));
    }
    settings.Add("ImStores", settingsAttributeList.ToArray());
    service.SetSettings("SEARCHDATA", settings);
    return base.SaveSettings();
  }

  public override bool LeaveControl()
  {
    base.LeaveControl();
    int num = (int) this.SaveSettings();
    return true;
  }

  protected override string getCaption() => "Настройка перекачки данных Search";

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
      this._image = service.ImageList.Images[service.ImageIndex("imgSearchData")];
    return this._image;
  }

  private void PumpArtVersionsCheckBox_CheckedChanged(object sender, EventArgs e)
  {
    this.PumpSysArtVersionsCheckBox.Enabled = this.PumpArtVersionsCheckBox.Checked;
    if (this.PumpSysArtVersionsCheckBox.Enabled)
      return;
    this.PumpSysArtVersionsCheckBox.Checked = false;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SearchDataSettingsControl));
    this.groupBox1 = new GroupBox();
    this.AddArtIDCheckBox = new CheckBox();
    this.AddDocIDCheckBox = new CheckBox();
    this.HintLabel = new AutoSizeLabel();
    this.ImdocsGroupBox = new GroupBox();
    this.GridView = new DataGridView();
    this.Alias = new DataGridViewTextBoxColumn();
    this.Path = new DataGridViewTextBoxColumn();
    this.Button = new DataGridViewButtonColumn();
    this.label2 = new AutoSizeLabel();
    this.groupBox2 = new GroupBox();
    this.InvalidDataGroupBox = new GroupBox();
    this.lvInvalidData = new ListView();
    this.columnHeader1 = new ColumnHeader();
    this.columnHeader2 = new ColumnHeader();
    this.columnHeader4 = new ColumnHeader();
    this.columnHeader3 = new ColumnHeader();
    this.panel1 = new Panel();
    this.label3 = new Label();
    this.groupBox3 = new GroupBox();
    this.OptimizeReadTPСheckBox = new CheckBox();
    this.PumpSysArtVersionsCheckBox = new CheckBox();
    this.PumpArtVersionsCheckBox = new CheckBox();
    this.groupBox1.SuspendLayout();
    this.ImdocsGroupBox.SuspendLayout();
    ((ISupportInitialize) this.GridView).BeginInit();
    this.groupBox2.SuspendLayout();
    this.InvalidDataGroupBox.SuspendLayout();
    this.panel1.SuspendLayout();
    this.groupBox3.SuspendLayout();
    this.SuspendLayout();
    this.groupBox1.AutoSize = true;
    this.groupBox1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.groupBox1.Controls.Add((Control) this.AddArtIDCheckBox);
    this.groupBox1.Controls.Add((Control) this.AddDocIDCheckBox);
    this.groupBox1.Controls.Add((Control) this.HintLabel);
    this.groupBox1.Dock = DockStyle.Top;
    this.groupBox1.Location = new Point(10, 102);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Padding = new Padding(10);
    this.groupBox1.Size = new Size(659, 118);
    this.groupBox1.TabIndex = 3;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Идентификация";
    this.AddArtIDCheckBox.AutoSize = true;
    this.AddArtIDCheckBox.Dock = DockStyle.Top;
    this.AddArtIDCheckBox.Location = new Point(10, 40);
    this.AddArtIDCheckBox.Name = "AddArtIDCheckBox";
    this.AddArtIDCheckBox.Size = new Size(639, 17);
    this.AddArtIDCheckBox.TabIndex = 1;
    this.AddArtIDCheckBox.Text = "Записывать старый идентификатор изделия для перекачиваемых изделий";
    this.AddArtIDCheckBox.UseVisualStyleBackColor = true;
    this.AddDocIDCheckBox.AutoSize = true;
    this.AddDocIDCheckBox.Dock = DockStyle.Top;
    this.AddDocIDCheckBox.Location = new Point(10, 23);
    this.AddDocIDCheckBox.Name = "AddDocIDCheckBox";
    this.AddDocIDCheckBox.Size = new Size(639, 17);
    this.AddDocIDCheckBox.TabIndex = 0;
    this.AddDocIDCheckBox.Text = "Записывать старый идентификатор документа для перекачиваемых документов";
    this.AddDocIDCheckBox.UseVisualStyleBackColor = true;
    this.HintLabel.BackColor = SystemColors.Control;
    this.HintLabel.Dock = DockStyle.Bottom;
    this.HintLabel.Location = new Point(10, 57);
    this.HintLabel.Name = "HintLabel";
    this.HintLabel.Padding = new Padding(0, 10, 0, 0);
    this.HintLabel.Size = new Size(639, 51);
    this.HintLabel.TabIndex = 2;
    this.HintLabel.Text = componentResourceManager.GetString("HintLabel.Text");
    this.ImdocsGroupBox.Controls.Add((Control) this.GridView);
    this.ImdocsGroupBox.Controls.Add((Control) this.label2);
    this.ImdocsGroupBox.Dock = DockStyle.Top;
    this.ImdocsGroupBox.Location = new Point(10, 220);
    this.ImdocsGroupBox.Name = "ImdocsGroupBox";
    this.ImdocsGroupBox.Padding = new Padding(10);
    this.ImdocsGroupBox.Size = new Size(659, 189);
    this.ImdocsGroupBox.TabIndex = 4;
    this.ImdocsGroupBox.TabStop = false;
    this.ImdocsGroupBox.Text = "Службы документов Intermech";
    this.GridView.AllowUserToAddRows = false;
    this.GridView.AllowUserToDeleteRows = false;
    this.GridView.AllowUserToResizeRows = false;
    this.GridView.BackgroundColor = SystemColors.Control;
    this.GridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    this.GridView.Columns.AddRange((DataGridViewColumn) this.Alias, (DataGridViewColumn) this.Path, (DataGridViewColumn) this.Button);
    this.GridView.Dock = DockStyle.Fill;
    this.GridView.EditMode = DataGridViewEditMode.EditOnEnter;
    this.GridView.Location = new Point(10, 74);
    this.GridView.Name = "GridView";
    this.GridView.RowHeadersVisible = false;
    this.GridView.Size = new Size(639, 105);
    this.GridView.TabIndex = 5;
    this.GridView.CellContentClick += new DataGridViewCellEventHandler(this.GridView_CellContentClick);
    this.Alias.DataPropertyName = "Alias";
    this.Alias.HeaderText = "Псевдоним";
    this.Alias.Name = "Alias";
    this.Alias.ReadOnly = true;
    this.Alias.Width = 200;
    this.Path.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    this.Path.DataPropertyName = "Path";
    this.Path.HeaderText = "Путь к папке";
    this.Path.Name = "Path";
    this.Path.Resizable = DataGridViewTriState.True;
    this.Path.SortMode = DataGridViewColumnSortMode.NotSortable;
    this.Button.HeaderText = "";
    this.Button.MinimumWidth = 20;
    this.Button.Name = "Button";
    this.Button.Resizable = DataGridViewTriState.False;
    this.Button.SortMode = DataGridViewColumnSortMode.Automatic;
    this.Button.Text = "...";
    this.Button.UseColumnTextForButtonValue = true;
    this.Button.Width = 20;
    this.label2.BackColor = SystemColors.Control;
    this.label2.Dock = DockStyle.Top;
    this.label2.Location = new Point(10, 23);
    this.label2.Name = "label2";
    this.label2.Padding = new Padding(0, 0, 0, 10);
    this.label2.Size = new Size(639, 51);
    this.label2.TabIndex = 3;
    this.label2.Text = componentResourceManager.GetString("label2.Text");
    this.groupBox2.Controls.Add((Control) this.InvalidDataGroupBox);
    this.groupBox2.Controls.Add((Control) this.ImdocsGroupBox);
    this.groupBox2.Controls.Add((Control) this.groupBox1);
    this.groupBox2.Controls.Add((Control) this.groupBox3);
    this.groupBox2.Dock = DockStyle.Fill;
    this.groupBox2.Location = new Point(10, 10);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Padding = new Padding(10);
    this.groupBox2.Size = new Size(679, 583);
    this.groupBox2.TabIndex = 3;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Настройки перекачки данных Search";
    this.InvalidDataGroupBox.Controls.Add((Control) this.lvInvalidData);
    this.InvalidDataGroupBox.Controls.Add((Control) this.panel1);
    this.InvalidDataGroupBox.Dock = DockStyle.Fill;
    this.InvalidDataGroupBox.Location = new Point(10, 409);
    this.InvalidDataGroupBox.Name = "InvalidDataGroupBox";
    this.InvalidDataGroupBox.Size = new Size(659, 164);
    this.InvalidDataGroupBox.TabIndex = 5;
    this.InvalidDataGroupBox.TabStop = false;
    this.InvalidDataGroupBox.Text = "Нарушена целостность данных";
    this.lvInvalidData.Columns.AddRange(new ColumnHeader[4]
    {
      this.columnHeader1,
      this.columnHeader2,
      this.columnHeader4,
      this.columnHeader3
    });
    this.lvInvalidData.Dock = DockStyle.Fill;
    this.lvInvalidData.FullRowSelect = true;
    this.lvInvalidData.HideSelection = false;
    this.lvInvalidData.Location = new Point(3, 39);
    this.lvInvalidData.Name = "lvInvalidData";
    this.lvInvalidData.Size = new Size(653, 122);
    this.lvInvalidData.TabIndex = 1;
    this.lvInvalidData.UseCompatibleStateImageBehavior = false;
    this.lvInvalidData.View = View.Details;
    this.columnHeader1.Text = "Объект";
    this.columnHeader1.Width = 229;
    this.columnHeader2.Text = "Идентификатор";
    this.columnHeader2.Width = 130;
    this.columnHeader4.Text = "Версия";
    this.columnHeader4.Width = 130;
    this.columnHeader3.Text = "Тип объекта";
    this.columnHeader3.Width = 130;
    this.panel1.Controls.Add((Control) this.label3);
    this.panel1.Dock = DockStyle.Top;
    this.panel1.Location = new Point(3, 16 /*0x10*/);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(653, 23);
    this.panel1.TabIndex = 0;
    this.label3.AutoSize = true;
    this.label3.Dock = DockStyle.Fill;
    this.label3.Location = new Point(0, 0);
    this.label3.Name = "label3";
    this.label3.Size = new Size(533, 13);
    this.label3.TabIndex = 0;
    this.label3.Text = "В исходной базе данных обнаружены объекты с несуществующими типами, которые будут пропущены";
    this.groupBox3.Controls.Add((Control) this.OptimizeReadTPСheckBox);
    this.groupBox3.Controls.Add((Control) this.PumpSysArtVersionsCheckBox);
    this.groupBox3.Controls.Add((Control) this.PumpArtVersionsCheckBox);
    this.groupBox3.Dock = DockStyle.Top;
    this.groupBox3.Location = new Point(10, 23);
    this.groupBox3.Name = "groupBox3";
    this.groupBox3.Size = new Size(659, 79);
    this.groupBox3.TabIndex = 6;
    this.groupBox3.TabStop = false;
    this.groupBox3.Text = "Настройки";
    this.OptimizeReadTPСheckBox.AutoSize = true;
    this.OptimizeReadTPСheckBox.Location = new Point(10, 57);
    this.OptimizeReadTPСheckBox.Name = "OptimizeReadTPСheckBox";
    this.OptimizeReadTPСheckBox.Size = new Size(286, 17);
    this.OptimizeReadTPСheckBox.TabIndex = 2;
    this.OptimizeReadTPСheckBox.Text = "Оптимизировать чтение тематических параметров";
    this.OptimizeReadTPСheckBox.UseVisualStyleBackColor = true;
    this.PumpSysArtVersionsCheckBox.AutoSize = true;
    this.PumpSysArtVersionsCheckBox.Location = new Point(28, 38);
    this.PumpSysArtVersionsCheckBox.Name = "PumpSysArtVersionsCheckBox";
    this.PumpSysArtVersionsCheckBox.Size = new Size(296, 17);
    this.PumpSysArtVersionsCheckBox.TabIndex = 1;
    this.PumpSysArtVersionsCheckBox.Text = "Перекачивать версии изделий, созданные системой";
    this.PumpSysArtVersionsCheckBox.UseVisualStyleBackColor = true;
    this.PumpArtVersionsCheckBox.AutoSize = true;
    this.PumpArtVersionsCheckBox.Location = new Point(10, 19);
    this.PumpArtVersionsCheckBox.Name = "PumpArtVersionsCheckBox";
    this.PumpArtVersionsCheckBox.Size = new Size(182, 17);
    this.PumpArtVersionsCheckBox.TabIndex = 0;
    this.PumpArtVersionsCheckBox.Text = "Перекачивать версии изделий";
    this.PumpArtVersionsCheckBox.UseVisualStyleBackColor = true;
    this.PumpArtVersionsCheckBox.CheckedChanged += new EventHandler(this.PumpArtVersionsCheckBox_CheckedChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.groupBox2);
    this.Name = nameof (SearchDataSettingsControl);
    this.Padding = new Padding(10);
    this.Size = new Size(699, 603);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.ImdocsGroupBox.ResumeLayout(false);
    ((ISupportInitialize) this.GridView).EndInit();
    this.groupBox2.ResumeLayout(false);
    this.groupBox2.PerformLayout();
    this.InvalidDataGroupBox.ResumeLayout(false);
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    this.groupBox3.ResumeLayout(false);
    this.groupBox3.PerformLayout();
    this.ResumeLayout(false);
  }

  private delegate void AddInvalidObjectDelegate(InvalidObject obj);
}

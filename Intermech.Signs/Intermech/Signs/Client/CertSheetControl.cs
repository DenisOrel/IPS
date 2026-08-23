// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetControl
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Checksums;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>
/// Контрол для указания опций формирования УЛ (как для виртуального, так при сохранении на диск)
/// </summary>
public class CertSheetControl : UserControl
{
  private bool saveToDiskInterfaceFlag;
  /// <summary>алгоритм по умолчанию</summary>
  private ChecksumAlgorithm checksumAlgorithm;
  /// <summary>разрешен выбор альтернатив</summary>
  private bool enableChecksumAlternatives;
  /// <summary>список граф для подписей в системе</summary>
  private List<object[]> graphList;
  /// <summary>список расширений файлов документов</summary>
  private List<long> objectIDList;
  private bool blockChecked;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private GroupBox gbOptions;
  private CheckBox cbSaveToStandaloneFolder;
  private CheckBox cbExpandComposition;
  private CheckBox cbExpandECO;
  private ComboBox cbChecksumAlgorithm;
  private Label label1;
  private RadioButton rbAuth;
  private RadioButton rbNormal;
  private ListView lvGraphs;
  private ListView lvFileExts;
  private SplitContainer splitContainer;
  private CheckBox cbEnabled;
  private TableLayoutPanel tableLayoutPanel;
  private Panel panel1;
  private CheckBox cbGraphs;
  private CheckBox cbFileExts;
  private SplitContainer splitContainerGraphs;
  private ListView lvEmptyGraphs;
  private CheckBox cbEmptyGraphs;

  /// <summary>Режим сохранения на диск</summary>
  public bool SaveToDiskInterfaceFlag
  {
    get => this.saveToDiskInterfaceFlag;
    set
    {
      int num = this.saveToDiskInterfaceFlag ? 1 : 0;
      this.saveToDiskInterfaceFlag = value;
      if (this.DesignMode)
        return;
      this.cbEnabled.Enabled = this.saveToDiskInterfaceFlag;
      this.cbEnabled.Checked = !this.saveToDiskInterfaceFlag;
      this.ProcessControlStates();
    }
  }

  public List<long> ObjectIDList => this.objectIDList;

  public CertSheetControl() => this.InitializeComponent();

  /// <summary>
  /// инициализация, для сохранения на диск инициализация списка объектов значения не имеет - там все равно идет пообъектное сохранение
  /// </summary>
  /// <param name="objectIDlist"></param>
  public void InitControl(List<long> _objectIDlist) => this.objectIDList = _objectIDlist;

  /// <summary>Получить из ISelectedItems список id объектов</summary>
  /// <param name="items"></param>
  /// <returns></returns>
  public static List<long> InitItemsID(ISelectedItems items)
  {
    List<long> longList = new List<long>();
    for (int index = 0; index < items.Count; ++index)
      longList.Add((items.GetItemID(index) as NodeID).ObjectID);
    return longList;
  }

  private void CertSheetControl_Load(object sender, EventArgs e) => this.FillControl();

  /// <summary>заполнение (+ по items)</summary>
  public void FillControl()
  {
    if (this.DesignMode)
      return;
    this.cbExpandECO.Visible = !this.SaveToDiskInterfaceFlag;
    this.cbExpandComposition.Visible = !this.SaveToDiskInterfaceFlag;
    this.cbSaveToStandaloneFolder.Visible = this.SaveToDiskInterfaceFlag;
    IDBConfigurations service = ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations;
    this.checksumAlgorithm = (ChecksumAlgorithm) service.ReadInteger("CLIENT", "AUTHFILES", "ALGORITHM", 0L, DBConfigMode.GlobalOnly);
    this.enableChecksumAlternatives = service.ReadBool("CLIENT", "AUTHFILES", "ENABLEALTERNATIVES", true, DBConfigMode.GlobalOnly);
    this.cbChecksumAlgorithm.Enabled = this.enableChecksumAlternatives;
    this.LoadCrcAlgorithms();
    this.SetAlgorithmType(this.checksumAlgorithm);
    this.FillGrapList();
    this.FillExtList();
    this.ProcessControlStates();
  }

  private void ProcessControlStates()
  {
    this.gbOptions.Enabled = this.cbEnabled.Checked;
    this.splitContainer.Enabled = this.cbEnabled.Checked;
  }

  /// <summary>Заполнить список граф в lvGraphs</summary>
  private void FillGrapList()
  {
    if (this.graphList == null)
      this.graphList = CertSheetProcessor.GetGraphs();
    this.lvGraphs.Items.Clear();
    this.lvEmptyGraphs.Items.Clear();
    for (int index = 0; index < this.graphList.Count; ++index)
    {
      object[] graph = this.graphList[index];
      this.lvGraphs.Items.Add((string) graph[0], (string) graph[1], -1).Tag = (object) graph;
      this.lvEmptyGraphs.Items.Add((string) graph[0], (string) graph[1], -1).Tag = (object) graph;
    }
    this.cbGraphs.Checked = true;
  }

  /// <summary>Заполнить список расширений</summary>
  private void FillExtList()
  {
    List<string> stringList = (List<string>) null;
    if (this.rbAuth.Checked)
      stringList = CertSheetProcessor.GetPossibleExtensions4AuthFiles();
    if (this.rbNormal.Checked)
      stringList = CertSheetProcessor.GetExtensions();
    this.lvFileExts.Items.Clear();
    if (stringList == null)
      return;
    for (int index = 0; index < stringList.Count; ++index)
      this.lvFileExts.Items.Add(stringList[index]);
  }

  private void LoadCrcAlgorithms()
  {
    this.cbChecksumAlgorithm.Items.Clear();
    foreach (ChecksumAlgorithm checksumAlgorithm in Enum.GetValues(typeof (ChecksumAlgorithm)))
      this.cbChecksumAlgorithm.Items.Add((object) new ChecksumAlgorithmPropertyClass(checksumAlgorithm));
  }

  private void SetAlgorithmType(ChecksumAlgorithm alg)
  {
    for (int index = 0; index < this.cbChecksumAlgorithm.Items.Count; ++index)
    {
      if (((ChecksumAlgorithmPropertyClass) this.cbChecksumAlgorithm.Items[index]).ChecksumAlgorithm == alg)
      {
        this.cbChecksumAlgorithm.SelectedIndex = index;
        break;
      }
    }
  }

  private void rbNormal_CheckedChanged(object sender, EventArgs e)
  {
    this.FillExtList();
    this.ProcessControlStates();
  }

  /// <summary>заполнить класс с опциями текущими значениями</summary>
  /// <param name="certSheetOptions"></param>
  public CertSheetOptions GetCertSheetOptions()
  {
    CertSheetOptions certSheetOptions = new CertSheetOptions();
    certSheetOptions.ProcessCertSheets = this.cbEnabled.Checked;
    if (certSheetOptions.ProcessCertSheets)
    {
      if (this.objectIDList != null)
        certSheetOptions.ObjectIDList.AddRange((IEnumerable<long>) this.objectIDList.ToArray());
      certSheetOptions.ChecksumAlgorithm = ((ChecksumAlgorithmPropertyClass) this.cbChecksumAlgorithm.SelectedItem).ChecksumAlgorithm;
      certSheetOptions.NormalFilesMode = this.rbNormal.Checked;
      certSheetOptions.AuthFilesMode = this.rbAuth.Checked;
      certSheetOptions.ExpandECO = this.cbExpandECO.Checked;
      certSheetOptions.ExpandComposition = this.cbExpandComposition.Checked;
      certSheetOptions.SaveToStandaloneFolder = this.cbSaveToStandaloneFolder.Checked;
      for (int index = 0; index < this.lvGraphs.CheckedItems.Count; ++index)
        certSheetOptions.Graphs.Add((object[]) this.lvGraphs.CheckedItems[index].Tag);
      for (int index = 0; index < this.lvEmptyGraphs.CheckedItems.Count; ++index)
        certSheetOptions.EmptyGraphs.Add((object[]) this.lvEmptyGraphs.CheckedItems[index].Tag);
      for (int index = 0; index < this.lvFileExts.CheckedItems.Count; ++index)
        certSheetOptions.Extensions.Add(this.lvFileExts.CheckedItems[index].Text);
    }
    return certSheetOptions;
  }

  private void cbEnabled_CheckedChanged(object sender, EventArgs e) => this.ProcessControlStates();

  private void cbGraphs_CheckedChanged(object sender, EventArgs e)
  {
    this.cbCheckedChanged(sender, this.lvGraphs);
  }

  private void cbEmptyGraphs_CheckedChanged(object sender, EventArgs e)
  {
    this.cbCheckedChanged(sender, this.lvEmptyGraphs);
  }

  private void cbCheckedChanged(object sender, ListView lv)
  {
    if (this.blockChecked)
      return;
    this.blockChecked = true;
    try
    {
      for (int index = 0; index < lv.Items.Count; ++index)
        lv.Items[index].Checked = (sender as CheckBox).Checked;
    }
    finally
    {
      this.blockChecked = false;
    }
  }

  private void cbFileExts_CheckedChanged(object sender, EventArgs e)
  {
    if (this.blockChecked)
      return;
    this.blockChecked = true;
    try
    {
      for (int index = 0; index < this.lvFileExts.Items.Count; ++index)
        this.lvFileExts.Items[index].Checked = this.cbFileExts.Checked;
    }
    finally
    {
      this.blockChecked = false;
    }
  }

  private void lvGraphs_ItemChecked(object sender, ItemCheckedEventArgs e)
  {
    this.lvItemChecked(sender, e, this.cbGraphs);
  }

  private void lvEmptyGraphs_ItemChecked(object sender, ItemCheckedEventArgs e)
  {
    this.lvItemChecked(sender, e, this.cbEmptyGraphs);
  }

  private void lvItemChecked(object sender, ItemCheckedEventArgs e, CheckBox cb)
  {
    if (this.blockChecked || e.Item.Checked || !cb.Checked)
      return;
    this.blockChecked = true;
    try
    {
      cb.Checked = false;
    }
    finally
    {
      this.blockChecked = false;
    }
  }

  private void lvFileExts_ItemChecked(object sender, ItemCheckedEventArgs e)
  {
    if (this.blockChecked || e.Item.Checked || !this.cbFileExts.Checked)
      return;
    this.blockChecked = true;
    try
    {
      this.cbFileExts.Checked = false;
    }
    finally
    {
      this.blockChecked = false;
    }
  }

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.gbOptions = new GroupBox();
    this.cbSaveToStandaloneFolder = new CheckBox();
    this.cbExpandComposition = new CheckBox();
    this.cbExpandECO = new CheckBox();
    this.cbChecksumAlgorithm = new ComboBox();
    this.label1 = new Label();
    this.rbAuth = new RadioButton();
    this.rbNormal = new RadioButton();
    this.lvGraphs = new ListView();
    this.lvFileExts = new ListView();
    this.splitContainer = new SplitContainer();
    this.splitContainerGraphs = new SplitContainer();
    this.cbGraphs = new CheckBox();
    this.lvEmptyGraphs = new ListView();
    this.cbEmptyGraphs = new CheckBox();
    this.cbFileExts = new CheckBox();
    this.cbEnabled = new CheckBox();
    this.tableLayoutPanel = new TableLayoutPanel();
    this.panel1 = new Panel();
    this.gbOptions.SuspendLayout();
    this.splitContainer.BeginInit();
    this.splitContainer.Panel1.SuspendLayout();
    this.splitContainer.Panel2.SuspendLayout();
    this.splitContainer.SuspendLayout();
    this.splitContainerGraphs.BeginInit();
    this.splitContainerGraphs.Panel1.SuspendLayout();
    this.splitContainerGraphs.Panel2.SuspendLayout();
    this.splitContainerGraphs.SuspendLayout();
    this.tableLayoutPanel.SuspendLayout();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    this.gbOptions.Controls.Add((Control) this.cbSaveToStandaloneFolder);
    this.gbOptions.Controls.Add((Control) this.cbExpandComposition);
    this.gbOptions.Controls.Add((Control) this.cbExpandECO);
    this.gbOptions.Controls.Add((Control) this.cbChecksumAlgorithm);
    this.gbOptions.Controls.Add((Control) this.label1);
    this.gbOptions.Controls.Add((Control) this.rbAuth);
    this.gbOptions.Controls.Add((Control) this.rbNormal);
    this.gbOptions.Dock = DockStyle.Fill;
    this.gbOptions.Location = new Point(3, 28);
    this.gbOptions.Name = "gbOptions";
    this.gbOptions.Size = new Size(426, 108);
    this.gbOptions.TabIndex = 0;
    this.gbOptions.TabStop = false;
    this.cbSaveToStandaloneFolder.AutoSize = true;
    this.cbSaveToStandaloneFolder.Location = new Point(218, 82);
    this.cbSaveToStandaloneFolder.Name = "cbSaveToStandaloneFolder";
    this.cbSaveToStandaloneFolder.Size = new Size(177, 17);
    this.cbSaveToStandaloneFolder.TabIndex = 6;
    this.cbSaveToStandaloneFolder.Text = "Сохранять в отдельную папку";
    this.cbSaveToStandaloneFolder.UseVisualStyleBackColor = true;
    this.cbExpandComposition.AutoSize = true;
    this.cbExpandComposition.Location = new Point(218, 43);
    this.cbExpandComposition.Name = "cbExpandComposition";
    this.cbExpandComposition.Size = new Size(126, 17);
    this.cbExpandComposition.TabIndex = 5;
    this.cbExpandComposition.Text = "Раскрывать состав";
    this.cbExpandComposition.UseVisualStyleBackColor = true;
    this.cbExpandECO.AutoSize = true;
    this.cbExpandECO.Location = new Point(218, 19);
    this.cbExpandECO.Name = "cbExpandECO";
    this.cbExpandECO.Size = new Size(148, 17);
    this.cbExpandECO.TabIndex = 4;
    this.cbExpandECO.Text = "Раскрывать извещения";
    this.cbExpandECO.UseVisualStyleBackColor = true;
    this.cbChecksumAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbChecksumAlgorithm.FormattingEnabled = true;
    this.cbChecksumAlgorithm.Location = new Point(11, 80 /*0x50*/);
    this.cbChecksumAlgorithm.Name = "cbChecksumAlgorithm";
    this.cbChecksumAlgorithm.Size = new Size(121, 21);
    this.cbChecksumAlgorithm.TabIndex = 3;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(10, 64 /*0x40*/);
    this.label1.Name = "label1";
    this.label1.Size = new Size(109, 13);
    this.label1.TabIndex = 2;
    this.label1.Text = "Контрольная сумма";
    this.rbAuth.AutoSize = true;
    this.rbAuth.Location = new Point(11, 42);
    this.rbAuth.Name = "rbAuth";
    this.rbAuth.Size = new Size(172, 17);
    this.rbAuth.TabIndex = 1;
    this.rbAuth.Text = "для аутентичных документов";
    this.rbAuth.UseVisualStyleBackColor = true;
    this.rbNormal.AutoSize = true;
    this.rbNormal.Checked = true;
    this.rbNormal.Location = new Point(11, 18);
    this.rbNormal.Name = "rbNormal";
    this.rbNormal.Size = new Size(158, 17);
    this.rbNormal.TabIndex = 0;
    this.rbNormal.TabStop = true;
    this.rbNormal.Text = "для основных документов";
    this.rbNormal.UseVisualStyleBackColor = true;
    this.rbNormal.CheckedChanged += new EventHandler(this.rbNormal_CheckedChanged);
    this.lvGraphs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.lvGraphs.CheckBoxes = true;
    this.lvGraphs.Location = new Point(0, 33);
    this.lvGraphs.Name = "lvGraphs";
    this.lvGraphs.Size = new Size(210, 153);
    this.lvGraphs.Sorting = SortOrder.Ascending;
    this.lvGraphs.TabIndex = 0;
    this.lvGraphs.UseCompatibleStateImageBehavior = false;
    this.lvGraphs.View = View.List;
    this.lvGraphs.ItemChecked += new ItemCheckedEventHandler(this.lvGraphs_ItemChecked);
    this.lvFileExts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.lvFileExts.CheckBoxes = true;
    this.lvFileExts.Location = new Point(3, 33);
    this.lvFileExts.Name = "lvFileExts";
    this.lvFileExts.Size = new Size(420, 155);
    this.lvFileExts.Sorting = SortOrder.Ascending;
    this.lvFileExts.TabIndex = 0;
    this.lvFileExts.UseCompatibleStateImageBehavior = false;
    this.lvFileExts.View = View.List;
    this.lvFileExts.ItemChecked += new ItemCheckedEventHandler(this.lvFileExts_ItemChecked);
    this.splitContainer.Dock = DockStyle.Fill;
    this.splitContainer.Location = new Point(3, 142);
    this.splitContainer.Name = "splitContainer";
    this.splitContainer.Orientation = Orientation.Horizontal;
    this.splitContainer.Panel1.Controls.Add((Control) this.splitContainerGraphs);
    this.splitContainer.Panel1MinSize = 50;
    this.splitContainer.Panel2.Controls.Add((Control) this.cbFileExts);
    this.splitContainer.Panel2.Controls.Add((Control) this.lvFileExts);
    this.splitContainer.Panel2MinSize = 50;
    this.splitContainer.Size = new Size(426, 381);
    this.splitContainer.SplitterDistance = 186;
    this.splitContainer.TabIndex = 3;
    this.splitContainerGraphs.Dock = DockStyle.Fill;
    this.splitContainerGraphs.Location = new Point(0, 0);
    this.splitContainerGraphs.Name = "splitContainerGraphs";
    this.splitContainerGraphs.Panel1.Controls.Add((Control) this.lvGraphs);
    this.splitContainerGraphs.Panel1.Controls.Add((Control) this.cbGraphs);
    this.splitContainerGraphs.Panel2.Controls.Add((Control) this.lvEmptyGraphs);
    this.splitContainerGraphs.Panel2.Controls.Add((Control) this.cbEmptyGraphs);
    this.splitContainerGraphs.Size = new Size(426, 186);
    this.splitContainerGraphs.SplitterDistance = 213;
    this.splitContainerGraphs.TabIndex = 2;
    this.cbGraphs.AutoSize = true;
    this.cbGraphs.Location = new Point(11, 11);
    this.cbGraphs.Name = "cbGraphs";
    this.cbGraphs.Size = new Size(128 /*0x80*/, 17);
    this.cbGraphs.TabIndex = 1;
    this.cbGraphs.Text = "Графы c подписями";
    this.cbGraphs.UseVisualStyleBackColor = true;
    this.cbGraphs.CheckedChanged += new EventHandler(this.cbGraphs_CheckedChanged);
    this.lvEmptyGraphs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.lvEmptyGraphs.CheckBoxes = true;
    this.lvEmptyGraphs.Location = new Point(3, 33);
    this.lvEmptyGraphs.Name = "lvEmptyGraphs";
    this.lvEmptyGraphs.Size = new Size(203, 153);
    this.lvEmptyGraphs.Sorting = SortOrder.Ascending;
    this.lvEmptyGraphs.TabIndex = 2;
    this.lvEmptyGraphs.UseCompatibleStateImageBehavior = false;
    this.lvEmptyGraphs.View = View.List;
    this.lvEmptyGraphs.ItemChecked += new ItemCheckedEventHandler(this.lvEmptyGraphs_ItemChecked);
    this.cbEmptyGraphs.AutoSize = true;
    this.cbEmptyGraphs.Location = new Point(13, 11);
    this.cbEmptyGraphs.Name = "cbEmptyGraphs";
    this.cbEmptyGraphs.Size = new Size(132, 17);
    this.cbEmptyGraphs.TabIndex = 3;
    this.cbEmptyGraphs.Text = "Графы без подписей";
    this.cbEmptyGraphs.UseVisualStyleBackColor = true;
    this.cbEmptyGraphs.CheckedChanged += new EventHandler(this.cbEmptyGraphs_CheckedChanged);
    this.cbFileExts.AutoSize = true;
    this.cbFileExts.Location = new Point(11, 10);
    this.cbFileExts.Name = "cbFileExts";
    this.cbFileExts.Size = new Size(214, 17);
    this.cbFileExts.TabIndex = 1;
    this.cbFileExts.Text = "Типы файлов, которые попадут в УЛ";
    this.cbFileExts.UseVisualStyleBackColor = true;
    this.cbFileExts.CheckedChanged += new EventHandler(this.cbFileExts_CheckedChanged);
    this.cbEnabled.AutoSize = true;
    this.cbEnabled.Checked = true;
    this.cbEnabled.CheckState = CheckState.Checked;
    this.cbEnabled.Enabled = false;
    this.cbEnabled.Location = new Point(13, 3);
    this.cbEnabled.Name = "cbEnabled";
    this.cbEnabled.Size = new Size(222, 17);
    this.cbEnabled.TabIndex = 4;
    this.cbEnabled.Text = "Формировать удостоверяющие листы";
    this.cbEnabled.UseVisualStyleBackColor = true;
    this.cbEnabled.CheckedChanged += new EventHandler(this.cbEnabled_CheckedChanged);
    this.tableLayoutPanel.ColumnCount = 1;
    this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel.Controls.Add((Control) this.panel1, 0, 0);
    this.tableLayoutPanel.Controls.Add((Control) this.gbOptions, 0, 1);
    this.tableLayoutPanel.Controls.Add((Control) this.splitContainer, 0, 2);
    this.tableLayoutPanel.Dock = DockStyle.Fill;
    this.tableLayoutPanel.Location = new Point(0, 0);
    this.tableLayoutPanel.Name = "tableLayoutPanel";
    this.tableLayoutPanel.RowCount = 3;
    this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
    this.tableLayoutPanel.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel.Size = new Size(432, 526);
    this.tableLayoutPanel.TabIndex = 5;
    this.panel1.Controls.Add((Control) this.cbEnabled);
    this.panel1.Dock = DockStyle.Fill;
    this.panel1.Location = new Point(3, 3);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(426, 19);
    this.panel1.TabIndex = 7;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel);
    this.Name = nameof (CertSheetControl);
    this.Size = new Size(432, 526);
    this.Load += new EventHandler(this.CertSheetControl_Load);
    this.gbOptions.ResumeLayout(false);
    this.gbOptions.PerformLayout();
    this.splitContainer.Panel1.ResumeLayout(false);
    this.splitContainer.Panel2.ResumeLayout(false);
    this.splitContainer.Panel2.PerformLayout();
    this.splitContainer.EndInit();
    this.splitContainer.ResumeLayout(false);
    this.splitContainerGraphs.Panel1.ResumeLayout(false);
    this.splitContainerGraphs.Panel1.PerformLayout();
    this.splitContainerGraphs.Panel2.ResumeLayout(false);
    this.splitContainerGraphs.Panel2.PerformLayout();
    this.splitContainerGraphs.EndInit();
    this.splitContainerGraphs.ResumeLayout(false);
    this.tableLayoutPanel.ResumeLayout(false);
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    this.ResumeLayout(false);
  }
}

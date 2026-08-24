// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ArchiveSettingsControl
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Search.Controls;
using Intermech.ImpExp.Search.Properties;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.LifeCycles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Search;

internal sealed class ArchiveSettingsControl : StepControl
{
  private Button bDelete;
  private Button bAdd;
  private SplitContainer splitContainer1;
  private ListView lwArchives;
  private TabControl tabControl1;
  private TabPage tabPage1;
  private TreeView twSchemes;
  private IContainer components;
  private ColumnHeader columnHeader1;
  private Panel panel1;
  private Panel panel2;
  private List<ArchiveSettingsControl.ArchiveSettings> _archives;
  private SelectSchemeForm _selectForm;
  private int _currentArchiveID = -1;
  private const string _setName = "SCHEM_ARCHIVES";
  private Panel panel3;
  private Label label2;
  private readonly SearchPlugin _plugin;
  private GroupBox groupBox1;
  private GroupBox groupBox2;
  private Button bClear;
  private Button bSet;
  private TreeView tvStatuses;
  private List<Tuple<string, int>> _levels;
  private ImageList _levelsImageList;
  private SplitContainer splitContainer2;
  private Image _image;

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
      this._image = service.ImageList.Images[service.ImageIndex("imgArchiveParams")];
    return this._image;
  }

  private void ReadLCSteps4Archives(IUserSession session, IImportingData cacheData)
  {
    Dictionary<object, DictionaryValue> category = cacheData.GetCategory(ImportingCategory.LCSteps4Archives);
    if (category == null || category.Count <= 0)
      return;
    this._archives = new List<ArchiveSettingsControl.ArchiveSettings>(category.Count);
    foreach (KeyValuePair<object, DictionaryValue> keyValuePair in category)
    {
      int int32 = Convert.ToInt32(keyValuePair.Key);
      DictionaryValue dictionaryValue = keyValuePair.Value;
      if (dictionaryValue != null)
      {
        ArchiveSettingsControl.ArchiveSettings archiveSettings = new ArchiveSettingsControl.ArchiveSettings()
        {
          ArchiveID = int32,
          ArchiveName = dictionaryValue.Caption
        };
        if (dictionaryValue.Tag is LCSteps4Archives tag && tag.LCSteps4 != null)
        {
          IDictionaryEnumerator enumerator = (IDictionaryEnumerator) tag.LCSteps4.GetEnumerator();
          while (enumerator.MoveNext())
          {
            int key = (int) enumerator.Key;
            int aLCStepID = (int) enumerator.Value;
            ArchiveSettingsControl.ArchiveSheme archiveSheme = new ArchiveSettingsControl.ArchiveSheme();
            IDBLCSchema lcSchema = session.GetLCSchema(key, false);
            if (lcSchema != null)
            {
              archiveSheme.SchemeID = lcSchema.SchemaID;
              archiveSheme.SchemeName = lcSchema.Name;
              IDBLifecycleStep lifecycleStep = session.GetLifecycleStep(aLCStepID, false);
              if (lifecycleStep == null)
              {
                int firstStep = lcSchema.GetStepsCollection().GetFirstStep();
                lifecycleStep = session.GetLifecycleStep(firstStep);
              }
              archiveSheme.LCStepID = lifecycleStep.LCStep;
              archiveSheme.LCStepName = lifecycleStep.LCName;
              archiveSettings.Schemes.Add(archiveSheme);
            }
          }
        }
        if (archiveSettings.ArchiveID != -1)
          this._archives.Add(archiveSettings);
      }
    }
  }

  private void ReadLevelsFromBase(IUserSession session)
  {
    if (this._levels != null)
      return;
    this._levelsImageList = new ImageList()
    {
      ColorDepth = ColorDepth.Depth24Bit
    };
    this._levelsImageList.Images.Add(Resources.EmptyIcon);
    DataTable dataTable = session.GetLifecycleLevelCollection().Select(string.Empty);
    this._levels = new List<Tuple<string, int>>();
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      this._levels.Add(new Tuple<string, int>(Convert.ToString(row["F_LEVEL_NAME"]), Convert.ToInt32(row["F_LEVEL_ID"])));
      Icon icon = (Icon) null;
      if (row["F_ICON"] != DBNull.Value)
      {
        using (MemoryStream memoryStream = new MemoryStream((byte[]) row["F_ICON"]))
          icon = new Icon((Stream) memoryStream);
      }
      else
        icon = Resources.EmptyIcon;
      this._levelsImageList.Images.Add(icon);
    }
    this._levelsImageList.Images.Add(Resources.SearchStatus);
    this.tvStatuses.ImageList = this._levelsImageList;
  }

  private void ReadSearchStatuses(IUserSession session, IImportingData cacheData)
  {
    if (this.tvStatuses.Nodes.Count > 0)
      this.tvStatuses.Nodes.Clear();
    this.ReadLevelsFromBase(session);
    cacheData.GetCategory(ImportingCategory.StatusesToLevels);
    IDbCommand command = this._plugin.idb.DbConnection.CreateCommand();
    command.CommandText = "SELECT VERSION_STATE_ID, VERSION_STATE_NAME FROM VERSION_STATE WHERE VERSION_STATE_ID > 0";
    using (IDataReader dataReader = command.ExecuteReader())
    {
      while (dataReader.Read())
      {
        int int32 = BasePumpHelper.ToInt32(dataReader[0]);
        string stateName = dataReader.IsDBNull(1) ? "" : dataReader.GetString(1);
        TreeNode treeNode1 = this.tvStatuses.Nodes.Add(stateName);
        treeNode1.Tag = (object) int32;
        int num1;
        int num2 = num1 = this._levelsImageList.Images.Count - 1;
        treeNode1.SelectedImageIndex = num1;
        treeNode1.ImageIndex = num2;
        DictionaryValue val = cacheData.GetValue(ImportingCategory.StatusesToLevels, (object) int32);
        int levelID = 0;
        string text = "<Не настроено>";
        if (val == null)
        {
          Tuple<string, int> tuple = this._levels.Find((Predicate<Tuple<string, int>>) (x => x.Item1.Equals(stateName, StringComparison.CurrentCultureIgnoreCase)));
          if (tuple != null)
          {
            levelID = tuple.Item2;
            text = tuple.Item1;
          }
        }
        else
        {
          Tuple<string, int> tuple = this._levels.Find((Predicate<Tuple<string, int>>) (x => x.Item2.Equals(Convert.ToInt32(val.NewObjectID))));
          if (tuple != null)
          {
            levelID = tuple.Item2;
            text = tuple.Item1;
          }
        }
        TreeNode treeNode2 = treeNode1.Nodes.Add(text);
        treeNode2.Tag = (object) levelID;
        int num3;
        int num4 = num3 = levelID != 0 ? this._levels.FindIndex((Predicate<Tuple<string, int>>) (x => x.Item2.Equals(levelID))) + 1 : 0;
        treeNode2.SelectedImageIndex = num3;
        treeNode2.ImageIndex = num4;
      }
    }
    this.tvStatuses.ExpandAll();
    this.RefreshStatusesButtons();
  }

  private void RefreshStatusesButtons()
  {
    this.bSet.Enabled = this.bClear.Enabled = this.tvStatuses.Nodes.Count > 0 && this.tvStatuses.SelectedNode != null && this.tvStatuses.SelectedNode.Nodes.Count == 0;
  }

  public override void RefreshControl()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service.GetCache(ImportingCategory.LCSteps4Archives, ImportingCategory.StatusesToLevels);
    try
    {
      IUserSession userSession = (ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter).GetUserSession();
      this.ReadLCSteps4Archives(userSession, cache);
      this.ReadSearchStatuses(userSession, cache);
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.LCSteps4Archives, ImportingCategory.StatusesToLevels);
    }
  }

  public ArchiveSettingsControl(SearchPlugin plugin)
  {
    this._plugin = plugin;
    this.InitializeComponent();
  }

  private void RefreshArchives()
  {
    if (this.lwArchives.SmallImageList == null)
    {
      ImageList imageList = new ImageList()
      {
        ColorDepth = ColorDepth.Depth24Bit
      };
      imageList.Images.Add(Resources.archives);
      this.lwArchives.SmallImageList = imageList;
    }
    this.lwArchives.Items.Clear();
    if (this._archives == null || this._archives.Count == 0)
      return;
    this.lwArchives.BeginUpdate();
    foreach (ArchiveSettingsControl.ArchiveSettings archive in this._archives)
      this.lwArchives.Items.Add(new ListViewItem(archive.ArchiveName)
      {
        Tag = (object) archive.ArchiveID,
        ImageIndex = 0
      });
    this.lwArchives.EndUpdate();
  }

  private void SelectArchive(int archiveID, string name)
  {
    this.twSchemes.Nodes.Clear();
    if (this._archives == null || this._archives.Count == 0)
      return;
    ArchiveSettingsControl.ArchiveSettings archiveSettings = this._archives.Find((Predicate<ArchiveSettingsControl.ArchiveSettings>) (x => x.ArchiveID == archiveID));
    try
    {
      if (archiveSettings == null || archiveSettings.Schemes == null || archiveSettings.Schemes.Count <= 0)
        return;
      this.twSchemes.BeginUpdate();
      foreach (ArchiveSettingsControl.ArchiveSheme scheme in archiveSettings.Schemes)
      {
        TreeNode node1 = new TreeNode(scheme.SchemeName)
        {
          Tag = (object) scheme.SchemeID
        };
        TreeNode node2 = new TreeNode(scheme.LCStepName);
        node1.Nodes.Add(node2);
        this.twSchemes.Nodes.Add(node1);
      }
      this.twSchemes.EndUpdate();
    }
    finally
    {
      this._currentArchiveID = archiveID;
      this.label2.Text = name;
    }
  }

  protected override string getCaption() => "Параметры импорта архивов";

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void SaveToCache()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    service.DeleteCache(ImportingCategory.StatusesToLevels);
    if (this.tvStatuses.Nodes.Count > 0)
    {
      IImportingData cache = service.GetCache(ImportingCategory.StatusesToLevels);
      try
      {
        foreach (TreeNode node in this.tvStatuses.Nodes)
        {
          int tag = (int) node.Nodes[0].Tag;
          if (!tag.Equals(0))
            cache.AddValue((object) (int) node.Tag, (long) tag);
        }
      }
      finally
      {
        service.ReleaseCache(ImportingCategory.StatusesToLevels);
      }
    }
    service.DeleteCache(ImportingCategory.LCSteps4Archives);
    if (this._archives == null || this._archives.Count <= 0)
      return;
    ServicesManager.GetService(typeof (IMetadataInfo));
    IImportingData cache1 = service.GetCache(ImportingCategory.LCSteps4Archives);
    try
    {
      foreach (ArchiveSettingsControl.ArchiveSettings archive in this._archives)
      {
        if (archive.Schemes != null && archive.Schemes.Count > 0)
        {
          LCSteps4Archives tag = new LCSteps4Archives();
          foreach (ArchiveSettingsControl.ArchiveSheme scheme in archive.Schemes)
            tag.LCSteps4.Add(scheme.SchemeID, scheme.LCStepID);
          cache1.AddValue((object) archive.ArchiveID, long.MinValue, archive.ArchiveName, (ITagImportObject) tag);
        }
      }
    }
    finally
    {
      service.ReleaseCache(ImportingCategory.LCSteps4Archives);
    }
  }

  public override bool LeaveControl()
  {
    this.SaveToCache();
    return true;
  }

  public override SaveSettingsResult SaveSettings()
  {
    this.SaveToCache();
    return SaveSettingsResult.ssrOk;
  }

  private void InitializeComponent()
  {
    this.bDelete = new Button();
    this.bAdd = new Button();
    this.splitContainer1 = new SplitContainer();
    this.lwArchives = new ListView();
    this.columnHeader1 = new ColumnHeader();
    this.panel3 = new Panel();
    this.label2 = new Label();
    this.tabControl1 = new TabControl();
    this.tabPage1 = new TabPage();
    this.panel1 = new Panel();
    this.twSchemes = new TreeView();
    this.panel2 = new Panel();
    this.groupBox1 = new GroupBox();
    this.bClear = new Button();
    this.bSet = new Button();
    this.tvStatuses = new TreeView();
    this.groupBox2 = new GroupBox();
    this.splitContainer2 = new SplitContainer();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.panel3.SuspendLayout();
    this.tabControl1.SuspendLayout();
    this.tabPage1.SuspendLayout();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.splitContainer2.BeginInit();
    this.splitContainer2.Panel1.SuspendLayout();
    this.splitContainer2.Panel2.SuspendLayout();
    this.splitContainer2.SuspendLayout();
    this.SuspendLayout();
    this.bDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bDelete.Enabled = false;
    this.bDelete.Location = new Point((int) byte.MaxValue, 6);
    this.bDelete.Name = "bDelete";
    this.bDelete.Size = new Size(75, 23);
    this.bDelete.TabIndex = 1;
    this.bDelete.Text = "Удалить";
    this.bDelete.UseVisualStyleBackColor = true;
    this.bDelete.Click += new EventHandler(this.Delete_Click);
    this.bAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bAdd.Location = new Point(174, 6);
    this.bAdd.Name = "bAdd";
    this.bAdd.Size = new Size(75, 23);
    this.bAdd.TabIndex = 0;
    this.bAdd.Text = "Добавить";
    this.bAdd.UseVisualStyleBackColor = true;
    this.bAdd.Click += new EventHandler(this.Add_Click);
    this.splitContainer1.Dock = DockStyle.Fill;
    this.splitContainer1.Location = new Point(3, 16 /*0x10*/);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.lwArchives);
    this.splitContainer1.Panel1.Controls.Add((Control) this.panel3);
    this.splitContainer1.Panel2.Controls.Add((Control) this.tabControl1);
    this.splitContainer1.Size = new Size(531, 188);
    this.splitContainer1.SplitterDistance = 173;
    this.splitContainer1.TabIndex = 3;
    this.lwArchives.Columns.AddRange(new ColumnHeader[1]
    {
      this.columnHeader1
    });
    this.lwArchives.Dock = DockStyle.Fill;
    this.lwArchives.HideSelection = false;
    this.lwArchives.Location = new Point(0, 0);
    this.lwArchives.MultiSelect = false;
    this.lwArchives.Name = "lwArchives";
    this.lwArchives.Size = new Size(173, 148);
    this.lwArchives.TabIndex = 0;
    this.lwArchives.UseCompatibleStateImageBehavior = false;
    this.lwArchives.View = View.Details;
    this.lwArchives.SelectedIndexChanged += new EventHandler(this.Archives_SelectedIndexChanged);
    this.columnHeader1.Text = "Архивы";
    this.columnHeader1.Width = 200;
    this.panel3.Controls.Add((Control) this.label2);
    this.panel3.Dock = DockStyle.Bottom;
    this.panel3.Location = new Point(0, 148);
    this.panel3.Name = "panel3";
    this.panel3.Size = new Size(173, 40);
    this.panel3.TabIndex = 1;
    this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
    this.label2.Location = new Point(0, 11);
    this.label2.Name = "label2";
    this.label2.Size = new Size(170, 29);
    this.label2.TabIndex = 0;
    this.tabControl1.Controls.Add((Control) this.tabPage1);
    this.tabControl1.Dock = DockStyle.Fill;
    this.tabControl1.Location = new Point(0, 0);
    this.tabControl1.Name = "tabControl1";
    this.tabControl1.SelectedIndex = 0;
    this.tabControl1.Size = new Size(354, 188);
    this.tabControl1.TabIndex = 0;
    this.tabPage1.BackColor = Color.Transparent;
    this.tabPage1.Controls.Add((Control) this.panel1);
    this.tabPage1.Controls.Add((Control) this.panel2);
    this.tabPage1.Location = new Point(4, 22);
    this.tabPage1.Name = "tabPage1";
    this.tabPage1.Padding = new Padding(3);
    this.tabPage1.Size = new Size(346, 162);
    this.tabPage1.TabIndex = 0;
    this.tabPage1.Text = "Схемы ЖЦ";
    this.tabPage1.UseVisualStyleBackColor = true;
    this.panel1.Controls.Add((Control) this.twSchemes);
    this.panel1.Dock = DockStyle.Fill;
    this.panel1.Location = new Point(3, 3);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(340, 123);
    this.panel1.TabIndex = 2;
    this.twSchemes.Dock = DockStyle.Fill;
    this.twSchemes.Location = new Point(0, 0);
    this.twSchemes.Name = "twSchemes";
    this.twSchemes.Size = new Size(340, 123);
    this.twSchemes.TabIndex = 0;
    this.twSchemes.AfterSelect += new TreeViewEventHandler(this.Schemes_AfterSelect);
    this.panel2.Controls.Add((Control) this.bDelete);
    this.panel2.Controls.Add((Control) this.bAdd);
    this.panel2.Dock = DockStyle.Bottom;
    this.panel2.Location = new Point(3, 126);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(340, 33);
    this.panel2.TabIndex = 1;
    this.groupBox1.Controls.Add((Control) this.bClear);
    this.groupBox1.Controls.Add((Control) this.bSet);
    this.groupBox1.Controls.Add((Control) this.tvStatuses);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(0, 0);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(537, 102);
    this.groupBox1.TabIndex = 4;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Соответствия уровней продвижения объектов в IPS статусам документов в Search";
    this.bClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.bClear.Location = new Point(452, 47);
    this.bClear.Name = "bClear";
    this.bClear.Size = new Size(75, 23);
    this.bClear.TabIndex = 2;
    this.bClear.Text = "Очистить";
    this.bClear.UseVisualStyleBackColor = true;
    this.bClear.Click += new EventHandler(this.Clear_Click);
    this.bSet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.bSet.Location = new Point(452, 19);
    this.bSet.Name = "bSet";
    this.bSet.Size = new Size(75, 23);
    this.bSet.TabIndex = 1;
    this.bSet.Text = "Установить";
    this.bSet.UseVisualStyleBackColor = true;
    this.bSet.Click += new EventHandler(this.Set_Click);
    this.tvStatuses.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.tvStatuses.Location = new Point(9, 19);
    this.tvStatuses.Name = "tvStatuses";
    this.tvStatuses.Size = new Size(437, 77);
    this.tvStatuses.TabIndex = 0;
    this.tvStatuses.AfterSelect += new TreeViewEventHandler(this.Statuses_AfterSelect);
    this.groupBox2.Controls.Add((Control) this.splitContainer1);
    this.groupBox2.Dock = DockStyle.Fill;
    this.groupBox2.Location = new Point(0, 0);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(537, 207);
    this.groupBox2.TabIndex = 5;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Cхемы ЖЦ для документов соответствующих архивов";
    this.splitContainer2.Dock = DockStyle.Fill;
    this.splitContainer2.Location = new Point(5, 5);
    this.splitContainer2.Name = "splitContainer2";
    this.splitContainer2.Orientation = Orientation.Horizontal;
    this.splitContainer2.Panel1.Controls.Add((Control) this.groupBox2);
    this.splitContainer2.Panel2.Controls.Add((Control) this.groupBox1);
    this.splitContainer2.Size = new Size(537, 313);
    this.splitContainer2.SplitterDistance = 207;
    this.splitContainer2.TabIndex = 6;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.Controls.Add((Control) this.splitContainer2);
    this.Name = nameof (ArchiveSettingsControl);
    this.Padding = new Padding(5);
    this.Size = new Size(547, 323);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.tabControl1.ResumeLayout(false);
    this.tabPage1.ResumeLayout(false);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.groupBox2.ResumeLayout(false);
    this.splitContainer2.Panel1.ResumeLayout(false);
    this.splitContainer2.Panel2.ResumeLayout(false);
    this.splitContainer2.EndInit();
    this.splitContainer2.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  public void AddArchive(string archiveName, int archiveID)
  {
    if (this._archives == null)
      this._archives = new List<ArchiveSettingsControl.ArchiveSettings>();
    if (this._archives.Find((Predicate<ArchiveSettingsControl.ArchiveSettings>) (x => x.ArchiveID == archiveID)) != null)
      return;
    this._archives.Add(new ArchiveSettingsControl.ArchiveSettings()
    {
      ArchiveID = archiveID,
      ArchiveName = archiveName,
      Schemes = new List<ArchiveSettingsControl.ArchiveSheme>()
    });
  }

  private void Schemes_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this.bDelete.Enabled = e.Node.Tag != null;
  }

  private void Archives_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (this.lwArchives.SelectedItems == null || this.lwArchives.SelectedItems.Count == 0)
      return;
    object tag = this.lwArchives.SelectedItems[0].Tag;
    if (tag == null)
      return;
    this.SelectArchive((int) tag, this.lwArchives.SelectedItems[0].Text);
  }

  private void Add_Click(object sender, EventArgs e)
  {
    if (this._selectForm == null)
    {
      IUserSession userSession = (ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter).GetUserSession();
      this._selectForm = new SelectSchemeForm();
      this._selectForm.FillControls(userSession);
    }
    if (this._selectForm.ShowDialog() != DialogResult.OK)
      return;
    ArchiveSettingsControl.ArchiveSettings archiveSettings = this._archives.Find((Predicate<ArchiveSettingsControl.ArchiveSettings>) (p => p.ArchiveID == this._currentArchiveID));
    if (archiveSettings == null)
    {
      archiveSettings = new ArchiveSettingsControl.ArchiveSettings(this._currentArchiveID, this.lwArchives.SelectedItems[0].Text);
      this._archives.Add(archiveSettings);
    }
    ArchiveSettingsControl.ArchiveSheme archiveSheme = archiveSettings.Schemes.Find((Predicate<ArchiveSettingsControl.ArchiveSheme>) (p => p.SchemeID == this._selectForm.SchemeID));
    if (archiveSheme != null)
      archiveSheme.LCStepID = this._selectForm.DefaultStep.ID;
    else
      archiveSettings.Schemes.Add(new ArchiveSettingsControl.ArchiveSheme(this._selectForm.SchemeID, this._selectForm.SchemeName, this._selectForm.DefaultStep.ID, this._selectForm.DefaultStep.Name));
    this.SelectArchive(archiveSettings.ArchiveID, archiveSettings.ArchiveName);
  }

  private void Delete_Click(object sender, EventArgs e)
  {
    if (this.twSchemes.SelectedNode == null)
      return;
    foreach (ArchiveSettingsControl.ArchiveSettings archive in this._archives)
    {
      if (archive.ArchiveID == this._currentArchiveID)
      {
        ArchiveSettingsControl.ArchiveSheme archiveSheme = (ArchiveSettingsControl.ArchiveSheme) null;
        foreach (ArchiveSettingsControl.ArchiveSheme scheme in archive.Schemes)
        {
          if ((int) this.twSchemes.SelectedNode.Tag == scheme.SchemeID)
          {
            archiveSheme = scheme;
            break;
          }
        }
        if (archiveSheme == null)
          break;
        archive.Schemes.Remove(archiveSheme);
        this.SelectArchive(archive.ArchiveID, archive.ArchiveName);
        break;
      }
    }
  }

  private void InternalRefreshData()
  {
    this.RefreshArchives();
    if (this.lwArchives.Items.Count <= 0)
      return;
    ListViewItem listViewItem = this.lwArchives.Items[0];
    this.SelectArchive((int) listViewItem.Tag, listViewItem.Text);
  }

  public void RefreshData()
  {
    this.BeginInvoke((Delegate) new MethodInvoker(this.InternalRefreshData));
  }

  private void Clear_Click(object sender, EventArgs e)
  {
    if (this.tvStatuses.SelectedNode == null || !(this.tvStatuses.SelectedNode.Tag is int tag) || tag == 0)
      return;
    this.tvStatuses.SelectedNode.Text = "<Не настроено>";
    this.tvStatuses.SelectedNode.Tag = (object) 0;
    this.tvStatuses.SelectedNode.ImageIndex = this.tvStatuses.SelectedNode.SelectedImageIndex = 0;
  }

  private void Set_Click(object sender, EventArgs e)
  {
    if (this.tvStatuses.SelectedNode == null || this.tvStatuses.SelectedNode.Tag == null)
      return;
    using (SelectLevelForm selectLevelForm = new SelectLevelForm(this._levels, this._levelsImageList))
    {
      if (selectLevelForm.ShowDialog() != DialogResult.OK)
        return;
      Tuple<string, int> newLevel = selectLevelForm.SelectedLevel;
      if (newLevel == null)
        return;
      TreeNode selectedNode = this.tvStatuses.SelectedNode;
      selectedNode.Text = newLevel.Item1;
      selectedNode.Tag = (object) newLevel.Item2;
      int num1;
      int num2 = num1 = newLevel.Item2 != 0 ? this._levels.FindIndex((Predicate<Tuple<string, int>>) (x => x.Item2.Equals(newLevel.Item2))) + 1 : 0;
      selectedNode.SelectedImageIndex = num1;
      selectedNode.ImageIndex = num2;
    }
  }

  private void Statuses_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this.RefreshStatusesButtons();
  }

  private class ArchiveSettings
  {
    public string ArchiveName;
    public int ArchiveID;
    public List<ArchiveSettingsControl.ArchiveSheme> Schemes = new List<ArchiveSettingsControl.ArchiveSheme>();

    public ArchiveSettings()
      : this(-1, string.Empty)
    {
    }

    public ArchiveSettings(int id, string name)
    {
      this.ArchiveName = name;
      this.ArchiveID = id;
    }
  }

  private class ArchiveSheme
  {
    public string SchemeName;
    public int SchemeID;
    public string LCStepName;
    public int LCStepID;

    public ArchiveSheme()
    {
    }

    public ArchiveSheme(int schemeID, string schemeName, int lcStepID, string lcStepName)
    {
      this.SchemeID = schemeID;
      this.SchemeName = schemeName;
      this.LCStepID = lcStepID;
      this.LCStepName = lcStepName;
    }
  }

  private delegate void addArchiveDelegate(string archiveName, int archiveID);
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.PreciseProducts.CreatePreciseProductForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;
using Intermech.Archives;
using Intermech.AVS;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Pdm;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using Intermech.Search.UI;
using Intermech.Search.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.PreciseProducts;

public class CreatePreciseProductForm : Form
{
  private Tuple<long, long> _compositionPartID = new Tuple<long, long>(0L, 0L);
  private long _topObjectVersionID;
  private int _topObjectTypeID = -1;
  private BindingList<PreciseProductBlank> _preciseProductsBlanks = new BindingList<PreciseProductBlank>();
  private ArchivesClientStartup.ArchiveEditor _archiveEditor = new ArchivesClientStartup.ArchiveEditor();
  private ArchivesClientStartup.ArchiveProxy _archive;
  private CreatePreciseProductResult _createPreciseProductResult;
  private IContainer components;
  private Button _createButton;
  private CheckBox _keepCheckedOutCreatedObjects;
  private CheckBox _copyDocumentionCheckBox;
  private Intermech.VirtualTreeView.VirtualTreeView _treeView;
  private Column _relationIDColumn;
  private Column _productVersionIDColumn;
  private Column _productCaptionColumn;
  private Column _preciseProductDesignationColumn;
  private Column _preciseProductNameColumn;
  private Label label2;
  private Button _generateDesignationsButton;
  private TextBox _separatorTextBox;
  private MessageList _messageList;
  private SplitContainer splitContainer1;
  private Button _selectArchiveButton;
  private TextBox _archiveTextBox;
  private TabControl tabControl1;
  private TabPage tabPage1;
  private TabPage tabPage2;
  private TabPage tabPage3;
  private TabPage tabPage4;
  private Button _closeButton;
  private CheckBox _useConfigurationCodeCheckBox;
  private TextBox _optionSeparatorTextBox;
  private Label label;
  private TextBox _optionAndOptionValuesSeparatorTextBox;
  private Label label1;
  private Label label3;
  private TextBox _emptyOptionCodeReplacementTextBox;
  private Label label4;
  private TextBox _emptyOptionValueCodeReplacementTextBox;
  private Label label5;
  private ToolStrip _toolStrip;
  private ToolStripButton _openInNewWindowToolStripButton;
  private ContextMenuStrip _contextMenuStrip;
  private ToolStripMenuItem _openInNewWindowToolStripMenuItem;
  private CheckBox _useExistProductsCheckBox;
  private TableLayoutPanel tableLayoutPanel1;
  private FlowLayoutPanel flowLayoutPanel1;

  public CreatePreciseProductForm()
  {
    this.InitializeComponent();
    CreatePreciseProductForm.PreciseProductBlankRowBinding productBlankRowBinding = new CreatePreciseProductForm.PreciseProductBlankRowBinding();
    productBlankRowBinding.CellBindings.Add((CellBinding) new ObjectCellBinding(this._productCaptionColumn, "ProductCaption"));
    productBlankRowBinding.CellBindings.Add((CellBinding) new ObjectCellBinding(this._productVersionIDColumn, "ProductVersionID"));
    CellBindingList cellBindings1 = productBlankRowBinding.CellBindings;
    ObjectCellBinding objectCellBinding1 = new ObjectCellBinding(this._preciseProductDesignationColumn, "PreciseProductDesignation");
    objectCellBinding1.Editor = new CellEditor((Control) new TextBox());
    cellBindings1.Add((CellBinding) objectCellBinding1);
    CellBindingList cellBindings2 = productBlankRowBinding.CellBindings;
    ObjectCellBinding objectCellBinding2 = new ObjectCellBinding(this._preciseProductNameColumn, "PreciseProductName");
    objectCellBinding2.Editor = new CellEditor((Control) new TextBox());
    cellBindings2.Add((CellBinding) objectCellBinding2);
    productBlankRowBinding.CellBindings.Add((CellBinding) new ObjectCellBinding(this._relationIDColumn, "RelationID"));
    this._treeView.RowBindings.Add((RowBinding) productBlankRowBinding);
    this._treeView.DataSource = (object) this._preciseProductsBlanks;
    if (ServicesManager.GetService(typeof (INamedImageList)) is INamedImageList service)
      this._openInNewWindowToolStripButton.Image = this._openInNewWindowToolStripMenuItem.Image = service.ImageList.Images[service.ImageIndex("imgNavigator")];
    this.UpdateDialog();
  }

  [Browsable(false)]
  public Tuple<long, long> CompositionPartID
  {
    get => this._compositionPartID;
    set
    {
      if (this.IsUnknownCompositonPartID(value))
        throw new ArgumentException();
      this.SetCompositonPartID(value);
    }
  }

  private void TreeView_SelectionChanged(object sender, EventArgs e) => this.UpdateDialog();

  private void GenerateDesignationsButton_Click(object sender, EventArgs e)
  {
    this.GenerateDesignations();
  }

  private void CreateButton_Click(object sender, EventArgs e) => this.CreatePreciseProduct();

  private void SelectArchiveButton_Click(object sender, EventArgs e)
  {
    this._archive = this._archiveEditor.EditValue((ITypeDescriptorContext) null, (IServiceProvider) null, (object) this._archive) as ArchivesClientStartup.ArchiveProxy;
    if (this._archive == null)
      return;
    this._archiveTextBox.Text = this.GetObjectCaption(this._archive.Value);
  }

  private void CloseButton_Click(object sender, EventArgs e) => this.Close();

  private void UseConfigurationCodeCheckBox_CheckedChanged(object sender, EventArgs e)
  {
    this._separatorTextBox.Enabled = !this._useConfigurationCodeCheckBox.Checked;
    this._optionSeparatorTextBox.Enabled = !this._useConfigurationCodeCheckBox.Checked;
    this._optionAndOptionValuesSeparatorTextBox.Enabled = !this._useConfigurationCodeCheckBox.Checked;
    this._emptyOptionCodeReplacementTextBox.Enabled = !this._useConfigurationCodeCheckBox.Checked;
    this._emptyOptionValueCodeReplacementTextBox.Enabled = !this._useConfigurationCodeCheckBox.Checked;
  }

  private void OpenInNewWindowToolStripMenuItem_Click(object sender, EventArgs e)
  {
    this.OpenInNewWindow();
  }

  private void OpenInNewWindowToolStripButton_Click(object sender, EventArgs e)
  {
    this.OpenInNewWindow();
  }

  private void UpdateDialog()
  {
    this._openInNewWindowToolStripButton.Enabled = this._openInNewWindowToolStripMenuItem.Enabled = this.CanOpenInNewWindow();
  }

  private bool CanOpenInNewWindow()
  {
    return this._treeView.SelectedItems != null && this._treeView.SelectedItems.Count > 0 && this._createPreciseProductResult != null && this._createPreciseProductResult.CreatedPreciseProductVersionIDDictionaryByCompositionPartID != null && this._treeView.SelectedItems.Cast<PreciseProductBlank>().All<PreciseProductBlank>((Func<PreciseProductBlank, bool>) (o => this._createPreciseProductResult.CreatedPreciseProductVersionIDDictionaryByCompositionPartID.ContainsKey(new Tuple<long, long>(o.RelationID, o.ProductVersionID))));
  }

  private bool IsUnknownCompositonPartID(Tuple<long, long> compositionPartID)
  {
    return RelationHelper.IsUnknownRelationID(compositionPartID.Item1) || ObjectHelper.IsUnknownObjectVersionID(compositionPartID.Item2);
  }

  private void SetCompositonPartID(Tuple<long, long> compositionPartID)
  {
    this._compositionPartID = compositionPartID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this._topObjectVersionID = sessionKeeper.Session.GetRelation(this._compositionPartID.Item1).ProjID;
      this._topObjectTypeID = sessionKeeper.Session.GetObject(this._topObjectVersionID).ObjectType;
    }
    this._messageList.Messages.Clear();
    this._preciseProductsBlanks.Clear();
    this.CreatePreciseAssemblyBlanks();
  }

  private void CreatePreciseAssemblyBlanks()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IPreciseProductsServerService customService = sessionKeeper.Session.GetCustomService(typeof (IPreciseProductsServerService)) as IPreciseProductsServerService;
      CreatePreciseProductsBlanksParams createPreciseProductsBlanksParams = new CreatePreciseProductsBlanksParams(this._compositionPartID.Item1, this._compositionPartID.Item2);
      this._preciseProductsBlanks = new BindingList<PreciseProductBlank>((IList<PreciseProductBlank>) customService.CreatePreciseProductsBlanks(sessionKeeper.Session.SessionGUID, createPreciseProductsBlanksParams));
      this._treeView.DataSource = (object) this._preciseProductsBlanks;
    }
  }

  private void CreatePreciseProduct()
  {
    try
    {
      this.CheckPreciseProductsBlanks();
      if (this._messageList.Messages.Count > 0)
        return;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IPreciseProductsServerService customService = sessionKeeper.Session.GetCustomService(typeof (IPreciseProductsServerService)) as IPreciseProductsServerService;
        CreatePreciseProductParams createPreciseProductParams = new CreatePreciseProductParams(this._compositionPartID.Item1, this._compositionPartID.Item2)
        {
          Blanks = this._preciseProductsBlanks.ToList<PreciseProductBlank>(),
          SpecificationArchiveVersionID = this._archive.Value,
          CopyDocumentation = this._copyDocumentionCheckBox.Checked,
          KeepCheckedOutCreatedObjects = this._keepCheckedOutCreatedObjects.Checked,
          UseExistsProducts = this._useExistProductsCheckBox.Checked
        };
        Cursor cursor = this.Cursor;
        try
        {
          this.Cursor = Cursors.WaitCursor;
          try
          {
            this._createPreciseProductResult = customService.CreatePreciseProduct(sessionKeeper.Session.SessionGUID, createPreciseProductParams);
          }
          catch
          {
            this._messageList.Messages.Add(new _Message(_MessageType.Error, "Создание точных изделий было прервано. Произошла ошибка"));
            this._createPreciseProductResult = (CreatePreciseProductResult) null;
            throw;
          }
          this._messageList.Messages.Add(new _Message(_MessageType.Success, "Создание точных изделий успешно завершено"));
          foreach (long preciseAssemblyVersionID in this._createPreciseProductResult.CreatedPreciseProductVersionIDDictionaryByCompositionPartID.Values)
          {
            try
            {
              this.CreateSpecification(preciseAssemblyVersionID, createPreciseProductParams.SpecificationArchiveVersionID);
            }
            catch
            {
              this._messageList.Messages.Add(new _Message(_MessageType.Error, $"Создание спецификации точного изделия #{preciseAssemblyVersionID} было прервано. Произошла ошибка"));
              throw;
            }
            this._messageList.Messages.Add(new _Message(_MessageType.Success, $"Создание спецификации точного изделия #{preciseAssemblyVersionID} успешно завершено"));
          }
        }
        finally
        {
          this.Cursor = cursor;
        }
      }
    }
    finally
    {
      this.UpdateDialog();
    }
  }

  private string GetObjectCaption(long objectVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return sessionKeeper.Session.GetObject(objectVersionID).Caption;
  }

  private void CreateSpecification(long preciseAssemblyVersionID, long archiveVersionID)
  {
    AVSDocument avsDocument = AVSPlugin.Instance.LoadAVSDocument(preciseAssemblyVersionID, false);
    avsDocument.SaveAVSDocumentToDB();
    long documentId = avsDocument.DocumentID;
    avsDocument.Dispose();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(documentId);
      dbObject.Attributes.FindByID(PreciseProductsConstants.ArchiveAttributeTypeID).Value = (object) Math.Abs(archiveVersionID);
      if (this._keepCheckedOutCreatedObjects.Checked)
        return;
      dbObject.CheckIn();
    }
  }

  private void GenerateDesignations()
  {
    foreach (PreciseProductBlank preciseProductsBlank in (Collection<PreciseProductBlank>) this._preciseProductsBlanks)
      this.GenerateDesignation(preciseProductsBlank);
  }

  private void GenerateDesignation(PreciseProductBlank preciseProductBlank)
  {
    string str = "";
    if (this._useConfigurationCodeCheckBox.Checked)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBRelation relation = sessionKeeper.Session.GetRelation(preciseProductBlank.RelationID);
        ObjectOptionsHolder objectOptionsHolder1 = new ObjectOptionsHolder((object) sessionKeeper.Session.GetObject(preciseProductBlank.ProductVersionID));
        if (objectOptionsHolder1.Incompatibilities != null && objectOptionsHolder1.Incompatibilities.ConfigurationCode != null)
        {
          str = ConfigurationCode.BuildConfigurationCode(preciseProductBlank.ProductVersionID, this.CreatePdmConfiguratorContext(preciseProductBlank));
        }
        else
        {
          IDBObject source = sessionKeeper.Session.GetObject(this._compositionPartID.Item2);
          ObjectOptionsHolder objectOptionsHolder2 = new ObjectOptionsHolder((object) source);
          str = objectOptionsHolder2.Incompatibilities == null || objectOptionsHolder2.Incompatibilities.ConfigurationCode == null ? preciseProductBlank.ProductCaption : ConfigurationCode.BuildConfigurationCode(relation, source, sessionKeeper.Session);
        }
      }
    }
    else
      str = this.GenerateSimpleDesignation(preciseProductBlank);
    preciseProductBlank.PreciseProductDesignation = str;
  }

  private PdmConfiguratorContext CreatePdmConfiguratorContext(
    PreciseProductBlank preciseProductBlank)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      PdmConfiguratorContextsCache configuratorContextsCache = new PdmConfiguratorContextsCache();
      PdmConfiguratorContext configuratorContext1 = new PdmConfiguratorContext((object) sessionKeeper.Session.GetRelation(preciseProductBlank.RelationID));
      configuratorContext1.Key = new RelationPair(0L, this._topObjectVersionID, this._topObjectTypeID, preciseProductBlank.RelationID);
      PdmConfiguratorContext configuratorContext2 = configuratorContext1;
      foreach (Tuple<long, long> tuple in preciseProductBlank.Context.Reverse<Tuple<long, long>>().Skip<Tuple<long, long>>(1))
      {
        PdmConfiguratorContext configuratorContext3 = new PdmConfiguratorContext((object) sessionKeeper.Session.GetRelation(tuple.Item1));
        configuratorContext3.Key = new RelationPair(0L, this._topObjectVersionID, this._topObjectTypeID, tuple.Item1);
        if (configuratorContext2.ContextsCache == null)
          configuratorContext2.ContextsCache = configuratorContextsCache;
        configuratorContext2.ContextsCache[configuratorContext3.Key] = configuratorContext3;
        configuratorContext2.ParentKey = configuratorContext3.Key;
        configuratorContext2 = configuratorContext3;
      }
      return configuratorContext1;
    }
  }

  private string GenerateSimpleDesignation(PreciseProductBlank preciseProductBlank)
  {
    string simpleDesignation = preciseProductBlank.ProductDesignation + this._separatorTextBox.Text;
    List<Tuple<long, long>> list = preciseProductBlank.Context.ToList<Tuple<long, long>>();
    list.Add(new Tuple<long, long>(preciseProductBlank.RelationID, preciseProductBlank.ProductVersionID));
    List<Guid> configuratorOptionGuids = this.GetVisiblePdmConfiguratorOptionGuids(preciseProductBlank.ProductVersionID);
    Dictionary<Guid, string> configuratorOptionValueIds = this.GetPdmConfiguratorOptionValueIds(list);
    foreach (Guid guid in configuratorOptionGuids)
    {
      OptionHolder option = PdmConfiguratorCache.CacheFindOption(guid);
      simpleDesignation += !string.IsNullOrEmpty(option.OptionCode) ? option.OptionCode : this._emptyOptionCodeReplacementTextBox.Text ?? "";
      simpleDesignation += this._optionAndOptionValuesSeparatorTextBox.Text;
      string str = (string) null;
      if (configuratorOptionValueIds.TryGetValue(guid, out str))
      {
        OptionValue optionValue = option.OptionValues.FindValue(str);
        simpleDesignation += !string.IsNullOrEmpty(optionValue.Code) ? optionValue.Code : this._emptyOptionValueCodeReplacementTextBox.Text ?? "";
      }
      if (guid != configuratorOptionGuids.Last<Guid>())
        simpleDesignation += this._optionSeparatorTextBox.Text;
    }
    return simpleDesignation;
  }

  private List<Guid> GetVisiblePdmConfiguratorOptionGuids(long assemblyVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return new ObjectOptionsHolder((object) sessionKeeper.Session.GetObject(assemblyVersionID)).VisibleOptionValues.Items.Keys.ToList<Guid>();
  }

  private Dictionary<Guid, string> GetPdmConfiguratorOptionValueIds(
    List<Tuple<long, long>> contextAndSelf)
  {
    Dictionary<Guid, string> configuratorOptionValueIds = new Dictionary<Guid, string>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (Tuple<long, long> tuple in contextAndSelf)
      {
        foreach (KeyValuePair<Guid, string> optionsValue in new PdmConfiguratorContext((object) sessionKeeper.Session.GetRelation(tuple.Item1)).OptionsValues)
        {
          if (!configuratorOptionValueIds.ContainsKey(optionsValue.Key))
            configuratorOptionValueIds.Add(optionsValue.Key, optionsValue.Value);
          else
            configuratorOptionValueIds[optionsValue.Key] = optionsValue.Value;
        }
      }
    }
    return configuratorOptionValueIds;
  }

  private void CheckPreciseProductsBlanks()
  {
    this._messageList.Messages.Clear();
    if (this._useExistProductsCheckBox.Checked)
    {
      PreciseProductBlank preciseProductBlank = this._preciseProductsBlanks.FirstOrDefault<PreciseProductBlank>((Func<PreciseProductBlank, bool>) (o => o.ProductVersionID == this._compositionPartID.Item2));
      if (preciseProductBlank != null && this.CheckObjectDesignationExist(preciseProductBlank.ProductObjectTypeID, preciseProductBlank.PreciseProductDesignation))
        this._messageList.Messages.Add((_Message) new CreatePreciseProductForm.PreciseProductBlankMessage(_MessageType.Error, $"Невозможно создать точное изделие. Обозначение {preciseProductBlank.PreciseProductDesignation} не является уникальным", preciseProductBlank));
    }
    else
    {
      foreach (PreciseProductBlank preciseProductsBlank in (Collection<PreciseProductBlank>) this._preciseProductsBlanks)
      {
        if (this.CheckObjectDesignationExist(preciseProductsBlank.ProductObjectTypeID, preciseProductsBlank.PreciseProductDesignation))
          this._messageList.Messages.Add((_Message) new CreatePreciseProductForm.PreciseProductBlankMessage(_MessageType.Error, $"Невозможно создать точное изделие. Обозначение {preciseProductsBlank.PreciseProductDesignation} не является уникальным", preciseProductsBlank));
      }
    }
    if (this._archive != null)
      return;
    this._messageList.Messages.Add(new _Message(_MessageType.Error, "Не выбран архив спецификации"));
  }

  private bool CheckObjectDesignationExist(int objectTypeID, string designation)
  {
    return !ObjectHelper.IsUnknownObjectVersionID(ServiceLocator.Get<IPDMSpecificationsService>().GetObjectWithDesignation(objectTypeID, designation));
  }

  private void OpenInNewWindow()
  {
    foreach (PreciseProductBlank selectedItem in (IEnumerable) this._treeView.SelectedItems)
    {
      long num = 0;
      this._createPreciseProductResult.CreatedPreciseProductVersionIDDictionaryByCompositionPartID.TryGetValue(new Tuple<long, long>(selectedItem.RelationID, selectedItem.ProductVersionID), out num);
      if (!ObjectHelper.IsUnknownObjectVersionID(num))
        Utils.OpenNewWindow((IDescriptor) new Intermech.Navigator.DBObjects.Descriptor(num), (IServiceProvider) ServicesManager.ServiceContainer);
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CreatePreciseProductForm));
    this._createButton = new Button();
    this._keepCheckedOutCreatedObjects = new CheckBox();
    this._copyDocumentionCheckBox = new CheckBox();
    this._treeView = new Intermech.VirtualTreeView.VirtualTreeView();
    this._relationIDColumn = new Column();
    this._productVersionIDColumn = new Column();
    this._productCaptionColumn = new Column();
    this._preciseProductDesignationColumn = new Column();
    this._preciseProductNameColumn = new Column();
    this._contextMenuStrip = new ContextMenuStrip(this.components);
    this._openInNewWindowToolStripMenuItem = new ToolStripMenuItem();
    this.label2 = new Label();
    this._generateDesignationsButton = new Button();
    this._separatorTextBox = new TextBox();
    this.splitContainer1 = new SplitContainer();
    this.tabControl1 = new TabControl();
    this.tabPage1 = new TabPage();
    this._toolStrip = new ToolStrip();
    this._openInNewWindowToolStripButton = new ToolStripButton();
    this.tabPage2 = new TabPage();
    this.label5 = new Label();
    this._emptyOptionValueCodeReplacementTextBox = new TextBox();
    this._emptyOptionCodeReplacementTextBox = new TextBox();
    this.label4 = new Label();
    this.label3 = new Label();
    this._optionAndOptionValuesSeparatorTextBox = new TextBox();
    this.label1 = new Label();
    this._useConfigurationCodeCheckBox = new CheckBox();
    this._optionSeparatorTextBox = new TextBox();
    this.label = new Label();
    this.tabPage3 = new TabPage();
    this._selectArchiveButton = new Button();
    this._archiveTextBox = new TextBox();
    this.tabPage4 = new TabPage();
    this._messageList = new MessageList();
    this._closeButton = new Button();
    this._useExistProductsCheckBox = new CheckBox();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.flowLayoutPanel1 = new FlowLayoutPanel();
    this._treeView.BeginInit();
    this._contextMenuStrip.SuspendLayout();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.tabControl1.SuspendLayout();
    this.tabPage1.SuspendLayout();
    this._toolStrip.SuspendLayout();
    this.tabPage2.SuspendLayout();
    this.tabPage3.SuspendLayout();
    this.tabPage4.SuspendLayout();
    this.tableLayoutPanel1.SuspendLayout();
    this.flowLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this._createButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._createButton.AutoSize = true;
    this._createButton.Location = new Point(989, 3);
    this._createButton.Name = "_createButton";
    this._createButton.Size = new Size(75, 23);
    this._createButton.TabIndex = 0;
    this._createButton.Text = "Создать";
    this._createButton.UseVisualStyleBackColor = true;
    this._createButton.Click += new EventHandler(this.CreateButton_Click);
    this._keepCheckedOutCreatedObjects.AutoSize = true;
    this._keepCheckedOutCreatedObjects.Location = new Point(3, 11);
    this._keepCheckedOutCreatedObjects.Name = "_keepCheckedOutCreatedObjects";
    this._keepCheckedOutCreatedObjects.Size = new Size(308, 17);
    this._keepCheckedOutCreatedObjects.TabIndex = 13;
    this._keepCheckedOutCreatedObjects.Text = "Оставлять созданные объекты взятыми на изменение";
    this._keepCheckedOutCreatedObjects.UseVisualStyleBackColor = true;
    this._copyDocumentionCheckBox.AutoSize = true;
    this._copyDocumentionCheckBox.Location = new Point(3, 34);
    this._copyDocumentionCheckBox.Name = "_copyDocumentionCheckBox";
    this._copyDocumentionCheckBox.Size = new Size(163, 17);
    this._copyDocumentionCheckBox.TabIndex = 16 /*0x10*/;
    this._copyDocumentionCheckBox.Text = "Копировать документацию";
    this._copyDocumentionCheckBox.UseVisualStyleBackColor = true;
    this._treeView.AllowDrop = true;
    this._treeView.Columns.Add(this._relationIDColumn);
    this._treeView.Columns.Add(this._productVersionIDColumn);
    this._treeView.Columns.Add(this._productCaptionColumn);
    this._treeView.Columns.Add(this._preciseProductDesignationColumn);
    this._treeView.Columns.Add(this._preciseProductNameColumn);
    this._treeView.ContextMenuStrip = this._contextMenuStrip;
    this._treeView.DisableHeaderContextMenu = false;
    this._treeView.Dock = DockStyle.Fill;
    this._treeView.ImageList = (ImageList) null;
    this._treeView.Location = new Point(3, 28);
    this._treeView.Name = "_treeView";
    this._treeView.ShowRootRow = false;
    this._treeView.Size = new Size(1134, 190);
    this._treeView.TabIndex = 17;
    this._treeView.UseThemedHeaders = false;
    this._treeView.SelectionChanged += new EventHandler(this.TreeView_SelectionChanged);
    this._relationIDColumn.Caption = "Идентификатор связи";
    this._relationIDColumn.Name = "_relationIDColumn";
    this._productVersionIDColumn.Caption = "Идентификатор версии изделия";
    this._productVersionIDColumn.Name = "_productVersionIDColumn";
    this._productCaptionColumn.Caption = "Заголовок изделия";
    this._productCaptionColumn.Name = "_productCaptionColumn";
    this._productCaptionColumn.Width = 200;
    this._preciseProductDesignationColumn.Caption = "Обозначение точного изделия";
    this._preciseProductDesignationColumn.Name = "_preciseProductDesignationColumn";
    this._preciseProductDesignationColumn.Width = 200;
    this._preciseProductNameColumn.Caption = "Наименование точного изделия";
    this._preciseProductNameColumn.Name = "_preciseProductNameColumn";
    this._preciseProductNameColumn.Width = 200;
    this._contextMenuStrip.Items.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this._openInNewWindowToolStripMenuItem
    });
    this._contextMenuStrip.Name = "_contextMenuStrip";
    this._contextMenuStrip.Size = new Size(199, 26);
    this._openInNewWindowToolStripMenuItem.Name = "_openInNewWindowToolStripMenuItem";
    this._openInNewWindowToolStripMenuItem.Size = new Size(198, 22);
    this._openInNewWindowToolStripMenuItem.Text = "Открыть в новом окне";
    this._openInNewWindowToolStripMenuItem.ToolTipText = "Открыть в новом окне";
    this._openInNewWindowToolStripMenuItem.Click += new EventHandler(this.OpenInNewWindowToolStripMenuItem_Click);
    this.label2.AutoSize = true;
    this.label2.Location = new Point(6, 46);
    this.label2.Name = "label2";
    this.label2.Size = new Size(225, 13);
    this.label2.TabIndex = 19;
    this.label2.Text = "Разделитель обозначения и списка опций:";
    this._generateDesignationsButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._generateDesignationsButton.Location = new Point(1036, 198);
    this._generateDesignationsButton.Name = "_generateDesignationsButton";
    this._generateDesignationsButton.Size = new Size(90, 23);
    this._generateDesignationsButton.TabIndex = 2;
    this._generateDesignationsButton.Text = "Генерировать";
    this._generateDesignationsButton.UseVisualStyleBackColor = true;
    this._generateDesignationsButton.Click += new EventHandler(this.GenerateDesignationsButton_Click);
    this._separatorTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this._separatorTextBox.Location = new Point(279, 43);
    this._separatorTextBox.Name = "_separatorTextBox";
    this._separatorTextBox.Size = new Size(847, 20);
    this._separatorTextBox.TabIndex = 0;
    this._separatorTextBox.Text = "|";
    this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.splitContainer1.Location = new Point(3, 3);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Orientation = Orientation.Horizontal;
    this.splitContainer1.Panel1.Controls.Add((Control) this.tabControl1);
    this.splitContainer1.Panel2.Controls.Add((Control) this._messageList);
    this.splitContainer1.Size = new Size(1148, 519);
    this.splitContainer1.SplitterDistance = 247;
    this.splitContainer1.TabIndex = 20;
    this.tabControl1.Controls.Add((Control) this.tabPage1);
    this.tabControl1.Controls.Add((Control) this.tabPage2);
    this.tabControl1.Controls.Add((Control) this.tabPage3);
    this.tabControl1.Controls.Add((Control) this.tabPage4);
    this.tabControl1.Dock = DockStyle.Fill;
    this.tabControl1.Location = new Point(0, 0);
    this.tabControl1.Name = "tabControl1";
    this.tabControl1.SelectedIndex = 0;
    this.tabControl1.Size = new Size(1148, 247);
    this.tabControl1.TabIndex = 22;
    this.tabPage1.Controls.Add((Control) this._treeView);
    this.tabPage1.Controls.Add((Control) this._toolStrip);
    this.tabPage1.Location = new Point(4, 22);
    this.tabPage1.Name = "tabPage1";
    this.tabPage1.Padding = new Padding(3);
    this.tabPage1.Size = new Size(1140, 221);
    this.tabPage1.TabIndex = 0;
    this.tabPage1.Text = "Заготовки точных изделий";
    this.tabPage1.UseVisualStyleBackColor = true;
    this._toolStrip.Items.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this._openInNewWindowToolStripButton
    });
    this._toolStrip.Location = new Point(3, 3);
    this._toolStrip.Name = "_toolStrip";
    this._toolStrip.Size = new Size(1134, 25);
    this._toolStrip.TabIndex = 18;
    this._toolStrip.Text = "toolStrip1";
    this._openInNewWindowToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._openInNewWindowToolStripButton.Image = (Image) componentResourceManager.GetObject("_openInNewWindowToolStripButton.Image");
    this._openInNewWindowToolStripButton.ImageTransparentColor = Color.Magenta;
    this._openInNewWindowToolStripButton.Name = "_openInNewWindowToolStripButton";
    this._openInNewWindowToolStripButton.Size = new Size(23, 22);
    this._openInNewWindowToolStripButton.Text = "Открыть в новом окне";
    this._openInNewWindowToolStripButton.Click += new EventHandler(this.OpenInNewWindowToolStripButton_Click);
    this.tabPage2.Controls.Add((Control) this.label5);
    this.tabPage2.Controls.Add((Control) this._emptyOptionValueCodeReplacementTextBox);
    this.tabPage2.Controls.Add((Control) this._emptyOptionCodeReplacementTextBox);
    this.tabPage2.Controls.Add((Control) this.label4);
    this.tabPage2.Controls.Add((Control) this.label3);
    this.tabPage2.Controls.Add((Control) this._optionAndOptionValuesSeparatorTextBox);
    this.tabPage2.Controls.Add((Control) this.label1);
    this.tabPage2.Controls.Add((Control) this._useConfigurationCodeCheckBox);
    this.tabPage2.Controls.Add((Control) this._optionSeparatorTextBox);
    this.tabPage2.Controls.Add((Control) this.label);
    this.tabPage2.Controls.Add((Control) this._generateDesignationsButton);
    this.tabPage2.Controls.Add((Control) this.label2);
    this.tabPage2.Controls.Add((Control) this._separatorTextBox);
    this.tabPage2.Location = new Point(4, 22);
    this.tabPage2.Name = "tabPage2";
    this.tabPage2.Padding = new Padding(3);
    this.tabPage2.Size = new Size(1132, 227);
    this.tabPage2.TabIndex = 1;
    this.tabPage2.Text = "Генерация обозначений";
    this.tabPage2.UseVisualStyleBackColor = true;
    this.label5.AutoSize = true;
    this.label5.Location = new Point(6, 19);
    this.label5.Name = "label5";
    this.label5.Size = new Size(1106, 13);
    this.label5.TabIndex = 27;
    this.label5.Text = componentResourceManager.GetString("label5.Text");
    this._emptyOptionValueCodeReplacementTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this._emptyOptionValueCodeReplacementTextBox.Location = new Point(279, 144 /*0x90*/);
    this._emptyOptionValueCodeReplacementTextBox.Name = "_emptyOptionValueCodeReplacementTextBox";
    this._emptyOptionValueCodeReplacementTextBox.Size = new Size(847, 20);
    this._emptyOptionValueCodeReplacementTextBox.TabIndex = 26;
    this._emptyOptionValueCodeReplacementTextBox.Text = "{Код значения опции не указан}";
    this._emptyOptionCodeReplacementTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this._emptyOptionCodeReplacementTextBox.Location = new Point(279, 118);
    this._emptyOptionCodeReplacementTextBox.Name = "_emptyOptionCodeReplacementTextBox";
    this._emptyOptionCodeReplacementTextBox.Size = new Size(847, 20);
    this._emptyOptionCodeReplacementTextBox.TabIndex = 26;
    this._emptyOptionCodeReplacementTextBox.Text = "{Код опции не указан}";
    this.label4.AutoSize = true;
    this.label4.Location = new Point(6, 147);
    this.label4.Name = "label4";
    this.label4.Size = new Size(224 /*0xE0*/, 13);
    this.label4.TabIndex = 25;
    this.label4.Text = "Заменитель пустого кода значения опции:";
    this.label3.AutoSize = true;
    this.label3.Location = new Point(6, 121);
    this.label3.Name = "label3";
    this.label3.Size = new Size(174, 13);
    this.label3.TabIndex = 25;
    this.label3.Text = "Заменитель пустого кода опции:";
    this._optionAndOptionValuesSeparatorTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this._optionAndOptionValuesSeparatorTextBox.Location = new Point(279, 92);
    this._optionAndOptionValuesSeparatorTextBox.Name = "_optionAndOptionValuesSeparatorTextBox";
    this._optionAndOptionValuesSeparatorTextBox.Size = new Size(847, 20);
    this._optionAndOptionValuesSeparatorTextBox.TabIndex = 24;
    this._optionAndOptionValuesSeparatorTextBox.Text = "-";
    this.label1.AutoSize = true;
    this.label1.Location = new Point(6, 95);
    this.label1.Name = "label1";
    this.label1.Size = new Size((int) byte.MaxValue, 13);
    this.label1.TabIndex = 23;
    this.label1.Text = "Разделитель кода опции и кода значения опции:";
    this._useConfigurationCodeCheckBox.AutoSize = true;
    this._useConfigurationCodeCheckBox.Location = new Point(6, 180);
    this._useConfigurationCodeCheckBox.Name = "_useConfigurationCodeCheckBox";
    this._useConfigurationCodeCheckBox.Size = new Size(130, 17);
    this._useConfigurationCodeCheckBox.TabIndex = 22;
    this._useConfigurationCodeCheckBox.Text = "Использовать шифр";
    this._useConfigurationCodeCheckBox.UseVisualStyleBackColor = true;
    this._useConfigurationCodeCheckBox.CheckedChanged += new EventHandler(this.UseConfigurationCodeCheckBox_CheckedChanged);
    this._optionSeparatorTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this._optionSeparatorTextBox.Location = new Point(279, 68);
    this._optionSeparatorTextBox.Name = "_optionSeparatorTextBox";
    this._optionSeparatorTextBox.Size = new Size(847, 20);
    this._optionSeparatorTextBox.TabIndex = 21;
    this._optionSeparatorTextBox.Text = "|";
    this.label.AutoSize = true;
    this.label.Location = new Point(6, 71);
    this.label.Name = "label";
    this.label.Size = new Size(109, 13);
    this.label.TabIndex = 20;
    this.label.Text = "Разделитель опций:";
    this.tabPage3.Controls.Add((Control) this._selectArchiveButton);
    this.tabPage3.Controls.Add((Control) this._archiveTextBox);
    this.tabPage3.Location = new Point(4, 22);
    this.tabPage3.Name = "tabPage3";
    this.tabPage3.Padding = new Padding(3);
    this.tabPage3.Size = new Size(1132, 227);
    this.tabPage3.TabIndex = 2;
    this.tabPage3.Text = "Архив спецификации";
    this.tabPage3.UseVisualStyleBackColor = true;
    this._selectArchiveButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this._selectArchiveButton.Location = new Point(1041, 4);
    this._selectArchiveButton.Name = "_selectArchiveButton";
    this._selectArchiveButton.Size = new Size(75, 23);
    this._selectArchiveButton.TabIndex = 1;
    this._selectArchiveButton.Text = "Выбрать";
    this._selectArchiveButton.UseVisualStyleBackColor = true;
    this._selectArchiveButton.Click += new EventHandler(this.SelectArchiveButton_Click);
    this._archiveTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this._archiveTextBox.Location = new Point(6, 6);
    this._archiveTextBox.Name = "_archiveTextBox";
    this._archiveTextBox.Size = new Size(1026, 20);
    this._archiveTextBox.TabIndex = 0;
    this.tabPage4.Controls.Add((Control) this._keepCheckedOutCreatedObjects);
    this.tabPage4.Controls.Add((Control) this._copyDocumentionCheckBox);
    this.tabPage4.Location = new Point(4, 22);
    this.tabPage4.Name = "tabPage4";
    this.tabPage4.Padding = new Padding(3);
    this.tabPage4.Size = new Size(1132, 227);
    this.tabPage4.TabIndex = 3;
    this.tabPage4.Text = "Настройки";
    this.tabPage4.UseVisualStyleBackColor = true;
    this._messageList.Dock = DockStyle.Fill;
    this._messageList.Location = new Point(0, 0);
    this._messageList.Name = "_messageList";
    this._messageList.Size = new Size(1148, 268);
    this._messageList.TabIndex = 19;
    this._closeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._closeButton.AutoSize = true;
    this._closeButton.Location = new Point(1070, 3);
    this._closeButton.Name = "_closeButton";
    this._closeButton.Size = new Size(75, 23);
    this._closeButton.TabIndex = 21;
    this._closeButton.Text = "Закрыть";
    this._closeButton.UseVisualStyleBackColor = true;
    this._closeButton.Click += new EventHandler(this.CloseButton_Click);
    this._useExistProductsCheckBox.AutoSize = true;
    this._useExistProductsCheckBox.Dock = DockStyle.Fill;
    this._useExistProductsCheckBox.Location = new Point(3, 528);
    this._useExistProductsCheckBox.Name = "_useExistProductsCheckBox";
    this._useExistProductsCheckBox.Size = new Size(1148, 17);
    this._useExistProductsCheckBox.TabIndex = 22;
    this._useExistProductsCheckBox.Text = "При совпадении обозначений подсборок связывать создаваемые сборки с уже существующими подсборками";
    this._useExistProductsCheckBox.UseVisualStyleBackColor = true;
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this._useExistProductsCheckBox, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.flowLayoutPanel1, 0, 2);
    this.tableLayoutPanel1.Controls.Add((Control) this.splitContainer1, 0, 0);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 3;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.Size = new Size(1154, 583);
    this.tableLayoutPanel1.TabIndex = 23;
    this.flowLayoutPanel1.AutoSize = true;
    this.flowLayoutPanel1.Controls.Add((Control) this._closeButton);
    this.flowLayoutPanel1.Controls.Add((Control) this._createButton);
    this.flowLayoutPanel1.Dock = DockStyle.Fill;
    this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
    this.flowLayoutPanel1.Location = new Point(3, 551);
    this.flowLayoutPanel1.Name = "flowLayoutPanel1";
    this.flowLayoutPanel1.Size = new Size(1148, 29);
    this.flowLayoutPanel1.TabIndex = 23;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(1154, 583);
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (CreatePreciseProductForm);
    this.ShowIcon = false;
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Создание точных изделий";
    this._treeView.EndInit();
    this._contextMenuStrip.ResumeLayout(false);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.tabControl1.ResumeLayout(false);
    this.tabPage1.ResumeLayout(false);
    this.tabPage1.PerformLayout();
    this._toolStrip.ResumeLayout(false);
    this._toolStrip.PerformLayout();
    this.tabPage2.ResumeLayout(false);
    this.tabPage2.PerformLayout();
    this.tabPage3.ResumeLayout(false);
    this.tabPage3.PerformLayout();
    this.tabPage4.ResumeLayout(false);
    this.tabPage4.PerformLayout();
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.flowLayoutPanel1.ResumeLayout(false);
    this.flowLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }

  private sealed class PreciseProductBlankRowBinding : ObjectRowBinding
  {
    public PreciseProductBlankRowBinding()
      : base(typeof (PreciseProductBlank))
    {
    }

    public override void GetRowData(Row row, RowData rowData)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (rowData == null)
        throw new ArgumentNullException(nameof (rowData));
      base.GetRowData(row, rowData);
      if (!(row.Item is PreciseProductBlank preciseProductBlank) || !(ServicesManager.GetService(typeof (ICategoryTypeIconService)) is ICategoryTypeIconService service))
        return;
      rowData.ImageList = service.ImageList;
      rowData.ImageIndex = service.IndexOf(4, preciseProductBlank.ProductObjectTypeID);
    }
  }

  private sealed class PreciseProductBlankMessage : _Message
  {
    public PreciseProductBlankMessage(
      _MessageType type,
      string text,
      PreciseProductBlank preciseProductBlank)
      : base(type, text)
    {
      this.PreciseProductBlank = preciseProductBlank;
    }

    public PreciseProductBlank PreciseProductBlank { get; private set; }
  }
}

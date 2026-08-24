// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.ConfigMainEditorView
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 0BD7AB18-9725-4F3A-95EA-AF9537E2626A
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.dll

using Infralution.Controls.VirtualTree;
using Intermech.Bars;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ParamValueConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Utils;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.Resources;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.Services;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor;

public class ConfigMainEditorView : UserControl
{
  private ServiceContainer _globalServices = new ServiceContainer();
  private TechcardToIpsConfig _config;
  private IContainer components;
  private Infralution.Controls.VirtualTree.VirtualTree vtvConfig;
  private ImageList Images;
  private SplitContainer mainPanel;
  private OpenFileDialog dlgOpenFile;
  private Column column1;
  private Panel pnlConfigEditor;
  private SaveFileDialog dlgSaveFile;
  private ButtonItem btnSaveConfig;
  private ButtonItem btnOpenConfig;
  private Intermech.Bars.ToolBar tbMain;
  private Intermech.Bars.ToolBar tbChanges;
  private ButtonItem btnAdd;
  private ButtonItem btnApply;
  private ButtonItem btnCancel;
  private Intermech.Actions.Action action1;
  private ButtonItem btnDelete;

  public ConfigMainEditorView()
  {
    this.InitializeComponent();
    this.InitServices();
    this.UpdateCommandStates();
  }

  private void btnOpenConfig_Click(object sender, EventArgs e)
  {
    this._globalServices.RemoveService<TechcardToIpsConfig>();
    if (this.dlgOpenFile.ShowDialog() == DialogResult.OK)
    {
      this._config = this.LoadConfig(this.dlgOpenFile.FileName);
      if (this._config != null)
        this._globalServices.AddService<TechcardToIpsConfig>(this._config);
      this.BuildConfigTree(this._config);
    }
    else
      this._config = (TechcardToIpsConfig) null;
    this.UpdateCommandStates();
  }

  private void BuildConfigTree(TechcardToIpsConfig config)
  {
    this.vtvConfig.DataSource = (object) null;
    this.vtvConfig.DataSource = (object) config;
  }

  private void InitServices()
  {
    IpsXmlLogger service = new IpsXmlLogger(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "convert.log"));
    service.Clear();
    this._globalServices.AddService<IpsXmlLogger>(service);
    service.LoggerConfig.MessageTypes = LogMessageTypes.Warn | LogMessageTypes.Error;
    service.Clear();
    this._globalServices.AddService<EditorViewService>(new EditorViewService((IServiceProvider) this._globalServices));
    this._globalServices.AddService<TechCardToIpsConfigLoader>(new TechCardToIpsConfigLoader((IServiceProvider) this._globalServices));
    this._globalServices.AddService<TechcardToIpsConfigSerializer>(new TechcardToIpsConfigSerializer((IServiceProvider) this._globalServices));
  }

  private void vtvConfig_GetChildren(object sender, GetChildrenEventArgs e)
  {
    switch (e.Row.Item)
    {
      case TechcardToIpsConfig _:
        e.Children = (IList) new ArrayList()
        {
          (object) this._config.LoggerConfig,
          (object) this._config.OutputConfig,
          (object) this._config.ConstValueConfigs,
          (object) this._config.ObjectConfigs,
          (object) this._config.ValueConverterConfigs,
          (object) this._config.IdConfigs
        };
        break;
      case Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig idConfig:
        e.Children = (IList) new ArrayList()
        {
          (object) idConfig.Content
        };
        break;
      case IdPartGroup idPartGroup:
        ArrayList arrayList1 = new ArrayList();
        arrayList1.AddRange((ICollection) idPartGroup.Content);
        e.Children = (IList) arrayList1;
        break;
      case ParamConfig paramConfig:
        e.Children = (IList) new ArrayList()
        {
          (object) paramConfig.ValueConfigs,
          (object) paramConfig.ConvertStrategies
        };
        break;
      case BaseConvertableEntityConfig convertableEntityConfig:
        ArrayList arrayList2 = new ArrayList();
        if (convertableEntityConfig is ObjectConfig objectConfig)
        {
          arrayList2.Add((object) objectConfig.UniqueControlRule);
          arrayList2.Add((object) objectConfig.ConvertationRules);
          arrayList2.Add((object) objectConfig.ParamConfigs);
          arrayList2.Add((object) objectConfig.RelationConfigs);
          arrayList2.Add((object) objectConfig.ConvertStrategies);
        }
        if (convertableEntityConfig is RelationConfig relationConfig)
        {
          arrayList2.Add((object) relationConfig.UniqueControlRule);
          arrayList2.Add((object) relationConfig.ConvertationRules);
          arrayList2.Add((object) relationConfig.ParamConfigs);
          arrayList2.Add((object) relationConfig.ConvertStrategies);
        }
        e.Children = (IList) arrayList2;
        break;
      case Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig.ValueConverterConfig valueConverterConfig:
        e.Children = (IList) valueConverterConfig.OriginValueConfigs;
        break;
      case OriginValueConfig complexConfig:
        if (complexConfig.Count >= 1 && string.IsNullOrEmpty(complexConfig[complexConfig.Ids.First<string>()].Context))
        {
          e.Children = (IList) null;
          break;
        }
        IReadOnlyList<BaseConfig> fromComplexConfig1 = ConfigTypesUtils.GetChildsFromComplexConfig((BaseConfig) complexConfig);
        e.Children = fromComplexConfig1 != null ? (IList) new ArrayList((ICollection) fromComplexConfig1.ToList<BaseConfig>()) : (IList) null;
        break;
      case BaseConfig baseConfig:
        if (ConfigTypesUtils.IsComplexConfig(baseConfig))
        {
          IReadOnlyList<BaseConfig> fromComplexConfig2 = ConfigTypesUtils.GetChildsFromComplexConfig(baseConfig);
          e.Children = fromComplexConfig2 != null ? (IList) new ArrayList((ICollection) fromComplexConfig2.ToList<BaseConfig>()) : (IList) null;
          break;
        }
        e.Children = (IList) null;
        break;
      case ParamConfigs paramConfigs:
        ArrayList arrayList3 = new ArrayList();
        IReadOnlyList<BaseConfig> fromComplexConfig3 = ConfigTypesUtils.GetChildsFromComplexConfig((BaseConfig) paramConfigs.ConstConfigs);
        if (fromComplexConfig3 != null)
          arrayList3.AddRange((ICollection) fromComplexConfig3.ToList<BaseConfig>());
        IReadOnlyList<BaseConfig> fromComplexConfig4 = ConfigTypesUtils.GetChildsFromComplexConfig((BaseConfig) paramConfigs.SimpleConfigs);
        if (fromComplexConfig4 != null)
          arrayList3.AddRange((ICollection) fromComplexConfig4.ToList<BaseConfig>());
        IReadOnlyList<BaseConfig> fromComplexConfig5 = ConfigTypesUtils.GetChildsFromComplexConfig((BaseConfig) paramConfigs.CalcConfigs);
        if (fromComplexConfig5 != null)
          arrayList3.AddRange((ICollection) fromComplexConfig5.ToList<BaseConfig>());
        e.Children = (IList) arrayList3;
        break;
    }
  }

  private void vtvConfig_GetCellData(object sender, GetCellDataEventArgs e)
  {
    switch (e.Row.Item)
    {
      case TechcardToIpsConfig _:
        e.CellData.Value = (object) LocalizationHolder.rm.GetString("msgSettings");
        break;
      case Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.LoggerConfig _:
        e.CellData.Value = (object) LocalizationHolder.rm.GetString("msgLogConfig");
        break;
      case OutputConfig _:
        e.CellData.Value = (object) LocalizationHolder.rm.GetString("msgOutputConfig");
        break;
      case ConstValueConfigs _:
        e.CellData.Value = (object) LocalizationHolder.rm.GetString("msgConstValuesConfig");
        break;
      case ConstValueConfig config1:
        e.CellData.Value = this.GetBaseConfigDescription((BaseConfig) config1);
        break;
      case ObjectConfigs _:
        e.CellData.Value = (object) LocalizationHolder.rm.GetString("msgObjectsConfig");
        break;
      case ConvertStrategyConfigs _:
        e.CellData.Value = (object) LocalizationHolder.rm.GetString("msgConverterStrategyConfigs");
        break;
      case ConvertStrategyConfig config2:
        e.CellData.Value = this.GetBaseConfigDescription((BaseConfig) config2);
        break;
      case UniqueControlRuleConfig _:
        e.CellData.Value = (object) LocalizationHolder.rm.GetString("msgUniqueControlRuleConfig");
        break;
      case ConvertationRulesConfig _:
        e.CellData.Value = (object) LocalizationHolder.rm.GetString("msgConvertationRulesConfig");
        break;
      case RelationConfigs _:
        e.CellData.Value = (object) LocalizationHolder.rm.GetString("msgRelationConfigs");
        break;
      case RelationConfig config3:
        e.CellData.Value = this.GetBaseConfigDescription((BaseConfig) config3);
        break;
      case ParamConfigs _:
        e.CellData.Value = (object) LocalizationHolder.rm.GetString("msgParamConfigs");
        break;
      case ParamConfig config4:
        e.CellData.Value = this.GetBaseConfigDescription((BaseConfig) config4);
        break;
      case ObjectConfig config5:
        e.CellData.Value = this.GetBaseConfigDescription((BaseConfig) config5);
        break;
      case ValueConverterConfigs _:
        e.CellData.Value = (object) LocalizationHolder.rm.GetString("msgValueConvertersConfig");
        break;
      case Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig.ValueConverterConfig config6:
        e.CellData.Value = this.GetBaseConfigDescription((BaseConfig) config6);
        break;
      case OriginValueConfig config7:
        e.CellData.Value = (object) $"{this.GetBaseConfigDescription((BaseConfig) config7)}({config7.Value})";
        break;
      case IdConfigs _:
        e.CellData.Value = (object) LocalizationHolder.rm.GetString("msgIdConfigs");
        break;
      case Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig config8:
        e.CellData.Value = (object) $"{this.GetBaseConfigDescription((BaseConfig) config8)}; {LocalizationHolder.rm.GetString("msgType")}{config8.Type.ToXMLTag()}; {LocalizationHolder.rm.GetString("msgCalcResultType")}{config8.CalcResultType.ToXMLTag()}";
        break;
      case IdPartGroup config9:
        e.CellData.Value = (object) $"{this.GetBaseConfigDescription((BaseConfig) config9)}; {LocalizationHolder.rm.GetString("msgCondition")}{config9.Condition.ToXMLTag()}";
        break;
      case BaseIdPart config10:
        e.CellData.Value = this.GetBaseConfigDescription((BaseConfig) config10);
        break;
      case ValueConfigs _:
        e.CellData.Value = (object) LocalizationHolder.rm.GetString("msgValueConfigs");
        break;
      case ValueConfig config11:
        e.CellData.Value = this.GetBaseConfigDescription((BaseConfig) config11);
        break;
      default:
        e.CellData.Value = (object) $"{LocalizationHolder.rm.GetString("msgUnsupportedConfig")}: {e.Row.Item.ToString()}";
        break;
    }
  }

  private object GetBaseConfigDescription(BaseConfig config)
  {
    if (!string.IsNullOrEmpty(config.Description))
      return (object) config.Description;
    return string.IsNullOrEmpty(config.Id) ? (object) config.Name : (object) config.Id;
  }

  private void vtvConfig_SelectionChanged(object sender, EventArgs e)
  {
    if (this.vtvConfig.SelectedItem is BaseConfig selectedItem)
    {
      BaseConfig targetParentConfig = this.vtvConfig.SelectedRow.ParentRow == null || !(this.vtvConfig.SelectedRow.ParentRow.Item is BaseConfig baseConfig) ? (BaseConfig) null : baseConfig;
      this._globalServices.GetService<EditorViewService>().EditConfig(selectedItem, targetParentConfig, (Control) this.pnlConfigEditor, new EventHandler<bool>(this.OnDataChanged));
    }
    else
      this.ClearConfigEditorPanel();
  }

  private void ClearConfigEditorPanel()
  {
    this.pnlConfigEditor.Controls.Clear();
    this.btnApply.Enabled = false;
    this.btnCancel.Enabled = false;
    this.UpdateChangeStateButtons(false);
  }

  private void OnDataChanged(object sender, bool dataChanged)
  {
    this.UpdateChangeStateButtons(dataChanged);
  }

  private void UpdateChangeStateButtons(bool dataChanged)
  {
    this.btnApply.Enabled = dataChanged;
    this.btnCancel.Enabled = dataChanged;
    this.UpdateCommandStates();
  }

  private void UpdateCommandStates()
  {
    this.btnAdd.Enabled = !this.btnApply.Enabled && this._config != null;
    this.btnDelete.Enabled = !this.btnApply.Enabled && this._config != null;
    this.btnOpenConfig.Enabled = !this.btnApply.Enabled;
    this.btnSaveConfig.Enabled = this._config != null;
  }

  private TechcardToIpsConfig LoadConfig(string configFileName)
  {
    return this._globalServices.GetService<TechCardToIpsConfigLoader>().LoadConfig(configFileName);
  }

  private void btnSaveConfig_Click(object sender, EventArgs e)
  {
    if (this.btnApply.Enabled)
      this.btnApply.PerformClick();
    if (this.dlgSaveFile.ShowDialog() != DialogResult.OK)
      return;
    this._globalServices.GetService<TechcardToIpsConfigSerializer>().SerializeConfig(this._config, this.dlgSaveFile.FileName);
  }

  private void btnAdd_Click(object sender, EventArgs e)
  {
    switch (this.vtvConfig.SelectedItem)
    {
      case ConstValueConfig _:
      case ConstValueConfigs _:
        this.AddConstValue(this.vtvConfig.SelectedRow);
        break;
    }
  }

  private void AddConstValue(Row selectedRow)
  {
    ConstValueConfig constValueConfig = new ConstValueConfig();
    constValueConfig.Id = Guid.NewGuid().ToString();
    constValueConfig.Name = constValueConfig.Id;
    this._config.ConstValueConfigs[constValueConfig.Id] = constValueConfig;
    if (selectedRow.Item is ConstValueConfig)
    {
      selectedRow.ParentRow.UpdateChildren(true, false);
      this.vtvConfig.SelectedRow = selectedRow.ParentRow.ChildRow((object) constValueConfig);
    }
    else
    {
      selectedRow.UpdateChildren(true, false);
      this.vtvConfig.SelectedRow = selectedRow.ChildRow((object) constValueConfig);
    }
  }

  private void btnApply_Click(object sender, EventArgs e)
  {
    if (!this._globalServices.GetService<EditorViewService>().ApplyChanges())
      return;
    this.vtvConfig.UpdateRowData(this.vtvConfig.SelectedRow);
    this.UpdateChangeStateButtons(false);
  }

  private void btnCancel_Click(object sender, EventArgs e)
  {
    this._globalServices.GetService<EditorViewService>().CancelChanges();
    this.UpdateChangeStateButtons(false);
  }

  private void vtvConfig_SelectionChanging(object sender, SelectionChangingEventArgs e)
  {
    e.Cancel = this.btnApply.Enabled;
  }

  private void btnDelete_Click(object sender, EventArgs e)
  {
    if (MessageBox.Show(LocalizationHolder.rm.GetString("msgAskToDeleteConfig"), LocalizationHolder.rm.GetString("cptAppName"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
      return;
    HashSet<Row> rowSet = new HashSet<Row>();
    foreach (Row selectedRow in this.vtvConfig.SelectedRows)
    {
      if (selectedRow.Item is ConstValueConfig constValueConfig)
        this._config.ConstValueConfigs.Remove(constValueConfig.Id);
      if (!rowSet.Contains(selectedRow.ParentRow))
        rowSet.Add(selectedRow.ParentRow);
    }
    foreach (Row row in rowSet)
      row.UpdateChildren(true, false);
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ConfigMainEditorView));
    this.Images = new ImageList(this.components);
    this.mainPanel = new SplitContainer();
    this.vtvConfig = new Infralution.Controls.VirtualTree.VirtualTree();
    this.column1 = new Column();
    this.tbMain = new Intermech.Bars.ToolBar();
    this.btnAdd = new ButtonItem();
    this.btnDelete = new ButtonItem();
    this.btnOpenConfig = new ButtonItem();
    this.btnSaveConfig = new ButtonItem();
    this.pnlConfigEditor = new Panel();
    this.tbChanges = new Intermech.Bars.ToolBar();
    this.btnApply = new ButtonItem();
    this.btnCancel = new ButtonItem();
    this.dlgOpenFile = new OpenFileDialog();
    this.dlgSaveFile = new SaveFileDialog();
    this.action1 = new Intermech.Actions.Action(this.components);
    this.mainPanel.BeginInit();
    this.mainPanel.Panel1.SuspendLayout();
    this.mainPanel.Panel2.SuspendLayout();
    this.mainPanel.SuspendLayout();
    this.vtvConfig.BeginInit();
    this.SuspendLayout();
    this.Images.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("Images.ImageStream");
    this.Images.TransparentColor = Color.Fuchsia;
    this.Images.Images.SetKeyName(0, "open.bmp");
    this.Images.Images.SetKeyName(1, "save.bmp");
    this.Images.Images.SetKeyName(2, "Add.bmp");
    this.Images.Images.SetKeyName(3, "Apply.bmp");
    this.Images.Images.SetKeyName(4, "Cancel.bmp");
    this.Images.Images.SetKeyName(5, "delete.bmp");
    this.mainPanel.Dock = DockStyle.Fill;
    this.mainPanel.Location = new Point(0, 0);
    this.mainPanel.Name = "mainPanel";
    this.mainPanel.Panel1.Controls.Add((Control) this.vtvConfig);
    this.mainPanel.Panel1.Controls.Add((Control) this.tbMain);
    this.mainPanel.Panel2.Controls.Add((Control) this.pnlConfigEditor);
    this.mainPanel.Panel2.Controls.Add((Control) this.tbChanges);
    this.mainPanel.Size = new Size(443, 364);
    this.mainPanel.SplitterDistance = 193;
    this.mainPanel.TabIndex = 4;
    this.vtvConfig.AllowDrop = true;
    this.vtvConfig.BackColor = SystemColors.Control;
    this.vtvConfig.Columns.Add(this.column1);
    this.vtvConfig.Dock = DockStyle.Fill;
    this.vtvConfig.ImageList = (ImageList) null;
    this.vtvConfig.Location = new Point(0, 24);
    this.vtvConfig.MainColumn = this.column1;
    this.vtvConfig.Name = "vtvConfig";
    this.vtvConfig.ShowColumnHeaders = false;
    this.vtvConfig.Size = new Size(193, 340);
    this.vtvConfig.TabIndex = 0;
    this.vtvConfig.GetCellData += new GetCellDataHandler(this.vtvConfig_GetCellData);
    this.vtvConfig.GetChildren += new GetChildrenHandler(this.vtvConfig_GetChildren);
    this.vtvConfig.SelectionChanged += new EventHandler(this.vtvConfig_SelectionChanged);
    this.vtvConfig.SelectionChanging += new SelectionChangingHandler(this.vtvConfig_SelectionChanging);
    this.column1.AutoSizePolicy = ColumnAutoSizePolicy.AutoIncrease;
    this.column1.Caption = (string) null;
    this.column1.Name = "column1";
    this.column1.Resizable = false;
    this.column1.Sortable = false;
    this.tbMain.FullMenus = true;
    this.tbMain.Guid = new Guid("37008034-404a-400e-96a1-c4194e0a69ae");
    this.tbMain.Hidden = false;
    this.tbMain.ImageList = this.Images;
    this.tbMain.Items.AddRange(new ToolbarItemBase[4]
    {
      (ToolbarItemBase) this.btnAdd,
      (ToolbarItemBase) this.btnDelete,
      (ToolbarItemBase) this.btnOpenConfig,
      (ToolbarItemBase) this.btnSaveConfig
    });
    this.tbMain.Location = new Point(0, 0);
    this.tbMain.Name = "tbMain";
    this.tbMain.Size = new Size(193, 24);
    this.tbMain.TabIndex = 5;
    this.tbMain.Text = "tbMain";
    this.btnAdd.CommandName = "btnAdd";
    this.btnAdd.Enabled = false;
    this.btnAdd.ImageIndex = 2;
    this.btnAdd.Text = "Добавить";
    this.btnAdd.ToolTipText = "Добавить";
    this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
    this.btnDelete.CommandName = "btnDelete";
    this.btnDelete.Enabled = false;
    this.btnDelete.ImageIndex = 5;
    this.btnDelete.Text = "Удалить";
    this.btnDelete.ToolTipText = "Удалить";
    this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
    this.btnOpenConfig.BeginGroup = true;
    this.btnOpenConfig.CommandName = "btnOpenConfig";
    this.btnOpenConfig.ImageIndex = 0;
    this.btnOpenConfig.ToolTipText = "Отрыть файл настроек";
    this.btnOpenConfig.Click += new EventHandler(this.btnOpenConfig_Click);
    this.btnSaveConfig.CommandName = "btnSaveConfig";
    this.btnSaveConfig.ImageIndex = 1;
    this.btnSaveConfig.ToolTipText = "Сохранить настройки в файл";
    this.btnSaveConfig.Click += new EventHandler(this.btnSaveConfig_Click);
    this.pnlConfigEditor.Dock = DockStyle.Fill;
    this.pnlConfigEditor.Location = new Point(0, 24);
    this.pnlConfigEditor.Name = "pnlConfigEditor";
    this.pnlConfigEditor.Size = new Size(246, 340);
    this.pnlConfigEditor.TabIndex = 0;
    this.tbChanges.FullMenus = true;
    this.tbChanges.Guid = new Guid("37008034-404a-400e-96a1-c4194e0a69ae");
    this.tbChanges.Hidden = false;
    this.tbChanges.ImageList = this.Images;
    this.tbChanges.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this.btnApply,
      (ToolbarItemBase) this.btnCancel
    });
    this.tbChanges.Location = new Point(0, 0);
    this.tbChanges.Name = "tbChanges";
    this.tbChanges.Size = new Size(246, 24);
    this.tbChanges.TabIndex = 5;
    this.tbChanges.Text = "tbChanges";
    this.btnApply.CommandName = "btnApply";
    this.btnApply.Enabled = false;
    this.btnApply.ImageIndex = 3;
    this.btnApply.ToolTipText = "Применить изменения";
    this.btnApply.Click += new EventHandler(this.btnApply_Click);
    this.btnCancel.CommandName = "btnCancel";
    this.btnCancel.Enabled = false;
    this.btnCancel.ImageIndex = 4;
    this.btnCancel.ToolTipText = "Отменить изменения";
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    this.dlgOpenFile.DefaultExt = "config";
    this.dlgOpenFile.FileName = "openFileDialog1";
    this.dlgOpenFile.Filter = "Config files|*.config";
    this.dlgOpenFile.Title = "Открыть файл настроек";
    this.dlgOpenFile.RestoreDirectory = true;
    this.dlgSaveFile.DefaultExt = "config";
    this.dlgSaveFile.RestoreDirectory = true;
    this.action1.Hint = (string) null;
    this.action1.Text = "action1";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.mainPanel);
    this.Name = nameof (ConfigMainEditorView);
    this.Size = new Size(443, 364);
    this.mainPanel.Panel1.ResumeLayout(false);
    this.mainPanel.Panel2.ResumeLayout(false);
    this.mainPanel.EndInit();
    this.mainPanel.ResumeLayout(false);
    this.vtvConfig.EndInit();
    this.ResumeLayout(false);
  }
}

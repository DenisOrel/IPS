// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.StepControls.StepControlSettings
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.ImpExp.Interface.Controls;
using Intermech.ImpExp.Manager.CommonData.ItemsToCreate;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.ImpExp.Manager.StepControls;

internal sealed class StepControlSettings : StepControl
{
  private Guid newLocalGuid = Guid.Empty;
  private string _errorKey = "ATTR";
  private Image _image;
  private IAttributeTypeToCreateList _attrService;
  private IAttributeGroupToCreateList _attrGrService;
  private IObjectTypeToCreateList _objService;
  private ISettingsGroupService _groupService;
  private StepControlSettings.DisplayMode _displayMode = StepControlSettings.DisplayMode.Problem;
  private IContainer components;
  private SplitContainer splitContainer1;
  private Button buttonSelectAT;
  private PropertyGrid propertyGridAT;
  private TextBox textBoxAT;
  private RichTextBox richTextBox1;
  private PictureBox pictureBox1;
  private ImageList imageList1;
  private RadioButton radioButton3;
  private RadioButton radioButton2;
  private RadioButton radioButton1;
  private Panel panel1;
  private GroupBox groupBox1;
  private ToolStripButton toolStripButton1;
  private ToolStrip toolStrip1;
  private ToolStripMenuItem miFindErrorNode;
  private ContextMenuStrip cmenuAttrs;
  private GroupBox groupBox2;
  private Panel panel4;
  private Panel panel5;
  private GroupBox groupBox3;
  private Panel panel2;
  private iGrid iGrid1;
  private iGCellStyleDesign iGCellStyleDesign1;
  private RadioButton radioButton4;

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
      this._image = service.ImageList.Images[service.ImageIndex("imgSearchMetadata")];
    return this._image;
  }

  protected override string getCaption() => "Настройка метаданных SEARCH";

  public StepControlSettings(object owner)
    : base(owner)
  {
    this.InitializeComponent();
    this._attrService = ServicesManager.ServiceContainer.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
    this._attrGrService = ServicesManager.ServiceContainer.GetService(typeof (IAttributeGroupToCreateList)) as IAttributeGroupToCreateList;
    this._objService = ServicesManager.ServiceContainer.GetService(typeof (IObjectTypeToCreateList)) as IObjectTypeToCreateList;
    this._groupService = ServicesManager.ServiceContainer.GetService(typeof (ISettingsGroupService)) as ISettingsGroupService;
  }

  public override void RefreshControl()
  {
    this.newLocalGuid = Guid.Empty;
    this.iGrid1.Rows.Clear();
    for (int index1 = 0; index1 < this._groupService.Groups.Count; ++index1)
    {
      ISettingsGroup group = this._groupService.Groups[index1];
      if (group.Visible)
      {
        group.Sort();
        iGRow iGrow = this.iGrid1.Rows.Add();
        iGrow.Level = 1;
        iGrow.Tag = (object) group;
        bool flag1 = false;
        for (int index2 = 0; index2 < group.GroupItems.Count; ++index2)
        {
          bool flag2 = this.AddSettingsRow(1, group.GroupItems[index2]);
          if (!flag1)
            flag1 = flag2;
        }
        if (!flag1)
          iGrow.Visible = false;
        else
          iGrow.TreeButton = iGTreeButtonState.Visible;
      }
    }
    this.iGrid1.Rows.CollapseAll();
    this.propertyGridAT.SelectedObject = (object) null;
    this.textBoxAT.Text = string.Empty;
    this.richTextBox1.Clear();
    this.pictureBox1.Image = (Image) null;
    this.buttonSelectAT.Enabled = false;
  }

  private bool IsNewType(ISettingsItem settingsItem)
  {
    IItemToCreate itemToCreate = (IItemToCreate) null;
    switch (settingsItem)
    {
      case ISettingsAttributeTypeItem _:
        itemToCreate = (IItemToCreate) this._attrService?.GetByGuid(settingsItem.AttrGuid);
        break;
      case ISettingsAttributeGroupItem _:
        itemToCreate = (IItemToCreate) this._attrGrService?.GetByGuid(settingsItem.AttrGuid);
        break;
      case ISettingsObjectTypeItem _:
        itemToCreate = (IItemToCreate) this._objService?.GetByGuid(settingsItem.AttrGuid);
        break;
    }
    return itemToCreate == null || itemToCreate.IsNew;
  }

  private bool AddSettingsRow(int parentLevel, ISettingsGroupItem item)
  {
    iGRow iGrow1 = this.iGrid1.Rows.Add();
    iGrow1.Level = parentLevel + 1;
    iGrow1.Tag = (object) item;
    bool flag = false;
    if (item.SettingsItems != null && item.SettingsItems.Count > 0)
    {
      item.Sort();
      for (int index = 0; index < item.SettingsItems.Count; ++index)
      {
        if (item.SettingsItems[index] != null)
        {
          ISettingsItem settingsItem = item.SettingsItems[index];
          if (this._displayMode == StepControlSettings.DisplayMode.NotProblem && settingsItem.Error == null || this._displayMode == StepControlSettings.DisplayMode.Problem && settingsItem.Error != null || this._displayMode == StepControlSettings.DisplayMode.New && this.IsNewType(settingsItem) || this._displayMode == StepControlSettings.DisplayMode.All)
          {
            iGRow iGrow2 = this.iGrid1.Rows.Add();
            iGrow2.Level = iGrow1.Level + 1;
            iGrow2.Tag = (object) settingsItem;
            flag = true;
          }
        }
        else if (item.SettingsItems[index] is ISettingsGroupItem && !flag)
          flag = this.AddSettingsRow(iGrow1.Level + 1, item.SettingsItems[index] as ISettingsGroupItem);
      }
      iGrow1.TreeButton = iGTreeButtonState.Visible;
    }
    else if (item is ISettingsItem)
    {
      if (this._displayMode == StepControlSettings.DisplayMode.NotProblem && (item as ISettingsItem).Error == null || this._displayMode == StepControlSettings.DisplayMode.Problem && (item as ISettingsItem).Error != null || this._displayMode == StepControlSettings.DisplayMode.New && this.IsNewType(item as ISettingsItem) || this._displayMode == StepControlSettings.DisplayMode.All)
        flag = true;
      iGrow1.TreeButton = iGTreeButtonState.Hidden;
    }
    if (!flag)
      iGrow1.Visible = false;
    return flag;
  }

  public override SaveSettingsResult SaveSettings()
  {
    try
    {
      foreach (ISettingsGroup group in this._groupService.Groups)
      {
        foreach (ISettingsGroupItem groupItem in group.GroupItems)
        {
          if (groupItem is ISettingsItem)
          {
            ItemError error = (groupItem as ISettingsItem).Error;
            if (error != null && error.ErrorMessages.Exists((Predicate<MessageItem>) (x => x.ErrorType == ItemErrorType.Error)))
            {
              int num = (int) MessageBox.Show("В дереве настроек присутствуют элементы с ошибками", "Ошибка сохранения настроек", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return SaveSettingsResult.ssrRetry;
            }
          }
          else if (groupItem.SettingsItems != null)
          {
            foreach (ISettingsItem settingsItem in groupItem.SettingsItems)
            {
              ItemError error = settingsItem.Error;
              if (error != null && error.ErrorMessages.Exists((Predicate<MessageItem>) (x => x.ErrorType == ItemErrorType.Error)))
              {
                int num = (int) MessageBox.Show("В дереве настроек присутствуют элементы с ошибками", "Ошибка сохранения настроек", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return SaveSettingsResult.ssrRetry;
              }
            }
          }
        }
      }
      if (!this.CreateAndSaveToMetadata())
        throw new StepControlSettings.SaveMetadataException("Во время сохранения результатов настроек произошла ошибка!");
      return SaveSettingsResult.ssrOk;
    }
    catch (Exception ex)
    {
      if (!(ex is StepControlSettings.SaveMetadataException))
      {
        this.logFile.WriteMessage("StepControlSettings: " + ex.Message);
        this.logFile.WriteMessage("StepControlSettings: " + ex.StackTrace);
      }
      int num = (int) MessageBox.Show($"{ex.Message} Подробная информация по ошибке записана в файле {"import.log"}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return SaveSettingsResult.ssrRetry;
    }
  }

  private bool CreateAndSaveToMetadata() => new MetadataCreator(this.logFile).Create();

  private Guid GetSelectedNodeGuid()
  {
    return this.iGrid1.SelectedCells != null && this.iGrid1.SelectedCells.Count > 0 && this.iGrid1.SelectedCells[0].Row.Tag is ISettingsItem ? (this.iGrid1.SelectedCells[0].Row.Tag as ISettingsItem).AttrGuid : Guid.Empty;
  }

  private ISettingsItem GetSelectedNodeISettingsItem()
  {
    return this.iGrid1.SelectedCells != null && this.iGrid1.SelectedCells.Count > 0 && this.iGrid1.SelectedCells[0].Row.Tag is ISettingsItem ? this.iGrid1.SelectedCells[0].Row.Tag as ISettingsItem : (ISettingsItem) null;
  }

  private void UpdateAttributeType()
  {
    Guid selectedNodeGuid = this.GetSelectedNodeGuid();
    if (this.newLocalGuid.Equals(Guid.Empty))
      this.newLocalGuid = selectedNodeGuid;
    ISettingsItem nodeIsettingsItem = this.GetSelectedNodeISettingsItem();
    IItemToCreate itemToCreate = (IItemToCreate) null;
    switch (nodeIsettingsItem)
    {
      case ISettingsAttributeTypeItem _:
        itemToCreate = (IItemToCreate) this._attrService?.GetByGuid(this.newLocalGuid.Equals(Guid.Empty) ? selectedNodeGuid : this.newLocalGuid);
        break;
      case ISettingsAttributeGroupItem _:
        itemToCreate = (IItemToCreate) this._attrGrService?.GetByGuid(this.newLocalGuid.Equals(Guid.Empty) ? selectedNodeGuid : this.newLocalGuid);
        break;
      case ISettingsObjectTypeItem _:
        itemToCreate = (IItemToCreate) this._objService?.GetByGuid(this.newLocalGuid.Equals(Guid.Empty) ? selectedNodeGuid : this.newLocalGuid);
        break;
    }
    this.textBoxAT.Text = itemToCreate != null ? itemToCreate.Name : string.Empty;
    this.propertyGridAT.SelectedObject = (object) itemToCreate;
    if (selectedNodeGuid.Equals(this.newLocalGuid))
      return;
    this.SaveNewLocalId();
    this.UpdateSelectedAttributeType();
  }

  private void SettErrorForItem(ISettingsAttributeTypeItem si, ItemError error)
  {
    if (si.Error != null)
    {
      si.Error.ErrorMessages.RemoveAll((Predicate<MessageItem>) (x => x.Key.Equals(this._errorKey)));
      if (error == null)
        return;
      si.Error.ErrorMessages.AddRange((IEnumerable<MessageItem>) error.ErrorMessages);
    }
    else
      si.Error = error;
  }

  private void RefreshError(ISettingsItem si)
  {
    if (si != null && si.Error != null)
    {
      this.richTextBox1.Lines = si.Error.ErrorMessage;
      switch (si.Error.HeavyErrorType)
      {
        case ItemErrorType.None:
          this.pictureBox1.Image = (Image) null;
          break;
        case ItemErrorType.Renamed:
        case ItemErrorType.Warning:
          this.pictureBox1.Image = this.imageList1.Images[0];
          break;
        case ItemErrorType.Error:
          this.pictureBox1.Image = this.imageList1.Images[1];
          break;
      }
    }
    else
    {
      this.richTextBox1.Clear();
      this.pictureBox1.Image = (Image) null;
    }
  }

  private void UpdateSelectedAttributeType()
  {
    this.newLocalGuid = Guid.Empty;
    this.UpdateAttributeType();
  }

  private ItemError CheckAttribute(ISettingsItem rec, Guid newGuid)
  {
    return this.CheckAttribute(ServicesManager.ServiceContainer.GetService<IAttributeTypeToCreateList>().GetByGuid(newGuid), ((ISettingsAttributeTypeItem) rec).FieldType, ((ISettingsAttributeTypeItem) rec).ValueMaxLength);
  }

  private void SaveNewLocalId()
  {
    ISettingsItem nodeIsettingsItem = this.GetSelectedNodeISettingsItem();
    if (nodeIsettingsItem == null)
      return;
    if (nodeIsettingsItem is ISettingsAttributeTypeItem)
    {
      ItemError error = this.CheckAttribute(nodeIsettingsItem, this.newLocalGuid);
      this.SettErrorForItem((ISettingsAttributeTypeItem) nodeIsettingsItem, error);
      if (error != null)
      {
        string text = string.Empty;
        foreach (string str in error.ErrorMessage)
          text = $"{text}{str}\n";
        switch (error.HeavyErrorType)
        {
          case ItemErrorType.Warning:
            if (MessageBox.Show(text + "Продолжить ?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
              return;
            nodeIsettingsItem.Error = error;
            break;
          case ItemErrorType.Error:
            int num = (int) MessageBox.Show(text, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          default:
            nodeIsettingsItem.Error = error;
            break;
        }
      }
    }
    nodeIsettingsItem.AttrGuid = this.newLocalGuid;
  }

  private ItemError CheckAttribute(IAttributeTypeToCreate attrType, FieldTypes fieldType, int size)
  {
    ItemErrorType errorType = ItemErrorType.None;
    string[] errorMessages = AttributeTypeComparer.CompareAttributeType(attrType, fieldType, size, ref errorType);
    return errorType != ItemErrorType.None ? new ItemError(errorType, errorMessages) : (ItemError) null;
  }

  private void ButtonSelectAT_Click(object sender, EventArgs e)
  {
    if (this.newLocalGuid.Equals(Guid.Empty))
      this.newLocalGuid = this.GetSelectedNodeGuid();
    bool flag = false;
    ServicesManager.GetService<INotificationService>();
    ISettingsItem nodeIsettingsItem = this.GetSelectedNodeISettingsItem();
    switch (nodeIsettingsItem)
    {
      case ISettingsAttributeGroupItem _ when this._attrGrService != null && this._attrGrService.SelectDialog != null:
        this._attrGrService.SelectDialog.SelectedItemGUID = this.newLocalGuid;
        if (this._attrGrService.SelectDialog.ShowDialog() == DialogResult.OK)
        {
          this.newLocalGuid = this._attrGrService.SelectDialog.SelectedItemGUID;
          break;
        }
        break;
      case ISettingsAttributeTypeItem _ when this._attrService != null && this._attrService.SelectDialog != null:
        this._attrService.SelectDialog.SelectedItemGUID = this.newLocalGuid;
        if (this._attrService.SelectDialog.ShowDialog() == DialogResult.OK)
        {
          this.newLocalGuid = this._attrService.SelectDialog.SelectedItemGUID;
          flag = true;
          break;
        }
        break;
      case ISettingsObjectTypeItem _ when this._objService != null && this._objService.SelectDialog != null:
        this._objService.SelectDialog.SelectedItemGUID = this.newLocalGuid;
        if (this._objService.SelectDialog.ShowDialog() == DialogResult.OK)
        {
          if (ServicesManager.ServiceContainer.GetService<IObjectTypeToCreateList>().GetByGuid(this._objService.SelectDialog.SelectedItemGUID).VersionMode == ObjectVersionModes.Abstract && MessageBox.Show("Внимание! Вы привязали к абстрактныму типу объектов! Продолжить?", "Привязка типов", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
            return;
          flag = true;
          nodeIsettingsItem.Error = (ItemError) null;
          this.newLocalGuid = this._objService.SelectDialog.SelectedItemGUID;
          break;
        }
        break;
    }
    this.UpdateAttributeType();
    if (flag)
      this.FireBindNotification(this.newLocalGuid);
    this.RefreshError(nodeIsettingsItem);
  }

  private void FireBindNotification(Guid guid)
  {
    if (this.iGrid1.CurRow == null || !(this.iGrid1.CurRow.Tag is ISettingsItem))
      return;
    iGRow iGrow = (iGRow) null;
    for (int index = this.iGrid1.CurRow.Index - 1; index >= 0; --index)
    {
      iGrow = this.iGrid1.Rows[index];
      if (iGrow.Tag is ISettingsGroup)
        break;
    }
    if (iGrow == null || !(iGrow.Tag is ISettingsGroup))
      return;
    this._groupService.FireItemBindChanged(iGrow.Tag as ISettingsGroup, this.iGrid1.CurRow.Tag as ISettingsItem);
  }

  private void PropertyGridAT_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
  {
    ISettingsItem nodeIsettingsItem = this.GetSelectedNodeISettingsItem();
    IItemToCreate itemToCreate = (IItemToCreate) null;
    ItemToCreateSelectDialog createSelectDialog = (ItemToCreateSelectDialog) null;
    switch (nodeIsettingsItem)
    {
      case ISettingsAttributeGroupItem _ when this._attrGrService != null:
        itemToCreate = (IItemToCreate) this._attrGrService.GetByGuid(this.newLocalGuid);
        createSelectDialog = this._attrGrService.SelectDialog as ItemToCreateSelectDialog;
        break;
      case ISettingsAttributeTypeItem _ when this._attrService != null:
        itemToCreate = (IItemToCreate) this._attrService.GetByGuid(this.newLocalGuid);
        createSelectDialog = this._attrService.SelectDialog as ItemToCreateSelectDialog;
        break;
      case ISettingsObjectTypeItem _ when this._objService != null:
        itemToCreate = (IItemToCreate) this._objService.GetByGuid(this.newLocalGuid);
        createSelectDialog = this._objService.SelectDialog as ItemToCreateSelectDialog;
        break;
    }
    if (e.ChangedItem.PropertyDescriptor.Name == "Name")
      this.textBoxAT.Text = itemToCreate != null ? itemToCreate.Name : string.Empty;
    createSelectDialog?.UpdateNodeInfo(this.newLocalGuid, e);
    this.propertyGridAT.Refresh();
  }

  private void RadioButton2_CheckedChanged(object sender, EventArgs e)
  {
    if (!((RadioButton) sender).Checked)
      return;
    this._displayMode = (StepControlSettings.DisplayMode) Convert.ToInt32(((Control) sender).Tag);
    this.RefreshControl();
  }

  private TreeNode GetNextNode(TreeNode currentNode)
  {
    if (currentNode.Nodes.Count > 0)
      return currentNode.Nodes[0];
    if (currentNode.Parent == null)
      return (TreeNode) null;
    int num = currentNode.Parent.Nodes.IndexOf(currentNode);
    return num == currentNode.Parent.Nodes.Count - 1 ? this.GetNextNode(currentNode.Parent) : currentNode.Parent.Nodes[num + 1];
  }

  private iGRow NextRow(iGRow curentRow)
  {
    return curentRow.Index + 1 > this.iGrid1.Rows.Count - 1 ? (iGRow) null : this.iGrid1.Rows[curentRow.Index + 1];
  }

  private void ToolStripButton1_Click(object sender, EventArgs e)
  {
    if (this.iGrid1.Rows.Count == 0)
      return;
    iGRow curentRow1 = this.iGrid1.SelectedCells == null || this.iGrid1.SelectedCells.Count <= 0 ? this.iGrid1.Rows[0] : this.iGrid1.SelectedCells[0].Row;
    curentRow1.Expanded = true;
    for (iGRow curentRow2 = this.NextRow(curentRow1); curentRow2 != null; curentRow2 = this.NextRow(curentRow2))
    {
      curentRow2.Expanded = true;
      if (curentRow2.Tag is ISettingsItem)
      {
        ISettingsItem tag = curentRow2.Tag as ISettingsItem;
        if (tag.Error != null && tag.Error.ErrorMessages.Exists((Predicate<MessageItem>) (x => x.ErrorType == ItemErrorType.Error)))
        {
          curentRow2.Cells[0].Selected = true;
          return;
        }
      }
    }
    int num = (int) MessageBox.Show("Метаданных с ошибкой привязки не найдено", "Результат поиска", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
  }

  private void IGrid1_SelectionStartChange(object sender, iGSelectionStartEndChangeEventArgs e)
  {
    if (this.newLocalGuid.Equals(Guid.Empty) || this.newLocalGuid.Equals(this.GetSelectedNodeGuid()) || MessageBox.Show("Привязка поля была изменена. Сохранить изменения", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    this.SaveNewLocalId();
  }

  private void IGrid1_SelectionChanged(object sender, EventArgs e)
  {
    this.UpdateSelectedAttributeType();
    this.richTextBox1.Lines = (string[]) null;
    this.pictureBox1.Image = (Image) null;
    iGRow row = this.iGrid1.SelectedCells[0].Row;
    this.buttonSelectAT.Enabled = row.Tag != null && (row.Tag is ISettingsItem || row.Tag is ISettingsAttributeTypeItem || row.Tag is ISettingsObjectTypeItem || row.Tag is ISettingsAttributeGroupItem);
    if (!(row.Tag is ISettingsItem))
      return;
    this.RefreshError(row.Tag as ISettingsItem);
  }

  private void IGrid1_CustomDrawCellForeground(object sender, iGCustomDrawCellEventArgs e)
  {
    if (e.ColIndex != 0)
      return;
    iGRow row = this.iGrid1.Rows[e.RowIndex];
    string s = string.Empty;
    if (row.Tag is ISettingsGroup)
      s = ((ISettingsGroup) row.Tag).Caption;
    else if (row.Tag is ISettingsGroupItem)
      s = ((ISettingsGroupItem) row.Tag).Caption;
    else if (row.Tag is ISettingsItem)
      s = ((ISettingsItem) row.Tag).LongName;
    using (SolidBrush solidBrush = new SolidBrush(this.iGrid1.ForeColor))
      e.Graphics.DrawString(s, this.iGrid1.Font, (Brush) solidBrush, (PointF) new Point(e.Bounds.Left, e.Bounds.Top));
  }

  private void TextBoxAT_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.KeyData != Keys.Delete)
      return;
    iGRow row = this.iGrid1.SelectedCells[0].Row;
    if (row == null || !(row.Tag is ISettingsObjectTypeItem))
      return;
    this.propertyGridAT.SelectedObject = (object) null;
    ((ISettingsItem) row.Tag).AttrGuid = Guid.Empty;
    this.newLocalGuid = Guid.Empty;
    ((ISettingsItem) row.Tag).Error = new ItemError(ItemErrorType.Warning, "Тип объектов в миграции не участвует");
    this.UpdateAttributeType();
    this.RefreshError((ISettingsItem) row.Tag);
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
    iGColPattern iGcolPattern = new iGColPattern();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (StepControlSettings));
    this.iGCellStyleDesign1 = new iGCellStyleDesign();
    this.pictureBox1 = new PictureBox();
    this.richTextBox1 = new RichTextBox();
    this.propertyGridAT = new PropertyGrid();
    this.buttonSelectAT = new Button();
    this.textBoxAT = new TextBox();
    this.splitContainer1 = new SplitContainer();
    this.groupBox1 = new GroupBox();
    this.iGrid1 = new iGrid();
    this.toolStrip1 = new ToolStrip();
    this.toolStripButton1 = new ToolStripButton();
    this.panel1 = new Panel();
    this.radioButton3 = new RadioButton();
    this.radioButton2 = new RadioButton();
    this.radioButton1 = new RadioButton();
    this.groupBox2 = new GroupBox();
    this.panel4 = new Panel();
    this.panel5 = new Panel();
    this.groupBox3 = new GroupBox();
    this.panel2 = new Panel();
    this.cmenuAttrs = new ContextMenuStrip(this.components);
    this.miFindErrorNode = new ToolStripMenuItem();
    this.imageList1 = new ImageList(this.components);
    this.radioButton4 = new RadioButton();
    ((ISupportInitialize) this.pictureBox1).BeginInit();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.groupBox1.SuspendLayout();
    ((ISupportInitialize) this.iGrid1).BeginInit();
    this.toolStrip1.SuspendLayout();
    this.panel1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.panel4.SuspendLayout();
    this.panel5.SuspendLayout();
    this.groupBox3.SuspendLayout();
    this.panel2.SuspendLayout();
    this.cmenuAttrs.SuspendLayout();
    this.SuspendLayout();
    this.iGCellStyleDesign1.BackColor = SystemColors.Window;
    this.iGCellStyleDesign1.CustomDrawFlags = iGCustomDrawFlags.Foreground;
    this.pictureBox1.Location = new Point(15, 36);
    this.pictureBox1.Name = "pictureBox1";
    this.pictureBox1.Size = new Size(32 /*0x20*/, 32 /*0x20*/);
    this.pictureBox1.TabIndex = 15;
    this.pictureBox1.TabStop = false;
    this.richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.richTextBox1.BorderStyle = BorderStyle.None;
    this.richTextBox1.Location = new Point(53, 19);
    this.richTextBox1.Name = "richTextBox1";
    this.richTextBox1.ReadOnly = true;
    this.richTextBox1.Size = new Size(379, 69);
    this.richTextBox1.TabIndex = 14;
    this.richTextBox1.Text = "";
    this.propertyGridAT.Dock = DockStyle.Fill;
    this.propertyGridAT.Location = new Point(0, 0);
    this.propertyGridAT.Name = "propertyGridAT";
    this.propertyGridAT.Size = new Size(442, 434);
    this.propertyGridAT.TabIndex = 2;
    this.propertyGridAT.ToolbarVisible = false;
    this.propertyGridAT.PropertyValueChanged += new PropertyValueChangedEventHandler(this.PropertyGridAT_PropertyValueChanged);
    this.buttonSelectAT.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.buttonSelectAT.Enabled = false;
    this.buttonSelectAT.Location = new Point(415, 9);
    this.buttonSelectAT.Name = "buttonSelectAT";
    this.buttonSelectAT.Size = new Size(24, 20);
    this.buttonSelectAT.TabIndex = 3;
    this.buttonSelectAT.Text = "...";
    this.buttonSelectAT.UseVisualStyleBackColor = true;
    this.buttonSelectAT.Click += new EventHandler(this.ButtonSelectAT_Click);
    this.textBoxAT.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.textBoxAT.BackColor = SystemColors.Window;
    this.textBoxAT.Location = new Point(3, 9);
    this.textBoxAT.Name = "textBoxAT";
    this.textBoxAT.ReadOnly = true;
    this.textBoxAT.Size = new Size(407, 20);
    this.textBoxAT.TabIndex = 3;
    this.textBoxAT.KeyDown += new KeyEventHandler(this.TextBoxAT_KeyDown);
    this.splitContainer1.Dock = DockStyle.Fill;
    this.splitContainer1.Location = new Point(0, 0);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.groupBox1);
    this.splitContainer1.Panel1.Controls.Add((Control) this.panel1);
    this.splitContainer1.Panel2.Controls.Add((Control) this.groupBox2);
    this.splitContainer1.Size = new Size(695, 585);
    this.splitContainer1.SplitterDistance = 243;
    this.splitContainer1.TabIndex = 0;
    this.groupBox1.Controls.Add((Control) this.iGrid1);
    this.groupBox1.Controls.Add((Control) this.toolStrip1);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(0, 0);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(243, 496);
    this.groupBox1.TabIndex = 13;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Метаданные для перекачки";
    this.iGrid1.AutoResizeCols = true;
    iGcolPattern.CellStyle = (iGCellStyle) this.iGCellStyleDesign1;
    iGcolPattern.Text = (object) "Наименование";
    iGcolPattern.Width = 233;
    this.iGrid1.Cols.AddRange(new iGColPattern[1]
    {
      iGcolPattern
    });
    this.iGrid1.Dock = DockStyle.Fill;
    this.iGrid1.Header.Height = 19;
    this.iGrid1.Location = new Point(3, 41);
    this.iGrid1.Name = "iGrid1";
    this.iGrid1.ReadOnly = true;
    this.iGrid1.Size = new Size(237, 452);
    this.iGrid1.TabIndex = 16 /*0x10*/;
    this.iGrid1.CustomDrawCellForeground += new iGCustomDrawCellEventHandler(this.IGrid1_CustomDrawCellForeground);
    this.iGrid1.SelectionStartChange += new iGSelectionStartEndChangeEventHandler(this.IGrid1_SelectionStartChange);
    this.iGrid1.SelectionChanged += new EventHandler(this.IGrid1_SelectionChanged);
    this.toolStrip1.Items.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this.toolStripButton1
    });
    this.toolStrip1.Location = new Point(3, 16 /*0x10*/);
    this.toolStrip1.Name = "toolStrip1";
    this.toolStrip1.Size = new Size(237, 25);
    this.toolStrip1.TabIndex = 14;
    this.toolStrip1.Text = "toolStrip1";
    this.toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.toolStripButton1.Image = (Image) componentResourceManager.GetObject("toolStripButton1.Image");
    this.toolStripButton1.ImageTransparentColor = Color.Magenta;
    this.toolStripButton1.Name = "toolStripButton1";
    this.toolStripButton1.Size = new Size(23, 22);
    this.toolStripButton1.Text = "Найти элемент с ошибкой в привязке";
    this.toolStripButton1.Click += new EventHandler(this.ToolStripButton1_Click);
    this.panel1.Controls.Add((Control) this.radioButton4);
    this.panel1.Controls.Add((Control) this.radioButton3);
    this.panel1.Controls.Add((Control) this.radioButton2);
    this.panel1.Controls.Add((Control) this.radioButton1);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 496);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(243, 89);
    this.panel1.TabIndex = 12;
    this.radioButton3.AutoSize = true;
    this.radioButton3.Location = new Point(28, 46);
    this.radioButton3.Name = "radioButton3";
    this.radioButton3.Size = new Size(156, 17);
    this.radioButton3.TabIndex = 18;
    this.radioButton3.Tag = (object) "2";
    this.radioButton3.Text = "Показать не проблемные";
    this.radioButton3.UseVisualStyleBackColor = true;
    this.radioButton3.CheckedChanged += new EventHandler(this.RadioButton2_CheckedChanged);
    this.radioButton2.AutoSize = true;
    this.radioButton2.Checked = true;
    this.radioButton2.Location = new Point(28, 28);
    this.radioButton2.Name = "radioButton2";
    this.radioButton2.Size = new Size(141, 17);
    this.radioButton2.TabIndex = 17;
    this.radioButton2.TabStop = true;
    this.radioButton2.Tag = (object) "1";
    this.radioButton2.Text = "Показать проблемные";
    this.radioButton2.UseVisualStyleBackColor = true;
    this.radioButton2.CheckedChanged += new EventHandler(this.RadioButton2_CheckedChanged);
    this.radioButton1.AutoSize = true;
    this.radioButton1.Location = new Point(18, 8);
    this.radioButton1.Name = "radioButton1";
    this.radioButton1.Size = new Size(95, 17);
    this.radioButton1.TabIndex = 16 /*0x10*/;
    this.radioButton1.Tag = (object) "0";
    this.radioButton1.Text = "Показать все";
    this.radioButton1.UseVisualStyleBackColor = true;
    this.radioButton1.CheckedChanged += new EventHandler(this.RadioButton2_CheckedChanged);
    this.groupBox2.Controls.Add((Control) this.panel4);
    this.groupBox2.Controls.Add((Control) this.panel2);
    this.groupBox2.Dock = DockStyle.Fill;
    this.groupBox2.Location = new Point(0, 0);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(448, 585);
    this.groupBox2.TabIndex = 1;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Соответствующий тип в IPS";
    this.panel4.Controls.Add((Control) this.panel5);
    this.panel4.Controls.Add((Control) this.groupBox3);
    this.panel4.Dock = DockStyle.Fill;
    this.panel4.Location = new Point(3, 54);
    this.panel4.Name = "panel4";
    this.panel4.Size = new Size(442, 528);
    this.panel4.TabIndex = 1;
    this.panel5.Controls.Add((Control) this.propertyGridAT);
    this.panel5.Dock = DockStyle.Fill;
    this.panel5.Location = new Point(0, 94);
    this.panel5.Name = "panel5";
    this.panel5.Size = new Size(442, 434);
    this.panel5.TabIndex = 1;
    this.groupBox3.Controls.Add((Control) this.richTextBox1);
    this.groupBox3.Controls.Add((Control) this.pictureBox1);
    this.groupBox3.Dock = DockStyle.Top;
    this.groupBox3.Location = new Point(0, 0);
    this.groupBox3.Name = "groupBox3";
    this.groupBox3.Size = new Size(442, 94);
    this.groupBox3.TabIndex = 0;
    this.groupBox3.TabStop = false;
    this.groupBox3.Text = "Результат привязки";
    this.panel2.Controls.Add((Control) this.textBoxAT);
    this.panel2.Controls.Add((Control) this.buttonSelectAT);
    this.panel2.Dock = DockStyle.Top;
    this.panel2.Location = new Point(3, 16 /*0x10*/);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(442, 38);
    this.panel2.TabIndex = 0;
    this.cmenuAttrs.Items.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this.miFindErrorNode
    });
    this.cmenuAttrs.Name = "cmenuAttrs";
    this.cmenuAttrs.Size = new Size(284, 26);
    this.miFindErrorNode.Name = "miFindErrorNode";
    this.miFindErrorNode.Size = new Size(283, 22);
    this.miFindErrorNode.Text = "Найти элемент с ошибкой в привязке";
    this.miFindErrorNode.Click += new EventHandler(this.ToolStripButton1_Click);
    this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
    this.imageList1.TransparentColor = Color.Transparent;
    this.imageList1.Images.SetKeyName(0, "sign-alert.png");
    this.imageList1.Images.SetKeyName(1, "sign-stop.png");
    this.radioButton4.AutoSize = true;
    this.radioButton4.Location = new Point(28, 64 /*0x40*/);
    this.radioButton4.Name = "radioButton4";
    this.radioButton4.Size = new Size(109, 17);
    this.radioButton4.TabIndex = 19;
    this.radioButton4.Tag = (object) "3";
    this.radioButton4.Text = "Показать новые";
    this.radioButton4.UseVisualStyleBackColor = true;
    this.radioButton4.CheckedChanged += new EventHandler(this.RadioButton2_CheckedChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.splitContainer1);
    this.Name = nameof (StepControlSettings);
    this.Size = new Size(695, 585);
    ((ISupportInitialize) this.pictureBox1).EndInit();
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    ((ISupportInitialize) this.iGrid1).EndInit();
    this.toolStrip1.ResumeLayout(false);
    this.toolStrip1.PerformLayout();
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    this.groupBox2.ResumeLayout(false);
    this.panel4.ResumeLayout(false);
    this.panel5.ResumeLayout(false);
    this.groupBox3.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.panel2.PerformLayout();
    this.cmenuAttrs.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private enum DisplayMode
  {
    All,
    Problem,
    NotProblem,
    New,
  }

  private class SaveMetadataException(string message) : Exception(message)
  {
  }
}

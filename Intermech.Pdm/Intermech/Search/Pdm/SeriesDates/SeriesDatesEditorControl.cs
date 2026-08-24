// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.SeriesDates.SeriesDatesEditorControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using Intermech.Search.Data.Repositories;
using Intermech.Search.UI;
using Intermech.Search.UI.VirtualTree;
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
namespace Intermech.Search.Pdm.SeriesDates;

public class SeriesDatesEditorControl : UserControl
{
  private const string ObjectVersionIDColumnKey = "ObjectVersionID";
  private const string HeadProductColumnKey = "Head product";
  private const string HasStartRangeColumnKey = "Has start range";
  private const string SeriesOrStartRangeColumnKey = "Series/Start range";
  private const string HasEndRangeColumnKey = "Has end range";
  private const string DatesOrEndRangeColumnKey = "Dates/End range";
  private long[] _objectVersionIds;
  private SeriesDatesPack _seriesDatesPack;
  private Dictionary<long, Dictionary<long, SeriesDatesPack>> _otherVersionsSeriesDatesPackDictionaryDictionary;
  private bool _hasChanges;
  private Dictionary<IRange, string> _errorDictionary = new Dictionary<IRange, string>();
  private LazyService<IAttributeTypeForObjectRepository> _attributeTypeForObjectRepository = new LazyService<IAttributeTypeForObjectRepository>();
  private bool _editable;
  private IContainer components;
  private ToolStrip toolStrip1;
  private Intermech.Search.UI.VirtualTree.VirtualTree _seriesDatesEditTree;
  private ToolStripButton _addSeriesDatesGroupToolStripButton;
  private ToolStripButton _removeToolStripButton;
  private Column _headProductColumn;
  private Column _seriesOrStrartRangeColumn;
  private Column _datesOrEndRangeColumn;
  private Button _cancelButton;
  private Button _applyButton;
  private Column _hasStartRangeColumn;
  private Column _hasEndRangeColumn;
  private ToolStripButton _addSeriesDatesRangeToolStripButton;
  private SplitContainer splitContainer1;
  private Intermech.Search.UI.VirtualTree.VirtualTree _seriesDatesHelpTree;
  private Column _objectVersionIDColumn;
  private Column _objectCaptionHeadProductColumn;
  private Column _seriesColumn;
  private Column _datesColumn;
  private MessageControl _helpMessageControl;
  private MessageControl _notEditableObjectMessageControl;
  private Panel panel1;
  private Panel panel2;

  public SeriesDatesEditorControl()
  {
    this.InitializeComponent();
    this._headProductColumn.DataField = "Head product";
    this._hasStartRangeColumn.DataField = "Has start range";
    this._seriesOrStrartRangeColumn.DataField = "Series/Start range";
    this._hasEndRangeColumn.DataField = "Has end range";
    this._datesOrEndRangeColumn.DataField = "Dates/End range";
    this._seriesDatesEditTree.RowBindings.Add((RowBinding) new SeriesDatesEditorControl.SeriesDatesPackRowBinding());
    this._seriesDatesEditTree.RowBindings.Add((RowBinding) new SeriesDatesEditorControl.SeriesDatesGroupRowBinding());
    this._seriesDatesEditTree.RowBindings.Add((RowBinding) new SeriesDatesEditorControl.SeriesRangeCollectionRowBinding());
    this._seriesDatesEditTree.RowBindings.Add((RowBinding) new SeriesDatesEditorControl.DateRangeCollectionRowBinding());
    SeriesDatesEditorControl.SeriesRangeRowBinding seriesRangeRowBinding = new SeriesDatesEditorControl.SeriesRangeRowBinding();
    seriesRangeRowBinding.GetRowError += new EventHandler<SeriesDatesEditorControl.GetRowErrorEventArgs>(this.SeriesRangeRowBinding_GetRowError);
    this._seriesDatesEditTree.RowBindings.Add((RowBinding) seriesRangeRowBinding);
    SeriesDatesEditorControl.DateRangeRowBinding dateRangeRowBinding = new SeriesDatesEditorControl.DateRangeRowBinding();
    dateRangeRowBinding.GetRowError += new EventHandler<SeriesDatesEditorControl.GetRowErrorEventArgs>(this.DateRangeRowBinding_GetRowError);
    this._seriesDatesEditTree.RowBindings.Add((RowBinding) dateRangeRowBinding);
    this._objectVersionIDColumn.DataField = "ObjectVersionID";
    this._objectCaptionHeadProductColumn.DataField = "Head product";
    this._seriesColumn.DataField = "Series/Start range";
    this._datesColumn.DataField = "Dates/End range";
    this._seriesDatesHelpTree.RowBindings.Add((RowBinding) new SeriesDatesEditorControl.SeriesDatesPackRowBinding());
    this._seriesDatesHelpTree.RowBindings.Add((RowBinding) new SeriesDatesEditorControl.SeriesDatesGroupRowBinding(SeriesDatesEditorControl.SeriesDatesGroupRowBindingOptions.None));
    this.SetObjectVersionIds();
  }

  public long[] ObjectVersionIds
  {
    get => this._objectVersionIds;
    set
    {
      if (this._objectVersionIds == value)
        return;
      this._objectVersionIds = value;
      this.SetObjectVersionIds();
    }
  }

  public bool HasChanges
  {
    get => this._hasChanges;
    private set
    {
      if (this._hasChanges == value)
        return;
      this._hasChanges = value;
      this.SetHasChanges();
    }
  }

  public bool HasErrors => this._errorDictionary.Count > 0;

  public bool Editable
  {
    get => this._editable;
    set
    {
      if (this._editable == value)
        return;
      this._editable = value;
      this.SetEditable();
    }
  }

  public void ApplyChanges()
  {
    if (!this.HasChanges || this.HasErrors)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      sessionKeeper.Session.StartLogHistory();
      try
      {
        (sessionKeeper.Session.GetCustomService(typeof (ISeriesDatesServerService)) as ISeriesDatesServerService).SaveSeriesDates(sessionKeeper.Session.SessionGUID, this._objectVersionIds, this._seriesDatesPack);
        NotificationHelper.Notify((object) this, sessionKeeper.Session.GetModificationsHistoryList());
      }
      finally
      {
        sessionKeeper.Session.StopLogHistory();
      }
    }
    this.HasChanges = false;
  }

  public void CancelChanges()
  {
    if (!this.HasChanges || MessageBox.Show("Настройки серий и дат были изменены. Отменить изменения?", "Intermech Professional Solution", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
      return;
    this.SetObjectVersionIds();
    this.HasChanges = false;
  }

  private void AddSeriesDatesGroupToolStripButton_Click(object sender, EventArgs e)
  {
    object[] objArray = SelectionWindow.Select(LocalizationHolder.rm.GetString("Pdm_569"), LocalizationHolder.rm.GetString("Pdm_568"), (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID("cadd940b-306c-11d8-b4e9-00304f19f545")), typeof (IDBTypedObjectID), (IServiceProvider) ServicesManager.ServiceContainer, SelectionOptions.Default | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect);
    if (objArray == null || objArray.Length == 0)
      return;
    IDBTypedObjectID @object = objArray[0] as IDBTypedObjectID;
    SeriesDatesGroup seriesDatesGroup = this._seriesDatesPack.Groups.FirstOrDefault<SeriesDatesGroup>((Func<SeriesDatesGroup, bool>) (o => o.HeadProductVersionID == @object.ObjectID));
    if (seriesDatesGroup == null)
    {
      seriesDatesGroup = new SeriesDatesGroup(@object.ObjectID);
      seriesDatesGroup.Dates.Add(DateRange.Empty);
      seriesDatesGroup.Series.Add(SeriesRange.Empty);
      this._seriesDatesPack.Groups.Add(seriesDatesGroup);
    }
    this._seriesDatesEditTree.SelectedItem = (object) seriesDatesGroup;
  }

  private void AddSeriesDatesRangeToolStripButton_Click(object sender, EventArgs e)
  {
    object selectedItem = this._seriesDatesEditTree.SelectedItem;
    switch (selectedItem)
    {
      case SeriesRangeCollection _:
        ((Collection<SeriesRange>) selectedItem).Add(SeriesRange.Empty);
        break;
      case SeriesRange _:
        ((SeriesRange) selectedItem).Group.Series.Add(SeriesRange.Empty);
        break;
      case DateRangeCollection _:
        ((Collection<DateRange>) selectedItem).Add(DateRange.Empty);
        break;
      case DateRange _:
        ((DateRange) selectedItem).Group.Dates.Add(DateRange.Empty);
        break;
    }
  }

  private void RemoveSeriesDatesGroupToolStripButton_Click(object sender, EventArgs e)
  {
    object selectedItem = this._seriesDatesEditTree.SelectedItem;
    switch (selectedItem)
    {
      case SeriesDatesGroup _:
        SeriesDatesGroup seriesDatesGroup = (SeriesDatesGroup) selectedItem;
        seriesDatesGroup.Pack.Groups.Remove(seriesDatesGroup);
        break;
      case SeriesRange _:
        SeriesRange seriesRange = (SeriesRange) selectedItem;
        seriesRange.Group.Series.Remove(seriesRange);
        break;
      case DateRange _:
        DateRange dateRange = (DateRange) selectedItem;
        dateRange.Group.Dates.Remove(dateRange);
        break;
    }
    this._seriesDatesEditTree.UpdateRowDataRecurcive(this._seriesDatesEditTree.RootRow);
  }

  private void ApplyButton_Click(object sender, EventArgs e) => this.ApplyChanges();

  private void CancelButton_Click(object sender, EventArgs e) => this.CancelChanges();

  private void DateRangeRowBinding_GetRowError(
    object sender,
    SeriesDatesEditorControl.GetRowErrorEventArgs e)
  {
    DateRange key = (DateRange) e.Row.Item;
    string str = (string) null;
    this._errorDictionary.TryGetValue((IRange) key, out str);
    e.Error = str;
  }

  private void SeriesRangeRowBinding_GetRowError(
    object sender,
    SeriesDatesEditorControl.GetRowErrorEventArgs e)
  {
    SeriesRange key = (SeriesRange) e.Row.Item;
    string str = (string) null;
    this._errorDictionary.TryGetValue((IRange) key, out str);
    e.Error = str;
  }

  private void SeriesDatesEditTree_SelectionChanged(object sender, EventArgs e)
  {
    this.SetAddSeriesDatesRangeToolStripButtonEnabled();
    this.SetRemoveToolStripButtonEnabled();
  }

  private void Groups_ListChanged(object sender, ListChangedEventArgs e)
  {
    if (e.ListChangedType == ListChangedType.ItemAdded)
    {
      SeriesDatesGroup seriesDatesGroup = ((Collection<SeriesDatesGroup>) sender)[e.NewIndex];
    }
    this.HasChanges = true;
    this.SetErrors();
  }

  private void SetObjectVersionIds()
  {
    if (this._objectVersionIds == null || this._objectVersionIds.Length == 0)
    {
      this._seriesDatesPack = new SeriesDatesPack();
      this._otherVersionsSeriesDatesPackDictionaryDictionary = new Dictionary<long, Dictionary<long, SeriesDatesPack>>();
      this.Editable = false;
    }
    else
    {
      this.Editable = SeriesDatesHelper.CheckObjectsForSaveSeriesDates(this._objectVersionIds);
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        ISeriesDatesServerService customService = sessionKeeper.Session.GetCustomService(typeof (ISeriesDatesServerService)) as ISeriesDatesServerService;
        this._seriesDatesPack = customService.FindSeriesDates(sessionKeeper.Session.SessionGUID, this._objectVersionIds);
        this._otherVersionsSeriesDatesPackDictionaryDictionary = customService.FindSeriesDatesForOtherVersions(sessionKeeper.Session.SessionGUID, this._objectVersionIds);
      }
    }
    this._seriesDatesPack.Groups.ListChanged += new ListChangedEventHandler(this.Groups_ListChanged);
    this._seriesDatesEditTree.DataSource = (object) this._seriesDatesPack;
    this._seriesDatesEditTree.SelectedItem = (object) null;
    List<SeriesDatesPack> seriesDatesPackList = new List<SeriesDatesPack>();
    foreach (Dictionary<long, SeriesDatesPack> dictionary in this._otherVersionsSeriesDatesPackDictionaryDictionary.Values)
      seriesDatesPackList.AddRange((IEnumerable<SeriesDatesPack>) dictionary.Values);
    this._seriesDatesHelpTree.DataSource = (object) seriesDatesPackList;
    this._seriesDatesHelpTree.SelectedItem = (object) null;
    this.HasChanges = false;
  }

  private void SetHasChanges()
  {
    this.SetApplyButtonEnabled();
    this.SetCancelButtonEnabled();
  }

  private void SetEditable()
  {
    this.SetAddSeriesDatesGroupToolStripButtonEnabled();
    this.SetAddSeriesDatesRangeToolStripButtonEnabled();
    this.SetRemoveToolStripButtonEnabled();
    this.SetWarningVisible();
    this.SetSeriesDatesEditTreeEnabled();
    this.SetApplyButtonEnabled();
    this.SetCancelButtonEnabled();
  }

  private void SetAddSeriesDatesGroupToolStripButtonEnabled()
  {
    this._addSeriesDatesGroupToolStripButton.Enabled = this.Editable;
  }

  private void SetAddSeriesDatesRangeToolStripButtonEnabled()
  {
    object selectedItem = this._seriesDatesEditTree.SelectedItem;
    ToolStripButton rangeToolStripButton = this._addSeriesDatesRangeToolStripButton;
    int num;
    if (this.Editable)
    {
      switch (selectedItem)
      {
        case SeriesRangeCollection _:
        case SeriesRange _:
        case DateRangeCollection _:
          num = 1;
          break;
        default:
          num = selectedItem is DateRange ? 1 : 0;
          break;
      }
    }
    else
      num = 0;
    rangeToolStripButton.Enabled = num != 0;
  }

  private void SetRemoveToolStripButtonEnabled()
  {
    object selectedItem = this._seriesDatesEditTree.SelectedItem;
    ToolStripButton removeToolStripButton = this._removeToolStripButton;
    int num;
    if (this.Editable)
    {
      switch (selectedItem)
      {
        case SeriesDatesGroup _:
        case SeriesRange _:
          num = 1;
          break;
        default:
          num = selectedItem is DateRange ? 1 : 0;
          break;
      }
    }
    else
      num = 0;
    removeToolStripButton.Enabled = num != 0;
  }

  private void SetWarningVisible()
  {
    this._notEditableObjectMessageControl.Visible = !this.Editable;
  }

  private void SetSeriesDatesEditTreeEnabled() => this._seriesDatesEditTree.Enabled = this.Editable;

  private void SetApplyButtonEnabled()
  {
    this._applyButton.Enabled = this.Editable && this.HasChanges && !this.HasErrors;
  }

  private void SetCancelButtonEnabled()
  {
    this._cancelButton.Enabled = this.Editable && this.HasChanges;
  }

  private void SetErrors()
  {
    SeriesDatesHelper.CheckSeriesDatesIntersectionsWithOtherVersions(this._seriesDatesPack, this._otherVersionsSeriesDatesPackDictionaryDictionary, out this._errorDictionary);
    this.SetApplyButtonEnabled();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SeriesDatesEditorControl));
    this.toolStrip1 = new ToolStrip();
    this._addSeriesDatesGroupToolStripButton = new ToolStripButton();
    this._addSeriesDatesRangeToolStripButton = new ToolStripButton();
    this._removeToolStripButton = new ToolStripButton();
    this._cancelButton = new Button();
    this._applyButton = new Button();
    this.splitContainer1 = new SplitContainer();
    this._seriesDatesEditTree = new Intermech.Search.UI.VirtualTree.VirtualTree();
    this._headProductColumn = new Column();
    this._hasStartRangeColumn = new Column();
    this._seriesOrStrartRangeColumn = new Column();
    this._hasEndRangeColumn = new Column();
    this._datesOrEndRangeColumn = new Column();
    this._seriesDatesHelpTree = new Intermech.Search.UI.VirtualTree.VirtualTree();
    this._objectVersionIDColumn = new Column();
    this._objectCaptionHeadProductColumn = new Column();
    this._seriesColumn = new Column();
    this._datesColumn = new Column();
    this._notEditableObjectMessageControl = new MessageControl();
    this._helpMessageControl = new MessageControl();
    this.panel1 = new Panel();
    this.panel2 = new Panel();
    this.toolStrip1.SuspendLayout();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this._seriesDatesEditTree.BeginInit();
    this._seriesDatesHelpTree.BeginInit();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.toolStrip1.Items.AddRange(new ToolStripItem[3]
    {
      (ToolStripItem) this._addSeriesDatesGroupToolStripButton,
      (ToolStripItem) this._addSeriesDatesRangeToolStripButton,
      (ToolStripItem) this._removeToolStripButton
    });
    this.toolStrip1.Location = new Point(0, 0);
    this.toolStrip1.Name = "toolStrip1";
    this.toolStrip1.Size = new Size(744, 25);
    this.toolStrip1.TabIndex = 0;
    this.toolStrip1.Text = "toolStrip1";
    this._addSeriesDatesGroupToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._addSeriesDatesGroupToolStripButton.Image = (Image) componentResourceManager.GetObject("_addSeriesDatesGroupToolStripButton.Image");
    this._addSeriesDatesGroupToolStripButton.ImageTransparentColor = Color.Magenta;
    this._addSeriesDatesGroupToolStripButton.Name = "_addSeriesDatesGroupToolStripButton";
    this._addSeriesDatesGroupToolStripButton.Size = new Size(23, 22);
    this._addSeriesDatesGroupToolStripButton.Text = "Добавить группу серий/дат";
    this._addSeriesDatesGroupToolStripButton.Click += new EventHandler(this.AddSeriesDatesGroupToolStripButton_Click);
    this._addSeriesDatesRangeToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._addSeriesDatesRangeToolStripButton.Image = (Image) componentResourceManager.GetObject("_addSeriesDatesRangeToolStripButton.Image");
    this._addSeriesDatesRangeToolStripButton.ImageTransparentColor = Color.Magenta;
    this._addSeriesDatesRangeToolStripButton.Name = "_addSeriesDatesRangeToolStripButton";
    this._addSeriesDatesRangeToolStripButton.Size = new Size(23, 22);
    this._addSeriesDatesRangeToolStripButton.Text = "Добавить диапазон серий/дат";
    this._addSeriesDatesRangeToolStripButton.Click += new EventHandler(this.AddSeriesDatesRangeToolStripButton_Click);
    this._removeToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._removeToolStripButton.Image = (Image) componentResourceManager.GetObject("_removeToolStripButton.Image");
    this._removeToolStripButton.ImageTransparentColor = Color.Magenta;
    this._removeToolStripButton.Name = "_removeToolStripButton";
    this._removeToolStripButton.Size = new Size(23, 22);
    this._removeToolStripButton.Text = "Удалить";
    this._removeToolStripButton.Click += new EventHandler(this.RemoveSeriesDatesGroupToolStripButton_Click);
    this._cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._cancelButton.Location = new Point(664, 6);
    this._cancelButton.Name = "_cancelButton";
    this._cancelButton.Size = new Size(75, 23);
    this._cancelButton.TabIndex = 2;
    this._cancelButton.Text = "Отмена";
    this._cancelButton.UseVisualStyleBackColor = true;
    this._cancelButton.Click += new EventHandler(this.CancelButton_Click);
    this._applyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._applyButton.Location = new Point(584, 6);
    this._applyButton.Name = "_applyButton";
    this._applyButton.Size = new Size(75, 23);
    this._applyButton.TabIndex = 2;
    this._applyButton.Text = "Применить";
    this._applyButton.UseVisualStyleBackColor = true;
    this._applyButton.Click += new EventHandler(this.ApplyButton_Click);
    this.splitContainer1.Dock = DockStyle.Fill;
    this.splitContainer1.Location = new Point(0, 0);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Orientation = Orientation.Horizontal;
    this.splitContainer1.Panel1.Controls.Add((Control) this._seriesDatesEditTree);
    this.splitContainer1.Panel2.Controls.Add((Control) this._seriesDatesHelpTree);
    this.splitContainer1.Size = new Size(744, 262);
    this.splitContainer1.SplitterDistance = 132;
    this.splitContainer1.TabIndex = 3;
    this._seriesDatesEditTree.AllowDrop = true;
    this._seriesDatesEditTree.Columns.Add(this._headProductColumn);
    this._seriesDatesEditTree.Columns.Add(this._hasStartRangeColumn);
    this._seriesDatesEditTree.Columns.Add(this._seriesOrStrartRangeColumn);
    this._seriesDatesEditTree.Columns.Add(this._hasEndRangeColumn);
    this._seriesDatesEditTree.Columns.Add(this._datesOrEndRangeColumn);
    this._seriesDatesEditTree.Dock = DockStyle.Fill;
    this._seriesDatesEditTree.EditOnKeyboardFocus = true;
    this._seriesDatesEditTree.EnableRowCaching = false;
    this._seriesDatesEditTree.IconWidth = 0;
    this._seriesDatesEditTree.ImageList = (ImageList) null;
    this._seriesDatesEditTree.LineStyle = LineStyle.None;
    this._seriesDatesEditTree.Location = new Point(0, 0);
    this._seriesDatesEditTree.Name = "_seriesDatesEditTree";
    this._seriesDatesEditTree.RowStyle.BorderStyle = Border3DStyle.Adjust;
    this._seriesDatesEditTree.RowStyle.BorderWidth = 1;
    this._seriesDatesEditTree.ShowRootRow = false;
    this._seriesDatesEditTree.ShowRowHeaders = true;
    this._seriesDatesEditTree.Size = new Size(744, 132);
    this._seriesDatesEditTree.TabIndex = 1;
    this._seriesDatesEditTree.SelectionChanged += new EventHandler(this.SeriesDatesEditTree_SelectionChanged);
    this._headProductColumn.Caption = "Головное изделие";
    this._headProductColumn.Name = "_headProductColumn";
    this._headProductColumn.ToolTip = "Головное изделие";
    this._headProductColumn.Width = 150;
    this._hasStartRangeColumn.Caption = "Есть начало";
    this._hasStartRangeColumn.CellStyle.HorzAlignment = StringAlignment.Center;
    this._hasStartRangeColumn.Name = "_hasStartRangeColumn";
    this._hasStartRangeColumn.ToolTip = "Есть начало";
    this._hasStartRangeColumn.Width = 80 /*0x50*/;
    this._seriesOrStrartRangeColumn.Caption = "Серии/Начало диапазона";
    this._seriesOrStrartRangeColumn.Name = "_seriesOrStrartRangeColumn";
    this._seriesOrStrartRangeColumn.ToolTip = "Серии/Начало диапазона";
    this._seriesOrStrartRangeColumn.Width = 250;
    this._hasEndRangeColumn.Caption = "Есть конец";
    this._hasEndRangeColumn.CellStyle.HorzAlignment = StringAlignment.Center;
    this._hasEndRangeColumn.Name = "_hasEndRangeColumn";
    this._hasEndRangeColumn.ToolTip = "Есть конец";
    this._hasEndRangeColumn.Width = 80 /*0x50*/;
    this._datesOrEndRangeColumn.Caption = "Даты/Конец диапазона";
    this._datesOrEndRangeColumn.Name = "_datesOrEndRangeColumn";
    this._datesOrEndRangeColumn.ToolTip = "Даты/Конец диапазона";
    this._datesOrEndRangeColumn.Width = 250;
    this._seriesDatesHelpTree.AllowDrop = true;
    this._seriesDatesHelpTree.Columns.Add(this._objectVersionIDColumn);
    this._seriesDatesHelpTree.Columns.Add(this._objectCaptionHeadProductColumn);
    this._seriesDatesHelpTree.Columns.Add(this._seriesColumn);
    this._seriesDatesHelpTree.Columns.Add(this._datesColumn);
    this._seriesDatesHelpTree.Dock = DockStyle.Fill;
    this._seriesDatesHelpTree.IconWidth = 0;
    this._seriesDatesHelpTree.ImageList = (ImageList) null;
    this._seriesDatesHelpTree.LineStyle = LineStyle.None;
    this._seriesDatesHelpTree.Location = new Point(0, 0);
    this._seriesDatesHelpTree.Name = "_seriesDatesHelpTree";
    this._seriesDatesHelpTree.RowStyle.BorderStyle = Border3DStyle.Adjust;
    this._seriesDatesHelpTree.RowStyle.BorderWidth = 1;
    this._seriesDatesHelpTree.ShowRootRow = false;
    this._seriesDatesHelpTree.Size = new Size(744, 126);
    this._seriesDatesHelpTree.TabIndex = 0;
    this._objectVersionIDColumn.Caption = "Идентификатор версии объекта";
    this._objectVersionIDColumn.Name = "_objectVersionIDColumn";
    this._objectVersionIDColumn.ToolTip = "Идентификатор версии объекта";
    this._objectVersionIDColumn.Width = 150;
    this._objectCaptionHeadProductColumn.Caption = "Заголовок объекта/Головное изделие";
    this._objectCaptionHeadProductColumn.Name = "_objectCaptionHeadProductColumn";
    this._objectCaptionHeadProductColumn.ToolTip = "Заголовок объекта/Головное изделие";
    this._objectCaptionHeadProductColumn.Width = 250;
    this._seriesColumn.Caption = "Серии";
    this._seriesColumn.Name = "_seriesColumn";
    this._seriesColumn.ToolTip = "Серии";
    this._seriesColumn.Width = 200;
    this._datesColumn.Caption = "Даты";
    this._datesColumn.Name = "_datesColumn";
    this._datesColumn.ToolTip = "Даты";
    this._datesColumn.Width = 200;
    this._notEditableObjectMessageControl.BackColor = Color.LightYellow;
    this._notEditableObjectMessageControl.BorderStyle = BorderStyle.FixedSingle;
    this._notEditableObjectMessageControl.Dock = DockStyle.Top;
    this._notEditableObjectMessageControl.Location = new Point(0, 25);
    this._notEditableObjectMessageControl.Margin = new Padding(4);
    this._notEditableObjectMessageControl.Name = "_notEditableObjectMessageControl";
    this._notEditableObjectMessageControl.Size = new Size(744, 58);
    this._notEditableObjectMessageControl.TabIndex = 5;
    this._notEditableObjectMessageControl.Text = "Внимание один или несколько выбранных объектов редактировать невозможно\r\nДля редактирования возмите объекты на изменение";
    this._notEditableObjectMessageControl.Type = _MessageType.Warning;
    this._notEditableObjectMessageControl.Visible = false;
    this._helpMessageControl.BackColor = Color.LightBlue;
    this._helpMessageControl.BorderStyle = BorderStyle.FixedSingle;
    this._helpMessageControl.Dock = DockStyle.Top;
    this._helpMessageControl.Location = new Point(0, 83);
    this._helpMessageControl.Margin = new Padding(4);
    this._helpMessageControl.Name = "_helpMessageControl";
    this._helpMessageControl.Size = new Size(744, 62);
    this._helpMessageControl.TabIndex = 4;
    this._helpMessageControl.Text = componentResourceManager.GetString("_helpMessageControl.Text");
    this.panel1.Controls.Add((Control) this._applyButton);
    this.panel1.Controls.Add((Control) this._cancelButton);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 407);
    this.panel1.Margin = new Padding(2);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(744, 36);
    this.panel1.TabIndex = 6;
    this.panel2.Controls.Add((Control) this.splitContainer1);
    this.panel2.Dock = DockStyle.Fill;
    this.panel2.Location = new Point(0, 145);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(744, 262);
    this.panel2.TabIndex = 7;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this._helpMessageControl);
    this.Controls.Add((Control) this._notEditableObjectMessageControl);
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.toolStrip1);
    this.Name = nameof (SeriesDatesEditorControl);
    this.Size = new Size(744, 443);
    this.toolStrip1.ResumeLayout(false);
    this.toolStrip1.PerformLayout();
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this._seriesDatesEditTree.EndInit();
    this._seriesDatesHelpTree.EndInit();
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private sealed class SeriesDatesPackRowBinding : ObjectRowBinding
  {
    private LazyService<ICategoryTypeIconService> _categoryTypeIconService = new LazyService<ICategoryTypeIconService>();
    private Dictionary<long, Tuple<int, string>> _objectInfoDictonary = new Dictionary<long, Tuple<int, string>>();

    public SeriesDatesPackRowBinding()
      : base(typeof (SeriesDatesPack))
    {
      this.ChildProperty = "Groups";
    }

    public override void GetCellData(Row row, Column column, CellData cellData)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (column == null)
        throw new ArgumentNullException(nameof (column));
      if (cellData == null)
        throw new ArgumentNullException(nameof (cellData));
      SeriesDatesPack seriesDatesPack = (SeriesDatesPack) row.Item;
      if (column.DataField == "ObjectVersionID")
      {
        cellData.Value = (object) seriesDatesPack.ObjectVersionID;
      }
      else
      {
        if (!(column.DataField == "Head product") || ObjectHelper.IsUnknownObjectVersionID(seriesDatesPack.ObjectVersionID))
          return;
        cellData.Value = (object) this.GetObjectCaption(seriesDatesPack.ObjectVersionID);
      }
    }

    public override void GetRowData(Row row, RowData rowData)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (rowData == null)
        throw new ArgumentNullException(nameof (rowData));
      base.GetRowData(row, rowData);
      SeriesDatesPack seriesDatesPack = (SeriesDatesPack) row.Item;
      if (ObjectHelper.IsUnknownObjectVersionID(seriesDatesPack.ObjectVersionID))
        return;
      rowData.ImageList = this._categoryTypeIconService.Value.ImageList;
      rowData.ImageIndex = this._categoryTypeIconService.Value.IndexOf(4, this.GetObjectTypeID(seriesDatesPack.ObjectVersionID));
    }

    private string GetObjectCaption(long objectVersionID) => this.GetObject(objectVersionID).Item2;

    private int GetObjectTypeID(long objectVersionID) => this.GetObject(objectVersionID).Item1;

    private Tuple<int, string> GetObject(long objectVersionID)
    {
      if (!this._objectInfoDictonary.ContainsKey(objectVersionID))
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(objectVersionID);
          string str = dbObject.Caption;
          int versionId = dbObject.VersionID;
          if (!string.IsNullOrEmpty(str) && versionId != 0)
            str = $"{str} [{(object) versionId}]";
          this._objectInfoDictonary[objectVersionID] = new Tuple<int, string>(dbObject.ObjectType, str);
        }
      }
      return this._objectInfoDictonary[objectVersionID];
    }
  }

  [Flags]
  private enum SeriesDatesGroupRowBindingOptions
  {
    None = 0,
    AllowChildren = 1,
    AllowEdit = 2,
    Default = AllowEdit | AllowChildren, // 0x00000003
  }

  private sealed class SeriesDatesGroupRowBinding : ObjectRowBinding
  {
    private Dictionary<long, string> _headProductCaptionDictionary = new Dictionary<long, string>();
    private LazyService<ICategoryTypeIconService> _categoryTypeIconService = new LazyService<ICategoryTypeIconService>();

    public SeriesDatesGroupRowBinding(
      SeriesDatesEditorControl.SeriesDatesGroupRowBindingOptions options = SeriesDatesEditorControl.SeriesDatesGroupRowBindingOptions.Default)
      : base(typeof (SeriesDatesGroup))
    {
      this.Options = options;
      this.ChildPolicy = RowChildPolicy.AutoExpand;
    }

    public SeriesDatesEditorControl.SeriesDatesGroupRowBindingOptions Options { get; private set; }

    public override void GetCellData(Row row, Column column, CellData cellData)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (column == null)
        throw new ArgumentNullException(nameof (column));
      if (cellData == null)
        throw new ArgumentNullException(nameof (cellData));
      base.GetCellData(row, column, cellData);
      cellData.Editor = (CellEditor) null;
      SeriesDatesGroup seriesDatesGroup = row.Item as SeriesDatesGroup;
      if (column.DataField == "Head product")
        cellData.Value = (object) this.GetHeadProductCaption(seriesDatesGroup.HeadProductVersionID);
      else if (column.DataField == "Series/Start range")
      {
        if (this.Options.HasFlag((Enum) SeriesDatesEditorControl.SeriesDatesGroupRowBindingOptions.AllowEdit))
          cellData.Editor = new CellEditor((Control) new TextBox());
        cellData.Value = (object) SeriesDatesHelper.ConvertSeriesRangeArrayToString(seriesDatesGroup.Series.ToArray<SeriesRange>());
      }
      else
      {
        if (!(column.DataField == "Dates/End range"))
          return;
        if (this.Options.HasFlag((Enum) SeriesDatesEditorControl.SeriesDatesGroupRowBindingOptions.AllowEdit))
          cellData.Editor = new CellEditor((Control) new TextBox());
        cellData.Value = (object) SeriesDatesHelper.ConvertDateRangeArrayToString(seriesDatesGroup.Dates.ToArray<DateRange>());
      }
    }

    public override IList GetChildrenForRow(Row row)
    {
      SeriesDatesGroup seriesDatesGroup = row != null ? row.Item as SeriesDatesGroup : throw new ArgumentNullException(nameof (row));
      if (!this.Options.HasFlag((Enum) SeriesDatesEditorControl.SeriesDatesGroupRowBindingOptions.AllowChildren))
        return (IList) null;
      BindingList<object> childrenForRow = new BindingList<object>();
      childrenForRow.Add((object) seriesDatesGroup.Series);
      childrenForRow.Add((object) seriesDatesGroup.Dates);
      return (IList) childrenForRow;
    }

    public override void GetRowData(Row row, RowData rowData)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (rowData == null)
        throw new ArgumentNullException(nameof (rowData));
      base.GetRowData(row, rowData);
      rowData.ImageList = this._categoryTypeIconService.Value.ImageList;
      rowData.ImageIndex = this._categoryTypeIconService.Value.IndexOf(4, SeriesDatesConstants.HeadProductObjectTypeID);
    }

    public override bool SetCellValue(Row row, Column column, object oldValue, object newValue)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (column == null)
        throw new ArgumentNullException(nameof (column));
      SeriesDatesGroup seriesDatesGroup = (SeriesDatesGroup) row.Item;
      if (column.DataField == "Series/Start range")
      {
        string @string = newValue as string;
        seriesDatesGroup.Series.Clear();
        if (!string.IsNullOrEmpty(@string))
          seriesDatesGroup.Series.AddRange(SeriesDatesHelper.ConvertStringToSeriesRangeArray(@string));
        return true;
      }
      if (!(column.DataField == "Dates/End range"))
        return false;
      string string1 = newValue as string;
      seriesDatesGroup.Dates.Clear();
      if (!string.IsNullOrEmpty(string1))
        seriesDatesGroup.Dates.AddRange(SeriesDatesHelper.ConvertStringToDateRangeArray(string1));
      return true;
    }

    private string GetHeadProductCaption(long headProductVersionID)
    {
      if (!this._headProductCaptionDictionary.ContainsKey(headProductVersionID))
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(headProductVersionID);
          this._headProductCaptionDictionary[headProductVersionID] = dbObject.Caption;
        }
      }
      return this._headProductCaptionDictionary[headProductVersionID];
    }
  }

  private sealed class SeriesRangeCollectionRowBinding : ObjectRowBinding
  {
    public SeriesRangeCollectionRowBinding()
      : base(typeof (SeriesRangeCollection))
    {
    }

    public override void GetCellData(Row row, Column column, CellData cellData)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (column == null)
        throw new ArgumentNullException(nameof (column));
      if (cellData == null)
        throw new ArgumentNullException(nameof (cellData));
      base.GetCellData(row, column, cellData);
      cellData.Editor = (CellEditor) null;
      SeriesRangeCollection seriesRangeCollection = (SeriesRangeCollection) row.Item;
      if (!(column.DataField == "Head product"))
        return;
      cellData.Value = (object) "Серии";
    }

    public override IList GetChildrenForRow(Row row)
    {
      return row != null ? (IList) (row.Item as SeriesRangeCollection) : throw new ArgumentNullException(nameof (row));
    }

    public override void GetRowData(Row row, RowData rowData)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (rowData == null)
        throw new ArgumentNullException(nameof (rowData));
      base.GetRowData(row, rowData);
      rowData.ImageList = SeriesDatesImageHelper.ImageList;
      rowData.ImageIndex = SeriesDatesImageHelper.ImageList.Images.IndexOfKey("Series_16x16.png");
    }
  }

  private sealed class DateRangeCollectionRowBinding : ObjectRowBinding
  {
    public DateRangeCollectionRowBinding()
      : base(typeof (DateRangeCollection))
    {
    }

    public override void GetCellData(Row row, Column column, CellData cellData)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (column == null)
        throw new ArgumentNullException(nameof (column));
      if (cellData == null)
        throw new ArgumentNullException(nameof (cellData));
      base.GetCellData(row, column, cellData);
      cellData.Editor = (CellEditor) null;
      if (!(column.DataField == "Head product"))
        return;
      cellData.Value = (object) "Даты";
    }

    public override IList GetChildrenForRow(Row row)
    {
      return row != null ? (IList) (row.Item as DateRangeCollection) : throw new ArgumentNullException(nameof (row));
    }

    public override void GetRowData(Row row, RowData rowData)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (rowData == null)
        throw new ArgumentNullException(nameof (rowData));
      base.GetRowData(row, rowData);
      rowData.ImageList = SeriesDatesImageHelper.ImageList;
      rowData.ImageIndex = SeriesDatesImageHelper.ImageList.Images.IndexOfKey("Dates_16x16.png");
    }
  }

  private sealed class GetRowErrorEventArgs : EventArgs
  {
    public GetRowErrorEventArgs(Row row)
    {
      this.Row = row != null ? row : throw new ArgumentNullException(nameof (row));
    }

    public Row Row { get; private set; }

    public string Error { get; set; }
  }

  private sealed class SeriesRangeRowBinding : ObjectRowBinding, ICellWidgetCustomer
  {
    public SeriesRangeRowBinding()
      : base(typeof (SeriesRange))
    {
    }

    public event EventHandler<SeriesDatesEditorControl.GetRowErrorEventArgs> GetRowError;

    public override void GetCellData(Row row, Column column, CellData cellData)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (column == null)
        throw new ArgumentNullException(nameof (column));
      if (cellData == null)
        throw new ArgumentNullException(nameof (cellData));
      base.GetCellData(row, column, cellData);
      SeriesRange seriesRange = row.Item as SeriesRange;
      if (column.DataField == "Series/Start range")
      {
        cellData.Editor = this.GetRangeStartEndEditor(0, seriesRange.End);
        cellData.Value = seriesRange.HasStart ? (object) (seriesRange.Start > 0 ? seriesRange.Start : 0) : (object) null;
      }
      else
      {
        if (!(column.DataField == "Dates/End range"))
          return;
        cellData.Editor = this.GetRangeStartEndEditor(seriesRange.Start > 0 ? seriesRange.Start : 0, int.MaxValue);
        cellData.Value = seriesRange.HasEnd ? (object) (seriesRange.End > 0 ? seriesRange.End : 0) : (object) null;
      }
    }

    public override void GetRowData(Row row, RowData rowData)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (rowData == null)
        throw new ArgumentNullException("owData");
      base.GetRowData(row, rowData);
      rowData.Error = this.OnGetRowError(row);
    }

    public override bool SetCellValue(Row row, Column column, object oldValue, object newValue)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (column == null)
        throw new ArgumentNullException(nameof (column));
      SeriesRange seriesRange = row.Item as SeriesRange;
      try
      {
        if (column.DataField == "Series/Start range")
        {
          int int32 = Convert.ToInt32(newValue);
          seriesRange.Start = int32;
        }
        else if (column.DataField == "Dates/End range")
        {
          int int32 = Convert.ToInt32(newValue);
          seriesRange.End = int32;
        }
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Intermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    public void InitializeCellWidget(ExtendedCellWidget cellWidget)
    {
      if (cellWidget == null)
        throw new ArgumentNullException(nameof (cellWidget));
      if (cellWidget.Row == null || cellWidget.Column == null)
        throw new ArgumentException();
      SeriesRange seriesRange = cellWidget.Row.Item as SeriesRange;
      if (cellWidget.Column.DataField == "Has start range")
      {
        cellWidget.Behaviour = CellBehaviour.CheckBox;
        cellWidget.Checked = seriesRange.HasStart;
      }
      else
      {
        if (!(cellWidget.Column.DataField == "Has end range"))
          return;
        cellWidget.Behaviour = CellBehaviour.CheckBox;
        cellWidget.Checked = seriesRange.HasEnd;
      }
    }

    public void CellWidgetChanged(ExtendedCellWidget cellWidget)
    {
      if (cellWidget == null)
        throw new ArgumentNullException(nameof (cellWidget));
      if (cellWidget.Row == null || cellWidget.Column == null)
        throw new ArgumentException();
      SeriesRange seriesRange = cellWidget.Row.Item as SeriesRange;
      if (cellWidget.Column.DataField == "Has start range")
      {
        seriesRange.HasStart = cellWidget.Checked;
      }
      else
      {
        if (!(cellWidget.Column.DataField == "Has end range"))
          return;
        seriesRange.HasEnd = cellWidget.Checked;
      }
    }

    private CellEditor GetRangeStartEndEditor(int minimum, int maximum)
    {
      return new CellEditor((Control) new NumericUpDown()
      {
        Minimum = (Decimal) (minimum > 0 ? minimum : 0),
        Maximum = (Decimal) (maximum > 0 ? maximum : 0)
      });
    }

    private string OnGetRowError(Row row)
    {
      EventHandler<SeriesDatesEditorControl.GetRowErrorEventArgs> getRowError = this.GetRowError;
      SeriesDatesEditorControl.GetRowErrorEventArgs e = new SeriesDatesEditorControl.GetRowErrorEventArgs(row);
      if (getRowError != null)
        getRowError((object) this, e);
      return e.Error;
    }
  }

  private sealed class DateRangeRowBinding : ObjectRowBinding, ICellWidgetCustomer
  {
    public DateRangeRowBinding()
      : base(typeof (DateRange))
    {
    }

    public event EventHandler<SeriesDatesEditorControl.GetRowErrorEventArgs> GetRowError;

    public override void GetCellData(Row row, Column column, CellData cellData)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (column == null)
        throw new ArgumentNullException(nameof (column));
      if (cellData == null)
        throw new ArgumentNullException(nameof (cellData));
      base.GetCellData(row, column, cellData);
      DateRange dateRange = (DateRange) row.Item;
      if (column.DataField == "Series/Start range")
      {
        cellData.Editor = this.GetRangeStartEndEditor(DateRange.MinValue, dateRange.End);
        cellData.Value = dateRange.HasStart ? (object) dateRange.Start.ToString("d") : (object) (string) null;
      }
      else
      {
        if (!(column.DataField == "Dates/End range"))
          return;
        cellData.Editor = this.GetRangeStartEndEditor(dateRange.Start, DateRange.MaxValue);
        cellData.Value = dateRange.HasEnd ? (object) dateRange.End.ToString("d") : (object) (string) null;
      }
    }

    public override void GetRowData(Row row, RowData rowData)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (rowData == null)
        throw new ArgumentNullException("owData");
      base.GetRowData(row, rowData);
      rowData.Error = this.OnGetRowError(row);
    }

    public override bool SetCellValue(Row row, Column column, object oldValue, object newValue)
    {
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (column == null)
        throw new ArgumentNullException(nameof (column));
      try
      {
        DateRange dateRange = (DateRange) row.Item;
        if (column.DataField == "Series/Start range")
        {
          DateTime dateTime = Convert.ToDateTime(newValue);
          dateRange.Start = dateTime;
        }
        else if (column.DataField == "Dates/End range")
        {
          DateTime dateTime = Convert.ToDateTime(newValue);
          dateRange.End = dateTime;
        }
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Intermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    public void InitializeCellWidget(ExtendedCellWidget cellWidget)
    {
      if (cellWidget == null)
        throw new ArgumentNullException(nameof (cellWidget));
      if (cellWidget.Row == null || cellWidget.Column == null)
        throw new ArgumentException();
      DateRange dateRange = cellWidget.Row.Item as DateRange;
      if (cellWidget.Column.DataField == "Has start range")
      {
        cellWidget.Behaviour = CellBehaviour.CheckBox;
        cellWidget.Checked = dateRange.HasStart;
      }
      else
      {
        if (!(cellWidget.Column.DataField == "Has end range"))
          return;
        cellWidget.Behaviour = CellBehaviour.CheckBox;
        cellWidget.Checked = dateRange.HasEnd;
      }
    }

    public void CellWidgetChanged(ExtendedCellWidget cellWidget)
    {
      if (cellWidget == null)
        throw new ArgumentNullException(nameof (cellWidget));
      if (cellWidget.Row == null || cellWidget.Column == null)
        throw new ArgumentException();
      DateRange dateRange = cellWidget.Row.Item as DateRange;
      if (cellWidget.Column.DataField == "Has start range")
      {
        dateRange.HasStart = cellWidget.Checked;
      }
      else
      {
        if (!(cellWidget.Column.DataField == "Has end range"))
          return;
        dateRange.HasEnd = cellWidget.Checked;
      }
    }

    private void CellEditor_SetControlValue(object sender, CellEditorSetValueEventArgs e)
    {
      DateTimePicker control = (DateTimePicker) e.Control;
      if (e.Value == null)
        return;
      DateTime dateTime = Convert.ToDateTime(e.Value);
      if (!(control.MinDate <= dateTime) || !(dateTime <= control.MaxDate))
        return;
      control.Value = dateTime;
    }

    private CellEditor GetRangeStartEndEditor(DateTime minimum, DateTime maximum)
    {
      DateTimePicker dateTimePicker = new DateTimePicker();
      if (minimum.Date != DateRange.MinValue && minimum.Date != DateRange.MaxValue)
        dateTimePicker.MinDate = minimum.Date;
      if (maximum.Date != DateRange.MinValue && maximum.Date != DateRange.MaxValue)
        dateTimePicker.MaxDate = maximum.Date;
      CellEditor rangeStartEndEditor = new CellEditor((Control) dateTimePicker);
      rangeStartEndEditor.SetControlValue += new CellEditorSetValueHandler(this.CellEditor_SetControlValue);
      return rangeStartEndEditor;
    }

    private string OnGetRowError(Row row)
    {
      EventHandler<SeriesDatesEditorControl.GetRowErrorEventArgs> getRowError = this.GetRowError;
      SeriesDatesEditorControl.GetRowErrorEventArgs e = new SeriesDatesEditorControl.GetRowErrorEventArgs(row);
      if (getRowError != null)
        getRowError((object) this, e);
      return e.Error;
    }
  }
}

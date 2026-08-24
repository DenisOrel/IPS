// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.OptionEditor
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Client.Core.Thumbnail;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using Intermech.Search.Pdm.CompositionsConfigurator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.PdmConfigurator;

public class OptionEditor : UserControl
{
  private bool _readOnly;
  private bool _inEvents;
  private OptionAccessRights _optionAccessRights;
  private INamedImageList _namedImageList;
  private ICategoryTypeIconService _categoryTypeIconService;
  private INavGraphicsCache _navGraphicsCache;
  private ICurrentUserAndRole _currentUserAndRole;
  private IUserNamesCache _userNamesCache;
  private IPicturesCache _picturesCache;
  private OptionHolder _optionHolder = new OptionHolder();
  private OptionHolder _optionHolderBackup = new OptionHolder();
  private IServiceProvider _serviceProvider;
  private bool _isChanged;
  private static Dictionary<string, int> _columnWidthDictionary = new Dictionary<string, int>();
  private bool _developerMode;
  private static bool _thumbnailMode = true;
  private static bool _historyMode = false;
  private string _oldValue = string.Empty;
  private bool _hasChanges;
  private static StringFormat _imageStringFormat = new StringFormat();
  private EventHandler handlerDoDefaultView;
  private EventHandler handlerDoThumbnailsView;
  private static iGCellStyle cellStyle;
  private static iGCellStyle cellStyleStatus;
  private static iGCellStyle cellHistory;
  private static iGCellStyle cellCheckBox;
  private static iGCellStyle cellCheckBoxRO;
  private static iGCellStyle cellInt64;
  private static iGCellStyle cellInt64RO;
  private static iGCellStyle cellDouble;
  private static iGCellStyle cellDoubleRO;
  private static iGCellStyle cellDateTime;
  private static iGCellStyle cellDateTimeRO;
  private static iGCellStyle cellString;
  private static iGCellStyle cellStringRO;
  private static iGCellStyle cellImage;
  private static iGColHdrStyle headerStyle;
  private IContainer components;
  private MenuBar menuBar;
  private ContextMenuBarItem contextMenuBarItem;
  private MenuButtonItem _addOptionValueMenuButtonItem;
  private MenuButtonItem _deleteOptionValuesMenuButtonItem;
  private MenuButtonItem _moveUpOptionValuesMenuButtonItem;
  private MenuButtonItem _moveDownOptionValuesMenuButtonItem;
  private Panel panelTop;
  private Label lbCaption;
  private Label lbCode;
  private Label lbDataType;
  private Label lbCategory;
  private Label lbDescr;
  private ImageList imagesToolbars;
  private Intermech.Bars.ToolBar toolBarTop;
  private Intermech.Bars.ToolBar toolBarRight;
  private ButtonItem _moveUpOptionValuesButtonItem;
  private ButtonItem _moveDownOptionValuesButtonItem;
  private ButtonItem _moveTopOptionValuesButtonItem;
  private ButtonItem _moveBottomOptionValuesButtonItem;
  private iGrid _optionValuesGrid;
  private Panel panelMain;
  private ButtonItem _addOptionValueButtonItem;
  private ButtonItem _deleteOptionValuesButtonItem;
  private MenuButtonItem _moveTopOptionValuesMenuButtonItem;
  private MenuButtonItem _moveBottomOptionValuesMenuButtonItem;
  private ImageList imagesGrid;
  private DropDownMenuItem _viewDropDownMenuItem;
  private MenuButtonItem _defaultViewMenuButtonItem;
  private MenuButtonItem _thumbnailsViewMenuButtonItem;
  private ButtonItem _historyButtonItem;
  private ButtonItem _restoreOptionValuesButtonItem;
  private MenuButtonItem _restoreOptionValuesMenuButtonItem;
  private ComboBox _optionTypeComboBox;
  private TextBox _optionDescriptionTextBox;
  private TextBox _optionCategoryTextBox;
  private TextBox _optionCodeTextBox;
  private TextBox _optionNameTextBox;
  private CheckBox _obsoleteOptionCheckBox;
  private ButtonItem _addOptionValueRangeButtonItem;

  public event EventHandler Changed;

  public OptionEditor()
  {
    this.InitializeComponent();
    if (ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      service.RendererChanged += new EventHandler(this.BarManager_RendererChanged);
      this.BarManager_RendererChanged((object) service, EventArgs.Empty);
    }
    if (!(ServicesManager.GetService(typeof (IGuidMapper)) is IGuidMapper))
      return;
    this.Init();
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool ReadOnly
  {
    [DebuggerStepThrough] get => this._readOnly;
    set => this._readOnly = value;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public OptionAccessRights AccessRights
  {
    [DebuggerStepThrough] get => this._optionAccessRights;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public long CurrentOptionObjectID
  {
    get => this._optionHolder.OptionObjectID;
    set
    {
      Exception exception = (Exception) null;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        this.Clear();
        if (!(sessionKeeper.Session.GetObject(value, false) is IDBConfiguratorOption source))
          return;
        try
        {
          this._optionHolder.Assign((object) source);
        }
        catch (Exception ex)
        {
          this.Clear();
          exception = ex;
        }
      }
      this.FillEditor(true);
      this.Fix();
      if (exception == null)
        return;
      ExceptionHelper.ExceptionService.ShowException(exception);
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public OptionHolder Option
  {
    get => this._optionHolder.Clone() as OptionHolder;
    set
    {
      this._optionHolder.Assign((object) value);
      this.FillEditor(true);
      this.Fix();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public IServiceProvider Services
  {
    [DebuggerStepThrough] get => this._serviceProvider;
    set => this._serviceProvider = value;
  }

  [Category("Appearance")]
  [Browsable(true)]
  public bool IsChanged
  {
    [DebuggerStepThrough] get => this._isChanged;
    set
    {
      this._isChanged = value;
      this.OnChanged();
      this.UpdateControls();
    }
  }

  public void Clear()
  {
    this._optionHolder.Clear();
    this.FillEditor(false);
  }

  public void Fix()
  {
    this._optionHolderBackup.Assign((object) this._optionHolder);
    this._isChanged = false;
    this.UpdateControls();
    this.OnChanged();
  }

  public void Undo()
  {
    this.Option = this._optionHolderBackup;
    this.OnChanged();
  }

  private void BarManager_RendererChanged(object sender, EventArgs e)
  {
    IToolBarRenderer renderer = (sender as BarManager).Renderer;
    this.menuBar.Renderer = renderer;
    this.toolBarRight.Renderer = renderer;
  }

  private void OptionNameTextBox_TextChanged(object sender, EventArgs e)
  {
    this.EditValueChanged(sender, e);
  }

  private void OptionCodeTextBox_TextChanged(object sender, EventArgs e)
  {
    this.EditValueChanged(sender, e);
  }

  private void OptionCategoryTextBox_TextChanged(object sender, EventArgs e)
  {
    this.EditValueChanged(sender, e);
  }

  private void OptionCategoryTextBox_Click(object sender, EventArgs e)
  {
    if (!(this._optionCategoryTextBox.Tag is ObjectVersionDescription))
      return;
    object[] objArray = SelectionWindow.Select(LocalizationHolder.rm.GetString("PdmConfigurator_80"), LocalizationHolder.rm.GetString("PdmConfigurator_81"), (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionsGroupID), typeof (IDBTypedObjectID), SelectionOptions.Default | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect);
    if (objArray == null || objArray.Length == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this._optionHolder.OptionCategory = ((IDBTypedObjectID) objArray[0]).ObjectID;
      ObjectVersionDescription versionDescription = ObjectVersionDescriptionsHelper.LoadDescription(sessionKeeper.Session, typeof (ObjectVersionDescription), this._optionHolder.OptionCategory) as ObjectVersionDescription;
      this._optionCategoryTextBox.Tag = (object) versionDescription;
      this._optionCategoryTextBox.Text = versionDescription != null ? $"{versionDescription.CAPTION}" : string.Empty;
    }
    this.IsChanged = true;
  }

  private void OptionDescriptionTextBox_TextChanged(object sender, EventArgs e)
  {
    this.EditValueChanged(sender, e);
  }

  private void OptionTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.EditValueChanged(sender, e);
  }

  private void ObsoleteOptionCheckBox_CheckedChanged(object sender, EventArgs e)
  {
    this.ObsoleteOption();
  }

  private void DefaultViewMenuButtonItem_Click(object sender, EventArgs e) => this.DefaultView();

  private void ThumbnailsViewMenuButtonItem_Click(object sender, EventArgs e)
  {
    this.ThumbnailsView();
  }

  private void AddOptionValueButtonItem_Click(object sender, EventArgs e) => this.AddOptionValue();

  private void AddOptionValueRangeButtonItem_Click(object sender, EventArgs e)
  {
    this.AddOptionValueRange();
  }

  private void DeleteOptionValuesButtonItem_Click(object sender, EventArgs e)
  {
    this.DeleteOptionValues();
  }

  private void HistoryButtonItem_Click(object sender, EventArgs e) => this.History();

  private void RestoreOptionValuesButtonItem_Click(object sender, EventArgs e)
  {
    this.RestoreOptionValues();
  }

  private void ObsoleteOptionButtonItem_Click(object sender, EventArgs e) => this.ObsoleteOption();

  private void MoveTopOptionValuesButtonItem_Click(object sender, EventArgs e)
  {
    this.MoveTopOptionValues();
  }

  private void MoveUpOptionValuesButtonItem_Click(object sender, EventArgs e)
  {
    this.MoveUpOptionValues();
  }

  private void MoveDownOptionValuesButtonItem_Click(object sender, EventArgs e)
  {
    this.MoveDownOptionValues();
  }

  private void MoveBottomOptionValuesButtonItem_Click(object sender, EventArgs e)
  {
    this.MoveBottomOptionValues();
  }

  private void AddOptionValueMenuButtonItem_Click(object sender, EventArgs e)
  {
    this.AddOptionValue();
  }

  private void DeleteOptionValuesMenuButtonItem_Click(object sender, EventArgs e)
  {
    this.DeleteOptionValues();
  }

  private void RestoreOptionValuesMenuButtonItem_Click(object sender, EventArgs e)
  {
    this.RestoreOptionValues();
  }

  private void MoveTopOptionValuesMenuButtonItem_Click(object sender, EventArgs e)
  {
    this.MoveTopOptionValues();
  }

  private void MoveUpOptionValuesMenuButtonItem_Click(object sender, EventArgs e)
  {
    this.MoveUpOptionValues();
  }

  private void MoveDownOptionValuesMenuButtonItem_Click(object sender, EventArgs e)
  {
    this.MoveDownOptionValues();
  }

  private void MoveBottomOptionValuesMenuButtonItem_Click(object sender, EventArgs e)
  {
    this.MoveBottomOptionValues();
  }

  private void OptionValuesGrid_SelectionChanged(object sender, EventArgs e)
  {
    this.UpdateControls();
  }

  private void OptionValuesGrid_CellClick(object sender, iGCellClickEventArgs e)
  {
    if (this._optionAccessRights != OptionAccessRights.FullAccess || this._obsoleteOptionCheckBox.Checked || this._optionValuesGrid.Cols[e.ColIndex].Key != "IMAGE")
      return;
    iGRow row = this._optionValuesGrid.Rows[e.RowIndex];
    iGCell cell = row.Cells[e.ColIndex];
    OptionValue optionValue = (OptionValue) row.Cells["TAG"].Value;
    if (optionValue == null || (optionValue.Flags & OptionValueFlags.Locked) == OptionValueFlags.Locked || (optionValue.Flags & OptionValueFlags.Obsolete) == OptionValueFlags.Obsolete)
      return;
    iGrid optionValuesGrid = this._optionValuesGrid;
    Point location = cell.TextBounds.Location;
    int x = location.X;
    location = cell.TextBounds.Location;
    int y = location.Y + cell.TextBounds.Height;
    Point p = new Point(x, y);
    Point screen = optionValuesGrid.PointToScreen(p);
    using (LibraryImagePopupControl imagePopupControl = new LibraryImagePopupControl())
    {
      if (imagePopupControl.Execute(screen, new Size(0, 0), (IServiceProvider) null, (object) optionValue.Image) != DialogResult.OK)
        return;
      Guid image = optionValue.Image;
      Guid guid1 = (Guid) imagePopupControl.Value;
      Guid guid2 = guid1;
      if (!(image != guid2))
        return;
      cell.ImageIndex = guid1 != Guid.Empty ? 1 : -1;
      optionValue.Image = guid1;
      optionValue.User = this._currentUserAndRole.UserGuid;
      optionValue.LastModified = DateTime.UtcNow;
      this.SetCellsStyle(row);
      row.AutoHeight();
      this.IsChanged = true;
    }
  }

  private void OptionValuesGrid_EllipsisBtnClick(object sender, iGEllipsisBtnClickEventArgs e)
  {
    if (this._optionValuesGrid.Cols[e.ColIndex].Key != "VALUE")
      return;
    iGRow row = this._optionValuesGrid.Rows[e.RowIndex];
    iGCell cell = row.Cells[e.ColIndex];
    OptionValue optionValue = (OptionValue) row.Cells["TAG"].Value;
    if (optionValue == null || (optionValue.Flags & OptionValueFlags.Locked) == OptionValueFlags.Locked || (optionValue.Flags & OptionValueFlags.Obsolete) == OptionValueFlags.Obsolete)
      return;
    Point screen = this._optionValuesGrid.PointToScreen(new Point(cell.TextBounds.Location.X, cell.TextBounds.Location.Y + cell.TextBounds.Height));
    if (this._optionHolder.OptionDataType != FieldTypes.ftDateTime)
      return;
    using (DateTimePopupControl timePopupControl = new DateTimePopupControl())
    {
      if (timePopupControl.Execute(screen, new Size(0, 0), (IServiceProvider) null, (object) this._optionHolder.GetAsDateTime(optionValue.ID)) != DialogResult.OK)
        return;
      string str1 = cell.Value.ToString();
      string shortDateString = ((DateTime) timePopupControl.Value).ToShortDateString();
      string str2 = shortDateString;
      if (!(str1 != str2))
        return;
      cell.Value = (object) shortDateString;
      this._optionHolder.SetAsDateTime(optionValue.ID, shortDateString);
      optionValue.User = this._currentUserAndRole.UserGuid;
      optionValue.LastModified = DateTime.UtcNow;
      this.SetCellsStyle(row);
      this.IsChanged = true;
    }
  }

  private void OptionValuesGrid_BeforeCommitEdit(object sender, iGBeforeCommitEditEventArgs e)
  {
    this._hasChanges = false;
    if (e.NewValue == null)
      e.NewValue = (object) string.Empty;
    if (this._optionValuesGrid.Cols[e.ColIndex].Key == "VALUE")
    {
      this._oldValue = this._optionHolder.OptionValues[e.RowIndex].Value;
      switch (this._optionHolder.OptionDataType)
      {
        case FieldTypes.ftString:
          if (!this._optionHolder.SetAsString(this._optionHolder.OptionValues[e.RowIndex].ID, e.NewText))
          {
            e.Result = iGEditResult.Proceed;
            break;
          }
          break;
        case FieldTypes.ftInteger:
          if (!this._optionHolder.SetAsInt64(this._optionHolder.OptionValues[e.RowIndex].ID, e.NewText))
          {
            e.Result = iGEditResult.Proceed;
            break;
          }
          break;
        case FieldTypes.ftDouble:
          if (!this._optionHolder.SetAsDouble(this._optionHolder.OptionValues[e.RowIndex].ID, e.NewText))
          {
            e.Result = iGEditResult.Proceed;
            break;
          }
          break;
        case FieldTypes.ftDateTime:
          if (!this._optionHolder.SetAsDateTime(this._optionHolder.OptionValues[e.RowIndex].ID, e.NewText))
          {
            e.Result = iGEditResult.Proceed;
            break;
          }
          break;
        case FieldTypes.ftBoolean:
          if (!this._optionHolder.SetAsBoolean(this._optionHolder.OptionValues[e.RowIndex].ID, (bool) e.NewValue))
          {
            e.Result = iGEditResult.Proceed;
            break;
          }
          break;
      }
      this._hasChanges = this._oldValue != this._optionHolder.OptionValues[e.RowIndex].Value;
    }
    else
    {
      switch (this._optionValuesGrid.Cols[e.ColIndex].Key)
      {
        case "CODE":
          this._oldValue = this._optionHolder.OptionValues[e.RowIndex].Code;
          break;
        case "NOTE":
          this._oldValue = this._optionHolder.OptionValues[e.RowIndex].Description;
          break;
      }
    }
  }

  private void OptionValuesGrid_AfterCommitEdit(object sender, iGAfterCommitEditEventArgs e)
  {
    iGRow row = this._optionValuesGrid.Rows[e.RowIndex];
    object obj = row.Cells[e.ColIndex].Value;
    OptionValue optionValue = this._optionHolder.OptionValues[e.RowIndex];
    switch (this._optionValuesGrid.Cols[e.ColIndex].Key)
    {
      case "CODE":
        optionValue.Code = obj.ToString();
        this._hasChanges = optionValue.Code != this._oldValue;
        break;
      case "NOTE":
        optionValue.Description = obj.ToString();
        this._hasChanges = optionValue.Description != this._oldValue;
        break;
    }
    try
    {
      if (!this._hasChanges)
        return;
      this.IsChanged = true;
      optionValue.User = this._currentUserAndRole.UserGuid;
      optionValue.LastModified = DateTime.UtcNow;
      this.SetCellsStyle(row);
      row.AutoHeight();
    }
    finally
    {
      this._oldValue = string.Empty;
      this._hasChanges = false;
    }
  }

  private void OptionValuesGrid_ColWidthChanging(object sender, iGColWidthEventArgs e)
  {
    this._optionValuesGrid.Rows.AutoHeight();
  }

  private void OptionValuesGrid_ColWidthEndChange(object sender, iGColWidthEventArgs e)
  {
    OptionEditor._columnWidthDictionary[this._optionValuesGrid.Cols[e.ColIndex].Key] = e.Width;
    this._optionValuesGrid.Rows.AutoHeight();
  }

  private void OptionValuesGrid_CustomDrawCellForeground(object sender, iGCustomDrawCellEventArgs e)
  {
    iGCol col = this._optionValuesGrid.Cols[e.ColIndex];
    if (col.Key == "STATUS" || !OptionEditor._thumbnailMode || col.Key != "IMAGE")
      return;
    object picture = !(this._optionValuesGrid.Rows[e.RowIndex].Cells["TAG"].Value is OptionValue optionValue) || !(optionValue.Image != Guid.Empty) ? (object) null : this._picturesCache.GetPicture(optionValue.Image);
    if (picture == null)
      return;
    Rectangle imageBounds;
    ref Rectangle local = ref imageBounds;
    int x = e.Bounds.Left + 1;
    Rectangle bounds = e.Bounds;
    int y = bounds.Top + 1;
    bounds = e.Bounds;
    int width = bounds.Width - 2;
    bounds = e.Bounds;
    int height = bounds.Height - 2;
    local = new Rectangle(x, y, width, height);
    ThumbnailRenderer.GetImageObjectSizeAdv(picture, imageBounds);
    ThumbnailRenderer.DrawImageObjectAdv(e.Graphics, picture, imageBounds, this._optionValuesGrid.Font, OptionEditor._imageStringFormat);
  }

  private void OptionValuesGrid_CustomDrawCellGetHeight(
    object sender,
    iGCustomDrawCellGetHeightEventArgs e)
  {
    iGCol col = this._optionValuesGrid.Cols[e.ColIndex];
    if (col.Key == "STATUS")
    {
      e.Height = this._optionValuesGrid.DefaultRow.Height;
    }
    else
    {
      e.Height = this._optionValuesGrid.DefaultRow.Height;
      if (col.Key != "IMAGE")
        return;
      iGRow row = this._optionValuesGrid.Rows[e.RowIndex];
      object picture = !(row.Cells["TAG"].Value is OptionValue optionValue) || !(optionValue.Image != Guid.Empty) ? (object) null : this._picturesCache.GetPicture(optionValue.Image);
      if (picture == null)
        return;
      Size imageObjectSizeAdv = ThumbnailRenderer.GetImageObjectSizeAdv(picture, row.Cells[e.ColIndex].Bounds);
      if (imageObjectSizeAdv.Height == 0 || imageObjectSizeAdv.Width == 0)
        return;
      double num1 = (double) imageObjectSizeAdv.Width / (double) row.Cells[e.ColIndex].Bounds.Width;
      double num2 = (double) imageObjectSizeAdv.Height / num1;
      e.Height = Math.Max(e.Height, Convert.ToInt32(num2));
    }
  }

  private void OptionValuesGrid_KeyUp(object sender, KeyEventArgs e)
  {
    if (e.KeyData == Keys.Insert)
    {
      if (!this.CanAddOptionValue())
        return;
      this.AddOptionValueButtonItem_Click(sender, (EventArgs) e);
    }
    else
    {
      if (e.KeyData != Keys.Delete || !this.CanDeleteOptionValues())
        return;
      this.DeleteOptionValuesButtonItem_Click(sender, (EventArgs) e);
    }
  }

  private void DefaultView()
  {
    if (this._inEvents)
      return;
    OptionEditor._thumbnailMode = false;
    this.SetHandlers();
    this.FillGrid();
    this.UpdateControls();
  }

  private void ThumbnailsView()
  {
    if (this._inEvents)
      return;
    OptionEditor._thumbnailMode = true;
    this.SetHandlers();
    this.FillGrid();
    this.UpdateControls();
  }

  private void AddOptionValue()
  {
    if (this._optionAccessRights != OptionAccessRights.FullAccess || this._obsoleteOptionCheckBox.Checked)
      return;
    if (this._optionValuesGrid.IsEditing)
      this._optionValuesGrid.CommitEditCurCell();
    OptionValue optionValue = this._optionHolder.NewValue();
    optionValue.User = this._currentUserAndRole.UserGuid;
    optionValue.LastModified = DateTime.UtcNow;
    this._optionHolder.OptionValues.Add(optionValue);
    iGRow row = this.AddValue(optionValue);
    this.SetCellsStyle(row);
    this._optionValuesGrid.SetCurRow(row.Index);
    this._optionValuesGrid.SetCurCell(row.Index, "VALUE");
    this._optionValuesGrid.RequestEditCurCell();
    row.AutoHeight();
    this.IsChanged = true;
  }

  private void AddOptionValueRange()
  {
    this._optionValuesGrid.CommitEditCurCell();
    using (CompositionsConfiguratorOptionValueRangeCreatorDialog rangeCreatorDialog = new CompositionsConfiguratorOptionValueRangeCreatorDialog())
    {
      if (rangeCreatorDialog.ShowDialog() != DialogResult.OK)
        return;
      foreach (double num in rangeCreatorDialog.Control.CreateValueRange())
      {
        string optionValueValueAsString = this._optionHolder.OptionDataType == FieldTypes.ftInteger ? ((int) num).ToString((IFormatProvider) CultureInfo.InvariantCulture) : num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        if (this._optionHolder.OptionValues.Where<OptionValue>((Func<OptionValue, bool>) (o => o.Value == optionValueValueAsString && !o.Flags.HasFlag((Enum) OptionValueFlags.Obsolete))).Count<OptionValue>() == 0)
        {
          OptionValue optionValue = this._optionHolder.NewValue();
          optionValue.User = this._currentUserAndRole.UserGuid;
          optionValue.LastModified = DateTime.UtcNow;
          optionValue.Value = optionValueValueAsString;
          this._optionHolder.OptionValues.Add(optionValue);
          iGRow row = this.AddValue(optionValue);
          this.SetCellsStyle(row);
          this._optionValuesGrid.SetCurRow(row.Index);
          this._optionValuesGrid.SetCurCell(row.Index, "VALUE");
          row.AutoHeight();
        }
      }
      this.IsChanged = true;
    }
  }

  private void DeleteOptionValues()
  {
    OptionValue[] selectedOptionValues = this.GetSelectedOptionValues();
    foreach (OptionValue optionValue in selectedOptionValues)
    {
      if (optionValue.Flags.HasFlag((Enum) OptionValueFlags.Obsolete))
      {
        this._optionHolder.OptionValues.Remove(optionValue);
      }
      else
      {
        optionValue.Flags |= OptionValueFlags.Obsolete;
        optionValue.Flags &= ~OptionValueFlags.Recovered;
      }
    }
    this.FillGrid();
    if (this._historyButtonItem.Checked)
      this.SelectRowForOptionValues((IEnumerable<OptionValue>) selectedOptionValues);
    this.IsChanged = true;
  }

  private void History()
  {
    if (this._inEvents || this._optionAccessRights != OptionAccessRights.FullAccess)
      return;
    OptionEditor._historyMode = this._historyButtonItem.Checked;
    this.FillGrid();
    this.UpdateControls();
  }

  private void RestoreOptionValues()
  {
    OptionValue[] selectedOptionValues = this.GetSelectedOptionValues();
    foreach (OptionValue optionValue in selectedOptionValues)
    {
      optionValue.Flags &= ~OptionValueFlags.Obsolete;
      optionValue.Flags |= OptionValueFlags.Recovered;
    }
    this.FillGrid();
    this.SelectRowForOptionValues((IEnumerable<OptionValue>) selectedOptionValues);
    this.IsChanged = true;
  }

  private void ObsoleteOption()
  {
    if (this.ReadOnly || this._inEvents)
      return;
    if (this._optionValuesGrid.IsEditing)
      this._optionValuesGrid.CommitEditCurCell();
    if (this._obsoleteOptionCheckBox.Checked)
      this._optionHolder.OptionFlags |= OptionFlags.Obsolete;
    else
      this._optionHolder.OptionFlags &= ~OptionFlags.Obsolete;
    this.SetRowsStyle();
    this.IsChanged = true;
  }

  private void MoveTopOptionValues()
  {
    OptionValue[] array = ((IEnumerable<iGRow>) this.GetSelectedRows()).OrderBy<iGRow, int>((Func<iGRow, int>) (o => o.Index)).Reverse<iGRow>().Select<iGRow, OptionValue>((Func<iGRow, OptionValue>) (o => this.GetOptionValueForRow(o))).ToArray<OptionValue>();
    foreach (OptionValue optionValue in array)
      this._optionHolder.OptionValues.Move(this._optionHolder.OptionValues.IndexOf(optionValue), 0);
    this.FillGrid();
    this.SelectRowForOptionValues((IEnumerable<OptionValue>) array);
    this.IsChanged = true;
  }

  private void MoveUpOptionValues()
  {
    OptionValue[] array = ((IEnumerable<iGRow>) this.GetSelectedRows()).OrderBy<iGRow, int>((Func<iGRow, int>) (o => o.Index)).Select<iGRow, OptionValue>((Func<iGRow, OptionValue>) (o => this.GetOptionValueForRow(o))).ToArray<OptionValue>();
    foreach (OptionValue optionValue in array)
    {
      int index = this._optionHolder.OptionValues.IndexOf(optionValue);
      this._optionHolder.OptionValues.Move(index, this.FindIndexBeforeCurrent(index));
    }
    this.FillGrid();
    this.SelectRowForOptionValues((IEnumerable<OptionValue>) array);
    this.IsChanged = true;
  }

  private int FindIndexBeforeCurrent(int index)
  {
    if (index <= 0)
      return 0;
    if (index > this._optionHolder.OptionValues.Count - 1)
      return this._optionHolder.OptionValues.Count - 1;
    for (int index1 = index - 1; index1 >= 0; --index1)
    {
      if (OptionEditor._historyMode || !this._optionHolder.OptionValues[index1].Flags.HasFlag((Enum) OptionValueFlags.Obsolete))
        return index1;
    }
    return 0;
  }

  private int FindIndexAfterCurrent(int index)
  {
    if (index < 0)
      return 0;
    if (index >= this._optionHolder.OptionValues.Count - 1)
      return this._optionHolder.OptionValues.Count - 1;
    for (int index1 = index + 1; index1 <= this._optionHolder.OptionValues.Count - 1; ++index1)
    {
      if (OptionEditor._historyMode || !this._optionHolder.OptionValues[index1].Flags.HasFlag((Enum) OptionValueFlags.Obsolete))
        return index1;
    }
    return this._optionHolder.OptionValues.Count - 1;
  }

  private void MoveDownOptionValues()
  {
    OptionValue[] array = ((IEnumerable<iGRow>) this.GetSelectedRows()).OrderBy<iGRow, int>((Func<iGRow, int>) (o => o.Index)).Reverse<iGRow>().Select<iGRow, OptionValue>((Func<iGRow, OptionValue>) (o => this.GetOptionValueForRow(o))).ToArray<OptionValue>();
    foreach (OptionValue optionValue in array)
    {
      int index = this._optionHolder.OptionValues.IndexOf(optionValue);
      this._optionHolder.OptionValues.Move(index, this.FindIndexAfterCurrent(index));
    }
    this.FillGrid();
    this.SelectRowForOptionValues((IEnumerable<OptionValue>) array);
    this.IsChanged = true;
  }

  private void MoveBottomOptionValues()
  {
    OptionValue[] array = ((IEnumerable<iGRow>) this.GetSelectedRows()).OrderBy<iGRow, int>((Func<iGRow, int>) (o => o.Index)).Select<iGRow, OptionValue>((Func<iGRow, OptionValue>) (o => this.GetOptionValueForRow(o))).ToArray<OptionValue>();
    foreach (OptionValue optionValue in array)
      this._optionHolder.OptionValues.Move(this._optionHolder.OptionValues.IndexOf(optionValue), this._optionHolder.OptionValues.Count - 1);
    this.FillGrid();
    this.SelectRowForOptionValues((IEnumerable<OptionValue>) array);
    this.IsChanged = true;
  }

  private bool CanObsoleteOption()
  {
    return !this.ReadOnly && this._optionAccessRights.HasFlag((Enum) OptionAccessRights.FullAccess);
  }

  private bool CanAddOptionValue()
  {
    return !this.ReadOnly && this._optionAccessRights.HasFlag((Enum) OptionAccessRights.FullAccess) && !this._obsoleteOptionCheckBox.Checked;
  }

  private bool CanAddOptionValueRange()
  {
    if (this.ReadOnly || !this._optionAccessRights.HasFlag((Enum) OptionAccessRights.FullAccess) || this._obsoleteOptionCheckBox.Checked)
      return false;
    return this._optionHolder.OptionDataType == FieldTypes.ftDouble || this._optionHolder.OptionDataType == FieldTypes.ftInteger;
  }

  private bool CanDeleteOptionValues()
  {
    OptionValue[] selectedOptionValues = this.GetSelectedOptionValues();
    if (this.ReadOnly || !this._optionAccessRights.HasFlag((Enum) OptionAccessRights.FullAccess) || this._obsoleteOptionCheckBox.Checked || selectedOptionValues.Length == 0)
      return false;
    return selectedOptionValues.Length == this.CountNotLockedAndNotObsoleteOptionValues((IEnumerable<OptionValue>) selectedOptionValues) || this._developerMode;
  }

  private bool CanRestoreOptionValues()
  {
    OptionValue[] selectedOptionValues = this.GetSelectedOptionValues();
    return !this.ReadOnly && this._optionAccessRights.HasFlag((Enum) OptionAccessRights.FullAccess) && !this._obsoleteOptionCheckBox.Checked && this._historyButtonItem.Checked && selectedOptionValues.Length != 0 && selectedOptionValues.Length == this.CountNotLockedAndObsoleteOptionValues((IEnumerable<OptionValue>) selectedOptionValues);
  }

  private bool CanMoveTopOptionValues()
  {
    iGRow[] selectedRows = this.GetSelectedRows();
    return !this.ReadOnly && this._optionAccessRights.HasFlag((Enum) OptionAccessRights.FullAccess) && selectedRows.Length != 0 && ((IEnumerable<iGRow>) selectedRows).OrderBy<iGRow, int>((Func<iGRow, int>) (o => o.Index)).First<iGRow>().Index > 0;
  }

  private bool CanMoveUpOptionValues()
  {
    iGRow[] selectedRows = this.GetSelectedRows();
    return !this.ReadOnly && this._optionAccessRights.HasFlag((Enum) OptionAccessRights.FullAccess) && selectedRows.Length != 0 && ((IEnumerable<iGRow>) selectedRows).OrderBy<iGRow, int>((Func<iGRow, int>) (o => o.Index)).First<iGRow>().Index > 0;
  }

  private bool CanMoveDownOptionValues()
  {
    iGRow[] selectedRows = this.GetSelectedRows();
    return !this.ReadOnly && this._optionAccessRights.HasFlag((Enum) OptionAccessRights.FullAccess) && selectedRows.Length != 0 && ((IEnumerable<iGRow>) selectedRows).OrderBy<iGRow, int>((Func<iGRow, int>) (o => o.Index)).Last<iGRow>().Index < this._optionValuesGrid.Rows.Count - 1;
  }

  private bool CanMoveBottomOptionValues()
  {
    iGRow[] selectedRows = this.GetSelectedRows();
    return !this.ReadOnly && this._optionAccessRights.HasFlag((Enum) OptionAccessRights.FullAccess) && selectedRows.Length != 0 && ((IEnumerable<iGRow>) selectedRows).OrderBy<iGRow, int>((Func<iGRow, int>) (o => o.Index)).Last<iGRow>().Index < this._optionValuesGrid.Rows.Count - 1;
  }

  private OptionValue[] GetSelectedOptionValues()
  {
    return ((IEnumerable<iGRow>) this.GetSelectedRows()).Select<iGRow, OptionValue>((Func<iGRow, OptionValue>) (o => this.GetOptionValueForRow(o))).ToArray<OptionValue>();
  }

  private int CountNotLockedAndNotObsoleteOptionValues(IEnumerable<OptionValue> optionValues)
  {
    return optionValues.Where<OptionValue>((Func<OptionValue, bool>) (o => !o.Flags.HasFlag((Enum) OptionValueFlags.Locked) && !o.Flags.HasFlag((Enum) OptionValueFlags.Obsolete))).Count<OptionValue>();
  }

  private int CountNotLockedAndObsoleteOptionValues(IEnumerable<OptionValue> optionValues)
  {
    return optionValues.Where<OptionValue>((Func<OptionValue, bool>) (o => !o.Flags.HasFlag((Enum) OptionValueFlags.Locked) && o.Flags.HasFlag((Enum) OptionValueFlags.Obsolete))).Count<OptionValue>();
  }

  private iGRow[] GetSelectedRows()
  {
    List<iGRow> iGrowList = new List<iGRow>();
    foreach (iGCell selectedCell in this._optionValuesGrid.SelectedCells)
    {
      if (!iGrowList.Contains(selectedCell.Row))
        iGrowList.Add(selectedCell.Row);
    }
    return iGrowList.ToArray();
  }

  private OptionValue GetOptionValueForRow(iGRow row) => (OptionValue) row.Cells["TAG"].Value;

  private void SelectRowForOptionValues(IEnumerable<OptionValue> optionValues)
  {
    foreach (OptionValue optionValue in optionValues)
    {
      foreach (iGRow row in (IEnumerable) this._optionValuesGrid.Rows)
      {
        if (optionValue == this.GetOptionValueForRow(row))
        {
          row.Cells["VALUE"].Selected = true;
          row.EnsureVisible();
        }
      }
    }
  }

  private void OnChanged()
  {
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, EventArgs.Empty);
  }

  private int GetTypeImageIndex(FieldTypes attrType)
  {
    return this._categoryTypeIconService == null ? -1 : this._categoryTypeIconService.IndexOf(3, -1, (object) attrType);
  }

  private void Init()
  {
    this._namedImageList = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this._picturesCache = ServicesManager.GetService(typeof (IPicturesCache)) as IPicturesCache;
    this._categoryTypeIconService = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this._navGraphicsCache = ServicesManager.GetService(typeof (INavGraphicsCache)) as INavGraphicsCache;
    this._currentUserAndRole = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    this._userNamesCache = CacheManager.Cache("UserNamesCache") as IUserNamesCache;
    this._isChanged = false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this._developerMode = sessionKeeper.Session.DeveloperMode;
    this.SetHandlers();
    this.UpdateControls();
  }

  private void CheckAccessRights()
  {
    this._optionAccessRights = OptionAccessRights.ReadOnly;
    if (this._optionHolder.OptionObjectID == 0L)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._optionHolder.OptionObjectID, false);
      if (dbObject == null)
        return;
      if (dbObject is IDBSecurity dbSecurity && dbSecurity.CheckAccess(ActionType.Edit, true, false))
        this._optionAccessRights = OptionAccessRights.FullAccess;
      if (dbObject.ObjectModifyMode != ObjectModifyModes.CantModify && (dbObject.ObjectModifyMode != ObjectModifyModes.Checkout || dbObject.CheckoutBy == this._currentUserAndRole.UserID) && (dbObject.ObjectModifyMode != ObjectModifyModes.CreateVersion || dbObject.CheckoutBy == this._currentUserAndRole.UserID))
        return;
      this._optionAccessRights = OptionAccessRights.ReadOnly;
    }
  }

  private void UpdateControls()
  {
    this._optionNameTextBox.Enabled = this._optionCodeTextBox.Enabled = this._optionCategoryTextBox.Enabled = this.CanEditOptionValues();
    this._optionTypeComboBox.Enabled = this.CanEditOptionValues() && this._optionHolder.OptionValues.Count == 0;
    this._optionDescriptionTextBox.Enabled = this.CanEditOptionValues();
    this._obsoleteOptionCheckBox.Enabled = this.CanObsoleteOption();
    this._addOptionValueButtonItem.Enabled = this._addOptionValueMenuButtonItem.Enabled = this.CanAddOptionValue();
    this._addOptionValueRangeButtonItem.Enabled = this.CanAddOptionValueRange();
    this._deleteOptionValuesButtonItem.Enabled = this._deleteOptionValuesMenuButtonItem.Enabled = this.CanDeleteOptionValues();
    this._restoreOptionValuesButtonItem.Enabled = this._restoreOptionValuesMenuButtonItem.Enabled = this.CanRestoreOptionValues();
    this._restoreOptionValuesButtonItem.Visible = this._restoreOptionValuesMenuButtonItem.Visible = this._historyButtonItem.Checked;
    this._moveTopOptionValuesButtonItem.Enabled = this._moveTopOptionValuesMenuButtonItem.Enabled = this.CanMoveTopOptionValues();
    this._moveUpOptionValuesButtonItem.Enabled = this._moveUpOptionValuesMenuButtonItem.Enabled = this.CanMoveUpOptionValues();
    this._moveDownOptionValuesButtonItem.Enabled = this._moveDownOptionValuesMenuButtonItem.Enabled = this.CanMoveDownOptionValues();
    this._moveBottomOptionValuesButtonItem.Enabled = this._moveBottomOptionValuesMenuButtonItem.Enabled = this.CanMoveBottomOptionValues();
    this._optionValuesGrid.ReadOnly = !this.CanEditOptionValues();
  }

  private bool CanEditOptionValues()
  {
    return !this.ReadOnly && this._optionAccessRights.HasFlag((Enum) OptionAccessRights.FullAccess);
  }

  private void SetCellsStyle(iGRow row)
  {
    if (row == null || !(row.Cells["TAG"].Value is OptionValue optionValue))
      return;
    bool flag = (this.ReadOnly ? 0 : ((this._optionAccessRights & OptionAccessRights.FullAccess) != 0 ? 1 : 0)) != 0 && (optionValue.Flags & OptionValueFlags.Obsolete) != OptionValueFlags.Obsolete && (optionValue.Flags & OptionValueFlags.Locked) != OptionValueFlags.Locked;
    switch (this._optionHolder.OptionDataType)
    {
      case FieldTypes.ftInteger:
        row.Cells["VALUE"].Style = flag ? OptionEditor.cellInt64 : OptionEditor.cellInt64RO;
        break;
      case FieldTypes.ftDouble:
        row.Cells["VALUE"].Style = flag ? OptionEditor.cellDouble : OptionEditor.cellDoubleRO;
        break;
      case FieldTypes.ftDateTime:
        row.Cells["VALUE"].Style = flag ? OptionEditor.cellDateTime : OptionEditor.cellDateTimeRO;
        break;
      case FieldTypes.ftBoolean:
        row.Cells["VALUE"].Style = flag ? OptionEditor.cellCheckBox : OptionEditor.cellCheckBoxRO;
        break;
      default:
        row.Cells["VALUE"].Style = flag ? OptionEditor.cellString : OptionEditor.cellStringRO;
        break;
    }
    row.Cells["CODE"].Style = flag ? OptionEditor.cellString : OptionEditor.cellStringRO;
    row.Cells["NOTE"].Style = flag ? OptionEditor.cellString : OptionEditor.cellStringRO;
    row.Cells["FLAGS"].ImageList = this.imagesToolbars;
    row.Cells["FLAGS"].ImageIndex = -1;
    if ((optionValue.Flags & OptionValueFlags.Obsolete) == OptionValueFlags.Obsolete)
      row.Cells["FLAGS"].ImageIndex = 11;
    if ((optionValue.Flags & OptionValueFlags.Recovered) == OptionValueFlags.Recovered)
      row.Cells["FLAGS"].ImageIndex = 12;
    if ((optionValue.Flags & OptionValueFlags.Locked) == OptionValueFlags.Locked)
      row.Cells["FLAGS"].ImageIndex = 13;
    row.Cells["USER"].Value = (object) this._userNamesCache.GetUserName(optionValue.User);
    row.Cells["DATE"].ValueType = typeof (DateTime);
    row.Cells["DATE"].Value = (object) optionValue.LastModified.ToLocalTime();
    for (int colIndex = 0; colIndex < row.Cells.Count; ++colIndex)
      row.Cells[colIndex].ForeColor = !this._obsoleteOptionCheckBox.Checked ? SystemColors.ControlText : SystemColors.GrayText;
    if (this._obsoleteOptionCheckBox.Checked || !flag)
    {
      for (int colIndex = 0; colIndex < row.Cells.Count; ++colIndex)
        row.Cells[colIndex].BackColor = Color.LavenderBlush;
    }
    else
    {
      for (int colIndex = 0; colIndex < row.Cells.Count; ++colIndex)
        row.Cells[colIndex].BackColor = SystemColors.Window;
    }
  }

  private void SetRowsStyle()
  {
    for (int index = 0; index < this._optionValuesGrid.Rows.Count; ++index)
      this.SetCellsStyle(this._optionValuesGrid.Rows[index]);
  }

  private void FillGrid()
  {
    bool inEvents = this._inEvents;
    this._optionValuesGrid.BeginUpdate();
    try
    {
      this._inEvents = true;
      this._optionValuesGrid.Rows.Clear();
      this.PrepareGridsColumns();
      if (this._optionHolder != null)
      {
        for (int index = 0; index < this._optionHolder.OptionValues.Count; ++index)
          this.SetCellsStyle(this.AddValue(this._optionHolder.OptionValues[index]));
      }
      this._optionValuesGrid.Rows.AutoHeight();
    }
    finally
    {
      this._inEvents = inEvents;
      this._optionValuesGrid.EndUpdate();
      this._optionValuesGrid.Update();
    }
    this._optionValuesGrid.Rows.AutoHeight();
  }

  public virtual void FillEditor(bool checkAccess)
  {
    if (checkAccess)
      this.CheckAccessRights();
    bool inEvents = this._inEvents;
    try
    {
      this._inEvents = true;
      if (this._optionHolder != null)
      {
        this._optionNameTextBox.Text = this._optionHolder.OptionCaption;
        this._optionCodeTextBox.Text = this._optionHolder.OptionCode;
        List<MyElement> supportedTypes = Intermech.Interfaces.PdmConfigurator.Helper.GetSupportedTypes();
        MyElement typeElement = Intermech.Interfaces.PdmConfigurator.Helper.GetTypeElement(this._optionHolder.OptionDataType);
        this._optionTypeComboBox.BeginUpdate();
        try
        {
          this._optionTypeComboBox.Items.Clear();
          this._optionTypeComboBox.Items.AddRange((object[]) supportedTypes.ToArray());
        }
        finally
        {
          this._optionTypeComboBox.EndUpdate();
        }
        this._optionTypeComboBox.SelectedIndex = supportedTypes.IndexOf(typeElement);
        this._optionTypeComboBox.Enabled = this._optionHolder.OptionValues.Count == 0;
        this._optionDescriptionTextBox.Text = this._optionHolder.OptionDescription;
        this._obsoleteOptionCheckBox.Checked = (this._optionHolder.OptionFlags & OptionFlags.Obsolete) == OptionFlags.Obsolete;
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          sessionKeeper.Session.GetCustomService(typeof (ICompositionLoadService));
          ObjectVersionDescription versionDescription = ObjectVersionDescriptionsHelper.LoadDescription(sessionKeeper.Session, typeof (ObjectVersionDescription), this._optionHolder.OptionCategory) as ObjectVersionDescription;
          this._optionCategoryTextBox.Tag = (object) versionDescription;
          this._optionCategoryTextBox.Text = versionDescription != null ? $"{versionDescription.CAPTION}" : string.Empty;
        }
      }
      else
      {
        this._optionNameTextBox.Text = string.Empty;
        this._optionCodeTextBox.Text = string.Empty;
        this._optionTypeComboBox.SelectedIndex = -1;
        this._optionTypeComboBox.BeginUpdate();
        try
        {
          this._optionTypeComboBox.Items.Clear();
        }
        finally
        {
          this._optionTypeComboBox.EndUpdate();
        }
        this._optionCategoryTextBox.Text = string.Empty;
        this._optionCategoryTextBox.Tag = (object) null;
        this._optionDescriptionTextBox.Text = string.Empty;
      }
    }
    finally
    {
      this._inEvents = inEvents;
    }
    this.FillGrid();
    this.UpdateControls();
    this.OnChanged();
  }

  private void EditValueChanged(object sender, EventArgs e)
  {
    if (this.ReadOnly || this._inEvents)
      return;
    this._optionHolder.OptionCaption = this._optionNameTextBox.Text;
    this._optionHolder.OptionCode = this._optionCodeTextBox.Text;
    this._optionHolder.OptionDescription = this._optionDescriptionTextBox.Text;
    if (this._optionTypeComboBox.SelectedItem is MyElement selectedItem && this._optionHolder.OptionValues.Count == 0)
      this._optionHolder.OptionDataType = (FieldTypes) selectedItem.Value;
    if (sender == this._optionTypeComboBox)
      this.PrepareGridsColumns();
    this.IsChanged = true;
  }

  private iGRow AddValue(OptionValue value)
  {
    iGRow iGrow = this._optionValuesGrid.Rows.Add();
    iGrow.Cells["IMAGE"].Value = (object) value.Image;
    iGrow.Cells["CODE"].Value = (object) value.Code;
    switch (this._optionHolder.OptionDataType)
    {
      case FieldTypes.ftInteger:
        iGrow.Cells["VALUE"].Value = (object) this._optionHolder.GetAsInt64(value.ID);
        break;
      case FieldTypes.ftDouble:
        iGrow.Cells["VALUE"].Value = (object) this._optionHolder.GetAsDouble(value.ID);
        break;
      case FieldTypes.ftDateTime:
        DateTime asDateTime = this._optionHolder.GetAsDateTime(value.ID);
        iGrow.Cells["VALUE"].Value = (object) asDateTime.ToShortDateString();
        break;
      case FieldTypes.ftBoolean:
        iGrow.Cells["VALUE"].Value = (object) this._optionHolder.GetAsBoolean(value.ID);
        break;
      default:
        iGrow.Cells["VALUE"].Value = (object) this._optionHolder.GetAsString(value.ID);
        break;
    }
    iGrow.Cells["NOTE"].Value = (object) value.Description;
    iGrow.Cells["ID"].Value = (object) value.ID;
    iGrow.Cells["TAG"].Value = (object) value;
    iGrow.Height = 20;
    if ((value.Flags & OptionValueFlags.Obsolete) == OptionValueFlags.Obsolete)
      iGrow.Visible = OptionEditor._historyMode;
    return iGrow;
  }

  private int GetOptionValueIndexForMove()
  {
    if (this._optionAccessRights != OptionAccessRights.FullAccess || this._obsoleteOptionCheckBox.Checked || this._optionValuesGrid.SelectedCells.Count == 0)
      return -1;
    iGRow row = this._optionValuesGrid.SelectedCells[0].Row;
    int index = row.Index;
    OptionValue optionValue = (OptionValue) row.Cells["TAG"].Value;
    return optionValue == null || this._optionHolder.OptionValues.IndexOf(optionValue) < 0 ? -1 : this._optionHolder.OptionValues.IndexOf(optionValue);
  }

  private void MoveOptionValue(int index, int newIndex)
  {
    this._optionHolder.OptionValues.Move(index, newIndex);
    this.FillGrid();
    this._optionValuesGrid.SetCurRow(newIndex);
    this.IsChanged = true;
  }

  private void SetHandlers()
  {
    if (this.handlerDoDefaultView == null)
    {
      this.handlerDoDefaultView = new EventHandler(this.DefaultViewMenuButtonItem_Click);
      this.handlerDoThumbnailsView = new EventHandler(this.ThumbnailsViewMenuButtonItem_Click);
    }
    bool inEvents = this._inEvents;
    try
    {
      this._historyButtonItem.Checked = OptionEditor._historyMode;
      if (OptionEditor._thumbnailMode)
      {
        this._viewDropDownMenuItem.Text = this._thumbnailsViewMenuButtonItem.Text;
        this._viewDropDownMenuItem.ToolTipText = this._thumbnailsViewMenuButtonItem.ToolTipText;
        this._viewDropDownMenuItem.ImageIndex = this._thumbnailsViewMenuButtonItem.ImageIndex;
        this._viewDropDownMenuItem.Click -= this.handlerDoThumbnailsView;
        this._viewDropDownMenuItem.Click -= this.handlerDoDefaultView;
        this._viewDropDownMenuItem.Click += this.handlerDoDefaultView;
        this._defaultViewMenuButtonItem.Checked = false;
        this._thumbnailsViewMenuButtonItem.Checked = true;
      }
      else
      {
        this._viewDropDownMenuItem.Text = this._defaultViewMenuButtonItem.Text;
        this._viewDropDownMenuItem.ToolTipText = this._defaultViewMenuButtonItem.ToolTipText;
        this._viewDropDownMenuItem.ImageIndex = this._defaultViewMenuButtonItem.ImageIndex;
        this._viewDropDownMenuItem.Click -= this.handlerDoDefaultView;
        this._viewDropDownMenuItem.Click -= this.handlerDoThumbnailsView;
        this._viewDropDownMenuItem.Click += this.handlerDoThumbnailsView;
        this._defaultViewMenuButtonItem.Checked = true;
        this._thumbnailsViewMenuButtonItem.Checked = false;
      }
    }
    finally
    {
      this._inEvents = inEvents;
    }
  }

  private void PrepareGridsColumns()
  {
    this.PrepareGridsStyles();
    this._optionValuesGrid.Header.ImageList = this._categoryTypeIconService.ImageList;
    FieldTypes optionDataType = this._optionHolder.OptionDataType;
    bool flag = !this.ReadOnly && (this._optionAccessRights & OptionAccessRights.FullAccess) != OptionAccessRights.ReadOnly && !this._obsoleteOptionCheckBox.Checked;
    if (OptionEditor._columnWidthDictionary.Count == 0)
    {
      OptionEditor._columnWidthDictionary.Add("IMAGE", 48 /*0x30*/);
      OptionEditor._columnWidthDictionary.Add("CODE", 128 /*0x80*/);
      OptionEditor._columnWidthDictionary.Add("VALUE", 192 /*0xC0*/);
      OptionEditor._columnWidthDictionary.Add("NOTE", 192 /*0xC0*/);
      OptionEditor._columnWidthDictionary.Add("FLAGS", 64 /*0x40*/);
      OptionEditor._columnWidthDictionary.Add("USER", 128 /*0x80*/);
      OptionEditor._columnWidthDictionary.Add("DATE", 128 /*0x80*/);
      OptionEditor._columnWidthDictionary.Add("ID", 64 /*0x40*/);
      OptionEditor._columnWidthDictionary.Add("TAG", 0);
      OptionEditor._columnWidthDictionary.Add("STATUS", 0);
      int num = this._optionValuesGrid.ClientRectangle.Width - 30 - OptionEditor._columnWidthDictionary["IMAGE"] - OptionEditor._columnWidthDictionary["CODE"] - OptionEditor._columnWidthDictionary["VALUE"] - OptionEditor._columnWidthDictionary["FLAGS"] - OptionEditor._columnWidthDictionary["USER"] - OptionEditor._columnWidthDictionary["DATE"];
      if (num > 64 /*0x40*/)
        OptionEditor._columnWidthDictionary["NOTE"] = num;
    }
    iGCol iGcol1 = this._optionValuesGrid.Cols["IMAGE"] ?? this._optionValuesGrid.Cols.Add(new iGColPattern(Math.Max(36, OptionEditor._columnWidthDictionary["IMAGE"]), true, true, 36, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_38"), "IMAGE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol1.CellStyle = OptionEditor.cellImage;
    iGcol1.Width = OptionEditor._columnWidthDictionary["IMAGE"];
    iGcol1.ColHdrStyle = OptionEditor.headerStyle;
    iGcol1.Visible = OptionEditor._thumbnailMode;
    iGCol iGcol2 = this._optionValuesGrid.Cols["VALUE"] ?? this._optionValuesGrid.Cols.Add(new iGColPattern(Math.Max(64 /*0x40*/, OptionEditor._columnWidthDictionary["VALUE"]), true, true, 64 /*0x40*/, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_43"), "VALUE", this.GetTypeImageIndex(optionDataType), (object) string.Empty, (object) string.Empty, -1));
    iGcol2.Width = OptionEditor._columnWidthDictionary["VALUE"];
    iGcol2.ColHdrStyle = OptionEditor.headerStyle;
    iGcol2.ImageIndex = this.GetTypeImageIndex(optionDataType);
    switch (optionDataType)
    {
      case FieldTypes.ftInteger:
        iGcol2.CellStyle = flag ? OptionEditor.cellInt64 : OptionEditor.cellInt64RO;
        break;
      case FieldTypes.ftDouble:
        iGcol2.CellStyle = flag ? OptionEditor.cellDouble : OptionEditor.cellDoubleRO;
        break;
      case FieldTypes.ftDateTime:
        iGcol2.CellStyle = flag ? OptionEditor.cellDateTime : OptionEditor.cellDateTimeRO;
        break;
      case FieldTypes.ftBoolean:
        iGcol2.CellStyle = flag ? OptionEditor.cellCheckBox : OptionEditor.cellCheckBoxRO;
        break;
      default:
        iGcol2.CellStyle = flag ? OptionEditor.cellString : OptionEditor.cellStringRO;
        break;
    }
    iGCol iGcol3 = this._optionValuesGrid.Cols["CODE"] ?? this._optionValuesGrid.Cols.Add(new iGColPattern(Math.Max(64 /*0x40*/, OptionEditor._columnWidthDictionary["CODE"]), true, true, 64 /*0x40*/, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_44"), "CODE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol3.CellStyle = flag ? OptionEditor.cellString : OptionEditor.cellStringRO;
    iGcol3.Width = OptionEditor._columnWidthDictionary["CODE"];
    iGcol3.ColHdrStyle = OptionEditor.headerStyle;
    iGCol iGcol4 = this._optionValuesGrid.Cols["NOTE"] ?? this._optionValuesGrid.Cols.Add(new iGColPattern(Math.Max(64 /*0x40*/, OptionEditor._columnWidthDictionary["NOTE"]), true, true, 64 /*0x40*/, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_40"), "NOTE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol4.CellStyle = flag ? OptionEditor.cellString : OptionEditor.cellStringRO;
    iGcol4.Width = OptionEditor._columnWidthDictionary["NOTE"];
    iGcol4.ColHdrStyle = OptionEditor.headerStyle;
    iGCol iGcol5 = this._optionValuesGrid.Cols["FLAGS"] ?? this._optionValuesGrid.Cols.Add(new iGColPattern(Math.Max(36, OptionEditor._columnWidthDictionary["FLAGS"]), OptionEditor._historyMode, true, 36, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_41"), "FLAGS", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol5.CellStyle = OptionEditor.cellHistory;
    iGcol5.Width = OptionEditor._columnWidthDictionary["FLAGS"];
    iGcol5.ColHdrStyle = OptionEditor.headerStyle;
    iGcol5.Visible = OptionEditor._historyMode;
    iGCol iGcol6 = this._optionValuesGrid.Cols["USER"] ?? this._optionValuesGrid.Cols.Add(new iGColPattern(Math.Max(64 /*0x40*/, OptionEditor._columnWidthDictionary["USER"]), OptionEditor._historyMode, true, 64 /*0x40*/, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_76"), "USER", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol6.CellStyle = OptionEditor.cellHistory;
    iGcol6.Width = OptionEditor._columnWidthDictionary["USER"];
    iGcol6.ColHdrStyle = OptionEditor.headerStyle;
    iGcol6.Visible = OptionEditor._historyMode;
    iGCol iGcol7 = this._optionValuesGrid.Cols["DATE"] ?? this._optionValuesGrid.Cols.Add(new iGColPattern(Math.Max(64 /*0x40*/, OptionEditor._columnWidthDictionary["DATE"]), OptionEditor._historyMode, true, 64 /*0x40*/, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_77"), "DATE", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol7.CellStyle = OptionEditor.cellHistory;
    iGcol7.Width = OptionEditor._columnWidthDictionary["DATE"];
    iGcol7.ColHdrStyle = OptionEditor.headerStyle;
    iGcol7.Visible = OptionEditor._historyMode;
    iGCol iGcol8 = this._optionValuesGrid.Cols["ID"] ?? this._optionValuesGrid.Cols.Add(new iGColPattern(Math.Max(64 /*0x40*/, OptionEditor._columnWidthDictionary["ID"]), this._developerMode, true, 64 /*0x40*/, -1, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_45"), "ID", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol8.CellStyle = OptionEditor.cellStyle;
    iGcol8.Width = OptionEditor._columnWidthDictionary["ID"];
    iGcol8.ColHdrStyle = OptionEditor.headerStyle;
    iGcol8.Visible = false;
    (this._optionValuesGrid.Cols["TAG"] ?? this._optionValuesGrid.Cols.Add(new iGColPattern(OptionEditor._columnWidthDictionary["TAG"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "TAG", -1, (object) null, (object) null, -1))).Width = OptionEditor._columnWidthDictionary["TAG"];
    iGCol iGcol9 = this._optionValuesGrid.Cols["STATUS"] ?? this._optionValuesGrid.Cols.Add(new iGColPattern(OptionEditor._columnWidthDictionary["STATUS"], true, false, 0, -1, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) "", "STATUS", -1, (object) null, (object) null, -1));
    iGcol9.Width = OptionEditor._columnWidthDictionary["STATUS"];
    iGcol9.CellStyle = OptionEditor.cellStyleStatus;
  }

  private void PrepareGridsStyles()
  {
    if (OptionEditor.cellStyle != null)
      return;
    OptionEditor.cellStyle = new iGCellStyle(true);
    OptionEditor.cellStyle.ReadOnly = iGBool.True;
    OptionEditor.cellStyle.SingleClickEdit = iGBool.False;
    OptionEditor.cellStyle.TextAlign = iGContentAlignment.TopLeft;
    OptionEditor.cellStyle.TextFormatFlags = iGStringFormatFlags.WordWrap;
    OptionEditor.cellHistory = new iGCellStyle(true);
    OptionEditor.cellHistory.ImageList = this.imagesToolbars;
    OptionEditor.cellHistory.ImageAlign = iGContentAlignment.TopCenter;
    OptionEditor.cellHistory.ReadOnly = iGBool.True;
    OptionEditor.cellHistory.SingleClickEdit = iGBool.False;
    OptionEditor.cellHistory.TextAlign = iGContentAlignment.TopLeft;
    OptionEditor.cellHistory.TextFormatFlags = iGStringFormatFlags.WordWrap;
    OptionEditor.cellCheckBox = new iGCellStyle(true);
    OptionEditor.cellCheckBox.EmptyStringAs = iGEmptyStringAs.EmptyString;
    OptionEditor.cellCheckBox.ImageAlign = iGContentAlignment.MiddleLeft;
    OptionEditor.cellCheckBox.ReadOnly = iGBool.False;
    OptionEditor.cellCheckBox.SingleClickEdit = iGBool.True;
    OptionEditor.cellCheckBox.TextAlign = iGContentAlignment.TopLeft;
    OptionEditor.cellCheckBox.Type = iGCellType.Check;
    OptionEditor.cellCheckBox.ValueType = typeof (bool);
    OptionEditor.cellCheckBoxRO = OptionEditor.cellCheckBox.Clone();
    OptionEditor.cellCheckBoxRO.SingleClickEdit = iGBool.False;
    OptionEditor.cellCheckBoxRO.ReadOnly = iGBool.True;
    OptionEditor.cellInt64 = new iGCellStyle(true);
    OptionEditor.cellInt64.EmptyStringAs = iGEmptyStringAs.EmptyString;
    OptionEditor.cellInt64.ReadOnly = iGBool.False;
    OptionEditor.cellInt64.SingleClickEdit = iGBool.True;
    OptionEditor.cellInt64.TextAlign = iGContentAlignment.TopLeft;
    OptionEditor.cellInt64.ValueType = typeof (long);
    OptionEditor.cellInt64RO = OptionEditor.cellInt64.Clone();
    OptionEditor.cellInt64RO.ReadOnly = iGBool.True;
    OptionEditor.cellInt64RO.SingleClickEdit = iGBool.False;
    OptionEditor.cellDouble = new iGCellStyle(true);
    OptionEditor.cellDouble.EmptyStringAs = iGEmptyStringAs.EmptyString;
    OptionEditor.cellDouble.ReadOnly = iGBool.False;
    OptionEditor.cellDouble.SingleClickEdit = iGBool.True;
    OptionEditor.cellDouble.TextAlign = iGContentAlignment.TopLeft;
    OptionEditor.cellDouble.ValueType = typeof (string);
    OptionEditor.cellDoubleRO = OptionEditor.cellDouble.Clone();
    OptionEditor.cellDoubleRO.ReadOnly = iGBool.True;
    OptionEditor.cellDoubleRO.SingleClickEdit = iGBool.False;
    OptionEditor.cellDateTime = new iGCellStyle(true);
    OptionEditor.cellDateTime.EmptyStringAs = iGEmptyStringAs.EmptyString;
    OptionEditor.cellDateTime.ReadOnly = iGBool.False;
    OptionEditor.cellDateTime.SingleClickEdit = iGBool.True;
    OptionEditor.cellDateTime.TextAlign = iGContentAlignment.TopLeft;
    OptionEditor.cellDateTime.TypeFlags = iGCellTypeFlags.HasEllipsisBtn;
    OptionEditor.cellDateTime.ValueType = typeof (string);
    OptionEditor.cellDateTimeRO = OptionEditor.cellDateTime.Clone();
    OptionEditor.cellDateTimeRO.ReadOnly = iGBool.True;
    OptionEditor.cellDateTimeRO.SingleClickEdit = iGBool.False;
    OptionEditor.cellDateTimeRO.TypeFlags = iGCellTypeFlags.None;
    OptionEditor.cellString = new iGCellStyle(true);
    OptionEditor.cellString.EmptyStringAs = iGEmptyStringAs.EmptyString;
    OptionEditor.cellString.ReadOnly = iGBool.False;
    OptionEditor.cellString.SingleClickEdit = iGBool.True;
    OptionEditor.cellString.TextAlign = iGContentAlignment.TopLeft;
    OptionEditor.cellString.ValueType = typeof (string);
    OptionEditor.cellStringRO = OptionEditor.cellString.Clone();
    OptionEditor.cellStringRO.ReadOnly = iGBool.True;
    OptionEditor.cellStringRO.SingleClickEdit = iGBool.False;
    OptionEditor.cellImage = new iGCellStyle(true);
    OptionEditor.cellImage.CustomDrawFlags = iGCustomDrawFlags.Foreground;
    OptionEditor.cellImage.EmptyStringAs = iGEmptyStringAs.EmptyString;
    OptionEditor.cellImage.ReadOnly = iGBool.True;
    OptionEditor.cellImage.SingleClickEdit = iGBool.False;
    OptionEditor.cellImage.TextAlign = iGContentAlignment.TopCenter;
    OptionEditor.cellImage.ValueType = typeof (Image);
    OptionEditor.cellStyleStatus = OptionEditor.cellImage.Clone();
    OptionEditor.cellStyleStatus.ImageAlign = iGContentAlignment.TopCenter;
    OptionEditor.headerStyle = new iGColHdrStyle(true);
    OptionEditor.headerStyle.TextAlign = iGContentAlignment.TopLeft;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      this.toolBarRight.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.menuBar.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      service.RendererChanged -= new EventHandler(this.BarManager_RendererChanged);
    }
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OptionEditor));
    this.menuBar = new MenuBar();
    this.imagesToolbars = new ImageList(this.components);
    this.contextMenuBarItem = new ContextMenuBarItem();
    this._addOptionValueMenuButtonItem = new MenuButtonItem();
    this._deleteOptionValuesMenuButtonItem = new MenuButtonItem();
    this._restoreOptionValuesMenuButtonItem = new MenuButtonItem();
    this._moveTopOptionValuesMenuButtonItem = new MenuButtonItem();
    this._moveUpOptionValuesMenuButtonItem = new MenuButtonItem();
    this._moveDownOptionValuesMenuButtonItem = new MenuButtonItem();
    this._moveBottomOptionValuesMenuButtonItem = new MenuButtonItem();
    this._optionValuesGrid = new iGrid();
    this.panelTop = new Panel();
    this._obsoleteOptionCheckBox = new CheckBox();
    this._optionTypeComboBox = new ComboBox();
    this._optionDescriptionTextBox = new TextBox();
    this._optionCategoryTextBox = new TextBox();
    this._optionCodeTextBox = new TextBox();
    this._optionNameTextBox = new TextBox();
    this.lbDescr = new Label();
    this.lbCategory = new Label();
    this.lbDataType = new Label();
    this.lbCode = new Label();
    this.lbCaption = new Label();
    this.toolBarTop = new Intermech.Bars.ToolBar();
    this._viewDropDownMenuItem = new DropDownMenuItem();
    this._defaultViewMenuButtonItem = new MenuButtonItem();
    this._thumbnailsViewMenuButtonItem = new MenuButtonItem();
    this._historyButtonItem = new ButtonItem();
    this._addOptionValueButtonItem = new ButtonItem();
    this._addOptionValueRangeButtonItem = new ButtonItem();
    this._deleteOptionValuesButtonItem = new ButtonItem();
    this._restoreOptionValuesButtonItem = new ButtonItem();
    this.toolBarRight = new Intermech.Bars.ToolBar();
    this._moveTopOptionValuesButtonItem = new ButtonItem();
    this._moveUpOptionValuesButtonItem = new ButtonItem();
    this._moveDownOptionValuesButtonItem = new ButtonItem();
    this._moveBottomOptionValuesButtonItem = new ButtonItem();
    this.panelMain = new Panel();
    this.imagesGrid = new ImageList(this.components);
    ((ISupportInitialize) this._optionValuesGrid).BeginInit();
    this.panelTop.SuspendLayout();
    this.panelMain.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.menuBar, "menuBar");
    this.menuBar.Guid = new Guid("0909a734-928b-4c5d-9a6d-05be64690c06");
    this.menuBar.Hidden = false;
    this.menuBar.ImageList = this.imagesToolbars;
    this.menuBar.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.contextMenuBarItem
    });
    this.menuBar.Name = "menuBar";
    this.menuBar.OwnerForm = (Form) null;
    this.imagesToolbars.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesToolbars.ImageStream");
    this.imagesToolbars.TransparentColor = Color.Transparent;
    this.imagesToolbars.Images.SetKeyName(0, "add.png");
    this.imagesToolbars.Images.SetKeyName(1, "delete.png");
    this.imagesToolbars.Images.SetKeyName(2, "arrow_up_blue.ico");
    this.imagesToolbars.Images.SetKeyName(3, "arrow_down_blue.ico");
    this.imagesToolbars.Images.SetKeyName(4, "arrow_top_blue.ico");
    this.imagesToolbars.Images.SetKeyName(5, "arrow_bottom_blue.ico");
    this.imagesToolbars.Images.SetKeyName(6, "");
    this.imagesToolbars.Images.SetKeyName(7, "");
    this.imagesToolbars.Images.SetKeyName(8, "EventLog2.ico");
    this.imagesToolbars.Images.SetKeyName(9, "image.ico");
    this.imagesToolbars.Images.SetKeyName(10, "photo_portrait.png");
    this.imagesToolbars.Images.SetKeyName(11, "garbage.png");
    this.imagesToolbars.Images.SetKeyName(12, "recycle.png");
    this.imagesToolbars.Images.SetKeyName(13, "lock_16.ico");
    componentResourceManager.ApplyResources((object) this.contextMenuBarItem, "contextMenuBarItem");
    this.contextMenuBarItem.Items.AddRange(new ToolbarItemBase[7]
    {
      (ToolbarItemBase) this._addOptionValueMenuButtonItem,
      (ToolbarItemBase) this._deleteOptionValuesMenuButtonItem,
      (ToolbarItemBase) this._restoreOptionValuesMenuButtonItem,
      (ToolbarItemBase) this._moveTopOptionValuesMenuButtonItem,
      (ToolbarItemBase) this._moveUpOptionValuesMenuButtonItem,
      (ToolbarItemBase) this._moveDownOptionValuesMenuButtonItem,
      (ToolbarItemBase) this._moveBottomOptionValuesMenuButtonItem
    });
    this.contextMenuBarItem.ShowText = true;
    this._addOptionValueMenuButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._addOptionValueMenuButtonItem, "_addOptionValueMenuButtonItem");
    this._addOptionValueMenuButtonItem.ImageIndex = 0;
    this._addOptionValueMenuButtonItem.ShowText = true;
    this._addOptionValueMenuButtonItem.Click += new EventHandler(this.AddOptionValueMenuButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._deleteOptionValuesMenuButtonItem, "_deleteOptionValuesMenuButtonItem");
    this._deleteOptionValuesMenuButtonItem.ImageIndex = 1;
    this._deleteOptionValuesMenuButtonItem.ShowText = true;
    this._deleteOptionValuesMenuButtonItem.Click += new EventHandler(this.DeleteOptionValuesMenuButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._restoreOptionValuesMenuButtonItem, "_restoreOptionValuesMenuButtonItem");
    this._restoreOptionValuesMenuButtonItem.ImageIndex = 11;
    this._restoreOptionValuesMenuButtonItem.ShowText = true;
    this._restoreOptionValuesMenuButtonItem.Click += new EventHandler(this.RestoreOptionValuesMenuButtonItem_Click);
    this._moveTopOptionValuesMenuButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._moveTopOptionValuesMenuButtonItem, "_moveTopOptionValuesMenuButtonItem");
    this._moveTopOptionValuesMenuButtonItem.ImageIndex = 4;
    this._moveTopOptionValuesMenuButtonItem.ShowText = true;
    this._moveTopOptionValuesMenuButtonItem.Click += new EventHandler(this.MoveTopOptionValuesMenuButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._moveUpOptionValuesMenuButtonItem, "_moveUpOptionValuesMenuButtonItem");
    this._moveUpOptionValuesMenuButtonItem.ImageIndex = 2;
    this._moveUpOptionValuesMenuButtonItem.ShowText = true;
    this._moveUpOptionValuesMenuButtonItem.Click += new EventHandler(this.MoveUpOptionValuesMenuButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._moveDownOptionValuesMenuButtonItem, "_moveDownOptionValuesMenuButtonItem");
    this._moveDownOptionValuesMenuButtonItem.ImageIndex = 3;
    this._moveDownOptionValuesMenuButtonItem.ShowText = true;
    this._moveDownOptionValuesMenuButtonItem.Click += new EventHandler(this.MoveDownOptionValuesMenuButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._moveBottomOptionValuesMenuButtonItem, "_moveBottomOptionValuesMenuButtonItem");
    this._moveBottomOptionValuesMenuButtonItem.ImageIndex = 5;
    this._moveBottomOptionValuesMenuButtonItem.ShowText = true;
    this._moveBottomOptionValuesMenuButtonItem.Click += new EventHandler(this.MoveBottomOptionValuesMenuButtonItem_Click);
    this._optionValuesGrid.BackColorEvenRows = Color.White;
    this._optionValuesGrid.DefaultAutoGroupRow.Height = 20;
    this._optionValuesGrid.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height");
    this._optionValuesGrid.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight");
    componentResourceManager.ApplyResources((object) this._optionValuesGrid, "_optionValuesGrid");
    this._optionValuesGrid.GridLines.GroupRows = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this._optionValuesGrid.GridLines.Horizontal = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this._optionValuesGrid.GridLines.HorizontalExtended = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this._optionValuesGrid.GridLines.HorizontalLastRow = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this._optionValuesGrid.GridLines.Vertical = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this._optionValuesGrid.GridLines.VerticalExtended = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this._optionValuesGrid.GridLines.VerticalLastCol = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this._optionValuesGrid.GroupBox.Text = componentResourceManager.GetString("_optionValuesGrid.GroupBox.Text");
    this._optionValuesGrid.Header.Height = (int) componentResourceManager.GetObject("_optionValuesGrid.Header.Height");
    this._optionValuesGrid.HighlightBackColorNoFocus = SystemColors.Highlight;
    this._optionValuesGrid.HotTracking = false;
    this._optionValuesGrid.Name = "_optionValuesGrid";
    this.menuBar.SetPopupMenu((Control) this._optionValuesGrid, (MenuBarItem) this.contextMenuBarItem);
    this._optionValuesGrid.PressedMouseMoveMode = iGPressedMouseMoveMode.Normal;
    this._optionValuesGrid.ProcessTab = false;
    this._optionValuesGrid.SelectionMode = iGSelectionMode.MultiExtended;
    this._optionValuesGrid.ShowControlsInAllCells = false;
    this._optionValuesGrid.SilentValidation = true;
    this._optionValuesGrid.CellClick += new iGCellClickEventHandler(this.OptionValuesGrid_CellClick);
    this._optionValuesGrid.EllipsisBtnClick += new iGEllipsisBtnClickEventHandler(this.OptionValuesGrid_EllipsisBtnClick);
    this._optionValuesGrid.CustomDrawCellForeground += new iGCustomDrawCellEventHandler(this.OptionValuesGrid_CustomDrawCellForeground);
    this._optionValuesGrid.CustomDrawCellGetHeight += new iGCustomDrawCellGetHeightEventHandler(this.OptionValuesGrid_CustomDrawCellGetHeight);
    this._optionValuesGrid.ColWidthEndChange += new iGColWidthEventHandler(this.OptionValuesGrid_ColWidthEndChange);
    this._optionValuesGrid.ColWidthChanging += new iGColWidthEventHandler(this.OptionValuesGrid_ColWidthChanging);
    this._optionValuesGrid.SelectionChanged += new EventHandler(this.OptionValuesGrid_SelectionChanged);
    this._optionValuesGrid.BeforeCommitEdit += new iGBeforeCommitEditEventHandler(this.OptionValuesGrid_BeforeCommitEdit);
    this._optionValuesGrid.AfterCommitEdit += new iGAfterCommitEditEventHandler(this.OptionValuesGrid_AfterCommitEdit);
    this._optionValuesGrid.KeyUp += new KeyEventHandler(this.OptionValuesGrid_KeyUp);
    this.panelTop.Controls.Add((Control) this._obsoleteOptionCheckBox);
    this.panelTop.Controls.Add((Control) this._optionTypeComboBox);
    this.panelTop.Controls.Add((Control) this._optionDescriptionTextBox);
    this.panelTop.Controls.Add((Control) this._optionCategoryTextBox);
    this.panelTop.Controls.Add((Control) this._optionCodeTextBox);
    this.panelTop.Controls.Add((Control) this._optionNameTextBox);
    this.panelTop.Controls.Add((Control) this.lbDescr);
    this.panelTop.Controls.Add((Control) this.lbCategory);
    this.panelTop.Controls.Add((Control) this.lbDataType);
    this.panelTop.Controls.Add((Control) this.lbCode);
    this.panelTop.Controls.Add((Control) this.lbCaption);
    componentResourceManager.ApplyResources((object) this.panelTop, "panelTop");
    this.panelTop.Name = "panelTop";
    componentResourceManager.ApplyResources((object) this._obsoleteOptionCheckBox, "_obsoleteOptionCheckBox");
    this._obsoleteOptionCheckBox.Name = "_obsoleteOptionCheckBox";
    this._obsoleteOptionCheckBox.UseVisualStyleBackColor = true;
    this._obsoleteOptionCheckBox.CheckedChanged += new EventHandler(this.ObsoleteOptionCheckBox_CheckedChanged);
    componentResourceManager.ApplyResources((object) this._optionTypeComboBox, "_optionTypeComboBox");
    this._optionTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
    this._optionTypeComboBox.FormattingEnabled = true;
    this._optionTypeComboBox.Name = "_optionTypeComboBox";
    this._optionTypeComboBox.SelectedIndexChanged += new EventHandler(this.OptionTypeComboBox_SelectedIndexChanged);
    componentResourceManager.ApplyResources((object) this._optionDescriptionTextBox, "_optionDescriptionTextBox");
    this._optionDescriptionTextBox.Name = "_optionDescriptionTextBox";
    this._optionDescriptionTextBox.TextChanged += new EventHandler(this.OptionDescriptionTextBox_TextChanged);
    componentResourceManager.ApplyResources((object) this._optionCategoryTextBox, "_optionCategoryTextBox");
    this._optionCategoryTextBox.Name = "_optionCategoryTextBox";
    this._optionCategoryTextBox.ReadOnly = true;
    this._optionCategoryTextBox.Click += new EventHandler(this.OptionCategoryTextBox_Click);
    this._optionCategoryTextBox.TextChanged += new EventHandler(this.OptionCategoryTextBox_TextChanged);
    componentResourceManager.ApplyResources((object) this._optionCodeTextBox, "_optionCodeTextBox");
    this._optionCodeTextBox.Name = "_optionCodeTextBox";
    this._optionCodeTextBox.TextChanged += new EventHandler(this.OptionCodeTextBox_TextChanged);
    componentResourceManager.ApplyResources((object) this._optionNameTextBox, "_optionNameTextBox");
    this._optionNameTextBox.Name = "_optionNameTextBox";
    this._optionNameTextBox.TextChanged += new EventHandler(this.OptionNameTextBox_TextChanged);
    componentResourceManager.ApplyResources((object) this.lbDescr, "lbDescr");
    this.lbDescr.Name = "lbDescr";
    componentResourceManager.ApplyResources((object) this.lbCategory, "lbCategory");
    this.lbCategory.Name = "lbCategory";
    componentResourceManager.ApplyResources((object) this.lbDataType, "lbDataType");
    this.lbDataType.Name = "lbDataType";
    componentResourceManager.ApplyResources((object) this.lbCode, "lbCode");
    this.lbCode.Name = "lbCode";
    componentResourceManager.ApplyResources((object) this.lbCaption, "lbCaption");
    this.lbCaption.Name = "lbCaption";
    this.toolBarTop.AddRemoveButtonsVisible = false;
    this.toolBarTop.AllowHorizontalDock = false;
    this.toolBarTop.DockLine = 3;
    this.toolBarTop.DrawActionsButton = false;
    this.toolBarTop.FullMenus = true;
    this.toolBarTop.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarTop.Hidden = false;
    this.toolBarTop.ImageList = this.imagesToolbars;
    this.toolBarTop.Items.AddRange(new ToolbarItemBase[6]
    {
      (ToolbarItemBase) this._viewDropDownMenuItem,
      (ToolbarItemBase) this._historyButtonItem,
      (ToolbarItemBase) this._addOptionValueButtonItem,
      (ToolbarItemBase) this._addOptionValueRangeButtonItem,
      (ToolbarItemBase) this._deleteOptionValuesButtonItem,
      (ToolbarItemBase) this._restoreOptionValuesButtonItem
    });
    componentResourceManager.ApplyResources((object) this.toolBarTop, "toolBarTop");
    this.toolBarTop.MinimumFloatingSize = new Size(250, 30);
    this.toolBarTop.Name = "toolBarTop";
    this.toolBarTop.Overflow = ToolBarOverflow.Wrap;
    this.toolBarTop.Stretch = true;
    this.toolBarTop.Tearable = false;
    componentResourceManager.ApplyResources((object) this._viewDropDownMenuItem, "_viewDropDownMenuItem");
    this._viewDropDownMenuItem.ImageIndex = 8;
    this._viewDropDownMenuItem.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this._defaultViewMenuButtonItem,
      (ToolbarItemBase) this._thumbnailsViewMenuButtonItem
    });
    this._viewDropDownMenuItem.MenuImageList = this.imagesToolbars;
    this._viewDropDownMenuItem.ShowText = true;
    this._defaultViewMenuButtonItem.Checked = true;
    componentResourceManager.ApplyResources((object) this._defaultViewMenuButtonItem, "_defaultViewMenuButtonItem");
    this._defaultViewMenuButtonItem.ImageIndex = 8;
    this._defaultViewMenuButtonItem.ShowText = true;
    this._defaultViewMenuButtonItem.Click += new EventHandler(this.DefaultViewMenuButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._thumbnailsViewMenuButtonItem, "_thumbnailsViewMenuButtonItem");
    this._thumbnailsViewMenuButtonItem.ImageIndex = 9;
    this._thumbnailsViewMenuButtonItem.ShowText = true;
    this._thumbnailsViewMenuButtonItem.Click += new EventHandler(this.ThumbnailsViewMenuButtonItem_Click);
    this._historyButtonItem.AutoToggle = AutoToggleType.Single;
    this._historyButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._historyButtonItem, "_historyButtonItem");
    this._historyButtonItem.ImageIndex = 10;
    this._historyButtonItem.ShowText = true;
    this._historyButtonItem.Click += new EventHandler(this.HistoryButtonItem_Click);
    this._addOptionValueButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._addOptionValueButtonItem, "_addOptionValueButtonItem");
    this._addOptionValueButtonItem.ImageIndex = 0;
    this._addOptionValueButtonItem.ShowText = true;
    this._addOptionValueButtonItem.Click += new EventHandler(this.AddOptionValueButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._addOptionValueRangeButtonItem, "_addOptionValueRangeButtonItem");
    this._addOptionValueRangeButtonItem.ImageIndex = 0;
    this._addOptionValueRangeButtonItem.ShowText = true;
    this._addOptionValueRangeButtonItem.Click += new EventHandler(this.AddOptionValueRangeButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._deleteOptionValuesButtonItem, "_deleteOptionValuesButtonItem");
    this._deleteOptionValuesButtonItem.ImageIndex = 1;
    this._deleteOptionValuesButtonItem.ShowText = true;
    this._deleteOptionValuesButtonItem.Click += new EventHandler(this.DeleteOptionValuesButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._restoreOptionValuesButtonItem, "_restoreOptionValuesButtonItem");
    this._restoreOptionValuesButtonItem.ImageIndex = 12;
    this._restoreOptionValuesButtonItem.ShowText = true;
    this._restoreOptionValuesButtonItem.Click += new EventHandler(this.RestoreOptionValuesButtonItem_Click);
    this.toolBarRight.AddRemoveButtonsVisible = false;
    this.toolBarRight.AllowHorizontalDock = false;
    componentResourceManager.ApplyResources((object) this.toolBarRight, "toolBarRight");
    this.toolBarRight.DockLine = 3;
    this.toolBarRight.DrawActionsButton = false;
    this.toolBarRight.Flow = ToolBarLayout.Vertical;
    this.toolBarRight.FullMenus = true;
    this.toolBarRight.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarRight.Hidden = false;
    this.toolBarRight.ImageList = this.imagesToolbars;
    this.toolBarRight.Items.AddRange(new ToolbarItemBase[4]
    {
      (ToolbarItemBase) this._moveTopOptionValuesButtonItem,
      (ToolbarItemBase) this._moveUpOptionValuesButtonItem,
      (ToolbarItemBase) this._moveDownOptionValuesButtonItem,
      (ToolbarItemBase) this._moveBottomOptionValuesButtonItem
    });
    this.toolBarRight.MinimumFloatingSize = new Size(250, 30);
    this.toolBarRight.Name = "toolBarRight";
    this.toolBarRight.Overflow = ToolBarOverflow.Wrap;
    this.toolBarRight.Stretch = true;
    this.toolBarRight.Tearable = false;
    componentResourceManager.ApplyResources((object) this._moveTopOptionValuesButtonItem, "_moveTopOptionValuesButtonItem");
    this._moveTopOptionValuesButtonItem.ImageIndex = 4;
    this._moveTopOptionValuesButtonItem.Click += new EventHandler(this.MoveTopOptionValuesButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._moveUpOptionValuesButtonItem, "_moveUpOptionValuesButtonItem");
    this._moveUpOptionValuesButtonItem.ImageIndex = 2;
    this._moveUpOptionValuesButtonItem.Click += new EventHandler(this.MoveUpOptionValuesButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._moveDownOptionValuesButtonItem, "_moveDownOptionValuesButtonItem");
    this._moveDownOptionValuesButtonItem.ImageIndex = 3;
    this._moveDownOptionValuesButtonItem.Click += new EventHandler(this.MoveDownOptionValuesButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._moveBottomOptionValuesButtonItem, "_moveBottomOptionValuesButtonItem");
    this._moveBottomOptionValuesButtonItem.ImageIndex = 5;
    this._moveBottomOptionValuesButtonItem.Click += new EventHandler(this.MoveBottomOptionValuesButtonItem_Click);
    this.panelMain.Controls.Add((Control) this._optionValuesGrid);
    this.panelMain.Controls.Add((Control) this.toolBarRight);
    this.panelMain.Controls.Add((Control) this.toolBarTop);
    this.panelMain.Controls.Add((Control) this.menuBar);
    componentResourceManager.ApplyResources((object) this.panelMain, "panelMain");
    this.panelMain.Name = "panelMain";
    this.imagesGrid.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesGrid.ImageStream");
    this.imagesGrid.TransparentColor = Color.Transparent;
    this.imagesGrid.Images.SetKeyName(0, "images.ico");
    this.imagesGrid.Images.SetKeyName(1, "images.ico");
    this.imagesGrid.Images.SetKeyName(2, "image_gray.ico");
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((Control) this.panelMain);
    this.Controls.Add((Control) this.panelTop);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Name = nameof (OptionEditor);
    ((ISupportInitialize) this._optionValuesGrid).EndInit();
    this.panelTop.ResumeLayout(false);
    this.panelTop.PerformLayout();
    this.panelMain.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}

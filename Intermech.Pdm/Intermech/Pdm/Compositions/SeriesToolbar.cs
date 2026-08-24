// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.SeriesToolbar
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class SeriesToolbar : IClientPluginsDataTransfer
{
  protected Intermech.Bars.ToolBar toolbar;
  private int maxItemsCount = 5;
  private ButtonItem btnApply;
  private ButtonItem btnSeriesOn;
  private ComboBoxItem cbMainArticle;
  private ButtonItem btnSelectMainArticle;
  private ComboBoxItem cbSeries;
  private ComboBoxItem cbDate;
  private ButtonItem btnDate;
  private INamedImageList _namedImageList;
  private IFiltrationService _filtrationService;
  private INotificationService _notificationService;
  private volatile bool _isInEvent;
  private MyElement itemNoMainArticle = new MyElement((object) 0L, "", (object) 0L);
  private MyElement itemNoSeries = new MyElement((object) int.MinValue, "", (object) int.MinValue);
  private MyElement itemNoDate = new MyElement((object) DateTime.MinValue, "", (object) DateTime.MinValue);

  public SeriesToolbar(IFiltrationService filtrationService)
  {
    if (this.toolbar != null)
      return;
    this._filtrationService = filtrationService;
  }

  public void Initialize(bool register)
  {
    this.CreateToolbar();
    this.InitServices(register);
  }

  protected virtual Guid guid => new Guid("{7D565FEB-526C-42ED-B67E-ACFED972AC3D}");

  public void CreateToolbar()
  {
    try
    {
      this._isInEvent = true;
      INamedImageList service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
      this.toolbar = new Intermech.Bars.ToolBar();
      this.toolbar.Name = LocalizationHolder.rm.GetString("Pdm_557");
      this.toolbar.Text = LocalizationHolder.rm.GetString("Pdm_558");
      this.toolbar.ImageList = service.ImageList;
      this.toolbar.Guid = new Guid("{76510669-A2FD-4925-BEA5-658B3B8ECB05}");
      this.toolbar.AddRemoveButtonsVisible = false;
      this.toolbar.AllowHorizontalDock = true;
      this.toolbar.AllowVerticalDock = false;
      this.toolbar.FullMenus = true;
      this.toolbar.MinimumFloatingSize = new Size(450, 30);
      this.toolbar.Size = new Size(640, 26);
      this.cbMainArticle = new ComboBoxItem();
      this.cbMainArticle.BeginGroup = true;
      this.cbMainArticle.CommandName = "PDM.Series.MainArticle";
      this.cbMainArticle.MinimumControlWidth = 250;
      this.cbMainArticle.Stretch = true;
      this.cbMainArticle.Text = LocalizationHolder.rm.GetString("Pdm_559");
      this.cbMainArticle.ToolTipText = LocalizationHolder.rm.GetString("Pdm_560");
      this.cbMainArticle.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbMainArticle.ComboBox.DropDownWidth = 400;
      this.cbMainArticle.ComboBox.SelectedIndexChanged += new EventHandler(this.DoSeriesOn);
      this.AlterItem(this.cbMainArticle.ComboBox, this.itemNoMainArticle, true);
      this.btnSelectMainArticle = new ButtonItem();
      this.btnSelectMainArticle.CommandName = "PDM.Series.SelectMainArticle";
      this.btnSelectMainArticle.ShowText = false;
      this.btnSelectMainArticle.ToolTipText = LocalizationHolder.rm.GetString("Pdm_561");
      this.btnSelectMainArticle.Click += new EventHandler(this.DoSelectMainArticle);
      this.btnSelectMainArticle.ImageIndex = service.ImageIndex("imgSelectObjects");
      this.cbSeries = new ComboBoxItem();
      this.cbSeries.BeginGroup = true;
      this.cbSeries.CommandName = "PDM.Series.Series";
      this.cbSeries.MinimumControlWidth = 150;
      this.cbSeries.Stretch = false;
      this.cbSeries.Text = LocalizationHolder.rm.GetString("Pdm_562");
      this.cbSeries.ToolTipText = LocalizationHolder.rm.GetString("Pdm_563");
      this.cbSeries.ComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cbSeries.ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
      this.cbSeries.ComboBox.DropDownWidth = 250;
      this.cbSeries.ComboBox.SelectedIndexChanged += new EventHandler(this.DoSeriesOn);
      this.AlterItem(this.cbSeries.ComboBox, this.itemNoSeries, true);
      this.cbDate = new ComboBoxItem();
      this.cbDate.BeginGroup = false;
      this.cbDate.CommandName = "PDM.Series.Date";
      this.cbDate.MinimumControlWidth = 150;
      this.cbDate.Stretch = false;
      this.cbDate.Text = LocalizationHolder.rm.GetString("Pdm_564");
      this.cbDate.ToolTipText = LocalizationHolder.rm.GetString("Pdm_565");
      this.cbDate.ComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cbDate.ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
      this.cbDate.ComboBox.DropDownWidth = 250;
      this.cbDate.ComboBox.SelectedIndexChanged += new EventHandler(this.DoSeriesOn);
      this.AlterItem(this.cbDate.ComboBox, this.itemNoDate, true);
      this.btnDate = new ButtonItem();
      this.btnDate.CommandName = "PDM.Series.SelectDate";
      this.btnDate.ShowText = false;
      this.btnDate.ToolTipText = LocalizationHolder.rm.GetString("Pdm_566");
      this.btnDate.Click += new EventHandler(this.DoSelectDate);
      this.btnDate.ImageIndex = service.ImageIndex("imgListView");
      this.btnSeriesOn = new ButtonItem();
      this.btnSeriesOn.BeginGroup = true;
      this.btnSeriesOn.CommandName = "PDM.Series.SeriesOn";
      this.btnSeriesOn.ToolTipText = LocalizationHolder.rm.GetString("Pdm_567");
      this.btnSeriesOn.ShowText = false;
      this.btnSeriesOn.AutoToggle = AutoToggleType.Single;
      this.btnSeriesOn.Checked = false;
      this.btnSeriesOn.Click += new EventHandler(this.DoSeriesOn);
      this.btnSeriesOn.ImageIndex = service.ImageIndex("imgObjectsFilter");
      this.btnApply = new ButtonItem();
      this.btnApply.BeginGroup = true;
      this.btnApply.CommandName = "PDM.Series.Apply";
      this.btnApply.Text = LocalizationHolder.rm.GetString("Pdm_710");
      this.btnApply.ToolTipText = LocalizationHolder.rm.GetString("Pdm_711");
      this.btnApply.ShowText = true;
      this.btnApply.AutoToggle = AutoToggleType.None;
      this.btnApply.Checked = false;
      this.btnApply.Visible = false;
      this.btnApply.Click += new EventHandler(this.DoSeriesApply);
      this.btnApply.ImageIndex = service.ImageIndex("imgApplyBall");
      this.toolbar.Items.AddRange(new ToolbarItemBase[7]
      {
        (ToolbarItemBase) this.cbMainArticle,
        (ToolbarItemBase) this.btnSelectMainArticle,
        (ToolbarItemBase) this.cbSeries,
        (ToolbarItemBase) this.cbDate,
        (ToolbarItemBase) this.btnDate,
        (ToolbarItemBase) this.btnSeriesOn,
        (ToolbarItemBase) this.btnApply
      });
      this.AddToolbar();
    }
    finally
    {
      this.SlowUpdateControls();
      this._isInEvent = false;
    }
  }

  protected virtual void AddToolbar()
  {
    BarManager service = ServicesManager.GetService(typeof (BarManager)) as BarManager;
    service.AddToolbar(this.toolbar);
    this.toolbar.Parent = (Control) service.FindSuitableContainer(DockStyle.Top);
    this.toolbar.DockLine = 4;
    this.toolbar.DockOffset = 0;
    this.toolbar.Location = new Point(0, 0);
  }

  private void InitServices(bool register)
  {
    this._namedImageList = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this._notificationService = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    if (register)
      (ServicesManager.GetService(typeof (IClientPluginsService)) as IClientPluginsService).RegisterClientPlugin(this.guid, (IClientPluginsDataTransfer) this);
    this._filtrationService.OnFiltrationChanged += new FiltrationChanged(this.OnFiltrationChanged);
  }

  public Guid PluginGuid
  {
    [DebuggerStepThrough] get => this.guid;
  }

  public void GetPluginData(HybridDictionary PluginsData)
  {
    if (PluginsData == null)
      return;
    SeriesDateSettingsHolder dateSettingsHolder = this.ReadFromControls();
    if (dateSettingsHolder == null || !dateSettingsHolder.Enabled)
      return;
    PluginsData[(object) "{E2390B62-E0BA-4F7E-89CC-1E9E33F0BB5C}"] = (object) dateSettingsHolder;
  }

  public void PutPluginData(HybridDictionary PluginsData)
  {
  }

  private void DoSelectMainArticle(object sender, EventArgs e)
  {
    if (this._isInEvent)
      return;
    if (!(this._filtrationService.Filtration.Tags[(object) "{E2390B62-E0BA-4F7E-89CC-1E9E33F0BB5C}"] is SeriesDateSettingsHolder dateSettingsHolder))
      dateSettingsHolder = this.ReadFromControls() ?? new SeriesDateSettingsHolder();
    SeriesDateSettingsHolder holder = dateSettingsHolder;
    string str = LocalizationHolder.rm.GetString("Pdm_568");
    string caption = LocalizationHolder.rm.GetString("Pdm_569");
    SelectionOptions selectionOptions = SelectionOptions.Default | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect;
    string description = str;
    Intermech.Navigator.DBObjectTypes.Descriptor rootDescriptor = new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID("cadd940b-306c-11d8-b4e9-00304f19f545"));
    Type dataFormat = typeof (IDBTypedObjectID);
    IServiceContainer serviceContainer = ServicesManager.ServiceContainer;
    long options = (long) selectionOptions;
    object[] objArray = SelectionWindow.Select(caption, description, (IDescriptor) rootDescriptor, dataFormat, (IServiceProvider) serviceContainer, (SelectionOptions) options);
    if (objArray == null || !(objArray[0] is IDBTypedObjectID dbTypedObjectId))
      return;
    holder.MasterArticle = dbTypedObjectId.ObjectID;
    this.ApplyUpdates(holder);
  }

  private void DoSelectDate(object sender, EventArgs e)
  {
    if (this._isInEvent)
      return;
    if (!(this._filtrationService.Filtration.Tags[(object) "{E2390B62-E0BA-4F7E-89CC-1E9E33F0BB5C}"] is SeriesDateSettingsHolder dateSettingsHolder))
      dateSettingsHolder = this.ReadFromControls() ?? new SeriesDateSettingsHolder();
    SeriesDateSettingsHolder holder = dateSettingsHolder;
    Intermech.Bars.ToolBar toolBar = this.btnDate.ToolBar;
    Rectangle rectangle = this.btnDate.ButtonBounds;
    int x1 = rectangle.X;
    rectangle = this.btnDate.ButtonBounds;
    int y1 = rectangle.Y + this.btnDate.ToolBar.Height + 5;
    Point p = new Point(x1, y1);
    Point screen = toolBar.PointToScreen(p);
    int num1 = screen.X + (int) byte.MaxValue;
    rectangle = Screen.PrimaryScreen.WorkingArea;
    int x2 = rectangle.X;
    rectangle = Screen.PrimaryScreen.WorkingArea;
    int width1 = rectangle.Width;
    int num2 = x2 + width1;
    if (num1 > num2)
    {
      ref Point local = ref screen;
      rectangle = Screen.PrimaryScreen.WorkingArea;
      int x3 = rectangle.X;
      rectangle = Screen.PrimaryScreen.WorkingArea;
      int width2 = rectangle.Width;
      int num3 = x3 + width2 - (int) byte.MaxValue;
      local.X = num3;
    }
    int x4 = screen.X;
    rectangle = Screen.PrimaryScreen.WorkingArea;
    int x5 = rectangle.X;
    if (x4 < x5)
    {
      ref Point local = ref screen;
      rectangle = Screen.PrimaryScreen.WorkingArea;
      int x6 = rectangle.X;
      local.X = x6;
    }
    int num4 = screen.Y + 205;
    rectangle = Screen.PrimaryScreen.WorkingArea;
    int y2 = rectangle.Y;
    rectangle = Screen.PrimaryScreen.WorkingArea;
    int height1 = rectangle.Height;
    int num5 = y2 + height1;
    if (num4 > num5)
    {
      ref Point local = ref screen;
      rectangle = Screen.PrimaryScreen.WorkingArea;
      int y3 = rectangle.Y;
      rectangle = Screen.PrimaryScreen.WorkingArea;
      int height2 = rectangle.Height;
      int num6 = y3 + height2 - 205;
      local.Y = num6;
    }
    int y4 = screen.Y;
    rectangle = Screen.PrimaryScreen.WorkingArea;
    int y5 = rectangle.Y;
    if (y4 < y5)
    {
      ref Point local = ref screen;
      rectangle = Screen.PrimaryScreen.WorkingArea;
      int y6 = rectangle.Y;
      local.Y = y6;
    }
    using (DateTimePopupControl timePopupControl = new DateTimePopupControl())
    {
      if (timePopupControl.Execute(screen, new Size(0, 0), (IServiceProvider) null, (object) (holder.Date != DateTime.MinValue ? holder.Date : DateTime.Now)) != DialogResult.OK)
        return;
      holder.Date = ((DateTime) timePopupControl.Value).Date;
      this.ApplyUpdates(holder);
    }
  }

  private void DoSeriesApply(object sender, EventArgs e)
  {
    if (this._isInEvent || this._filtrationService == null)
      return;
    this.ApplyUpdates(this.ReadFromControls());
  }

  private void DoSeriesOn(object sender, EventArgs e)
  {
    if (this._isInEvent || this._filtrationService == null)
      return;
    this.ApplyUpdates(this.ReadFromControls());
  }

  private void OnFiltrationChanged(IFiltrationSettings NewFiltration, bool FiltrationValid)
  {
    if (this._isInEvent || this._filtrationService == null)
      return;
    SeriesDateSettingsHolder holder = (SeriesDateSettingsHolder) null;
    if (this._filtrationService.Filtration.Tags[(object) "{E2390B62-E0BA-4F7E-89CC-1E9E33F0BB5C}"] != null)
      holder = this._filtrationService.Filtration.Tags[(object) "{E2390B62-E0BA-4F7E-89CC-1E9E33F0BB5C}"] as SeriesDateSettingsHolder;
    this.WriteToControls(holder);
  }

  private void ApplyUpdates(SeriesDateSettingsHolder holder)
  {
    try
    {
      this.WriteToControls(holder);
      this._isInEvent = true;
      this._filtrationService.Filtration.Tags[(object) "{E2390B62-E0BA-4F7E-89CC-1E9E33F0BB5C}"] = (object) new SeriesDateSettingsHolder((object) holder);
      this._filtrationService.FiltrationApplyUpdates(true);
    }
    finally
    {
      this._isInEvent = false;
    }
  }

  private SeriesDateSettingsHolder ReadFromControls()
  {
    IMainFormUpdate service = ApplicationServices.Container.GetService(typeof (IMainFormUpdate)) as IMainFormUpdate;
    return service.MainForm.InvokeRequired ? service.MainForm.Invoke((Delegate) new SeriesToolbar.ReadFromControlsDelegate(this.InternalReadFromControls)) as SeriesDateSettingsHolder : this.InternalReadFromControls();
  }

  private SeriesDateSettingsHolder InternalReadFromControls()
  {
    lock (this.toolbar)
    {
      long int64Value = this.cbMainArticle.ComboBox.SelectedItem is MyElement selectedItem1 ? DataSetProcessor.GetInt64Value(selectedItem1.Value, 0L) : 0L;
      MyElement selectedItem2 = this.cbSeries.ComboBox.SelectedItem as MyElement;
      int int32Value = DataSetProcessor.GetInt32Value((object) this.cbSeries.ComboBox.Text, int.MinValue);
      int series = int32Value >= 0 || selectedItem2 == null ? int32Value : DataSetProcessor.GetInt32Value(selectedItem2.Value, int.MinValue);
      MyElement selectedItem3 = this.cbDate.ComboBox.SelectedItem as MyElement;
      DateTime dateTimeValue = DataSetProcessor.GetDateTimeValue((object) this.cbDate.ComboBox.Text, DateTime.MinValue);
      DateTime date = !(dateTimeValue == DateTime.MinValue) || selectedItem3 == null ? dateTimeValue : DataSetProcessor.GetDateTimeValue(selectedItem3.Value, DateTime.MinValue);
      return new SeriesDateSettingsHolder(this.btnSeriesOn.Checked, int64Value, date, series);
    }
  }

  private void WriteToControls(SeriesDateSettingsHolder holder)
  {
    IMainFormUpdate service = ApplicationServices.Container.GetService(typeof (IMainFormUpdate)) as IMainFormUpdate;
    if (service.MainForm.InvokeRequired)
      service.MainForm.Invoke((Delegate) new SeriesToolbar.WriteToControlsDelegate(this.InternalWriteToControls), (object) holder);
    else
      this.InternalWriteToControls(holder);
  }

  private void InternalWriteToControls(SeriesDateSettingsHolder holder)
  {
    if (this._isInEvent)
      return;
    holder = holder ?? new SeriesDateSettingsHolder();
    bool flag = holder != null && holder.Enabled;
    try
    {
      this._isInEvent = true;
      lock (this.toolbar)
      {
        if (holder.MasterArticle == 0L)
        {
          this.AlterItem(this.cbMainArticle.ComboBox, this.itemNoMainArticle, true);
        }
        else
        {
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(holder.MasterArticle);
            if (!objectInfo.Empty)
              this.AlterItem(this.cbMainArticle.ComboBox, new MyElement((object) holder.MasterArticle, objectInfo.Caption, (object) null), true);
            else
              this.AlterItem(this.cbMainArticle.ComboBox, this.itemNoMainArticle, true);
          }
        }
        if (holder.Series == int.MinValue)
          this.AlterItem(this.cbSeries.ComboBox, this.itemNoSeries, true);
        else
          this.AlterItem(this.cbSeries.ComboBox, new MyElement((object) holder.Series, holder.Series.ToString(), (object) null), true);
        if (holder.Date == DateTime.MinValue)
        {
          this.AlterItem(this.cbDate.ComboBox, this.itemNoDate, true);
        }
        else
        {
          ComboBox comboBox = this.cbDate.ComboBox;
          // ISSUE: variable of a boxed type
          __Boxed<DateTime> date1 = (ValueType) holder.Date.Date;
          DateTime date2 = holder.Date;
          date2 = date2.Date;
          string shortDateString = date2.ToShortDateString();
          MyElement myElement = new MyElement((object) date1, shortDateString, (object) null);
          this.AlterItem(comboBox, myElement, true);
        }
        this.btnSeriesOn.Checked = flag;
        this.SlowUpdateControls();
      }
    }
    finally
    {
      this._isInEvent = false;
    }
  }

  private void AlterItem(ComboBox cb, MyElement item, bool makeSelected)
  {
    if (cb == null)
      return;
    if (item == null)
      return;
    try
    {
      for (int index = 0; index < cb.Items.Count; ++index)
      {
        if (cb.Items[index] is MyElement myElement && string.Compare(myElement.Caption, item.Caption, StringComparison.CurrentCultureIgnoreCase) == 0)
        {
          myElement.Value = item.Value;
          if (!makeSelected)
            return;
          cb.SelectedIndex = index;
          return;
        }
      }
      cb.Items.Insert(string.IsNullOrEmpty(item.Caption) ? 0 : 1, (object) item);
      if (!makeSelected)
        return;
      cb.SelectedIndex = string.IsNullOrEmpty(item.Caption) ? 0 : 1;
    }
    finally
    {
      if (cb.Items.Count > this.maxItemsCount)
      {
        while (cb.Items.Count > this.maxItemsCount)
          cb.Items.RemoveAt(cb.Items.Count - 1);
      }
    }
  }

  private void SlowUpdateControls()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this.cbMainArticle.Enabled = sessionKeeper.Session.EnabledSeriesDates;
      this.btnSelectMainArticle.Enabled = this.cbMainArticle.Enabled;
      this.cbSeries.Enabled = this.cbMainArticle.Enabled;
      this.cbDate.Enabled = this.cbMainArticle.Enabled;
      this.btnDate.Enabled = this.cbMainArticle.Enabled;
      this.btnSeriesOn.Visible = true;
      this.btnSeriesOn.Enabled = this.cbMainArticle.Enabled;
      this.btnApply.Enabled = this.cbMainArticle.Enabled;
      this.btnApply.Visible = this.btnSeriesOn.Checked;
      if (this.cbMainArticle.Enabled)
        return;
      this.toolbar.Hidden = true;
    }
  }

  private delegate SeriesDateSettingsHolder ReadFromControlsDelegate();

  private delegate void WriteToControlsDelegate(SeriesDateSettingsHolder holder);
}

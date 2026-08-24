// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.ManufactOrdersCreatorControl
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Bars;
using Intermech.Client.Core.ObjectCreator;
using Intermech.Client.Core.ObjectCreator.Controls;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MRP;
using Intermech.Navigator.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Страничка мастера по созданию производственных заказов
/// </summary>
internal sealed class ManufactOrdersCreatorControl : ObjectCreatorControl, IStepCompleteManager
{
  /// <summary>
  /// Возможен ли переход со страницы (не важно, вперёд/назад или по кнопке "Готово").
  /// Значение проверяется при попытке перехода, т.е. кнопки Далее/Назад/Готово доступны,
  /// но при значении false данного св-ва переход не будет выполнен и пользователь останется
  /// на прежней странице
  /// </summary>
  private bool _isCurrentStepComplete = true;
  private ObjectCreatorForm _objectCreatorForm;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private ImageList ilTracing;
  private ImageList ilState;
  private ImageList ilLinked;
  private Panel panel1;
  private iGrid igTracing;
  private Intermech.Bars.ToolBar tbTracing;
  private ButtonItem btnStartTracing;
  private ButtonItem btnStopTracing;
  internal ManufactOrdersEditor _editor;

  /// <summary>
  /// Создать страничку мастера по созданию производственных заказов
  /// </summary>
  /// <param name="createdObject"></param>
  public ManufactOrdersCreatorControl(CreatedObjectItem createdObject)
    : base(createdObject)
  {
    this.InitializeComponent();
    this._editor.Changed += new ManufactOrdersChangedEventHandler(this.Editor_Changed);
    this._editor.ErrorsInEditor += new ManufactOrdersErrorsInEditorEventHandler(this.Editor_ErrorsInEditor);
    this._editor.Init(string.Empty, createdObject, (ISelectedItems) null, (IServiceProvider) ServicesManager.ServiceContainer);
    this._NeedSaveWhenNotVisible = true;
    this._SaveInTransaction = false;
  }

  public ObjectCreatorForm ObjectCreatorForm
  {
    get => this._objectCreatorForm;
    set
    {
      if (this._objectCreatorForm == value)
        return;
      if (this._objectCreatorForm != null)
      {
        this._objectCreatorForm.Shown -= new EventHandler(this.ObjectCreatorForm_Shown);
        this._objectCreatorForm.NextButton.Click -= new EventHandler(this.NextButton_Click);
        this._objectCreatorForm.PreviousButton.Click -= new EventHandler(this.PreviousButton_Click);
        this._objectCreatorForm.SkipButton.Click -= new EventHandler(this.SkipButton_Click);
      }
      this._objectCreatorForm = value;
      if (this._objectCreatorForm == null)
        return;
      this._objectCreatorForm.Shown += new EventHandler(this.ObjectCreatorForm_Shown);
      this._objectCreatorForm.NextButton.Click += new EventHandler(this.NextButton_Click);
      this._objectCreatorForm.PreviousButton.Click += new EventHandler(this.PreviousButton_Click);
      this._objectCreatorForm.SkipButton.Click += new EventHandler(this.SkipButton_Click);
    }
  }

  /// <summary>
  /// Признак завершённости шага и доступности завершения работы мастера
  /// </summary>
  public override bool StepIsReady => base.StepIsReady && !this._editor.HasErrorsInEditor;

  /// <summary>
  /// Признак завершённости шага и доступности к переходу на следующий шаг
  /// </summary>
  public override bool NextIsAccessible => base.NextIsAccessible && !this._editor.HasErrorsInEditor;

  public override bool Refresh(PageRefreshArgs args)
  {
    this._editor.RefreshEditor(false);
    return base.Refresh(args);
  }

  /// <summary>Сохранение данных в объекте CreatedObject</summary>
  /// <param name="args">Информации для метода сохранения шага мастера создания объектов</param>
  /// <returns>Если сохранение прошло без ошибок возвращается true, иначе возвращается false</returns>
  public override bool Save(PageSaveArgs args)
  {
    if (args == null)
      throw new ArgumentNullException(nameof (args));
    if (args.NextPageIndex == this.PageIndex - 1)
      return true;
    if (this._editor.HasErrorsInEditor)
    {
      args.errorType = ErrorType.CheckNotCompleted;
      return false;
    }
    ManufactureOrderHolder manufactureOrderHolder = this._editor._manufactureOrderHolder;
    if (ServicesManager.GetService(typeof (ManufactureOrderHolder)) != null)
      ServicesManager.RemoveService(typeof (ManufactureOrderHolder));
    ServicesManager.AddService(typeof (ManufactureOrderHolder), (object) manufactureOrderHolder);
    return base.Save(args);
  }

  /// <summary>Начать проверку на ошибки</summary>
  public override void StartErrorCheck() => this._editor.TraceStart();

  /// <summary>
  /// Событие, уведомляющее о том, что завершена работа на текущем шаге мастера
  /// </summary>
  public event StepCompletedHandler StepCompletedEvent;

  private void ObjectCreatorForm_Shown(object sender, EventArgs e)
  {
    this.SetObjectCreatorControlFinishButtonEnabled();
  }

  private void NextButton_Click(object sender, EventArgs e)
  {
    this.SetObjectCreatorControlFinishButtonEnabled();
  }

  private void PreviousButton_Click(object sender, EventArgs e)
  {
    this.SetObjectCreatorControlFinishButtonEnabled();
  }

  private void SkipButton_Click(object sender, EventArgs e)
  {
    this.SetObjectCreatorControlFinishButtonEnabled();
  }

  private void Editor_Changed(object sender, EventArgs e) => this.Refresh(new PageRefreshArgs());

  private void Editor_ErrorsInEditor(object sender, EventArgs e)
  {
    this.Refresh(new PageRefreshArgs());
  }

  private void SetObjectCreatorControlFinishButtonEnabled()
  {
    this._objectCreatorForm.FinishButton.Enabled = this._objectCreatorForm.CurrentObjectCreatorControl == this;
  }

  public bool IsCompletedEventSubscribed => this.StepCompletedEvent != null;

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
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ManufactOrdersCreatorControl));
    this.ilTracing = new ImageList(this.components);
    this.ilState = new ImageList(this.components);
    this.ilLinked = new ImageList(this.components);
    this.panel1 = new Panel();
    this.igTracing = new iGrid();
    this.tbTracing = new Intermech.Bars.ToolBar();
    this.btnStartTracing = new ButtonItem();
    this.btnStopTracing = new ButtonItem();
    this._editor = new ManufactOrdersEditor();
    this.panel1.SuspendLayout();
    ((ISupportInitialize) this.igTracing).BeginInit();
    this.SuspendLayout();
    this.ilTracing.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilTracing.ImageStream");
    this.ilTracing.TransparentColor = Color.Transparent;
    this.ilTracing.Images.SetKeyName(0, "gear_stop.png");
    this.ilTracing.Images.SetKeyName(1, "gear_run.png");
    this.ilTracing.Images.SetKeyName(2, "standard_add.ico");
    this.ilTracing.Images.SetKeyName(3, "pdm_delete.ico");
    this.ilState.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilState.ImageStream");
    this.ilState.TransparentColor = Color.Transparent;
    this.ilState.Images.SetKeyName(0, "pcsIncompatibilities.ico");
    this.ilState.Images.SetKeyName(1, "pcsContextNotFound.ico");
    this.ilState.Images.SetKeyName(2, "pcsException.ico");
    this.ilState.Images.SetKeyName(3, "pcsOptionNotFound.ico");
    this.ilState.Images.SetKeyName(4, "pcsOptionValueNotFound.ico");
    this.ilState.Images.SetKeyName(5, "pcsConfigured.ico");
    this.ilState.Images.SetKeyName(6, "pcsNone.ico");
    this.ilState.Images.SetKeyName(7, "gear_information.png");
    this.ilLinked.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilLinked.ImageStream");
    this.ilLinked.TransparentColor = Color.Transparent;
    this.ilLinked.Images.SetKeyName(0, "add.png");
    this.ilLinked.Images.SetKeyName(1, "delete.png");
    this.panel1.Controls.Add((Control) this.igTracing);
    this.panel1.Controls.Add((Control) this.tbTracing);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    this.igTracing.BackColorEvenRows = Color.WhiteSmoke;
    this.igTracing.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height");
    this.igTracing.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight");
    componentResourceManager.ApplyResources((object) this.igTracing, "igTracing");
    this.igTracing.Header.Height = (int) componentResourceManager.GetObject("igTracing.Header.Height");
    this.igTracing.HighlightBackColorNoFocus = SystemColors.ControlLight;
    this.igTracing.HotTracking = false;
    this.igTracing.LayoutObject.Flags = iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this.igTracing.Name = "igTracing";
    this.igTracing.ReadOnly = true;
    this.igTracing.RowMode = true;
    this.igTracing.VScrollBar.Visibility = iGScrollBarVisibility.Always;
    this.tbTracing.FullMenus = true;
    this.tbTracing.Guid = new Guid("37056402-c6d1-47d4-be0f-e941c1a06e55");
    this.tbTracing.Hidden = false;
    this.tbTracing.ImageList = this.ilTracing;
    this.tbTracing.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this.btnStartTracing,
      (ToolbarItemBase) this.btnStopTracing
    });
    componentResourceManager.ApplyResources((object) this.tbTracing, "tbTracing");
    this.tbTracing.Name = "tbTracing";
    this.tbTracing.Tag = (object) "";
    componentResourceManager.ApplyResources((object) this.btnStartTracing, "btnStartTracing");
    this.btnStartTracing.ImageIndex = 1;
    componentResourceManager.ApplyResources((object) this.btnStopTracing, "btnStopTracing");
    this.btnStopTracing.Enabled = false;
    this.btnStopTracing.ImageIndex = 0;
    componentResourceManager.ApplyResources((object) this._editor, "_editor");
    this._editor.HasErrorsInEditor = true;
    this._editor.HeaderVisiblity = true;
    this._editor.IsChanged = false;
    this._editor.Name = "_editor";
    this.Controls.Add((Control) this._editor);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Name = nameof (ManufactOrdersCreatorControl);
    this.panel1.ResumeLayout(false);
    ((ISupportInitialize) this.igTracing).EndInit();
    this.ResumeLayout(false);
  }
}

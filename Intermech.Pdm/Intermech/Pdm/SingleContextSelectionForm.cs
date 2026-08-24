// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SingleContextSelectionForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Pdm;

public class SingleContextSelectionForm : Form
{
  public static long DefaultContext = 2;
  public static bool DontShowAgain = false;
  private MyAttributeMetadata _contextAttr = new MyAttributeMetadata();
  private IContainer components;
  private Label lbPromt;
  private Panel panel1;
  private Button btnOK;
  private Button btnCancel;
  private CheckBox cbDontShowAgain;
  protected internal iGrid grid;
  private iGCellStyle gridCol0CellStyle;
  private iGColHdrStyle gridCol0ColHdrStyle;
  private ImageList images;

  public SingleContextSelectionForm()
  {
    this.InitializeComponent();
    this.FillContextsList();
  }

  private long SelectedContext
  {
    get
    {
      iGRow row = (this.grid.SelectedCells.Count > 0 ? this.grid.SelectedCells[0] : (iGCell) null)?.Row;
      return row == null ? SingleContextSelectionForm.DefaultContext : (long) row.Tag;
    }
  }

  public static DialogResult Execute()
  {
    if (SingleContextSelectionForm.DontShowAgain)
      return DialogResult.OK;
    using (SingleContextSelectionForm contextSelectionForm = new SingleContextSelectionForm())
    {
      int num = (int) contextSelectionForm.ShowDialog();
      if (num == 1)
      {
        SingleContextSelectionForm.DefaultContext = contextSelectionForm.SelectedContext;
        SingleContextSelectionForm.DontShowAgain = contextSelectionForm.cbDontShowAgain.Checked;
      }
      return (DialogResult) num;
    }
  }

  private void FillContextsList()
  {
    this._contextAttr.SetByGUID("cad00651-306c-11d8-b4e9-00304f19f545");
    this.grid.Rows.Clear();
    if (this._contextAttr.AttrPossibleValues != null)
    {
      for (int index = 0; index < this._contextAttr.AttrPossibleValues.Count; ++index)
      {
        MyElement attrPossibleValue = this._contextAttr.AttrPossibleValues[index] as MyElement;
        long int64Value = DataSetProcessor.GetInt64Value(attrPossibleValue.Value, 0L);
        string caption = attrPossibleValue.Caption;
        iGRow iGrow = this.grid.Rows.Add();
        iGrow.Tag = (object) int64Value;
        iGrow.Cells["columnMain"].Value = (object) caption;
        if (int64Value >= 0L && int64Value < (long) this.images.Images.Count)
          iGrow.Cells["columnMain"].ImageIndex = Convert.ToInt32(int64Value);
        if (SingleContextSelectionForm.DefaultContext == int64Value)
        {
          for (int colIndex = 0; colIndex < iGrow.Cells.Count; ++colIndex)
          {
            if (iGrow.Cells[colIndex].Selectable != iGBool.False)
              iGrow.Cells[colIndex].Selected = true;
          }
        }
      }
    }
    this.UpdateControls();
  }

  private void UpdateControls()
  {
    this.btnOK.Enabled = this.grid.SelectedCells.Count > 0;
    this.btnCancel.Enabled = true;
  }

  private void DoSelectionChanged(object sender, EventArgs e) => this.UpdateControls();

  private void DoDoubleClick(object sender, iGCellDoubleClickEventArgs e)
  {
    this.UpdateControls();
    if (this.grid.SelectedCells.Count <= 0)
      return;
    this.DialogResult = DialogResult.OK;
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SingleContextSelectionForm));
    iGColPattern iGcolPattern = new iGColPattern();
    this.gridCol0CellStyle = new iGCellStyle(true);
    this.gridCol0ColHdrStyle = new iGColHdrStyle(true);
    this.lbPromt = new Label();
    this.panel1 = new Panel();
    this.cbDontShowAgain = new CheckBox();
    this.btnOK = new Button();
    this.btnCancel = new Button();
    this.grid = new iGrid();
    this.images = new ImageList(this.components);
    this.panel1.SuspendLayout();
    ((ISupportInitialize) this.grid).BeginInit();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.lbPromt, "lbPromt");
    this.lbPromt.Name = "lbPromt";
    this.panel1.Controls.Add((Control) this.cbDontShowAgain);
    this.panel1.Controls.Add((Control) this.btnOK);
    this.panel1.Controls.Add((Control) this.btnCancel);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.cbDontShowAgain, "cbDontShowAgain");
    this.cbDontShowAgain.Name = "cbDontShowAgain";
    this.cbDontShowAgain.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
    this.btnOK.Cursor = Cursors.Hand;
    this.btnOK.DialogResult = DialogResult.OK;
    this.btnOK.Name = "btnOK";
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.Cursor = Cursors.Hand;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Name = "btnCancel";
    this.grid.AllowDrop = true;
    this.grid.AutoResizeCols = true;
    this.grid.AutoWidthColMode = iGAutoWidthColMode.Cells;
    this.grid.BackColorEvenRows = SystemColors.Window;
    this.grid.BackColorOddRows = SystemColors.Window;
    iGcolPattern.AllowGrouping = false;
    iGcolPattern.AllowMoving = false;
    iGcolPattern.CellStyle = this.gridCol0CellStyle;
    iGcolPattern.ColHdrStyle = this.gridCol0ColHdrStyle;
    componentResourceManager.ApplyResources((object) iGcolPattern, "iGColPattern1");
    this.grid.Cols.AddRange(new iGColPattern[1]
    {
      iGcolPattern
    });
    this.grid.Cursor = Cursors.Default;
    this.grid.DefaultAutoGroupRow.Height = 21;
    this.grid.DefaultCol.AllowGrouping = false;
    this.grid.DefaultCol.AllowMoving = false;
    this.grid.DefaultCol.Width = (int) componentResourceManager.GetObject("resource.Width");
    this.grid.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height");
    this.grid.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight");
    componentResourceManager.ApplyResources((object) this.grid, "grid");
    this.grid.GroupBox.BackColor = SystemColors.AppWorkspace;
    this.grid.GroupBox.HintBackColor = SystemColors.AppWorkspace;
    this.grid.GroupBox.HintForeColor = SystemColors.ControlText;
    this.grid.GroupBox.Text = componentResourceManager.GetString("grid.GroupBox.Text");
    this.grid.Header.AutoHeightFlags = iGHdrAutoHeightFlags.OnAddCol | iGHdrAutoHeightFlags.OnRemoveCol | iGHdrAutoHeightFlags.OnShowCol | iGHdrAutoHeightFlags.OnContentsChange | iGHdrAutoHeightFlags.OnThemeChange | iGHdrAutoHeightFlags.OnResizeCol;
    this.grid.Header.Height = (int) componentResourceManager.GetObject("grid.Header.Height");
    this.grid.HighlightBackColorNoFocus = SystemColors.Highlight;
    this.grid.HighlightForeColorNoFocus = SystemColors.HighlightText;
    this.grid.HotTracking = false;
    this.grid.ImageList = this.images;
    this.grid.LayoutObject.Flags = iGLayoutFlags.Grouping | iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this.grid.Name = "grid";
    this.grid.PageCapacity = 500;
    this.grid.PressedMouseMoveMode = iGPressedMouseMoveMode.Normal;
    this.grid.ProcessTab = false;
    this.grid.ReadOnly = true;
    this.grid.RowMode = true;
    this.grid.RowTextStartColNear = 211;
    this.grid.ShowControlsInAllCells = false;
    this.grid.SilentValidation = true;
    this.grid.SortByLevels = true;
    this.grid.UniqueKeys = true;
    this.grid.CellDoubleClick += new iGCellDoubleClickEventHandler(this.DoDoubleClick);
    this.grid.SelectionChanged += new EventHandler(this.DoSelectionChanged);
    this.images.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("images.ImageStream");
    this.images.TransparentColor = Color.Transparent;
    this.images.Images.SetKeyName(0, "rsCommonContext.ico");
    this.images.Images.SetKeyName(1, "rsDesignContext.ico");
    this.images.Images.SetKeyName(2, "rsTechContext.ico");
    this.images.Images.SetKeyName(3, "rsTechnologicalContext.ico");
    this.AcceptButton = (IButtonControl) this.btnOK;
    this.CancelButton = (IButtonControl) this.btnCancel;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Controls.Add((Control) this.grid);
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.lbPromt);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (SingleContextSelectionForm);
    this.SizeGripStyle = SizeGripStyle.Hide;
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    ((ISupportInitialize) this.grid).EndInit();
    this.ResumeLayout(false);
  }
}

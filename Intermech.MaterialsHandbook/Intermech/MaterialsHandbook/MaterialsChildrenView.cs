// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.MaterialsChildrenView
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Interfaces;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System.ComponentModel;
using System.Drawing;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class MaterialsChildrenView : ChildrenView
{
  private static int _imageIndex = -1;
  private IContainer components;

  public MaterialsChildrenView() => this.InitializeComponent();

  public override int ImageIndex
  {
    get
    {
      if (MaterialsChildrenView._imageIndex == -1 && ChildrenView._namedImageList != null)
        MaterialsChildrenView._imageIndex = ChildrenView._namedImageList.ImageIndex("imgContains");
      return MaterialsChildrenView._imageIndex;
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public override string StateStreamPrefix
  {
    get => "Handbook";
    set
    {
    }
  }

  public override ContentType ViewContentType => ContentType.Folders;

  public override void Activate(IView previousView)
  {
    INavigatorColumnsService service = ServiceUtils.GetService<INavigatorColumnsService>((object) ApplicationServices.Container, false);
    if (service != null)
    {
      service.RemoveNavigatorColumns(Consts.IMHMaterialsNodeCategoryID, -1, this.StateStreamPrefix);
      service.RemoveNavigatorColumns(Consts.IMHAssortmentNodeCategoryID, -1, this.StateStreamPrefix);
    }
    base.Activate(previousView);
  }

  protected override ICommandsProvider GetCommandsProvider()
  {
    return (ICommandsProvider) new MaterialsChildrenViewCommandsProvider((ChildrenView) this);
  }

  protected override bool ShowContextMenu4Header(Point location) => false;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ((ISupportInitialize) this._grid).BeginInit();
    ((ISupportInitialize) this._pictureBox).BeginInit();
    this.SuspendLayout();
    this._grid.DefaultAutoGroupRow.Height = 21;
    this._grid.FrozenArea.ColCount = 1;
    this._grid.FrozenArea.SortFrozenRows = true;
    this._grid.GroupBox.BackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintBackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintForeColor = SystemColors.ControlText;
    this._grid.GroupBox.Text = "Перетащите заголовок колонки в эту область для группировки по значениям этой колонки";
    this._grid.Header.AutoHeightFlags = iGHdrAutoHeightFlags.OnAddCol | iGHdrAutoHeightFlags.OnRemoveCol | iGHdrAutoHeightFlags.OnShowCol | iGHdrAutoHeightFlags.OnContentsChange | iGHdrAutoHeightFlags.OnThemeChange | iGHdrAutoHeightFlags.OnResizeCol;
    this._grid.Header.Height = 20;
    this._grid.LayoutObject.Flags = iGLayoutFlags.Grouping | iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this._grid.Size = new Size(1151, 160 /*0xA0*/);
    this._filtersComboBoxItem.Padding.Bottom = 0;
    this._filtersComboBoxItem.Padding.Left = 1;
    this._filtersComboBoxItem.Padding.Right = 1;
    this._filtersComboBoxItem.Padding.Top = 0;
    this._toggleGroupingButtonItem.Checked = false;
    this.buttonHeightSet.Padding.Bottom = 0;
    this.buttonHeightSet.Padding.Left = 0;
    this.buttonHeightSet.Padding.Right = 0;
    this.buttonHeightSet.Padding.Top = 0;
    this.DisableColumnsGrouping = true;
    this.DisableFiltration = true;
    this.DisableGroupBox = true;
    this.Name = nameof (MaterialsChildrenView);
    this.Controls.SetChildIndex((System.Windows.Forms.Control) this._gridHeaderMenuBar, 0);
    this.Controls.SetChildIndex((System.Windows.Forms.Control) this._pictureBox, 0);
    this.Controls.SetChildIndex((System.Windows.Forms.Control) this._toolBar, 0);
    this.Controls.SetChildIndex((System.Windows.Forms.Control) this._grid, 0);
    ((ISupportInitialize) this._grid).EndInit();
    ((ISupportInitialize) this._pictureBox).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}

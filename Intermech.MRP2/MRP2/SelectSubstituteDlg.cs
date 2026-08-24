// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.SelectSubstituteDlg
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

/// <summary>
/// Диало для выбора группы заменителей (немного скопипащен с ArtSubstitutionsEditor)
/// </summary>
public class SelectSubstituteDlg : Form
{
  private long _groupNo;
  private RelationAttributesPackage _relAttrs;
  private INavGraphicsCache _navGraphicsCache;
  private ICategoryTypeIconService _categoryTypeIconService;
  /// <summary>Значок для группы заменителей</summary>
  private static Icon _iconGroup;
  /// <summary>Значок для актуального заменителя в группе</summary>
  private static Icon _iconActualSubstitute;
  /// <summary>Значок для заменителя в группе</summary>
  private static Icon _iconSubstitute;
  private static readonly Color AuxiliaryPositionColor = Color.LightYellow;
  private static readonly Color DesignActualVariantColor = Color.LightGreen;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private Panel panelBottom;
  private Button _cancelButton;
  private Button _okButton;
  private Intermech.VirtualTreeView.VirtualTreeView _substitutesTree;
  private Column _captionSubstitutesTreeColumn;
  private Column _quantitySubstitutesTreeColumn;
  private ImageList imagesTreeview;

  private SubstituteObjects _substituteObjects { get; set; }

  public SelectSubstituteDlg()
  {
    this.InitializeComponent();
    this._navGraphicsCache = ServicesManager.GetService(typeof (INavGraphicsCache)) as INavGraphicsCache;
    this._categoryTypeIconService = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    if (SelectSubstituteDlg._iconGroup != null)
      return;
    SelectSubstituteDlg._iconGroup = ImageHelper.BitmapToIcon(this.imagesTreeview.Images[0] as Bitmap);
    SelectSubstituteDlg._iconActualSubstitute = ImageHelper.BitmapToIcon(this.imagesTreeview.Images[1] as Bitmap);
    SelectSubstituteDlg._iconSubstitute = ImageHelper.BitmapToIcon(this.imagesTreeview.Images[2] as Bitmap);
  }

  public static DialogResult Execute(SubstituteObjects substitutes, out List<long> relationIds)
  {
    relationIds = (List<long>) null;
    SelectSubstituteDlg selectSubstituteDlg = new SelectSubstituteDlg()
    {
      _substituteObjects = substitutes,
      _groupNo = substitutes.Groups[0],
      _relAttrs = substitutes.RelationAttributes
    };
    selectSubstituteDlg._substitutesTree.DataSource = (object) substitutes;
    selectSubstituteDlg._substitutesTree.UpdateRows();
    selectSubstituteDlg._substitutesTree.RootRow.ExpandChildren(true);
    DialogResult dialogResult = selectSubstituteDlg.ShowDialog();
    if (dialogResult == DialogResult.OK)
    {
      if (selectSubstituteDlg._substitutesTree.SelectedRow == null)
        return DialogResult.Cancel;
      long SubstInGroup = selectSubstituteDlg._substitutesTree.SelectedRow.Level == 1 ? (long) selectSubstituteDlg._substitutesTree.SelectedRow.ChildIndex : (long) selectSubstituteDlg._substitutesTree.SelectedRow.ParentRow.ChildIndex;
      relationIds = selectSubstituteDlg._substituteObjects[selectSubstituteDlg._groupNo, SubstInGroup];
    }
    return dialogResult;
  }

  private Style CreateStyleWithNewBackColor(Style style, Color backColor)
  {
    return new Style(style, new StyleDelta()
    {
      BackColor = backColor,
      GradientColor = backColor
    });
  }

  private void _substitutesTree_GetCellData(object sender, GetCellDataEventArgs e)
  {
    if (e.Row.Level == 1)
    {
      object obj = e.Row.Item;
      bool flag = false;
      if (obj is List<long> && this._substituteObjects.IsDesignActualVariant(this._groupNo, (long) e.Row.ChildIndex))
        flag = true;
      if (flag)
      {
        e.CellData.EvenStyle = this.CreateStyleWithNewBackColor(e.CellData.EvenStyle, SelectSubstituteDlg.DesignActualVariantColor);
        e.CellData.OddStyle = this.CreateStyleWithNewBackColor(e.CellData.OddStyle, SelectSubstituteDlg.DesignActualVariantColor);
      }
      else
      {
        e.CellData.EvenStyle = this.CreateStyleWithNewBackColor(e.CellData.EvenStyle, Color.White);
        e.CellData.OddStyle = this.CreateStyleWithNewBackColor(e.CellData.OddStyle, Color.White);
      }
      if (e.Column == this._captionSubstitutesTreeColumn)
      {
        e.CellData.Value = e.Row.ChildIndex == 0 ? (object) $"Актуальный заменитель [{this._groupNo}.{e.Row.ChildIndex}]" : (object) $"Допустимый заменитель [{this._groupNo}.{e.Row.ChildIndex}]";
        if (e.Row.ChildIndex > 0)
          return;
      }
    }
    if (e.Row.Level != 2)
      return;
    long num = (long) e.Row.Item;
    if (this._substituteObjects.IsAuxiliaryPosition(num))
    {
      e.CellData.EvenStyle = this.CreateStyleWithNewBackColor(e.CellData.EvenStyle, SelectSubstituteDlg.AuxiliaryPositionColor);
      e.CellData.OddStyle = this.CreateStyleWithNewBackColor(e.CellData.OddStyle, SelectSubstituteDlg.AuxiliaryPositionColor);
    }
    else
    {
      e.CellData.EvenStyle = this.CreateStyleWithNewBackColor(e.CellData.EvenStyle, Color.White);
      e.CellData.OddStyle = this.CreateStyleWithNewBackColor(e.CellData.OddStyle, Color.White);
    }
    object obj1 = (object) null;
    if (e.Column == this._captionSubstitutesTreeColumn)
      obj1 = this._relAttrs[num, -50];
    else if (e.Column == this._quantitySubstitutesTreeColumn)
      obj1 = this._relAttrs[num, SubstituteObjects.attrQuantity];
    if (obj1 != null)
      e.CellData.Value = (object) obj1.ToString();
    else
      e.CellData.Value = (object) "";
  }

  private void _substitutesTree_GetChildren(object sender, GetChildrenEventArgs e)
  {
    if (e.Row.Item == null)
      return;
    if (e.Row.Level == 0)
    {
      e.Children = (IList) this._substituteObjects[this._groupNo];
    }
    else
    {
      if (e.Row.Level != 1)
        return;
      List<long> source = this._substituteObjects[this._groupNo, (long) e.Row.ChildIndex];
      if (source != null)
        source = source.OrderBy<long, long>((Func<long, long>) (o => this._substituteObjects.GetPositionNumber(o))).ToList<long>();
      e.Children = (IList) source;
    }
  }

  private void _substitutesTree_GetRowData(object sender, GetRowDataEventArgs e)
  {
    e.RowData.AutoFitHeight = true;
    if (e.Row.Item == null)
      return;
    if (e.Row.Level == 1)
    {
      e.RowData.Icon = e.Row.ChildIndex == 0 ? SelectSubstituteDlg._iconActualSubstitute : SelectSubstituteDlg._iconSubstitute;
    }
    else
    {
      if (e.Row.Level != 2)
        return;
      long prjLinkID = (long) e.Row.Item;
      e.RowData.ImageList = this._categoryTypeIconService.ImageList;
      e.RowData.ImageSize = 32 /*0x20*/;
      object obj = this._relAttrs[prjLinkID, -7] ?? (object) -1;
      e.RowData.ImageIndex = Images32x16_Cache.GetImage32x16Index(1, Convert.ToInt32(obj), (NavigatorTreeNode) null);
    }
  }

  private void SelectSubstituteDlg_Load(object sender, EventArgs e)
  {
  }

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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectSubstituteDlg));
    this.panelBottom = new Panel();
    this._cancelButton = new Button();
    this._okButton = new Button();
    this._substitutesTree = new Intermech.VirtualTreeView.VirtualTreeView();
    this._captionSubstitutesTreeColumn = new Column();
    this._quantitySubstitutesTreeColumn = new Column();
    this.imagesTreeview = new ImageList(this.components);
    this.panelBottom.SuspendLayout();
    this._substitutesTree.BeginInit();
    this.SuspendLayout();
    this.panelBottom.BorderStyle = BorderStyle.Fixed3D;
    this.panelBottom.Controls.Add((Control) this._cancelButton);
    this.panelBottom.Controls.Add((Control) this._okButton);
    this.panelBottom.Dock = DockStyle.Bottom;
    this.panelBottom.Location = new Point(0, 318);
    this.panelBottom.Name = "panelBottom";
    this.panelBottom.Size = new Size(645, 60);
    this.panelBottom.TabIndex = 18;
    this._cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._cancelButton.Cursor = Cursors.Hand;
    this._cancelButton.DialogResult = DialogResult.Cancel;
    this._cancelButton.FlatStyle = FlatStyle.System;
    this._cancelButton.ImageAlign = ContentAlignment.MiddleLeft;
    this._cancelButton.ImeMode = ImeMode.NoControl;
    this._cancelButton.Location = new Point(506, 15);
    this._cancelButton.Name = "_cancelButton";
    this._cancelButton.Size = new Size(121, 27);
    this._cancelButton.TabIndex = 3;
    this._cancelButton.Text = "Отмена";
    this._okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._okButton.Cursor = Cursors.Hand;
    this._okButton.DialogResult = DialogResult.OK;
    this._okButton.FlatStyle = FlatStyle.System;
    this._okButton.ImageAlign = ContentAlignment.MiddleLeft;
    this._okButton.ImeMode = ImeMode.NoControl;
    this._okButton.Location = new Point(379, 15);
    this._okButton.Name = "_okButton";
    this._okButton.Size = new Size(121, 27);
    this._okButton.TabIndex = 2;
    this._okButton.Text = "ОК";
    this._substitutesTree.AllowDrop = true;
    this._substitutesTree.AllowMultiSelect = false;
    this._substitutesTree.BackColor = SystemColors.Control;
    this._substitutesTree.Columns.Add(this._captionSubstitutesTreeColumn);
    this._substitutesTree.Columns.Add(this._quantitySubstitutesTreeColumn);
    this._substitutesTree.DisableHeaderContextMenu = true;
    this._substitutesTree.Dock = DockStyle.Fill;
    this._substitutesTree.Font = new Font("Microsoft Sans Serif", 8.25f);
    this._substitutesTree.ImageList = (ImageList) null;
    this._substitutesTree.LineStyle = LineStyle.Dot;
    this._substitutesTree.Location = new Point(0, 0);
    this._substitutesTree.MainColumn = this._captionSubstitutesTreeColumn;
    this._substitutesTree.MinRowHeight = 18;
    this._substitutesTree.Name = "_substitutesTree";
    this._substitutesTree.RowSelectedUnfocusedStyle.BackColor = SystemColors.Highlight;
    this._substitutesTree.RowSelectedUnfocusedStyle.ForeColor = SystemColors.HighlightText;
    this._substitutesTree.RowStyle.BorderColor = SystemColors.Control;
    this._substitutesTree.RowStyle.BorderStyle = Border3DStyle.Adjust;
    this._substitutesTree.RowStyle.BorderWidth = 1;
    this._substitutesTree.RowStyle.VertAlignment = StringAlignment.Near;
    this._substitutesTree.RowStyle.WordWrap = true;
    this._substitutesTree.ShowRootRow = false;
    this._substitutesTree.Size = new Size(645, 318);
    this._substitutesTree.SuppressErrorMessages = true;
    this._substitutesTree.TabIndex = 19;
    this._substitutesTree.GetCellData += new GetCellDataHandler(this._substitutesTree_GetCellData);
    this._substitutesTree.GetChildren += new GetChildrenHandler(this._substitutesTree_GetChildren);
    this._substitutesTree.GetRowData += new GetRowDataHandler(this._substitutesTree_GetRowData);
    this._captionSubstitutesTreeColumn.Caption = "Допустимые замены / Заголовок";
    this._captionSubstitutesTreeColumn.CellStyle.BorderWidth = 0;
    this._captionSubstitutesTreeColumn.HeaderStyle.HorzAlignment = StringAlignment.Near;
    this._captionSubstitutesTreeColumn.HeaderStyle.WordWrap = true;
    this._captionSubstitutesTreeColumn.MinWidth = 50;
    this._captionSubstitutesTreeColumn.Movable = false;
    this._captionSubstitutesTreeColumn.Name = "_captionSubstitutesTreeColumn";
    this._captionSubstitutesTreeColumn.Sortable = false;
    this._captionSubstitutesTreeColumn.ToolTip = "Группы и допустимые замены в группах / Заголовок";
    this._captionSubstitutesTreeColumn.Width = 300;
    this._quantitySubstitutesTreeColumn.Caption = "Кол.";
    this._quantitySubstitutesTreeColumn.CellStyle.HorzAlignment = StringAlignment.Near;
    this._quantitySubstitutesTreeColumn.HeaderStyle.HorzAlignment = StringAlignment.Near;
    this._quantitySubstitutesTreeColumn.HeaderStyle.WordWrap = true;
    this._quantitySubstitutesTreeColumn.Movable = false;
    this._quantitySubstitutesTreeColumn.Name = "_quantitySubstitutesTreeColumn";
    this._quantitySubstitutesTreeColumn.Sortable = false;
    this._quantitySubstitutesTreeColumn.ToolTip = "Количество";
    this._quantitySubstitutesTreeColumn.Width = 60;
    this.imagesTreeview.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesTreeview.ImageStream");
    this.imagesTreeview.TransparentColor = Color.Transparent;
    this.imagesTreeview.Images.SetKeyName(0, "group.ico");
    this.imagesTreeview.Images.SetKeyName(1, "main.ico");
    this.imagesTreeview.Images.SetKeyName(2, "alt.ico");
    this.AcceptButton = (IButtonControl) this._okButton;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._cancelButton;
    this.ClientSize = new Size(645, 378);
    this.Controls.Add((Control) this._substitutesTree);
    this.Controls.Add((Control) this.panelBottom);
    this.Name = nameof (SelectSubstituteDlg);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Выберите заменитель";
    this.Load += new EventHandler(this.SelectSubstituteDlg_Load);
    this.panelBottom.ResumeLayout(false);
    this._substitutesTree.EndInit();
    this.ResumeLayout(false);
  }
}

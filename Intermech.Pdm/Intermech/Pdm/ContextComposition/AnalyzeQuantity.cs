// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ContextComposition.AnalyzeQuantity
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using DevExpress.IM.Utils;
using DevExpress.IM.XtraEditors.Controls;
using DevExpress.IM.XtraTreeList;
using DevExpress.IM.XtraTreeList.Columns;
using DevExpress.IM.XtraTreeList.Nodes;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.ContextComposition;

public class AnalyzeQuantity : Form
{
  private DataTable _tableDesign;
  private DataTable _tableTech;
  private IContainer components;
  private TreeList treeAnalyze;
  private TreeListColumn columnVersion;
  private TreeListColumn columnDesign;
  private TreeListColumn columnDesignQuantity;
  private TreeListColumn columnTechQuantity;
  private Label labelProgress;
  private GroupBox groupBox1;
  private Button btnClose;

  public AnalyzeQuantity(DataTable tableDesign, DataTable tableTech)
  {
    this.InitializeComponent();
    this._tableDesign = tableDesign;
    this._tableTech = tableTech;
  }

  public int Analyze()
  {
    try
    {
      ICategoryTypeIconService service = ApplicationServices.Container.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
      this.treeAnalyze.SelectImageList = service.ImageList;
      if (this._tableDesign == null || this._tableTech == null)
        return -1;
      this.treeAnalyze.BeginUpdate();
      this.treeAnalyze.BeginSort();
      this.treeAnalyze.ClearNodes();
      if (this._tableDesign.Rows.Count == 0 || this._tableTech.Rows.Count == 0)
        return -1;
      SortedList<long, ElementQuantity> sortedList1 = new SortedList<long, ElementQuantity>(0);
      SortedList<long, ElementQuantity> sortedList2 = new SortedList<long, ElementQuantity>(0);
      foreach (DataRow row in (InternalDataCollectionBase) this._tableDesign.Rows)
      {
        try
        {
          long int64Value = DataSetProcessor.GetInt64Value(row, 2, 0L);
          int int32Value = DataSetProcessor.GetInt32Value(row, 0, -1);
          string stringValue1 = DataSetProcessor.GetStringValue(row, 3, string.Empty);
          string stringValue2 = DataSetProcessor.GetStringValue(row, 4, string.Empty);
          if (!sortedList1.ContainsKey(int64Value))
          {
            ElementQuantity elementQuantity = new ElementQuantity(stringValue2, int32Value, stringValue1, string.Empty);
            sortedList1[int64Value] = elementQuantity;
          }
          else
          {
            ElementQuantity elementQuantity = sortedList1[int64Value];
            if (stringValue1 != string.Empty)
            {
              MeasuredValue baseMeasure = MeasureHelper.ConvertToBaseMeasure(MeasureHelper.ConvertToMeasuredValue(stringValue1));
              if (elementQuantity.DesignQuantity == null)
                elementQuantity.DesignQuantity = baseMeasure;
              else
                elementQuantity.DesignQuantity.Value += baseMeasure.Value;
            }
          }
        }
        catch
        {
        }
      }
      foreach (DataRow row in (InternalDataCollectionBase) this._tableTech.Rows)
      {
        try
        {
          long int64Value = DataSetProcessor.GetInt64Value(row, 2, 0L);
          string stringValue = DataSetProcessor.GetStringValue(row, 3, string.Empty);
          if (sortedList1.ContainsKey(int64Value))
          {
            ElementQuantity elementQuantity = sortedList1[int64Value];
            if (stringValue != string.Empty)
            {
              MeasuredValue baseMeasure = MeasureHelper.ConvertToBaseMeasure(MeasureHelper.ConvertToMeasuredValue(stringValue));
              if (elementQuantity.TechQuantity == null)
                elementQuantity.TechQuantity = baseMeasure;
              else
                elementQuantity.TechQuantity.Value += baseMeasure.Value;
            }
          }
        }
        catch
        {
        }
      }
      IEnumerator<KeyValuePair<long, ElementQuantity>> enumerator1 = sortedList1.GetEnumerator();
      KeyValuePair<long, ElementQuantity> current;
      if (enumerator1 != null)
      {
        enumerator1.Reset();
        while (enumerator1.MoveNext())
        {
          current = enumerator1.Current;
          if (current.Value.TechQuantity != null)
          {
            current = enumerator1.Current;
            if (current.Value.DesignQuantity != null)
            {
              current = enumerator1.Current;
              double num1 = current.Value.TechQuantity.Value;
              current = enumerator1.Current;
              double num2 = current.Value.DesignQuantity.Value;
              if (num1 > num2)
              {
                SortedList<long, ElementQuantity> sortedList3 = sortedList2;
                current = enumerator1.Current;
                long key = current.Key;
                current = enumerator1.Current;
                ElementQuantity elementQuantity = current.Value;
                sortedList3.Add(key, elementQuantity);
              }
            }
          }
        }
      }
      IEnumerator<KeyValuePair<long, ElementQuantity>> enumerator2 = sortedList2.GetEnumerator();
      if (enumerator2 != null)
      {
        enumerator2.Reset();
        while (enumerator2.MoveNext())
        {
          current = enumerator2.Current;
          double aValue = current.Value.TechQuantity.Value;
          current = enumerator2.Current;
          long measureId = current.Value.TechQuantity.MeasureID;
          MeasuredValue measuredValue = new MeasuredValue(aValue, measureId);
          TreeList treeAnalyze = this.treeAnalyze;
          object[] nodeData = new object[4];
          current = enumerator2.Current;
          nodeData[0] = (object) current.Key;
          current = enumerator2.Current;
          nodeData[1] = (object) current.Value.Caption;
          current = enumerator2.Current;
          nodeData[2] = (object) current.Value.DesignQuantity.Caption;
          nodeData[3] = (object) measuredValue.Caption;
          TreeListNode treeListNode = treeAnalyze.AppendNode((object) nodeData, (TreeListNode) null);
          ICategoryTypeIconService categoryTypeIconService = service;
          current = enumerator2.Current;
          int objectType = current.Value.ObjectType;
          treeListNode.ImageIndex = categoryTypeIconService.IndexOf(4, objectType);
          treeListNode.SelectImageIndex = treeListNode.ImageIndex;
        }
      }
      return sortedList2.Count == 0 ? 0 : -3;
    }
    finally
    {
      this.treeAnalyze.EndSort();
      this.treeAnalyze.EndUpdate();
    }
  }

  private void btnClose_Click(object sender, EventArgs e) => this.Close();

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this.treeAnalyze = new TreeList();
    this.columnVersion = new TreeListColumn();
    this.columnDesign = new TreeListColumn();
    this.columnDesignQuantity = new TreeListColumn();
    this.columnTechQuantity = new TreeListColumn();
    this.labelProgress = new Label();
    this.groupBox1 = new GroupBox();
    this.btnClose = new Button();
    this.treeAnalyze.BeginInit();
    this.groupBox1.SuspendLayout();
    this.SuspendLayout();
    this.treeAnalyze.BehaviorOptions = BehaviorOptionsFlags.MoveOnEdit | BehaviorOptionsFlags.ExpandNodeOnDrag | BehaviorOptionsFlags.ResizeNodes | BehaviorOptionsFlags.AutoSelectAllInEditor | BehaviorOptionsFlags.AutoNodeHeight | BehaviorOptionsFlags.CloseEditorOnLostFocus | BehaviorOptionsFlags.SmartMouseHover;
    this.treeAnalyze.BestFitVisibleOnly = true;
    this.treeAnalyze.BorderStyle = BorderStyles.UltraFlat;
    this.treeAnalyze.Columns.AddRange(new TreeListColumn[4]
    {
      this.columnVersion,
      this.columnDesign,
      this.columnDesignQuantity,
      this.columnTechQuantity
    });
    this.treeAnalyze.Dock = DockStyle.Fill;
    this.treeAnalyze.Location = new Point(0, 22);
    this.treeAnalyze.MenuOptions = MenuOptionsFlags.None;
    this.treeAnalyze.Name = "treeAnalyze";
    this.treeAnalyze.ShowButtonMode = ShowButtonModeEnum.ShowOnlyInEditor;
    this.treeAnalyze.Size = new Size(800, 373);
    this.treeAnalyze.Styles.AddReplace("HideSelectionRow", (object) new ViewStyle("HideSelectionRow", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.MenuHighlight, SystemColors.HighlightText));
    this.treeAnalyze.Styles.AddReplace("HorzLine", (object) new ViewStyle("HorzLine", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.ControlLight, SystemColors.ControlLight));
    this.treeAnalyze.Styles.AddReplace("FocusedRow", (object) new ViewStyle("FocusedRow", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.MenuHighlight, SystemColors.HighlightText));
    this.treeAnalyze.Styles.AddReplace("TreeLine", (object) new ViewStyle("TreeLine", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Window, SystemColors.ControlLight));
    this.treeAnalyze.Styles.AddReplace("FocusedCell", (object) new ViewStyle("FocusedCell", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.MenuHighlight, SystemColors.HighlightText));
    this.treeAnalyze.Styles.AddReplace("SelectedRow", (object) new ViewStyle("SelectedRow", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.MenuHighlight, SystemColors.HighlightText));
    this.treeAnalyze.TabIndex = 2;
    this.treeAnalyze.ViewOptions = ViewOptionsFlags.AutoWidth | ViewOptionsFlags.ShowColumns | ViewOptionsFlags.ShowHorzLines | ViewOptionsFlags.ShowRoot | ViewOptionsFlags.ShowVertLines | ViewOptionsFlags.ShowFocusedFrame;
    this.columnVersion.Caption = "Версия";
    this.columnVersion.FieldName = "Version";
    this.columnVersion.Name = "columnVersion";
    this.columnVersion.Options = ColumnOptions.CanResized | ColumnOptions.CanSorted | ColumnOptions.ReadOnly | ColumnOptions.FixedWidth | ColumnOptions.CanFocused;
    this.columnVersion.VisibleIndex = 0;
    this.columnVersion.Width = 125;
    this.columnDesign.Caption = "Заголовок объекта состава с некорректным количеством";
    this.columnDesign.FieldName = "Design";
    this.columnDesign.Name = "columnDesign";
    this.columnDesign.Options = ColumnOptions.CanResized | ColumnOptions.CanSorted | ColumnOptions.ReadOnly | ColumnOptions.CanFocused;
    this.columnDesign.SortOrder = SortOrder.Ascending;
    this.columnDesign.VisibleIndex = 1;
    this.columnDesign.Width = (int) byte.MaxValue;
    this.columnDesignQuantity.Caption = "Количество";
    this.columnDesignQuantity.FieldName = "DesignQuantity";
    this.columnDesignQuantity.Name = "columnDesignQuantity";
    this.columnDesignQuantity.Options = ColumnOptions.CanResized | ColumnOptions.CanSorted | ColumnOptions.ReadOnly | ColumnOptions.FixedWidth | ColumnOptions.CanFocused;
    this.columnDesignQuantity.VisibleIndex = 2;
    this.columnDesignQuantity.Width = 125;
    this.columnTechQuantity.Caption = "Количество в ТСЕ";
    this.columnTechQuantity.FieldName = "TechQuantity";
    this.columnTechQuantity.Name = "columnTechQuantity";
    this.columnTechQuantity.Options = ColumnOptions.CanResized | ColumnOptions.CanSorted | ColumnOptions.ReadOnly | ColumnOptions.FixedWidth | ColumnOptions.CanFocused;
    this.columnTechQuantity.VisibleIndex = 3;
    this.columnTechQuantity.Width = 125;
    this.labelProgress.Dock = DockStyle.Top;
    this.labelProgress.ImeMode = ImeMode.NoControl;
    this.labelProgress.Location = new Point(0, 0);
    this.labelProgress.Name = "labelProgress";
    this.labelProgress.Size = new Size(800, 22);
    this.labelProgress.TabIndex = 27;
    this.labelProgress.Text = "Список объектов, у которых некорректно задано значение количества:\r\n";
    this.labelProgress.TextAlign = ContentAlignment.BottomLeft;
    this.groupBox1.Controls.Add((Control) this.btnClose);
    this.groupBox1.Dock = DockStyle.Bottom;
    this.groupBox1.Location = new Point(0, 395);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(800, 55);
    this.groupBox1.TabIndex = 28;
    this.groupBox1.TabStop = false;
    this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnClose.Location = new Point(698, 20);
    this.btnClose.Name = "btnClose";
    this.btnClose.Size = new Size(90, 23);
    this.btnClose.TabIndex = 0;
    this.btnClose.Text = "Закрыть";
    this.btnClose.UseVisualStyleBackColor = true;
    this.btnClose.Click += new EventHandler(this.btnClose_Click);
    this.AcceptButton = (IButtonControl) this.btnClose;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(800, 450);
    this.Controls.Add((Control) this.treeAnalyze);
    this.Controls.Add((Control) this.groupBox1);
    this.Controls.Add((Control) this.labelProgress);
    this.Name = nameof (AnalyzeQuantity);
    this.Text = "Результат сравнительного анализа по атрибуту \"Количество\"";
    this.treeAnalyze.EndInit();
    this.groupBox1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}

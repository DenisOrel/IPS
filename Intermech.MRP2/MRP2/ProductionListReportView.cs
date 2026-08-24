// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionListReportView
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MRP;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.MRP2;

[ViewDescriptionProvider(typeof (ProductionListReportView.ProductionListReportViewDescriptionProvider))]
public class ProductionListReportView : UserControl, IView
{
  private long _objectID;
  private IServiceProvider _provider;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private iGCellStyle iGrid1DefaultCellStyle;
  private iGColHdrStyle iGrid1DefaultColHdrStyle;
  private iGCellStyle iGrid1RowTextColCellStyle;
  private DataGridView dataGridView1;
  private ContextMenuStrip contextMenuStrip1;
  private ToolStripMenuItem FindInTreeMenuItem;
  private ToolStripMenuItem CopyTextMenuItem;

  public ProductionListReportView() => this.InitializeComponent();

  public string Caption => "Проверка состава ЭС ПВ";

  public int ImageIndex => -1;

  public int OrderID => 15;

  public void Activate(IView previousView)
  {
    if (previousView == PageViewsManager.BlackHoleView)
      return;
    IProductionListReportService service = ServiceUtils.GetService<IProductionListReportService>((object) ServicesManager.ServiceContainer, false);
    if (service == null)
      return;
    object reportDataSource = service.GetReportDataSource(this._objectID);
    this.dataGridView1.AutoGenerateColumns = true;
    this.dataGridView1.DataSource = reportDataSource;
    this.dataGridView1.DataMember = !(reportDataSource is DataSet dataSet) ? "" : dataSet.Tables[0].TableName;
    this.dataGridView1.ReadOnly = true;
  }

  public void Deactivate(IView nextView)
  {
  }

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    this._provider = provider;
    this._objectID = 0L;
    if (!(items.GetItemData(0, typeof (IDBObjectID)) is IDBObjectID itemData))
      return;
    this._objectID = itemData.Value;
  }

  private void FindInTreeMenuItem_Click(object sender, EventArgs e)
  {
    if (this._provider.GetService<NavigatorTreeView>(false) == null || this.dataGridView1.SelectedRows.Count <= 0)
      return;
    long.TryParse(this.dataGridView1.SelectedRows[0].Cells[0].FormattedValue.ToString(), out long _);
  }

  private void ClipboardSetText(string Data)
  {
    try
    {
      Clipboard.SetText(Data);
    }
    catch
    {
      try
      {
        Clipboard.SetText(Data);
      }
      catch
      {
      }
    }
  }

  private void CopyTextMenuItem_Click(object sender, EventArgs e)
  {
    if (this.dataGridView1.SelectedCells.Count <= 0)
      return;
    this.ClipboardSetText(this.dataGridView1.SelectedCells[0].FormattedValue.ToString());
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
    this.iGrid1DefaultCellStyle = new iGCellStyle(true);
    this.iGrid1DefaultColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1RowTextColCellStyle = new iGCellStyle(true);
    this.dataGridView1 = new DataGridView();
    this.contextMenuStrip1 = new ContextMenuStrip(this.components);
    this.FindInTreeMenuItem = new ToolStripMenuItem();
    this.CopyTextMenuItem = new ToolStripMenuItem();
    ((ISupportInitialize) this.dataGridView1).BeginInit();
    this.contextMenuStrip1.SuspendLayout();
    this.SuspendLayout();
    this.dataGridView1.AllowUserToAddRows = false;
    this.dataGridView1.AllowUserToDeleteRows = false;
    this.dataGridView1.AllowUserToResizeRows = false;
    this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    this.dataGridView1.CausesValidation = false;
    this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
    this.dataGridView1.Dock = DockStyle.Fill;
    this.dataGridView1.Location = new Point(0, 0);
    this.dataGridView1.Name = "dataGridView1";
    this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    this.dataGridView1.Size = new Size(387, 280);
    this.dataGridView1.TabIndex = 0;
    this.contextMenuStrip1.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.FindInTreeMenuItem,
      (ToolStripItem) this.CopyTextMenuItem
    });
    this.contextMenuStrip1.Name = "contextMenuStrip1";
    this.contextMenuStrip1.Size = new Size(181, 70);
    this.FindInTreeMenuItem.Name = "FindInTreeMenuItem";
    this.FindInTreeMenuItem.Size = new Size(180, 22);
    this.FindInTreeMenuItem.Text = "Найти в дереве";
    this.FindInTreeMenuItem.Visible = false;
    this.FindInTreeMenuItem.Click += new EventHandler(this.FindInTreeMenuItem_Click);
    this.CopyTextMenuItem.Name = "CopyTextMenuItem";
    this.CopyTextMenuItem.Size = new Size(170, 22);
    this.CopyTextMenuItem.Text = "Копировать текст";
    this.CopyTextMenuItem.Click += new EventHandler(this.CopyTextMenuItem_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.dataGridView1);
    this.Name = nameof (ProductionListReportView);
    this.Size = new Size(387, 280);
    ((ISupportInitialize) this.dataGridView1).EndInit();
    this.contextMenuStrip1.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private sealed class ProductionListReportViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      return new ViewDescription()
      {
        Caption = "Проверка состава ЭС ПВ",
        ImageIndex = -1,
        OrderID = 15
      };
    }
  }
}

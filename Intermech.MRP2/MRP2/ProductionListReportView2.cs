// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionListReportView2
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.MRP2;

[ViewDescriptionProvider(typeof (ProductionListReportView2.ProductionListReportViewDescriptionProvider2))]
public class ProductionListReportView2 : ChildrenView
{
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private iGCellStyle iGrid1DefaultCellStyle;
  private iGColHdrStyle iGrid1DefaultColHdrStyle;
  private iGCellStyle iGrid1RowTextColCellStyle;
  private DataGridView dataGridView1;

  public ProductionListReportView2() => this.InitializeComponent();

  public override string Caption => "Проверка состава ЭС ПВ";

  public override int OrderID => 15;

  public override void Activate(IView previousView) => base.Activate(previousView);

  public override void Deactivate(IView nextView) => base.Deactivate(nextView);

  public override void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    base.Initialize(items, provider);
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
    this.iGrid1DefaultCellStyle = new iGCellStyle(true);
    this.iGrid1DefaultColHdrStyle = new iGColHdrStyle(true);
    this.iGrid1RowTextColCellStyle = new iGCellStyle(true);
    this.dataGridView1 = new DataGridView();
    ((ISupportInitialize) this.dataGridView1).BeginInit();
    this.SuspendLayout();
    this.dataGridView1.AllowUserToAddRows = false;
    this.dataGridView1.AllowUserToDeleteRows = false;
    this.dataGridView1.AllowUserToResizeRows = false;
    this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    this.dataGridView1.CausesValidation = false;
    this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    this.dataGridView1.Dock = DockStyle.Fill;
    this.dataGridView1.Location = new Point(0, 0);
    this.dataGridView1.Name = "dataGridView1";
    this.dataGridView1.Size = new Size(387, 280);
    this.dataGridView1.TabIndex = 0;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((System.Windows.Forms.Control) this.dataGridView1);
    this.Name = "ProductionListReportView";
    this.Size = new Size(387, 280);
    ((ISupportInitialize) this.dataGridView1).EndInit();
    this.ResumeLayout(false);
  }

  private sealed class ProductionListReportViewDescriptionProvider2 : BaseViewDescriptionProvider
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

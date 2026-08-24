// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.Controls.MaterialProperties.MainPropertiesCtrl
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook.Controls.MaterialProperties;

public class MainPropertiesCtrl : BasePropertiesCtrl
{
  private IContainer components;

  public MainPropertiesCtrl() => this.InitializeComponent();

  public override void LoadSettings()
  {
    base.LoadSettings();
    this.Pages.ForEach((Action<Page>) (page =>
    {
      page.Header.ReadOnly = true;
      page.Tables.ForEach((Action<TableDescription>) (table => table.Header.ReadOnly = true));
    }));
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.SuspendLayout();
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.AutoScroll = true;
    this.Name = nameof (MainPropertiesCtrl);
    this.ResumeLayout(false);
  }
}

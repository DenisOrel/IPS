// Decompiled with JetBrains decompiler
// Type: Intermech.Sales.KeyComposition4RequestView
// Assembly: Intermech.Sales, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D9A9043-6548-439B-99F7-AF22F44A5D2B
// Assembly location: D:\IPS\Client\Intermech.Sales.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Sales;

public class KeyComposition4RequestView : UserControl, IView
{
  protected int imageIndex = -1;
  private IContainer components;

  public KeyComposition4RequestView() => this.InitializeComponent();

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
  }

  public void Activate(IView previousView)
  {
  }

  public void Deactivate(IView nextView)
  {
  }

  public string Caption
  {
    [DebuggerStepThrough] get => LocalizationHolder.rm.GetString("Sales_7");
  }

  public int ImageIndex
  {
    [DebuggerStepThrough] get
    {
      if (this.imageIndex < 0)
        this.imageIndex = (ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList).ImageIndex("");
      return this.imageIndex;
    }
  }

  public int OrderID
  {
    [DebuggerStepThrough] get => 20;
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
    this.AutoScaleMode = AutoScaleMode.Font;
  }
}

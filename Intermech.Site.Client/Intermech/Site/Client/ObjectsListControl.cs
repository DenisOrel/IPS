// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ObjectsListControl
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Controls;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

internal class ObjectsListControl : ChildrenView
{
  private IContainer components;

  public ObjectsListControl()
  {
    this.InitializeComponent();
    this.UseInheritedNavViews = false;
  }

  public bool DataLoaded
  {
    get => this._dataLoaded;
    set => this._dataLoaded = value;
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

// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.Analogs.AnalogsView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.Analogs;

public sealed class AnalogsView : UserControl, IView
{
  private IDBTypedObjectID _typedObjectID;
  private NavigatorTreeView _navigatorTreeView;
  private bool _disableChangeNavigatorTreeViewSelectedNodeBackup;
  private int _imageIndex = -1;
  private IContainer components;
  private AnalogsControl _analogsControl;

  public AnalogsView() => this.InitializeComponent();

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    IDBTypedObjectID typedObjectID;
    if (!AnalogsViewsProvider.CheckParamsForAnalogsView(items, provider, out typedObjectID))
      throw new ArgumentException();
    this._typedObjectID = typedObjectID;
    this._navigatorTreeView = provider.GetService(typeof (NavigatorTreeView)) as NavigatorTreeView;
  }

  public void Activate(IView previousView)
  {
    if (this._typedObjectID != null)
      this._analogsControl.ObjectVersionID = this._typedObjectID.ObjectID;
    if (this._navigatorTreeView == null)
      return;
    this._disableChangeNavigatorTreeViewSelectedNodeBackup = this._navigatorTreeView.DisableChangeSelectedNodeDuringNotificationProcessing;
    this._navigatorTreeView.DisableChangeSelectedNodeDuringNotificationProcessing = true;
  }

  public void Deactivate(IView nextView)
  {
    if (this._navigatorTreeView == null)
      return;
    this._navigatorTreeView.DisableChangeSelectedNodeDuringNotificationProcessing = this._disableChangeNavigatorTreeViewSelectedNodeBackup;
  }

  public string Caption => "Аналоги";

  public int ImageIndex
  {
    get
    {
      if (this._imageIndex < 0)
        this._imageIndex = ServiceLocator.Get<INamedImageList>().ImageIndex(nameof (AnalogsView));
      return this._imageIndex;
    }
  }

  public int OrderID => int.MaxValue;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this._analogsControl = new AnalogsControl();
    this._analogsControl.BeginInit();
    this.SuspendLayout();
    this._analogsControl.Dock = DockStyle.Fill;
    this._analogsControl.Location = new Point(0, 0);
    this._analogsControl.Name = "_analogsControl";
    this._analogsControl.Size = new Size(980, 434);
    this._analogsControl.TabIndex = 0;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._analogsControl);
    this.Name = nameof (AnalogsView);
    this.Size = new Size(980, 434);
    this._analogsControl.EndInit();
    this.ResumeLayout(false);
  }
}

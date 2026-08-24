// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ReceiptContentBaseView
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Bars;
using Intermech.Document.Client;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Site.Client.PortalNavigator;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

internal abstract class ReceiptContentBaseView : UserControl, ICommandTarget, ICommandTarget2, IView
{
  private bool _initmode;
  private ImDocumentEditorForm _form;
  private IContainer components;

  public ReceiptContentBaseView() => this.InitializeComponent();

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    this.OnInitialize(items, provider);
    this._initmode = true;
  }

  protected abstract void OnInitialize(ISelectedItems items, IServiceProvider provider);

  public void Activate(IView previousView)
  {
    if (!this._initmode)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (this._form != null)
      {
        this.Controls.Remove((Control) this._form);
        this._form.Dispose();
        this._form = (ImDocumentEditorForm) null;
      }
      string caption;
      DateTime createDate;
      DataTable receiptContent = this.GetReceiptContent(sessionKeeper.Session, out caption, out createDate);
      if (receiptContent != null)
      {
        this._form = ReceiptTableHelper.LoadDocumentToForm(sessionKeeper.Session, receiptContent, caption, createDate);
        this._form.Dock = DockStyle.Fill;
        this._form.BorderStyle = Intermech.Docking.Rendering.BorderStyle.None;
        this._form.Parent = (Control) this;
        this._form.Visible = true;
      }
      this._initmode = false;
    }
  }

  protected abstract DataTable GetReceiptContent(
    IUserSession session,
    out string caption,
    out DateTime createDate);

  public void Deactivate(IView nextView)
  {
  }

  public string Caption => "Просмотр";

  public int ImageIndex
  {
    get
    {
      return (ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList).ImageIndex("imgView");
    }
  }

  public int OrderID => 40;

  public bool Execute(ICommandState commandState)
  {
    return this._form != null && this._form.Execute(commandState);
  }

  public bool QueryStatus(ICommandState commandState)
  {
    return this._form != null && this._form.QueryStatus(commandState);
  }

  public void BeginQuery()
  {
    if (this._form == null)
      return;
    this._form.BeginQuery();
  }

  public void EndQuery()
  {
    if (this._form == null)
      return;
    this._form.EndQuery();
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

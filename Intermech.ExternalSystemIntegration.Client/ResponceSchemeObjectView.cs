// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.ResponceSchemeObjectView
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Bars;
using Intermech.ExternalSystemIntegration.Interfaces;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

[ViewDescriptionProvider(typeof (ResponceSchemeObjectView.ResponceSchemeObjectViewDescriptionProvider))]
public class ResponceSchemeObjectView : NavBaseView
{
  internal ICommandManager _commandManager;
  private INamedImageList _namedImageList;
  private IContainer components;
  private Label lblRequestSchemeName;
  private ResponceSchemeTreeView responceSchemeTreeView;
  private TextBox tbSchemeName;

  public ResponceSchemeObjectView() => this.InitializeComponent();

  public override string Caption => Const.ResponceSchemeTabName;

  public override int ImageIndex
  {
    get
    {
      INamedImageList namedImageList = this._namedImageList;
      return namedImageList == null ? -1 : namedImageList.ImageIndex(Const.ResponceSchemeIconName);
    }
  }

  public override int OrderID => 1;

  protected override void InitServices(IServiceProvider services)
  {
    base.InitServices(services);
    this._namedImageList = ServiceHolder.NamedImageList;
    this._commandManager = ServicesManager.GetService(typeof (ICommandManager)) as ICommandManager;
  }

  protected override void SaveData(bool sendNotifications = true)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetObject(this._objID, true) is IResponceSchemeObject responceSchemeObject))
        return;
      responceSchemeObject.SchemeName = this.tbSchemeName.Text;
      responceSchemeObject.SchemeData = this.responceSchemeTreeView.SchemeData;
      base.SaveData(sendNotifications);
      if (!sendNotifications)
        return;
      ServiceHolder.NotificationService.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", responceSchemeObject.ObjectID));
    }
  }

  protected override void LoadData()
  {
    base.LoadData();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetObject(this._objID, true) is IResponceSchemeObject responceSchemeObject))
        return;
      this.responceSchemeTreeView.DataChanged -= new EventHandler(this.responceSchemeTreeView_DataChanged);
      this.tbSchemeName.TextChanged -= new EventHandler(this.SchemeName_Changed);
      this.tbSchemeName.Text = responceSchemeObject.SchemeName;
      this.responceSchemeTreeView.Activate(responceSchemeObject.SchemeData);
      this.responceSchemeTreeView.DataChanged += new EventHandler(this.responceSchemeTreeView_DataChanged);
      this.tbSchemeName.TextChanged += new EventHandler(this.SchemeName_Changed);
    }
  }

  private void SchemeName_Changed(object sender, EventArgs e) => this.Modified = true;

  private void responceSchemeTreeView_DataChanged(object sender, EventArgs e)
  {
    this.Modified = true;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ResponceSchemeObjectView));
    this.lblRequestSchemeName = new Label();
    this.tbSchemeName = new TextBox();
    this.responceSchemeTreeView = new ResponceSchemeTreeView();
    this.pnButtons.SuspendLayout();
    this.SuspendLayout();
    this.lblRequestSchemeName.AutoSize = true;
    this.lblRequestSchemeName.Location = new Point(15, 12);
    this.lblRequestSchemeName.Name = "lblRequestSchemeName";
    this.lblRequestSchemeName.Size = new Size(68, 13);
    this.lblRequestSchemeName.TabIndex = 6;
    this.lblRequestSchemeName.Text = "Имя схемы:";
    this.tbSchemeName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbSchemeName.Location = new Point(15, 28);
    this.tbSchemeName.Name = "tbSchemeName";
    this.tbSchemeName.Size = new Size(570, 20);
    this.tbSchemeName.TabIndex = 8;
    this.responceSchemeTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.responceSchemeTreeView.AttributeImage = (Image) componentResourceManager.GetObject("responceSchemeTreeView.AttributeImage");
    this.responceSchemeTreeView.Location = new Point(15, 54);
    this.responceSchemeTreeView.Name = "responceSchemeTreeView";
    this.responceSchemeTreeView.NodeImage = (Image) componentResourceManager.GetObject("responceSchemeTreeView.NodeImage");
    this.responceSchemeTreeView.Size = new Size(570, 286);
    this.responceSchemeTreeView.TabIndex = 7;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tbSchemeName);
    this.Controls.Add((Control) this.responceSchemeTreeView);
    this.Controls.Add((Control) this.lblRequestSchemeName);
    this.Name = nameof (ResponceSchemeObjectView);
    this.Size = new Size(600, 400);
    this.Controls.SetChildIndex((Control) this.pnButtons, 0);
    this.Controls.SetChildIndex((Control) this.lblRequestSchemeName, 0);
    this.Controls.SetChildIndex((Control) this.responceSchemeTreeView, 0);
    this.Controls.SetChildIndex((Control) this.tbSchemeName, 0);
    this.pnButtons.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private sealed class ResponceSchemeObjectViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      if (!(serviceProvider.GetService(typeof (INamedImageList)) is INamedImageList service))
        service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
      INamedImageList namedImageList = service;
      return new ViewDescription()
      {
        Caption = Const.ResponceSchemeTabName,
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex(Const.ResponceSchemeIconName) : -1,
        OrderID = 1
      };
    }
  }
}

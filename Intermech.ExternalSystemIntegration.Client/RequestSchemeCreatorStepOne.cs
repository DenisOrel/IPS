// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.RequestSchemeCreatorStepOne
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Client.Core.ObjectCreator;
using Intermech.Client.Core.ObjectCreator.Controls;
using Intermech.ExternalSystemIntegration.Interfaces;
using Intermech.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class RequestSchemeCreatorStepOne : ObjectCreatorControl
{
  private IContainer components;
  private Label lblRequestSchemeName;
  private RequestSchemeTreeView requestTransfSchemeTreeView;
  private TextBox tbSchemeName;

  public RequestSchemeCreatorStepOne(CreatedObjectItem createdObject)
    : base(createdObject)
  {
    this.InitializeComponent();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetObject(this.CreatedObject.ObjectID, true) is IRequestSchemeObject requestSchemeObject))
        return;
      this.tbSchemeName.Text = requestSchemeObject.Caption;
      this.requestTransfSchemeTreeView.Activate(requestSchemeObject.SchemeData);
    }
  }

  public override bool Save(PageSaveArgs args)
  {
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetObject(this.CreatedObject.ObjectID, true) is IRequestSchemeObject requestSchemeObject)
        {
          requestSchemeObject.Caption = this.tbSchemeName.Text;
          requestSchemeObject.SchemeData = this.requestTransfSchemeTreeView.SchemeData;
        }
      }
      return true;
    }
    catch (Exception ex)
    {
      args.Error = ex;
      return false;
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RequestSchemeCreatorStepOne));
    this.lblRequestSchemeName = new Label();
    this.requestTransfSchemeTreeView = new RequestSchemeTreeView();
    this.tbSchemeName = new TextBox();
    this.SuspendLayout();
    this.lblRequestSchemeName.AutoSize = true;
    this.lblRequestSchemeName.Location = new Point(15, 12);
    this.lblRequestSchemeName.Name = "lblRequestSchemeName";
    this.lblRequestSchemeName.Size = new Size(68, 13);
    this.lblRequestSchemeName.TabIndex = 7;
    this.lblRequestSchemeName.Text = "Имя схемы:";
    this.requestTransfSchemeTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.requestTransfSchemeTreeView.AttributeImage = (Image) componentResourceManager.GetObject("requestTransfSchemeTreeView.AttributeImage");
    this.requestTransfSchemeTreeView.Location = new Point(15, 54);
    this.requestTransfSchemeTreeView.Name = "requestTransfSchemeTreeView";
    this.requestTransfSchemeTreeView.NodeImage = (Image) componentResourceManager.GetObject("requestTransfSchemeTreeView.NodeImage");
    this.requestTransfSchemeTreeView.Size = new Size(570, 328);
    this.requestTransfSchemeTreeView.TabIndex = 8;
    this.tbSchemeName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbSchemeName.Location = new Point(15, 28);
    this.tbSchemeName.Name = "tbSchemeName";
    this.tbSchemeName.Size = new Size(570, 20);
    this.tbSchemeName.TabIndex = 9;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tbSchemeName);
    this.Controls.Add((Control) this.requestTransfSchemeTreeView);
    this.Controls.Add((Control) this.lblRequestSchemeName);
    this.Name = nameof (RequestSchemeCreatorStepOne);
    this.Size = new Size(600, 400);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}

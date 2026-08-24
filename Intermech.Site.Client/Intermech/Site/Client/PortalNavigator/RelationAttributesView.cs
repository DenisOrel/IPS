// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.RelationAttributesView
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class RelationAttributesView : UserControl, IView
{
  private bool _initmode;
  private long _relationId;
  private AttributesForm _attributesForm;

  public RelationAttributesView()
  {
    INamedImageList service = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
    if (service == null)
      return;
    this.ImageIndex = service.ImageIndex("imgLink");
  }

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    if (!(items.GetItemData(0, typeof (IPublishRelationID)) is IPublishRelationID itemData))
      return;
    this._relationId = itemData.RelationID;
    this._initmode = true;
  }

  public void Activate(IView previousView)
  {
    if (!this._initmode)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      Guid connectGuid = Guid.Empty;
      IPortalConnector customService = (IPortalConnector) sessionKeeper.Session.GetCustomService(typeof (IPortalConnector));
      try
      {
        connectGuid = customService.Login(sessionKeeper.Session.SessionGUID);
        PublishAttribute[] relationAttributes = customService.GetRelationAttributes(connectGuid, this._relationId);
        if (this._attributesForm == null)
        {
          this._attributesForm = new AttributesForm();
          this._attributesForm.SetParent((Control) this);
        }
        this._attributesForm.LoadData(sessionKeeper.Session, relationAttributes);
        this._initmode = false;
      }
      finally
      {
        if (connectGuid != Guid.Empty && customService != null)
          customService.Logout(connectGuid);
      }
    }
  }

  public void Deactivate(IView nextView)
  {
  }

  public string Caption { get; } = LocalizationHolder.rm.GetString("Site.Client_7");

  public int ImageIndex { get; } = -1;

  public int OrderID { get; } = 2;
}

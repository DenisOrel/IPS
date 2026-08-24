// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.ObjectAttributesView
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

internal sealed class ObjectAttributesView : UserControl, IView
{
  private bool _initmode;
  private long _objectID;
  private AttributesForm _attributesForm;

  public ObjectAttributesView()
  {
    INamedImageList service = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
    if (service == null)
      return;
    this.ImageIndex = service.ImageIndex("imgProp");
  }

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    if (!(items.GetItemData(0, typeof (IPublishTypedID)) is IPublishTypedID itemData))
      return;
    this._objectID = itemData.ObjectID;
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
        PublishAttribute[] objectAttributes = customService.GetObjectAttributes(connectGuid, this._objectID);
        if (this._attributesForm == null)
        {
          this._attributesForm = new AttributesForm();
          this._attributesForm.SetParent((Control) this);
        }
        this._attributesForm.LoadData(sessionKeeper.Session, objectAttributes);
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

  public int ImageIndex { get; } = -1;

  public string Caption { get; } = LocalizationHolder.rm.GetString("Site.Client_6");

  public int OrderID { get; }
}

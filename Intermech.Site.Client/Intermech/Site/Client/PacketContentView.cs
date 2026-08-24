// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PacketContentView
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Interfaces;
using Intermech.Site.Client.PortalNavigator;
using System;
using System.Data;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class PacketContentView : ReceiptContentBaseView
{
  private IPacketNodeID _packet;

  protected override void OnInitialize(ISelectedItems items, IServiceProvider provider)
  {
    if (!(items.GetItemData(0, typeof (IPacketNodeID)) is IPacketNodeID itemData))
      return;
    this._packet = itemData;
  }

  protected override DataTable GetReceiptContent(
    IUserSession session,
    out string caption,
    out DateTime createDate)
  {
    if (this._packet == null)
    {
      caption = string.Empty;
      createDate = DateTime.MinValue;
      return (DataTable) null;
    }
    Guid guid = Guid.Empty;
    IPortalConnector customService = (IPortalConnector) session.GetCustomService(typeof (IPortalConnector));
    try
    {
      guid = customService.Login(session.SessionGUID);
      caption = $"Содержимое пакета {this._packet.Caption}";
      createDate = this._packet.CreateDate;
      return customService.GetPacketContent(guid, this._packet.ObjectID);
    }
    finally
    {
      if (guid != Guid.Empty && customService != null)
        customService.Logout(guid);
    }
  }
}

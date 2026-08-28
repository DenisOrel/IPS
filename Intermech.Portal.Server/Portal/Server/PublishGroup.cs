// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.PublishGroup
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class PublishGroup(IUserSession session, IDBObject obj) : GroupPublishItem(session, obj)
{
  private IDBAttribute _attrRelationsList;

  public void AddItemToRelationList(Guid relationGuid)
  {
    if (this._attrRelationsList == null)
    {
      this._attrRelationsList = this.DBObject.GetAttributeByGuid(PortalServerConsts.attributeRelationsList, false);
      if (this._attrRelationsList == null)
        this._attrRelationsList = this.DBObject.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalServerConsts.attributeRelationsList), false);
    }
    if (this._attrRelationsList.IsNull)
      this._attrRelationsList.Value = (object) relationGuid;
    else
      this._attrRelationsList.AddValue((object) relationGuid);
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.GroupPublishItem
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;

#nullable disable
namespace Intermech.Portal.Server;

internal class GroupPublishItem
{
  protected IUserSession session;

  public GroupPublishItem(IUserSession session, IDBObject obj)
  {
    this.session = session;
    this.DBObject = obj;
  }

  public IDBObject DBObject { get; private set; }

  public static GroupPublishItem GetPacket(IUserSession session, long objectID)
  {
    IDBObject dbObject = session.GetObject(objectID);
    if (dbObject.ObjectType == MetaDataHelper.GetObjectTypeID(PortalConsts.objtypeGroup))
      return new GroupPublishItem(session, dbObject);
    return dbObject.ObjectType == MetaDataHelper.GetObjectTypeID(PortalConsts.objtypePacket) ? (GroupPublishItem) new PublishPacket(session, dbObject) : (GroupPublishItem) null;
  }

  public virtual void CommitCreate()
  {
  }
}

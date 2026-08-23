// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Server.SignLinkedObjectsHandler
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Signs.Interfaces;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Server;

internal sealed class SignLinkedObjectsHandler : LinkedObjectsHandler, ILinkedObjectsHandler
{
  public List<int> HandleTypes
  {
    get
    {
      return new List<int>((IEnumerable<int>) new int[1]
      {
        SignsHolder.SignObjectTypeID
      });
    }
  }

  public List<int> OutputTypes
  {
    get
    {
      return new List<int>((IEnumerable<int>) new int[1]
      {
        MetaDataHelper.GetObjectTypeID("cad00002-306c-11d8-b4e9-00304f19f545")
      });
    }
  }

  public List<LinkedObject> Handle(
    IUserSession session,
    long objectID,
    int objectType,
    string filtrationOwnerID)
  {
    IDBAttribute attributeById = session.GetObject(objectID).GetAttributeByID(SignsHolder.SignUpAttrTypeID);
    if (attributeById == null || attributeById.AsInteger == 0L)
      return (List<LinkedObject>) null;
    return new List<LinkedObject>()
    {
      new LinkedObject(attributeById.AsInteger)
    };
  }

  public string Name => "Модуль Подписей";

  protected override void OnReloadTypes()
  {
  }

  bool ILinkedObjectsHandler.IsTypesChanged(IUserSession session) => this.IsTypesChanged(session);

  void ILinkedObjectsHandler.UpdateHandleAndOutputTypes(IUserSession session, bool force)
  {
    this.UpdateHandleAndOutputTypes(session, force);
  }
}

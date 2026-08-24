// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.PdmObject
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Pdm;

public static class PdmObject
{
  public static MyObjectElement GetItemInfo(
    IUserSession session,
    ISelectedItems items,
    int itemIndex)
  {
    return items == null || items.Count <= 0 || itemIndex >= items.Count ? (MyObjectElement) null : PdmObject.GetItemInfo(session, items.GetItemData(itemIndex, typeof (IDBTypedObjectID)) as IDBTypedObjectID, items.GetItemData(itemIndex, typeof (IDBRelationID)) as IDBRelationID);
  }

  public static MyObjectElement GetItemInfo(
    IUserSession session,
    IDBTypedObjectID typedObj,
    IDBRelationID relID)
  {
    if (typedObj == null || relID == null || session == null)
      return (MyObjectElement) null;
    long objectID = typedObj.ObjectID;
    if (relID != null)
    {
      IDBRelation relation = session.GetRelation(relID.Value, false);
      if (relation != null)
        objectID = relation.ProjID;
    }
    IDBObject dbObject = session.GetObject(objectID, false);
    return dbObject == null ? (MyObjectElement) null : new MyObjectElement(dbObject.ID, dbObject.ObjectID, dbObject.ObjectType, relID.Value, relID.RelationType, dbObject.Caption, false, dbObject.GUID, (long) dbObject.VersionID, Convert.ToInt64(dbObject.IsBaseVersion), Array.Empty<object>());
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.RootItemFactory
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using System;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class RootItemFactory
{
  public CompositionItem GetItem(IUserSession session, IDBTypedObjectID item)
  {
    IDBObject dbObject = session.GetObject(item.ObjectID);
    return new CompositionItem(item.ObjectID, item.ID, item.ObjectType, item.Version, item.Caption, item.Owner, item.BaseVersion, item.SiteID, item.ModificationID, session.GetLifecycleStep(dbObject.LCStep).LevelID, dbObject.CheckoutBy, dbObject.ProjectID);
  }

  public CompositionItem GetItem(IUserSession session, long objectID)
  {
    IDBObject dbObject = session.GetObject(objectID, false);
    if (dbObject == null)
      throw new Exception($"Ошибка при чтении корневого объекта для дерева сравнения составов. Объект {objectID} не найден!");
    return new CompositionItem(dbObject.ObjectID, dbObject.ID, dbObject.ObjectType, (long) dbObject.VersionID, dbObject.Caption, dbObject.OwnerID, dbObject.IsBaseVersion ? 1L : 0L, dbObject.SiteID, dbObject.ModificationID, session.GetLifecycleStep(dbObject.LCStep).LevelID, dbObject.CheckoutBy, dbObject.ProjectID);
  }
}

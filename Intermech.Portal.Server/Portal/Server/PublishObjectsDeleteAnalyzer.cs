// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.PublishObjectsDeleteAnalyzer
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Portal.Server;

internal class PublishObjectsDeleteAnalyzer : IObjectsDeleteAnalyzer
{
  public Guid Guid => new Guid("CE2DAF42-6135-47DB-A67F-56A70BE31C70");

  public int Analyze(
    IUserSession session,
    DeletingObjects deletingObjects,
    DeleteAnalyzerOptions options)
  {
    List<int> childrenIdRecursive = MetaDataHelper.GetObjectTypeChildrenIDRecursive(PortalConsts.objtypePublishObjects);
    List<long> longList = new List<long>();
    foreach (DeletingObject deletingObject in (List<DeletingObject>) deletingObjects)
    {
      if (childrenIdRecursive.Contains(deletingObject.ObjectType))
        longList.Add(deletingObject.ObjectID);
    }
    if (longList.Count == 0)
      return 0;
    List<Tuple<long, bool>> objects = new List<Tuple<long, bool>>();
    List<Tuple<Guid, long>> relations = new List<Tuple<Guid, long>>();
    CompositionHelper.GetComposition(session, longList.ToArray(), (string[]) null, objects, relations, -1);
    if (objects.Count == 0)
      return 0;
    int num = 0;
    IDBRelationCollection relationCollection = session.GetRelationCollection(MetaDataHelper.GetRelationTypeID(PortalConsts.reltypePublish));
    foreach (Tuple<long, bool> tuple in objects)
    {
      Tuple<long, bool> objID = tuple;
      if (deletingObjects.FindDeletingObject(objID.Item1) == null)
      {
        QuickObjectInfo objectInfo = session.GetObjectInfo(objID.Item1);
        bool removeObject = true;
        DataTable relTable = relationCollection.EntersInVersion(new DBRecordSetParams((ConditionStructure[]) null, new object[1]
        {
          (object) -26
        }), objID.Item1);
        for (int i = 0; i < relTable.Rows.Count; i++)
        {
          if (!relations.Exists((Predicate<Tuple<Guid, long>>) (x => x.Item1.Equals(new Guid(Convert.ToString(relTable.Rows[i][0]))) && x.Item2 == objID.Item1)))
          {
            removeObject = false;
            break;
          }
        }
        deletingObjects.Add(0L, objectInfo.ID, objID.Item1, removeObject);
        ++num;
      }
    }
    return num;
  }
}

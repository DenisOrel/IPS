// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareHelper
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal static class CompareHelper
{
  public static bool Grouping(List<int> relationTypes, List<int> compareAttributes, bool recursive)
  {
    return recursive && relationTypes.Count == 1 && relationTypes[0] == MetaDataHelper.GetRelationTypeID("cad00023-306c-11d8-b4e9-00304f19f545") && compareAttributes.Contains(MetaDataHelper.GetAttributeTypeID("cad00267-306c-11d8-b4e9-00304f19f545"));
  }

  public static Dictionary<int, bool> GetOwnRelationTypes(
    IUserSession session,
    List<int> objectTypes)
  {
    Hashtable hashtable = new Hashtable();
    HashSet<int> intSet = new HashSet<int>(objectTypes.Count);
    foreach (int objectType1 in objectTypes)
    {
      foreach (DataRow row in (InternalDataCollectionBase) session.GetRelationsApplicabilityCollection().GetApplicabilitiesList(-1, -1, objectType1).Rows)
      {
        int int32 = Convert.ToInt32(row["F_RELATION_TYPE"]);
        if (hashtable.Contains((object) int32))
          hashtable[(object) int32] = (object) (Convert.ToInt32(hashtable[(object) int32]) + 1);
        else
          hashtable.Add((object) int32, (object) 1);
      }
      IDBObjectType objectType2 = session.GetObjectType(objectType1);
      if (objectType2.DefaultRelation != -1 && !intSet.Contains(objectType2.DefaultRelation))
        intSet.Add(objectType2.DefaultRelation);
    }
    Dictionary<int, bool> ownRelationTypes = new Dictionary<int, bool>(hashtable.Count);
    IDictionaryEnumerator enumerator = hashtable.GetEnumerator();
    while (enumerator.MoveNext())
    {
      if ((int) enumerator.Value >= objectTypes.Count)
        ownRelationTypes.Add((int) enumerator.Key, intSet.Contains((int) enumerator.Key));
    }
    return ownRelationTypes;
  }
}

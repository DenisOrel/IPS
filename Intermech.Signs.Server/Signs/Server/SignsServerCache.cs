// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Server.SignsServerCache
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using Intermech.Interfaces;
using Intermech.Signs.Interfaces;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.Signs.Server;

public class SignsServerCache
{
  private static object SyncSignRelation = new object();
  private static ArrayList ObjectTypesForSignRelation = new ArrayList();
  private static DualKeysCache ObjectTypeCache = new DualKeysCache();
  private static Hashtable ObjectLevelCache = Hashtable.Synchronized(new Hashtable());
  private static ConcurrentDictionary<long, byte[]> SignsSetupCache = new ConcurrentDictionary<long, byte[]>();
  internal static Dictionary<string, string> PossibleGraphs = new Dictionary<string, string>();
  private static object SyncGraphs = new object();

  public static void LoadObjectTypesForSignRelation()
  {
    lock (SignsServerCache.SyncSignRelation)
    {
      SignsServerCache.ObjectTypesForSignRelation.Clear();
      foreach (IMSObjectType objectTypes in MetaDataHelper.GetObjectTypesList())
      {
        IMSApplicability applicability = MetaDataHelper.GetApplicability(objectTypes.ObjectTypeID, SignsHolder.SignObjectTypeID, SignsHolder.SignRelationTypeID);
        if (applicability != null && applicability.ApplicabilityMode != ApplicabilityModes.Disabled && !SignsServerCache.ObjectTypesForSignRelation.Contains((object) objectTypes.ObjectTypeID))
          SignsServerCache.ObjectTypesForSignRelation.Add((object) objectTypes.ObjectTypeID);
      }
    }
  }

  public static void AddObjectTypeForSign(int objectType)
  {
    lock (SignsServerCache.SyncSignRelation)
    {
      if (SignsServerCache.ObjectTypesForSignRelation.Contains((object) objectType))
        return;
      SignsServerCache.ObjectTypesForSignRelation.Add((object) objectType);
    }
  }

  public static void RemoveObjectTypeForSign(int objectType)
  {
    lock (SignsServerCache.SyncSignRelation)
    {
      if (!SignsServerCache.ObjectTypesForSignRelation.Contains((object) objectType))
        return;
      SignsServerCache.ObjectTypesForSignRelation.Remove((object) objectType);
    }
  }

  public static bool HasSignApp(int objectType)
  {
    lock (SignsServerCache.SyncSignRelation)
      return SignsServerCache.ObjectTypesForSignRelation.Contains((object) objectType);
  }

  public static void CleanCaches()
  {
    SignsServerCache.ObjectLevelCache.Clear();
    SignsServerCache.ObjectTypeCache.Clear();
    SignsServerCache.ClearRankSignsSetupCache();
  }

  public static void AddObjectType(Guid objectType, Guid step, GraphsSet tgSet)
  {
    if (SignsServerCache.ObjectTypeCache.ContainsKeys((object) objectType, (object) step))
      return;
    SignsServerCache.ObjectTypeCache.Add((object) objectType, (object) step, (object) tgSet);
  }

  public static GraphsSet GetGraphsSetForObjectType(Guid objectType, Guid step)
  {
    return !SignsServerCache.ObjectTypeCache.ContainsKeys((object) objectType, (object) step) ? (GraphsSet) null : SignsServerCache.ObjectTypeCache[(object) objectType, (object) step] as GraphsSet;
  }

  public static void AddObjectLevel(Guid level, GraphsSet tgSet)
  {
    if (SignsServerCache.ObjectLevelCache.ContainsKey((object) level))
      return;
    SignsServerCache.ObjectLevelCache.Add((object) level, (object) tgSet);
  }

  public static GraphsSet GetGraphsSetForLevel(Guid level)
  {
    return !SignsServerCache.ObjectLevelCache.ContainsKey((object) level) ? (GraphsSet) null : SignsServerCache.ObjectLevelCache[(object) level] as GraphsSet;
  }

  public static byte[] GetSignsSetup(IUserSession session, long objSetupId)
  {
    byte[] signsSetup = (byte[]) null;
    if (SignsServerCache.SignsSetupCache.TryGetValue(objSetupId, out signsSetup))
      return signsSetup;
    IDBObject dbObject = session.GetObject(objSetupId);
    if (dbObject != null)
    {
      IDBAttribute attributeById = dbObject.GetAttributeByID(SignsHolder.SignsSetupAttrTypeID);
      using (MemoryStream aDestStream = new MemoryStream())
      {
        try
        {
          new BlobProcReader(attributeById, 0, (Stream) aDestStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData(session);
          if (aDestStream.Length > 0L)
          {
            aDestStream.Position = 0L;
            byte[] array = aDestStream.ToArray();
            SignsServerCache.SignsSetupCache.TryAdd(objSetupId, array);
            return array;
          }
        }
        catch
        {
        }
      }
    }
    return (byte[]) null;
  }

  public static void DropSignsSetup(long objSetupId)
  {
    SignsServerCache.SignsSetupCache.TryRemove(objSetupId, out byte[] _);
  }

  public static void ClearRankSignsSetupCache() => SignsServerCache.SignsSetupCache.Clear();

  internal static void LoadPossibleGraphs(IUserSession session)
  {
    lock (SignsServerCache.SyncGraphs)
    {
      SignsServerCache.PossibleGraphs.Clear();
      DataTable possibleValues = session.GetAttributeType(SignsHolder.GraphAttrTypeID).GetPossibleValues();
      if (possibleValues == null)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) possibleValues.Rows)
      {
        string key = row["F_STRING_VALUE"].ToString();
        string str = row["F_DESCRIPTION"].ToString();
        if (str.Equals(string.Empty))
          str = key;
        SignsServerCache.PossibleGraphs[key] = str;
      }
    }
  }

  public static string GetGraphDescr(string graphValue)
  {
    lock (SignsServerCache.SyncGraphs)
      return SignsServerCache.PossibleGraphs.ContainsKey(graphValue) ? SignsServerCache.PossibleGraphs[graphValue] : string.Empty;
  }

  public static Dictionary<string, string> GetPossibleGraphs()
  {
    lock (SignsServerCache.SyncGraphs)
      return new Dictionary<string, string>((IDictionary<string, string>) SignsServerCache.PossibleGraphs);
  }
}

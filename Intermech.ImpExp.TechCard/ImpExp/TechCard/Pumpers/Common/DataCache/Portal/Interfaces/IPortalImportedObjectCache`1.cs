// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Interfaces.IPortalImportedObjectCache`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Interfaces;

public interface IPortalImportedObjectCache<T> where T : PortalImportedObject
{
  string GetUniqueObjId(T target);

  string GetUniqueObjId(params object[] idParams);

  IReadOnlyCollection<string> Ids { get; }

  IReadOnlyCollection<T> Objects { get; }

  T FindObjectInCache(string uniqueId);

  T this[string uniqueId] { get; }

  Guid ObjectType { get; }

  void Load();

  bool Loaded { get; }
}

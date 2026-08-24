// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ResolutionCopiesForMultipleUsersDescriptor
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Persistence;
using Intermech.Office.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Office.Client;

internal class ResolutionCopiesForMultipleUsersDescriptor(
  [NotNull] string caption,
  [NotNull] IEnumerable<long> resolutionIDs) : 
  MultipleResolutionsDescriptor(caption, resolutionIDs),
  IDescriptor,
  INodeItems,
  IPersistable
{
  public ResolutionCopiesForMultipleUsersDescriptor([NotNull] IEnumerable<long> resolutionIDs)
    : this(Session.Invoke<string>((Session.SessionHandler<string>) (session => session.GetObjectInfo(resolutionIDs.First<long>()).Caption)), resolutionIDs)
  {
  }

  protected override IEnumerable<IDescriptor> CreateChildDescriptors(IEnumerable<long> resolutionIDs)
  {
    using (SessionKeeper sk = new SessionKeeper())
    {
      foreach (long resolutionId in resolutionIDs)
      {
        IDBObject dbObject = sk.Session.GetObject(resolutionId, false);
        if (dbObject != null)
        {
          IDBAttribute attributeById = dbObject.GetAttributeByID(OfficeConsts.AttrExecutorsID);
          if (attributeById != null && !attributeById.IsNull && attributeById.ValuesCount > 0)
          {
            long int64 = Convert.ToInt64(attributeById.Value);
            if (int64 != 0L)
              yield return (IDescriptor) new ResolutionCopyForUserDescriptor(resolutionId, int64);
          }
        }
      }
    }
  }
}

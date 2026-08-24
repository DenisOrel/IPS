// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareObjectsListQuery
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Specialized;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class CompareObjectsListQuery(
  INodeQuerySupport support,
  int objTypeID,
  ConditionStructure[] conditions,
  IServiceProvider services) : ObjectsQuery(support, objTypeID, conditions, services)
{
  protected override void BeforeSelect(DBRecordSetParams queryParams)
  {
    if (queryParams.Tags == null)
      queryParams.Tags = new HybridDictionary(1);
    queryParams.Tags.Add((object) "ShowNotOwnedWorkCopies", (object) true);
  }
}

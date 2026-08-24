// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SubstitutesQuery
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;

#nullable disable
namespace Intermech.Pdm;

public class SubstitutesQuery : RelatedObjectsQuery, IFiltrationClass
{
  private int _relationTypeID;
  private string _filtrationOwnerID;
  private List<long> _contexts;

  public static void BlockPluginFiltrations(ref DBRecordSetParams paramsSet)
  {
    IFiltrationService service = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
    paramsSet.Tags = service == null || service.Filtration.Tags == null ? new HybridDictionary(0, true) : service.Filtration.Tags;
    paramsSet.Tags[(object) "{2FACA180-73B8-4F24-9928-5623661BBBE6}"] = (object) true;
    paramsSet.Tags[(object) "{325F5CDB-8B8E-4B2D-9AA9-5624A0A64D7E}"] = (object) true;
    paramsSet.Tags[(object) "{529FFE92-FDA7-48B8-AADF-ADB1EE6EF584}"] = (object) false;
    paramsSet.Tags[(object) "{0422E069-0A1D-4235-85E8-C52C3516CFC1}"] = (object) true;
  }

  public SubstitutesQuery(
    IServiceProvider services,
    INodeQuerySupport support,
    long objId,
    int objTypeID,
    RelatedObjectsRole role,
    int relTypeId,
    ConditionStructure[] conditions,
    string filtrationOwnerID,
    List<long> contexts)
    : base(support, objId, objTypeID, role, relTypeId, conditions)
  {
    this.Services = services;
    SubstitutesDescriptor.CorrectStatics();
    this._relationTypeID = relTypeId >= 0 ? relTypeId : SubstitutesDescriptor.ProjectRelationTypeID;
    this._filtrationOwnerID = filtrationOwnerID != string.Empty ? filtrationOwnerID : "cad001e2-306c-11d8-b4e9-00304f19f545";
    if (contexts == null || contexts.Count <= 0)
      return;
    this._contexts = contexts;
  }

  protected override DBRecordSetParams GetQueryParams(
    object bookmark,
    int count,
    RecordMapping mapping)
  {
    DBRecordSetParams queryParams = base.GetQueryParams(bookmark, count, mapping);
    SubstitutesQuery.BlockPluginFiltrations(ref queryParams);
    if (this._contexts != null && queryParams.Tags != null)
      queryParams.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) this._contexts;
    return queryParams;
  }

  public string FiltrationOwnerID
  {
    [DebuggerStepThrough] get => this._filtrationOwnerID;
  }
}

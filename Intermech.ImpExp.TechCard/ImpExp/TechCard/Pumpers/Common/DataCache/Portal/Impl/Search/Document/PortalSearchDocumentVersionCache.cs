// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Search.Document.PortalSearchDocumentVersionCache
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Interfaces;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Search.Document;

public class PortalSearchDocumentVersionCache : 
  PortalImportedObjectVersionCache<PortalSearchDocumentVersion>,
  IPortalSearchDocumentVersionCache,
  IPortalImportedObjectCache<PortalSearchDocumentVersion>
{
  protected override string GetQueryToPortalImportData()
  {
    return "select REC_ID, DOC_ID, VERSION_ID, DOC_GUID from RC";
  }

  protected override IDataBase GetDbConnection() => SearchConnectionsManager.GetConnection();

  protected override PortalSearchDocumentVersion CreateImportedData()
  {
    return new PortalSearchDocumentVersion();
  }

  protected override bool FillImportedData(
    PortalSearchDocumentVersion target,
    IDataReader dataReader)
  {
    target.DocVerId = dataReader.GetInt32(0);
    target.DocId = dataReader.GetInt32(1);
    target.DocVer = dataReader.GetInt32(2);
    Guid result;
    if (Guid.TryParse(dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3), out result))
      target.IpsObjVerGuid = result;
    return true;
  }

  public override string GetUniqueObjId(PortalSearchDocumentVersion target)
  {
    return $"{target.DocId}|{target.DocVer}";
  }

  public override string GetUniqueObjId(params object[] idParams)
  {
    return idParams.Length >= 2 ? $"{idParams[0]}|{idParams[1]}" : string.Empty;
  }

  public override Guid ObjectType => new Guid("cad00070-306c-11d8-b4e9-00304f19f545");
}

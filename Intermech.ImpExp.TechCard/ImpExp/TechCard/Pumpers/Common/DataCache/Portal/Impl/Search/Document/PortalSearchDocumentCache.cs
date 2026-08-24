// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Search.Document.PortalSearchDocumentCache
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Common;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Search.Document;

public class PortalSearchDocumentCache : PortalImportedObjectCache<PortalSearchDocument>
{
  protected override string GetQueryToPortalImportData() => "select DOC_ID, F_GUID from GUIDS_DOC";

  protected override IDataBase GetDbConnection() => SearchConnectionsManager.GetConnection();

  protected override PortalSearchDocument CreateImportedData() => new PortalSearchDocument();

  protected override bool FillImportedData(PortalSearchDocument target, IDataReader dataReader)
  {
    target.DocId = dataReader.GetInt32(0);
    Guid result;
    if (Guid.TryParse(dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1), out result))
      target.IpsObjVerGuid = result;
    return true;
  }

  public override string GetUniqueObjId(PortalSearchDocument target) => target.DocId.ToString();

  public override string GetUniqueObjId(params object[] idParams)
  {
    return idParams.Length != 0 ? idParams[0].ToString() : string.Empty;
  }

  public override Guid ObjectType => new Guid("cad00070-306c-11d8-b4e9-00304f19f545");
}

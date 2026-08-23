// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetCache
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Interfaces;
using Intermech.Signs.Interfaces;
using System;

#nullable disable
namespace Intermech.Signs.Client;

internal class CertSheetCache
{
  public static readonly Guid DocumentObjectTypeGuid = new Guid("cad00070-306c-11d8-b4e9-00304f19f545");
  private static int? documentObjectTypeID = new int?();
  public static readonly Guid ECOObjectTypeGuid = new Guid("cad00348-306c-11d8-b4e9-00304f19f545");
  private static int? ecoObjectTypeID = new int?();
  public static readonly Guid SpecObjectTypeGuid = new Guid("cad00133-306c-11d8-b4e9-00304f19f545");
  private static int? specObjectTypeID = new int?();
  public static readonly Guid ECORelationTypeGuid = new Guid("cad0036b-306c-11d8-b4e9-00304f19f545");
  private static int? ecoRelationTypeID = new int?();
  public static readonly Guid CompositionRelationTypeGuid = new Guid("cad00023-306c-11d8-b4e9-00304f19f545");
  private static int? compositionRelationTypeID = new int?();
  public static readonly Guid DocumentationRelationTypeGuid = new Guid("cad00154-306c-11d8-b4e9-00304f19f545");
  private static int? documentationRelationTypeID = new int?();
  public static readonly Guid SignsRelationTypeGuid = SignsHolder.SignRelationTypeGuid;
  private static int? signsRelationTypeID = new int?();

  public static int DocumentObjectTypeID
  {
    get
    {
      if (!CertSheetCache.documentObjectTypeID.HasValue)
        CertSheetCache.documentObjectTypeID = new int?(MetaDataHelper.GetObjectType(CertSheetCache.DocumentObjectTypeGuid).ObjectTypeID);
      return CertSheetCache.documentObjectTypeID.Value;
    }
  }

  public static int ECOObjectTypeID
  {
    get
    {
      if (!CertSheetCache.ecoObjectTypeID.HasValue)
        CertSheetCache.ecoObjectTypeID = new int?(MetaDataHelper.GetObjectType(CertSheetCache.ECOObjectTypeGuid).ObjectTypeID);
      return CertSheetCache.ecoObjectTypeID.Value;
    }
  }

  public static int SpecObjectTypeID
  {
    get
    {
      if (!CertSheetCache.specObjectTypeID.HasValue)
        CertSheetCache.specObjectTypeID = new int?(MetaDataHelper.GetObjectType(CertSheetCache.SpecObjectTypeGuid).ObjectTypeID);
      return CertSheetCache.specObjectTypeID.Value;
    }
  }

  public static int ECORelationTypeID
  {
    get
    {
      if (!CertSheetCache.ecoRelationTypeID.HasValue)
        CertSheetCache.ecoRelationTypeID = new int?(MetaDataHelper.GetRelationType(CertSheetCache.ECORelationTypeGuid).RelationTypeID);
      return CertSheetCache.ecoRelationTypeID.Value;
    }
  }

  public static int CompositionRelationTypeID
  {
    get
    {
      if (!CertSheetCache.compositionRelationTypeID.HasValue)
        CertSheetCache.compositionRelationTypeID = new int?(MetaDataHelper.GetRelationType(CertSheetCache.CompositionRelationTypeGuid).RelationTypeID);
      return CertSheetCache.compositionRelationTypeID.Value;
    }
  }

  public static int DocumentationRelationTypeID
  {
    get
    {
      if (!CertSheetCache.documentationRelationTypeID.HasValue)
        CertSheetCache.documentationRelationTypeID = new int?(MetaDataHelper.GetRelationType(CertSheetCache.DocumentationRelationTypeGuid).RelationTypeID);
      return CertSheetCache.documentationRelationTypeID.Value;
    }
  }

  public static int SignsRelationTypeID
  {
    get
    {
      if (!CertSheetCache.signsRelationTypeID.HasValue)
        CertSheetCache.signsRelationTypeID = new int?(MetaDataHelper.GetRelationType(CertSheetCache.SignsRelationTypeGuid).RelationTypeID);
      return CertSheetCache.signsRelationTypeID.Value;
    }
  }
}

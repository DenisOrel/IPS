// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetCache
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Interfaces;
using Intermech.Signs.Interfaces;
using System;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Кэш различных данных для модуля удостоверяющих листов</summary>
internal class CertSheetCache
{
  /// <summary>Тип объектов Документы</summary>
  public static readonly Guid DocumentObjectTypeGuid = new Guid("cad00070-306c-11d8-b4e9-00304f19f545");
  private static int? documentObjectTypeID = new int?();
  /// <summary>Тип объектов Извещения</summary>
  public static readonly Guid ECOObjectTypeGuid = new Guid("cad00348-306c-11d8-b4e9-00304f19f545");
  private static int? ecoObjectTypeID = new int?();
  /// <summary>Тип объектов Спецификации</summary>
  public static readonly Guid SpecObjectTypeGuid = new Guid("cad00133-306c-11d8-b4e9-00304f19f545");
  private static int? specObjectTypeID = new int?();
  /// <summary>Тип связи "Изменяется по извещению"</summary>
  public static readonly Guid ECORelationTypeGuid = new Guid("cad0036b-306c-11d8-b4e9-00304f19f545");
  private static int? ecoRelationTypeID = new int?();
  /// <summary>Тип связи "Состав изделия"</summary>
  public static readonly Guid CompositionRelationTypeGuid = new Guid("cad00023-306c-11d8-b4e9-00304f19f545");
  private static int? compositionRelationTypeID = new int?();
  /// <summary>Тип связи "Документация на изделие"</summary>
  public static readonly Guid DocumentationRelationTypeGuid = new Guid("cad00154-306c-11d8-b4e9-00304f19f545");
  private static int? documentationRelationTypeID = new int?();
  /// <summary>Тип связи "Подписи"</summary>
  public static readonly Guid SignsRelationTypeGuid = SignsHolder.SignRelationTypeGuid;
  private static int? signsRelationTypeID = new int?();

  /// <summary>Идентификатор типа объектов Документы</summary>
  public static int DocumentObjectTypeID
  {
    get
    {
      if (!CertSheetCache.documentObjectTypeID.HasValue)
        CertSheetCache.documentObjectTypeID = new int?(MetaDataHelper.GetObjectType(CertSheetCache.DocumentObjectTypeGuid).ObjectTypeID);
      return CertSheetCache.documentObjectTypeID.Value;
    }
  }

  /// <summary>Идентификатор типа объектов Извещения</summary>
  public static int ECOObjectTypeID
  {
    get
    {
      if (!CertSheetCache.ecoObjectTypeID.HasValue)
        CertSheetCache.ecoObjectTypeID = new int?(MetaDataHelper.GetObjectType(CertSheetCache.ECOObjectTypeGuid).ObjectTypeID);
      return CertSheetCache.ecoObjectTypeID.Value;
    }
  }

  /// <summary>Идентификатор типа объектов Спецификации</summary>
  public static int SpecObjectTypeID
  {
    get
    {
      if (!CertSheetCache.specObjectTypeID.HasValue)
        CertSheetCache.specObjectTypeID = new int?(MetaDataHelper.GetObjectType(CertSheetCache.SpecObjectTypeGuid).ObjectTypeID);
      return CertSheetCache.specObjectTypeID.Value;
    }
  }

  /// <summary>Идентификатор типа связи "Изменяется по извещению"</summary>
  public static int ECORelationTypeID
  {
    get
    {
      if (!CertSheetCache.ecoRelationTypeID.HasValue)
        CertSheetCache.ecoRelationTypeID = new int?(MetaDataHelper.GetRelationType(CertSheetCache.ECORelationTypeGuid).RelationTypeID);
      return CertSheetCache.ecoRelationTypeID.Value;
    }
  }

  /// <summary>Идентификатор типа связи "Состав изделия"</summary>
  public static int CompositionRelationTypeID
  {
    get
    {
      if (!CertSheetCache.compositionRelationTypeID.HasValue)
        CertSheetCache.compositionRelationTypeID = new int?(MetaDataHelper.GetRelationType(CertSheetCache.CompositionRelationTypeGuid).RelationTypeID);
      return CertSheetCache.compositionRelationTypeID.Value;
    }
  }

  /// <summary>Идентификатор типа связи "Документация на изделие"</summary>
  public static int DocumentationRelationTypeID
  {
    get
    {
      if (!CertSheetCache.documentationRelationTypeID.HasValue)
        CertSheetCache.documentationRelationTypeID = new int?(MetaDataHelper.GetRelationType(CertSheetCache.DocumentationRelationTypeGuid).RelationTypeID);
      return CertSheetCache.documentationRelationTypeID.Value;
    }
  }

  /// <summary>Идентификатор типа связи "Подписи"</summary>
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

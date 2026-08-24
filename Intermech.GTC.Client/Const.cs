// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.Const
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.GTC.Client;

internal class Const
{
  public static readonly string StartPageName = "StartPage";
  public static readonly string EndPageName = "EndPage";
  public static readonly string IconName = "GtcToolIcon";
  public static readonly string GtcViewName = "GtcItemView";
  public static readonly string BsuCode = "BSU_CODE";
  public static readonly string PluginName = "Клиентская часть GTC";
  public static int AttrsRelationshipTypeAttributeTypeId;
  public static readonly Guid AttrsRelationshipTypeAttributeTypeGuid = new Guid("cadd989e-306c-11d8-b4e9-00304f19f545");
  public static int ClassAttrTypeAttributeTypeId;
  public static readonly Guid ClassAttrTypeAttributeTypeGuid = new Guid("cadd989d-306c-11d8-b4e9-00304f19f545");
  public static int CatalogTypeAttributeTypeId;
  public static readonly Guid CatalogTypeAttributeTypeGuid = new Guid("cad00200-306c-11d8-b4e9-00304f19f545");
  public static int ClassifFolderKeyAttributeTypeId;
  public static readonly Guid ClassifFolderKeyAttributeTypeGuid = new Guid("cad0014d-306c-11d8-b4e9-00304f19f545");
  public static int BaseItemObjectTypeId;
  public static readonly Guid BaseItemObjectTypeGuid = new Guid("cadd96ca-306c-11d8-b4e9-00304f19f545");
  public static int GtcToolObjectTypeId;
  public static readonly Guid GtcToolObjectTypeGuid = new Guid("cadd9722-306c-11d8-b4e9-00304f19f545");
  public static int ImbaseCatalogObjectTypeId;
  public static readonly Guid ImbaseCatalogObjectTypeGuid = new Guid("cad00221-306c-11d8-b4e9-00304f19f545");
  public static int ImbaseFolderObjectTypeId;
  public static readonly Guid ImbaseFolderObjectTypeGuid = new Guid("cad00222-306c-11d8-b4e9-00304f19f545");
  public static int AdaptiveItemObjectTypeId;
  public static readonly Guid AdaptiveItemTypeGuid = new Guid("cadd96e6-306c-11d8-b4e9-00304f19f545");
  public static int ToolItemObjectTypeId;
  public static readonly Guid ToolItemTypeGuid = new Guid("cadd96e7-306c-11d8-b4e9-00304f19f545");
  public static int CuttingItemObjectTypeId;
  public static readonly Guid CuttingItemTypeGuid = new Guid("cadd96e9-306c-11d8-b4e9-00304f19f545");

  static Const()
  {
    Const.AttrsRelationshipTypeAttributeTypeId = MetaDataHelper.GetAttributeTypeID(Const.AttrsRelationshipTypeAttributeTypeGuid);
    Const.ClassAttrTypeAttributeTypeId = MetaDataHelper.GetAttributeTypeID(Const.ClassAttrTypeAttributeTypeGuid);
    Const.CatalogTypeAttributeTypeId = MetaDataHelper.GetAttributeTypeID(Const.CatalogTypeAttributeTypeGuid);
    Const.ClassifFolderKeyAttributeTypeId = MetaDataHelper.GetAttributeTypeID(Const.ClassifFolderKeyAttributeTypeGuid);
    Const.BaseItemObjectTypeId = MetaDataHelper.GetObjectTypeID(Const.BaseItemObjectTypeGuid);
    Const.GtcToolObjectTypeId = MetaDataHelper.GetObjectTypeID(Const.GtcToolObjectTypeGuid);
    Const.ImbaseCatalogObjectTypeId = MetaDataHelper.GetObjectTypeID(Const.ImbaseCatalogObjectTypeGuid);
    Const.ImbaseFolderObjectTypeId = MetaDataHelper.GetObjectTypeID(Const.ImbaseFolderObjectTypeGuid);
    Const.AdaptiveItemObjectTypeId = MetaDataHelper.GetObjectTypeID(Const.AdaptiveItemTypeGuid);
    Const.ToolItemObjectTypeId = MetaDataHelper.GetObjectTypeID(Const.ToolItemTypeGuid);
    Const.CuttingItemObjectTypeId = MetaDataHelper.GetObjectTypeID(Const.CuttingItemTypeGuid);
  }
}

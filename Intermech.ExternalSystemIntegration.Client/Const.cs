// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.Const
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class Const
{
  public static readonly string RequestCommandName = "CreateRequest";
  public static readonly string RequestCommandImage = "CreateRequestIcon";
  public static readonly string RequestConfigIconName = "RequestConfigIcon";
  public static readonly string RequestConfigTabName = "Конфигурация запроса";
  public static readonly string ResponceConfigIconName = "ResponceConfigIcon";
  public static readonly string RequestSchemeIconName = "RequestSchemeIcon";
  public static readonly string RequestSchemeTabName = "Схема исходящего запроса";
  public static readonly string ResponceSchemeIconName = "RequestSchemeIcon";
  public static readonly string ResponceSchemeTabName = "Схема входящего запроса";
  public static readonly string ObjectTypeSettingItemIconName = "ObjectTypeSettingItem";
  public static readonly string ObjectTypeSettingItemTabName = "Настройка типа объекта";
  public static readonly string CommonSettingsName = "Общие настройки";
  public static int ObjectTypeIDAttrTypeID = 0;
  public static readonly Guid ObjectTypeIDAttrTypeGUID = new Guid("cad001a0-306c-11d8-b4e9-00304f19f545");
  public static int LinkObjectAttrTypeID = 0;
  public static readonly Guid LinkObjectAttrTypeGuid = new Guid("cad0156a-306c-11d8-b4e9-00304f19f545");
  public static int TransfSchemeLinkAttrTypeID = 0;
  public static readonly Guid TransfSchemeLinkAttrTypeGUID = new Guid("cadd958b-306c-11d8-b4e9-00304f19f545");
  public static int RequestSchemeLinkAttrTypeID = 0;
  public static readonly Guid RequestSchemeLinkAttrTypeGUID = new Guid("cadd95b1-306c-11d8-b4e9-00304f19f545");
  public static int ResponceSchemeLinkAttrTypeID = 0;
  public static readonly Guid ResponceSchemeLinkAttrTypeGUID = new Guid("cadd95b0-306c-11d8-b4e9-00304f19f545");
  public static int NameAttrTypeID = 0;
  public static readonly Guid NameAttrTypeGuid = new Guid("cad00020-306c-11d8-b4e9-00304f19f545");
  public static int ResponceObjTypeID = -1;
  public static readonly Guid ResponceObjTypeGuid = new Guid("cadd9536-306c-11d8-b4e9-00304f19f545");
  public static int RequestObjTypeID = -1;
  public static readonly Guid RequestObjTypeGuid = new Guid("cadd9534-306c-11d8-b4e9-00304f19f545");
  public static int ResponceConfigObjTypeID = -1;
  public static readonly Guid ResponceConfigObjTypeGUID = new Guid("cadd958f-306c-11d8-b4e9-00304f19f545");
  public static int RequestConfigObjTypeID = -1;
  public static readonly Guid RequestConfigObjTypeGUID = new Guid("cadd9590-306c-11d8-b4e9-00304f19f545");
  public static int TypeSettingItemObjTypeID = -1;
  public static readonly Guid TypeSettingItemObjTypeGuid = new Guid("cadd958e-306c-11d8-b4e9-00304f19f545");
  public static int ResponceSchemeObjTypeID = -1;
  public static readonly Guid ResponceSchemeObjTypeGuid = new Guid("cadd956a-306c-11d8-b4e9-00304f19f545");
  public static int RequestSchemeObjTypeID = -1;
  public static readonly Guid RequestSchemeObjTypeGuid = new Guid("cadd956b-306c-11d8-b4e9-00304f19f545");

  static Const()
  {
    Const.ObjectTypeIDAttrTypeID = MetaDataHelper.GetAttributeTypeID(Const.ObjectTypeIDAttrTypeGUID);
    Const.LinkObjectAttrTypeID = MetaDataHelper.GetAttributeTypeID(Const.LinkObjectAttrTypeGuid);
    Const.TransfSchemeLinkAttrTypeID = MetaDataHelper.GetAttributeTypeID(Const.TransfSchemeLinkAttrTypeGUID);
    Const.RequestSchemeLinkAttrTypeID = MetaDataHelper.GetAttributeTypeID(Const.RequestSchemeLinkAttrTypeGUID);
    Const.ResponceSchemeLinkAttrTypeID = MetaDataHelper.GetAttributeTypeID(Const.ResponceSchemeLinkAttrTypeGUID);
    Const.NameAttrTypeID = MetaDataHelper.GetAttributeTypeID(Const.NameAttrTypeGuid);
    Const.ResponceObjTypeID = MetaDataHelper.GetObjectTypeID(Const.ResponceObjTypeGuid);
    Const.RequestObjTypeID = MetaDataHelper.GetObjectTypeID(Const.RequestObjTypeGuid);
    Const.ResponceSchemeObjTypeID = MetaDataHelper.GetObjectTypeID(Const.ResponceSchemeObjTypeGuid);
    Const.RequestSchemeObjTypeID = MetaDataHelper.GetObjectTypeID(Const.RequestSchemeObjTypeGuid);
    Const.TypeSettingItemObjTypeID = MetaDataHelper.GetObjectTypeID(Const.TypeSettingItemObjTypeGuid);
    Const.RequestConfigObjTypeID = MetaDataHelper.GetObjectTypeID(Const.RequestConfigObjTypeGUID);
    Const.ResponceConfigObjTypeID = MetaDataHelper.GetObjectTypeID(Const.ResponceConfigObjTypeGUID);
  }
}

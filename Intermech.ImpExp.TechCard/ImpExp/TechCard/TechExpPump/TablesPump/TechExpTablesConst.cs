// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.TablesPump.TechExpTablesConst
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.Interfaces.Client;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.TablesPump;

internal static class TechExpTablesConst
{
  private static readonly Guid DbFolderObjectTypeGuid = new Guid("cadd9715-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid DbTableObjectTypeGuid = new Guid("cad00102-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid DbEntriesAttributeTypeGuid = new Guid("cad00069-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid DbDataAttributeTypeGuid = new Guid("cad00065-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid DbNameAttributeTypeGuid = new Guid("cad00060-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid DbLayersAttributeTypeGuid = new Guid("cad0006c-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid DbColumnsAttributeTypeGuid = new Guid("cad0006b-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid DbRowsAttributeTypeGuid = new Guid("cad0006a-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid DbRolesAttributeTypeGuid = new Guid("cad0006d-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid DbAttrTypesListAttributeTypeGuid = new Guid("cad00061-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid DbObjTypesListAttributeTypeGuid = new Guid("cad00062-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid DbObjLinksListAttributeTypeGuid = new Guid("cad00063-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid DbConditionAttributeTypeGuid = new Guid("cad00064-306c-11d8-b4e9-00304f19f545");
  public static int DBFolderObjectTypeID = -1;
  public static int DBTableObjectTypeID = -1;
  public static int DBEntriesAttributeTypeID = -1;
  public static int DBDataAttributeTypeID = -1;
  public static int DBBigDataAttributeTypeID = -1;
  public static int DBNameAttributeTypeID = -1;
  public static int DBLayersAttributeTypeID = -1;
  public static int DBColumnsAttributeTypeID = -1;
  public static int DBRowsAttributeTypeID = -1;
  public static int DBRolesAttributeTypeID = -1;
  public static int DBAttrTypesListAttributeTypeID = -1;
  public static int DBObjTypesListAttributeTypeID = -1;
  public static int DBObjLinksListAttributeTypeID = -1;
  public static int DBConditionAttributeTypeID = -1;

  public static void Initialize()
  {
    IMetadataInfo service = ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    TechExpTablesConst.DBFolderObjectTypeID = service.ObjectTypes.GetByGuid(TechExpTablesConst.DbFolderObjectTypeGuid).ID;
    TechExpTablesConst.DBTableObjectTypeID = service.ObjectTypes.GetByGuid(TechExpTablesConst.DbTableObjectTypeGuid).ID;
    TechExpTablesConst.DBEntriesAttributeTypeID = service.AttributeTypes.GetByGuid(TechExpTablesConst.DbEntriesAttributeTypeGuid).ID;
    TechExpTablesConst.DBDataAttributeTypeID = service.AttributeTypes.GetByGuid(TechExpTablesConst.DbDataAttributeTypeGuid).ID;
    TechExpTablesConst.DBBigDataAttributeTypeID = service.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atExpertBigDataAttrGuid).ID;
    TechExpTablesConst.DBNameAttributeTypeID = service.AttributeTypes.GetByGuid(TechExpTablesConst.DbNameAttributeTypeGuid).ID;
    TechExpTablesConst.DBLayersAttributeTypeID = service.AttributeTypes.GetByGuid(TechExpTablesConst.DbLayersAttributeTypeGuid).ID;
    TechExpTablesConst.DBColumnsAttributeTypeID = service.AttributeTypes.GetByGuid(TechExpTablesConst.DbColumnsAttributeTypeGuid).ID;
    TechExpTablesConst.DBRowsAttributeTypeID = service.AttributeTypes.GetByGuid(TechExpTablesConst.DbRowsAttributeTypeGuid).ID;
    TechExpTablesConst.DBRolesAttributeTypeID = service.AttributeTypes.GetByGuid(TechExpTablesConst.DbRolesAttributeTypeGuid).ID;
    TechExpTablesConst.DBAttrTypesListAttributeTypeID = service.AttributeTypes.GetByGuid(TechExpTablesConst.DbAttrTypesListAttributeTypeGuid).ID;
    TechExpTablesConst.DBObjTypesListAttributeTypeID = service.AttributeTypes.GetByGuid(TechExpTablesConst.DbObjTypesListAttributeTypeGuid).ID;
    TechExpTablesConst.DBObjLinksListAttributeTypeID = service.AttributeTypes.GetByGuid(TechExpTablesConst.DbObjLinksListAttributeTypeGuid).ID;
    TechExpTablesConst.DBConditionAttributeTypeID = service.AttributeTypes.GetByGuid(TechExpTablesConst.DbConditionAttributeTypeGuid).ID;
  }
}

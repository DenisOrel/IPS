// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ImbaseIDHelper
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using System;

#nullable disable
namespace Intermech.ImpExp.Imbase;

public static class ImbaseIDHelper
{
  public static int ObjTypeIdImCtl = -1;
  public static int ObjTypeIdImObject = -1;
  public static int ObjTypeIdImFolder = -1;
  public static int ObjTypeIdImCtlRec = -1;
  public static int ObjTypeIdImTab = -1;
  public static int ObjTypeIdImTabMixData = -1;
  public static int ObjTypeIdImTabLink = -1;
  public static int ObjTypeIdImLibImage = -1;
  public static int ObjTypeIdImageFolder = -1;
  public static int ObjTypeIdImTemplate = -1;
  public static int ObjTypeIdBloвImbase = -1;
  public static long AttrTableDataLength = 0;
  public static int AttrTableData = 0;
  public static int AttrLongTableData = 0;
  public static int AttrIdImCode = 0;
  public static int AttrIdImObjCopy = 0;
  public static int AttrIdImSort = 0;
  public static int AttrIdImListAtr = 0;
  public static int AttrIdImLinkCtlRec = 0;
  public static int AttrIdImLinkTabRec = 0;
  public static int AttrIdImLinkCtl = 0;
  public static int AttrIdImLinkObj = 0;
  public static int AttrIdLinkFolder = 0;
  public static int AttrIdImLinkTable = 0;
  public static int AttrIdImLinkTabLink = 0;
  public static int AttrIdImTypeTabRec = 0;
  public static int AttrIdImTypeCtl = 0;
  public static int AttrIdImTypeCreatedObj = 0;
  public static int AttrIdName = 0;
  public static int AttrIdSizeAndParm = 0;
  public static int AttrIdCaption = 0;
  public static int AttrIdDescription = 0;
  public static int AttrIdPicture = 0;
  public static int AttrIdLibraryImage = 0;
  public static int AttrIdTableName = 0;
  public static int AttrIdTemplateData = 0;
  public static int AttrIdTemplateRef = 0;
  public static int AttrIdNeedHandle = 0;
  public static int AttrIdClassifierKey = 0;
  public static int AttrComentText = 0;
  public static int AttrFlags = 0;
  public static int AttrVisibility = 0;
  public static readonly Guid ImbaseTemplateTypeGUID = new Guid("cad00228-306c-11d8-b4e9-00304f19f545");
  public static int RelTypeIDImSimple = -1;
  public static int RelTypeIDImSorted = -1;

  public static void Initialize(IMetadataInfo mi)
  {
    try
    {
      ImbaseIDHelper.ObjTypeIdImCtl = mi.ObjectTypes.GetByGuid(new Guid("cad00221-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.ObjTypeIdImFolder = mi.ObjectTypes.GetByGuid(new Guid("cad00222-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.ObjTypeIdImCtlRec = mi.ObjectTypes.GetByGuid(new Guid("cad00223-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.ObjTypeIdImTab = mi.ObjectTypes.GetByGuid(new Guid("cad00224-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.ObjTypeIdImTabLink = mi.ObjectTypes.GetByGuid(new Guid("cad00227-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.ObjTypeIdImLibImage = mi.ObjectTypes.GetByGuid(new Guid("cad00140-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.ObjTypeIdImageFolder = mi.ObjectTypes.GetByGuid(new Guid("cad0013f-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.ObjTypeIdImTemplate = mi.ObjectTypes.GetByGuid(new Guid("cad00228-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.ObjTypeIdBloвImbase = mi.ObjectTypes.GetByGuid(new Guid("cadd9693-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.ObjTypeIdImObject = mi.ObjectTypes.GetByGuid(new Guid("cad00220-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.ObjTypeIdImTabMixData = mi.ObjectTypes.GetByGuid(Intermech.Imbase.Consts.ImbaseTableMixTypeGUID).ID;
      IAttributeTypeItem byGuid = mi.AttributeTypes.GetByGuid(new Guid("cad00215-306c-11d8-b4e9-00304f19f545"));
      ImbaseIDHelper.AttrTableData = byGuid.ID;
      ImbaseIDHelper.AttrTableDataLength = (long) byGuid.MaxSize;
      ImbaseIDHelper.AttrLongTableData = mi.AttributeTypes.GetByGuid(new Guid("cad001b2-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdImCode = mi.AttributeTypes.GetByGuid(new Guid("cad0020f-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdImObjCopy = mi.AttributeTypes.GetByGuid(new Guid("cad00204-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdImSort = mi.AttributeTypes.GetByGuid(new Guid("cad00202-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdImListAtr = mi.AttributeTypes.GetByGuid(new Guid("cad0020c-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdImLinkCtlRec = mi.AttributeTypes.GetByGuid(new Guid("cad00206-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdImLinkTabRec = mi.AttributeTypes.GetByGuid(new Guid("cad00205-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdImLinkCtl = mi.AttributeTypes.GetByGuid(new Guid("cad00207-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdImLinkObj = mi.AttributeTypes.GetByGuid(new Guid("cad00209-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdLinkFolder = mi.AttributeTypes.GetByGuid(new Guid("cad00208-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdImLinkTable = mi.AttributeTypes.GetByGuid(new Guid("cad0020b-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdImLinkTabLink = mi.AttributeTypes.GetByGuid(new Guid("cad0020a-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdImTypeTabRec = mi.AttributeTypes.GetByGuid(new Guid("cad0020d-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdImTypeCtl = mi.AttributeTypes.GetByGuid(new Guid("cad00200-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdImTypeCreatedObj = mi.AttributeTypes.GetByGuid(new Guid("cad00203-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdName = mi.AttributeTypes.GetByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdSizeAndParm = mi.AttributeTypes.GetByGuid(new Guid("cad00211-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdCaption = mi.AttributeTypes.GetByGuid(new Guid("cad00047-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdDescription = mi.AttributeTypes.GetByGuid(new Guid("cad0001c-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdPicture = mi.AttributeTypes.GetByGuid(new Guid("cad0013e-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdLibraryImage = mi.AttributeTypes.GetByGuid(new Guid("cad0013d-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdTableName = mi.AttributeTypes.GetByGuid(new Guid("cad0020e-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdTemplateData = mi.AttributeTypes.GetByGuid(new Guid("cad00212-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdTemplateRef = mi.AttributeTypes.GetByGuid(new Guid("cad00213-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdNeedHandle = mi.AttributeTypes.GetByGuid(new Guid("cad00354-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrIdClassifierKey = mi.AttributeTypes.GetByGuid(new Guid("cad0014d-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrComentText = mi.AttributeTypes.GetByGuid(new Guid("cadd9691-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrFlags = mi.AttributeTypes.GetByGuid(new Guid("cad00072-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.AttrVisibility = mi.AttributeTypes.GetByGuid(new Guid("cad0062f-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.RelTypeIDImSimple = mi.RelationTypes.GetByGuid(new Guid("cad00022-306c-11d8-b4e9-00304f19f545")).ID;
      ImbaseIDHelper.RelTypeIDImSorted = mi.RelationTypes.GetByGuid(new Guid("cad00151-306c-11d8-b4e9-00304f19f545")).ID;
    }
    catch (Exception ex)
    {
      throw new Exception($"Ошибка при инициализации метаданных: {ex.Message}. Проверьте необходимые системные метаданные.");
    }
  }
}

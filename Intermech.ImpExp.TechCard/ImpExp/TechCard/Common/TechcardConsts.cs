// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechcardConsts
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ECO.Client;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Techcard;
using Intermech.ImpExp.TechCard.Pumpers;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

public class TechcardConsts : ITechcardCommon
{
  public static PluginClass Plugin;
  public static ITechcardCommon TechcardCommon;
  internal static TechConnectionsManager ConnectionManager;

  Dictionary<string, Guid> ITechcardCommon.Code2AttributeGuid
  {
    get => TechPumpData.Entities.Code2AttributeGuid;
    set => TechPumpData.Entities.Code2AttributeGuid = value;
  }

  public class TableTypesConst
  {
    public static string CtlType = "CATALOG";
    public static string RecType = "CTLREC";
    public static string CtlRefType = "CTLREF";
    public static string TCRefType = "TCREF";
    public static string TblRefType = "TBLREF";
    public static string TableType = "TABLE";
    public static string IndexType = "INDEX";
  }

  public class TechCacheConsts
  {
    public static ImportingCategory[] GetAllTechCategory()
    {
      return new ImportingCategory[21]
      {
        ImportingCategory.TechArea,
        ImportingCategory.TechArticlesPump,
        ImportingCategory.TechCeh,
        ImportingCategory.TechCehZahodPump,
        ImportingCategory.TechComment,
        ImportingCategory.TechManufacturingRouting,
        ImportingCategory.TechMatGrPump,
        ImportingCategory.TechMatPump,
        ImportingCategory.TechOperation,
        ImportingCategory.TechOutfitPump,
        ImportingCategory.TechPerehPump,
        ImportingCategory.TechPersonalPump,
        ImportingCategory.TechProcessPump,
        ImportingCategory.TechRezPump,
        ImportingCategory.TechRoute,
        ImportingCategory.TechRouteElem,
        ImportingCategory.TechRouteTemplate,
        ImportingCategory.TechToolsPump,
        ImportingCategory.TechWorkTypes,
        ImportingCategory.TechZagot,
        ImportingCategory.TechOsnPos
      };
    }

    public static ImportingCategory[] GetAllSEARCHCategory()
    {
      return new ImportingCategory[3]
      {
        ImportingCategory.UserGroups,
        ImportingCategory.Users,
        ImportingCategory.UsersToGroups
      };
    }

    public static ImportingCategory[] GetAllIMBASECategory()
    {
      return new ImportingCategory[11]
      {
        ImportingCategory.ImbaseBlobs,
        ImportingCategory.ImbaseCatalogBinding,
        ImportingCategory.ImbaseCatalogBindingType,
        ImportingCategory.ImbaseCatalogs,
        ImportingCategory.ImbaseFolders,
        ImportingCategory.ImbaseGroups,
        ImportingCategory.ImbaseGroupsAttributes,
        ImportingCategory.ImbaseMaterials,
        ImportingCategory.ImbaseTables,
        ImportingCategory.ImbaseTablesAttributes,
        ImportingCategory.ImbaseTablesInCatalogs
      };
    }

    public static ImportingCategory[] GetTPTechCategory()
    {
      return new ImportingCategory[18]
      {
        ImportingCategory.TechCehZahodPump,
        ImportingCategory.TechComment,
        ImportingCategory.TechManufacturingRouting,
        ImportingCategory.TechMatGrPump,
        ImportingCategory.TechMatPump,
        ImportingCategory.TechOperation,
        ImportingCategory.TechOutfitPump,
        ImportingCategory.TechPerehPump,
        ImportingCategory.TechPersonalPump,
        ImportingCategory.TechProcessPump,
        ImportingCategory.TechRezPump,
        ImportingCategory.TechRoute,
        ImportingCategory.TechRouteElem,
        ImportingCategory.TechRouteTemplate,
        ImportingCategory.TechToolsPump,
        ImportingCategory.TechZagot,
        ImportingCategory.Articles,
        ImportingCategory.TechOsnPos
      };
    }

    public static ImportingCategory GetImportingCategoryByRecordTypeId(int recordTypeId)
    {
      ImportingCategory categoryByRecordTypeId = ImportingCategory.None;
      switch (recordTypeId)
      {
        case 1:
          categoryByRecordTypeId = ImportingCategory.TechOperation;
          break;
        case 2:
          categoryByRecordTypeId = ImportingCategory.TechOutfitPump;
          break;
        case 3:
          categoryByRecordTypeId = ImportingCategory.TechManufacturingRouting;
          break;
        case 5:
          categoryByRecordTypeId = ImportingCategory.TechAddMovement;
          break;
        case 7:
          categoryByRecordTypeId = ImportingCategory.TechRouteTemplate;
          break;
        case 8:
        case 15:
        case 21:
          categoryByRecordTypeId = ImportingCategory.TechProcessPump;
          break;
        case 10:
          categoryByRecordTypeId = ImportingCategory.TechComment;
          break;
        case 11:
          categoryByRecordTypeId = ImportingCategory.TechOsnPos;
          break;
        case 12:
          categoryByRecordTypeId = ImportingCategory.TechMatPump;
          break;
        case 13:
          categoryByRecordTypeId = ImportingCategory.TechArticlesPump;
          break;
        case 14:
          categoryByRecordTypeId = ImportingCategory.TechPerehPump;
          break;
        case 16 /*0x10*/:
          categoryByRecordTypeId = ImportingCategory.TechRezPump;
          break;
        case 18:
          categoryByRecordTypeId = ImportingCategory.TechToolsPump;
          break;
        case 20:
          categoryByRecordTypeId = ImportingCategory.TechTPOverpatching;
          break;
        case 22:
          categoryByRecordTypeId = ImportingCategory.TechRouteElem;
          break;
        case 23:
          categoryByRecordTypeId = ImportingCategory.TechZagot;
          break;
        case 24:
          categoryByRecordTypeId = ImportingCategory.TechMatGrPump;
          break;
      }
      return categoryByRecordTypeId;
    }
  }

  internal enum imTablesConsts
  {
    Unknown,
    VidZagot,
    VidIzd,
    OsnMat,
    Sortament,
    Operations,
    Oborud,
    Perehod,
    Tool,
    Ceh,
    Works,
    ProductTools,
    Surfaces,
    Precision,
    PrecisionCut,
    SupportMater,
    TypeTPProcess,
    Comments,
    WorkType,
    SupportMaterDict,
    SpecialWorks,
  }

  public class TypeConsts
  {
    public static readonly Guid otWorksVidObjTypeGuid = new Guid("cad005af-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otProductionObjTypeGuid = new Guid("cad005ae-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otZagotGUID = new Guid("cad001da-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otDopPrGUID = new Guid("cad00164-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otEdSostArt = new Guid("cad00165-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otKomplArt = new Guid("cad00166-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otSobArt = new Guid("cad00167-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otRouteObjTypeGuid = new Guid("cad001e5-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otRoutesTemplatesObjTypeGuid = new Guid("cad001fd-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otRoutesTemplIzgotObjTypeGuid = new Guid("cad001e6-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otRoutesTemplSborkaObjTypeGuid = new Guid("cad001e7-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTechRouteElemObjTypeGuid = new Guid("cad001e8-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTechCehZahodObjTypeGuid = new Guid("cad001ff-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTechCehAreaObjTypeGuid = new Guid("cad001b6-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTechCehObjTypeGuid = new Guid("cad001b7-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTechAreaObjTypeGuid = new Guid("cad001b8-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTechTPBaseObjTypeGuid = new Guid("cad00185-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTechTPGroupObjTypeGuid = new Guid("cad00186-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTechTPOneObjTypeGuid = new Guid("cad00187-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTechTPTypeObjTypeGuid = new Guid("cad00188-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTechInstrumentObjTypeGuid = new Guid("cad0017a-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTechRoute2AtrObjectTypeGuid = new Guid("cad0016f-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otProcRoutingEntryGUID = new Guid("cadd9bbb-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otScenarioObjTypeGuid = new Guid("cad0011c-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otOperationObjTypeGuid = new Guid("cad00178-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTechDocumentsObjTypeGuid = new Guid("cad009ec-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otPersonalObjTypeGuid = new Guid("cad00180-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otCommentsObjTypeGuid = new Guid("cad00168-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otInstrumentalPositionObjTypeGuid = new Guid("cad0017b-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otMaterialsObjTypeGuid = new Guid("cad00172-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTechPerehodObjTypeGuid = new Guid("cad0017d-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otConditionsObjTypeGuid = new Guid("cad00184-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otProductObjTypeGuid = new Guid("cad00268-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otRiggingObjTypeGuid = new Guid("cad0017c-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTPModificationObjTypeGuid = new Guid("cad0134b-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otInstrumentationObjTypeGuid = new Guid("cad00177-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTechobjectObjTypeGuid = new Guid("cad00163-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otInstrumentationModelObjTypeGuid = new Guid("cad001ac-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otChangeAtNotificationObjTypeGuid = new Guid("cad00349-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otAutoSelectionTreeObjTypeGuid = new Guid("cad001b0-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otArticleTypes = new Guid("cad00835-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otPerehNorm = new Guid("cad005c3-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otOperNorm = new Guid("cad005c2-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otTPNorm = new Guid("cad005c4-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otVidZag = new Guid("cad00834-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otStandartItemGuid = new Guid("cad00252-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otDraftGuid = new Guid("cad00195-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otDraftCadmechTGuid = new Guid("cad005bd-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otDraftOLEGuid = new Guid("cad005bc-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otToolRequestGuid = new Guid("cadd951e-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid otSignGuid = new Guid("cad00137-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid rtTechRelationGuid = new Guid("cad0019f-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid rtTechCollectRelationGuid = new Guid("cad0019e-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid rtTechGTPRelationGuid = new Guid("cad005b8-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid rtChangeAtNotificationRelationGuid = new Guid("cad0036b-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid rtSignGuid = new Guid("cad00139-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atNaimAttrTypeGuid = new Guid("cad00020-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atShortNaimAttrTypeGuid = new Guid("cad00005-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atObozAttrTypeGuid = new Guid("cad0001f-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atProductionAttrTypeGuid = new Guid("cad0019c-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atGuidOTsAttrTypeGuid = new Guid("cad00149-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atTechTypeKeyAttrGuid = new Guid("cad005bf-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atObjectTypeAttrGuid = new Guid("cad0002e-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atZSearchObjectAttrGuid = new Guid("cad009ef-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atImbaseObjectAttrGuid = new Guid("cad00209-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atObjectLinktAttrGuid = new Guid("cad001be-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atPartObjectAttrGuid = new Guid("cad005d1-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atCehAttrGuid = new Guid("cad001fb-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atCodWorkTypeAttrGuid = new Guid("cad005dc-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atShortNameWorkVidAttrGuid = new Guid("cad005d2-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atLastLevelSeek = new Guid("cad001bb-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atAreaAttrGuid = new Guid("cad001fc-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atLA_SortAttrGuid = new Guid("cad00202-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atMemberOfSborkaObjectAttrGUID = new Guid("cad001d5-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atMemberOfZakazObjectAttrGUID = new Guid("cad001d6-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atMemberOfExitAssemblyAttrGUID = new Guid("cadd9bca-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atProcRouteDefaultAttrGuid = new Guid("cad005b9-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atTechRouteVidAttrGuid = new Guid("cad001ed-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atTechRouteNaznAttrGuid = new Guid("cad001ec-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atTechRouteTipAttrGuid = new Guid("cad001eb-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atImbaseKeyAttrGuid = new Guid("cad00162-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atTechObjectName = new Guid("cad005ea-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atCommentTextAtrGuid = new Guid("cad005d4-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atTechArtAtrGuid = new Guid("cad0134a-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atTechInstrumPosNomGuid = new Guid("cad005e6-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atFormAttrTypeGuid = new Guid("cad0011d-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atObjectMissionAddAtrTypeGuid = new Guid("cad007a3-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atFlagsTypeGuid = new Guid("cad00072-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atDellObjectIfDellIzwTypeGuid = new Guid(RevReqHelper.guidAttrDelWhenExcluded);
    public static readonly Guid atGlobalObjectTypeGuid = new Guid("cad001a0-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atGlobalAttributeTypeGuid = new Guid("cad001d0-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atFormListAttributeTypeGuid = new Guid("cad0019d-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atImageLinkGuid = new Guid("cad014b6-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atConditionGuid = new Guid("cad00064-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atVidZagAttrTypeGuid = new Guid("cad005d0-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid atDraftOLEObjectGuid = new Guid("cad005be-306c-11d8-b4e9-00304f19f545");
    public static Guid atExpertBigDataAttrGuid = new Guid("cad01523-306C-11D8-B4E9-00304F19F545");
    public static Guid atTechProcGroupRelAttrGUID = new Guid("cad009ee-306c-11d8-b4e9-00304f19f545");
    public static Guid atGtpContextAttrGUID = new Guid("cadd93fb-306c-11d8-b4e9-00304f19f545");
    public static Guid atTechCehCodeAttrGuid = new Guid("cad009e2-306c-11d8-b4e9-00304f19f545");
    public static Guid atTechWorkAreaCodeAttrGuid = new Guid("cad009e1-306c-11d8-b4e9-00304f19f545");
    public static Guid atNonNumerateFlagAttrGuid = new Guid("cadd9710-306c-11d8-b4e9-00304f19f545");
    public static Guid atFileAttrGuid = new Guid("cad0004b-306c-11d8-b4e9-00304f19f545");
    public static Guid atOleDataAttrGuid = new Guid("cad005be-306c-11d8-b4e9-00304f19f545");
    public static Guid atBasicTpAttrGuid = new Guid("cadd9c39-306c-11d8-b4e9-00304f19f545");
    public static readonly Guid gaImportedTechAtributeGroupGuid = new Guid("cad009ee-306c-11d8-b4e9-00304f19f545");
  }

  public enum TpRecordType
  {
    Unknown = 0,
    Oper = 1,
    Oborud = 2,
    RouteVariant = 3,
    Document = 4,
    DopPriem = 5,
    Surface = 6,
    RouteTemplate = 7,
    GenaralInfo = 8,
    Personal = 9,
    Comments = 10, // 0x0000000A
    ToolsPosition = 11, // 0x0000000B
    MaterialAdd = 12, // 0x0000000C
    ArtComposition = 13, // 0x0000000D
    Perehod = 14, // 0x0000000E
    Passport = 15, // 0x0000000F
    Rezhim = 16, // 0x00000010
    Article = 17, // 0x00000011
    Tool = 18, // 0x00000012
    ControlParam = 19, // 0x00000013
    TpChanges = 20, // 0x00000014
    TpType = 21, // 0x00000015
    TemplateElem = 22, // 0x00000016
    MaterialMain = 23, // 0x00000017
    MaterialGroup = 24, // 0x00000018
    KtpEntities = 25, // 0x00000019
    CehFixGroup = 27, // 0x0000001B
    CehFix = 28, // 0x0000001C
    Drafts = 36, // 0x00000024
  }

  public static class Utils
  {
    public static long CodeHashCode(int recTypeId, int recKey)
    {
      return Convert.ToInt64(recTypeId) << 32 /*0x20*/ | Convert.ToInt64(recKey);
    }

    public static int CodeHashCode(short leftKey, short rightKey)
    {
      return Convert.ToInt32(leftKey) << 16 /*0x10*/ | Convert.ToInt32(rightKey);
    }

    public static void DecodeHashCode(long hashCode, out int recTypeId, out int recKey)
    {
      int maxValue = int.MaxValue;
      recKey = Convert.ToInt32(hashCode & (long) maxValue);
      recTypeId = Convert.ToInt32(hashCode >> 32 /*0x20*/);
    }

    public static void DecodeHashCode(int hashCode, out short leftKey, out short rightKey)
    {
      int maxValue = (int) short.MaxValue;
      leftKey = Convert.ToInt16(hashCode & maxValue);
      rightKey = Convert.ToInt16(hashCode >> 16 /*0x10*/);
    }

    public static string GetImbaseKey(int imCatalogKey, int imCatalogRecKey, int imTableRecKey)
    {
      return $"I6{imCatalogKey,6:X}{imCatalogRecKey,6:X}{imTableRecKey,6:X}".Replace(' ', '0');
    }

    public static void DecodeImbaseKey(
      string imbaseKey,
      out int imCatalogKey,
      out int imCatalogRecKey,
      out int imTableRecKey)
    {
      imCatalogKey = int.Parse(imbaseKey.Substring(2, 6), NumberStyles.HexNumber);
      imCatalogRecKey = int.Parse(imbaseKey.Substring(8, 6), NumberStyles.HexNumber);
      imTableRecKey = int.Parse(imbaseKey.Substring(14, 6), NumberStyles.HexNumber);
    }
  }
}

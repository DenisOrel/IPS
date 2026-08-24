// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpHelper
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Intermech.ImpExp.SearchData;

public class PumpHelper : BasePumpHelper
{
  public static int AttrTypeDesignationID = 0;
  public static int AttrTypeNameID = 0;
  public static int AttrTypeVersionCodeID = 0;
  public static int AttrTypeOKPCodeID = 0;
  public static int AttrTypeLiteraID = 0;
  public static int AttrTypeNoteID = 0;
  public static int AttrTypeMassaID = 0;
  public static int AttrTypeGroupInstanceID = 0;
  public static int AttrTypeFileID = 0;
  public static int AttrWorkFileID = 0;
  public static int AttrVerLinkID = 0;
  public static int AttrCountID = 0;
  public static int AttrSPSectionID = 0;
  public static int AttrSPSectionNumID = 0;
  public static int AttrPositionID = 0;
  public static int AttrCompositionContextID = 0;
  public static int AttrSortIndexID = 0;
  public static int AttrOwnerID = 0;
  public static readonly Guid AttrPurchasedGuid = new Guid("cad0038f-306c-11d8-b4e9-00304f19f545");
  public static int AttrPurchasedID = 0;
  public static readonly Guid AttrArchiveGuid = SystemGUIDs.attributeArchive;
  public static int AttrArchiveID = 0;
  public static int AttrFormatID = 0;
  public static int AttrDocWorkPath = 0;
  public static int AttrPaperDocPath = 0;
  public static int AttrContentModifiedDate = 0;
  public static int AttrModificationID = 0;
  public static int AttrAccessLevelID = 0;
  public static int AttrGroupNo = 0;
  public static int AttrSubInGroup = 0;
  public static readonly Guid AttrMaterialGuid = new Guid("cad0038c-306c-11d8-b4e9-00304f19f545");
  public static int AttrMaterialID;
  public static int AttrImbaseKeyID;
  public static int AttrImbaseLinkID;
  public static int AttrImbaseCodeID;
  public static int AttrSerialNoID;
  public static int AttrArtStorageID;
  public static readonly Guid AttrBCHNameGuid = new Guid("cadd938d-306c-11d8-b4e9-00304f19f545");
  public static int AttrBCHNameID;
  public static int AttrLRINList1 = 0;
  public static int AttrLRINList2 = 0;
  public static int AttrLRINList3 = 0;
  public static int AttrLRINList4 = 0;
  public static int AttrLRINList5 = 0;
  public static int AttrLRISoprovDoc = 0;
  public static int AttrLRIDocNo = 0;
  public static int AttrLRIDate = 0;
  public static int AttrLRIPodpis = 0;
  public static int AttrNormID;
  public static int AttrNormNameID;
  public static int AttrEndDateID = 0;
  public static int AttrDateOfReleaseID = 0;
  public static int AttrTermOfChangeID = 0;
  public static int AttrChangeNoID = 0;
  public static int AttrReasonCodeID = 0;
  public static int AttrECO_DateDueID = 0;
  public static int AttrECOLinkID = 0;
  public static int AttrFormNameID;
  public static readonly Guid AttrFormBodyGuid = new Guid("cad0011d-306c-11d8-b4e9-00304f19f545");
  public static int AttrFormBodyID;
  public static int AttrFormObjectTypesID;
  public static int AttrSeriesID;
  public static int AttrBasedOnCadModelID;
  public static readonly Guid AttrSheetsCountGuid = new Guid("cad003a7-306c-11d8-b4e9-00304f19f545");
  public static int AttrSheetsCountID;
  public static readonly Guid RelTypeDocRefGuid = new Guid("cad0057c-306c-11d8-b4e9-00304f19f545");
  public static int RelTypeDocumentationID = 0;
  public static int RelTypeDocRefID = 0;
  public static int RelTypeCompositionID = 0;
  public static int RelTypeECOID = 0;
  public static int RelTypeBuildingCompositionID = 0;
  public static int RelTypeInstancesID = 0;
  public static int ObjTypeSpecificationSectionID = 0;
  public static int ObjTypePartWithoutDrawingID = 0;
  public static int ObjTypePaperDocumentID = 0;
  public static int ObjTypeAssemblyUnitID = 0;
  public static int ObjTypeFormID = 0;
  public static int ObjTypeHeadArticle = 0;
  public static int ObjTypeProductionListsID = 0;
  public const int DocumentationSectID = 1;
  public static int BuildingDocumentationSectID = 0;
  public const int PartSectID = 4;
  public const int MaterialSectID = 7;
  public const int OrderSectID = 99999990;
  public const int CopySectID = 99999916;
  public static readonly Guid SignTypeGuid = new Guid("cad00137-306c-11d8-b4e9-00304f19f545");
  public const int SignTypeID = 0;
  public static readonly Guid ModelSBTypeGuid = new Guid("cad00768-306c-11d8-b4e9-00304f19f545");
  public static readonly Guid SBTypeGuid = new Guid("cad00260-306c-11d8-b4e9-00304f19f545");
  protected static IMetadataInfo _minfo = (IMetadataInfo) null;
  public static Dictionary<int, string> MU = new Dictionary<int, string>();
  public static Dictionary<int, string> FileStoreAliases = new Dictionary<int, string>();
  public static SearchDataPlugin Plugin;
  internal static SpecificationSections SpecificationSections = new SpecificationSections();
  public static Dictionary<char, int> LinkTypesMapper = new Dictionary<char, int>();
  public static DateTime MinDBDateTime = DateTime.ParseExact("01.01.1980", "dd.MM.yyyy", (IFormatProvider) null);
  private static Dictionary<char, long> _purchasedCodes = new Dictionary<char, long>();
  private static Dictionary<int, string> _versionStates = new Dictionary<int, string>();
  private static List<Tuple<int, int, int>> _docVersionStates = new List<Tuple<int, int, int>>();
  private static Dictionary<int, string> _RReasons = new Dictionary<int, string>();
  public static Dictionary<int, string> TechcardDocTypes = new Dictionary<int, string>();
  public static Dictionary<int, string> ProeDocTypes = new Dictionary<int, string>();
  private static List<int> _buildingSectIDs = new List<int>();
  private static List<int> _buildingTypeIDs = (List<int>) null;
  private static int _dbVersion = 0;
  private static List<long> _addedInErrorsObjectLinkAttrs;
  private static List<int> _ECOTypes = new List<int>((IEnumerable<int>) new int[3]
  {
    100000,
    100001,
    100002
  });
  private static Dictionary<int, List<PumpHelper.TypeAttribute>> _obligatoryRelationAttribute = new Dictionary<int, List<PumpHelper.TypeAttribute>>(1);
  private static Dictionary<long, string> _objectCaptions = new Dictionary<long, string>();
  private static List<int> _instanceTypeIDs = new List<int>();
  private static Dictionary<int, Dictionary<ArticlesInManufacture, int>> _cachedInstanceObjectTypes = new Dictionary<int, Dictionary<ArticlesInManufacture, int>>();
  private static string RusLetters = "ЕТОРАНКХСВМ";
  private static string LatLetters = "ETOPAHKXCBM";
  private static List<int> _techProcessTypes = (List<int>) null;
  private static List<int> _SPDocTypes = (List<int>) null;
  private static Regex _verRegex = new Regex("^(.*?)\\.(\\d+)$", RegexOptions.Compiled);
  private static List<int> _CADModelDocTypes = (List<int>) null;
  public static HashSet<string> SkipAttrsForCopies = new HashSet<string>((IEnumerable<string>) new string[7]
  {
    "adoc_id",
    "aversion_id",
    "no_of_sheets",
    "a4_sheets",
    "inv_nomer",
    "acopy_number",
    "abon"
  });
  private static Regex _suffixRegex = (Regex) null;

  public static IMetadataInfo MetadataInfo => PumpHelper._minfo;

  public static bool IsNewPCFormat => PumpHelper.DBVersion >= 1701;

  public static bool IsNewDocsLinksFormat => PumpHelper.DBVersion >= 1835;

  public static bool IsZPCExists => PumpHelper.DBVersion >= 1842;

  public static bool IsVariantsExists => PumpHelper.DBVersion < 2063;

  public static bool IsS4LinkedAuthorExists => PumpHelper.DBVersion >= 2018;

  public static bool IsTechcardDocument(int DocType)
  {
    return PumpHelper.TechcardDocTypes.ContainsKey(DocType);
  }

  public static bool IsProeDocument(int DocType) => PumpHelper.ProeDocTypes.ContainsKey(DocType);

  private static void FillBuildingTypeIDs()
  {
    if (PumpHelper._buildingTypeIDs != null)
      return;
    PumpHelper._buildingTypeIDs = new List<int>();
    CacheCategory cacheCategory = PumpCache.Category[ImportingCategory.ArticleTypes];
    foreach (int buildingSectId in PumpHelper._buildingSectIDs)
    {
      int int32 = Convert.ToInt32(cacheCategory.GetNewKey((object) buildingSectId));
      if (!PumpHelper._buildingTypeIDs.Contains(int32))
        PumpHelper._buildingTypeIDs.Add(int32);
    }
  }

  public static bool IsBuildingSection(int sectID)
  {
    PumpHelper.FillBuildingTypeIDs();
    return PumpHelper._buildingTypeIDs.Contains(sectID);
  }

  public static void Init(SearchDataPlugin plugin)
  {
    BasePumpHelper.Init((PluginClass) plugin);
    PumpHelper.Plugin = plugin;
    PumpHelper._minfo = plugin.Imdi;
    PumpHelper.AttrTypeDesignationID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrTypeNameID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrTypeVersionCodeID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad001fa-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrTypeOKPCodeID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad0038a-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrTypeLiteraID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad0038b-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrTypeNoteID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00021-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrTypeMassaID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00275-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrTypeGroupInstanceID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad001f9-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrTypeFileID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrWorkFileID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cadd98bc-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrVerLinkID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad001c2-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrCountID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00267-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrSPSectionID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00266-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrSPSectionNumID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00279-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrPositionID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00270-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrCompositionContextID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00651-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrSortIndexID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00202-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrOwnerID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad0002f-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrPurchasedID = PumpHelper._minfo.AttributeTypes.GetByGuid(PumpHelper.AttrPurchasedGuid).ID;
    PumpHelper.AttrArchiveID = PumpHelper._minfo.AttributeTypes.GetByGuid(PumpHelper.AttrArchiveGuid).ID;
    PumpHelper.AttrFormatID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00255-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrDocWorkPath = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad007a1-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrPaperDocPath = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad007a2-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrContentModifiedDate = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad0013a-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrModificationID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad014d2-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrAccessLevelID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00816-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrNormID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad0011a-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrNormNameID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00798-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrMaterialID = PumpHelper._minfo.AttributeTypes.GetByGuid(PumpHelper.AttrMaterialGuid).ID;
    PumpHelper.AttrImbaseKeyID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00162-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrImbaseLinkID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00209-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrImbaseCodeID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad0020f-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrEndDateID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad0079e-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrDateOfReleaseID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad0079f-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrTermOfChangeID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad007a0-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrChangeNoID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00770-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrReasonCodeID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad0077d-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrECO_DateDueID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cadd9562-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrECOLinkID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cadd9645-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrGroupNo = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad001c0-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrSubInGroup = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad001c1-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrLRINList1 = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00771-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrLRINList2 = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00772-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrLRINList3 = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00773-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrLRINList4 = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00774-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrLRINList5 = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00775-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrLRISoprovDoc = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00776-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrLRIDate = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00778-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrLRIPodpis = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00779-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrSerialNoID = PumpHelper._minfo.AttributeTypes.GetByGuid(PDMHelper.attributeSerialNo).ID;
    PumpHelper.AttrArtStorageID = PumpHelper._minfo.AttributeTypes.GetByGuid(PDMHelper.attributeStorageArticle).ID;
    PumpHelper.AttrBCHNameID = PumpHelper._minfo.AttributeTypes.GetByGuid(PumpHelper.AttrBCHNameGuid).ID;
    PumpHelper.RelTypeDocumentationID = PumpHelper._minfo.RelationTypes.GetByGuid(new Guid("cad00154-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.RelTypeDocRefID = PumpHelper._minfo.RelationTypes.GetByGuid(PumpHelper.RelTypeDocRefGuid).ID;
    PumpHelper.RelTypeCompositionID = PumpHelper._minfo.RelationTypes.GetByGuid(new Guid("cad00023-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.RelTypeECOID = PumpHelper._minfo.RelationTypes.GetByGuid(new Guid("cad0036b-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.RelTypeBuildingCompositionID = PumpHelper._minfo.RelationTypes.GetByGuid(new Guid("cad008d6-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.RelTypeInstancesID = PumpHelper._minfo.RelationTypes.GetByGuid(PDMHelper.relationTypeInstances).ID;
    PumpHelper.ObjTypeSpecificationSectionID = PumpHelper._minfo.ObjectTypes.GetByGuid(new Guid("cad00254-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.ObjTypePartWithoutDrawingID = PumpHelper._minfo.ObjectTypes.GetByGuid(new Guid("cad00861-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.ObjTypePaperDocumentID = PumpHelper._minfo.ObjectTypes.GetByGuid(new Guid("cad0090f-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.ObjTypeAssemblyUnitID = PumpHelper._minfo.ObjectTypes.GetByGuid(new Guid("cad00132-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.ObjTypeFormID = PumpHelper._minfo.ObjectTypes.GetByGuid(new Guid("cad0011c-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.ObjTypeHeadArticle = PumpHelper._minfo.ObjectTypes.GetByGuid(new Guid("cadd940b-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.ObjTypeProductionListsID = PumpHelper._minfo.ObjectTypes.GetByGuid(new Guid("cadd9a5c-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrFormNameID = BasePumpHelper._session.IdentHelper.NameID;
    PumpHelper.AttrFormBodyID = PumpHelper._minfo.AttributeTypes.GetByGuid(PumpHelper.AttrFormBodyGuid).ID;
    PumpHelper.AttrFormObjectTypesID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad00149-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrSheetsCountID = PumpHelper._minfo.AttributeTypes.GetByGuid(PumpHelper.AttrSheetsCountGuid).ID;
    PumpHelper.AttrSeriesID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cadd940c-306c-11d8-b4e9-00304f19f545")).ID;
    PumpHelper.AttrBasedOnCadModelID = PumpHelper._minfo.AttributeTypes.GetByGuid(new Guid("cad0153e-306c-11d8-b4e9-00304f19f545")).ID;
    using (IDataReader dataReader = BasePumpHelper.S4Query("select MU_ID,MU_SHORT_NAME from MU"))
    {
      while (dataReader.Read())
        PumpHelper.MU.Add(Convert.ToInt32(dataReader[0]), dataReader[1].ToString());
    }
    using (IDataReader dataReader = BasePumpHelper.S4Query("select DIRKEY_ID, DIR_NAME from DC where DIR_STATUS = 'B'"))
    {
      while (dataReader.Read())
        PumpHelper.FileStoreAliases.Add(Convert.ToInt32(dataReader[0]), dataReader[1].ToString());
    }
    IDBObjectCollection objectCollection = BasePumpHelper._session.GetObjectCollection(PumpHelper.ObjTypeSpecificationSectionID);
    DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new object[3]
    {
      (object) -2,
      (object) PumpHelper.AttrSPSectionNumID,
      (object) -50
    }, 0L, (object) null, -1);
    foreach (DataRow row in (InternalDataCollectionBase) objectCollection.Select(paramSet).Rows)
    {
      SpecificationSection specificationSection = new SpecificationSection((long) Convert.ToInt32(row[0]), row[2].ToString());
      try
      {
        PumpHelper.SpecificationSections.Add(Convert.ToInt32(row[1]), specificationSection);
      }
      catch
      {
        BasePumpHelper.AppManager.AddWarningMessage($"В базе IPS обнаружено дублирование раздела конструкторской спецификации (номер раздела: {row[1].ToString()})");
      }
    }
    PumpHelper.LinkTypesMapper.Add('M', 2);
    PumpHelper.LinkTypesMapper.Add('P', 3);
    PumpHelper._purchasedCodes.Add('-', 1L);
    PumpHelper._purchasedCodes.Add('+', 2L);
    PumpHelper._purchasedCodes.Add('*', 3L);
    PumpHelper._purchasedCodes.Add('!', 4L);
    using (IDataReader dataReader = BasePumpHelper.S4Query("select REASONCODE, REASONKOD from RREASONS"))
    {
      while (dataReader.Read())
        PumpHelper._RReasons.Add(BasePumpHelper.ToInt32(dataReader[0]), dataReader.IsDBNull(1) ? "" : dataReader.GetString(1));
    }
    using (IDataReader dataReader = BasePumpHelper.S4Query("SELECT VERSION_STATE_ID, VERSION_STATE_NAME FROM VERSION_STATE"))
    {
      while (dataReader.Read())
        PumpHelper._versionStates.Add(BasePumpHelper.ToInt32(dataReader[0]), dataReader.IsDBNull(1) ? "" : dataReader.GetString(1));
    }
    using (IDataReader dataReader = BasePumpHelper.S4Query("SELECT DOC_ID, VERSION_ID, VERSION_STATE_ID FROM RC ORDER BY DOC_ID, VERSION_ID"))
    {
      while (dataReader.Read())
        PumpHelper._docVersionStates.Add(new Tuple<int, int, int>(BasePumpHelper.ToInt32(dataReader[0]), BasePumpHelper.ToInt32(dataReader[1]), BasePumpHelper.ToInt32(dataReader[2])));
    }
    List<string> stringList = new List<string>((IEnumerable<string>) new string[11]
    {
      "Альбом",
      "Групповой техпроцесс",
      "Перевод техпроцесса",
      "Расцеховочный маршрут",
      "Техпроцесс",
      "Типовой техпроцесс",
      "Комплект ведомостей",
      "Комплект технологических документов",
      "Техпроцесс единичный",
      "Техпроцесс групповой",
      "Техпроцесс типовой"
    });
    string str1 = "";
    foreach (string str2 in stringList)
    {
      if (str1 != "")
        str1 += ",";
      str1 = $"{str1}'{str2}'";
    }
    using (IDataReader dataReader = BasePumpHelper.S4Query($"select DOC_TYPE, DOC_NAME from DOCTYPES where DOC_NAME in ({str1}) or DOC_CODE like 'TC_%'"))
    {
      while (dataReader.Read())
        PumpHelper.TechcardDocTypes.Add(BasePumpHelper.ToInt32(dataReader[0]), dataReader.GetString(1));
    }
    using (IDataReader dataReader = BasePumpHelper.S4Query("select DOC_TYPE, DOC_NAME from DOCTYPES where Upper(DOC_CODE) like 'PEPAR%' or Upper(DOC_CODE) like 'PEASM%' or Upper(DOC_CODE) like 'PEDRW%' or Upper(DOC_CODE) like 'PELAY%' or Upper(DOC_CODE) like 'PEMFG%'"))
    {
      while (dataReader.Read())
        PumpHelper.ProeDocTypes.Add(BasePumpHelper.ToInt32(dataReader[0]), dataReader.GetString(1));
    }
    IObjectTypeItem byGuid = PumpHelper._minfo.ObjectTypes.GetByGuid(new Guid("cad0057f-306c-11d8-b4e9-00304f19f545"));
    DTSuffixesHelper.FillDTSuffixes(BasePumpHelper._session, PumpHelper.Plugin.Imdi.ObjectTypes, byGuid);
    using (IDataReader dataReader = BasePumpHelper.S4Query("select s.section_id from ssections s where exists (select * from contexts_for_objs c where c.bo_id = 23 and c.f_context_id = 2 and s.section_id = c.f_instance_id) and not exists (select * from contexts_for_objs c where c.bo_id = 23 and c.f_context_id = 1 and s.section_id = c.f_instance_id)"))
    {
      while (dataReader.Read())
      {
        int int32 = BasePumpHelper.ToInt32(dataReader[0]);
        if (int32 > 0)
          PumpHelper._buildingSectIDs.Add(int32);
      }
    }
    using (IDataReader dataReader = BasePumpHelper.S4Query("select section_id from ssections where sectname = 'Проектная документация'"))
    {
      if (dataReader.Read())
        PumpHelper.BuildingDocumentationSectID = BasePumpHelper.ToInt32(dataReader[0]);
    }
    SignsHolder.Init(BasePumpHelper._session, (IServiceProvider) null);
    if (PumpHelper.DBVersion != 0)
      return;
    PumpHelper._dbVersion = BasePumpHelper.S4IntQuery("select version_id from dbversion");
  }

  public static int DBVersion => PumpHelper._dbVersion;

  public static MeasuredValue GetCountAttrValue(int muID, double count_pc)
  {
    string str1;
    PumpHelper.MU.TryGetValue(muID, out str1);
    string str2 = str1.Trim();
    ServicesManager.GetService(typeof (IMeasures));
    return MeasureHelper.ConvertToMeasuredValue($"{count_pc} {str2}");
  }

  private static object SmartConvert(object value, FieldTypes ft)
  {
    if (!DBNull.Value.Equals(value))
    {
      switch (ft)
      {
        case FieldTypes.ftInteger:
          long result1 = 0;
          value = !long.TryParse(value.ToString(), out result1) ? (object) DBNull.Value : (object) result1;
          break;
        case FieldTypes.ftDouble:
          double result2 = 0.0;
          value = !double.TryParse(value.ToString(), out result2) ? (object) DBNull.Value : (object) result2;
          break;
        case FieldTypes.ftDateTime:
          DateTime result3;
          value = !DateTime.TryParse(value.ToString(), out result3) || !(result3 >= PumpHelper.MinDBDateTime) ? (object) DBNull.Value : (object) result3;
          break;
      }
    }
    return value;
  }

  public static void AddAttribute(IImportedObjectList writer, int attrTypeID, object value)
  {
    PumpHelper.InternalAddAttribute((object) writer, attrTypeID, value, (DictionaryValue) null, 0L, string.Empty);
  }

  public static void AddAttribute(
    IImportedObjectList writer,
    int attrTypeID,
    object value,
    DictionaryValue artInfo,
    string key)
  {
    PumpHelper.InternalAddAttribute((object) writer, attrTypeID, value, artInfo, 0L, key);
  }

  public static void AddAttribute(
    IImportedRelationList writer,
    int attrTypeID,
    object value,
    DictionaryValue artInfo)
  {
    PumpHelper.InternalAddAttribute((object) writer, attrTypeID, value, artInfo, 0L, string.Empty);
  }

  private static void InternalAddAttribute(
    object objwriter,
    int attrTypeID,
    object value,
    DictionaryValue artInfo,
    long objID,
    string key)
  {
    if (CompareValuesHelper.NormalizedValue(value) == null)
      return;
    IImportedObjectList importedObjectList = (IImportedObjectList) null;
    IImportedRelationList importedRelationList = (IImportedRelationList) null;
    switch (objwriter)
    {
      case IImportedObjectList _:
        importedObjectList = (IImportedObjectList) objwriter;
        break;
      case IImportedRelationList _:
        importedRelationList = (IImportedRelationList) objwriter;
        break;
    }
    AttrValueType attrValtype = AttrValueType.unknownVal;
    IAttributeTypeItem byId = PumpHelper._minfo.AttributeTypes.GetByID(attrTypeID);
    if (byId == null)
    {
      BasePumpHelper.AppManager.AddWarningMessage($"MetadataInfo не содержит информации об атрибуте {attrTypeID}, невозможно перекачать значение!");
    }
    else
    {
      bool flag = false;
      switch (byId.AttrValueType)
      {
        case 1:
          attrValtype = AttrValueType.stringVal;
          break;
        case 2:
        case 12:
          attrValtype = AttrValueType.integerVal;
          break;
        case 3:
          attrValtype = AttrValueType.doubleVal;
          break;
        case 4:
          attrValtype = AttrValueType.datetimeVal;
          break;
        case 5:
        case 10:
        case 11:
          flag = true;
          break;
        case 8:
          Guid guid;
          if (!GuidHelper.IsGuid(artInfo.Caption))
          {
            guid = new Guid("cad00172-306c-11d8-b4e9-00304f19f545");
            if (PumpHelper._addedInErrorsObjectLinkAttrs == null)
              PumpHelper._addedInErrorsObjectLinkAttrs = new List<long>(1);
            if (PumpHelper._addedInErrorsObjectLinkAttrs.IndexOf(artInfo.NewObjectID) < 0)
            {
              BasePumpHelper.AppManager.AddWarningMessage($"Атрибут с новым ID = {artInfo.NewObjectID} в кэше по ключу \"{key}\" записан GUID создаваемого типа объектов =\"{artInfo.Caption}\", ");
              PumpHelper._addedInErrorsObjectLinkAttrs.Add(artInfo.NewObjectID);
            }
          }
          else
            guid = new Guid(artInfo.Caption);
          IObjectTypeItem byGuid = PumpHelper._minfo.ObjectTypes.GetByGuid(guid);
          string materialName = value.ToString().Trim();
          MaterialInfo materialInfo = new MaterialInfo();
          if (materialName != "")
            materialInfo = PumpHelper._minfo.Materials.GetMaterial(materialName, byGuid.ID);
          if (materialInfo != null && materialInfo.ObjectID > 0L)
          {
            if (importedObjectList != null)
            {
              importedObjectList.AddAttributeLink(attrTypeID, materialInfo.ObjectID, materialInfo.Caption);
              return;
            }
            importedRelationList?.AddAttributeLink(attrTypeID, materialInfo.ObjectID, materialInfo.Caption);
            return;
          }
          BasePumpHelper.AppManager.AddWarningMessage($"Материал, записанный в параметре {key} изделия [{objID}], не был записан");
          return;
        case 13:
          return;
      }
      value = PumpHelper.SmartConvert(value, (FieldTypes) byId.AttrValueType);
      if (!flag && attrValtype == AttrValueType.unknownVal)
        throw new Exception($"Unknown type of data ({attrTypeID},{(value != null ? value : (object) "[null]")})");
      if (!flag)
      {
        if (importedObjectList != null)
          importedObjectList.AddAttribute(attrTypeID, attrValtype, value, 0);
        else
          importedRelationList?.AddAttribute(attrTypeID, attrValtype, value, 0);
      }
      else
      {
        BlobHelper.ReserveBlob(value.ToString());
        if (importedObjectList != null)
          importedObjectList.AddAttributeBlob(attrTypeID, BlobHelper.TempFileName, BlobHelper.FileSize, "", ArcMethods.NotPacked);
        else
          importedRelationList?.AddAttributeBlob(attrTypeID, BlobHelper.TempFileName, BlobHelper.FileSize, "", ArcMethods.NotPacked);
      }
    }
  }

  public static long AddUserLink(IImportedObjectList writer, int attrID, int oldUserID)
  {
    DictionaryValue dictionaryValue = BasePumpHelper._usersCache.GetValue((object) oldUserID);
    if (dictionaryValue == null)
      return 0;
    long newObjectId = dictionaryValue.NewObjectID;
    writer.AddAttributeLink(attrID, newObjectId, dictionaryValue.Caption);
    return newObjectId;
  }

  public static long PurchasedToLong(object value)
  {
    if (DBNull.Value.Equals(value) || value.ToString().Trim() == "")
      return 1;
    char key = Convert.ToChar(value);
    long num = -1;
    return PumpHelper._purchasedCodes.TryGetValue(key, out num) ? num : 1L;
  }

  public static object ToDateTime(object dt)
  {
    return dt == null || DBNull.Value.Equals(dt) ? (object) DBNull.Value : (object) Convert.ToDateTime(dt);
  }

  public static string ConvertECOReason(int reason)
  {
    string str = "";
    return !PumpHelper._RReasons.TryGetValue(reason, out str) ? (string) null : str;
  }

  public static bool IsECO(int docType) => PumpHelper._ECOTypes.Contains(docType);

  public static int SetUpLCStep(
    ObjectRecord objRec,
    S4Table data,
    int versionID,
    CacheCategory cache)
  {
    object obj = (object) -2;
    if (data.TryGetValue("doc_id", out obj))
    {
      int int32 = Convert.ToInt32(obj);
      if (int32 != -2)
        return PumpHelper.SetUpLCStep(objRec, Convert.ToInt32(data["archive_id"]), int32, versionID, cache);
    }
    return 0;
  }

  public static int SetUpLCStep(
    ObjectRecord objRec,
    int arcID,
    int docID,
    int versionID,
    CacheCategory cache)
  {
    int stepID = 0;
    if (docID != -2 && arcID != 0)
    {
      Tuple<int, int, int> tuple = PumpHelper._docVersionStates.Find((Predicate<Tuple<int, int, int>>) (x => x.Item1 == docID && x.Item2 == versionID));
      stepID = PumpHelper._minfo.GetLCStepForArchiveType(arcID, objRec.ObjectType, tuple != null ? tuple.Item3 : 0);
      if (objRec.Lc_step != stepID)
      {
        int lcLevel = (ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo).GetLCLevel(stepID);
        if (lcLevel != 0)
        {
          objRec.Lc_step = stepID;
          objRec.LevelId = lcLevel;
        }
        else
          BasePumpHelper.AppManager.AddWarningMessage($"Уровень продвижения для шага ЖЦ {stepID} не найден. Перевод документа {docID} (тип объектов {objRec.ObjectType}) на этот шаг невозможен.");
      }
    }
    return stepID;
  }

  public static int TypeGuidToID(Guid g)
  {
    IObjectTypeItem byGuid = PumpHelper._minfo.ObjectTypes.GetByGuid(g);
    return byGuid != null ? byGuid.ID : 0;
  }

  public static bool IsInstanceOrParty(int typeID) => PumpHelper._instanceTypeIDs.Contains(typeID);

  private static int InternalGetInstanceObjectType(int objType, ArticlesInManufacture am)
  {
    IDBObjectType instanceObjectType = PDMHelper.GetInstanceObjectType(BasePumpHelper._session, objType, am);
    if (instanceObjectType == null)
      return 0;
    int objectType = instanceObjectType.ObjectType;
    if (!PumpHelper._instanceTypeIDs.Contains(objectType))
      PumpHelper._instanceTypeIDs.Add(objectType);
    return objectType;
  }

  public static int GetInstanceObjectType(int objType, ArticlesInManufacture am)
  {
    Dictionary<ArticlesInManufacture, int> dictionary = (Dictionary<ArticlesInManufacture, int>) null;
    if (!PumpHelper._cachedInstanceObjectTypes.TryGetValue(objType, out dictionary))
    {
      dictionary = new Dictionary<ArticlesInManufacture, int>();
      PumpHelper._cachedInstanceObjectTypes.Add(objType, dictionary);
    }
    if (!dictionary.ContainsKey(am))
      dictionary.Add(am, PumpHelper.InternalGetInstanceObjectType(objType, am));
    return dictionary[am];
  }

  public static string GetArticleDesignation(int artID, int artVerID)
  {
    IDataReader dataReader1;
    if (artVerID >= 0)
      dataReader1 = BasePumpHelper.S4Query("select designatio from v_articles where art_id = @p1 and art_ver_id = @p2", (object) artID, (object) artVerID);
    else
      dataReader1 = BasePumpHelper.S4Query("select designatio from articles where art_id = @p1", (object) artID);
    using (IDataReader dataReader2 = dataReader1)
    {
      while (dataReader2.Read())
      {
        string designation = Convert.ToString(dataReader2[0]);
        if (designation != string.Empty)
          return PumpHelper.TrimArticleDesignationSuffix(designation);
      }
    }
    return string.Empty;
  }

  public static string GetDocumentDesignation(int docID)
  {
    using (IDataReader dataReader = BasePumpHelper.S4Query("select a.designatio, d.dt_code from doclist a, doctypes d where a.doc_type = d.doc_type and a.doc_id = @p1", (object) docID))
    {
      while (dataReader.Read())
      {
        string str1 = Convert.ToString(dataReader[0]);
        if (str1 != string.Empty)
        {
          string str2 = Convert.ToString(dataReader[1]);
          return str2 != string.Empty && str1.EndsWith($" {str2}") ? str1.Remove(str1.Length - str2.Length - 1, str2.Length + 1) : str1;
        }
      }
    }
    return string.Empty;
  }

  public static int GetInstanceObjectType(int objType, ArtClass artClass)
  {
    ArticlesInManufacture am;
    switch (artClass)
    {
      case ArtClass.Instance:
        am = ArticlesInManufacture.Instances;
        break;
      case ArtClass.Party:
        am = ArticlesInManufacture.Parties;
        break;
      default:
        return 0;
    }
    return PumpHelper.GetInstanceObjectType(objType, am);
  }

  public static string TestForError(ImportingObject item) => item == null ? "" : (string) null;

  private static string GetTypeName(int typeID)
  {
    string str = "???";
    IObjectTypeItem byId = PumpHelper._minfo.ObjectTypes.GetByID(typeID);
    if (byId != null)
      str = byId.Name;
    return $"{str} ({typeID.ToString()})";
  }

  public static string LatToRus(string s)
  {
    StringBuilder stringBuilder = new StringBuilder(s);
    int length = PumpHelper.LatLetters.Length;
    for (int index = 0; index < length; ++index)
      stringBuilder.Replace(PumpHelper.LatLetters[index], PumpHelper.RusLetters[index]);
    return stringBuilder.ToString();
  }

  public static string LiteraToString(object l)
  {
    return PumpHelper.LatToRus(l.ToString().Trim().Replace(" ", "").ToUpper());
  }

  internal static bool IsTechProcess(int docObjectType)
  {
    if (PumpHelper._techProcessTypes == null)
      PumpHelper._techProcessTypes = MetaDataHelper.GetObjectTypeChildrenID(new Guid("cad00185-306c-11d8-b4e9-00304f19f545"));
    return PumpHelper._techProcessTypes.Contains(docObjectType);
  }

  internal static List<int> SPDocTypes
  {
    get
    {
      if (PumpHelper._SPDocTypes == null)
      {
        PumpHelper._SPDocTypes = new List<int>();
        using (IDbCommand command = PumpHelper.Plugin.idb2.CreateCommand())
        {
          command.CommandText = "select doc_type from doctypes where upper(doc_ext) like 'SP%'";
          IDataReader dataReader = command.ExecuteReader();
          try
          {
            while (dataReader.Read())
              PumpHelper._SPDocTypes.Add(BasePumpHelper.ToInt32(dataReader[0]));
          }
          finally
          {
            dataReader.Close();
          }
        }
      }
      return PumpHelper._SPDocTypes;
    }
  }

  public static int ExtractFileVersionNumber(ref string fn)
  {
    Match match = PumpHelper._verRegex.Match(fn);
    if (!match.Success)
      return 0;
    fn = match.Groups[1].Value;
    return Convert.ToInt32(match.Groups[2].Value);
  }

  internal static List<int> CADModelDocTypes
  {
    get
    {
      if (PumpHelper._CADModelDocTypes == null)
      {
        PumpHelper._CADModelDocTypes = new List<int>();
        Guid[] guidArray = new Guid[2]
        {
          PumpHelper.ModelSBTypeGuid,
          PumpHelper.SBTypeGuid
        };
        List<int> parentTypeIDs = new List<int>();
        foreach (Guid objTypeGuid in guidArray)
        {
          int objectTypeId = MetaDataHelper.GetObjectTypeID(objTypeGuid);
          if (objectTypeId > 0)
            parentTypeIDs.Add(objectTypeId);
        }
        List<int> childrenIdRecursive = MetaDataHelper.GetObjectTypeChildrenIDRecursive((IEnumerable<int>) parentTypeIDs);
        foreach (KeyValuePair<object, DictionaryValue> keyValuePair in PumpCache.Category[ImportingCategory.DocTypes].Items)
        {
          if (childrenIdRecursive.Contains((int) keyValuePair.Value.NewObjectID))
          {
            int int32 = Convert.ToInt32(keyValuePair.Key);
            if (!PumpHelper._CADModelDocTypes.Contains(int32))
              PumpHelper._CADModelDocTypes.Add(int32);
          }
        }
      }
      return PumpHelper._CADModelDocTypes;
    }
  }

  public static string ConcatSign
  {
    get => BasePumpHelper.dbType == BasePumpHelper.DBType.MSSQL ? "+" : "||";
  }

  public static string TrimArticleDesignationSuffix(string designation)
  {
    if (PluginSettings.ArtSuffixesToDelete == null)
      return designation;
    if (PumpHelper._suffixRegex == null)
      PumpHelper._suffixRegex = new Regex($"(\\s+({string.Join("|", (IEnumerable<string>) PluginSettings.ArtSuffixesToDelete)}))$", RegexOptions.Compiled);
    return PumpHelper._suffixRegex.Match(designation).Success ? PumpHelper._suffixRegex.Replace(designation, string.Empty) : designation;
  }

  public static string GetArticleCaption(string designation, string name)
  {
    return !(designation == "") ? (!(name == "") ? $"{designation} ({name})" : designation) : name;
  }

  private class TypeAttribute
  {
    public int AttributeID;
    public RequiredModes Required;
    public FieldTypes FieldType;
    public string DefaultValue;

    public TypeAttribute(DataRow row, IUserSession session)
    {
      this.AttributeID = Convert.ToInt32(row["F_ATTRIBUTE_ID"]);
      this.Required = (RequiredModes) Convert.ToInt32(row["F_REQUIRED"]);
      this.FieldType = session.GetAttributeType(this.AttributeID).AttributeType;
      this.DefaultValue = Convert.ToString(row["F_DEFAULT_VALUE"]);
    }
  }
}

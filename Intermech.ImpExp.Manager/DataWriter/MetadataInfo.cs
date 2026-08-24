// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.MetadataInfo
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Manager.CommonData;
using Intermech.ImpExp.Manager.CommonData.ItemsToCreate;
using Intermech.Interfaces;
using Intermech.Interfaces.Briefcase;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.LifeCycles;
using Intermech.Protection;
using Intermech.Security;
using Intermech.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal sealed class MetadataInfo : IDataWriterProxy, IMetadataInfo
{
  private IUserSession _session;
  private long _userID;
  private Guid _userGUID = Guid.Empty;
  private IDBImporter _dbImporter;
  private DataSet metadataDS;
  private DataSet importingDS;
  private IAppManager _appManager;
  private MetadataInfo.CommonDataImpl _commonData;
  private ObjectTypeItemListImpl _objectTypes;
  private AttributeTypeItemListImpl _attributeTypes;
  private AttributeGroupItemListImpl _attributeGroups;
  private RelationTypeItemListImpl _relationTypes;
  private bool _configured;
  private Dictionary<int, int> _lcStepsToLevels;
  private LCSteps4ArchivesClass _lcSteps4Archives;
  private List<string> impListObjectsId = new List<string>();
  private List<string> impListObjectsGuid = new List<string>();
  private List<string> impListAttributesId = new List<string>();
  private List<string> impListAttributesGuid = new List<string>();
  private List<string> impListAttrGroupsId = new List<string>();
  private List<string> impListAttrGroupsGuid = new List<string>();
  private List<string> impListRelationsId = new List<string>();
  private List<string> impListRelationsGuid = new List<string>();
  private bool GuidGenerateMode = true;

  public IDBImporter dbImporter
  {
    get
    {
      if (this._dbImporter == null && this._session != null)
        this._dbImporter = this._session.GetImporter("ImpExpImport.log");
      return this._dbImporter;
    }
  }

  public IImportedObjects ImportedObjects { get; private set; }

  public IImportedUsers ImportedUsers { get; private set; }

  public IImportedRelations ImportedRelations { get; private set; }

  public IMaterials Materials { get; private set; }

  public MetadataInfo(IAppManager manager)
  {
    this._objectTypes = new ObjectTypeItemListImpl((IDataWriterProxy) this);
    this._attributeTypes = new AttributeTypeItemListImpl((IDataWriterProxy) this);
    this._attributeGroups = new AttributeGroupItemListImpl((IDataWriterProxy) this);
    this._relationTypes = new RelationTypeItemListImpl((IDataWriterProxy) this);
    this._commonData = new MetadataInfo.CommonDataImpl();
    this._appManager = manager;
    RBSClient.InitializeSecurityContext();
  }

  public void Close()
  {
    if (this._dbImporter == null)
      return;
    this._dbImporter.CloseImporter();
  }

  public bool CheckDBVersion(bool showErrorMessage)
  {
    try
    {
      this._session.CheckDBVersion("PUMPER", 11, true);
      return true;
    }
    catch (Exception ex)
    {
      if (showErrorMessage)
      {
        int num = (int) MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return false;
    }
  }

  public bool Login(int conCount)
  {
    bool flag = false;
    if (!this._configured)
    {
      RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, false);
      this._configured = true;
    }
    string empty1 = string.Empty;
    foreach (WellKnownClientTypeEntry wellKnownClientType in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
    {
      string objectUrl = wellKnownClientType.ObjectUrl;
    }
    try
    {
      IMServerService imServerService = new IMServerService()
      {
        ConnectionErrorStrategy = (IMServerConnectionErrorStrategy) new IMServerInteractiveConnectionErrorStrategy((IUIDispatcherService) ServicesManager.GetService(typeof (IUIDispatcherService)))
      };
      this._session = imServerService.ServerObject.CreateSession();
      DateTime now = DateTime.Now;
      TimeSpan aTimeZoneOffset = now - now.ToUniversalTime();
      string caption = "Подключение к серверу приложений IPS";
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      int num = conCount;
      while (num > 0 && !flag && IMConnection.Login(ref empty2, ref empty3, caption))
      {
        --num;
        try
        {
          this._session.Login(empty2, new PswPackage(empty3, imServerService.ServerObject.CryptMethod), SystemInformation.ComputerName, aTimeZoneOffset, this._session.IdentHelper.AdminRoleID);
          flag = true;
        }
        catch (Exception ex)
        {
          this._appManager.AddErrorMessage("Ошибка подключения к серверу Intermech: " + ex.Message);
          this._appManager.AddExceptionToLog(ex);
          if (MessageBox.Show(ex.Message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Hand) == DialogResult.Cancel)
            break;
        }
      }
      if (flag)
      {
        this._appManager.AddInfoMessage("Подключение к серверу Intermech прошло успешно");
        RBSClient.UpdateSecurityContext(this._session);
        SessionKeeper.InitializeAllocator((IUserSessionAllocator) new SingleSessionAllocator(this._session));
        this._lcSteps4Archives = new LCSteps4ArchivesClass();
        this._dbImporter = this._session.GetImporter("ImpExpImport.log");
      }
    }
    catch (Exception ex)
    {
      this._appManager.AddErrorMessage("Ошибка подключения к серверу Intermech: " + ex.Message);
      this._appManager.AddExceptionToLog(ex);
    }
    return flag;
  }

  private DataSet CreateImportingList()
  {
    DataSet importingList = new DataSet(BriefcaseConsts.XmlMetadataExportListDatasetName);
    DataTable dataTable = importingList.Tables.Add(BriefcaseConsts.XmlMetadataRecordTag);
    dataTable.Columns.Add(BriefcaseConsts.XmlCategoryTag, typeof (int));
    dataTable.Columns.Add(BriefcaseConsts.XmlIdTag, typeof (string));
    dataTable.Columns.Add(BriefcaseConsts.XmlExternalTag, typeof (string));
    return importingList;
  }

  private bool LoadFromServer()
  {
    if (this._session == null)
      return false;
    MetaDataHelper.SyncMetadata((this._session as IUserSessionCacheDataSet).CacheDataSet);
    IServerBriefcase briefcase = this._session.GetBriefcase();
    bool flag = false;
    try
    {
      briefcase.CheckExportRights(this._session.SessionGUID);
      this.importingDS = this.CreateImportingList();
      this.metadataDS = briefcase.GetDataset(this._session.SessionGUID, new string[1]
      {
        "SYSTEM"
      }, false);
      this.metadataDS.DataSetName = BriefcaseConsts.XmlMetadataDatasetName;
      this.Reload();
      DataTable datatable = briefcase.GetDatatable(this._session.SessionGUID, "IMS_LC_STEPS", string.Empty, string.Empty);
      this._lcStepsToLevels = new Dictionary<int, int>(datatable.Rows.Count);
      foreach (DataRow row in (InternalDataCollectionBase) datatable.Rows)
        this._lcStepsToLevels.Add(Convert.ToInt32(row["F_LC_STEP"]), Convert.ToInt32(row["F_LEVEL_ID"]));
      flag = true;
    }
    catch (Exception ex)
    {
      this._appManager.AddErrorMessage("Ошибка при экспорте метаданных: " + ex.Message);
      this._appManager.AddErrorMessage(" *************************************** ");
      this._appManager.AddErrorMessage(ex.StackTrace);
      this._appManager.AddErrorMessage(" *************************************** ");
    }
    return flag;
  }

  private bool SaveToServer()
  {
    this.metadataDS.RemotingFormat = SerializationFormat.Binary;
    this.importingDS.RemotingFormat = SerializationFormat.Binary;
    Guid guid = Guid.NewGuid();
    ImportMetadataThread importMetadataThread = new ImportMetadataThread(this.dbImporter, guid, this.metadataDS, this.importingDS, IgnoringErrors.IgnoreFormulaErrors);
    try
    {
      return importMetadataThread.Import();
    }
    finally
    {
      this.dbImporter.EndImportMetadata(guid);
    }
  }

  private bool CheckCaches()
  {
    long lastObjectId = this.dbImporter.LastObjectID;
    foreach (KeyValuePair<object, DictionaryValue> keyValuePair in this.ImportedObjects.Dictionary)
    {
      if ((long) keyValuePair.Key > lastObjectId)
        return false;
    }
    foreach (KeyValuePair<object, DictionaryValue> keyValuePair in this.ImportedRelations.Dictionary)
    {
      if ((long) keyValuePair.Key > lastObjectId)
        return false;
    }
    return true;
  }

  private void Reload()
  {
    if (this.ImportedObjects == null)
      this.ImportedObjects = (IImportedObjects) new Intermech.ImpExp.Manager.DataWriter.ImportedObjects();
    if (this.ImportedRelations == null)
      this.ImportedRelations = (IImportedRelations) new Intermech.ImpExp.Manager.DataWriter.ImportedRelations();
    if (!this.CheckCaches())
      throw new Exception("Кэши последней закачки более новее, чем база данных IPS.");
    if (this.ImportedUsers == null)
      this.ImportedUsers = (IImportedUsers) new Intermech.ImpExp.Manager.DataWriter.ImportedUsers();
    if (this.Materials == null)
    {
      this.Materials = (IMaterials) new Intermech.ImpExp.Manager.DataWriter.Materials(this._session);
      ((Intermech.ImpExp.Manager.DataWriter.Materials) this.Materials).LoadMaterialsFromBase(this._session);
    }
    IDBObject dbObject = this._session.GetObject(this.UserID);
    this._userID = this._session.IdentHelper.SysdbaID;
    if (this.ImportedObjects.GetID(dbObject.ObjectID) == 0L)
      this.ImportedObjects.AddValue(dbObject.ObjectID, dbObject.ID, dbObject.ObjectType, dbObject.ObjectGUID, dbObject.GUID);
    this._objectTypes.Clear();
    foreach (DataRow row in (InternalDataCollectionBase) this.metadataDS.Tables["IMS_OBJECT_TYPES"].Rows)
      this.AddObjectType(row);
    foreach (DataRow row in (InternalDataCollectionBase) this.metadataDS.Tables["IMS_OBJTYPES_TREE"].Rows)
    {
      int int32_1 = Convert.ToInt32(row["F_OBJECT_TYPE"]);
      int int32_2 = Convert.ToInt32(row["F_PARENT_ID"]);
      IDBObjectType objectType = this._session.GetObjectType(int32_2);
      IObjectTypeItem byId = this._objectTypes.GetByID(int32_1);
      if (byId != null)
      {
        byId.ParentID = (objectType as IDBGuid).GUID;
        (this.ObjectTypes.GetByID(int32_2) as ObjectTypeItemImpl).childIdAdd(int32_1);
      }
    }
    foreach (DataRow row in (InternalDataCollectionBase) this.metadataDS.Tables["IMS_ATTR4OBJ_TYPES"].Rows)
    {
      int int32_3 = Convert.ToInt32(row["F_OBJECT_TYPE"]);
      int int32_4 = Convert.ToInt32(row["F_ATTRIBUTE_ID"]);
      this._objectTypes.GetByID(int32_3)?.AddAttrTypeId(int32_4);
    }
    this._attributeTypes.Clear();
    foreach (DataRow row in (InternalDataCollectionBase) this.metadataDS.Tables["IMS_ATTRIBUTES"].Rows)
      this.AddAttributeType(row).ExistsInBase = true;
    foreach (IMSAttributeType imsAttributeType in MetaDataHelper.GetAttributeTypesList().Where<IMSAttributeType>((System.Func<IMSAttributeType, bool>) (x => x.AttributeID < 0)).ToList<IMSAttributeType>())
      this.AddAttributeType(imsAttributeType.AttributeID, (int) imsAttributeType.FieldType, imsAttributeType.AttributeGuid, imsAttributeType.Name, imsAttributeType.ShortName, imsAttributeType.Alias, imsAttributeType.MultiValueMode, (int) imsAttributeType.SizeType, imsAttributeType.DefaultValue).ExistsInBase = true;
    this._attributeGroups.Clear();
    foreach (DataRow row in (InternalDataCollectionBase) this.metadataDS.Tables["IMS_ATTR_GROUPS"].Rows)
      this.AddAttributeGroup(row);
    foreach (DataRow row in (InternalDataCollectionBase) this.metadataDS.Tables["IMS_ATTR_IN_GROUPS"].Rows)
    {
      int int32_5 = Convert.ToInt32(row["F_GROUP_ID"]);
      int int32_6 = Convert.ToInt32(row["F_ATTRIBUTE_ID"]);
      (this._attributeGroups.GetByID(int32_5) as AttributeGroupItemImpl).attrTypeIdAdd(int32_6);
    }
    foreach (DataRow row in (InternalDataCollectionBase) this.metadataDS.Tables["IMS_POSSIBLE_VALUES"].Rows)
      (this._attributeTypes.GetByID(Convert.ToInt32(row["F_ATTRIBUTE_ID"])) as AttributeTypeItemImpl).addPossibleValue((IAttributePossibleValue) this.AddAttributePosibleValue(row));
    this._relationTypes.Clear();
    foreach (DataRow row in (InternalDataCollectionBase) this.metadataDS.Tables["IMS_RELATION_TYPES"].Rows)
      this.AddRelationType(row);
    foreach (DataRow row in (InternalDataCollectionBase) this.metadataDS.Tables["IMS_ATTR4RELATION_TYPES"].Rows)
    {
      int int32_7 = Convert.ToInt32(row["F_RELATION_TYPE"]);
      int int32_8 = Convert.ToInt32(row["F_ATTRIBUTE_ID"]);
      this._relationTypes.GetByID(int32_7).AddAttrTypeId(int32_8);
    }
    this._commonData.Reload();
    this._lcSteps4Archives.Load();
  }

  private ObjectTypeItemImpl AddObjectType(DataRow dr)
  {
    IDBRelationType relationType = this._session.GetRelationType(Convert.ToInt32(dr["F_DEFAULT_RELATION"]));
    IDBLCSchema lcSchema = this._session.GetLCSchema(Convert.ToInt32(dr["F_SCHEMA_ID"]));
    ObjectTypeItemImpl objectTypeItemImpl = new ObjectTypeItemImpl((IDataWriterProxy) this, Convert.ToInt32(dr["F_OBJECT_TYPE"]), (relationType as IDBGuid).GUID, new Guid(Convert.ToString(dr["F_GUID"])), Convert.ToString(dr["F_OBJ_TYPE_NAME"]).Trim(), Convert.ToString(dr["F_SHORT_NAME"]).Trim(), Convert.ToString(dr["F_OBJ_NAME"]).Trim(), Convert.ToString(dr["F_AREA_ID"]).Trim(), (ObjectVersionModes) Convert.ToInt16(dr["F_VERSIONABLE"]), Convert.ToInt32(dr["F_CAPTION_ATTRIBUTE"]), (lcType) Convert.ToInt16(dr["F_PUBLIC_LC"]), Convert.ToInt32(dr["F_DEL_TIME"]), lcSchema.GUID, Convert.ToInt32(dr["F_ANY_ATTRIBUTES"]) > 0, Convert.ToString(dr["F_NOTE"]).Trim(), dr["F_ICON"] == DBNull.Value || dr["F_ICON"] == null ? (byte[]) null : dr["F_ICON"] as byte[]);
    this._objectTypes.Add((IObjectTypeItem) objectTypeItemImpl);
    return objectTypeItemImpl;
  }

  private AttributeTypeItemImpl AddAttributeType(DataRow dr)
  {
    AttributeTypeItemImpl attributeTypeItemImpl = new AttributeTypeItemImpl((IDataWriterProxy) this, Convert.ToInt32(dr["F_ATTRIBUTE_ID"]), Convert.ToInt32(dr["F_ATTRIBUTE_TYPE"]), new Guid(Convert.ToString(dr["F_GUID"])), Convert.ToString(dr["F_NAME"]).Trim(), Convert.ToString(dr["F_SHORT_NAME"]).Trim(), Convert.ToString(dr["F_ALIAS"]).Trim(), (MultiValueModes) Convert.ToInt32(dr["F_MULTIPLE_VALUED"]), Convert.ToInt32(dr["F_SIZE_TYPE"]), dr["F_DEFAULT_VALUE"]);
    this._attributeTypes.Add((IAttributeTypeItem) attributeTypeItemImpl);
    return attributeTypeItemImpl;
  }

  private AttributeTypeItemImpl AddAttributeType(
    int attrTypeID,
    int attrValueType,
    Guid attrGuid,
    string attrName,
    string shortName,
    string alias,
    MultiValueModes multiValueMode,
    int maxSize,
    object defValue)
  {
    AttributeTypeItemImpl attributeTypeItemImpl = new AttributeTypeItemImpl((IDataWriterProxy) this, attrTypeID, attrValueType, attrGuid, attrName, shortName, alias, multiValueMode, maxSize, defValue);
    this._attributeTypes.Add((IAttributeTypeItem) attributeTypeItemImpl);
    return attributeTypeItemImpl;
  }

  private AttributeGroupItemImpl AddAttributeGroup(DataRow dr)
  {
    AttributeGroupItemImpl attributeGroupItemImpl = new AttributeGroupItemImpl((IDataWriterProxy) this, Convert.ToInt32(dr["F_GROUP_ID"]), new Guid(Convert.ToString(dr["F_GUID"])), Convert.ToString(dr["F_GROUP_NAME"]).Trim(), Convert.ToString(dr["F_NOTE"]).Trim());
    this._attributeGroups.Add((IAttributeGroupItem) attributeGroupItemImpl);
    return attributeGroupItemImpl;
  }

  private RelationTypeItemImpl AddRelationType(DataRow dr)
  {
    RelationTypeItemImpl relationTypeItemImpl = new RelationTypeItemImpl((IDataWriterProxy) this, Convert.ToInt32(dr["F_RELATION_TYPE"]), new Guid(Convert.ToString(dr["F_GUID"])), Convert.ToString(dr["F_DESCRIPTION"]));
    this._relationTypes.Add((IRelationTypeItem) relationTypeItemImpl);
    return relationTypeItemImpl;
  }

  private AttributePossibleValueImpl AddAttributePosibleValue(DataRow dr)
  {
    int int32 = dr.IsNull("F_INLIST_ID") ? 0 : Convert.ToInt32(dr["F_INLIST_ID"]);
    string str1 = dr.IsNull("F_DESCRIPTION") ? string.Empty : Convert.ToString(dr["F_DESCRIPTION"]);
    string str2 = dr.IsNull("F_STRING_VALUE") ? (string) null : Convert.ToString(dr["F_STRING_VALUE"]);
    int num1 = dr.IsNull("F_INTEGER_VALUE") ? int.MinValue : Convert.ToInt32(dr["F_INTEGER_VALUE"]);
    double num2 = dr.IsNull("F_DOUBLE_VALUE") ? double.MinValue : Convert.ToDouble(dr["F_DOUBLE_VALUE"]);
    DateTime dateTime = dr.IsNull("F_DATE_VALUE") ? DateTime.MinValue : Convert.ToDateTime(dr["F_DATE_VALUE"]);
    string descr = str1;
    string valueStr = str2;
    int valueInt = num1;
    double valueDbl = num2;
    DateTime valueDat = dateTime;
    return new AttributePossibleValueImpl(int32, descr, valueStr, valueInt, valueDbl, valueDat);
  }

  private void AddToImportingList(int category, string id, string guid)
  {
    DataTable table = this.importingDS.Tables[BriefcaseConsts.XmlMetadataRecordTag];
    DataRow row = table.NewRow();
    row[BriefcaseConsts.XmlCategoryTag] = (object) category;
    row[BriefcaseConsts.XmlIdTag] = (object) id;
    row[BriefcaseConsts.XmlExternalTag] = (object) guid;
    table.Rows.Add(row);
  }

  private void AddObjTypeToImportingList(string id, string guid)
  {
    if (this.impListObjectsId.Contains(id) || this.impListObjectsGuid.Contains(guid))
      return;
    this.AddToImportingList(4, id, guid);
    this.impListObjectsId.Add(id);
    this.impListObjectsGuid.Add(guid);
  }

  internal void AddAttrTypeToImportingList(string id, string guid)
  {
    if (this.impListAttributesId.Contains(id) || this.impListAttributesGuid.Contains(guid))
      return;
    this.AddToImportingList(3, id, guid);
    this.impListAttributesId.Add(id);
    this.impListAttributesGuid.Add(guid);
  }

  private void AddAttrGroupToImportingList(string id, string guid)
  {
    if (this.impListAttrGroupsId.Contains(id) || this.impListAttrGroupsGuid.Contains(guid))
      return;
    this.AddToImportingList(12, id, guid);
    this.impListAttrGroupsId.Add(id);
    this.impListAttrGroupsGuid.Add(guid);
  }

  internal void AddRelTypeToImportingList(string id, string guid)
  {
    if (this.impListRelationsId.Contains(id) || this.impListRelationsGuid.Contains(guid))
      return;
    this.AddToImportingList(5, id, guid);
    this.impListRelationsId.Add(id);
    this.impListRelationsGuid.Add(guid);
  }

  public Guid NewPumpGuid()
  {
    Guid guid = Guid.NewGuid();
    if (this.GuidGenerateMode)
      guid = new Guid("cae0" + guid.ToString().Substring(4));
    return guid;
  }

  public IAttributeTypeItem CreateAttributeType(
    string name,
    string shortName,
    string alias,
    string note,
    FieldTypes fieldType,
    string defVal,
    MultiValueModes multiMode,
    ComputeValueModes computeMode,
    UniqueValueModes uniqueMode,
    long size,
    int level,
    string formula,
    string language,
    Guid guid,
    string area,
    bool isContent,
    short inView,
    AttributeOptions options,
    string mask,
    int groupID)
  {
    name = name.Trim();
    if (this._attributeTypes.ExistsByGuid(guid))
      return this._attributeTypes.GetByGuid(guid);
    if (this._attributeTypes.ExistsByName(name))
      return this._attributeTypes.GetByName(name);
    if (fieldType == FieldTypes.ftDouble || fieldType == FieldTypes.ftInteger || fieldType == FieldTypes.ftBoolean)
      size = 0L;
    if (fieldType == FieldTypes.ftString && size <= 0L)
      size = (long) Consts.MaxStringSize;
    DataTable table = this.metadataDS.Tables["IMS_ATTRIBUTES"];
    DataRow dataRow = table.NewRow();
    string guid1 = guid.ToString();
    int attrTypeId = this._attributeTypes.GenNextID();
    dataRow["F_ATTRIBUTE_ID"] = (object) attrTypeId;
    dataRow["F_NAME"] = (object) name;
    dataRow["F_SHORT_NAME"] = (object) shortName;
    dataRow["F_ALIAS"] = (object) alias;
    dataRow["F_NOTE"] = (object) note;
    dataRow["F_ATTRIBUTE_TYPE"] = (object) (int) fieldType;
    dataRow["F_DEFAULT_VALUE"] = (object) defVal;
    dataRow["F_MULTIPLE_VALUED"] = (object) (short) multiMode;
    dataRow["F_COMPUTED"] = (object) (short) computeMode;
    dataRow["F_UNIQUE"] = (object) (short) uniqueMode;
    dataRow["F_SIZE_TYPE"] = (object) size;
    dataRow["F_LEVEL_ID"] = (object) level;
    dataRow["F_FORMULA"] = (object) formula;
    dataRow["F_LANGUAGE_ID"] = (object) language;
    dataRow["F_GUID"] = (object) guid1;
    dataRow["F_AREA_ID"] = (object) area;
    dataRow["F_INVIEW"] = (object) inView;
    dataRow["F_CONTENT"] = (object) (isContent ? 1 : 0);
    dataRow["F_OPTIONS"] = (object) (int) options;
    dataRow["F_MASK"] = (object) mask;
    dataRow["F_OBJECT_GUID"] = (object) string.Empty;
    dataRow["F_MASTER_ID"] = (object) 0;
    dataRow["F_SOURCE_ID"] = (object) 0;
    table.Rows.Add(dataRow);
    this.AddAttrTypeToImportingList(attrTypeId.ToString(), guid1);
    if (groupID != 0)
      this.CreateLinkAttrTypeToGroup(attrTypeId, guid, groupID);
    return (IAttributeTypeItem) this.AddAttributeType(dataRow);
  }

  public void CreateLinkAttrTypeToRelType(
    int attrTypeId,
    int relTypeId,
    RequiredModes requiredMod,
    string validationRule,
    ComputeValueModes computeMode,
    string formula,
    string defaultValue,
    short inViewMode,
    bool isContent,
    AttributeOptions options,
    string mask,
    int masterId,
    int sourceId)
  {
    if (!this._relationTypes.ExistsById(relTypeId) || !this._attributeTypes.ExistsById(attrTypeId))
      return;
    IRelationTypeItem byId1 = this._relationTypes.GetByID(relTypeId);
    IAttributeTypeItem byId2 = this._attributeTypes.GetByID(attrTypeId);
    if (new List<int>((IEnumerable<int>) byId1.AttrTypeIDs).Contains(attrTypeId))
      return;
    DataTable table = this.metadataDS.Tables["IMS_ATTR4RELATION_TYPES"];
    DataRow row = table.NewRow();
    row["F_ATTRIBUTE_ID"] = (object) attrTypeId;
    row["F_RELATION_TYPE"] = (object) relTypeId;
    row["F_REQUIRED"] = (object) (short) requiredMod;
    row["F_VALIDATION_RULE"] = (object) validationRule;
    row["F_COMPUTED"] = (object) (short) computeMode;
    row["F_FORMULA"] = (object) formula;
    row["F_DEFAULT_VALUE"] = (object) defaultValue;
    row["F_INVIEW"] = (object) inViewMode;
    row["F_CONTENT"] = (object) (isContent ? 1 : 0);
    row["F_OPTIONS"] = (object) (int) options;
    row["F_MASK"] = (object) mask;
    row["F_MASTER_ID"] = (object) masterId;
    row["F_SOURCE_ID"] = (object) sourceId;
    table.Rows.Add(row);
    byId1.AddAttrTypeId(attrTypeId);
    this.AddRelTypeToImportingList(byId1.ID.ToString(), byId1.GUID.ToString());
    this.AddAttrTypeToImportingList(byId2.ID.ToString(), byId2.GUID.ToString());
  }

  public IObjectTypeItem CreateObjectType(
    Guid parentID,
    string name,
    string objectName,
    string shortName,
    ObjectVersionModes versionable,
    string note,
    Guid defRelId,
    Guid guid,
    string area,
    int captionAttribute,
    bool anyAttributes,
    lcType publicLc,
    int delTime,
    Guid shemaId,
    byte[] icon)
  {
    if (this._objectTypes.ExistsByGuid(guid))
      return this._objectTypes.GetByGuid(guid);
    if (this._objectTypes.ExistsByName(name))
      return this._objectTypes.GetByName(name);
    IRelationTypeItem byGuid1 = this._relationTypes.GetByGuid(defRelId);
    IDBLCSchema lcSchema = this._session.GetLCSchema(shemaId, false);
    DataTable table1 = this.metadataDS.Tables["IMS_OBJECT_TYPES"];
    DataRow dataRow1 = table1.NewRow();
    string guid1 = guid.ToString();
    int childID = this._objectTypes.GenNextID();
    dataRow1["F_OBJECT_TYPE"] = (object) childID;
    dataRow1["F_OBJ_TYPE_NAME"] = (object) name;
    dataRow1["F_OBJ_NAME"] = (object) objectName;
    dataRow1["F_ICON"] = (object) icon;
    dataRow1["F_VERSIONABLE"] = (object) (short) versionable;
    dataRow1["F_NOTE"] = (object) note;
    dataRow1["F_DEFAULT_RELATION"] = (object) (byGuid1 != null ? byGuid1.ID : -1);
    dataRow1["F_GUID"] = (object) guid1;
    dataRow1["F_AREA_ID"] = (object) area;
    dataRow1["F_CAPTION_ATTRIBUTE"] = (object) captionAttribute;
    dataRow1["F_ANY_ATTRIBUTES"] = (object) (short) (anyAttributes ? 1 : 0);
    dataRow1["F_PUBLIC_LC"] = (object) (short) publicLc;
    dataRow1["F_SHORT_NAME"] = (object) shortName;
    dataRow1["F_DEL_TIME"] = (object) delTime;
    table1.Rows.Add(dataRow1);
    dataRow1["F_OPTIONS"] = (object) 0;
    dataRow1["F_SCHEMA_ID"] = (object) (lcSchema != null ? lcSchema.SchemaID : -1);
    this.AddObjTypeToImportingList(childID.ToString(), guid1);
    Guid guid2;
    if (parentID != Guid.Empty)
    {
      IObjectTypeItem byGuid2 = this._objectTypes.GetByGuid(parentID);
      if (!byGuid2.ChildExists(childID))
      {
        DataTable table2 = this.metadataDS.Tables["IMS_OBJTYPES_TREE"];
        DataRow row = table2.NewRow();
        row["F_PARENT_ID"] = (object) byGuid2.ID;
        row["F_OBJECT_TYPE"] = (object) childID;
        table2.Rows.Add(row);
        (byGuid2 as ObjectTypeItemImpl).childIdAdd(childID);
        string id = byGuid2.ID.ToString();
        guid2 = byGuid2.GUID;
        string guid3 = guid2.ToString();
        this.AddObjTypeToImportingList(id, guid3);
      }
    }
    if (this.metadataDS.Tables["IMS_LC_SCHEMAS"].Rows.Find((object) lcSchema.SchemaID) == null)
    {
      DataRow row = this.metadataDS.Tables["IMS_LC_SCHEMAS"].NewRow();
      row["F_SCHEMA_ID"] = (object) lcSchema.SchemaID;
      row["F_NAME"] = (object) lcSchema.Name;
      row["F_NOTE"] = (object) lcSchema.Note;
      DataRow dataRow2 = row;
      guid2 = lcSchema.GUID;
      string str = guid2.ToString();
      dataRow2["F_GUID"] = (object) str;
      row["F_AREA_ID"] = (object) lcSchema.SchemaProperties.AreaID;
      row["F_OPTIONS"] = (object) lcSchema.Options;
      row["F_DEFAULT"] = (object) lcSchema.IsDefaultSchema;
      row["F_DRAW_DATA"] = (object) lcSchema.DrawData;
      this.metadataDS.Tables["IMS_LC_SCHEMAS"].Rows.Add(row);
      this.metadataDS.Tables["IMS_LC_SCHEMAS"].AcceptChanges();
    }
    ObjectTypeItemImpl objectType = this.AddObjectType(dataRow1);
    objectType.ParentID = parentID;
    return (IObjectTypeItem) objectType;
  }

  public void CreateLinkAttrTypeToObjType(
    int attrTypeId,
    int objTypeId,
    bool isPublic,
    RequiredModes requiredMod,
    string validationRule,
    ComputeValueModes computeMode,
    string formula,
    UniqueValueModes uniqueMode,
    int level,
    string defaultValue,
    OptimizationModes inViewMode,
    bool isContent,
    AttributeOptions options,
    string mask,
    int masterId,
    int sourceId)
  {
    if (!this._objectTypes.ExistsById(objTypeId) || !this._attributeTypes.ExistsById(attrTypeId))
      return;
    IObjectTypeItem byId1 = this._objectTypes.GetByID(objTypeId);
    IAttributeTypeItem byId2 = this._attributeTypes.GetByID(attrTypeId);
    if (byId1.AttrTypeExists(attrTypeId))
      return;
    DataTable table = this.metadataDS.Tables["IMS_ATTR4OBJ_TYPES"];
    DataRow row = table.NewRow();
    row["F_ATTRIBUTE_ID"] = (object) attrTypeId;
    row["F_OBJECT_TYPE"] = (object) objTypeId;
    row["F_PUBLIC"] = (object) (isPublic ? 1 : 0);
    row["F_REQUIRED"] = (object) (short) requiredMod;
    row["F_VALIDATION_RULE"] = (object) validationRule;
    row["F_COMPUTED"] = (object) (short) computeMode;
    row["F_FORMULA"] = (object) formula;
    row["F_LEVEL_ID"] = (object) level;
    row["F_UNIQUE"] = (object) (short) uniqueMode;
    row["F_DEFAULT_VALUE"] = (object) defaultValue;
    row["F_INVIEW"] = (object) (short) inViewMode;
    row["F_CONTENT"] = (object) (isContent ? 1 : 0);
    row["F_OPTIONS"] = (object) (int) options;
    row["F_MASK"] = (object) mask;
    row["F_MASTER_ID"] = (object) masterId;
    row["F_SOURCE_ID"] = (object) sourceId;
    table.Rows.Add(row);
    byId1.AddAttrTypeId(attrTypeId);
    this.AddObjTypeToImportingList(byId1.ID.ToString(), byId1.GUID.ToString());
    this.AddAttrTypeToImportingList(byId2.ID.ToString(), byId2.GUID.ToString());
  }

  public void CreateLinkAttrTypeToGroup(int attrTypeId, Guid attrTypeGuid, int attrGroupId)
  {
    if (!this._attributeGroups.ExistsById(attrGroupId))
      return;
    IAttributeGroupItem byId = this._attributeGroups.GetByID(attrGroupId);
    if (byId.AttrTypeExists(attrTypeId))
      return;
    DataTable table = this.metadataDS.Tables["IMS_ATTR_IN_GROUPS"];
    DataRow row = table.NewRow();
    row["F_GROUP_ID"] = (object) attrGroupId;
    row["F_ATTRIBUTE_ID"] = (object) attrTypeId;
    table.Rows.Add(row);
    (byId as AttributeGroupItemImpl).attrTypeIdAdd(attrTypeId);
    this.AddAttrGroupToImportingList(byId.ID.ToString(), byId.GUID.ToString());
    this.AddAttrTypeToImportingList(attrTypeId.ToString(), attrTypeGuid.ToString());
  }

  public IAttributeGroupItem CreateAttributeGroup(
    string groupName,
    Guid groupGuid,
    string note,
    string area,
    string lang)
  {
    DataTable table = this.metadataDS.Tables["IMS_ATTR_GROUPS"];
    DataRow dataRow = table.NewRow();
    string guid = groupGuid.ToString();
    int num = this._attributeGroups.GenNextID();
    dataRow["F_GROUP_ID"] = (object) num;
    dataRow["F_GROUP_NAME"] = (object) groupName;
    dataRow["F_NOTE"] = (object) note;
    dataRow["F_AREA_ID"] = (object) area;
    dataRow["F_LANGUAGE_ID"] = (object) lang;
    dataRow["F_GUID"] = (object) guid;
    dataRow["F_PARENT_ID"] = (object) -1;
    table.Rows.Add(dataRow);
    this.AddAttrGroupToImportingList(num.ToString(), guid);
    return (IAttributeGroupItem) this.AddAttributeGroup(dataRow);
  }

  public void CreateAttributePossibleValue(int attrId, IAttributePossibleValue possibleValue)
  {
    IAttributeTypeItem byId = this._attributeTypes.ExistsById(attrId) ? this._attributeTypes.GetByID(attrId) : (IAttributeTypeItem) null;
    if (byId == null)
      return;
    DataTable table = this.metadataDS.Tables["IMS_POSSIBLE_VALUES"];
    DataRow dataRow = table.NewRow();
    dataRow["F_ATTRIBUTE_ID"] = (object) attrId;
    dataRow["F_OBJECT_TYPE"] = (object) -1;
    dataRow["F_RELATION_TYPE"] = (object) -1;
    dataRow["F_INLIST_ID"] = (object) possibleValue.InListId;
    int num = possibleValue.ValueInteger;
    if (!num.Equals(int.MinValue))
      dataRow["F_INTEGER_VALUE"] = (object) possibleValue.ValueInteger;
    if (possibleValue.ValueString != null)
      dataRow["F_STRING_VALUE"] = (object) possibleValue.ValueString;
    if (!possibleValue.ValueDouble.Equals(double.MinValue))
      dataRow["F_DOUBLE_VALUE"] = (object) possibleValue.ValueDouble;
    if (!possibleValue.ValueDateTime.Equals(DateTime.MinValue))
      dataRow["F_DATE_VALUE"] = (object) possibleValue.ValueDateTime;
    dataRow["F_DESCRIPTION"] = (object) possibleValue.Description;
    table.Rows.Add(dataRow);
    table.AcceptChanges();
    (byId as AttributeTypeItemImpl).addPossibleValue((IAttributePossibleValue) this.AddAttributePosibleValue(dataRow));
    num = byId.ID;
    this.AddAttrTypeToImportingList(num.ToString(), byId.GUID.ToString());
    if (byId.MultiValueMode != MultiValueModes.SingleValue && byId.MultiValueMode != MultiValueModes.MultiValues || byId.ExistsInBase)
      return;
    MultiValueModes multiValueModes = byId.MultiValueMode == MultiValueModes.SingleValue ? MultiValueModes.SingleValueFromList : MultiValueModes.MultiValuesFromList;
    byId.MultiValueMode = multiValueModes;
    this.metadataDS.Tables["IMS_ATTRIBUTES"].Rows.Find((object) byId.ID)["F_MULTIPLE_VALUED"] = (object) (int) multiValueModes;
    this.metadataDS.Tables["IMS_ATTRIBUTES"].AcceptChanges();
  }

  public IObjectTypeItemList ObjectTypes => (IObjectTypeItemList) this._objectTypes;

  public IAttributeTypeItemList AttributeTypes => (IAttributeTypeItemList) this._attributeTypes;

  public IAttributeGroupItemList AttributeGroups => (IAttributeGroupItemList) this._attributeGroups;

  public IRelationTypeItemList RelationTypes => (IRelationTypeItemList) this._relationTypes;

  public bool MetadataLoadFromServer() => this.LoadFromServer();

  public bool MetadataSaveToServer()
  {
    this.MetadataApplyChanges();
    return this.SaveToServer() && this.LoadFromServer();
  }

  public bool MetadataApplyChanges()
  {
    foreach (DataTable table in (InternalDataCollectionBase) this.metadataDS.Tables)
      table.AcceptChanges();
    foreach (DataTable table in (InternalDataCollectionBase) this.importingDS.Tables)
      table.AcceptChanges();
    return true;
  }

  public bool MetadataEditTableFieldValue(
    string tableName,
    object key,
    string fieldName,
    object value)
  {
    this.metadataDS.Tables[tableName].Rows.Find(key)[fieldName] = value;
    return true;
  }

  public bool Login() => this.Login(3);

  public IUserSession UserSession => this._session;

  public long UserID
  {
    get
    {
      if (this._userID == 0L)
        this._userID = this._session.IdentHelper.SysdbaID;
      return this._userID;
    }
  }

  public Guid UserGUID
  {
    get
    {
      if (this._userGUID == Guid.Empty)
        this._userGUID = new Guid("cad00016-306c-11d8-b4e9-00304f19f545");
      return this._userGUID;
    }
  }

  public int GetLCStepForArchiveType(int archiveID, int objectType, int docStateID)
  {
    return this._lcSteps4Archives.GetLCStep(archiveID, objectType, docStateID);
  }

  public int GetLCLevel(int stepID)
  {
    int num;
    return this._lcStepsToLevels == null || !this._lcStepsToLevels.TryGetValue(stepID, out num) ? 0 : num;
  }

  public long GetNextID(string tableName) => this.dbImporter.GetNextID(tableName);

  private class CommonDataImpl
  {
    private PhysicalValuesImpl physicalValues;
    private MeasuresImpl measures;
    private AttributeTypeToCreateList attrTypeTCL;
    private AttributeGroupToCreateList attrGroupTCL;
    private ObjectTypeToCreateList objTypeTCL;
    private UsersImpl users;

    public CommonDataImpl()
    {
      this.physicalValues = new PhysicalValuesImpl();
      this.measures = new MeasuresImpl();
      this.attrTypeTCL = new AttributeTypeToCreateList();
      this.attrGroupTCL = new AttributeGroupToCreateList();
      this.objTypeTCL = new ObjectTypeToCreateList();
      this.users = new UsersImpl();
      ServicesManager.ServiceContainer.AddService(typeof (IPhysicalValues), (object) this.physicalValues);
      ServicesManager.ServiceContainer.AddService(typeof (IMeasures), (object) this.measures);
      ServicesManager.ServiceContainer.AddService(typeof (IAttributeTypeToCreateList), (object) this.attrTypeTCL);
      ServicesManager.ServiceContainer.AddService(typeof (IAttributeGroupToCreateList), (object) this.attrGroupTCL);
      ServicesManager.ServiceContainer.AddService(typeof (IObjectTypeToCreateList), (object) this.objTypeTCL);
    }

    public void Reload()
    {
      this.physicalValues.Reload();
      this.measures.Reload();
      this.attrTypeTCL.Reload();
      this.attrGroupTCL.Reload();
      this.objTypeTCL.Reload();
      this.users.Reload();
    }
  }
}

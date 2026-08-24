// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Entity
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace Intermech.ImpExp.TechCard;

[Serializable]
public class Entity : ICloneable
{
  public const string TableName = "TC_ENTITY";
  private string _code = string.Empty;
  private string _record = string.Empty;
  private int _recordID;
  private int _tp;
  private string _type = "S";
  private string _name = string.Empty;
  private bool _blank;
  private bool _askUser;
  private bool _protected;
  private int _tag;
  private string _masterCode = string.Empty;
  private bool _needSave;
  private string _oleClassName = string.Empty;
  private int _access;
  private int _flags;
  private bool _isMasterAttr;
  private int[] _productions = new int[0];
  private EntityReference _entityReference;
  private bool _lockedSettings;
  private bool _isPermisibleAttr2TypeObj = true;
  private EntitySetting _settings;
  [NonSerialized]
  internal IAttributeTypeItem _pumpToAttrType;
  private static int idx_F_CODE;
  private static int idx_F_RECORD;
  private static int idx_F_RECORDID;
  private static int idx_F_TP;
  private static int idx_F_TYPE;
  private static int idx_F_NAME;
  private static int idx_F_BLANK;
  private static int idx_F_ASKUSER;
  private static int idx_F_PROTECTED;
  private static int idx_F_TAG;
  private static int idx_F_MASTERCODE;
  private static int idx_F_NEEDSAVE;
  private static int idx_F_OLECLASSNAME;
  private static int idx_F_ACCESS;
  private static int idx_F_FLAGS;
  private const string F_CODE = "F_CODE";
  private const string F_RECORD = "F_RECORD";
  private const string F_RECORDID = "F_RECORDID";
  private const string F_TP = "F_TP";
  private const string F_TYPE = "F_TYPE";
  private const string F_NAME = "F_NAME";
  private const string F_BLANK = "F_BLANK";
  private const string F_ASKUSER = "F_ASKUSER";
  private const string F_PROTECTED = "F_PROTECTED";
  private const string F_TAG = "F_TAG";
  private const string F_MASTERCODE = "F_MASTERCODE";
  private const string F_NEEDSAVE = "F_NEEDSAVE";
  private const string F_OLECLASSNAME = "F_OLECLASSNAME";
  private const string F_ACCESS = "F_ACCESS";
  private const string F_FLAGS = "F_FLAGS";

  private Guid GetPumpToGuid()
  {
    Guid pumpToGuid = Guid.Empty;
    if (this.Settings == null || this.Settings.Properties == null || this.Settings.Properties.Status == EntityPumpStatus.NotPump || this.Settings.Properties.Status == EntityPumpStatus.None)
      return pumpToGuid;
    if (this.Settings.PumpTo is Guid)
      pumpToGuid = (Guid) this.Settings.PumpTo;
    if (!pumpToGuid.Equals(Guid.Empty))
      return pumpToGuid;
    if (TechcardConsts.TechcardCommon.Code2AttributeGuid == null)
      return Guid.Empty;
    TechcardConsts.TechcardCommon.Code2AttributeGuid.TryGetValue(this.Code, out pumpToGuid);
    return pumpToGuid;
  }

  private IAttributeTypeItem GetPumpToAttrType()
  {
    if (this._pumpToAttrType != null)
      return this._pumpToAttrType;
    IMetadataInfo imdi = TechcardConsts.Plugin != null ? TechcardConsts.Plugin.Imdi : (IMetadataInfo) null;
    if (imdi == null)
      return (IAttributeTypeItem) null;
    Guid pumpToGuid = this.GetPumpToGuid();
    if (!pumpToGuid.Equals(Guid.Empty))
      this._pumpToAttrType = imdi.AttributeTypes.GetByGuid(pumpToGuid);
    return this._pumpToAttrType;
  }

  private int GetPumpToAttrTypeId()
  {
    IAttributeTypeItem pumpToAttrType = this.GetPumpToAttrType();
    return pumpToAttrType == null ? 0 : pumpToAttrType.ID;
  }

  internal void InitializeSetting(IEnumerable<Entity> entCollection, bool initMasterAttr = false)
  {
    EntityReference entityReference;
    Entity entity;
    if (initMasterAttr && TechPumpData.Entities.EntityRefDataList.TryGetValue(this.Code, out entityReference) && entityReference != null && (!TechPumpData.Entities.EntitiesList.TryGetValue(entityReference.MasterCode, out entity) || entity == null ? 1 : (!(entity.Type != "I") ? 1 : 0)) != 0)
    {
      this.EntityReference = entityReference;
      this.IsMasterAttr = entityReference.Code == entityReference.MasterCode;
    }
    this.Settings.InitializeProperties(this, entCollection);
  }

  internal void InitializeProduction(EntityProductionList entProdlist)
  {
    List<int> intList;
    if (entProdlist == null || !entProdlist.TryGetValue(this.Code, out intList))
      return;
    this._productions = intList.ToArray();
  }

  public Entity() => this._settings = new EntitySetting(this);

  public Entity(string code, string name)
    : this(code, name, string.Empty, 0)
  {
  }

  public Entity(string code, string name, string record, int recordId)
    : this()
  {
    this._code = code;
    this._name = name;
    this._record = record;
    this._recordID = recordId;
  }

  public bool IsMeasureAtribute()
  {
    if (this.Settings == null || this.Settings.Properties == null)
      return false;
    switch (this.Settings.PumpMode)
    {
      case EntityPumModes.NewAttr:
        return this.Settings.Properties.FieldType == FieldTypes.ftMeasured;
      case EntityPumModes.ExistAttr:
        Guid guid1 = this.Settings.PumpTo as Guid? ?? Guid.Empty;
        if (guid1 != Guid.Empty)
        {
          if (TechcardConsts.Plugin != null)
          {
            if (TechcardConsts.Plugin.Imdi.AttributeTypes.ExistsByGuid(guid1))
              return TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid(guid1).AttrValueType == 13;
            break;
          }
          IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(guid1);
          return attributeType != null && attributeType.FieldType == FieldTypes.ftMeasured;
        }
        break;
      case EntityPumModes.ExistEntity:
        List<Entity> entitySettRefUpList = EntityHelper.GetEntitySett_RefUpList(this.Settings.PumpTo as Entity);
        if (entitySettRefUpList != null && entitySettRefUpList.Count != 0)
        {
          Entity entity = entitySettRefUpList[0];
          if (entity != null)
          {
            bool flag = entity.IsMeasureAtribute();
            if (!flag && entity.Settings.PumpMode == EntityPumModes.ExistEntity)
            {
              Guid guid2 = entity.Settings.PumpTo is Guid ? (Guid) entity.Settings.PumpTo : Guid.Empty;
              if (guid2 != Guid.Empty)
                flag = TechcardConsts.Plugin.Imdi.AttributeTypes.ExistsByGuid(guid2) && TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid(guid2).AttrValueType == 13;
            }
            return flag;
          }
          break;
        }
        break;
    }
    return false;
  }

  public override string ToString()
  {
    return this.Settings == null || this.Settings.Properties == null ? this.Code : $"{this.Code} ({this.Settings.Properties.Name})";
  }

  public override bool Equals(object other)
  {
    return other is Entity entity ? this.Code == entity.Code : this == other;
  }

  public override int GetHashCode() => this.Code.GetHashCode();

  public Entity Clone()
  {
    Entity entity = new Entity()
    {
      _access = this.Access,
      _askUser = this.AskUser,
      _blank = this.Blank,
      _code = this.Code,
      _flags = this.Flags,
      _masterCode = this.MasterCode,
      _name = this.Name,
      _needSave = this.NeedSave,
      _oleClassName = this.OleClassName,
      _protected = this.Protected,
      _record = this.Record,
      _recordID = this.RecordID,
      _tag = this.Tag,
      _tp = this.TP,
      _type = this.Type,
      _isMasterAttr = this._isMasterAttr,
      _isPermisibleAttr2TypeObj = this._isPermisibleAttr2TypeObj,
      _lockedSettings = this._lockedSettings,
      _entityReference = this.EntityReference,
      _productions = this.Productions,
      _settings = this.Settings.Clone()
    };
    entity._settings._entity = entity;
    return entity;
  }

  object ICloneable.Clone() => (object) this.Clone();

  public static void ParseSchema(Dictionary<string, int> schema)
  {
    Entity.idx_F_CODE = schema["F_CODE"];
    Entity.idx_F_RECORD = schema["F_RECORD"];
    Entity.idx_F_RECORDID = schema["F_RECORDID"];
    Entity.idx_F_TP = schema["F_TP"];
    Entity.idx_F_TYPE = schema["F_TYPE"];
    Entity.idx_F_NAME = schema["F_NAME"];
    Entity.idx_F_BLANK = schema["F_BLANK"];
    Entity.idx_F_ASKUSER = schema["F_ASKUSER"];
    Entity.idx_F_PROTECTED = schema["F_PROTECTED"];
    Entity.idx_F_TAG = schema["F_TAG"];
    Entity.idx_F_MASTERCODE = schema["F_MASTERCODE"];
    Entity.idx_F_NEEDSAVE = schema["F_NEEDSAVE"];
    Entity.idx_F_OLECLASSNAME = schema["F_OLECLASSNAME"];
    Entity.idx_F_ACCESS = schema["F_ACCESS"];
    Entity.idx_F_FLAGS = schema["F_FLAGS"];
  }

  public static Entity Parse(IDataReader dataReader)
  {
    if (dataReader == null)
      return (Entity) null;
    return new Entity()
    {
      _code = dataReader.IsDBNull(Entity.idx_F_CODE) ? string.Empty : dataReader.GetString(Entity.idx_F_CODE),
      _record = dataReader.IsDBNull(Entity.idx_F_RECORD) ? string.Empty : dataReader.GetString(Entity.idx_F_RECORD),
      _recordID = dataReader.IsDBNull(Entity.idx_F_RECORDID) ? 0 : BasePumpHelper.ToInt32(dataReader[Entity.idx_F_RECORDID]),
      _tp = dataReader.IsDBNull(Entity.idx_F_TP) ? 0 : BasePumpHelper.ToInt32(dataReader[Entity.idx_F_TP]),
      _type = dataReader.IsDBNull(Entity.idx_F_TYPE) ? "S" : dataReader.GetString(Entity.idx_F_TYPE),
      _name = dataReader.IsDBNull(Entity.idx_F_NAME) ? string.Empty : dataReader.GetString(Entity.idx_F_NAME).Trim(),
      _blank = !dataReader.IsDBNull(Entity.idx_F_BLANK) && dataReader.GetString(Entity.idx_F_BLANK).Equals("T"),
      _askUser = !dataReader.IsDBNull(Entity.idx_F_ASKUSER) && dataReader.GetString(Entity.idx_F_ASKUSER).Equals("T"),
      _protected = !dataReader.IsDBNull(Entity.idx_F_PROTECTED) && dataReader.GetString(Entity.idx_F_PROTECTED).Equals("T"),
      _tag = dataReader.IsDBNull(Entity.idx_F_TAG) ? 0 : BasePumpHelper.ToInt32(dataReader[Entity.idx_F_TAG]),
      _masterCode = dataReader.IsDBNull(Entity.idx_F_MASTERCODE) ? string.Empty : dataReader.GetString(Entity.idx_F_MASTERCODE),
      _needSave = !dataReader.IsDBNull(Entity.idx_F_NEEDSAVE) && dataReader.GetString(Entity.idx_F_NEEDSAVE).Equals("T"),
      _oleClassName = dataReader.IsDBNull(Entity.idx_F_OLECLASSNAME) ? string.Empty : dataReader.GetString(Entity.idx_F_OLECLASSNAME),
      _access = dataReader.IsDBNull(Entity.idx_F_ACCESS) ? 0 : BasePumpHelper.ToInt32(dataReader[Entity.idx_F_ACCESS]),
      _flags = dataReader.IsDBNull(Entity.idx_F_FLAGS) ? 0 : BasePumpHelper.ToInt32(dataReader[Entity.idx_F_FLAGS])
    };
  }

  public string Code
  {
    [DebuggerStepThrough] get => this._code;
  }

  public string Record
  {
    [DebuggerStepThrough] get => this._record;
  }

  public int RecordID
  {
    [DebuggerStepThrough] get => this._recordID;
  }

  public int TP
  {
    [DebuggerStepThrough] get => this._tp;
  }

  public string Type
  {
    [DebuggerStepThrough] get => this._type;
    set => this._type = value;
  }

  public string Name
  {
    [DebuggerStepThrough] get => this._name;
  }

  public bool Blank
  {
    [DebuggerStepThrough] get => this._blank;
  }

  public bool AskUser
  {
    [DebuggerStepThrough] get => this._askUser;
  }

  public bool Protected
  {
    [DebuggerStepThrough] get => this._protected;
  }

  public int Tag
  {
    [DebuggerStepThrough] get => this._tag;
  }

  public string MasterCode
  {
    [DebuggerStepThrough] get => this._masterCode;
  }

  public bool NeedSave
  {
    [DebuggerStepThrough] get => this._needSave;
  }

  public string OleClassName
  {
    [DebuggerStepThrough] get => this._oleClassName;
  }

  public int Access
  {
    [DebuggerStepThrough] get => this._access;
  }

  public int Flags
  {
    [DebuggerStepThrough] get => this._flags;
  }

  public bool IsMasterAttr
  {
    [DebuggerStepThrough] get => this._isMasterAttr;
    set => this._isMasterAttr = value;
  }

  public int[] Productions
  {
    [DebuggerStepThrough] get => this._productions;
    set => this._productions = value;
  }

  public EntityReference EntityReference
  {
    [DebuggerStepThrough] get => this._entityReference;
    set => this._entityReference = value;
  }

  public bool LockedSettings
  {
    [DebuggerStepThrough] get => this._lockedSettings;
    set => this._lockedSettings = value;
  }

  public bool IsPermisibleAttr2TypeObj
  {
    [DebuggerStepThrough] get => this._isPermisibleAttr2TypeObj;
    set => this._isPermisibleAttr2TypeObj = value;
  }

  public EntitySetting Settings
  {
    [DebuggerStepThrough] get => this._settings ?? (this._settings = new EntitySetting(this));
  }

  public IAttributeTypeItem PumpToAttrType => this.GetPumpToAttrType();

  public int PumpToAttrTypeID => this.GetPumpToAttrTypeId();
}

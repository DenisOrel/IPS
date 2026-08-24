// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.ItemsToCreate.ObjectTypeToCreate
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.LifeCycles;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Manager.CommonData.ItemsToCreate;

internal sealed class ObjectTypeToCreate : 
  ItemToCreate,
  IObjectTypeToCreate,
  IItemToCreate,
  ICustomTypeDescriptor
{
  private string _shortName;
  private string _instanceName;
  private ParentObjectTypeAttProxy _parentType;
  private LCSchemeAttProxy _lcScheme;
  private RelationTypeAttProxy _relationType;
  private Guid _parentTypeId = Guid.Empty;
  private ObjectVersionModes _versionMode = ObjectVersionModes.SingleVersion;
  private string note = string.Empty;
  private Guid defaultRelationId = Guid.Empty;
  private string area = string.Empty;
  private int captionAttrId;
  private bool anyAttributes = true;
  private lcType lcMode = lcType.lcPublic;
  private int daysToDelete;
  private Guid lcShemaId = Guid.Empty;
  private byte[] icon;

  public ObjectTypeToCreate(
    bool isNew,
    string name,
    string shortName,
    string instanceName,
    Guid guid,
    long sysID)
    : this(isNew, name, shortName, instanceName, guid, sysID, (byte[]) null)
  {
  }

  public ObjectTypeToCreate(
    bool isNew,
    string name,
    string shortName,
    string instanceName,
    Guid guid,
    long sysID,
    byte[] icon)
    : this(isNew, name, shortName, instanceName, guid, sysID, (byte[]) null, ObjectVersionModes.MultiVersion)
  {
    this._shortName = shortName;
    this._instanceName = instanceName == string.Empty ? name : instanceName;
    this.icon = icon;
  }

  public ObjectTypeToCreate(
    bool isNew,
    string name,
    string shortName,
    string instanceName,
    Guid guid,
    long sysID,
    byte[] icon,
    ObjectVersionModes versionable)
    : this(isNew, name, shortName, instanceName, guid, sysID, (byte[]) null, versionable, true, Guid.Empty, Guid.Empty, Guid.Empty)
  {
  }

  public ObjectTypeToCreate(
    bool isNew,
    string name,
    string shortName,
    string instanceName,
    Guid guid,
    long sysID,
    byte[] icon,
    ObjectVersionModes versionable,
    bool anyAttribute,
    Guid LcShemaId,
    Guid defaultRelationID,
    Guid parentTypeId)
    : base(isNew, name, guid, sysID)
  {
    this._shortName = shortName.Length > Consts.MaxShortNameLength ? shortName.Substring(0, Consts.MaxShortNameLength) : shortName;
    this._instanceName = instanceName == string.Empty ? name : instanceName;
    this.icon = icon;
    this._versionMode = versionable;
    this.anyAttributes = anyAttribute;
    this.LcShemaId = LcShemaId;
    this.defaultRelationId = defaultRelationID;
    this._parentTypeId = parentTypeId;
  }

  [DisplayName("Краткое наименование")]
  public string ShortName
  {
    get => this._shortName;
    set
    {
      if (!(this._shortName != value))
        return;
      this._shortName = value;
    }
  }

  [DisplayName("Наименование объекта данного типа")]
  public string InstanceName
  {
    get => this._instanceName;
    set
    {
      if (!(this._instanceName != value))
        return;
      this._instanceName = value;
    }
  }

  [Browsable(false)]
  public Guid ParentTypeId
  {
    get => this._parentTypeId;
    set
    {
      if (!(this._parentTypeId != value))
        return;
      this._parentTypeId = value;
    }
  }

  [DisplayName("Версионность")]
  [TypeConverter(typeof (EnumDescConverter))]
  [DefaultValue(1)]
  public ObjectVersionModes VersionMode
  {
    get => this._versionMode;
    set
    {
      if (this._versionMode == value)
        return;
      this._versionMode = value;
    }
  }

  [DisplayName("Комментарии")]
  public string Note
  {
    get => this.note;
    set
    {
      if (!(this.note != value))
        return;
      this.note = value;
    }
  }

  [Browsable(false)]
  public Guid DefaultRelationId
  {
    get => this.defaultRelationId;
    set
    {
      if (!(this.defaultRelationId != value))
        return;
      this.defaultRelationId = value;
    }
  }

  [Browsable(false)]
  public string Area
  {
    get => this.area;
    set
    {
      if (!(this.area != value))
        return;
      this.area = value;
    }
  }

  [Browsable(false)]
  public int CaptionAttrId
  {
    get => this.captionAttrId;
    set
    {
      if (this.captionAttrId == value)
        return;
      this.captionAttrId = value;
    }
  }

  [TypeConverter(typeof (BooleanConverter))]
  [DisplayName("Любой атрибут")]
  [DefaultValue(true)]
  public bool AnyAttributes
  {
    get => this.anyAttributes;
    set
    {
      if (this.anyAttributes == value)
        return;
      this.anyAttributes = value;
    }
  }

  [Browsable(false)]
  public lcType LcMode
  {
    get => this.lcMode;
    set
    {
      if (this.lcMode == value)
        return;
      this.lcMode = value;
    }
  }

  [Browsable(false)]
  public int DaysToDelete
  {
    get => this.daysToDelete;
    set
    {
      if (this.daysToDelete == value)
        return;
      this.daysToDelete = value;
    }
  }

  [Browsable(false)]
  public Guid LcShemaId
  {
    get => this.lcShemaId;
    set
    {
      if (!(this.lcShemaId != value))
        return;
      this.lcShemaId = value;
    }
  }

  [DisplayName("Иконка")]
  [TypeConverter(typeof (IconConverter))]
  public System.Drawing.Icon Image
  {
    get
    {
      if (this.Icon == null)
        return (System.Drawing.Icon) null;
      using (MemoryStream memoryStream = new MemoryStream(this.Icon))
        return new System.Drawing.Icon((Stream) memoryStream);
    }
    set => this.icon = ArraySrv.IconToArray(value);
  }

  [Browsable(false)]
  public byte[] Icon
  {
    get => this.icon;
    set => this.icon = value;
  }

  [Editor(typeof (ParentObjectTypeEditor), typeof (UITypeEditor))]
  [DisplayName("Родительский тип")]
  public ParentObjectTypeAttProxy ParentType
  {
    get
    {
      if (this._parentType == null)
      {
        if (this._parentTypeId == Guid.Empty)
          this._parentType = new ParentObjectTypeAttProxy();
        else if (ServicesManager.GetService(typeof (IMetadataInfo)) is IMetadataInfo service)
        {
          IObjectTypeItem byGuid1 = service.ObjectTypes.GetByGuid(this._parentTypeId);
          if (byGuid1 == null)
          {
            IObjectTypeToCreate byGuid2 = (ServicesManager.ServiceContainer.GetService(typeof (IObjectTypeToCreateList)) as IObjectTypeToCreateList).GetByGuid(this._parentTypeId);
            this._parentType = new ParentObjectTypeAttProxy(byGuid2.GUID, byGuid2.Name);
          }
          else
            this._parentType = new ParentObjectTypeAttProxy(byGuid1.GUID, byGuid1.Name);
        }
      }
      return this._parentType;
    }
    set
    {
      this._parentType = value;
      this._parentTypeId = value.ObjectType;
    }
  }

  [DisplayName("Схема ЖЦ")]
  [Editor(typeof (LCSchemeEditor), typeof (UITypeEditor))]
  public LCSchemeAttProxy LCScheme
  {
    get
    {
      if (this._lcScheme == null)
      {
        if (this.lcShemaId == Guid.Empty)
        {
          this._lcScheme = new LCSchemeAttProxy();
        }
        else
        {
          IDBLCSchema lcSchema = (ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter).GetUserSession().GetLCSchema(this.lcShemaId);
          this._lcScheme = new LCSchemeAttProxy(lcSchema.GUID, lcSchema.Name);
        }
      }
      return this._lcScheme;
    }
    set
    {
      this._lcScheme = value;
      this.lcShemaId = value.LCScheme;
    }
  }

  [Editor(typeof (RelationTypeEditor), typeof (UITypeEditor))]
  [DisplayName("Тип связи по-умолчанию")]
  public RelationTypeAttProxy RelationType
  {
    get
    {
      if (this._relationType == null)
      {
        if (this.defaultRelationId == Guid.Empty)
          this._relationType = new RelationTypeAttProxy();
        else if (ServicesManager.GetService(typeof (IMetadataInfo)) is IMetadataInfo service)
        {
          IRelationTypeItem byGuid = service.RelationTypes.GetByGuid(this.defaultRelationId);
          this._relationType = new RelationTypeAttProxy(byGuid.GUID, byGuid.Name);
        }
      }
      return this._relationType;
    }
    set
    {
      this._relationType = value;
      this.defaultRelationId = value.RelationType;
    }
  }
}

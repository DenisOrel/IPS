// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.ItemsToCreate.AttributeTypeToCreate
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager.CommonData.ItemsToCreate;

[Serializable]
internal class AttributeTypeToCreate : 
  ItemToCreate,
  IAttributeTypeToCreate,
  IItemToCreate,
  ICustomTypeDescriptor
{
  protected string shortName = string.Empty;
  protected string alias = string.Empty;
  protected FieldTypes fieldType;
  protected long size;
  protected bool hasValueInList;
  protected List<int> valuesListIds = new List<int>(0);
  protected Dictionary<int, string> valuesListMeasureIDs = new Dictionary<int, string>(0);
  protected string defaultValue = string.Empty;
  protected MultiValueModes multiValueMode;
  protected Guid createObjectType = Guid.Empty;
  protected Intermech.ImpExp.Interface.ObjectTypeAttProxy objectType;
  protected AttributeOptions options;

  [ReadOnly(true)]
  [DisplayName("Краткое наименование")]
  public string ShortName
  {
    get => this.shortName;
    set
    {
      if (!(this.shortName != value))
        return;
      this.shortName = value;
    }
  }

  [DisplayName("Псевдоним")]
  [Browsable(false)]
  public string Alias
  {
    get => this.alias;
    set
    {
      if (!(this.alias != value))
        return;
      this.alias = value;
    }
  }

  [DisplayName("Тип")]
  [TypeConverter(typeof (EnumDescConverter))]
  [ReadOnly(true)]
  public FieldTypes FieldType
  {
    get => this.fieldType;
    set
    {
      if (this.fieldType == value)
        return;
      this.fieldType = value;
    }
  }

  [DisplayName("Размер")]
  public string Size4Grid
  {
    get => this.fieldType != FieldTypes.ftString ? string.Empty : Convert.ToString(this.size);
  }

  [Browsable(false)]
  public long Size
  {
    get
    {
      return this.fieldType == FieldTypes.ftString || this.fieldType == FieldTypes.ftMemo || this.fieldType == FieldTypes.ftShortBlob || this.fieldType == FieldTypes.ftObjectLink || this.fieldType == FieldTypes.ftMeasured ? this.size : 0L;
    }
    set
    {
      if (this.size == value)
        return;
      this.size = value;
      if (this.isNew)
        return;
      IMetadataInfo service = ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
      IDBAttributeType attributeType = service.UserSession.GetAttributeType(this.guid);
      if (attributeType.AttributeType != FieldTypes.ftString)
        return;
      attributeType.SizeType = value;
      service.AttributeTypes.GetByGuid(this.guid).MaxSize = Convert.ToInt32(value);
    }
  }

  [DisplayName("Содержит значения из списка")]
  [Browsable(false)]
  public bool HasValueInList => this.hasValueInList;

  [Browsable(false)]
  [DisplayName("Идентификаторы ед.измерений для списков допустимых значений")]
  public Dictionary<int, string> ValuesListMeasureIDs
  {
    get => this.valuesListMeasureIDs;
    set => this.valuesListMeasureIDs = value;
  }

  [DisplayName("Идентификаторы наборов значений")]
  [Browsable(false)]
  public List<int> ValuesListIds => this.valuesListIds;

  [Browsable(true)]
  [DisplayName("Значение по умолчанию")]
  public string DefaultValue
  {
    get => this.defaultValue;
    set
    {
      if (!(this.defaultValue != value))
        return;
      this.defaultValue = value;
    }
  }

  [DisplayName("Список")]
  [TypeConverter(typeof (EnumDescConverter))]
  [ReadOnly(true)]
  public MultiValueModes MultiValueMode
  {
    get => this.multiValueMode;
    set => this.multiValueMode = value;
  }

  [Browsable(false)]
  public Guid CreatedObjectType
  {
    get => this.createObjectType;
    set => this.createObjectType = value;
  }

  [Browsable(true)]
  [TypeConverter(typeof (YesNoConverter))]
  [DisplayName("Локальный атрибут Imbase")]
  public bool LocalImbaseAttribute
  {
    get
    {
      return (this.options & AttributeOptions.LocalImbaseAttribute) == AttributeOptions.LocalImbaseAttribute;
    }
    set
    {
      if (value)
        this.options |= AttributeOptions.LocalImbaseAttribute;
      else
        this.options &= ~AttributeOptions.LocalImbaseAttribute;
    }
  }

  [Editor(typeof (Intermech.ImpExp.Interface.ObjectTypeEditor), typeof (UITypeEditor))]
  [Browsable(true)]
  [DisplayName("Тип создаваемого объекта")]
  public Intermech.ImpExp.Interface.ObjectTypeAttProxy CreateLinkObjType
  {
    get
    {
      if (this.objectType == null)
      {
        if (this.createObjectType == Guid.Empty)
          this.objectType = new Intermech.ImpExp.Interface.ObjectTypeAttProxy();
        else if (ServicesManager.GetService(typeof (IMetadataInfo)) is IMetadataInfo service)
        {
          IObjectTypeItem byGuid1 = service.ObjectTypes.GetByGuid(this.createObjectType);
          if (byGuid1 == null)
          {
            IObjectTypeToCreate byGuid2 = (ServicesManager.ServiceContainer.GetService(typeof (IObjectTypeToCreateList)) as IObjectTypeToCreateList).GetByGuid(this.createObjectType);
            this.objectType = new Intermech.ImpExp.Interface.ObjectTypeAttProxy(byGuid2.GUID, byGuid2.Name);
          }
          else
            this.objectType = new Intermech.ImpExp.Interface.ObjectTypeAttProxy(byGuid1.GUID, byGuid1.Name);
        }
      }
      return this.objectType;
    }
    set
    {
      IObjectTypeToCreate byGuid = (ServicesManager.ServiceContainer.GetService(typeof (IObjectTypeToCreateList)) as IObjectTypeToCreateList).GetByGuid(value.ObjectType);
      if (byGuid.VersionMode == ObjectVersionModes.Abstract)
      {
        int num = (int) MessageBox.Show("Нельзя создать объект абстрактного типа");
      }
      else
      {
        this.objectType = value;
        this.createObjectType = value.ObjectType;
        this.size = byGuid.SystemId;
      }
    }
  }

  public AttributeTypeToCreate(
    bool isNew,
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    long size,
    Guid guid,
    long sysID,
    bool valueInList)
    : this(isNew, name, shortName, alias, fieldType, size, guid, sysID, valueInList, -1)
  {
  }

  public AttributeTypeToCreate(
    bool isNew,
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    long size,
    Guid guid,
    long sysID,
    bool valueInList,
    int valueListID)
    : this(isNew, name, shortName, alias, fieldType, size, guid, sysID, valueInList, -1, string.Empty, MultiValueModes.SingleValue)
  {
  }

  public AttributeTypeToCreate(
    bool isNew,
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    long size,
    Guid guid,
    long sysID,
    bool valueInList,
    int valueListID,
    string defaultValue,
    MultiValueModes multiValueMode)
    : base(isNew, name, guid, sysID)
  {
    this.shortName = shortName;
    this.alias = alias;
    this.fieldType = fieldType;
    switch (fieldType)
    {
      case FieldTypes.ftString:
        this.size = size == 0L ? (long) Consts.MaxStringSize : size;
        break;
      case FieldTypes.ftShortBlob:
      case FieldTypes.ftMemo:
        this.size = size == 0L ? (long) Consts.DefaultShortBlobSize : size;
        break;
      case FieldTypes.ftMeasured:
        if (size == 0L)
        {
          size = -1L;
          break;
        }
        goto default;
      default:
        this.size = size;
        break;
    }
    this.hasValueInList = valueInList;
    this.valuesListIds = new List<int>();
    this.defaultValue = defaultValue;
    this.multiValueMode = multiValueMode;
  }

  [Browsable(false)]
  public AttributeOptions Options
  {
    get => this.options;
    set => this.options = value;
  }

  public void AddValueInListId(int id, string units)
  {
    if (!this.valuesListIds.Contains(id))
    {
      this.valuesListIds.Add(id);
      if (units != string.Empty)
        this.valuesListMeasureIDs.Add(id, units);
    }
    this.hasValueInList = true;
  }

  public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
  {
    PropertyDescriptorCollection properties1 = TypeDescriptor.GetProperties((object) this, attributes, true);
    PropertyDescriptorCollection properties2 = new PropertyDescriptorCollection((PropertyDescriptor[]) null);
    foreach (PropertyDescriptor propertyDescr in properties1)
    {
      bool flag = false;
      ItemToCreate.localPropertyDescriptor propertyDescriptor = new ItemToCreate.localPropertyDescriptor(propertyDescr);
      switch (propertyDescr.Name)
      {
        case "Size":
          flag = true;
          propertyDescriptor.SetIsReadOnly(!this.isNew);
          break;
        case "LocalID":
          flag = false;
          break;
        case "SizeType":
          switch (this.fieldType)
          {
            case FieldTypes.ftString:
            case FieldTypes.ftPassword:
              flag = true;
              break;
            case FieldTypes.ftShortBlob:
              flag = true;
              break;
            case FieldTypes.ftObjectLink:
              flag = true;
              break;
            case FieldTypes.ftMemo:
              flag = true;
              break;
          }
          break;
        case "CreateLinkObjType":
          if (this.FieldType == FieldTypes.ftObjectLink)
          {
            flag = true;
            propertyDescriptor.SetIsReadOnly(false);
            break;
          }
          break;
        default:
          propertyDescriptor.SetIsReadOnly(!this.isNew);
          flag = true;
          break;
      }
      if (flag)
        properties2.Add((PropertyDescriptor) propertyDescriptor);
    }
    return properties2;
  }
}

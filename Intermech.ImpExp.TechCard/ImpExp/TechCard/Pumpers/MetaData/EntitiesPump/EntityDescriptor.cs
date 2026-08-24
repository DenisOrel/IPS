// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntityDescriptor
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Advanced;
using Intermech.ImpExp.TechCard.Common;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

[RefreshProperties(RefreshProperties.All)]
[Serializable]
internal class EntityDescriptor : ICustomTypeDescriptor
{
  private string _name;
  private EntityDescriptor.Modes _status = EntityDescriptor.Modes.NoMode;
  private FieldTypes _fieldType;
  private string _alias;
  private string _shortName;
  private AttributeOptions _options;
  private string _mask;
  private string _note;
  private object _defaultValue;
  private MultiValueModes _multipleValued;
  private ComputeValueModes _computed;
  private long _sizeType;
  private string _formula;
  private UniqueValueModes _uniqueMode;
  private bool _isContent;
  private Guid _existAttrType = Guid.Empty;
  private EntityDescriptor.ProductionType _productionType;
  private EntityDescriptor.Locations _location;
  private EntitySetting.AttributeBelongs _attributeBelong;
  private readonly Entity _entity;
  private Entity _refEntity;
  private readonly List<Entity> _entityList;
  private string _joinGroupId;
  private static MeasureDescriptor[] _measureDescriptors;
  private static Dictionary<long, string> _phisicalValueList;

  private void SetExistAttributeType(Guid attrTypeGuid)
  {
    this._existAttrType = attrTypeGuid;
    if (this._entity == null || this._entity.Settings == null)
      return;
    this._entity.Settings.PumpTo = (object) (this._existAttrType = attrTypeGuid);
    if (this._entity.Settings.Properties == null)
      return;
    FieldTypes fieldTypes = FieldTypes.ftUnknown;
    IMetadataInfo imdi = TechcardConsts.Plugin.Imdi;
    if (imdi != null && imdi.AttributeTypes.ExistsByGuid(attrTypeGuid))
      fieldTypes = (FieldTypes) imdi.AttributeTypes.GetByGuid(attrTypeGuid).AttrValueType;
    if (fieldTypes == FieldTypes.ftUnknown)
      return;
    this._entity.Settings.Properties.FieldType = fieldTypes;
  }

  private EntityDescriptor.ProductionType GetProductionType()
  {
    if (this._entity == null || this._entity.Settings == null)
      return EntityDescriptor.ProductionType.ForAllTypes;
    if (this._productionType == EntityDescriptor.ProductionType.ForAllTypes)
    {
      if (this.Entity.Settings.MeasProdSettings != null && this.Entity.Settings.MeasProdSettings.Measure2ProdList.Count > 1)
        return EntityDescriptor.ProductionType.SettingsForOnlyType;
    }
    else if (this.Entity.Productions != null && this.Entity.Productions.Length > 1 || this.Entity.Settings.MeasProdSettings != null && this.Entity.Settings.MeasProdSettings.Measure2ProdList.Count > 1)
      return EntityDescriptor.ProductionType.SettingsForOnlyType;
    return EntityDescriptor.ProductionType.ForAllTypes;
  }

  private void InitializeData()
  {
    if (this._entity == null || this._entity.Settings == null)
      return;
    this._alias = this._entity.Settings.Properties.Alias;
    this._computed = this._entity.Settings.Properties.Computed;
    this._defaultValue = this._entity.Settings.Properties.DefaultValue;
    this._fieldType = this._entity.Settings.Properties.FieldType;
    this._formula = this._entity.Settings.Properties.Formula;
    this._isContent = this._entity.Settings.Properties.IsContent;
    this._mask = this._entity.Settings.Properties.Mask;
    this._multipleValued = this._entity.Settings.Properties.MultipleValued;
    this._name = this._entity.Settings.Properties.Name;
    this._note = this._entity.Settings.Properties.Note;
    this._options = this._entity.Settings.Properties.Options;
    this._shortName = this._entity.Settings.Properties.ShortName;
    this._sizeType = this._entity.Settings.Properties.SizeType;
    this._attributeBelong = this._entity.Settings.AttributeBelong;
    this._joinGroupId = this._entity.Settings.JoinGroupId;
    if (this._entity.Settings == null)
    {
      this._status = EntityDescriptor.Modes.NoMode;
    }
    else
    {
      switch (this._entity.Settings.PumpMode)
      {
        case EntityPumModes.NewAttr:
          switch (this._entity.Settings.Properties.Status)
          {
            case EntityPumpStatus.None:
              this.Status = EntityDescriptor.Modes.NoMode;
              break;
            case EntityPumpStatus.Exists:
            case EntityPumpStatus.New:
            case EntityPumpStatus.Commited:
              this.Status = EntityDescriptor.Modes.PumpToNewAtr;
              break;
            case EntityPumpStatus.NotPump:
              this.Status = EntityDescriptor.Modes.NoPump;
              break;
          }
          break;
        case EntityPumModes.ExistAttr:
          switch (this._entity.Settings.Properties.Status)
          {
            case EntityPumpStatus.None:
              this.Status = EntityDescriptor.Modes.NoMode;
              break;
            case EntityPumpStatus.Exists:
            case EntityPumpStatus.New:
            case EntityPumpStatus.Commited:
              this.Status = EntityDescriptor.Modes.PumpToAttr;
              break;
            case EntityPumpStatus.NotPump:
              this.Status = EntityDescriptor.Modes.NoPump;
              break;
          }
          break;
        case EntityPumModes.ExistEntity:
          switch (this._entity.Settings.Properties.Status)
          {
            case EntityPumpStatus.None:
              this.Status = EntityDescriptor.Modes.NoMode;
              break;
            case EntityPumpStatus.Exists:
            case EntityPumpStatus.New:
            case EntityPumpStatus.Commited:
              this.Status = EntityDescriptor.Modes.PumpToEntity;
              break;
            case EntityPumpStatus.NotPump:
              this.Status = EntityDescriptor.Modes.NoPump;
              break;
          }
          break;
      }
    }
    if (this._entity.Settings.PumpTo is Entity pumpTo)
      this._refEntity = pumpTo;
    this._productionType = this.GetProductionType();
    if (!(this._entity.Settings.PumpTo is Guid))
      return;
    this._existAttrType = (Guid) this._entity.Settings.PumpTo;
    if (!this._entity.IsMeasureAtribute() || this._entity.Settings.MeasProdSettings.isMeasureSet() || TechcardConsts.Plugin == null || TechcardConsts.Plugin.Imdi == null)
      return;
    IUserSession userSession = TechcardConsts.Plugin.Imdi.UserSession;
    if (userSession == null)
      return;
    IDBAttributeType attributeType = userSession.GetAttributeType(this._existAttrType);
    if (!(attributeType is IDBMeasureAttributeType))
      return;
    IDBMeasureAttributeType measureAttributeType = attributeType as IDBMeasureAttributeType;
    if (measureAttributeType.DefaultMeasureID == 0L)
      return;
    this._entity.Settings.MeasProdSettings.SetMeasure(measureAttributeType.DefaultMeasureID);
    this._entity.Settings.MeasProdSettings.PhysicalValueId = EntityDescriptor.GetPhisicalValueIdByMeasureId(measureAttributeType.DefaultMeasureID);
  }

  public EntityDescriptor(Entity entity, List<Entity> entityList)
  {
    this._entity = entity;
    this._entityList = entityList;
    this.InitializeData();
  }

  public Entity Entity
  {
    [DebuggerStepThrough] get => this._entity;
  }

  internal List<Entity> EntityList
  {
    [DebuggerStepThrough] get => this._entityList;
  }

  [Category("Настройка единиц измерения")]
  [DisplayName("Единица измерения - вид производства")]
  [Description("Настроить единицу измерения в зависимости от вида производства")]
  [Editor(typeof (MeasureProdDropDownEditor), typeof (UITypeEditor))]
  [TypeConverter(typeof (MeasureProdConverter))]
  public EntMeasureProdSetting MeasureSettings
  {
    get
    {
      return this.Entity == null || this.Entity.Settings == null ? (EntMeasureProdSetting) null : this.Entity.Settings.MeasProdSettings;
    }
    set
    {
      if (this.Entity == null || this.Entity.Settings == null)
        return;
      this.Entity.Settings.MeasProdSettings = value;
    }
  }

  [Category("Качать в атрибут")]
  [DisplayName("Добавлять в допустимые атрибуты")]
  [Description("Добавлять ли тип атрибута создаваемого по данному понятию в контекст допустимых для типа объекта.")]
  [TypeConverter(typeof (YesNoConverter))]
  public bool IsPermissibleAttr2TypeObject
  {
    get
    {
      return this.Entity != null && this.Entity.Settings != null && this.Entity.IsPermisibleAttr2TypeObj;
    }
    set
    {
      if (this.Entity == null || this.Entity.Settings == null)
        return;
      this.Entity.IsPermisibleAttr2TypeObj = value;
    }
  }

  [Category("Настройка понятия")]
  [DisplayName("Понятие качать в")]
  [Description("Производить закачку данного понятия в объект или атрибут")]
  [RefreshProperties(RefreshProperties.All)]
  [TypeConverter(typeof (EnumDescConverter))]
  public EntityDescriptor.Locations Location
  {
    get
    {
      return this.Entity != null && this.Entity.Settings != null && this.Entity.Settings.ObjectType != Guid.Empty ? EntityDescriptor.Locations.PumpToObject : this._location;
    }
    set
    {
      if (this.Entity != null && this.Entity.Settings != null && value == EntityDescriptor.Locations.PumpToAttribute)
        this.Entity.Settings.ObjectType = Guid.Empty;
      this._location = value;
    }
  }

  [Category("Настройка понятия")]
  [DisplayName("Тип объекта")]
  [Description("Тип объекта в который качать понятие")]
  [RefreshProperties(RefreshProperties.All)]
  [TypeConverter(typeof (ObjectTypeGuidConverter))]
  [Editor(typeof (ObjectTypeGuidEditor), typeof (UITypeEditor))]
  public Guid ObjectType
  {
    get
    {
      return this.Entity == null || this.Entity.Settings == null ? Guid.Empty : this.Entity.Settings.ObjectType;
    }
    set
    {
      if (this.Entity == null || this.Entity.Settings == null)
        return;
      this.Entity.Settings.ObjectType = value;
    }
  }

  [Description("Понятия одного типа объекта дополнительно группируются по указанным идентификаторам")]
  [Category("Настройка понятия")]
  [DisplayName("Идентификатор общей группы")]
  public string JoinGroupId
  {
    [DebuggerStepThrough] get => this._joinGroupId;
    set
    {
      this._joinGroupId = value;
      if (this._entity == null || this._entity.Settings == null)
        return;
      this._entity.Settings.JoinGroupId = value;
    }
  }

  [Category("Настройка единиц измерения")]
  [DisplayName("С учетом вида производства")]
  [Description("Настроить единицу измерения в зависимости от вида производства")]
  [RefreshProperties(RefreshProperties.All)]
  public EntityDescriptor.ProductionType ProductionTypeStatus
  {
    [DebuggerStepThrough] get => this._productionType;
    [DebuggerStepThrough] set
    {
      this._productionType = value;
      if (this._productionType != EntityDescriptor.ProductionType.ForAllTypes)
        return;
      this.MeasureSettings.Measure2ProdList.Clear();
    }
  }

  [Category("Настройка единиц измерения")]
  [DisplayName("Единица измерения")]
  [Description("Единица измерения для данного атрибута")]
  [TypeConverter(typeof (MeasuresTypeConverter))]
  public MeasureDescriptor Measure
  {
    get
    {
      MeasureDescriptor measure1 = new MeasureDescriptor(true);
      if (this.Entity == null || this.Entity.Settings == null || !this.Entity.Settings.MeasProdSettings.isMeasureSet() || this.Entity.Settings.MeasProdSettings.PhysicalValueId == -1L || this._productionType == EntityDescriptor.ProductionType.SettingsForOnlyType)
        return measure1;
      foreach (MeasureDescriptor measure2 in EntityDescriptor.GetMeasureDescriptorsByPhisicalValueId(this.Entity.Settings.MeasProdSettings.PhysicalValueId))
      {
        if (measure2.MeasureID == this.Entity.Settings.MeasProdSettings.GetMeasure())
          return measure2;
      }
      return measure1;
    }
    set
    {
      if (value == null || value.Empty || this.Entity == null || this.Entity.Settings == null)
        return;
      this.Entity.Settings.MeasProdSettings.SetMeasure(value.MeasureID);
    }
  }

  [Category("Настройка единиц измерения")]
  [DisplayName("Физическая величина")]
  [Description("К какой физической величине относится данный атрибут")]
  [RefreshProperties(RefreshProperties.All)]
  [TypeConverter(typeof (PhysicalValuesTypeConverter))]
  public string PhysicalValue
  {
    get
    {
      return this.Entity == null || this.Entity.Settings == null || this._entity.Settings.MeasProdSettings.PhysicalValueId == -1L || !EntityDescriptor.GetPhisicalValueList().ContainsKey(this._entity.Settings.MeasProdSettings.PhysicalValueId) ? string.Empty : EntityDescriptor.GetPhisicalValueList()[this._entity.Settings.MeasProdSettings.PhysicalValueId];
    }
    set
    {
      if (this.Entity == null || this.Entity.Settings == null)
        return;
      if (value == string.Empty)
        this._entity.Settings.MeasProdSettings.PhysicalValueId = -1L;
      else if (EntityDescriptor.GetPhisicalValueList().ContainsValue(value))
      {
        foreach (KeyValuePair<long, string> phisicalValue in EntityDescriptor.GetPhisicalValueList())
        {
          if (phisicalValue.Value == value)
          {
            this._entity.Settings.MeasProdSettings.PhysicalValueId = phisicalValue.Key;
            break;
          }
        }
      }
      else
        this._entity.Settings.MeasProdSettings.PhysicalValueId = -1L;
    }
  }

  [Category("Настройка единиц измерения")]
  [DisplayName("Понятие с ед. измерения")]
  [Description("Понятие, содержащее наименование единицы измерения")]
  [Editor(typeof (EntityWithMeasureEditor), typeof (UITypeEditor))]
  public string EntityWithMeasure
  {
    get => this.Entity?.Settings?.MeasProdSettings.EntityWithMeasure ?? string.Empty;
    set
    {
      EntMeasureProdSetting measProdSettings = this.Entity?.Settings?.MeasProdSettings;
      if (measProdSettings == null)
        return;
      measProdSettings.EntityWithMeasure = value;
    }
  }

  [Category("Настройка понятия")]
  [DisplayName("Принадлежность атрибута")]
  [Description("Куда производить закачку данного атрибута?")]
  [TypeConverter(typeof (EnumDescConverter))]
  public EntitySetting.AttributeBelongs AttributeBelonging
  {
    [DebuggerStepThrough] get => this._attributeBelong;
    set
    {
      this._attributeBelong = value;
      if (this.Entity == null || this.Entity.Settings == null)
        return;
      this.Entity.Settings.AttributeBelong = value;
    }
  }

  [RefreshProperties(RefreshProperties.All)]
  [Description("Статус закачки понятия")]
  [Category("Настройка понятия")]
  [DisplayName("Статус")]
  [TypeConverter(typeof (EnumDescConverter))]
  public EntityDescriptor.Modes Status
  {
    [DebuggerStepThrough] get => this._status;
    set
    {
      this._status = value;
      if (this.Entity == null || this.Entity.Settings == null)
        return;
      switch (this._status)
      {
        case EntityDescriptor.Modes.PumpToEntity:
          this._entity.Settings.PumpMode = EntityPumModes.ExistEntity;
          this._entity.Settings.Properties.Status = EntityPumpStatus.Exists;
          break;
        case EntityDescriptor.Modes.PumpToAttr:
          this._entity.Settings.PumpMode = EntityPumModes.ExistAttr;
          this._entity.Settings.Properties.Status = EntityPumpStatus.Exists;
          break;
        case EntityDescriptor.Modes.PumpToNewAtr:
          this._entity.Settings.PumpMode = EntityPumModes.NewAttr;
          this._entity.Settings.Properties.Status = EntityPumpStatus.New;
          break;
        case EntityDescriptor.Modes.NoMode:
          this._entity.Settings.Properties.Status = EntityPumpStatus.None;
          break;
        case EntityDescriptor.Modes.NoPump:
          this._entity.Settings.Properties.Status = EntityPumpStatus.NotPump;
          break;
      }
    }
  }

  [Category("Качать в атрибут")]
  [DisplayName("Атрибут")]
  [Description("Вы можете указать атрибут IPS в который качать данное понятие")]
  [RefreshProperties(RefreshProperties.All)]
  [TypeConverter(typeof (AttributeTypeConverter))]
  [Editor(typeof (EntityAttributeTypeEditor), typeof (UITypeEditor))]
  public Guid ExistAttribute
  {
    [DebuggerStepThrough] get => this._existAttrType;
    set => this.SetExistAttributeType(value);
  }

  [Category("Качать в понятие")]
  [DisplayName("Понятие")]
  [Description("Вы можете указать куда качать данное понятие")]
  [RefreshProperties(RefreshProperties.All)]
  [TypeConverter(typeof (EntityCollectionTypeConverter))]
  [Editor(typeof (ReferenceEntityEditor), typeof (UITypeEditor))]
  public Entity RefEntity
  {
    [DebuggerStepThrough] get => this._refEntity;
    set
    {
      this._refEntity = value;
      if (this.Entity == null || this.Entity.Settings == null)
        return;
      this.Entity.Settings.PumpTo = (object) value;
    }
  }

  [Category("Свойства атрибута")]
  [DisplayName("Имя атрибута")]
  [Description("Имя атрибута IPS")]
  [RefreshProperties(RefreshProperties.All)]
  public string Name
  {
    [DebuggerStepThrough] get => this._name;
    set
    {
      this._name = value;
      if (this._entity == null || this._entity.Settings == null)
        return;
      this._entity.Settings.Properties.Name = value;
    }
  }

  [ReadOnly(false)]
  [Description("Тип атрибута IPS")]
  [Category("Свойства атрибута")]
  [DisplayName("Тип атрибута")]
  [RefreshProperties(RefreshProperties.All)]
  [TypeConverter(typeof (AttributeListTypeConverter))]
  public string FieldType
  {
    [DebuggerStepThrough] get => EnumDescConverter.GetEnumDescription((Enum) this._fieldType);
    set
    {
      this._fieldType = (FieldTypes) EnumDescConverter.GetEnumValue(typeof (FieldTypes), value);
      switch (this._fieldType)
      {
        case FieldTypes.ftString:
          this.SizeType = 250L;
          break;
        case FieldTypes.ftShortBlob:
        case FieldTypes.ftMemo:
          this.SizeType = 131072L /*0x020000*/;
          break;
        case FieldTypes.ftObjectLink:
          this.SizeType = -1L;
          break;
        case FieldTypes.ftPassword:
          this.SizeType = 50L;
          break;
        default:
          this.SizeType = 0L;
          break;
      }
      if (this.Entity == null || this.Entity.Settings == null)
        return;
      this._entity.Settings.Properties.FieldType = this._fieldType;
    }
  }

  [Description("Псевдоним атрибута IPS(понятие)")]
  [Category("Свойства атрибута")]
  [DisplayName("Псевдоним")]
  public string Alias
  {
    [DebuggerStepThrough] get => this._alias;
    set
    {
      this._alias = value;
      if (this._entity == null || this._entity.Settings == null)
        return;
      this._entity.Settings.Properties.Alias = value;
    }
  }

  [Description("Короткое имя атрибута IPS")]
  [Category("Свойства атрибута")]
  [DisplayName("Короткое имя")]
  public string ShortName
  {
    [DebuggerStepThrough] get => this._shortName;
    set
    {
      this._shortName = value;
      if (this._entity == null || this._entity.Settings == null)
        return;
      this._entity.Settings.Properties.ShortName = value;
    }
  }

  [Description("Опции атрибута")]
  [Category("Свойства атрибута")]
  [DisplayName("Опции")]
  [Editor(typeof (AttributeOptionsDropDownEditor), typeof (UITypeEditor))]
  [TypeConverter(typeof (OptionsConverter))]
  public AttributeOptions Options
  {
    [DebuggerStepThrough] get => this._options;
    set
    {
      this._options = value;
      if (this._entity == null || this._entity.Settings == null)
        return;
      this._entity.Settings.Properties.Options = value;
    }
  }

  [Description("Маска")]
  [Category("Свойства атрибута")]
  [DisplayName("Маска")]
  public string Mask
  {
    [DebuggerStepThrough] get => this._mask;
    set
    {
      this._mask = value;
      if (this._entity == null || this._entity.Settings == null)
        return;
      this._entity.Settings.Properties.Mask = value;
    }
  }

  [Description("Комментарии")]
  [Category("Свойства атрибута")]
  [DisplayName("Комментарии")]
  public string Note
  {
    [DebuggerStepThrough] get => this._note;
    set
    {
      this._note = value;
      if (this._entity == null || this._entity.Settings == null)
        return;
      this._entity.Settings.Properties.Note = value;
    }
  }

  [Description("Значение атрибута по умолчанию")]
  [Category("Свойства атрибута")]
  [DisplayName("Значение по умолчанию")]
  [TypeConverter(typeof (StringConverter))]
  public object DefaultValue
  {
    [DebuggerStepThrough] get => this._defaultValue;
    set
    {
      this._defaultValue = value;
      if (this._entity == null || this._entity.Settings == null)
        return;
      this._entity.Settings.Properties.DefaultValue = value;
    }
  }

  [Description("Может ли атрибут принимать множественные значения")]
  [Category("Свойства атрибута")]
  [DisplayName("Множественное значение")]
  public MultiValueModes MultipleValued
  {
    [DebuggerStepThrough] get => this._multipleValued;
    set
    {
      this._multipleValued = value;
      if (this._entity == null || this._entity.Settings == null)
        return;
      this._entity.Settings.Properties.MultipleValued = value;
    }
  }

  [Description("Вычисляемое значение")]
  [Category("Свойства атрибута")]
  [DisplayName("Вычисляемое значение")]
  public ComputeValueModes Computed
  {
    [DebuggerStepThrough] get => this._computed;
    set
    {
      this._computed = value;
      if (this._entity == null || this._entity.Settings == null)
        return;
      this._entity.Settings.Properties.Computed = value;
    }
  }

  [Description("Длина")]
  [Category("Свойства атрибута")]
  [DisplayName("Длина")]
  public long SizeType
  {
    [DebuggerStepThrough] get => this._sizeType;
    set
    {
      this._sizeType = value;
      if (this._entity == null || this._entity.Settings == null)
        return;
      this._entity.Settings.Properties.SizeType = value;
    }
  }

  [Description("Формула атрибута")]
  [Category("Свойства атрибута")]
  [DisplayName("Формула")]
  public string Formula
  {
    [DebuggerStepThrough] get => this._formula;
    set
    {
      this._formula = value;
      if (this._entity == null || this._entity.Settings == null)
        return;
      this._entity.Settings.Properties.Formula = value;
    }
  }

  [Description("Уникальность значения атрибута")]
  [Category("Свойства атрибута")]
  [DisplayName("Уникальность значения")]
  public UniqueValueModes UniqueMode
  {
    [DebuggerStepThrough] get => this._uniqueMode;
    set
    {
      this._uniqueMode = value;
      if (this._entity == null || this._entity.Settings == null)
        return;
      this._entity.Settings.Properties.UniqueMode = value;
    }
  }

  [Description("Влияет на дату модификации содержимого объекта")]
  [Category("Свойства атрибута")]
  [DisplayName("Влияет на дату модификации содержимого объекта")]
  [TypeConverter(typeof (YesNoConverter))]
  public bool IsContent
  {
    [DebuggerStepThrough] get => this._isContent;
    set
    {
      this._isContent = value;
      if (this._entity == null || this._entity.Settings == null)
        return;
      this._entity.Settings.Properties.IsContent = value;
    }
  }

  public static long GetPhisicalValueIdByMeasureId(long measureId)
  {
    long valueIdByMeasureId = -1;
    if (EntityDescriptor._measureDescriptors == null)
      EntityDescriptor._measureDescriptors = TechcardConsts.Plugin.Imdi.UserSession.GetMeasuresList();
    foreach (MeasureDescriptor measureDescriptor in EntityDescriptor._measureDescriptors)
    {
      if (measureDescriptor.MeasureID == measureId)
      {
        valueIdByMeasureId = measureDescriptor.PhysicalQuantityID;
        break;
      }
    }
    return valueIdByMeasureId;
  }

  public static List<MeasureDescriptor> GetMeasureDescriptorsByPhisicalValueId(long id)
  {
    List<MeasureDescriptor> byPhisicalValueId = new List<MeasureDescriptor>();
    if (EntityDescriptor._measureDescriptors == null)
      EntityDescriptor._measureDescriptors = TechcardConsts.Plugin.Imdi.UserSession.GetMeasuresList();
    foreach (MeasureDescriptor measureDescriptor in EntityDescriptor._measureDescriptors)
    {
      if (measureDescriptor.PhysicalQuantityID == id)
        byPhisicalValueId.Add(measureDescriptor);
    }
    return byPhisicalValueId;
  }

  public static Dictionary<long, string> GetPhisicalValueList()
  {
    if (EntityDescriptor._phisicalValueList != null || TechcardConsts.Plugin == null)
      return EntityDescriptor._phisicalValueList;
    IUserSession userSession = TechcardConsts.Plugin.Imdi.UserSession;
    if (EntityDescriptor._measureDescriptors == null)
      EntityDescriptor._measureDescriptors = userSession.GetMeasuresList();
    MeasureDescriptor[] measureDescriptors = EntityDescriptor._measureDescriptors;
    SortedList<string, long> sortedList = new SortedList<string, long>();
    foreach (MeasureDescriptor measureDescriptor in measureDescriptors)
    {
      string caption = userSession.GetObjectInfo(measureDescriptor.PhysicalQuantityID).Caption;
      if (!sortedList.ContainsKey(caption))
        sortedList.Add(caption, measureDescriptor.PhysicalQuantityID);
    }
    SortedList<string, long> source = sortedList;
    return EntityDescriptor._phisicalValueList = source.ToDictionary<KeyValuePair<string, long>, long, string>((Func<KeyValuePair<string, long>, long>) (skvp => skvp.Value), (Func<KeyValuePair<string, long>, string>) (skvp => skvp.Key));
  }

  public PropertyDescriptorCollection GetProperties() => this.GetProperties(new Attribute[0]);

  public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
  {
    PropertyDescriptorCollection properties1 = TypeDescriptor.GetProperties((object) this, attributes, true);
    PropertyDescriptorCollection properties2 = new PropertyDescriptorCollection((PropertyDescriptor[]) null);
    foreach (PropertyDescriptor descr in properties1)
    {
      bool flag = false;
      lPropertyDescriptor propertyDescriptor = new lPropertyDescriptor(descr, descr.GetValue((object) this));
      if (this._entity != null && this._entity.LockedSettings)
        propertyDescriptor.SetReadOnly(true);
      switch (descr.Name)
      {
        case "Alias":
        case "Computed":
        case "Formula":
        case "IsContent":
        case "Mask":
        case "MultipleValued":
        case "Note":
        case "Options":
        case "ShortName":
        case "UniqueMode":
          if (this._status == EntityDescriptor.Modes.PumpToNewAtr)
          {
            flag = true;
            goto case "Ent";
          }
          goto case "Ent";
        case "AtributeBelonging":
        case "AttributeBelonging":
          if (this._status != EntityDescriptor.Modes.NoMode && this._status != EntityDescriptor.Modes.NoPump)
          {
            flag = true;
            goto case "Ent";
          }
          goto case "Ent";
        case "DefaultValue":
          if (this._status == EntityDescriptor.Modes.PumpToNewAtr)
          {
            flag = true;
            switch (this._fieldType)
            {
              case FieldTypes.ftString:
              case FieldTypes.ftPassword:
                goto label_54;
              case FieldTypes.ftInteger:
              case FieldTypes.ftAutoInc:
                propertyDescriptor.SetConverter(typeof (Int32Converter));
                goto label_54;
              case FieldTypes.ftDouble:
              case FieldTypes.ftMeasured:
                propertyDescriptor.SetConverter(typeof (DoubleConverter));
                goto label_54;
              case FieldTypes.ftDateTime:
                propertyDescriptor.SetConverter(typeof (DateTimeConverter));
                goto label_54;
              case FieldTypes.ftBoolean:
                propertyDescriptor.SetConverter(typeof (BooleanConverter));
                goto label_54;
              case FieldTypes.ftGuid:
                propertyDescriptor.SetConverter(typeof (GuidConverter));
                goto label_54;
              default:
                flag = false;
                goto label_54;
            }
          }
          else
            goto case "Ent";
        case "Ent":
        case "Entity":
label_54:
          if (flag)
          {
            properties2.Add((PropertyDescriptor) propertyDescriptor);
            continue;
          }
          continue;
        case "EntityWithMeasure":
        case "PhysicalValue":
          Entity entity1 = this.Entity;
          if (entity1 != null && (this._status != EntityDescriptor.Modes.NoMode || this._status != EntityDescriptor.Modes.NoPump) && entity1.IsMeasureAtribute())
          {
            flag = true;
            goto case "Ent";
          }
          goto case "Ent";
        case "ExistAttribute":
          if (this._status == EntityDescriptor.Modes.PumpToAttr || this._status == EntityDescriptor.Modes.PumpToImbaseAttr)
            flag = true;
          if (this._status != EntityDescriptor.Modes.PumpToImbaseAttr)
            goto case "Ent";
          goto case "Ent";
        case "FieldType":
          if (this._entity != null && this._entity.IsMasterAttr)
          {
            this._entity.Settings.Properties.FieldType = FieldTypes.ftObjectLink;
            this._fieldType = FieldTypes.ftObjectLink;
            propertyDescriptor.SetReadOnly(true);
          }
          if (this._status == EntityDescriptor.Modes.PumpToNewAtr)
          {
            flag = true;
            goto case "Ent";
          }
          goto case "Ent";
        case "IsPermissibleAttr2TypeObject":
          if (this._status == EntityDescriptor.Modes.PumpToNewAtr)
          {
            flag = true;
            goto case "Ent";
          }
          goto case "Ent";
        case "JoinGroupId":
          if (this.Location == EntityDescriptor.Locations.PumpToObject)
          {
            flag = true;
            goto case "Ent";
          }
          goto case "Ent";
        case "Location":
          if (this._status != EntityDescriptor.Modes.NoMode && this._status != EntityDescriptor.Modes.NoPump)
          {
            flag = true;
            goto case "Ent";
          }
          goto case "Ent";
        case "Measure":
          Entity entity2 = this.Entity;
          if (entity2 != null && (this._status != EntityDescriptor.Modes.NoMode || this._status != EntityDescriptor.Modes.NoPump) && entity2.IsMeasureAtribute() && entity2.Settings.MeasProdSettings.PhysicalValueId != -1L && this._productionType == EntityDescriptor.ProductionType.ForAllTypes)
          {
            flag = true;
            goto case "Ent";
          }
          goto case "Ent";
        case "MeasureProdSetting":
        case "MeasureSetting":
        case "MeasureSettings":
          if (this.Entity != null && this.Entity.IsMeasureAtribute() && this._productionType == EntityDescriptor.ProductionType.SettingsForOnlyType)
          {
            flag = true;
            goto case "Ent";
          }
          goto case "Ent";
        case "Name":
          if (this._status == EntityDescriptor.Modes.PumpToNewAtr)
          {
            flag = true;
            goto case "Ent";
          }
          goto case "Ent";
        case "ObjectType":
          if (this.Location == EntityDescriptor.Locations.PumpToObject)
          {
            flag = true;
            goto case "Ent";
          }
          goto case "Ent";
        case "ProductionTypeStatus":
          if (this.Entity != null && this.Entity.IsMeasureAtribute() && this.Entity.Productions.Length > 1)
          {
            flag = true;
            goto case "Ent";
          }
          goto case "Ent";
        case "RefEntity":
          if (this._status == EntityDescriptor.Modes.PumpToEntity)
          {
            flag = true;
            goto case "Ent";
          }
          goto case "Ent";
        case "SizeType":
          switch (this._fieldType)
          {
            case FieldTypes.ftString:
            case FieldTypes.ftPassword:
              propertyDescriptor.SetDisplayName("Длина строки");
              flag = true;
              break;
            case FieldTypes.ftShortBlob:
              propertyDescriptor.SetDisplayName("Длина blob поля");
              flag = true;
              break;
            case FieldTypes.ftObjectLink:
              propertyDescriptor.SetDisplayName("Тип объекта");
              propertyDescriptor.SetConverter(typeof (ObjectTypeIntConverter));
              propertyDescriptor.SetEditor(typeof (ObjectTypeIntEditor));
              flag = true;
              break;
            case FieldTypes.ftMemo:
              propertyDescriptor.SetDisplayName("Длина текста");
              flag = true;
              break;
          }
          if (this._status != EntityDescriptor.Modes.PumpToNewAtr)
          {
            flag = false;
            goto case "Ent";
          }
          goto case "Ent";
        case "Status":
          flag = true;
          goto case "Ent";
        default:
          string Message = $"Методу EntityView.GetProperties не известно свойство {descr.Name}";
          if (TechcardConsts.Plugin != null && TechcardConsts.Plugin.appManager != null)
          {
            TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
            goto case "Ent";
          }
          goto case "Ent";
      }
    }
    return properties2;
  }

  public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes((object) this, true);

  public object GetPropertyOwner(PropertyDescriptor pd) => (object) this;

  public string GetClassName() => TypeDescriptor.GetClassName((object) this, true);

  public string GetComponentName() => TypeDescriptor.GetComponentName((object) this, true);

  public TypeConverter GetConverter() => TypeDescriptor.GetConverter((object) this, true);

  public EventDescriptor GetDefaultEvent() => TypeDescriptor.GetDefaultEvent((object) this, true);

  public PropertyDescriptor GetDefaultProperty()
  {
    return TypeDescriptor.GetDefaultProperty((object) this, true);
  }

  public object GetEditor(Type editorBaseType)
  {
    return TypeDescriptor.GetEditor((object) this, editorBaseType, true);
  }

  public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents((object) this, true);

  public EventDescriptorCollection GetEvents(Attribute[] attributes)
  {
    return TypeDescriptor.GetEvents((object) this, attributes, true);
  }

  [TypeConverter(typeof (EnumDescConverter))]
  internal enum ProductionType
  {
    [Description("Для всех видов производства")] ForAllTypes,
    [Description("В зависимости от вида производства")] SettingsForOnlyType,
  }

  [Serializable]
  internal enum Locations
  {
    [Description("В настроеный атрибут")] PumpToAttribute,
    [Description("Качать в новый объект")] PumpToObject,
  }

  [Serializable]
  internal enum Modes
  {
    [Description("Качать по понятию")] PumpToEntity,
    [Description("Качать в существующий атрибут")] PumpToAttr,
    [Description("Качать в новый атрибут")] PumpToNewAtr,
    [Description("Не настроен")] NoMode,
    [Description("Не качать")] NoPump,
    [Browsable(false), Description("Качать в атрибут согласно настройкам Imbase")] PumpToImbaseAttr,
  }
}

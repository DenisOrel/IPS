// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.Controls.ImbaseAttribute
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Imbase.Controls;

internal class ImbaseAttribute : ICustomTypeDescriptor
{
  private string _name = string.Empty;
  private Guid _guid = Guid.Empty;
  private string _shortName = string.Empty;
  private ImDataTypeEx _type;
  private FieldTypes _newType;
  private AttributeTypeAttProxy _bindingAttribute = new AttributeTypeAttProxy();
  private List<string> _presentInTables;
  private List<TableInfo> _tableIDs;
  private bool _existInBase = true;
  private int _size;
  private List<int> _keys;
  private MultiValueModes _multiValueMode;
  private AttributeCheckResult _checkResult;
  private string _unit = string.Empty;

  public ImbaseAttribute(
    int key,
    Guid guid,
    string name,
    string shortName,
    ImDataTypeEx type,
    FieldTypes newType,
    bool existInBase,
    MultiValueModes multiValueMode,
    string unit)
  {
    this._keys = new List<int>(1);
    this._keys.Add(key);
    this._guid = guid;
    this._name = name;
    this._shortName = shortName;
    this._existInBase = existInBase;
    this._type = type;
    this._newType = newType;
    this._presentInTables = new List<string>();
    this._tableIDs = new List<TableInfo>();
    this._multiValueMode = multiValueMode;
    this._unit = unit;
  }

  public ImbaseAttribute(
    int key,
    Guid guid,
    string name,
    string shortName,
    ImDataTypeEx type,
    FieldTypes newType,
    bool existInBase,
    MultiValueModes multiValueMode,
    int size,
    string unit)
    : this(key, guid, name, shortName, type, newType, existInBase, multiValueMode, unit)
  {
    this._size = size;
  }

  public PropertyDescriptorCollection GetProperties() => this.GetProperties(new Attribute[0]);

  public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
  {
    return TypeDescriptor.GetProperties((object) this, attributes, true);
  }

  public object GetPropertyOwner(PropertyDescriptor pd) => (object) this;

  public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes((object) this, true);

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

  public EventDescriptorCollection GetEvents(Attribute[] attributes)
  {
    return TypeDescriptor.GetEvents((object) this, attributes, true);
  }

  public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents((object) this, true);

  [DisplayName("Имя атрибута")]
  [Description("Отображаемое имя атрибута")]
  public string Name => this._name;

  [DisplayName("Содержит данные")]
  [Description("Признак, вычисляемый атрибут, или его значение физически хранится в базе")]
  public bool ExistInBase => this._existInBase;

  [DisplayName("Краткое имя")]
  [Description("Краткое имя атрибута")]
  public string ShortName => this._shortName;

  [DisplayName("Тип данных")]
  [Description("Тип данных атрибута")]
  public ImDataTypeEx AttributeType => this._type;

  [DisplayName("Привязка")]
  [Description("Атрибут в базе назначения, которому сопоставлен текущий атрибут")]
  public AttributeTypeAttProxy BindingAttribute
  {
    get => this._bindingAttribute;
    set => this._bindingAttribute = value;
  }

  [DisplayName("Длина")]
  [Description("Максимальная длина значения атрибута (только для строковых атрибутов)")]
  public int Size => this._type == ImDataTypeEx.IEX_STRING ? this._size : 0;

  [Browsable(false)]
  public List<string> PresentInTables
  {
    set => this._presentInTables = value;
    get => this._presentInTables;
  }

  [Browsable(false)]
  public List<TableInfo> TableIDs
  {
    set => this._tableIDs = value;
    get => this._tableIDs;
  }

  [Browsable(false)]
  public AttributeCheckResult CheckResult
  {
    set => this._checkResult = value;
    get => this._checkResult;
  }

  [Browsable(false)]
  public FieldTypes NewType
  {
    set => this._newType = value;
    get => this._newType;
  }

  [Browsable(false)]
  public string Unit
  {
    set => this._unit = value;
    get => this._unit;
  }

  [DisplayName("Результат проверки")]
  [Description("Результат проверки атрибута")]
  public string CheckResultString => EnumDescConverter.GetEnumDescription((Enum) this._checkResult);

  [Browsable(false)]
  public Guid GUID => this._guid;

  [Browsable(false)]
  public List<int> Keys => this._keys;

  [Browsable(false)]
  public MultiValueModes MultiValueMode => this._multiValueMode;

  [Browsable(false)]
  public string CheckName
  {
    get
    {
      return ImbaseAttribute.GetCheckName(this._name, this._shortName, this._newType, this._size, this._bindingAttribute.AttributeType, this._checkResult, this._multiValueMode);
    }
  }

  [DisplayName("Таблицы")]
  [Description("Таблицы Imbase в которых встречается атрибут")]
  public ListTablesProxy InTables
  {
    get => new ListTablesProxy(this._presentInTables);
    set
    {
    }
  }

  public static string GetCheckName(
    string name,
    string shortName,
    FieldTypes type,
    int size,
    Guid bindingAttrGuid,
    AttributeCheckResult checkRes,
    MultiValueModes multiValueMode)
  {
    string str = type == FieldTypes.ftString ? Convert.ToString(size) : string.Empty;
    return $"{name.ToLower()}{shortName.ToLower()}{Convert.ToString((object) type)}{str}{Convert.ToString((object) bindingAttrGuid)}{(int) checkRes}{(int) multiValueMode}";
  }
}

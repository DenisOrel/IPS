// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.EntityProperties
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Advanced;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace Intermech.ImpExp.TechCard;

[RefreshProperties(RefreshProperties.All)]
[Serializable]
public class EntityProperties : ICustomTypeDescriptor
{
  private string _name;
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
  private EntityPumpStatus _status;
  private List<string> _invFieldsList = new List<string>();
  public List<int> _link2ObjTypes = new List<int>();
  public List<int> _link2RelTypes = new List<int>();

  internal EntityProperties Clone()
  {
    return new EntityProperties()
    {
      _status = this._status,
      _link2ObjTypes = new List<int>((IEnumerable<int>) this._link2ObjTypes),
      _link2RelTypes = new List<int>((IEnumerable<int>) this._link2RelTypes),
      _alias = this._alias,
      _computed = this._computed,
      _defaultValue = this._defaultValue,
      _fieldType = this._fieldType,
      _formula = this._formula,
      _mask = this._mask,
      _multipleValued = this._multipleValued,
      _name = this._name,
      _note = this._note,
      _options = this._options,
      _shortName = this._shortName,
      _sizeType = this._sizeType,
      _uniqueMode = this._uniqueMode,
      _isContent = this._isContent,
      _invFieldsList = new List<string>((IEnumerable<string>) this._invFieldsList)
    };
  }

  [Category("Закачка атрибута")]
  [DisplayName("Статус")]
  public EntityPumpStatus Status
  {
    [DebuggerStepThrough] get => this._status;
    [DebuggerStepThrough] set => this._status = value;
  }

  [Category("Свойства атрибута")]
  [DisplayName("Имя атрибута")]
  [RefreshProperties(RefreshProperties.All)]
  public string Name
  {
    [DebuggerStepThrough] get => this._name;
    [DebuggerStepThrough] set => this._name = value;
  }

  [ReadOnly(false)]
  [Category("Свойства атрибута")]
  [DisplayName("Тип атрибута")]
  [RefreshProperties(RefreshProperties.All)]
  public FieldTypes FieldType
  {
    [DebuggerStepThrough] get => this._fieldType;
    set
    {
      this._fieldType = value;
      switch (this._fieldType)
      {
        case FieldTypes.ftString:
          this._sizeType = 250L;
          break;
        case FieldTypes.ftShortBlob:
        case FieldTypes.ftMemo:
          this._sizeType = 131072L /*0x020000*/;
          break;
        case FieldTypes.ftObjectLink:
          this._sizeType = -1L;
          break;
        case FieldTypes.ftPassword:
          this._sizeType = 50L;
          break;
        default:
          this._sizeType = 0L;
          break;
      }
    }
  }

  [Category("Свойства атрибута")]
  [DisplayName("Псевдоним")]
  public string Alias
  {
    [DebuggerStepThrough] get => this._alias;
    [DebuggerStepThrough] set => this._alias = value;
  }

  [Category("Свойства атрибута")]
  [DisplayName("Короткое имя")]
  public string ShortName
  {
    [DebuggerStepThrough] get => this._shortName;
    [DebuggerStepThrough] set => this._shortName = value;
  }

  [Category("Свойства атрибута")]
  [DisplayName("Опции")]
  public AttributeOptions Options
  {
    [DebuggerStepThrough] get => this._options;
    [DebuggerStepThrough] set => this._options = value;
  }

  [Category("Свойства атрибута")]
  [DisplayName("Маска")]
  public string Mask
  {
    [DebuggerStepThrough] get => this._mask;
    [DebuggerStepThrough] set => this._mask = value;
  }

  [Category("Свойства атрибута")]
  [DisplayName("Комментарии")]
  public string Note
  {
    [DebuggerStepThrough] get => this._note;
    [DebuggerStepThrough] set => this._note = value;
  }

  [Category("Свойства атрибута")]
  [DisplayName("Значение по умолчанию")]
  [TypeConverter(typeof (StringConverter))]
  public object DefaultValue
  {
    [DebuggerStepThrough] get => this._defaultValue;
    [DebuggerStepThrough] set => this._defaultValue = value;
  }

  [Category("Свойства атрибута")]
  [DisplayName("Множественное значение")]
  public MultiValueModes MultipleValued
  {
    [DebuggerStepThrough] get => this._multipleValued;
    [DebuggerStepThrough] set => this._multipleValued = value;
  }

  [Category("Свойства атрибута")]
  [DisplayName("Вычисляемое значение")]
  public ComputeValueModes Computed
  {
    [DebuggerStepThrough] get => this._computed;
    [DebuggerStepThrough] set => this._computed = value;
  }

  [Category("Свойства атрибута")]
  [DisplayName("Длина")]
  public long SizeType
  {
    [DebuggerStepThrough] get => this._sizeType;
    set => this._sizeType = value;
  }

  [Category("Свойства атрибута")]
  [DisplayName("Формула")]
  public string Formula
  {
    [DebuggerStepThrough] get => this._formula;
    [DebuggerStepThrough] set => this._formula = value;
  }

  [Category("Свойства атрибута")]
  [DisplayName("Уникальность значения")]
  public UniqueValueModes UniqueMode
  {
    [DebuggerStepThrough] get => this._uniqueMode;
    [DebuggerStepThrough] set => this._uniqueMode = value;
  }

  [Category("Свойства атрибута")]
  [DisplayName("Влияет на дату модификации содержимого объекта")]
  [TypeConverter(typeof (YesNoConverter))]
  public bool IsContent
  {
    [DebuggerStepThrough] get => this._isContent;
    [DebuggerStepThrough] set => this._isContent = value;
  }

  public void SetFieldVisibleFlag(string fieldName, bool visibleFlag)
  {
    if (visibleFlag)
    {
      if (!this._invFieldsList.Contains(fieldName))
        return;
      this._invFieldsList.Remove(fieldName);
    }
    else
    {
      if (this._invFieldsList.Contains(fieldName))
        return;
      this._invFieldsList.Add(fieldName);
    }
  }

  public bool IsInVisibleField(string fieldName) => !this._invFieldsList.Contains(fieldName);

  public PropertyDescriptorCollection GetProperties() => this.GetProperties(new Attribute[0]);

  public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
  {
    PropertyDescriptorCollection properties1 = TypeDescriptor.GetProperties((object) this, attributes, true);
    PropertyDescriptorCollection properties2 = new PropertyDescriptorCollection((PropertyDescriptor[]) null);
    foreach (PropertyDescriptor descr in properties1)
    {
      bool flag = false;
      lPropertyDescriptor propertyDescriptor = new lPropertyDescriptor(descr, descr.GetValue((object) this));
      switch (descr.Name)
      {
        case "Alias":
        case "Computed":
        case "DefaultValue":
        case "FieldType":
        case "Formula":
        case "IsContent":
        case "Mask":
        case "MultipleValued":
        case "Name":
        case "Note":
        case "ShortName":
        case "Status":
        case "UniqueMode":
          flag = true;
          break;
        case "SizeType":
          switch (this._fieldType)
          {
            case FieldTypes.ftString:
            case FieldTypes.ftPassword:
              propertyDescriptor.SetDisplayName("Длина строки");
              flag = true;
              break;
            case FieldTypes.ftShortBlob:
              propertyDescriptor.SetDisplayName("Длина блоба");
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
          break;
      }
      if (flag && this.IsInVisibleField(descr.Name))
        properties2.Add((PropertyDescriptor) propertyDescriptor);
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
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechTypes.TechTypeSett
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Advanced;
using Intermech.ImpExp.TechCard.Common;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechTypes;

[Serializable]
public class TechTypeSett : ICustomTypeDescriptor
{
  internal TechTypePumpMode mode = TechTypePumpMode.NotPumpType;
  internal Guid objType = Guid.Empty;
  internal Guid ownerType = TechcardConsts.TypeConsts.otTechobjectObjTypeGuid;
  internal ObjectVersionModes versionable = ObjectVersionModes.MultiVersion;
  internal Guid relType = TechcardConsts.TypeConsts.rtTechRelationGuid;
  internal string area = string.Empty;
  internal bool anyAttributes = true;
  internal bool readOnly;

  public TechTypeSett Clone()
  {
    return new TechTypeSett()
    {
      mode = this.mode,
      objType = this.objType,
      relType = this.relType,
      area = this.area,
      readOnly = this.readOnly,
      ownerType = this.ownerType,
      Versionable = this.versionable,
      anyAttributes = this.anyAttributes
    };
  }

  [Category("Свойства типа записи")]
  [DisplayName("Режим закачки")]
  [RefreshProperties(RefreshProperties.All)]
  public TechTypePumpMode Mode
  {
    get => this.mode;
    set => this.mode = value;
  }

  [Category("Свойства типа записи")]
  [DisplayName("Тип объекта в IPS")]
  public Guid ObjType
  {
    get => this.objType;
    set => this.objType = value;
  }

  [Category("Свойства типа записи")]
  [DisplayName("Родительский тип объекта в IPS")]
  public Guid OwnerType
  {
    get => this.ownerType;
    set => this.ownerType = value;
  }

  [Category("Свойства типа записи")]
  [DisplayName("Тип версионности")]
  public ObjectVersionModes Versionable
  {
    get => this.versionable;
    set => this.versionable = value;
  }

  [Category("Свойства типа записи")]
  [DisplayName("Идентификатор типа связи")]
  public Guid RelType
  {
    get => this.relType;
    set => this.relType = value;
  }

  [Category("Свойства типа записи")]
  [DisplayName("Предметная область")]
  public string Area
  {
    get => this.area;
    set => this.area = value;
  }

  [Category("Свойства типа записи")]
  [DisplayName("Любые атрибуты")]
  [TypeConverter(typeof (YesNoConverter))]
  public bool AnyAttributes
  {
    get => this.anyAttributes;
    set => this.anyAttributes = value;
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
      switch (descr.Name)
      {
        case "AnyAttributes":
          if (this.mode == TechTypePumpMode.NewObjType)
            flag = true;
          propertyDescriptor.SetConverter(typeof (BooleanConverter));
          break;
        case "Area":
          if (this.mode == TechTypePumpMode.NewObjType)
          {
            flag = true;
            break;
          }
          break;
        case "Mode":
          propertyDescriptor.SetConverter(typeof (TechTypePumpModeConverter));
          flag = true;
          break;
        case "ObjType":
          if (this.mode == TechTypePumpMode.ExistObjType)
            flag = true;
          propertyDescriptor.SetConverter(typeof (ObjectTypeGuidConverter));
          propertyDescriptor.SetEditor(typeof (ObjectTypeGuidEditor));
          break;
        case "OwnerType":
          if (this.mode == TechTypePumpMode.NewObjType)
            flag = true;
          propertyDescriptor.SetConverter(typeof (ObjectTypeGuidConverter));
          propertyDescriptor.SetEditor(typeof (ObjectTypeGuidEditor));
          break;
        case "RelType":
          if (this.mode == TechTypePumpMode.NewObjType)
            flag = true;
          propertyDescriptor.SetConverter(typeof (RelationTypeConverter));
          propertyDescriptor.SetEditor(typeof (RelationGuidTypeEditor));
          break;
        case "Versionable":
          if (this.mode == TechTypePumpMode.NewObjType)
            flag = true;
          propertyDescriptor.SetConverter(typeof (EnumDescConverter));
          break;
      }
      if (flag)
        properties2.Add((PropertyDescriptor) propertyDescriptor);
      if (this.readOnly)
        propertyDescriptor.SetReadOnly(true);
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

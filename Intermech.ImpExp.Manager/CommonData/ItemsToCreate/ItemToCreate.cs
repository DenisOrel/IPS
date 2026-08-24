// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.ItemsToCreate.ItemToCreate
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Manager.CommonData.ItemsToCreate;

[Serializable]
internal class ItemToCreate : IItemToCreate, ICustomTypeDescriptor
{
  protected static int MaxIndex = 1;
  protected int localID;
  protected string name = string.Empty;
  protected bool isNew;
  protected Guid guid = Guid.Empty;
  protected long systemID;

  public ItemToCreate(bool isNew, string name, Guid guid, long sysID)
  {
    this.localID = ItemToCreate.MaxIndex++;
    this.isNew = isNew;
    this.name = name;
    this.guid = guid;
    this.systemID = sysID;
  }

  [DisplayName("Локальный идентификатор")]
  [Browsable(false)]
  public int LocalID => this.localID;

  [DisplayName("Наименование")]
  public string Name
  {
    get => this.name;
    set
    {
      if (!(this.name != value))
        return;
      this.name = value;
    }
  }

  [Browsable(false)]
  [DisplayName("Новый тип")]
  public bool IsNew => this.isNew;

  [DisplayName("Существует в базе назначения")]
  public string IsNewAttribute => this.isNew ? "Нет" : "Да";

  [DisplayName("Глобальный идентификатор")]
  public Guid GUID => this.guid;

  [DisplayName("Системный идентификатор")]
  [Browsable(false)]
  public long SystemId => this.systemID;

  public PropertyDescriptorCollection GetProperties() => this.GetProperties(new Attribute[0]);

  public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
  {
    PropertyDescriptorCollection properties1 = TypeDescriptor.GetProperties((object) this, attributes, true);
    PropertyDescriptorCollection properties2 = new PropertyDescriptorCollection((PropertyDescriptor[]) null);
    foreach (PropertyDescriptor propertyDescr in properties1)
    {
      ItemToCreate.localPropertyDescriptor propertyDescriptor = new ItemToCreate.localPropertyDescriptor(propertyDescr);
      bool flag;
      switch (propertyDescr.Name)
      {
        case "Size":
          flag = true;
          propertyDescriptor.SetIsReadOnly(!this.isNew);
          break;
        case "LocalID":
          flag = false;
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

  protected class localPropertyDescriptor : PropertyDescriptor
  {
    private PropertyDescriptor propertyDescriptor;
    private bool isReadOnly = true;

    public localPropertyDescriptor(PropertyDescriptor propertyDescr)
      : base((MemberDescriptor) propertyDescr)
    {
      this.propertyDescriptor = propertyDescr;
    }

    public void SetIsReadOnly(bool isReadOnly) => this.isReadOnly = isReadOnly;

    public override bool CanResetValue(object component)
    {
      return this.propertyDescriptor.CanResetValue(component);
    }

    public override bool IsReadOnly => this.propertyDescriptor.IsReadOnly || this.isReadOnly;

    public override Type PropertyType => this.propertyDescriptor.PropertyType;

    public override void SetValue(object component, object value)
    {
      this.propertyDescriptor.SetValue(component, value);
    }

    public override bool ShouldSerializeValue(object component)
    {
      return this.propertyDescriptor.ShouldSerializeValue(component);
    }

    public override void ResetValue(object component)
    {
      this.propertyDescriptor.ResetValue(component);
    }

    public override object GetValue(object component)
    {
      return this.propertyDescriptor.GetValue(component);
    }

    public override Type ComponentType => this.propertyDescriptor.ComponentType;
  }
}

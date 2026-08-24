// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.Controls.CatalogBinding
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Imbase.Controls;

internal class CatalogBinding : ICustomTypeDescriptor
{
  private string _name = string.Empty;
  private string _tableName = string.Empty;
  private DateTime _created = DateTime.MinValue;
  private string _user = string.Empty;
  private ImTablesType _type;
  private bool _importing = true;
  public CatalogBindingChange BindingChanged;
  private CatalogImbaseAttProxy _bindingCatalog = new CatalogImbaseAttProxy();

  public CatalogBinding(
    string name,
    string tableName,
    DateTime created,
    string user,
    ImTablesType type,
    bool importing)
  {
    this._name = name;
    this._tableName = tableName;
    this._created = created;
    this._user = user;
    this._type = type;
    this._importing = importing;
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

  public object GetEditor(System.Type editorBaseType)
  {
    return TypeDescriptor.GetEditor((object) this, editorBaseType, true);
  }

  public EventDescriptorCollection GetEvents(Attribute[] attributes)
  {
    return TypeDescriptor.GetEvents((object) this, attributes, true);
  }

  public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents((object) this, true);

  [DisplayName("Название")]
  [Description("Отображаемое название каталога/справочника")]
  public string Name => this._name;

  [DisplayName("Имя таблицы")]
  [Description("Имя таблицы в базе данных")]
  public string TableName => this._tableName;

  [DisplayName("Дата создания")]
  [Description("Дата создания каталога/справочника")]
  public DateTime DateCreated => this._created;

  [DisplayName("Пользователь")]
  [Description("Имя пользователя, создавшего каталог/справочник")]
  public string User => this._user;

  [DisplayName("Привязка")]
  [Description("Каталог/справочник в базе назначения, которому сопоставлен текущий каталог/справочник")]
  public CatalogImbaseAttProxy BindingCatalog
  {
    get => this._bindingCatalog;
    set
    {
      if (this._bindingCatalog == value)
        return;
      this._bindingCatalog = value;
      if (this.BindingChanged == null)
        return;
      this.BindingChanged((object) this, new CatalogBindingEventArgs(this._tableName));
    }
  }

  [Browsable(false)]
  public ImTablesType Type => this._type;

  [DisplayName("Импорт")]
  [Description("Флаг, показывающий, будет ли каталог импортирован программой миграции")]
  public bool Importing
  {
    get => this._importing;
    set => this._importing = value;
  }
}

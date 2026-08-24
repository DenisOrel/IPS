// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.VarnishPropertiesDescriptor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Localization;
using Intermech.MaterialsHandbook.Controls.MaterialProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class VarnishPropertiesDescriptor : ICustomTypeDescriptor
{
  private PropertyDescriptorCollection _pdc = new PropertyDescriptorCollection((PropertyDescriptor[]) null);
  private VarnishItem _item;
  private MainSettingsDataProvider _dataProvider;

  public VarnishPropertiesDescriptor(VarnishItem item, MainSettingsDataProvider dataProvider)
  {
    this._item = item;
    this._dataProvider = dataProvider;
    this.CreatePdc();
  }

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

  public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents((object) this, true);

  public EventDescriptorCollection GetEvents(Attribute[] attributes)
  {
    return TypeDescriptor.GetEvents((object) this, attributes, true);
  }

  public PropertyDescriptorCollection GetProperties()
  {
    return this._pdc ?? new PropertyDescriptorCollection((PropertyDescriptor[]) null);
  }

  public PropertyDescriptorCollection GetProperties(Attribute[] attributes) => this.GetProperties();

  public object GetPropertyOwner(PropertyDescriptor pd) => (object) this;

  private void CreatePdc()
  {
    PropertyDescriptor[] properties1 = new PropertyDescriptor[4];
    Attribute[] attributes1 = new Attribute[3]
    {
      (Attribute) new CategoryAttribute(LocalizationHolder.rm.GetString("IMH_Settings")),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_Color")),
      (Attribute) new DescriptionAttribute("")
    };
    PropertyDescriptorCollection properties2 = TypeDescriptor.GetProperties((object) this._item);
    VarnishPropertyDescriptor propertyDescriptor = new VarnishPropertyDescriptor((object) this._item, properties2["Color"], "Color", attributes1, (TypeConverter) new ImbaseRecordLinkPropConverter(this.GetColors()));
    properties1[0] = (PropertyDescriptor) propertyDescriptor;
    Attribute[] attributes2 = new Attribute[3]
    {
      (Attribute) new CategoryAttribute(LocalizationHolder.rm.GetString("IMH_Settings")),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_CoatingClass")),
      (Attribute) new DescriptionAttribute("")
    };
    properties1[1] = (PropertyDescriptor) new VarnishPropertyDescriptor((object) this._item, properties2["CoatingClass"], "CoatingClass", attributes2, (TypeConverter) new MultiValuesAttributeConverter(Consts.CoatingClassAttrTypeGuid));
    Attribute[] attributes3 = new Attribute[3]
    {
      (Attribute) new CategoryAttribute(LocalizationHolder.rm.GetString("IMH_Settings")),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_CoatingGroup")),
      (Attribute) new DescriptionAttribute("")
    };
    properties1[2] = (PropertyDescriptor) new VarnishPropertyDescriptor((object) this._item, properties2["CoatingGroup"], "CoatingGroup", attributes3, (TypeConverter) new ImbaseRecordLinkPropConverter(this.GetCoatingGroups()));
    Attribute[] attributes4 = new Attribute[3]
    {
      (Attribute) new CategoryAttribute(LocalizationHolder.rm.GetString("IMH_Settings")),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_TermsOfUseTableName")),
      (Attribute) new DescriptionAttribute("")
    };
    properties1[3] = (PropertyDescriptor) new VarnishPropertyDescriptor((object) this._item, properties2["TermOfUse"], "TermsOfUse", attributes4, (TypeConverter) new ImbaseRecordLinkPropConverter(this.GetTermsOfUse()));
    this._pdc = new PropertyDescriptorCollection(properties1);
  }

  private IEnumerable<string> GetTermsOfUse()
  {
    return this._dataProvider.InternalExternalCoatingTable.AsEnumerable().Select<DataRow, string>((System.Func<DataRow, string>) (row => row[0].ToString())).Distinct<string>();
  }

  private IEnumerable<string> GetColors()
  {
    List<string> list = this._dataProvider.CoatingColorTable.AsEnumerable().Select<DataRow, string>((System.Func<DataRow, string>) (row => row[0].ToString())).Distinct<string>().ToList<string>();
    return list.Count != 1 || !list[0].Equals(string.Empty) ? (IEnumerable<string>) list : this._dataProvider.RALColorTable.AsEnumerable().Select<DataRow, string>((System.Func<DataRow, string>) (row => row[0].ToString())).Distinct<string>();
  }

  private IEnumerable<string> GetCoatingGroups()
  {
    return this._dataProvider.PrefDestTable.AsEnumerable().Select<DataRow, string>((System.Func<DataRow, string>) (row => row[0].ToString())).Distinct<string>();
  }
}

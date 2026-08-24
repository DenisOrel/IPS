// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ConfigSettingsCustomDescriptor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Localization;
using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class ConfigSettingsCustomDescriptor : ICustomTypeDescriptor
{
  private PropertyDescriptorCollection _pdc = new PropertyDescriptorCollection((PropertyDescriptor[]) null);

  internal ConfigSettingsCustomDescriptor(
    Dictionary<string, string> dictSettings,
    IMHCoatingsSystemSettings coatingsSettings)
  {
    this.CreatePDC(dictSettings, coatingsSettings);
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

  public EventDescriptorCollection GetEvents(Attribute[] attributes)
  {
    return TypeDescriptor.GetEvents((object) this, attributes, true);
  }

  public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents((object) this, true);

  public PropertyDescriptorCollection GetProperties(Attribute[] attributes) => this.GetProperties();

  public PropertyDescriptorCollection GetProperties()
  {
    return this._pdc != null ? this._pdc : new PropertyDescriptorCollection((PropertyDescriptor[]) null);
  }

  public object GetPropertyOwner(PropertyDescriptor pd) => (object) this;

  private void CreatePDC(
    Dictionary<string, string> dictSettings,
    IMHCoatingsSystemSettings coatingsSettings)
  {
    if (dictSettings == null || dictSettings.Count <= 0)
      return;
    List<PropertyDescriptor> propertyDescriptorList = new List<PropertyDescriptor>(11);
    string category1 = LocalizationHolder.rm.GetString("IMH_SystemProperties_CategoryName_Catalogs");
    object dictSetting1 = dictSettings.ContainsKey("BASE_MATERIALS_CTL") ? (object) dictSettings["BASE_MATERIALS_CTL"] : (object) (string) null;
    Attribute[] attributes1 = new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category1),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_BaseCatalogName")),
      (Attribute) new DescriptionAttribute(LocalizationHolder.rm.GetString("IMH_BaseCatalog_Description")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptorList.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "BASE_MATERIALS_CTL", attributes1, DisableImbaseCategory.Catalog, dictSetting1));
    object dictSetting2 = dictSettings.ContainsKey("ADDITION_MATERIALS_CTL") ? (object) dictSettings["ADDITION_MATERIALS_CTL"] : (object) (string) null;
    Attribute[] attributes2 = new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category1),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_AdditionCatalogName")),
      (Attribute) new DescriptionAttribute(LocalizationHolder.rm.GetString("IMH_AdditionCatalog_Description")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptorList.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "ADDITION_MATERIALS_CTL", attributes2, DisableImbaseCategory.Catalog, dictSetting2));
    string category2 = LocalizationHolder.rm.GetString("IMH_SystemProperties_CategoryName_Folders");
    object dictSetting3 = dictSettings.ContainsKey("ASSORTMENT_FOLDER_NAME") ? (object) dictSettings["ASSORTMENT_FOLDER_NAME"] : (object) (string) null;
    Attribute[] attributes3 = new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category2),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_SortamentFolderName")),
      (Attribute) new DescriptionAttribute(LocalizationHolder.rm.GetString("IMH_AssortmentFolder_Description")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptorList.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "ASSORTMENT_FOLDER_NAME", attributes3, DisableImbaseCategory.Folder, dictSetting3));
    object dictSetting4 = dictSettings.ContainsKey("GLUE_FOLDER_NAME") ? (object) dictSettings["GLUE_FOLDER_NAME"] : (object) (string) null;
    Attribute[] attributes4 = new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category2),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_GlueFolderName")),
      (Attribute) new DescriptionAttribute(LocalizationHolder.rm.GetString("IMH_GlueFolder_Description")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptorList.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "GLUE_FOLDER_NAME", attributes4, DisableImbaseCategory.Folder, dictSetting4));
    object dictSetting5 = dictSettings.ContainsKey("COATING_FOLDER_NAME") ? (object) dictSettings["COATING_FOLDER_NAME"] : (object) (string) null;
    Attribute[] attributes5 = new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category2),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_CoatingFolderName")),
      (Attribute) new DescriptionAttribute(LocalizationHolder.rm.GetString("IMH_CoatingFolder_Description")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptorList.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_FOLDER_NAME", attributes5, DisableImbaseCategory.Folder, dictSetting5));
    object dictSetting6 = dictSettings.ContainsKey("OIL_FOLDER_NAME") ? (object) dictSettings["OIL_FOLDER_NAME"] : (object) (string) null;
    Attribute[] attributes6 = new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category2),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_OilFolderName")),
      (Attribute) new DescriptionAttribute(LocalizationHolder.rm.GetString("IMH_OilFolder_Description")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptorList.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "OIL_FOLDER_NAME", attributes6, DisableImbaseCategory.Folder, dictSetting6));
    object dictSetting7 = dictSettings.ContainsKey("VARNISH_FOLDER_NAME") ? (object) dictSettings["VARNISH_FOLDER_NAME"] : (object) (string) null;
    Attribute[] attributes7 = new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category2),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_VarnishFolderName")),
      (Attribute) new DescriptionAttribute(LocalizationHolder.rm.GetString("IMH_VarnishFolder_Description")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptorList.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "VARNISH_FOLDER_NAME", attributes7, DisableImbaseCategory.Folder, dictSetting7));
    string category3 = LocalizationHolder.rm.GetString("IMH_SystemProperties_CategoryName_Tables");
    object dictSetting8 = dictSettings.ContainsKey("MATERIAL_SUBSTITUTES_TABLE_NAME") ? (object) dictSettings["MATERIAL_SUBSTITUTES_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor1 = new ConfigSettingsPropertyDescriptor((object) this, "MATERIAL_SUBSTITUTES_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_MaterialSubstitutesTableName")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting8);
    object dictSetting9 = dictSettings.ContainsKey("MATERIAL_SUBSTITUTES_COLUMN_MATERIAL") ? (object) dictSettings["MATERIAL_SUBSTITUTES_COLUMN_MATERIAL"] : (object) (string) null;
    Attribute[] attributes8 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_MaterialField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor1.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "MATERIAL_SUBSTITUTES_COLUMN_MATERIAL", attributes8, DisableImbaseCategory.Table, dictSetting9));
    object dictSetting10 = dictSettings.ContainsKey("MATERIAL_SUBSTITUTES_COLUMN_SUBSTITUTES") ? (object) dictSettings["MATERIAL_SUBSTITUTES_COLUMN_SUBSTITUTES"] : (object) (string) null;
    Attribute[] attributes9 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_SubstitutesField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor1.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "MATERIAL_SUBSTITUTES_COLUMN_SUBSTITUTES", attributes9, DisableImbaseCategory.Table, dictSetting10));
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor1);
    object dictSetting11 = dictSettings.ContainsKey("MATERIAL_GROUPS_TABLE_NAME") ? (object) dictSettings["MATERIAL_GROUPS_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor2 = new ConfigSettingsPropertyDescriptor((object) this, "MATERIAL_GROUPS_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_MaterialsGroupsTableName")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting11);
    object dictSetting12 = dictSettings.ContainsKey("MATERIAL_GROUPS_COLUMN_NAME") ? (object) dictSettings["MATERIAL_GROUPS_COLUMN_NAME"] : (object) (string) null;
    Attribute[] attributes10 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_DetailMaterialField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor2.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "MATERIAL_GROUPS_COLUMN_NAME", attributes10, DisableImbaseCategory.Table, dictSetting12));
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor2);
    object dictSetting13 = dictSettings.ContainsKey("MATERIAL_PROPERTIES_TABLE_NAME") ? (object) dictSettings["MATERIAL_PROPERTIES_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor3 = new ConfigSettingsPropertyDescriptor((object) this, "MATERIAL_PROPERTIES_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_MaterialsPropertiesTableName")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting13);
    object dictSetting14 = dictSettings.ContainsKey("MATERIAL_PROPERTIES_COLUMN_MATERIAL") ? (object) dictSettings["MATERIAL_PROPERTIES_COLUMN_MATERIAL"] : (object) (string) null;
    Attribute[] attributes11 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_MaterialField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor3.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "MATERIAL_PROPERTIES_COLUMN_MATERIAL", attributes11, DisableImbaseCategory.Table, dictSetting14));
    object dictSetting15 = dictSettings.ContainsKey("MATERIAL_PROPERTIES_COLUMN_OBJECT") ? (object) dictSettings["MATERIAL_PROPERTIES_COLUMN_OBJECT"] : (object) (string) null;
    Attribute[] attributes12 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_ObjectField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor3.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "MATERIAL_PROPERTIES_COLUMN_OBJECT", attributes12, DisableImbaseCategory.Table, dictSetting15));
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor3);
    object dictSetting16 = dictSettings.ContainsKey("COATING_PROPERTIES_TABLE_NAME") ? (object) dictSettings["COATING_PROPERTIES_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor4 = new ConfigSettingsPropertyDescriptor((object) this, "COATING_PROPERTIES_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_CoatingsProperties")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting16);
    object dictSetting17 = dictSettings.ContainsKey("COATING_PROPERTIES_COLUMN_COATING") ? (object) dictSettings["COATING_PROPERTIES_COLUMN_COATING"] : (object) (string) null;
    Attribute[] attributes13 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_CoatingField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor4.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_PROPERTIES_COLUMN_COATING", attributes13, DisableImbaseCategory.Table, dictSetting17));
    object dictSetting18 = dictSettings.ContainsKey("COATING_PROPERTIES_COLUMN_MATERIAL") ? (object) dictSettings["COATING_PROPERTIES_COLUMN_MATERIAL"] : (object) (string) null;
    Attribute[] attributes14 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_MaterialField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor4.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_PROPERTIES_COLUMN_MATERIAL", attributes14, DisableImbaseCategory.Table, dictSetting18));
    object dictSetting19 = dictSettings.ContainsKey("COATING_PROPERTIES_COLUMN_PURPOSE") ? (object) dictSettings["COATING_PROPERTIES_COLUMN_PURPOSE"] : (object) (string) null;
    Attribute[] attributes15 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_Coating_Purpose")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor4.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_PROPERTIES_COLUMN_PURPOSE", attributes15, DisableImbaseCategory.Table, dictSetting19));
    object dictSetting20 = dictSettings.ContainsKey("COATING_PROPERTIES_COLUMN_INSTRUCTIONS") ? (object) dictSettings["COATING_PROPERTIES_COLUMN_INSTRUCTIONS"] : (object) (string) null;
    Attribute[] attributes16 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_Coating_Instructions")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor4.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_PROPERTIES_COLUMN_INSTRUCTIONS", attributes16, DisableImbaseCategory.Table, dictSetting20));
    object obj1 = coatingsSettings != null ? (object) coatingsSettings : (object) new IMHCoatingsSystemSettings((DataTable) null);
    Attribute[] attributes17 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_ParametersSettings")),
      (Attribute) new TypeConverterAttribute(typeof (CoatingsSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (CoatingsSettingsUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor4.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "CoatingsParams", attributes17, DisableImbaseCategory.Table, obj1));
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor4);
    object dictSetting21 = dictSettings.ContainsKey("GLUE_MATERIAL_GROUPS_TABLE_NAME") ? (object) dictSettings["GLUE_MATERIAL_GROUPS_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor5 = new ConfigSettingsPropertyDescriptor((object) this, "GLUE_MATERIAL_GROUPS_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_Glue_MaterialGroups_TableName")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting21);
    object dictSetting22 = dictSettings.ContainsKey("GLUE_MATERIAL_GROUPS_COLUMN_NAME") ? (object) dictSettings["GLUE_MATERIAL_GROUPS_COLUMN_NAME"] : (object) (string) null;
    Attribute[] attributes18 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_MaterialName_FieldName")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor5.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "GLUE_MATERIAL_GROUPS_COLUMN_NAME", attributes18, DisableImbaseCategory.Table, dictSetting22));
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor5);
    object dictSetting23 = dictSettings.ContainsKey("GLUE_TABLE_NAME") ? (object) dictSettings["GLUE_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor6 = new ConfigSettingsPropertyDescriptor((object) this, "GLUE_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_Glues")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting23);
    object dictSetting24 = dictSettings.ContainsKey("GLUE_COLUMN_MATERIAL1") ? (object) dictSettings["GLUE_COLUMN_MATERIAL1"] : (object) (string) null;
    Attribute[] attributes19 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute($"{LocalizationHolder.rm.GetString("IMH_MaterialField")} 1"),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor6.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "GLUE_COLUMN_MATERIAL1", attributes19, DisableImbaseCategory.Table, dictSetting24));
    object dictSetting25 = dictSettings.ContainsKey("GLUE_COLUMN_MATERIAL2") ? (object) dictSettings["GLUE_COLUMN_MATERIAL2"] : (object) (string) null;
    Attribute[] attributes20 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute($"{LocalizationHolder.rm.GetString("IMH_MaterialField")} 2"),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor6.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "GLUE_COLUMN_MATERIAL2", attributes20, DisableImbaseCategory.Table, dictSetting25));
    object dictSetting26 = dictSettings.ContainsKey("GLUE_COLUMN_GLUE") ? (object) dictSettings["GLUE_COLUMN_GLUE"] : (object) (string) null;
    Attribute[] attributes21 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_GlueField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor6.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "GLUE_COLUMN_GLUE", attributes21, DisableImbaseCategory.Table, dictSetting26));
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor6);
    object dictSetting27 = dictSettings.ContainsKey("SURFACE_MATERIALS_TABLE_NAME") ? (object) dictSettings["SURFACE_MATERIALS_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor7 = new ConfigSettingsPropertyDescriptor((object) this, "SURFACE_MATERIALS_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_SurfaceMaterialsTableName")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting27);
    object dictSetting28 = dictSettings.ContainsKey("SURFACE_MATERIALS_COLUMN_NAME") ? (object) dictSettings["SURFACE_MATERIALS_COLUMN_NAME"] : (object) (string) null;
    Attribute[] attributes22 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_SurfaceMaterialsField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor7.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "SURFACE_MATERIALS_COLUMN_NAME", attributes22, DisableImbaseCategory.Table, dictSetting28));
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor7);
    object dictSetting29 = dictSettings.ContainsKey("COATING_MATERIALS_TABLE_NAME") ? (object) dictSettings["COATING_MATERIALS_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor8 = new ConfigSettingsPropertyDescriptor((object) this, "COATING_MATERIALS_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_CoatingMaterialsTableName")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting29);
    object dictSetting30 = dictSettings.ContainsKey("COATING_MATERIALS_COLUMN_MATERIALS") ? (object) dictSettings["COATING_MATERIALS_COLUMN_MATERIALS"] : (object) (string) null;
    Attribute[] attributes23 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_MaterialField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor8.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_MATERIALS_COLUMN_MATERIALS", attributes23, DisableImbaseCategory.Table, dictSetting30));
    object dictSetting31 = dictSettings.ContainsKey("COATING_MATERIALS_COLUMN_COATING") ? (object) dictSettings["COATING_MATERIALS_COLUMN_COATING"] : (object) (string) null;
    Attribute[] attributes24 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_CoatingField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor8.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_MATERIALS_COLUMN_COATING", attributes24, DisableImbaseCategory.Table, dictSetting31));
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor8);
    object dictSetting32 = dictSettings.ContainsKey("TERMS_USE_TABLE_NAME") ? (object) dictSettings["TERMS_USE_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor9 = new ConfigSettingsPropertyDescriptor((object) this, "TERMS_USE_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_TermsOfUseTableName")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting32);
    object dictSetting33 = dictSettings.ContainsKey("TERMS_USE_COLUMN_NAME") ? (object) dictSettings["TERMS_USE_COLUMN_NAME"] : (object) (string) null;
    Attribute[] attributes25 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_TermsOfUseField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor9.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "TERMS_USE_COLUMN_NAME", attributes25, DisableImbaseCategory.Table, dictSetting33));
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor9);
    object dictSetting34 = dictSettings.ContainsKey("COATING_TERMS_USE_TABLE_NAME") ? (object) dictSettings["COATING_TERMS_USE_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor10 = new ConfigSettingsPropertyDescriptor((object) this, "COATING_TERMS_USE_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_CoatingTermsOfUseTableName")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting34);
    object dictSetting35 = dictSettings.ContainsKey("COATING_TERMS_USE_COLUMN_TERMS") ? (object) dictSettings["COATING_TERMS_USE_COLUMN_TERMS"] : (object) (string) null;
    Attribute[] attributes26 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_TermsOfUseField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor10.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_TERMS_USE_COLUMN_TERMS", attributes26, DisableImbaseCategory.Table, dictSetting35));
    object dictSetting36 = dictSettings.ContainsKey("COATING_TERMS_USE_COLUMN_COATING") ? (object) dictSettings["COATING_TERMS_USE_COLUMN_COATING"] : (object) (string) null;
    Attribute[] attributes27 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_CoatingField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor10.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_TERMS_USE_COLUMN_COATING", attributes27, DisableImbaseCategory.Table, dictSetting36));
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor10);
    object dictSetting37 = dictSettings.ContainsKey("COATING_SPHERE_USE_TABLE_NAME") ? (object) dictSettings["COATING_SPHERE_USE_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor11 = new ConfigSettingsPropertyDescriptor((object) this, "COATING_SPHERE_USE_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_CoatingSphereOfUseTableName")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting37);
    object dictSetting38 = dictSettings.ContainsKey("COATING_SPHERE_USE_COLUMN_SPHERE") ? (object) dictSettings["COATING_SPHERE_USE_COLUMN_SPHERE"] : (object) (string) null;
    Attribute[] attributes28 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_SphereOfUseField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor11.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_SPHERE_USE_COLUMN_SPHERE", attributes28, DisableImbaseCategory.Table, dictSetting38));
    object dictSetting39 = dictSettings.ContainsKey("COATING_SPHERE_USE_COLUMN_COATING") ? (object) dictSettings["COATING_SPHERE_USE_COLUMN_COATING"] : (object) (string) null;
    Attribute[] attributes29 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_CoatingField")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor11.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_SPHERE_USE_COLUMN_COATING", attributes29, DisableImbaseCategory.Table, dictSetting39));
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor11);
    object dictSetting40 = dictSettings.ContainsKey("COATING_INTERNAL_EXTERNAL_TABLE_NAME") ? (object) dictSettings["COATING_INTERNAL_EXTERNAL_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor12 = new ConfigSettingsPropertyDescriptor((object) this, "COATING_INTERNAL_EXTERNAL_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_Internal_External_Coating")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting40);
    object dictSetting41 = dictSettings.ContainsKey("COATING_INTERNAL_EXTERNAL_INTERNAL_COLUMN") ? (object) dictSettings["COATING_INTERNAL_EXTERNAL_INTERNAL_COLUMN"] : (object) (string) null;
    Attribute[] attributes30 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_Internal_Coating")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor12.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_INTERNAL_EXTERNAL_INTERNAL_COLUMN", attributes30, DisableImbaseCategory.Table, dictSetting41));
    object dictSetting42 = dictSettings.ContainsKey("COATING_INTERNAL_EXTERNAL_EXTERNAL_WITH_CONDITION_COLUMN") ? (object) dictSettings["COATING_INTERNAL_EXTERNAL_EXTERNAL_WITH_CONDITION_COLUMN"] : (object) (string) null;
    Attribute[] attributes31 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_External_Coating_With_Condidtion_Use")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor12.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_INTERNAL_EXTERNAL_EXTERNAL_WITH_CONDITION_COLUMN", attributes31, DisableImbaseCategory.Table, dictSetting42));
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor12);
    object dictSetting43 = dictSettings.ContainsKey("COATING_PREFERRED_DESTINATION_TABLE_NAME") ? (object) dictSettings["COATING_PREFERRED_DESTINATION_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor13 = new ConfigSettingsPropertyDescriptor((object) this, "COATING_PREFERRED_DESTINATION_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_Coating_Preferred_Destination")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting43);
    object dictSetting44 = dictSettings.ContainsKey("COATING_PREFERRED_DESTINATION_COLUMN_COATING") ? (object) dictSettings["COATING_PREFERRED_DESTINATION_COLUMN_COATING"] : (object) (string) null;
    Attribute[] attributes32 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_Coating_Varnish")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor13.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_PREFERRED_DESTINATION_COLUMN_COATING", attributes32, DisableImbaseCategory.Table, dictSetting44));
    object dictSetting45 = dictSettings.ContainsKey("COATING_PREFERRED_DESTINATION_COLUMN_PURPOSE") ? (object) dictSettings["COATING_PREFERRED_DESTINATION_COLUMN_PURPOSE"] : (object) (string) null;
    Attribute[] attributes33 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_Destination")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor13.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_PREFERRED_DESTINATION_COLUMN_PURPOSE", attributes33, DisableImbaseCategory.Table, dictSetting45));
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor13);
    object dictSetting46 = dictSettings.ContainsKey("COATING_COLOR_TABLE_NAME") ? (object) dictSettings["COATING_COLOR_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor14 = new ConfigSettingsPropertyDescriptor((object) this, "COATING_COLOR_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_Coating_And_Color")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting46);
    object dictSetting47 = dictSettings.ContainsKey("COATING_COLOR_COLUMN_COATING") ? (object) dictSettings["COATING_COLOR_COLUMN_COATING"] : (object) (string) null;
    Attribute[] attributes34 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_Coating")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor14.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_COLOR_COLUMN_COATING", attributes34, DisableImbaseCategory.Table, dictSetting47));
    object dictSetting48 = dictSettings.ContainsKey("COATING_COLOR_COLUMN_COLOR") ? (object) dictSettings["COATING_COLOR_COLUMN_COLOR"] : (object) (string) null;
    Attribute[] attributes35 = new Attribute[3]
    {
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_Color")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (FieldSelectorUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptor14.ChildProperties.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COATING_COLOR_COLUMN_COLOR", attributes35, DisableImbaseCategory.Table, dictSetting48));
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor14);
    object dictSetting49 = dictSettings.ContainsKey("COATING_COLOR_RAL_TABLE_NAME") ? (object) dictSettings["COATING_COLOR_RAL_TABLE_NAME"] : (object) (string) null;
    ConfigSettingsPropertyDescriptor propertyDescriptor15 = new ConfigSettingsPropertyDescriptor((object) this, "COATING_COLOR_RAL_TABLE_NAME", new Attribute[5]
    {
      (Attribute) new CategoryAttribute(category3),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_Color_RAL")),
      (Attribute) new DescriptionAttribute(""),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsUITypeEditor), typeof (UITypeEditor))
    }, DisableImbaseCategory.Table, dictSetting49);
    propertyDescriptorList.Add((PropertyDescriptor) propertyDescriptor15);
    string category4 = LocalizationHolder.rm.GetString("IMH_SystemProperties_CategoryName_Attributes");
    object dictSetting50 = dictSettings.ContainsKey("BASE_MATERIAL_ATTR") ? (object) dictSettings["BASE_MATERIAL_ATTR"] : (object) (string) null;
    Attribute[] attributes36 = new Attribute[4]
    {
      (Attribute) new CategoryAttribute(category4),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_BaseMaterial_Attribute_Name")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsForAttributesUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptorList.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "BASE_MATERIAL_ATTR", attributes36, DisableImbaseCategory.Attribute, dictSetting50));
    object dictSetting51 = dictSettings.ContainsKey("COLOR_VARNISH_ATTR") ? (object) dictSettings["COLOR_VARNISH_ATTR"] : (object) (string) null;
    Attribute[] attributes37 = new Attribute[4]
    {
      (Attribute) new CategoryAttribute(category4),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_ColorOfVarnishes_Attribute_Name")),
      (Attribute) new TypeConverterAttribute(typeof (ConfigSettingsTypeConverter)),
      (Attribute) new EditorAttribute(typeof (ConfigSettingsForAttributesUITypeEditor), typeof (UITypeEditor))
    };
    propertyDescriptorList.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "COLOR_VARNISH_ATTR", attributes37, DisableImbaseCategory.Attribute, dictSetting51));
    string category5 = LocalizationHolder.rm.GetString("IMH_SystemProperties_CategoryName_DisplaySettings");
    object obj2 = dictSettings.ContainsKey("DISPLAY_SETTING_SHOW_RECORDS") ? (object) dictSettings["DISPLAY_SETTING_SHOW_RECORDS"] : (object) Convert.ToString(true);
    Attribute[] attributes38 = new Attribute[4]
    {
      (Attribute) new CategoryAttribute(category5),
      (Attribute) new DisplayNameAttribute(LocalizationHolder.rm.GetString("IMH_DisplayAllRecords_Name")),
      (Attribute) new DescriptionAttribute(LocalizationHolder.rm.GetString("IMH_DisplayAllRecords_Descriptions")),
      (Attribute) new TypeConverterAttribute(typeof (YesNoConverter))
    };
    propertyDescriptorList.Add((PropertyDescriptor) new ConfigSettingsPropertyDescriptor((object) this, "DISPLAY_SETTING_SHOW_RECORDS", attributes38, DisableImbaseCategory.None, obj2));
    this._pdc = new PropertyDescriptorCollection(propertyDescriptorList.ToArray());
  }

  private void SaveProperty(PropertyDescriptor descr, Dictionary<string, string> dict)
  {
    if (descr == null)
      return;
    string str = Convert.ToString(descr.GetValue((object) this));
    if (GuidHelper.IsGuid(str))
    {
      Guid guid = new Guid(str);
      dict.Add(descr.Name, guid != Guid.Empty ? str : string.Empty);
    }
    else
      dict.Add(descr.Name, str);
  }

  internal void GetSettings(
    out Dictionary<string, string> dict,
    out IMHCoatingsSystemSettings coatingsSettings)
  {
    dict = new Dictionary<string, string>(26);
    coatingsSettings = (IMHCoatingsSystemSettings) null;
    foreach (PropertyDescriptor descr in this._pdc)
    {
      this.SaveProperty(descr, dict);
      if (descr is ConfigSettingsPropertyDescriptor propertyDescriptor && propertyDescriptor.PropertiesSupported)
      {
        foreach (PropertyDescriptor childProperty in propertyDescriptor.ChildProperties)
        {
          if (childProperty.Name == "CoatingsParams")
            coatingsSettings = childProperty.GetValue((object) this) as IMHCoatingsSystemSettings;
          else
            this.SaveProperty(childProperty, dict);
        }
      }
    }
  }
}

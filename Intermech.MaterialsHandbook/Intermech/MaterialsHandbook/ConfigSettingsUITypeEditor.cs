// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ConfigSettingsUITypeEditor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.DataFormats;
using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class ConfigSettingsUITypeEditor : UITypeEditor
{
  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    IDescriptor rootDescriptor = (IDescriptor) new ImbaseRootNodeDescriptor();
    AdvancedServiceContainer nodesContext = new AdvancedServiceContainer(provider);
    if (context != null)
    {
      if (context.PropertyDescriptor is ConfigSettingsPropertyDescriptor propertyDescriptor && propertyDescriptor.DescriptorCategory == DisableImbaseCategory.Catalog)
        nodesContext.AddService(typeof (ImbaseDisableCatalogsComposition), (object) new ImbaseDisableCatalogsComposition(DisableImbaseCategory.Catalog));
      else if (propertyDescriptor != null && propertyDescriptor.DescriptorCategory == DisableImbaseCategory.Folder)
      {
        nodesContext.AddService(typeof (ImbaseDisableCatalogsComposition), (object) new ImbaseDisableCatalogsComposition(DisableImbaseCategory.Folder));
        Intermech.Navigator.SelectionWindow.RegisterAnalyze((ISelectedItemsAnalyzer) new TypedObjectsSelectedItemsAnalyzer(Intermech.Imbase.Consts.ImbaseFolderTypeID, false), true);
      }
      else if (propertyDescriptor != null && propertyDescriptor.DescriptorCategory == DisableImbaseCategory.Table)
      {
        nodesContext.AddService(typeof (ImbaseDisableCatalogsComposition), (object) new ImbaseDisableCatalogsComposition(DisableImbaseCategory.Table));
        Intermech.Navigator.SelectionWindow.RegisterAnalyze((ISelectedItemsAnalyzer) new TypedObjectsSelectedItemsAnalyzer(Intermech.Imbase.Consts.ImbaseTableRefTypeID, false), true);
      }
    }
    if (Intermech.Navigator.SelectionWindow.Select(LocalizationHolder.rm.GetString("IMH_SelectCatalogIMBASE"), rootDescriptor, typeof (IDBObjectID), (IServiceProvider) nodesContext, SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect) is IDBObjectID[] dbObjectIdArray && dbObjectIdArray.Length != 0)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(dbObjectIdArray[0].Value);
        if (!objectInfo.Empty)
          value = (object) objectInfo.VersionGuid;
      }
    }
    return value;
  }
}

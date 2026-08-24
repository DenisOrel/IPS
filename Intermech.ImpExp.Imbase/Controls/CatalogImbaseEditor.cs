// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.Controls.CatalogImbaseEditor
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Imbase.Controls;

public class CatalogImbaseEditor : UITypeEditor
{
  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return context != null && context.PropertyDescriptor.IsReadOnly ? UITypeEditorEditStyle.None : UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider sp,
    object value)
  {
    if (ImbasePlugin.selectCatalogsForm.ShowDialog() == DialogResult.OK)
    {
      CatalogPres selectedCatalog = ImbasePlugin.selectCatalogsForm.SelectedCatalog;
      if (selectedCatalog != null)
        return (object) new CatalogImbaseAttProxy(selectedCatalog.ID, selectedCatalog.Name);
    }
    return value;
  }
}

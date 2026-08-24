// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.MeasureProdDropDownEditor
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

public class MeasureProdDropDownEditor : UITypeEditor
{
  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    if (context == null || provider == null || !(provider.GetService(typeof (IWindowsFormsEditorService)) is IWindowsFormsEditorService service))
      return base.EditValue(context, provider, value);
    PropertyGrid propertyGrid1 = new PropertyGrid();
    propertyGrid1.Dock = DockStyle.Fill;
    propertyGrid1.ToolbarVisible = false;
    propertyGrid1.PropertySort = PropertySort.NoSort;
    propertyGrid1.HelpVisible = false;
    propertyGrid1.SelectedObject = (object) (value as EntMeasureProdSetting);
    PropertyGrid propertyGrid2 = propertyGrid1;
    service.DropDownControl((Control) propertyGrid2);
    return value;
  }

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return context != null ? UITypeEditorEditStyle.DropDown : base.GetEditStyle((ITypeDescriptorContext) null);
  }
}

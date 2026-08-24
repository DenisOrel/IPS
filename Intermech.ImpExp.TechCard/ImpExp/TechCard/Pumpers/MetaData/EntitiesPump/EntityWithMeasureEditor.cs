// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntityWithMeasureEditor
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Advanced;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

public class EntityWithMeasureEditor : UITypeEditor
{
  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return context == null ? base.GetEditStyle((ITypeDescriptorContext) null) : UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    if (context == null || provider == null)
      return base.EditValue(context, provider, value);
    if ((IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService)) == null)
      return base.EditValue(context, provider, value);
    List<Entity> entityList = new List<Entity>();
    if (context.Instance is EntityDescriptor instance)
    {
      string currentEntityCode = Convert.ToString(value);
      entityList = instance.EntityList;
      entityList.RemoveAll((Predicate<Entity>) (item => item.Code == currentEntityCode));
    }
    using (EntitySelectorForm entitySelectorForm = new EntitySelectorForm("Выбор понятия", entityList, FieldTypes.ftString, (Entity) null))
    {
      if (entitySelectorForm.ShowDialog() == DialogResult.OK)
        value = (entitySelectorForm.SelectedItem is Entity selectedItem ? (object) selectedItem.Code : (object) null) ?? value;
    }
    return value;
  }
}

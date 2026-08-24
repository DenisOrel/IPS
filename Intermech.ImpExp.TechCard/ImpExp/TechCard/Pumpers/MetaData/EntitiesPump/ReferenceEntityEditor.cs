// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.ReferenceEntityEditor
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

public class ReferenceEntityEditor : UITypeEditor
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
    Entity entity1 = (Entity) null;
    List<Entity> entityList = new List<Entity>();
    if (context.Instance is EntityDescriptor instance)
    {
      entity1 = instance.Entity;
      entityList = instance.EntityList;
      if (entityList.Contains(entity1))
        entityList.Remove(entity1);
      for (int index = entityList.Count - 1; index >= 0; --index)
      {
        Entity entity2 = entityList[index];
        if (entity2.Settings.PumpMode == EntityPumModes.ExistEntity || entity2.Settings.Properties.Status == EntityPumpStatus.None || entity2.Settings.Properties.Status == EntityPumpStatus.NotPump)
          entityList.Remove(entity2);
      }
    }
    using (EntitySelectorForm entitySelectorForm = new EntitySelectorForm("Выбор понятия", entityList, (FieldTypes) ((int) entity1?.Settings?.Properties?.FieldType ?? 1), (Entity) null))
    {
      if (entitySelectorForm.ShowDialog() == DialogResult.OK)
        value = entitySelectorForm.SelectedItem;
    }
    return value;
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Advanced.EntityAttributeTypeEditor
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#nullable disable
namespace Intermech.ImpExp.TechCard.Advanced;

public class EntityAttributeTypeEditor : UITypeEditor
{
  private object EditAttrValue(EntityDescriptor entityDescriptor, object value)
  {
    using (AttributeTypeSelectorForm typeSelectorForm = new AttributeTypeSelectorForm("Выбор атрибута", entityDescriptor?.Entity))
    {
      if (typeSelectorForm.ShowDialog() == DialogResult.OK)
        value = typeSelectorForm.SelectedItem;
    }
    return value;
  }

  private object EditImbaseAttrValue(EntityDescriptor entityDescriptor, object value)
  {
    if (entityDescriptor == null)
      throw new ArgumentNullException(nameof (entityDescriptor));
    return value;
  }

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return context == null ? base.GetEditStyle((ITypeDescriptorContext) null) : UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    if (context != null && provider != null && (IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService)) != null)
      value = context.Instance is EntityDescriptor instance ? (instance.Status != EntityDescriptor.Modes.PumpToImbaseAttr ? this.EditAttrValue(instance, value) : this.EditImbaseAttrValue(instance, value)) : this.EditAttrValue((EntityDescriptor) null, value);
    return base.EditValue(context, provider, value);
  }
}

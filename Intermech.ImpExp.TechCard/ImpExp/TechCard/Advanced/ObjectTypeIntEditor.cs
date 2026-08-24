// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Advanced.ObjectTypeIntEditor
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#nullable disable
namespace Intermech.ImpExp.TechCard.Advanced;

internal class ObjectTypeIntEditor : UITypeEditor
{
  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return context != null ? UITypeEditorEditStyle.Modal : base.GetEditStyle(context);
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    if (context != null && provider != null && provider.GetService(typeof (IWindowsFormsEditorService)) is IWindowsFormsEditorService service)
    {
      switch (value)
      {
        case int _:
        case long _:
          Convert.ToInt32(value);
          using (ObjectTypeSelectorForm dialog = new ObjectTypeSelectorForm("Выберите тип объекта"))
          {
            if (service.ShowDialog((Form) dialog).Equals((object) DialogResult.OK))
              return (object) dialog.ObjType;
            break;
          }
      }
    }
    return base.EditValue(context, provider, value);
  }
}

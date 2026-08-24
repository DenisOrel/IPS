// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.AttributeOptionsDropDownEditor
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

public class AttributeOptionsDropDownEditor : UITypeEditor
{
  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    if (context != null && provider != null)
    {
      IWindowsFormsEditorService service = (IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService));
      if (service != null)
      {
        int int32 = Convert.ToInt32(value);
        CheckedListBox checkedListBox1 = new CheckedListBox();
        checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
        checkedListBox1.CheckOnClick = true;
        checkedListBox1.Height = 64 /*0x40*/;
        CheckedListBox checkedListBox2 = checkedListBox1;
        Array values = Enum.GetValues(typeof (AttributeOptions));
        int index1 = 0;
        for (int index2 = 0; index2 < values.Length; ++index2)
        {
          AttributeOptions attributeOptions = (AttributeOptions) values.GetValue(index2);
          if (attributeOptions != AttributeOptions.None)
          {
            checkedListBox2.Items.Add((object) EnumDescConverter.GetEnumDescription((Enum) attributeOptions));
            checkedListBox2.SetItemChecked(index1, (int32 | Convert.ToInt32((object) attributeOptions)) == int32);
            ++index1;
          }
        }
        service.DropDownControl((Control) checkedListBox2);
        int num = 0;
        foreach (int checkedIndex in checkedListBox2.CheckedIndices)
          num |= Convert.ToInt32((object) (AttributeOptions) values.GetValue(checkedIndex));
        return (object) num;
      }
    }
    return base.EditValue(context, provider, value);
  }

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return context == null ? base.GetEditStyle((ITypeDescriptorContext) null) : UITypeEditorEditStyle.DropDown;
  }
}

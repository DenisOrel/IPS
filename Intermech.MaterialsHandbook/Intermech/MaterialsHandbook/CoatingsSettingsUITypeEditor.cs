// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.CoatingsSettingsUITypeEditor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Interfaces;
using Intermech.Interfaces.MaterialsHandbook;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class CoatingsSettingsUITypeEditor : UITypeEditor
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
    if (value != null)
    {
      IMHCoatingsSystemSettings settings = value as IMHCoatingsSystemSettings;
      if (context != null)
      {
        string str = Convert.ToString(context.Instance);
        if (GuidHelper.IsGuid(str))
        {
          using (CoatingsSettingsForm coatingsSettingsForm = new CoatingsSettingsForm(new Guid(str), settings))
          {
            if (coatingsSettingsForm.ShowDialog() == DialogResult.OK)
              value = (object) coatingsSettingsForm.Settings;
          }
        }
      }
    }
    return value;
  }
}

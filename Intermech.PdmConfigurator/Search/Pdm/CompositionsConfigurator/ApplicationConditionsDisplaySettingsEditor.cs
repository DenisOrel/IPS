// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionsConfigurator.ApplicationConditionsDisplaySettingsEditor
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.CompositionsConfigurator;

public sealed class ApplicationConditionsDisplaySettingsEditor : UITypeEditor
{
  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    ApplicationConditionsDisplaySettings settings = (ApplicationConditionsDisplaySettings) null;
    if (!string.IsNullOrEmpty(value as string))
      settings = CompositionsConfiguratorHelper.ConvertStringLoadedFromConfigurationToApplicationConditionsDisplaySettings((string) value);
    using (ApplicationConditionsDisplaySettingsForm displaySettingsForm = new ApplicationConditionsDisplaySettingsForm())
    {
      if (settings != null)
        displaySettingsForm.Settings = settings;
      if (displaySettingsForm.ShowDialog() == DialogResult.OK)
        settings = displaySettingsForm.Settings;
    }
    if (settings == null)
      return (object) null;
    return (object) CompositionsConfiguratorHelper.ConvertApplicationConditionsDisplaySettingsToStringForSaveToConfiguration(settings);
  }

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return UITypeEditorEditStyle.Modal;
  }
}

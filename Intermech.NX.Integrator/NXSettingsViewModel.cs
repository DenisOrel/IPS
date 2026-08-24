// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXSettingsViewModel
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.PropertyEditors;
using Intermech.Tools.Integrators.CADInterface;
using System.ComponentModel;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXSettingsViewModel(NXSettingsFactory factory) : CADSettingsViewModel((CADSettingsFactory) factory)
{
  private bool enableModelJTFiles;

  public override void LoadContent(CADSettings settings)
  {
    base.LoadContent(settings);
    this.enableModelJTFiles = ((NXSettings) settings).EnableModelJTFiles;
  }

  public override void SaveContent(CADSettings settings)
  {
    base.SaveContent(settings);
    ((NXSettings) settings).EnableModelJTFiles = this.enableModelJTFiles;
  }

  protected override void ResetPropertiesToDefaults()
  {
    base.ResetPropertiesToDefaults();
    this.EnableModelJTFiles = false;
  }

  protected override void DoAssign(CADSettingsViewModel source)
  {
    base.DoAssign(source);
    if (!(source is NXSettingsViewModel settingsViewModel))
      return;
    this.EnableModelJTFiles = settingsViewModel.EnableModelJTFiles;
  }

  [DisplayName("Включить поддержку файлов __model.jt")]
  [Description("Если включено, то интегратор будет искать файлы __model.jt на диске и добавлять их к соответствующим документам NX в качестве дополнительных файлов.")]
  [Category("2. Сохранение изменений")]
  [TypeConverter(typeof (YesNoConverter))]
  public bool EnableModelJTFiles
  {
    get => this.enableModelJTFiles;
    set => this.enableModelJTFiles = value;
  }
}

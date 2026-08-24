// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.DXDSettingsSurrogate
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.PropertyEditors.ChangeHighlighting;
using Intermech.Tools.Integrators.Electrical;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class DXDSettingsSurrogate : MGSettingsSurrogate
{
  private ChangeTrackingListAdapter<NotImportedBoardSettingsSurrogate> notImportedBoardSettings;
  private string mainSchemeId;
  private string sheet;

  public DXDSettingsSurrogate(MGIntegratorSettings settings)
    : base(settings)
  {
    this.sheet = settings.Sheet;
    this.mainSchemeId = settings.MainSchemeId;
    if (settings.NotImportetBoardSettings != null)
      this.notImportedBoardSettings = new ChangeTrackingListAdapter<NotImportedBoardSettingsSurrogate>((IEnumerable<NotImportedBoardSettingsSurrogate>) settings.NotImportetBoardSettings.ConvertAll<NotImportedBoardSettingsSurrogate>((Converter<Tuple<StringKey, StringKey>, NotImportedBoardSettingsSurrogate>) (item => new NotImportedBoardSettingsSurrogate()
      {
        ParameterName = (string) item.Item1,
        ParameterValue = (string) item.Item2
      })));
    else
      this.notImportedBoardSettings = new ChangeTrackingListAdapter<NotImportedBoardSettingsSurrogate>();
  }

  protected override void SaveSettings(MGIntegratorSettings settings)
  {
    base.SaveSettings(settings);
    settings.Sheet = this.Sheet;
    settings.MainSchemeId = this.MainSchemeId;
    settings.NotImportetBoardSettings = new List<Tuple<StringKey, StringKey>>();
    foreach (NotImportedBoardSettingsSurrogate importedBoardSetting in this.notImportedBoardSettings)
      settings.NotImportetBoardSettings.Add(new Tuple<StringKey, StringKey>((StringKey) importedBoardSetting.ParameterName, (StringKey) importedBoardSetting.ParameterValue));
  }

  public override object Clone() => (object) new DXDSettingsSurrogate(this.Settings);

  [Category("Настройки штампа схемы")]
  [DisplayName("Параметр, идентифицирующий главную схему")]
  [Description("Параметр штампа, наличие которого идентифицирует главную схему проекта.")]
  public string MainSchemeId
  {
    get => this.mainSchemeId;
    set => this.mainSchemeId = value;
  }

  [Category("Настройки штампа схемы")]
  [DisplayName("Параметр для номера листа")]
  [Description("Параметр штампа, наличие которого идентифицирует штамп, а также указывает номер листа.")]
  public string Sheet
  {
    get => this.sheet;
    set => this.sheet = value;
  }

  [Category("Разное")]
  [DisplayName("Не заносить в IPS")]
  [Description("Это свойство позволяет определить какие Board проекта не будут импортированы в IPS.")]
  [Editor(typeof (ListParamValuesSettingsUIEditor<NotImportedBoardSettingsSurrogate>), typeof (UITypeEditor))]
  public ChangeTrackingListAdapter<NotImportedBoardSettingsSurrogate> NotImportedBoardSettings
  {
    get => this.notImportedBoardSettings;
    set => this.notImportedBoardSettings = value;
  }
}

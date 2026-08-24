// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.UpdateControlDataPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TechTypes.Settings;
using Intermech.ImpExp.TechCard.TechTypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData;

[TaskDescription("Инициализация загрузки типов объектов Techcard", "Загрузка типов объектов Techcard")]
[TaskType(PumperType.MetaData)]
internal class UpdateControlDataPump : PumpClass
{
  private readonly Guid _guid = new Guid("{CF2E86C1-FD5D-45CB-AE5F-C36A53365676}");
  private readonly IAppManager _manager;
  private readonly List<StepControl> _controls = new List<StepControl>();
  private TechTypeSettingsControl _typeSettingsControl;
  private EntitiesPumpSettings _entityControl;

  private void InitializeData()
  {
    this._typeSettingsControl = new TechTypeSettingsControl((object) this._manager);
    this._controls.Add((StepControl) this._typeSettingsControl);
    this._entityControl = new EntitiesPumpSettings((object) this._manager);
    this._controls.Add((StepControl) this._entityControl);
  }

  private void UpdateTypeSettingsControl()
  {
    if (this._typeSettingsControl == null)
      return;
    TechTypeList techTypeList;
    if (TechTypeListHelper.LoadFromSettings(out techTypeList))
    {
      foreach (KeyValuePair<int, TechTypeInfo> techType in (Dictionary<int, TechTypeInfo>) TechPumpData.TechType.TechTypeList)
      {
        TechTypeConvertionRule typeConvertionRule;
        TechTypeInfo techTypeInfo;
        if ((!TechTypeConversionPredefinedRules.ObjectTypeImportRules.TryGetValue(techType.Key, out typeConvertionRule) || typeConvertionRule == null || !typeConvertionRule.Mode.HasFlag((Enum) TechTypeConvertionRuleMode.ReadOnly)) && techTypeList.TryGetValue(techType.Key, out techTypeInfo) && techTypeInfo != null && techTypeInfo.TypeSett != null)
          techType.Value.TypeSett = techTypeInfo.TypeSett;
      }
    }
    this._typeSettingsControl.BeginInvoke((Delegate) new SetTechTypeList(this._typeSettingsControl.SetTechTypeList), (object) TechPumpData.TechType.TechTypeList);
  }

  private void UpdateEntityControl()
  {
    Dictionary<string, Entity> settingsAtPumpSett = this._entityControl.GetEntitySettingsAtPumpSett();
    if (settingsAtPumpSett != null)
    {
      this.plugin.appManager.AddInfoMessage("Правила конвертации технологических параметров успешно загружены из настроек");
      this._entityControl.BeginInvoke((Delegate) new SetValues(this._entityControl.LoadEntitySettings), (object) settingsAtPumpSett);
    }
    else
      this._entityControl.BeginInvoke((Delegate) new SetValues(this._entityControl.SetValues), (object) TechPumpData.Entities.EntitiesList);
  }

  public UpdateControlDataPump(PluginClass plugin, IAppManager manager)
    : base(plugin)
  {
    this._manager = manager;
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
    this.InitializeData();
  }

  public override void Exam()
  {
    this.ExamCheckPoint("Обновление настроек миграции TechCard", 0);
    try
    {
      this.UpdateTypeSettingsControl();
      this.ExamCheckPoint("Обновление настроек миграции TechCard", 50);
      this.UpdateEntityControl();
    }
    finally
    {
      this.ExamCheckPoint("Обновление настроек миграции TechCard завершено", 100);
    }
  }

  public override void Pump()
  {
  }

  protected override Guid GUID => this._guid;

  public List<StepControl> Controls => this._controls;
}

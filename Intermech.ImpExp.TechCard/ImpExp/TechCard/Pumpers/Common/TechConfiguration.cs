// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechConfiguration
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using System.Configuration;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common;

internal class TechConfiguration
{
  private readonly IAppManager _manager;

  private void InitializeData()
  {
    this.ImbaseCeh2ObjectPumpMode = this.GetConfigValue("TechCard.Ceh2ObjectPumpMode");
    if (this.ImbaseCeh2ObjectPumpMode)
      this._manager?.AddWarningMessage("Внимание! Включен режим привязки понятий со ссылками на справочник Цехов в объекты \"Цеха\"  !!");
    this.ZagotLink2ImbasePumpMode = this.GetConfigValue("TechCard.ZagotLink2Imbase") || this.GetConfigValue("TechCard.ZagotLink2ImbaseMode");
    if (this.ZagotLink2ImbasePumpMode)
      this._manager?.AddWarningMessage("Внимание! Включен режим привязки заготовки к справочникам Imbase!!");
    this.CehZahodIgnoreAreaPumpMode = this.GetConfigValue("TechCard.CehZahodCehOnlyMode") || this.GetConfigValue("TechCard.CehZahodIgnoreAreaMode");
    if (this.CehZahodIgnoreAreaPumpMode)
      this._manager?.AddWarningMessage("Внимание! Включен режим миграции цехозаходов только по цехам!!");
    this.CehZahodProductionPumpMode = this.GetConfigValue("TechCard.CehZahodProductionMode");
    if (this.CehZahodProductionPumpMode)
      this._manager?.AddWarningMessage("Внимание! Включен режим миграции цехозаходов с учетом вида производства!!");
    this.SpecToolDirectLinkPumpMode = this.GetConfigValue("TechCard.SpecToolSearchMode") || this.GetConfigValue("TechCard.SpecToolDirectLinkMode");
    if (this.SpecToolDirectLinkPumpMode)
      this._manager?.AddWarningMessage("Внимание! Включен режим миграции спецоснастки непосредственно в спецоснастку!!");
    this.UniqueObjectsLookupIpsPumpMode = this.GetConfigValue("TechCard.LoadMaterialFromIPS") || this.GetConfigValue("TechCard.UniqueObjectsLookupIpsMode");
    if (this.UniqueObjectsLookupIpsPumpMode)
      this._manager?.AddWarningMessage("Внимание! Включен режим загрузки / поиска технологических объектов в IPS!!");
    this.SkipMaterialComposition = this.GetConfigValue("TechCard.SkipMaterialComposition");
    if (this.SkipMaterialComposition)
      this._manager?.AddWarningMessage("Внимание! Включен режим миграции составных материалов (рецептур) без состава !!");
    this.CheckDopTablesDuplicates = this.GetConfigValue("TechCard.CheckDopTablesDuplicates");
    if (!this.CheckDopTablesDuplicates)
      return;
    this._manager?.AddWarningMessage("Внимание! Включен режим проверки дубликатов в дополнительных таблицах объектов БД Techcard");
  }

  private bool GetConfigValue(string paramName)
  {
    string s = ConfigurationManager.AppSettings.Get(paramName);
    bool result1;
    if (!bool.TryParse(s, out result1))
    {
      int result2;
      result1 = int.TryParse(s, out result2) && result2 > 0;
    }
    return result1;
  }

  public TechConfiguration(IAppManager manager)
  {
    this._manager = manager;
    this.InitializeData();
  }

  public bool ImbaseCeh2ObjectPumpMode { get; private set; }

  public bool ZagotLink2ImbasePumpMode { get; private set; }

  public bool CehZahodIgnoreAreaPumpMode { get; private set; }

  public bool CehZahodProductionPumpMode { get; private set; }

  public bool SpecToolDirectLinkPumpMode { get; private set; }

  public bool UniqueObjectsLookupIpsPumpMode { get; private set; }

  public bool SkipMaterialComposition { get; private set; }

  public bool CheckDopTablesDuplicates { get; private set; } = true;
}

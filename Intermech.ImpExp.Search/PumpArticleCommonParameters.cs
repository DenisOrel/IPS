// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpArticleCommonParameters
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskDescription("Инициализация данных для перекачки общих параметров изделий", "Перекачка данных об общих параметров изделий")]
internal sealed class PumpArticleCommonParameters(SearchPlugin plugin) : PumpCommonParameters(plugin, "COMMON_ART_ATTRS", MetaDataHelper.GetObjectTypeID("cad00268-306c-11d8-b4e9-00304f19f545"))
{
  protected override Guid GUID => new Guid("{86848EDC-6983-4AE9-9B10-AB7227010729}");

  protected override ImportingCategory CacheCategory => ImportingCategory.ArticleCommonParameters;

  protected override SettingsGroupType SettingsGroupType
  {
    get => SettingsGroupType.CommonArticleAttributes;
  }

  protected override string ConfigTableName => "ART_PARAMS_CFG";

  protected override string DataTableName => "ART_PARAMS";

  protected override string IDColumnName => "ART_ID";

  protected override string SettingsCaption => "Общие параметры изделий";
}

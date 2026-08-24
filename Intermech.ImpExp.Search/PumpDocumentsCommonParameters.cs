// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpDocumentsCommonParameters
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskDescription("Инициализация данных для перекачки общих параметров документов", "Перекачка данных об общих параметров документов")]
internal sealed class PumpDocumentsCommonParameters(SearchPlugin plugin) : PumpCommonParameters(plugin, "COMMON_DOC_ATTRS", MetaDataHelper.GetObjectTypeID("cad00070-306c-11d8-b4e9-00304f19f545"))
{
  protected override Guid GUID => new Guid("{EA56BB7A-D1F1-49DB-A5C6-3A173EBC8386}");

  protected override ImportingCategory CacheCategory => ImportingCategory.DocumentsCommonParameters;

  protected override SettingsGroupType SettingsGroupType
  {
    get => SettingsGroupType.CommonDocumentAttributes;
  }

  protected override string ConfigTableName => "DOC_PARAMS_CFG";

  protected override string DataTableName => "DOC_PARAMS";

  protected override string IDColumnName => "DOC_ID";

  protected override string SettingsCaption => "Общие параметры документов";
}

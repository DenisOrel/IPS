// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.SettingsUtils
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

#nullable disable
namespace Intermech.MG.Integrator;

internal static class SettingsUtils
{
  public const string ImportFilesCategory = "Настройки импорта файлов проекта";
  public const string StampCategory = "Настройки штампа схемы";
  public const string AttributesIDCategory = "Настройки идентифицирующих атрибутов";
  public const string TableAttributesCategory = "Настройки синхронизируемых атрибутов";
  public const string ElementListCategory = "Настройки типов перечней элементов";
  public const string CompositionFilterCategory = "Настройки фильтрации составов";
  public const string FGIDCategory = "Настройки функциональных групп";
  public const string MiscCategory = "Разное";
  public const string ImbaseSyncCategory = "Синхронизация с Imbase";

  public static string TrimStringValue(string value) => value == null ? string.Empty : value.Trim();
}

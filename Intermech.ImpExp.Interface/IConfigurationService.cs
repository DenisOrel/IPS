// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IConfigurationService
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Интерфейс управления конфигурацией</summary>
public interface IConfigurationService
{
  /// <summary>Загрузить конфигурацию из файла</summary>
  /// <param name="filename">путь к файлу</param>
  /// <returns>конфигурация</returns>
  IConfiguration Load(string filename);

  /// <summary>Сохранить конфигурация</summary>
  /// <param name="filename">путь к файлу</param>
  void Save(string filename);

  /// <summary>Основные настройки программы мигррации</summary>
  MainConfiguration Configuration { get; set; }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IConfigurable
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Интерфейс загрузки/сохранения параметров</summary>
public interface IConfigurable
{
  /// <summary>Загрузка параметров из главной конфигурации</summary>
  void LoadConfiguration();

  /// <summary>Загрузка параметров из конфигурации</summary>
  /// <param name="cfg">конфигурация</param>
  void LoadConfiguration(IConfiguration cfg);

  /// <summary>Сохранение параметров в главную конфигурацию</summary>
  void SaveConfiguration();

  /// <summary>Сохранение параметров конфигурацию</summary>
  /// <param name="cfg">конфигурация</param>
  void SaveConfiguration(IConfiguration cfg);
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ICache
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Интерфейс на кэш импортированных данных</summary>
public interface ICache
{
  /// <summary>Получить кэш</summary>
  /// <param name="categories">Категории объектов, которые должны присутствовать в кэше</param>
  IImportingData GetCache(params ImportingCategory[] categories);

  /// <summary>Получить кэш</summary>
  /// <param name="categories">Идентификаторы категории объектов, которые должны присутствовать в кэше</param>
  IImportingData GetCache(params int[] categories);

  /// <summary>Освободить из памяти загруженные категории</summary>
  /// <param name="categories"></param>
  void ReleaseCache(params ImportingCategory[] categories);

  /// <summary>Удалить физически категории</summary>
  /// <param name="categories"></param>
  void DeleteCache(params ImportingCategory[] categories);

  /// <summary>Освободить из памяти загруженные категории</summary>
  /// <param name="categories"></param>
  void ReleaseCache(params int[] categories);

  /// <summary>Удалить физически категории</summary>
  /// <param name="categories"></param>
  void DeleteCache(params int[] categories);

  /// <summary>Возвращает флаг, открыт ли кэш по данной категории</summary>
  /// <param name="category"></param>
  /// <returns></returns>
  bool Exist(ImportingCategory category);
}

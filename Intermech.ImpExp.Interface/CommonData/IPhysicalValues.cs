// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.IPhysicalValues
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData;

/// <summary>
/// Интерфейс для работы с физическими величинами при закачке (для организации кэша физических величин)
/// </summary>
public interface IPhysicalValues
{
  /// <summary>Получение массива всех физических величин</summary>
  /// <returns>Массив физических величин</returns>
  IPhysicalValueItem[] GetAllPhysicalValues();

  /// <summary>
  /// Проверка наличия в кэше физической величины по ее наименованию
  /// </summary>
  /// <param name="physiclValueName">Наименование физической величины</param>
  /// <returns></returns>
  bool PhysicalValueExists(string physiclValueName);

  /// <summary>
  /// Получение объекта физической величины по его идентификатору
  ///  </summary>
  /// <param name="objectId">Идентификатор физической величины в текущем сеансе перекачки</param>
  /// <returns>Интерфейс объекта физической величины</returns>
  IPhysicalValueItem GetPhysicalValue(long objectId);

  /// <summary>
  /// Получение объекта физической величины по ее наименованию
  ///  </summary>
  /// <param name="physValName">Наименование физической величины</param>
  /// <returns>Интерфейс объекта физической величины</returns>
  IPhysicalValueItem GetPhysicalValue(string physValName);

  /// <summary>
  /// Получение объекта физической величины по его идентификатору в новой базе
  ///  </summary>
  /// <param name="baseObjectId">Идентификатор физической величины в новой базе</param>
  /// <returns>Интерфейс объекта физической величины</returns>
  IPhysicalValueItem GetPhysicalValueByBaseId(long baseObjectId);

  /// <summary>Добавление новой физической величины</summary>
  /// <param name="name">Наименование физической величины</param>
  /// <param name="guid">Глобальный идентификатор физической величины в новой базе
  /// (если такой физической величины еще нет, то надо передавать Guid.Empty)</param>
  /// <param name="objID">Идентификатор физической величины в текущем сеансе перекачки</param>
  void AddPhysicalValue(long objID, string name, Guid guid);

  /// <summary>
  /// Загрузка уже имеющихся в базе объектов физических величин
  /// </summary>
  void Reload();
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ITagImportObject
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Интерфейс, который должны быть реализованы у
/// объектов передаваемых в качестве параметра tag функциям IImportingData
/// </summary>
public interface ITagImportObject
{
  /// <summary>Вызывается у объекта при сохранении данных в кэш</summary>
  /// <returns></returns>
  byte[] Save();

  /// <summary>Вызывается у объекта при чтении данных из кэш</summary>
  /// <param name="s">Что положили в Save, то и получили :)</param>
  void Load(byte[] s);

  /// <summary>
  /// Уникальный идентификатор класса
  /// (не забываем, реализуем также в TagImportObjectHelper.GetImportObject)
  /// </summary>
  short ClassID { get; }
}

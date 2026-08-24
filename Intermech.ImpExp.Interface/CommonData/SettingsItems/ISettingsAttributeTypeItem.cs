// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.SettingsItems.ISettingsAttributeTypeItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.SettingsItems;

public interface ISettingsAttributeTypeItem : ISettingsItem
{
  /// <summary>Короткое имя</summary>
  string ShortName { get; }

  /// <summary>Строковый псевдоним</summary>
  string Alias { get; }

  /// <summary>
  /// Тип данных у данного поля в базе (тип данных значения)
  /// </summary>
  FieldTypes FieldType { get; }

  /// <summary>Максимальная длина значения</summary>
  int ValueMaxLength { get; }

  /// <summary>
  /// Признак, вычисляемое поле, или оно физически хранится в базе
  /// </summary>
  bool ExistsInBase { get; set; }

  /// <summary>Дополнительные данные</summary>
  object Tag { get; set; }
}

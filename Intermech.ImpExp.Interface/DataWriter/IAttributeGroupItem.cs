// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.IAttributeGroupItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

public interface IAttributeGroupItem : ITypeItem
{
  /// <summary>
  /// Массив идентификаторов типов атрибутов, назначенных данной группе атрибутов
  /// </summary>
  int[] AttrTypeIDs { get; }

  /// <summary>
  /// Проверка наличия типа атрибута с заданным идентификатором у данной группы атрибутов
  /// </summary>
  /// <param name="attrTypeID">Идентификатор типа атрибута</param>
  /// <returns>Если тип атрибута с заданным идентификатором уже есть, то возвращается - true, иначе false</returns>
  bool AttrTypeExists(int attrTypeID);

  /// <summary>Комментарии</summary>
  string Note { get; set; }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IDataBaseManager
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Интерфейс менеджера подключений к базам</summary>
public interface IDataBaseManager
{
  /// <summary>Зарегистрировать тип базы</summary>
  /// <param name="dbType"></param>
  /// <returns></returns>
  bool RegisterDbType(IDataBaseType dbType);

  /// <summary>Получить тип базы данных</summary>
  /// <param name="DataBaseType">наименование типа базы данных</param>
  /// <returns>возвращается ссылка на интерфейс типа базы данных</returns>
  IDataBaseType GetDbType(string dbTypeName);

  /// <summary>Найти подключение с заданным псевдонимом</summary>
  /// <param name="dbAlias"></param>
  /// <returns>возвращается ссылка на интерфейс базы данных</returns>
  IDataBase FindDbByAlias(string dbAlias);

  /// <summary>Создание нового подключения к базе данных</summary>
  /// <param name="dbType">ссылка на интерфейс типа базы данных</param>
  /// <param name="dbAlias">строка с псевдонимом создаваемого подключения</param>
  /// <returns>возвращается ссылка на интерфейс базы данных</returns>
  IDataBase CreateDBConnection(IDataBaseType dbType, string dbAlias);
}

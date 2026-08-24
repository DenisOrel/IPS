// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.IUsers
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData;

public interface IUsers
{
  /// <summary>Добалвение нового пользователя в кэш</summary>
  /// <param name="searchUserID">ID в SEARCH</param>
  /// <param name="newUserID">ID в новой системе (F_OBJECT_ID)</param>
  void AddUserIntoCache(int searchUserID, long newUserID);

  /// <summary>Получить идентификатор пользователя в новой системе</summary>
  /// <param name="searchUserID">ID в SEARCH</param>
  /// <returns></returns>
  long GetNewUserID(int searchUserID);
}

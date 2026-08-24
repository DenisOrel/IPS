// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.ECO.ECO_ID_Cache
// Assembly: Intermech.Interfaces.ECO, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B25D666E-9146-4B6E-9222-8722321C22A6
// Assembly location: D:\IPS\Client\Intermech.Interfaces.ECO.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.ECO.xml

using System;

#nullable disable
namespace Intermech.Interfaces.ECO;

public class ECO_ID_Cache
{
  /// <summary>Идентификатор типа связи "Изменяется по извещению"</summary>
  public static int Relation_ECO = -1;
  /// <summary>Идентификаторы были закэшированы</summary>
  public static bool Cached = false;

  /// <summary>Кэшировать идентификаторы</summary>
  /// <param name="session">Сессия</param>
  public static void CacheSystemId(IUserSession session)
  {
    ECO_ID_Cache.Relation_ECO = session.GetRelationType(new Guid("cad0036b-306c-11d8-b4e9-00304f19f545")).RelationType;
    ECO_ID_Cache.Cached = true;
  }
}

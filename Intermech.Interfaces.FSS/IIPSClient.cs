// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.FSS.IIPSClient
// Assembly: Intermech.Interfaces.FSS, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 89C13FA3-8295-4BAF-985C-14C35172BA6B
// Assembly location: D:\IPS\Client\Intermech.Interfaces.FSS.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.FSS.xml

using System;

#nullable disable
namespace Intermech.Interfaces.FSS;

/// <summary>
/// Интерфейс описания экземпляра клиентской программы IPS
/// </summary>
public interface IIPSClient
{
  /// <summary>
  /// Уникальный идентификатор текущего пользователя Windows NT,
  /// от имени которого запущен экземпляр IPS
  /// </summary>
  string SID { get; }

  /// <summary>Уникальный глобальный идентификатор экземпляра IPS</summary>
  Guid Guid { get; }

  /// <summary>
  /// Уникальный глобальный идентификатор текущего пользователя IPS
  /// </summary>
  Guid UserGuid { get; }

  /// <summary>Пользовательские данные</summary>
  object Tag { get; set; }
}

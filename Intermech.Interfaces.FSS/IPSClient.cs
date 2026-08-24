// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.FSS.IPSClient
// Assembly: Intermech.Interfaces.FSS, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 89C13FA3-8295-4BAF-985C-14C35172BA6B
// Assembly location: D:\IPS\Client\Intermech.Interfaces.FSS.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.FSS.xml

using System;
using System.Diagnostics;
using System.Security.Principal;

#nullable disable
namespace Intermech.Interfaces.FSS;

/// <summary>Описание экземпляра клиентской программы IPS</summary>
[Serializable]
public class IPSClient : IIPSClient
{
  /// <summary>
  /// Уникальный идентификатор текущего пользователя Windows NT,
  /// от имени которого запущен экземпляр IPS
  /// </summary>
  protected string sid = string.Empty;
  /// <summary>Уникальный глобальный идентификатор экземпляра IPS</summary>
  protected Guid guid = Guid.Empty;
  /// <summary>
  /// Уникальный глобальный идентификатор текущего пользователя IPS
  /// </summary>
  protected Guid userGuid = Guid.Empty;
  /// <summary>Пользовательские данные</summary>
  protected object tag;

  /// <summary>Создать пустое описание экземпляра класса</summary>
  public IPSClient()
  {
  }

  /// <summary>Создать частично заполненное описание экземпляра IPS</summary>
  /// <param name="sid">Уникальный идентификатор текущего пользователя Windows NT,
  /// от имени которого запущен экземпляр IPS</param>
  /// <param name="guid">Уникальный глобальный идентификатор экземпляра IPS</param>
  public IPSClient(string sid, Guid guid)
    : this(sid, guid, Guid.Empty, (object) null)
  {
  }

  /// <summary>Создать частично заполненное описание экземпляра IPS</summary>
  /// <param name="guid">Уникальный глобальный идентификатор экземпляра IPS</param>
  public IPSClient(Guid guid)
    : this(string.Empty, guid, Guid.Empty, (object) null)
  {
  }

  /// <summary>Создать заполненное описание экземпляра IPS</summary>
  /// <param name="guid">Уникальный глобальный идентификатор экземпляра IPS</param>
  /// <param name="userGuid">Уникальный глобальный идентификатор текущего пользователя IPS</param>
  public IPSClient(Guid guid, Guid userGuid)
    : this(string.Empty, guid, userGuid, (object) null)
  {
  }

  /// <summary>Создать заполненное описание экземпляра IPS</summary>
  /// <param name="sid">Уникальный идентификатор текущего пользователя Windows NT,
  /// от имени которого запущен экземпляр IPS</param>
  /// <param name="guid">Уникальный глобальный идентификатор экземпляра IPS</param>
  /// <param name="userGuid">Уникальный глобальный идентификатор текущего пользователя IPS</param>
  public IPSClient(string sid, Guid guid, Guid userGuid)
    : this(sid, guid, userGuid, (object) null)
  {
  }

  /// <summary>Создать заполненное описание экземпляра IPS</summary>
  /// <param name="sid">Уникальный идентификатор текущего пользователя Windows NT,
  /// от имени которого запущен экземпляр IPS</param>
  /// <param name="guid">Уникальный глобальный идентификатор экземпляра IPS</param>
  /// <param name="userGuid">Уникальный глобальный идентификатор текущего пользователя IPS</param>
  /// <param name="tag">Пользовательские данные</param>
  public IPSClient(string sid, Guid guid, Guid userGuid, object tag)
  {
    this.sid = sid;
    this.guid = guid;
    this.userGuid = userGuid;
    this.tag = tag;
    if (!(this.sid == string.Empty))
      return;
    this.sid = WindowsIdentity.GetCurrent().User.Value;
  }

  /// <summary>
  /// Уникальный идентификатор текущего пользователя Windows NT,
  /// от имени которого запущен экземпляр IPS
  /// </summary>
  public virtual string SID
  {
    [DebuggerStepThrough] get => this.sid;
    set => this.sid = value;
  }

  /// <summary>Уникальный глобальный идентификатор экземпляра IPS</summary>
  public virtual Guid Guid
  {
    [DebuggerStepThrough] get => this.guid;
    set => this.guid = value;
  }

  /// <summary>
  /// Уникальный глобальный идентификатор текущего пользователя IPS
  /// </summary>
  public virtual Guid UserGuid
  {
    [DebuggerStepThrough] get => this.userGuid;
    set => this.userGuid = value;
  }

  /// <summary>Пользовательские данные</summary>
  public virtual object Tag
  {
    [DebuggerStepThrough] get => this.tag;
    set => this.tag = value;
  }

  /// <summary>Сравнить с указанным объектом</summary>
  /// <param name="obj">Объект для сравнения</param>
  /// <returns>true - объекты равны</returns>
  public override bool Equals(object obj)
  {
    return obj is IPSClient ipsClient && this.Guid.Equals(ipsClient.Guid) && this.UserGuid.Equals(ipsClient.UserGuid);
  }

  /// <summary>Вернуть 32-битный хэш-код экземпляра класса</summary>
  /// <returns>32-битный хэш-код экземпляра класса</returns>
  public override int GetHashCode()
  {
    Guid guid = this.Guid;
    int num = guid.GetHashCode() << 16 /*0x10*/;
    guid = this.UserGuid;
    int hashCode = guid.GetHashCode();
    return num ^ hashCode;
  }
}

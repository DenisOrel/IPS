// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.FSS.IIPS_FSS_Server
// Assembly: Intermech.Interfaces.FSS, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 89C13FA3-8295-4BAF-985C-14C35172BA6B
// Assembly location: D:\IPS\Client\Intermech.Interfaces.FSS.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.FSS.xml

#nullable disable
namespace Intermech.Interfaces.FSS;

/// <summary>
/// Интерфейс серверной службы IPS.FSS для доступа со стороны клиентов IPS
/// </summary>
public interface IIPS_FSS_Server
{
  /// <summary>Получить информацию о сервере</summary>
  IPSServerInfo ServerInfo { get; }

  /// <summary>Зарегистрировать указанный экземпляр IPS-клиента</summary>
  /// <param name="client">Описание экземпляра IPS-клиента</param>
  /// <returns>true - регистрация клиента выполнена успешно</returns>
  bool Login(IIPSClient client);

  /// <summary>Отключить указанный экземпляр IPS-клиента</summary>
  /// <param name="client">Описание экземпляра IPS-клиента</param>
  /// <returns>true - отключение клиента выполнено успешно</returns>
  bool Logout(IIPSClient client);

  /// <summary>
  /// Создать (при необходимости) указанное файловое хранилище.
  /// Если требуется - установить начальные значения прав доступа
  /// (постановка файлового хранилища на защиту)
  /// </summary>
  /// <param name="client">Клиент IPS</param>
  /// <param name="folderName">Полный путь к папке файлового хранилища</param>
  /// <param name="withLock">true - поставить хранилище на защиту</param>
  /// <returns>true - всё выполнено успешно, false - возникли ошибки</returns>
  bool CreateFileStorage(IIPSClient client, string folderName, bool withLock);

  /// <summary>
  /// Получить полный путь к текущему файловому хранилищу для указанного клиента IPS
  /// </summary>
  /// <param name="client">Клиент IPS</param>
  /// <returns>Полный путь к текущему файловому хранилищу для указанного клиента IPS</returns>
  string CurrentFileStorage(IIPSClient client);

  /// <summary>Подключиться к файловому хранилищу</summary>
  /// <param name="client">Клиент IPS</param>
  /// <param name="folderName">Полный путь к папке файлового хранилища</param>
  /// <returns>true - всё выполнено успешно, false - возникли ошибки</returns>
  bool ConnectFileStorage(IIPSClient client, string folderName);

  /// <summary>Отключиться от текущего файлового хранилища</summary>
  /// <param name="client">Клиент IPS</param>
  /// <returns>true - всё выполнено успешно, false - возникли ошибки</returns>
  bool DisconnectFileStorage(IIPSClient client);

  /// <summary>
  /// Создать (при необходимости) подпапку в текущем файловом хранилище пользователя.
  /// </summary>
  /// <param name="client">Клиент IPS</param>
  /// <param name="subFolder">Имя создаваемой подпапки</param>
  /// <returns>true - всё выполнено успешно, false - возникли ошибки</returns>
  bool CreateSubfolder(IIPSClient client, string subFolder);

  /// <summary>Подключиться к подпапке файлового хранилища</summary>
  /// <param name="client">Клиент IPS</param>
  /// <param name="subFolder">Имя подпапки файлового хранилища</param>
  /// <returns>true - всё выполнено успешно, false - возникли ошибки</returns>
  bool ConnectSubfolder(IIPSClient client, string subFolder);

  /// <summary>Отключить указанную подпапку файлового хранилища</summary>
  /// <param name="client">Клиент IPS</param>
  /// <param name="subFolder">Имя подпапки файлового хранилища</param>
  /// <returns>true - всё выполнено успешно, false - возникли ошибки</returns>
  bool DisconnectSubfolder(IIPSClient client, string subFolder);
}

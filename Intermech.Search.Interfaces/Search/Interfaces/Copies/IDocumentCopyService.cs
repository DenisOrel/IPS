// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Interfaces.Copies.IDocumentCopyService
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Search.Interfaces.Copies;

/// <summary>сервис для работы с копиями документов</summary>
public interface IDocumentCopyService
{
  /// <summary>
  /// Быстрая отправка копий при уже заполненных атрибутах Получатель и Абонент копии
  /// </summary>
  /// <param name="sessionID">Сессия</param>
  /// <param name="copiesIds">Список копий для отправки.</param>
  /// <returns>Накопительный эксепшн о том, что где надо изменить, чтобы произошла автоматическая отправка.
  /// Заполненные без ошибок копии отправятся в любом случае.</returns>
  Exception CopiesFastSending(object sessionID, List<long> copiesIds);

  /// <summary>Создать указанное количество копий документа</summary>
  /// <param name="docID">Документ, копии которого создаём</param>
  /// <param name="count">Количество создаваемых копий</param>
  /// <param name="copyKind">Вид копии</param>
  /// <param name="sessionID">Сессия или id/guid сессии</param>
  /// <returns>id версий созданных копий</returns>
  List<long> CreateCopies(long docID, int count, CopyKind copyKind, object sessionID);

  /// <summary>Выслать копии пользователю</summary>
  /// <param name="subscriberID">Абонент</param>
  /// <param name="recipientID">Получатель копии</param>
  /// <param name="listID">ID версии листа рассылки</param>
  /// <param name="copiesID">список высылаемых копий</param>
  /// <param name="date">Дата полчения копии</param>
  /// <param name="albumID">ID версии альбома, в который включить копии</param>
  /// <param name="sessionID">Сессия или id/guid сессии </param>
  void SendCopies(
    long subscriberID,
    long recipientID,
    long listID,
    List<long> copiesID,
    DateTime date,
    long albumID,
    object sessionID);

  /// <summary>Возврат копии</summary>
  /// <param name="copiesID">список версий возвращаемых копий</param>
  /// <param name="recipientID">кто вернул копию</param>
  /// <param name="date">дата возврата</param>
  /// <param name="sessionID">Сессия или id/guid сессии </param>
  void ReturnCopies(List<long> copiesID, long recipientID, DateTime date, object sessionID);

  /// <summary>Удалить выбранные копии</summary>
  /// <param name="copiesID">список версий удаляемых копий</param>
  /// <param name="sessionID">Сессия или id/guid сессии</param>
  void RemoveCopiesReferences(List<long> copiesID, object sessionID);

  /// <summary>
  /// Вернуть список пользователей, которым можно высылать копии документов
  /// (если у пользователя уже есть копия другой версии документа - пользователь в список не попадёт)
  /// </summary>
  /// <param name="listID">id списка рассылки для документа</param>
  /// <param name="docObjectID">id версии высылаемого документа</param>
  /// <param name="sessionID">сессия</param>
  /// <returns></returns>
  List<long> FormEnabledSubscribers(long listID, long docObjectID, object sessionID);
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.IImportedRelationList
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

public interface IImportedRelationList : IImportedAttributeList
{
  IImportedRelationListItems Items { get; }

  int PacketSize { get; set; }

  event AfterImportEventDelegate AfterImportEvent;

  /// <summary>Добавление связи</summary>
  RelationRecord AddRelation(long projId, long partId, int relType);

  RelationRecord AddRelation(RelationRecord rel);

  /// <summary>Добавление связи</summary>
  RelationRecord AddRelation(long projId, long partId, int relType, DateTime crtDate);

  /// <summary>
  /// Добавление связи между объектами (Начало действия связи - текущее время,
  /// дата окончания действия связи - максимальное значение DateTime)
  /// </summary>
  /// <param name="projId">Идентификатор версии объекта в сосстав которого производится добавление</param>
  /// <param name="partId">Идентификатор версии объекта, который надо добавить к составу</param>
  /// <param name="relType">Идентификатор вида связи</param>
  /// <returns>Идентификатор созданной связи</returns>
  RelationRecord AddRelationFromID(long projId, long partId, int relType);

  /// <summary>
  /// Добавление связи между объектами (дата окончания действия связи равна максимальному значению DateTime)
  /// </summary>
  /// <param name="projId">Идентификатор версии родительского объекта</param>
  /// <param name="partId">Идентификатор дочернего объекта (не версии!!!)</param>
  /// <param name="relType">Идентификатор вида связи</param>
  /// <param name="crtDate">Дата создания связи</param>
  /// <returns>Идентификатор созданной связи</returns>
  RelationRecord AddRelationFromID(long projId, long partId, int relType, DateTime crtDate);

  /// <summary>
  /// Установить курсор в списке связей для импорта на связь с идентификатором prjLinkID, если эта связь уже была импортирована ранее.
  /// </summary>
  /// <param name="objectID">Идентификатор связи</param>
  void UseRelation(long prjLinkID);

  /// <summary>
  /// Установить курсор в списке связей для импорта на связь rel. Если в списке такой связи не обнаружено,
  /// связь добавляеццо в конец списка, и курсор ставиццо на нее.
  /// </summary>
  /// <param name="obj"></param>
  void UseRelation(RelationRecord rel);

  /// <summary>Импорт пакета данных</summary>
  void Import();

  ImportingRelationCreator ImportingRelationCreator { get; set; }
}

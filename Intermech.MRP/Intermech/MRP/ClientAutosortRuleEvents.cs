// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.ClientAutosortRuleEvents
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MRP;
using Intermech.MRP.Orders;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP;

/// <summary>
/// Вспомогательный статический класс для обработки событий на стороне клиентского модуля расширения MRP
/// </summary>
internal class ClientAutosortRuleEvents
{
  /// <summary>Идентификатор типа связи "Состав изделий"</summary>
  private static int _relTypeArticles = -1;
  /// <summary>Guid типа связи "Состав изделий"</summary>
  private static Guid _relTypeArticlesGuid = new Guid("cad00023-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа связи "Документация на изделие"</summary>
  private static int _relTypeDocuments = -1;
  /// <summary>Guid типа связи "Документация на изделие"</summary>
  private static Guid _relTypeDocumentsGuid = new Guid("cad00154-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа связи "Состав документации"</summary>
  private static int _relTypeDocCompositions = -1;
  /// <summary>Guid типа связи "Состав документации"</summary>
  private static Guid _relTypeDocCompositionsGuid = new Guid("cad0057c-306c-11d8-b4e9-00304f19f545");

  /// <summary>Проверить, заполнены ли поля класса</summary>
  private static void CheckConsts()
  {
    if (ClientAutosortRuleEvents._relTypeArticles != -1)
      return;
    ClientAutosortRuleEvents._relTypeArticles = MetaDataHelper.GetRelationTypeID("cad00023-306c-11d8-b4e9-00304f19f545");
    ClientAutosortRuleEvents._relTypeDocuments = MetaDataHelper.GetRelationTypeID("cad00154-306c-11d8-b4e9-00304f19f545");
    ClientAutosortRuleEvents._relTypeDocCompositions = MetaDataHelper.GetRelationTypeID("cad0057c-306c-11d8-b4e9-00304f19f545");
  }

  /// <summary>
  /// Обработчик события, вызываемого правилом отображения и сортировки составов при получении списка видимых типов связей
  /// </summary>
  /// <param name="sender">Отправитель (ссылка на интерфейс ICompositionsAutosortRule)</param>
  /// <param name="e">Аргументы события</param>
  public static void CompositionsGetVisibleRelationsEventHandler(
    object sender,
    CompositionsAutosortRuleEventArgs e)
  {
    IMSObjectType objectType = MetaDataHelper.GetObjectType(e.ObjectType);
    if (objectType == null)
      return;
    ClientAutosortRuleEvents.CheckConsts();
    ManufactOrdersEditor service1 = ServicesManager.GetService(typeof (ManufactOrdersEditor)) as ManufactOrdersEditor;
    List<int> applicabilityRelationTypesId = MetaDataHelper.GetApplicabilityRelationTypesID(objectType.ObjectTypeID);
    if (applicabilityRelationTypesId.IndexOf(ClientAutosortRuleEvents._relTypeArticles) >= 0 && e.VisibleRelTypes.IndexOf(ClientAutosortRuleEvents._relTypeArticles) < 0)
      e.VisibleRelTypes.Insert(0, ClientAutosortRuleEvents._relTypeArticles);
    if (ServicesManager.GetService(typeof (IMRPSettings)) is IMRPSettings service2 && service2.UseDocumentation)
    {
      if (applicabilityRelationTypesId.IndexOf(ClientAutosortRuleEvents._relTypeDocuments) >= 0 && e.VisibleRelTypes.IndexOf(ClientAutosortRuleEvents._relTypeDocuments) < 0)
        e.VisibleRelTypes.Add(ClientAutosortRuleEvents._relTypeDocuments);
      if (applicabilityRelationTypesId.IndexOf(ClientAutosortRuleEvents._relTypeDocCompositions) >= 0 && e.VisibleRelTypes.IndexOf(ClientAutosortRuleEvents._relTypeDocCompositions) < 0)
        e.VisibleRelTypes.Add(ClientAutosortRuleEvents._relTypeDocCompositions);
    }
    if (service1 == null)
      return;
    int relationTypeId1 = MetaDataHelper.GetRelationTypeID("cad00584-306c-11d8-b4e9-00304f19f545");
    e.VisibleRelTypes.Remove(relationTypeId1);
    if (service2 == null || service2.UseDocumentation)
      return;
    int relationTypeId2 = MetaDataHelper.GetRelationTypeID("cad00154-306c-11d8-b4e9-00304f19f545");
    e.VisibleRelTypes.Remove(relationTypeId2);
  }

  /// <summary>
  /// Обработчик события, вызываемого правилом отображения и сортировки составов при получении списка видимых типов связей
  /// </summary>
  /// <param name="sender">Отправитель (ссылка на интерфейс ICompositionsAutosortRule)</param>
  /// <param name="e">Аргументы события</param>
  public static void CompositionsGetVisibleRelationsGuidEventHandler(
    object sender,
    CompositionsAutosortRuleGuidEventArgs e)
  {
    IMSObjectType objectType = MetaDataHelper.GetObjectType(e.ObjectType);
    if (objectType == null)
      return;
    ClientAutosortRuleEvents.CheckConsts();
    ManufactOrdersEditor service1 = ServicesManager.GetService(typeof (ManufactOrdersEditor)) as ManufactOrdersEditor;
    List<Guid> relationTypesGuids = MetaDataHelper.GetApplicabilityRelationTypesGuids(objectType.Guid);
    if (relationTypesGuids.IndexOf(ClientAutosortRuleEvents._relTypeArticlesGuid) >= 0 && e.VisibleRelTypes.IndexOf(ClientAutosortRuleEvents._relTypeArticlesGuid) < 0)
      e.VisibleRelTypes.Insert(0, ClientAutosortRuleEvents._relTypeArticlesGuid);
    if (ServicesManager.GetService(typeof (IMRPSettings)) is IMRPSettings service2 && service2.UseDocumentation)
    {
      if (relationTypesGuids.IndexOf(ClientAutosortRuleEvents._relTypeDocumentsGuid) >= 0 && e.VisibleRelTypes.IndexOf(ClientAutosortRuleEvents._relTypeDocumentsGuid) < 0)
        e.VisibleRelTypes.Add(ClientAutosortRuleEvents._relTypeDocumentsGuid);
      if (relationTypesGuids.IndexOf(ClientAutosortRuleEvents._relTypeDocCompositionsGuid) >= 0 && e.VisibleRelTypes.IndexOf(ClientAutosortRuleEvents._relTypeDocCompositionsGuid) < 0)
        e.VisibleRelTypes.Add(ClientAutosortRuleEvents._relTypeDocCompositionsGuid);
    }
    if (service1 == null)
      return;
    Guid guid1 = new Guid("cad00584-306c-11d8-b4e9-00304f19f545");
    e.VisibleRelTypes.Remove(guid1);
    if (service2 == null || service2.UseDocumentation)
      return;
    Guid guid2 = new Guid("cad00154-306c-11d8-b4e9-00304f19f545");
    e.VisibleRelTypes.Remove(guid2);
  }
}

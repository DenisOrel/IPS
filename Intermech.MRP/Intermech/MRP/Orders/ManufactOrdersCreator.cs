// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.ManufactOrdersCreator
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using ImSSP;
using Intermech.Client.Core.ObjectCreator;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Мастер по созданию объектов типа "Производственные заказы"
/// </summary>
internal sealed class ManufactOrdersCreator : 
  IObjectCreatorRiderCustomService,
  IObjectCreatorCustomService,
  IObjectCreatorFormProvider
{
  /// <summary>ID типа объекта, для которого создаётся мастер</summary>
  private int _createdObjectTypeID = -1;
  /// <summary>Набор страниц мастера создания объектов</summary>
  private IDictionary<ObjectCreatePages, bool> _createPages;

  /// <summary>
  /// Вызов диалога создания нового объекта (по прототипу) c созданием заданных связей с указанными объектами
  /// </summary>
  /// <param name="ObjectTypeID">Идентификатор типа создаваемого объекта</param>
  /// <param name="TemplateObjectID">Идентификатор объекта-прототипа</param>
  /// <param name="RelationTypeIDs">массив идентификаторов связей которые необходимо создавать</param>
  /// <param name="RelatedObjectIDs">массив идентификаторов объектов с которыми надо связать созданный объект</param>
  /// <param name="StartDate">время с которого начинают действовать связи (если они были созданы)</param>
  /// <param name="isVersion">признак, нужно ли создавать версию объекта</param>
  /// <returns>Идентификатор созданного объекта</returns>
  public long CreateObjectDialog(
    int ObjectTypeID,
    long TemplateObjectID,
    int[] RelationTypeIDs,
    long[] RelatedObjectIDs,
    DateTime StartDate,
    bool isVersion)
  {
    return -1;
  }

  public bool OnBeforeCommitAction(IUserSession session, IDBObject newObject) => true;

  /// <summary>Вызывать собственный диалог ?</summary>
  /// <param name="ObjectTypeID">Идентификатор типа создаваемого объекта</param>
  /// <param name="TemplateObjectID">Идентификатор объекта-прототипа</param>
  /// <param name="RelationTypeIDs">Массив идентификаторов связей которые необходимо создавать</param>
  /// <param name="RelatedObjectIDs">Массив идентификаторов объектов с которыми надо связать созданный объект</param>
  /// <param name="StartDate">Время с которого начинают действовать связи (если они были созданы)</param>
  /// <param name="isVersion">Признак, нужно ли создавать версию объекта</param>
  /// <returns></returns>
  public bool AcceptDialog(
    int ObjectTypeID,
    long TemplateObjectID,
    int[] RelationTypeIDs,
    long[] RelatedObjectIDs,
    DateTime StartDate,
    bool isVersion)
  {
    this._createdObjectTypeID = ObjectTypeID;
    return false;
  }

  /// <summary>
  /// Метод вызывается после создания новой заготовки до отображения диалога создания
  /// </summary>
  /// <param name="newObjectID">ID версии заготовки</param>
  /// <returns></returns>
  public bool AfterCreate(long newObjectID) => true;

  /// <summary>
  /// Возвращает коллекцию страниц, которые будут присутствовать в мастере по созданию объекта.
  /// Значение в коллекции обозначает, отображать ли эту страницу в мастере
  /// </summary>
  public IDictionary<ObjectCreatePages, bool> VisiblePages
  {
    get
    {
      if (this._createPages == null)
      {
        this._createPages = (IDictionary<ObjectCreatePages, bool>) new Dictionary<ObjectCreatePages, bool>();
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          if (ObjectsClassifyHelper.GetClassifierType(sessionKeeper.Session, this._createdObjectTypeID) != ObjectsClassifyType.None)
            this._createPages.Add(ObjectCreatePages.Classifier, true);
        }
        this._createPages.Add(ObjectCreatePages.Properties, true);
        this._createPages.Add(ObjectCreatePages.Template, true);
      }
      return this._createPages;
    }
  }

  /// <summary>
  /// Метод вызывается по нажатию на кнопку "Готово", внутри транзакции
  /// </summary>
  /// <param name="session">Пользовательская сессия</param>
  /// <param name="newObjectID">ID заготовки</param>
  /// <param name="nea">Список для хранения событий</param>
  /// <returns></returns>
  public bool OnCommitAction(
    IUserSession session,
    long newObjectID,
    List<NotificationEventArgs> nea)
  {
    return true;
  }

  /// <summary>
  /// Метод вызывается по нажатию на кнопку "Отмена", внутри транзакции
  /// </summary>
  /// <param name="session">Пользовательская сессия</param>
  /// <param name="newObjectID">ID заготовки</param>
  /// <param name="nea">Список для хранения событий</param>
  /// <returns></returns>
  public bool OnCancelAction(
    IUserSession session,
    long newObjectID,
    List<NotificationEventArgs> nea)
  {
    return true;
  }

  /// <summary>
  /// Добавить в мастер свои страницы, с порядковым номером следования в мастере (если -1 - добавить страничку в конец)
  /// </summary>
  /// <param name="CreatedObject"></param>
  /// <param name="propPageIndex"></param>
  /// <returns></returns>
  public Dictionary<UserControl, int> AddPages(object CreatedObject, int propPageIndex)
  {
    Dictionary<UserControl, int> dictionary = new Dictionary<UserControl, int>();
    if (CreatedObject is CreatedObjectItem createdObject)
      dictionary.Add((UserControl) new ManufactOrdersCreatorControl(createdObject)
      {
        ObjectCreatorForm = this.ObjectCreatorForm
      }, -sc_14785.ssp_mrp_14786(530880651));
    return dictionary.Count > 0 ? dictionary : (Dictionary<UserControl, int>) null;
  }

  public ObjectCreatorForm ObjectCreatorForm { get; set; }
}

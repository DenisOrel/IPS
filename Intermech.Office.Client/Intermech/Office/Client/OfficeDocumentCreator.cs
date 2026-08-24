// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeDocumentCreator
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Office.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

internal class OfficeDocumentCreator : IObjectCreatorRiderCustomService, IObjectCreatorCustomService
{
  [CanBeNull]
  private IDictionary<ObjectCreatePages, bool> _createPages;
  private long _parentDocumentID;
  private bool _isClassified;
  protected int _ObjectType = -1;

  public bool AcceptDialog(
    int objectType,
    long templateObject,
    [CanBeNull] int[] relationTypeIDs,
    long[] relatedObjectIDs,
    DateTime startDate,
    bool isVersion)
  {
    if (relationTypeIDs != null && relationTypeIDs.Length != 0)
    {
      for (int index = 0; index < relationTypeIDs.Length; ++index)
      {
        if (relationTypeIDs[index] == OfficeConsts.ReltypeAnswerID)
        {
          this._parentDocumentID = relatedObjectIDs[index];
          break;
        }
      }
    }
    if (!isVersion)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject containerForObjectType = sessionKeeper.Session.GetCustomService<IContainerService>().GetContainerForObjectType((object) sessionKeeper.Session.SessionGUID, objectType);
        if (containerForObjectType != null)
        {
          IDBAttribute byId = containerForObjectType.Attributes.FindByID(OfficeConsts.AttrClassifiedObjectsID);
          this._isClassified = byId != null && Convert.ToInt32(byId.Value) > 0;
        }
      }
    }
    this._ObjectType = objectType;
    return false;
  }

  public bool AfterCreate(long newObjectID)
  {
    return OfficeClientHelper.RegisterNewDocument(newObjectID, this._ObjectType, (OfficeDocumentTypes[]) null, this._parentDocumentID);
  }

  [NotNull]
  public IDictionary<ObjectCreatePages, bool> VisiblePages
  {
    get
    {
      if (this._createPages == null)
      {
        this._createPages = (IDictionary<ObjectCreatePages, bool>) new Dictionary<ObjectCreatePages, bool>();
        if (this._isClassified)
          this._createPages.Add(ObjectCreatePages.Classifier, true);
        this._createPages.Add(ObjectCreatePages.Properties, true);
        this._createPages.Add(ObjectCreatePages.FileAttributes, true);
        this._createPages.Add(ObjectCreatePages.Relations, true);
        this._createPages.Add(ObjectCreatePages.Template, true);
      }
      return this._createPages;
    }
  }

  public bool OnBeforeCommitAction(IUserSession session, IDBObject newObject) => true;

  public bool OnCommitAction(
    [NotNull] IUserSession session,
    long newObjectID,
    List<NotificationEventArgs> nea)
  {
    if (session.GetCustomService<IOfficeRegistrationService>().IsDocumentRegister(session.SessionGUID, newObjectID))
      OfficeDocumentCommands.CheckAndPrivateRegister(session, newObjectID, true);
    IDBObject dbObject = session.GetObject(newObjectID);
    if ((int) dbObject.AttributeByID(OfficeConsts.AttrOfficeDocumentTypeID).AsInteger == 1)
      return true;
    List<long> longList1 = new List<long>();
    IDBAttribute attributeById = dbObject.GetAttributeByID(OfficeConsts.AttrAddresseesID);
    if (attributeById != null && attributeById.ValuesCount > 0)
    {
      for (int index = 0; index < attributeById.ValuesCount; ++index)
      {
        if (attributeById.Values[index] != null)
        {
          long int64 = Convert.ToInt64(attributeById.Values[index]);
          if (!longList1.Contains(int64))
            longList1.Add(int64);
        }
      }
    }
    if (longList1.Count > 0)
    {
      List<long> longList2 = new List<long>();
      foreach (long addresseeID in longList1)
      {
        List<long> userAddresseeList = OfficeClientHelper.GetUserAddresseeList(session, addresseeID);
        if (userAddresseeList != null && userAddresseeList.Count > 0)
        {
          foreach (long num in userAddresseeList)
          {
            if (!longList2.Contains(num))
              longList2.Add(num);
          }
        }
      }
      if (longList2.Count > 0)
        OfficeClientHelper.CreateAddresseesMessage(session, longList2.ToArray(), dbObject);
    }
    return true;
  }

  public bool OnCancelAction(
    IUserSession session,
    long newObjectID,
    List<NotificationEventArgs> nea)
  {
    return true;
  }

  [CanBeNull]
  public Dictionary<UserControl, int> AddPages(object createdObject, int propPageIndex)
  {
    return (Dictionary<UserControl, int>) null;
  }

  public long CreateObjectDialog(
    int objectType,
    long templateObject,
    int[] relationTypeIDs,
    long[] relatedObjectIDs,
    DateTime startDate,
    bool isVersion)
  {
    return -1;
  }
}

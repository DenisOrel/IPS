// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionListCreator
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Client.Core.ObjectCreator;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

internal class ProductionListCreator : 
  IObjectCreatorRiderCustomService,
  IObjectCreatorCustomService,
  IObjectCreatorFormProvider
{
  private IDictionary<ObjectCreatePages, bool> _createPages;

  IDictionary<ObjectCreatePages, bool> IObjectCreatorRiderCustomService.VisiblePages
  {
    get
    {
      if (this._createPages == null)
      {
        this._createPages = (IDictionary<ObjectCreatePages, bool>) new Dictionary<ObjectCreatePages, bool>();
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          if (ObjectsClassifyHelper.GetClassifierType(sessionKeeper.Session, MRP2Consts.objtypeIdProductionLists) != ObjectsClassifyType.None)
            this._createPages.Add(ObjectCreatePages.Classifier, true);
        }
      }
      return this._createPages;
    }
  }

  ObjectCreatorForm IObjectCreatorFormProvider.ObjectCreatorForm { get; set; }

  bool IObjectCreatorRiderCustomService.AcceptDialog(
    int ObjectTypeID,
    long TemplateObjectID,
    int[] RelationTypeIDs,
    long[] RelatedObjectIDs,
    DateTime StartDate,
    bool isVersion)
  {
    return false;
  }

  Dictionary<UserControl, int> IObjectCreatorRiderCustomService.AddPages(
    object CreatedObject,
    int propPageIndex)
  {
    return (Dictionary<UserControl, int>) null;
  }

  bool IObjectCreatorRiderCustomService.AfterCreate(long newObjectID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttribute dbAttribute = sessionKeeper.Session.GetObject(newObjectID).Attributes.AddAttribute(MRP2Consts.attrIdFIOConstructor, false);
      if (dbAttribute != null)
        dbAttribute.AsInteger = sessionKeeper.Session.UserID;
    }
    return true;
  }

  long IObjectCreatorCustomService.CreateObjectDialog(
    int ObjectTypeID,
    long TemplateObjectID,
    int[] RelationTypeIDs,
    long[] RelatedObjectIDs,
    DateTime StartDate,
    bool isVersion)
  {
    return -1;
  }

  bool IObjectCreatorRiderCustomService.OnBeforeCommitAction(
    IUserSession session,
    IDBObject newObject)
  {
    return true;
  }

  bool IObjectCreatorRiderCustomService.OnCancelAction(
    IUserSession session,
    long newObjectID,
    List<NotificationEventArgs> nea)
  {
    return true;
  }

  bool IObjectCreatorRiderCustomService.OnCommitAction(
    IUserSession session,
    long newObjectID,
    List<NotificationEventArgs> nea)
  {
    return true;
  }
}

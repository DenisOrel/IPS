// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.Creator.OrderCreator
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Client.Core.ObjectCreator;
using Intermech.Client.Core.ObjectCreator.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.PdmConfigurator.Creator;

public sealed class OrderCreator : IObjectCreatorRiderCustomService, IObjectCreatorCustomService
{
  private int _createdObjectTypeID = -1;
  private IDictionary<ObjectCreatePages, bool> _createPages;
  private int _startPageIndex;

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

  public bool AfterCreate(long newObjectID) => true;

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
          {
            this._startPageIndex = 1;
            this._createPages.Add(ObjectCreatePages.Classifier, true);
          }
        }
        this._createPages.Add(ObjectCreatePages.Properties, true);
        this._createPages.Add(ObjectCreatePages.Relations, true);
        this._createPages.Add(ObjectCreatePages.Template, true);
      }
      return this._createPages;
    }
  }

  public bool OnCommitAction(
    IUserSession session,
    long newObjectID,
    List<NotificationEventArgs> nea)
  {
    return true;
  }

  public bool OnCancelAction(
    IUserSession session,
    long newObjectID,
    List<NotificationEventArgs> nea)
  {
    return true;
  }

  public Dictionary<UserControl, int> AddPages(object CreatedObject, int propPageIndex)
  {
    Dictionary<UserControl, int> dictionary = new Dictionary<UserControl, int>();
    if (CreatedObject is CreatedObjectItem createdObject)
    {
      ObjectCreatorControl key1 = (ObjectCreatorControl) new OptionsCreatorControl(createdObject);
      dictionary.Add((UserControl) key1, this._startPageIndex);
      ObjectCreatorControl key2 = (ObjectCreatorControl) new CodeCreatorControl(createdObject);
      IMSAttribute4ObjectType attribute4ObjectType1 = MetaDataHelper.GetAttribute4ObjectType(createdObject.ObjectTypeID, MetaDataHelper.GetAttributeTypeID("cad0001f-306c-11d8-b4e9-00304f19f545"));
      IMSAttribute4ObjectType attribute4ObjectType2 = MetaDataHelper.GetAttribute4ObjectType(createdObject.ObjectTypeID, MetaDataHelper.GetAttributeTypeID("cad00020-306c-11d8-b4e9-00304f19f545"));
      if (attribute4ObjectType1 != null && attribute4ObjectType1.Computed == ComputeValueModes.NotComputableValue || attribute4ObjectType2 != null && attribute4ObjectType2.Computed == ComputeValueModes.NotComputableValue)
        dictionary.Add((UserControl) key2, ++this._startPageIndex);
    }
    return dictionary.Count > 0 ? dictionary : (Dictionary<UserControl, int>) null;
  }
}

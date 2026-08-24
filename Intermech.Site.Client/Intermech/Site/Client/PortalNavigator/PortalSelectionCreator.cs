// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PortalSelectionCreator
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Client.Core.ObjectCreator;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PortalSelectionCreator : IObjectCreatorRiderCustomService, IObjectCreatorCustomService
{
  private IDictionary<ObjectCreatePages, bool> _createPages;
  private static List<int> _attachedObjectTypes;

  public bool OnBeforeCommitAction(IUserSession session, IDBObject newObject) => true;

  public bool AcceptDialog(
    int ObjectTypeID,
    long TemplateObjectID,
    int[] RelationTypeIDs,
    long[] RelatedObjectIDs,
    DateTime StartDate,
    bool isVersion)
  {
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
      PortalSelectionDialogControl key1 = new PortalSelectionDialogControl(createdObject);
      dictionary.Add((UserControl) key1, 0);
      PortalSelectionCreatorControl key2 = new PortalSelectionCreatorControl(createdObject);
      dictionary.Add((UserControl) key2, 1);
    }
    return dictionary.Count > 0 ? dictionary : (Dictionary<UserControl, int>) null;
  }

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

  public static void Attach(IObjectCreatorService service)
  {
    if (PortalSelectionCreator._attachedObjectTypes == null)
      PortalSelectionCreator._attachedObjectTypes = MetaDataHelper.GetObjectTypeChildrenIDRecursive(PortalConsts.objtypePortalSelections);
    foreach (int attachedObjectType in PortalSelectionCreator._attachedObjectTypes)
      service.RegisterCreatorCustomService(attachedObjectType, typeof (PortalSelectionCreator));
  }

  public static void Detach(IObjectCreatorService service)
  {
    if (PortalSelectionCreator._attachedObjectTypes == null || PortalSelectionCreator._attachedObjectTypes.Count <= 0)
      return;
    foreach (int attachedObjectType in PortalSelectionCreator._attachedObjectTypes)
      service.UnregisterCreatorCustomService(attachedObjectType, typeof (PortalSelectionCreator));
  }
}

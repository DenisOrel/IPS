// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.RequestCreatorCustomService
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.ExternalSystemIntegration.Interfaces;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

internal class RequestCreatorCustomService : 
  IObjectCreatorRiderParamCustomService,
  IObjectCreatorRiderCustomService,
  IObjectCreatorCustomService
{
  private IObjectCreatorParams _Pars;

  public void SetParams(IObjectCreatorParams createParams) => this._Pars = createParams;

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

  public bool AfterCreate(long newObjectID)
  {
    if (!(this._Pars is RequestCreatorParams pars))
      return false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetCustomService(typeof (IRequestObjectHelperService)) is IRequestObjectHelperService customService)
        customService.AssignAttributes(newObjectID, pars.SourceObjectID, sessionKeeper.Session.SessionGUID);
    }
    return true;
  }

  public IDictionary<ObjectCreatePages, bool> VisiblePages
  {
    get => (IDictionary<ObjectCreatePages, bool>) null;
  }

  public bool OnCommitAction(
    IUserSession session,
    long newObjectID,
    List<NotificationEventArgs> nea)
  {
    return true;
  }

  public bool OnBeforeCommitAction(IUserSession session, IDBObject newObject) => true;

  public bool OnCancelAction(
    IUserSession session,
    long newObjectID,
    List<NotificationEventArgs> nea)
  {
    return true;
  }

  public Dictionary<UserControl, int> AddPages(object CreatedObject, int propPageIndex)
  {
    return (Dictionary<UserControl, int>) null;
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
}

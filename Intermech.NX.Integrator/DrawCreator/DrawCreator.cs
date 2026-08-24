// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.DrawCreator.DrawCreator
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.Client.Core.ObjectCreator;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Tools.Integrators;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.NX.Integrator.DrawCreator;

internal sealed class DrawCreator : IObjectCreatorRiderCustomService, IObjectCreatorCustomService
{
  private IIntegrator _nxIntegrator;
  private IFileVault _fileVaultService;
  private NXModelDrawingsService _modelDrawingsService;
  private DrawCreatorResult _result;

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
    Dictionary<UserControl, int> dictionary = new Dictionary<UserControl, int>();
    if (CreatedObject is CreatedObjectItem objItem)
    {
      ChoiceDocument key = new ChoiceDocument(objItem, this._nxIntegrator, this._fileVaultService, this._modelDrawingsService, this._result);
      key.SetPageData();
      dictionary.Add((UserControl) key, propPageIndex);
    }
    return dictionary.Count > 0 ? dictionary : (Dictionary<UserControl, int>) null;
  }

  public DrawCreator(
    IIntegrator nxIntegrator,
    IFileVault fileVaultService,
    NXModelDrawingsService modelDrawingsService,
    DrawCreatorResult result)
  {
    this._nxIntegrator = nxIntegrator;
    this._fileVaultService = fileVaultService;
    this._modelDrawingsService = modelDrawingsService;
    this._result = result;
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.IDAttributesSyncronizer.SyncronizerService
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Document;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.IDAttributesSyncronizer;

internal static class SyncronizerService
{
  private static List<int> _enabledArticleTypes;
  private static List<int> _enabledDocTypes;
  public static int AttrDesignationID;
  public static int AttrNameID;
  public static long CurrentChangesArticle;
  public static long CurrentChangesDocument;

  public static void Initialize()
  {
    SyncronizerService.ReloadTypes();
    MetaDataHelperService.Instance.OnCacheReloaded += new MetaDataHelperEventHandler(SyncronizerService.MetaDataHelper_OnCacheReloaded);
    SyncronizerService.AttrDesignationID = MetaDataHelper.GetAttributeTypeID(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"));
    SyncronizerService.AttrNameID = MetaDataHelper.GetAttributeTypeID(new Guid("cad00020-306c-11d8-b4e9-00304f19f545"));
  }

  public static void ObjectChangedEvent(object sender, NotificationEventArgs e)
  {
    if (!(e is DBObjectsExtendedEventArgs eventArgs) || sender is IAVSDocument)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IObjectChangeHandler handler = SyncronizerService.GetHandler(eventArgs, sessionKeeper.Session);
      IDAttributeInfo attrDesignation = (IDAttributeInfo) null;
      IDAttributeInfo attrName = (IDAttributeInfo) null;
      if (handler == null || !handler.IDAttributesChanged(out attrDesignation, out attrName))
        return;
      handler.Handle(attrDesignation, attrName);
    }
  }

  private static void MetaDataHelper_OnCacheReloaded(object sender, EventArgs e)
  {
    SyncronizerService.ReloadTypes();
  }

  private static IObjectChangeHandler GetHandler(
    DBObjectsExtendedEventArgs eventArgs,
    IUserSession session)
  {
    if (SyncronizerService._enabledArticleTypes.Contains(eventArgs.ObjectType))
      return (IObjectChangeHandler) new ArticleChangeHandler(eventArgs, session);
    return !SyncronizerService._enabledDocTypes.Contains(eventArgs.ObjectType) ? (IObjectChangeHandler) null : (IObjectChangeHandler) new DocumentChangeHandler(eventArgs, session);
  }

  private static void ReloadTypes()
  {
    SyncronizerService._enabledArticleTypes = MetaDataHelper.GetObjectTypeChildrenIDRecursive(new Guid("cad00268-306c-11d8-b4e9-00304f19f545"));
    SyncronizerService._enabledDocTypes = MetaDataHelper.GetObjectTypeChildrenIDRecursive(new Guid("cad0057f-306c-11d8-b4e9-00304f19f545"));
  }
}

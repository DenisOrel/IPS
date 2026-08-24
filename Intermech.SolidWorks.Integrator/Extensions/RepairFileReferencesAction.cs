// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.Extensions.RepairFileReferencesAction
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.CADInterface.Proxies;
using Intermech.Collections;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.IO;
using Intermech.Runtime;
using Intermech.Runtime.ComInterop.Proxies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.SolidWorks.Integrator.Extensions;

internal sealed class RepairFileReferencesAction
{
  private IFileVault fileVaultService;
  private HashSet<DBObjectState> alreadyHandledObjects;

  public RepairFileReferencesAction(IFileVault fileVaultService)
  {
    this.fileVaultService = fileVaultService != null ? fileVaultService : throw new ArgumentNullException(nameof (fileVaultService));
    this.alreadyHandledObjects = new HashSet<DBObjectState>((IEqualityComparer<DBObjectState>) new DBObjectStateComparer());
  }

  public void Clear() => this.alreadyHandledObjects.Clear();

  public void RepairMovedFileReferences(
    long documentId,
    VersionsRulePackage versionsRule,
    CADSystemProxy cadSystem)
  {
    if (Intermech.Consts.IsUndefinedObjectId(documentId))
      throw new ArgumentException("Не задан идентификатор версии документа.", nameof (documentId));
    if (versionsRule == null)
      throw new ArgumentNullException(nameof (versionsRule));
    if (cadSystem == null)
      throw new ArgumentNullException(nameof (cadSystem));
    List<DBObjectState> listForObjectTree = this.fileVaultService.DBObjectsInfo.CreateStateListForObjectTree(documentId, versionsRule);
    if (listForObjectTree.Count == 1)
      return;
    List<DBObjectStateWithFiles> fileStates = this.fileVaultService.DBFilesInfo.GetFileStates((IList<DBObjectState>) listForObjectTree);
    HashSet<string> collection = new HashSet<string>(listForObjectTree.Count, (IEqualityComparer<string>) PathUtils.CurrentPathComparer);
    Dictionary<string, DBObjectState> dictionary = new Dictionary<string, DBObjectState>(listForObjectTree.Count, (IEqualityComparer<string>) PathUtils.CurrentPathComparer);
    foreach (DBObjectStateWithFiles objectStateWithFiles in fileStates)
    {
      foreach (FileState file in objectStateWithFiles.Files)
      {
        string str = Path.Combine(this.fileVaultService.WorkArea.AreaPath, file.FileName);
        collection.Add(Path.GetDirectoryName(str));
        DBObjectState owner = objectStateWithFiles.Owner;
        if (owner.IsEditableState && !this.alreadyHandledObjects.Contains(owner))
          dictionary.Add(str, owner);
      }
    }
    if (dictionary.Count == 0)
      return;
    this.fileVaultService.WorkArea.Publish((IList<DBObjectState>) listForObjectTree, (IReplaceFilePolicy) new PreserveAnyChanges());
    string searchFolders = string.Join(";", CollectionUtils.ToArray<string>((ICollection<string>) collection));
    // ISSUE: variable of a compiler-generated type
    SldWorks.SldWorks solidWorks = (SldWorks.SldWorks) null;
    string savedSearchFolders = (string) null;
    try
    {
      solidWorks = this.GetSolidWorksApplication();
      // ISSUE: reference to a compiler-generated method
      savedSearchFolders = solidWorks.GetSearchFolders(0);
      this.ConfigureSolidWorksApplication(solidWorks, searchFolders);
      List<CADDocumentProxy> cadDocumentProxyList = new List<CADDocumentProxy>(dictionary.Count);
      HashSet<DBObjectState> dbObjectStateSet = new HashSet<DBObjectState>((IEqualityComparer<DBObjectState>) new DBObjectStateComparer());
      foreach (KeyValuePair<string, DBObjectState> keyValuePair in dictionary)
      {
        string key = keyValuePair.Key;
        DBObjectState dbObjectState = keyValuePair.Value;
        CADDocumentProxy cadDocumentProxy = cadSystem.FindOpenDocument(key);
        if (cadDocumentProxy == null)
        {
          cadDocumentProxy = cadSystem.OpenDocument(key, true);
          cadDocumentProxyList.Add(cadDocumentProxy);
        }
        cadDocumentProxy.Save();
        dbObjectStateSet.Add(dbObjectState);
      }
      cadDocumentProxyList.Reverse();
      foreach (CADDocumentProxy cadDocumentProxy in cadDocumentProxyList)
        cadDocumentProxy.Close();
      foreach (DBObjectState dbObjectState in dbObjectStateSet)
      {
        this.fileVaultService.WorkArea.Save(dbObjectState.ObjectId);
        this.alreadyHandledObjects.Add(dbObjectState);
      }
    }
    finally
    {
      if (savedSearchFolders != null)
        SilentActionInvoker.Default.Invoke((Action) (() => this.ConfigureSolidWorksApplication(solidWorks, savedSearchFolders)));
      if (solidWorks != null)
      {
        Marshal.ReleaseComObject((object) solidWorks);
        solidWorks = (SldWorks.SldWorks) null;
      }
    }
  }

  private SldWorks.SldWorks GetSolidWorksApplication()
  {
    try
    {
      // ISSUE: variable of a compiler-generated type
      SldWorks.SldWorks activeObject = (SldWorks.SldWorks) Marshal.GetActiveObject("SldWorks.Application");
      return activeObject;
    }
    catch (Exception ex)
    {
      throw new ApplicationProxyException("Не удалось подключиться к SolidWorks.", ex);
    }
  }

  private void ConfigureSolidWorksApplication(SldWorks.SldWorks solidWorks, string searchFolders)
  {
    try
    {
      // ISSUE: reference to a compiler-generated method
      solidWorks.SetSearchFolders(0, searchFolders);
    }
    catch (COMException ex)
    {
      throw new ApplicationProxyException("Не удалось изменить пути поиска документов в настройках SolidWorks", (Exception) ex);
    }
  }
}

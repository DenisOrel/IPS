// Decompiled with JetBrains decompiler
// Type: Intermech.ImShape.Client.ImShapeCom
// Assembly: Intermech.ImShape.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EAEE73DE-1C1F-4401-8BB6-D181BFA32870
// Assembly location: D:\IPS\Client\Intermech.ImShape.Client.dll

using Intermech.CADInterface.Proxies;
using Intermech.DataFormats;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Runtime.ComInterop;
using Intermech.Tools.Data;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using Intermech.Tools.Integrators.Notifications;
using Interop.CADInterface;
using Interop.IMShape;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImShape.Client;

public static class ImShapeCom
{
  private static readonly ComObjectProvider shapeProvider = (ComObjectProvider) new ClsidProvider(typeof (ShapeComClass).GUID, true);
  private static IShapeCom3 _shape = (IShapeCom3) null;
  private static int _attrFileID = 0;
  private static int _nameAttrID = 0;
  private static int _designationAttrID = 0;

  private static void Connect()
  {
    if (ImShapeCom._shape != null)
      return;
    try
    {
      // ISSUE: reference to a compiler-generated field
      if (ImShapeCom.\u003C\u003Eo__5.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ImShapeCom.\u003C\u003Eo__5.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, IShapeCom3>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (IShapeCom3), typeof (ImShapeCom)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ImShapeCom._shape = ImShapeCom.\u003C\u003Eo__5.\u003C\u003Ep__0.Target((CallSite) ImShapeCom.\u003C\u003Eo__5.\u003C\u003Ep__0, ImShapeCom.shapeProvider.CreateInstance());
      ImShapeCom._shape.Init2(4);
    }
    catch (Exception ex)
    {
      throw new Exception($"{LocalizationHolder.rm.GetString("ImShape.Connect.Failed.Msg")} {ex.Message}");
    }
  }

  public static void Init()
  {
    ImShapeCom._attrFileID = MetaDataHelper.GetAttributeTypeID("cad0004b-306c-11d8-b4e9-00304f19f545");
    ImShapeCom._designationAttrID = MetaDataHelper.GetAttributeTypeID("cad0001f-306c-11d8-b4e9-00304f19f545");
    ImShapeCom._nameAttrID = MetaDataHelper.GetAttributeTypeID("cad00020-306c-11d8-b4e9-00304f19f545");
  }

  private static Guid DetectCAD(int objectTypeId)
  {
    Guid guid = Guid.Empty;
    IntegratorObject iobj = IntegratorServices.Find(objectTypeId);
    if (iobj != null)
    {
      IPDMBrowserService service = ServiceUtils.GetService<IPDMBrowserService>((object) ClientContext.Integrators.GetIntegrator(iobj, true), false);
      guid = service != null ? service.CADSystemId : Guid.Empty;
    }
    return guid;
  }

  private static long MainHandle
  {
    get
    {
      return (ServiceUtils.GetService<IMainFormUpdate>((object) ApplicationServices.Container, false) as IWin32Window).Handle.ToInt64();
    }
  }

  private static void OutputResult(StringBuilder sb)
  {
    IOutputView service = ServiceUtils.GetService<IOutputView>((object) ApplicationServices.Container, false);
    if (service == null)
      return;
    string category = LocalizationHolder.rm.GetString("ImShape_Name");
    service.WriteString(category, sb.ToString());
    service.WriteString(category, string.Empty);
    service.Activate(category);
    service.ShowView();
  }

  public static void AddDoc(ISelectedItems items)
  {
    if (items == null || items.Count <= 0)
      return;
    PdmModelsList pList = (PdmModelsList) new PdmModelsListClass();
    string bstrName = string.Empty;
    string bstrDesignation = string.Empty;
    Guid objectVersionGuid = Guid.Empty;
    bool flag = false;
    string empty = string.Empty;
    IArticleService service = ServiceUtils.GetService<IArticleService>((object) ServicesManager.ServiceContainer, true);
    if (service != null)
    {
      Dictionary<long, string> dictionary = new Dictionary<long, string>(items.Count);
      for (int index = 0; index < items.Count; ++index)
      {
        IDBTypedObjectID itemData = items.GetItemData(index, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IDBObject objectActualCopy = sessionKeeper.Session.GetObjectActualCopy(itemData.ObjectID, false);
          if (objectActualCopy != null)
          {
            IDBAttribute attributeById1 = objectActualCopy.GetAttributeByID(ImShapeCom._nameAttrID);
            bstrName = attributeById1 == null || attributeById1.Values == null || attributeById1.Values.Length == 0 ? string.Empty : Convert.ToString(attributeById1.Values[0]);
            IDBAttribute attributeById2 = objectActualCopy.GetAttributeByID(ImShapeCom._designationAttrID);
            bstrDesignation = attributeById2 == null || attributeById2.Values == null || attributeById2.Values.Length == 0 ? string.Empty : Convert.ToString(attributeById2.Values[0]);
            objectVersionGuid = objectActualCopy.ObjectGUID;
            if (!Intermech.Tools.Data.PDMHelper.IsDocumentWithArticles(itemData.ObjectType))
            {
              dictionary.Add(itemData.ObjectID, objectActualCopy.Caption);
              continue;
            }
            long[] articles = service.FindArticles(itemData.ObjectID, VersionsRuleSources.GetEditorRule().OwnerId, (object) sessionKeeper.Session);
            if (articles != null)
            {
              if (articles.Length != 0)
                goto label_14;
            }
            dictionary.Add(itemData.ObjectID, objectActualCopy.Caption);
            continue;
          }
          continue;
        }
label_14:
        Guid guid = ImShapeCom.DetectCAD(itemData.ObjectType);
        if (!(guid == Guid.Empty))
        {
          string bstrPathToFile;
          try
          {
            bstrPathToFile = ClientContext.FileVault.PublishTree(itemData.ObjectID, true, VersionsRuleSources.GetEditorRule(), (IFileArea) ClientContext.FileVault.WorkArea);
          }
          catch (Exception ex)
          {
            ExceptionHelper.ExceptionService.ShowException(ex);
            continue;
          }
          try
          {
            pList.AddDoc(PersistentIds.FromObjectVersion(objectVersionGuid), bstrDesignation, bstrName, bstrPathToFile, Convert.ToString((object) guid));
            flag = true;
          }
          catch (Exception ex)
          {
            ExceptionHelper.ExceptionService.ShowException(new Exception(ImShapeCom._shape.LastError, ex));
          }
        }
      }
      if (dictionary.Count > 0)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(LocalizationHolder.rm.GetString("ImShape_NeedExSave_Msg"));
        foreach (KeyValuePair<long, string> keyValuePair in dictionary)
          stringBuilder.AppendLine($"{keyValuePair.Value} (ID = {Convert.ToString(keyValuePair.Key)})");
        int num = (int) MessageBox.Show(stringBuilder.ToString(), LocalizationHolder.rm.GetString("ImShape_Msg"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      if (!flag)
        return;
      ImShapeCom.Connect();
      try
      {
        ImShapeCom._shape.RunAddPdmModelsDialog2(pList, ImShapeCom.MainHandle);
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionService.ShowException(new Exception(ImShapeCom._shape.LastError, ex));
      }
    }
    else
    {
      int num1 = (int) MessageBox.Show(LocalizationHolder.rm.GetString("ImShape_NullArtilceSrv_Msg"), LocalizationHolder.rm.GetString("ImShape_Msg"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  public static void AddDoc(IIntegrator integrator, List<IMShapeDocumentInfo> documents)
  {
    if (integrator == null || documents == null || documents.Count <= 0)
      return;
    ImShapeSystemSettingsService service1 = ServiceUtils.GetService<ImShapeSystemSettingsService>((object) ServicesManager.ServiceContainer, false);
    IArticleService service2 = ServiceUtils.GetService<IArticleService>((object) ServicesManager.ServiceContainer, false);
    if (service1 == null)
      return;
    Dictionary<long, int> dictionary = new Dictionary<long, int>(documents.Count);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (IMShapeDocumentInfo document in documents)
      {
        if (Intermech.Tools.Data.PDMHelper.IsDocumentWithArticles(document.ObjectTypeId))
        {
          if (service2 != null)
          {
            long[] articles = service2.FindArticles(document.ObjectId, VersionsRuleSources.GetEditorRule().OwnerId, (object) sessionKeeper.Session);
            if (articles == null || articles.Length == 0)
              continue;
          }
          QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(document.ObjectId);
          if (service1.TypeIDs.ContainsKey(objectInfo.ObjectTypeID) && service1.TypeIDs[objectInfo.ObjectTypeID])
            dictionary.Add(document.ObjectId, objectInfo.ObjectTypeID);
        }
      }
    }
    if (dictionary.Count <= 0)
      return;
    ImShapeCom.Connect();
    StringBuilder sb = new StringBuilder();
    sb.AppendLine($"{Convert.ToString(DateTime.Now)} - {LocalizationHolder.rm.GetString("ImShape_AddDoc_Error_Msg")}");
    bool flag = false;
    foreach (IMShapeDocumentInfo document in documents)
    {
      using (CADApiSession cadApiSession = new CADApiSession(integrator))
      {
        CADDocumentProxy cadDocumentProxy = cadApiSession.Application.OpenDocument(document.FilePath, false);
        try
        {
          ImShapeCom._shape.AddDocument(cadDocumentProxy.RawObject, true);
        }
        catch
        {
          flag = true;
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(document.ObjectId);
            sb.AppendLine($"'{objectInfo.Caption}' (ID = {document.ObjectId}) - {ImShapeCom._shape.LastError}");
          }
        }
      }
    }
    if (!flag)
      return;
    ImShapeCom.OutputResult(sb);
  }

  public static void SearchDoc(ISelectedItems items)
  {
    if (items == null || items.Count <= 0)
      return;
    ImShapeCom.Connect();
    IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    IArticleService service = ServiceUtils.GetService<IArticleService>((object) ServicesManager.ServiceContainer, true);
    Guid objectVersionGuid = Guid.Empty;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject baseArticle = service.FindBaseArticle(itemData.ObjectID, VersionsRuleSources.GetEditorRule().OwnerId, (object) sessionKeeper.Session);
      objectVersionGuid = baseArticle != null ? baseArticle.ObjectGUID : Guid.Empty;
    }
    if (objectVersionGuid != Guid.Empty)
    {
      bool pbIsRegistered = false;
      try
      {
        ImShapeCom._shape.RunSearchDialogFromPdm2(PersistentIds.FromObjectVersion(objectVersionGuid), ImShapeCom.MainHandle, out pbIsRegistered);
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionService.ShowException(new Exception(ImShapeCom._shape.LastError, ex));
      }
      if (pbIsRegistered)
        return;
      if (MessageBox.Show(LocalizationHolder.rm.GetString("ImShape.Search.UnregisteredObject.MsgNext"), LocalizationHolder.rm.GetString("ImShape.Information"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      try
      {
        IntegratorObject iobj = IntegratorServices.Find(itemData.ObjectType);
        if (iobj == null)
          return;
        string fullName = ClientContext.FileVault.PublishTree(itemData.ObjectID, true, VersionsRuleSources.GetEditorRule(), (IFileArea) ClientContext.FileVault.WorkArea);
        using (CADApiSession cadApiSession = new CADApiSession(ClientContext.Integrators.GetIntegrator(iobj, true)))
        {
          CADDocumentProxy cadDocumentProxy = cadApiSession.Application.OpenDocument(fullName, false);
          try
          {
            ImShapeCom._shape.RunSearchDialogByDoc2(cadDocumentProxy.RawObject, ImShapeCom.MainHandle);
          }
          catch (Exception ex)
          {
            ExceptionHelper.ExceptionService.ShowException(new Exception(ImShapeCom._shape.LastError, ex));
          }
        }
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionService.ShowException(ex);
      }
    }
    else
    {
      try
      {
        IntegratorObject iobj = IntegratorServices.Find(itemData.ObjectType);
        if (iobj == null)
          return;
        string fullName = ClientContext.FileVault.PublishTree(itemData.ObjectID, true, VersionsRuleSources.GetEditorRule(), (IFileArea) ClientContext.FileVault.WorkArea);
        using (CADApiSession cadApiSession = new CADApiSession(ClientContext.Integrators.GetIntegrator(iobj, true)))
        {
          IModelConfiguration2 rawObject = (IModelConfiguration2) cadApiSession.Application.OpenDocument(fullName, false).DefaultConfiguration.RawObject;
          try
          {
            ImShapeCom._shape.RunSearchDialogByConfiguration2(rawObject, ImShapeCom.MainHandle);
          }
          catch (Exception ex)
          {
            ExceptionHelper.ExceptionService.ShowException(new Exception(ImShapeCom._shape.LastError, ex));
          }
        }
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionService.ShowException(ex);
      }
    }
  }

  public static void SearchConfiguration(ISelectedItems items)
  {
    if (items == null || items.Count <= 0)
      return;
    ImShapeCom.Connect();
    IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    Guid objectVersionGuid = Guid.Empty;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      objectVersionGuid = sessionKeeper.Session.GetObjectInfo(itemData.ObjectID).VersionGuid;
    if (objectVersionGuid != Guid.Empty)
    {
      bool pbIsRegistered = false;
      try
      {
        ImShapeCom._shape.RunSearchDialogFromPdm2(PersistentIds.FromObjectVersion(objectVersionGuid), ImShapeCom.MainHandle, out pbIsRegistered);
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionService.ShowException(new Exception(ImShapeCom._shape.LastError, ex));
      }
      if (pbIsRegistered)
        return;
      string caption = LocalizationHolder.rm.GetString("ImShape.Information");
      if (MessageBox.Show(LocalizationHolder.rm.GetString("ImShape.Search.UnregisteredObject.MsgNext"), caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      long num1 = 0;
      List<Tuple<Guid, long>> articleDocuments = DBDocumentHelper.FindArticleDocuments(itemData.ObjectID, true, true, VersionsRuleSources.GetEditorRule());
      Dictionary<long, int> source = (Dictionary<long, int>) null;
      if (articleDocuments != null && articleDocuments.Count > 0)
      {
        using (SessionKeeper sk = new SessionKeeper())
          source = articleDocuments.Select<Tuple<Guid, long>, QuickObjectInfo>((Func<Tuple<Guid, long>, QuickObjectInfo>) (x => sk.Session.GetObjectInfo(x.Item2))).ToDictionary<QuickObjectInfo, long, int>((Func<QuickObjectInfo, long>) (x => x.ObjectID), (Func<QuickObjectInfo, int>) (y => y.ObjectTypeID));
        ImShapeSystemSettingsService service = ServiceUtils.GetService<ImShapeSystemSettingsService>((object) ServicesManager.ServiceContainer, false);
        if (service != null)
        {
          List<int> typeIDs = service.TypeIDs.Keys.ToList<int>();
          if (typeIDs != null && typeIDs.Count > 0)
            source = source.Where<KeyValuePair<long, int>>((Func<KeyValuePair<long, int>, bool>) (x => typeIDs.Contains(x.Value))).ToDictionary<KeyValuePair<long, int>, long, int>((Func<KeyValuePair<long, int>, long>) (x => x.Key), (Func<KeyValuePair<long, int>, int>) (y => y.Value));
        }
        if (source.Count > 1)
        {
          long[] numArray = SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("ImShape.SelectObject"), LocalizationHolder.rm.GetString("ImShape.SelectDocument"), (IDescriptor) new ListDescriptor(Intermech.Navigator.Consts.CategoryVersionsObjectNode, 0, LocalizationHolder.rm.GetString("ImShape.Documents"), (IList) source.Keys.ToList<long>()), SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromViews | SelectionOptions.DisableMultiselect);
          if (numArray == null || numArray.Length == 0)
            return;
          num1 = numArray[0];
        }
        else if (source.Count == 1)
          num1 = source.First<KeyValuePair<long, int>>().Key;
      }
      if (num1 != 0L)
      {
        try
        {
          IntegratorObject iobj = IntegratorServices.Find(source[num1]);
          if (iobj == null)
            return;
          string fullName = ClientContext.FileVault.PublishTree(num1, true, VersionsRuleSources.GetEditorRule(), (IFileArea) ClientContext.FileVault.WorkArea);
          using (CADApiSession cadApiSession = new CADApiSession(ClientContext.Integrators.GetIntegrator(iobj, true)))
          {
            CADDocumentProxy cadDocumentProxy = cadApiSession.Application.OpenDocument(fullName, false);
            try
            {
              ImShapeCom._shape.RunSearchDialogByDoc2(cadDocumentProxy.RawObject, ImShapeCom.MainHandle);
            }
            catch (Exception ex)
            {
              ExceptionHelper.ExceptionService.ShowException(new Exception(ImShapeCom._shape.LastError, ex));
            }
          }
        }
        catch (Exception ex)
        {
          ExceptionHelper.ExceptionService.ShowException(ex);
        }
      }
      else
      {
        int num2 = (int) MessageBox.Show(LocalizationHolder.rm.GetString("ImShape.Documen.3Dmodel.Empty"), caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }
    else
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("ImShape.SelectedObject.GuidEmpty"), LocalizationHolder.rm.GetString("ImShape.Information"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }
  }
}

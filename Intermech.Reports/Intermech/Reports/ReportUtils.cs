// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.ReportUtils
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Intermech.DataFormats;
using Intermech.Document.Client;
using Intermech.Document.Model;
using Intermech.Document.Model.ExternalDocuments;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Document;
using Intermech.Interfaces.Reports;
using Intermech.Navigator.Interfaces;
using Intermech.Search.Interfaces.Signs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Intermech.Reports;

/// <summary>
/// 
/// </summary>
public sealed class ReportUtils : MarshalByRefObject, IReportUtils
{
  /// <summary>
  /// 
  /// </summary>
  internal static readonly ReportUtils Instance = new ReportUtils();

  /// <summary>Восстановление / генерация данных документа</summary>
  /// <param name="reportsDoc">Базовый класс для передачи документов со стороны сервера</param>
  /// <param name="imDocNode">Визуальный узел документа</param>
  private bool DoRestoreDocumentData(ReportsBaseDoc reportsDoc, VisualNode imDocNode)
  {
    if (reportsDoc == null || imDocNode == null)
      return false;
    object obj;
    if (reportsDoc.Attributes.TryGetValue(ReportsConsts.CaptionAttrTypeGuid, out obj))
      imDocNode.Name = Convert.ToString(obj);
    int num = 0;
    foreach (ReportsBaseDoc reportsDoc1 in reportsDoc.Items)
    {
      if (reportsDoc1 != null)
      {
        VisualNode visualNode;
        if (reportsDoc1 is ReportsDocComplect)
          visualNode = (VisualNode) new DocumentsComplect();
        else if (reportsDoc1 is ReportsDoc reportsDoc2)
        {
          byte[] data = reportsDoc2.Data;
          if (data != null && data.Length != 0)
          {
            if (reportsDoc2.Attributes.ContainsKey(ReportsConsts.SourceLinkAttributeTypeGuid))
            {
              try
              {
                visualNode = (VisualNode) new ExternalDocumentCreator().CreateDocument(reportsDoc1.ObjectID, true);
              }
              catch (Exception ex)
              {
                LogManager.AddLine(ex, true);
                visualNode = (VisualNode) ReportsBaseTask.UnpackImDocument(data, true);
              }
            }
            else
            {
              ImDocument imDocument;
              visualNode = (VisualNode) (imDocument = ReportsBaseTask.UnpackImDocument(data, true));
              MemoryStream baseInputStream = new MemoryStream(data);
              InflaterInputStream inflaterInputStream = new InflaterInputStream((Stream) baseInputStream);
              MemoryStream destination = new MemoryStream();
              inflaterInputStream.CopyTo((Stream) destination);
              inflaterInputStream.Dispose();
              baseInputStream.Dispose();
              destination.Position = 0L;
              DocumentEditorPlugin.Instance.UpdateCheckSum((IUserSession) null, new CheckSumService(), (ImDocumentData) imDocument, (Stream) destination, true, true);
              DocumentEditorPlugin.UpdateDocumentDBObject(imDocument, reportsDoc1.ObjectID, false, false);
            }
          }
          else
            continue;
        }
        else
          continue;
        if (visualNode != null)
        {
          imDocNode.InsertChildNode(num++, (DocumentTreeNode) visualNode, false, true, false, false, false);
          ReportUtils.Instance.DoRestoreDocumentData(reportsDoc1, visualNode);
        }
      }
    }
    return true;
  }

  /// <summary>Восстановление / генерация данных документа</summary>
  /// <param name="reportsDoc">Базовый класс для передачи документов со стороны сервера</param>
  /// <param name="imDocNode">Визуальный узел документа</param>
  bool IReportUtils.RestoreComplectData(ReportsBaseDoc reportsDoc, out DocumentsComplect complect)
  {
    if (reportsDoc == null)
      throw new ArgumentNullException(nameof (reportsDoc));
    complect = new DocumentsComplect();
    return this.DoRestoreDocumentData(reportsDoc, (VisualNode) complect);
  }

  /// <summary>Восстановление / генерация данных документа</summary>
  /// <param name="reportsDoc">Базовый класс для передачи документов со стороны сервера</param>
  /// <param name="imDocNode">Визуальный узел документа</param>
  [Obsolete("Will be removed in IPS 6.0 - use IReportUtils.RestoreComplectData instead")]
  public static void ReportsDocGenerateData(ReportsBaseDoc reportsDoc, VisualNode imDocNode)
  {
    ReportUtils.Instance.DoRestoreDocumentData(reportsDoc, imDocNode);
  }

  /// <summary>Получение информации об объекте и его типе</summary>
  /// <param name="items"></param>
  /// <param name="objInfoList"></param>
  /// <param name="needCheckFromBase"></param>
  /// <returns></returns>
  internal static bool GetSelectedItemsInfo(
    ISelectedItems items,
    out IList<ObjInfoItem> objInfoList,
    bool needCheckFromBase)
  {
    objInfoList = (IList<ObjInfoItem>) new List<ObjInfoItem>();
    if (items == null || items.Count == 0)
      return false;
    List<ObjInfoItem> source = new List<ObjInfoItem>();
    for (int index = 0; index < items.Count; ++index)
    {
      long objectId = 0;
      int objTypeId = -1;
      if (items.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData2)
      {
        objectId = itemData2.ObjectID;
        objTypeId = itemData2.ObjectType;
      }
      else if (items.GetItemData(index, typeof (IDBObjectID)) is IDBObjectID itemData1)
      {
        objectId = itemData1.Value;
        if (items.GetItemData(index, typeof (IDBObjectTypeID)) is IDBObjectTypeID itemData)
          objTypeId = itemData.Value;
      }
      if (objectId != 0L)
        source.Add(new ObjInfoItem(objectId, objTypeId));
    }
    objInfoList.AddRange<ObjInfoItem>(source.Where<ObjInfoItem>((Func<ObjInfoItem, bool>) (item => item.ObjTypeID != -1)));
    ObjInfoItem[] array = source.Where<ObjInfoItem>((Func<ObjInfoItem, bool>) (item => item.ObjTypeID == -1)).ToArray<ObjInfoItem>();
    if (needCheckFromBase && ((IEnumerable<ObjInfoItem>) array).Any<ObjInfoItem>())
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        ObjInfoHelper.UpdateUnknownTypes((IEnumerable<ObjInfoItem>) array, sessionKeeper.Session);
    }
    objInfoList.AddRange<ObjInfoItem>((IEnumerable<ObjInfoItem>) array);
    GenericListHelper.MakeUnique<ObjInfoItem>((List<ObjInfoItem>) objInfoList);
    return true;
  }

  /// <summary>Получение имени контейнера параметров для объекта</summary>
  /// <param name="objectId"></param>
  /// <returns></returns>
  internal static string GetContainerName(long objectId)
  {
    if (objectId == 0L)
      return string.Empty;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return ReportUtils.GetContainerName(sessionKeeper.Session.GetObject(objectId, false));
  }

  /// <summary>Получение имени контейнера параметров для объекта</summary>
  /// <param name="dbObject"></param>
  internal static string GetContainerName(IDBObject dbObject)
  {
    return dbObject == null ? string.Empty : dbObject.ObjectGUID.ToString();
  }
}

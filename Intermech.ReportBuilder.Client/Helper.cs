// Decompiled with JetBrains decompiler
// Type: Intermech.ReportBuilder.Client.Helper
// Assembly: Intermech.ReportBuilder.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 84A30C1D-3856-44D0-92A6-A87D49736592
// Assembly location: D:\IPS\Client\Intermech.ReportBuilder.Client.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Document;
using Intermech.IO;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ReportBuilder.Client;

internal static class Helper
{
  public static long[] ConvertToInt64(ISelectedItems items)
  {
    if (items.Count == 0)
      return (long[]) null;
    List<long> longList = new List<long>();
    for (int index = 0; index < items.Count; ++index)
    {
      IDBObjectID itemData = (IDBObjectID) items.GetItemData(index, typeof (IDBObjectID));
      if (itemData != null)
        longList.Add(itemData.Value);
    }
    return longList.ToArray();
  }

  public static bool ObjectsInTypes(ISelectedItems items, List<int> possibleTypes)
  {
    for (int index = 0; index < items.Count; ++index)
    {
      if (!(items.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || possibleTypes.IndexOf(itemData.ObjectType) < 0)
        return false;
    }
    return true;
  }

  public static Stream LoadXMLFromObject(IDBObject templateObject, int fileAttributeID)
  {
    IDBAttribute attributeById = templateObject.GetAttributeByID(fileAttributeID);
    if (attributeById != null)
    {
      IBlobReader blobReader = attributeById as IBlobReader;
      BlobInformation blobInformation = blobReader.OpenBlob(0);
      try
      {
        if (blobInformation.RealFileSize > 0L)
        {
          Stream stream1 = (Stream) new MemoryStream(blobReader.ReadDataBlock());
          stream1.Position = 0L;
          if (blobInformation.ArcMethod == ArcMethods.ZLibPacked)
          {
            IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
            Stream stream2 = (Stream) new ImChunkedStream();
            Stream outStream = stream2;
            Stream inStream = stream1;
            service.UnpackStream(outStream, inStream);
            stream1.Close();
            stream1 = stream2;
            stream1.Position = 0L;
          }
          return stream1;
        }
      }
      finally
      {
        blobReader.CloseBlob();
      }
    }
    return (Stream) null;
  }

  public static long SaveToObjectDocument(
    IUserSession session,
    ImDocumentData document,
    int docTypeID)
  {
    IDBObject dbObject = session.GetObjectCollection(docTypeID).Create();
    dbObject.GetAttributeByID(session.IdentHelper.NameID).Value = (object) document.Name;
    using (MemoryStream aSourceStream = new MemoryStream())
    {
      document.SaveToXml((Stream) aSourceStream);
      aSourceStream.Position = 0L;
      BlobInformation aBlobInformation = new BlobInformation(0L, 0L, DateTime.Now, $"report_{dbObject.ObjectGUID}.imdx", ArcMethods.ZLibPacked, string.Empty);
      new BlobProcWriter(dbObject.ObjectID, AttributableElements.Object, session.IdentHelper.FileAttributeID, 0, 0, aBlobInformation, (Stream) aSourceStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
    }
    dbObject.CommitCreation(true);
    return dbObject.ObjectID;
  }
}

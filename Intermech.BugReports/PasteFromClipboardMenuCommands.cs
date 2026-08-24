// Decompiled with JetBrains decompiler
// Type: Intermech.BugReports.PasteFromClipboardMenuCommands
// Assembly: Intermech.BugReports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 16F80F46-2B9D-4747-9BFD-4CC209192F4E
// Assembly location: D:\IPS\Client\Intermech.BugReports.dll

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Navigator.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.BugReports;

internal class PasteFromClipboardMenuCommands
{
  private static void PasteFile(Bitmap bmp, IDBAttribute pasteFile, string fileName)
  {
    IBlobWriter blobWriter = (IBlobWriter) pasteFile;
    string fileName1 = fileName;
    string empty = string.Empty;
    using (MemoryStream memoryStream = new MemoryStream())
    {
      bmp.Save((Stream) memoryStream, ImageFormat.Bmp);
      memoryStream.Position = 0L;
      byte[] buffer = new byte[memoryStream.Length];
      memoryStream.Read(buffer, 0, buffer.Length);
      using (MemoryStream baseOutputStream = new MemoryStream())
      {
        DeflaterOutputStream deflaterOutputStream = new DeflaterOutputStream((Stream) baseOutputStream);
        deflaterOutputStream.Write(buffer, 0, buffer.Length);
        deflaterOutputStream.Finish();
        blobWriter.OpenBlob(new BlobInformation(memoryStream.Length, baseOutputStream.Length, DateTime.Now, fileName1, ArcMethods.ZLibPacked, empty), false);
        blobWriter.WriteDataBlock(baseOutputStream.ToArray());
      }
    }
  }

  public static void Paste(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long objectID = (items.GetItemData(0, typeof (IDBObjectID)) as IDBObjectID).Value;
      Bitmap data = (Bitmap) Clipboard.GetDataObject().GetData(System.Windows.Forms.DataFormats.Bitmap);
      int attributeId = sessionKeeper.Session.IdentHelper.GetAttributeID(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545").ToString());
      IDBAttribute pasteFile = sessionKeeper.Session.GetObject(objectID).Attributes.AddAttribute(attributeId, false);
      Random random = new Random();
      string fileName1 = random.Next(int.MaxValue).ToString() + ".bmp";
      if (pasteFile.IsNull)
      {
        PasteFromClipboardMenuCommands.PasteFile(data, pasteFile, fileName1);
        IMSObjectType objectType = MetaDataHelper.GetObjectType(new Guid("86b0b79c-2d71-4c13-80e5-4a208eee963f"));
        if (objectType == null)
          return;
        sessionKeeper.Session.GetObject(objectID).ObjectType = objectType.ObjectTypeID;
      }
      else
      {
        questionForm questionForm = new questionForm();
        int num = (int) questionForm.ShowDialog();
        if (questionForm.DialogResult != DialogResult.OK)
          return;
        if (questionForm.replace)
        {
          PasteFromClipboardMenuCommands.PasteFile(data, pasteFile, fileName1);
        }
        else
        {
          string fileName2 = random.Next(2000000).ToString() + ".bmp";
          pasteFile.AddValue((object) null);
          PasteFromClipboardMenuCommands.PasteFile(data, pasteFile, fileName2);
        }
      }
    }
  }
}

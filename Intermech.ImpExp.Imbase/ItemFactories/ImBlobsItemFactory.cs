// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImBlobsItemFactory
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.IO;
using System;
using System.Data;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal class ImBlobsItemFactory : PumpItemFactory
{
  public static string TableName = "IM_BLOBS";
  public static string TableColumns = "F_KEY,F_USED,F_SOURCE,F_HASH,F_BLOB";
  private int _idxKey;
  private int _idxUsed;
  private int _idxBlob;
  private int _idxSource;
  private int _idxHash;
  private string[] _pictureExtensions;
  private IPackedStream _packedStream;

  public ImBlobsItemFactory(IDataReader idr, IAppManager appMgr)
    : base(ImBlobsItemFactory.TableName, idr, appMgr)
  {
    this._idxKey = this.getFieldIndex("F_KEY");
    this._idxUsed = this.getFieldIndex("F_USED");
    this._idxBlob = this.getFieldIndex("F_BLOB");
    this._idxSource = this.getFieldIndex("F_SOURCE");
    this._idxHash = this.getFieldIndex("F_HASH");
    this._pictureExtensions = new string[12]
    {
      ".BMP",
      ".GIF",
      ".TIF",
      ".TIFF",
      ".PNG",
      ".JPG",
      ".JPEG",
      ".EXIF",
      ".ICO",
      ".EMF",
      ".WMF",
      ".SLD"
    };
    this._packedStream = (IPackedStream) ServicesManager.ServiceContainer.GetService(typeof (IPackedStream));
  }

  public IImBlobsItem NewItem(IDataReader idr) => this.NewItem(idr, BlobType.Other);

  public IImBlobsItem NewItem(IDataReader idr, BlobType type)
  {
    ImBlobsItem imBlobsItem = new ImBlobsItem();
    imBlobsItem.key = this.getInt32(idr, this._idxKey);
    imBlobsItem.used = this.getInt32(idr, this._idxUsed);
    try
    {
      string path = this.getString(idr, this._idxSource).Trim();
      char[] invalidChars = Path.GetInvalidFileNameChars();
      if (path.IndexOfAny(invalidChars) >= 0)
        path = new string(Array.FindAll<char>(path.ToCharArray(), (Predicate<char>) (c => Array.IndexOf<char>(invalidChars, c) < 0)));
      imBlobsItem.source = Path.GetFileName(path);
    }
    catch
    {
      imBlobsItem.source = string.Empty;
    }
    imBlobsItem.source = imBlobsItem.source.Equals(string.Empty) ? "tmp_file" : imBlobsItem.source;
    imBlobsItem.hash = this.getInt32(idr, this._idxHash);
    imBlobsItem.tmpFileName = Path.Combine(Path.GetTempPath(), imBlobsItem.Source);
    string tmpExt = Path.GetExtension(imBlobsItem.Source).ToUpper();
    if (type == BlobType.Other)
    {
      if (Array.Exists<string>(this._pictureExtensions, (Predicate<string>) (x => x.Equals(tmpExt))))
        imBlobsItem.type = BlobType.Picture;
      else if (tmpExt.Equals(".RTF"))
        imBlobsItem.type = BlobType.Text;
      else if (tmpExt.Equals(".SETCHR"))
        imBlobsItem.type = BlobType.Template;
    }
    else
      imBlobsItem.type = type;
    if (!idr.IsDBNull(this._idxBlob))
    {
      ImChunkedStream stream = (ImChunkedStream) null;
      try
      {
        stream = BlobHelper.ReadBlob(this._packedStream, idr, this._idxBlob, imBlobsItem.source);
        imBlobsItem.FileSize = stream.Length;
        BinaryWriter bWriter = new BinaryWriter((Stream) new FileStream(imBlobsItem.tmpFileName, FileMode.OpenOrCreate, FileAccess.Write), Encoding.UTF8);
        try
        {
          BlobHelper.WriteBlob(this._packedStream, bWriter, imBlobsItem.type, stream, imBlobsItem.Source, out imBlobsItem.isZipped);
        }
        finally
        {
          bWriter.Flush();
          bWriter.Close();
        }
      }
      catch (Exception ex)
      {
        this.appMngr.AddWarningMessage(ex.Message);
      }
      finally
      {
        stream?.Close();
      }
    }
    return (IImBlobsItem) imBlobsItem;
  }
}

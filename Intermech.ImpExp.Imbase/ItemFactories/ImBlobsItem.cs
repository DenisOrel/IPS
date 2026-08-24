// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImBlobsItem
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.IO;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal sealed class ImBlobsItem : IImBlobsItem
{
  public int key;
  public int used;
  public string source = "";
  public int hash;
  public string tmpFileName = "";
  public long tmpFileSize;
  public BlobType type = BlobType.Other;
  public bool isZipped;
  public long objectID;

  public int Key => this.key;

  public BlobType BlobType
  {
    get => this.type;
    set => this.type = value;
  }

  public int Used => this.used;

  public string Source => this.source;

  public int Hash => this.hash;

  public string TmpFileName
  {
    get => this.tmpFileName;
    set => this.tmpFileName = value;
  }

  public bool IsZipped => this.isZipped;

  public long ObjectID
  {
    get => this.objectID;
    set
    {
      if (this.objectID == value)
        return;
      this.objectID = value;
    }
  }

  public long FileSize
  {
    get => this.tmpFileSize;
    set => this.tmpFileSize = value;
  }

  public void UnpackTempFile(Encoding encoding)
  {
    if (!this.isZipped)
      return;
    ImChunkedStream outStream = new ImChunkedStream();
    FileStream inStream = new FileStream(this.tmpFileName, FileMode.Open, FileAccess.Read);
    try
    {
      ((IPackedStream) ServicesManager.ServiceContainer.GetService(typeof (IPackedStream))).UnpackStream((Stream) outStream, (Stream) inStream);
      outStream.Position = 0L;
    }
    catch
    {
    }
    finally
    {
      inStream.Close();
    }
    FileStream fileStream = new FileStream(this.tmpFileName, FileMode.OpenOrCreate, FileAccess.Write);
    try
    {
      if (encoding != null)
      {
        ImChunkedStream imChunkedStream = new ImChunkedStream();
        using (StreamReader streamReader = new StreamReader((Stream) outStream, encoding))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) imChunkedStream))
          {
            string str;
            while ((str = streamReader.ReadLine()) != null)
              streamWriter.WriteLine(str);
            imChunkedStream.WriteTo((Stream) fileStream);
          }
        }
      }
      else
        outStream.WriteTo((Stream) fileStream);
      this.isZipped = false;
      this.tmpFileSize = fileStream.Length;
    }
    finally
    {
      fileStream.Close();
    }
  }
}

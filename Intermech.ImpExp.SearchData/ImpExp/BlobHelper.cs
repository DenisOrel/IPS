// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.BlobHelper
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System.IO;

#nullable disable
namespace Intermech.ImpExp;

public class BlobHelper
{
  private static string _tempFN = "";
  private static string _tempFNMask = "";
  private static int _FNcounter = 0;
  private const string _fnmask = "pumpblob{0}.dat";
  private static long _fileSize;
  private static string _tempPath = "";

  static BlobHelper()
  {
    BlobHelper._tempFN = BlobHelper.TempPath + $"pumpblob{""}.dat";
    BlobHelper._tempFNMask = Path.GetTempPath() + "pumpblob{0}.dat";
  }

  public static void Reset() => BlobHelper._FNcounter = 0;

  public static string TempFileName
  {
    get => string.Format(BlobHelper._tempFNMask, (object) BlobHelper._FNcounter);
  }

  public static string NextFileName
  {
    get
    {
      ++BlobHelper._FNcounter;
      return BlobHelper.TempFileName;
    }
  }

  public static void ReserveBlob(SaveToStreamDelegate func)
  {
    FileStream fileStream = new FileStream(BlobHelper.NextFileName, FileMode.Create);
    try
    {
      func((Stream) fileStream);
    }
    finally
    {
      BlobHelper._fileSize = fileStream.Length;
      fileStream.Close();
    }
  }

  public static void ReserveBlob(string s)
  {
    FileStream fileStream = new FileStream(BlobHelper.NextFileName, FileMode.Create);
    try
    {
      StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
      streamWriter.Write(s);
      streamWriter.Flush();
    }
    finally
    {
      BlobHelper._fileSize = fileStream.Length;
      fileStream.Close();
    }
  }

  public static void ReserveBlob(byte[] arr)
  {
    ++BlobHelper._FNcounter;
    FileStream fileStream = new FileStream(BlobHelper.TempFileName, FileMode.Create);
    try
    {
      fileStream.Write(arr, 0, arr.Length);
    }
    finally
    {
      BlobHelper._fileSize = (long) arr.Length;
      fileStream.Close();
    }
  }

  public static void ReserveZBlob(SaveToStreamDelegate func)
  {
    ++BlobHelper._FNcounter;
    MemoryStream inStream = new MemoryStream();
    try
    {
      func((Stream) inStream);
      FileStream outStream = new FileStream(BlobHelper.TempFileName, FileMode.Create);
      try
      {
        ((IPackedStream) ServicesManager.ServiceContainer.GetService(typeof (IPackedStream))).PackStream((Stream) outStream, (Stream) inStream, 5, (PercentEventHandler) null);
        BlobHelper._fileSize = outStream.Length;
      }
      finally
      {
        outStream.Close();
      }
    }
    finally
    {
      inStream.Close();
    }
  }

  public static void UseFile(string fileName)
  {
    BlobHelper._fileSize = new FileInfo(fileName).Length;
  }

  public static long FileSize => BlobHelper._fileSize;

  public static void Clear()
  {
    foreach (string file in Directory.GetFiles(Path.GetTempPath(), $"pumpblob{"*"}.dat"))
      File.Delete(file);
  }

  public static string TempPath
  {
    get
    {
      if (BlobHelper._tempPath == "")
        BlobHelper._tempPath = Path.GetTempPath();
      return BlobHelper._tempPath;
    }
  }
}

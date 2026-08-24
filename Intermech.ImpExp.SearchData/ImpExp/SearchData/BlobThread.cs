// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.BlobThread
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using System;
using System.IO;
using System.Threading;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal class BlobThread
{
  protected SearchDataPlugin plugin;
  private long _totalBlobs;
  private double _totalMB;
  private double _totalSeconds;
  private static string _errorMessage = "";
  internal DateTime DocPumperStopTime = DateTime.MinValue;
  internal bool RunOnce;
  private Thread _thread;

  public BlobThread(SearchDataPlugin plugin) => this.plugin = plugin;

  private void Execute()
  {
    string blobsIndexFileName = this.plugin.BlobsIndexFileName;
    if (!new FileInfo(blobsIndexFileName).Exists)
      return;
    BasePumpHelper.AppManager.AddInfoMessage("Фоновая закачка файлов документов запущена");
    this._totalBlobs = 0L;
    this._totalMB = 0.0;
    this._totalSeconds = 0.0;
    CacheCategory cacheCategory = PumpCache.Category[ImportingCategory.DocFiles];
    try
    {
      IUserSession userSession = BasePumpHelper.Session.Clone("ImpExp.SearchData.Execute");
      try
      {
        IBlobImporter customService = userSession.GetCustomService(typeof (IBlobImporter)) as IBlobImporter;
        long num1 = 0;
        DateTime dateTime = DateTime.MinValue;
        while (dateTime < this.DocPumperStopTime || this.RunOnce)
        {
          if (this.RunOnce)
          {
            this.RunOnce = false;
            this.DocPumperStopTime = DateTime.MinValue;
          }
          int num2 = 0;
          if (dateTime != DateTime.MinValue)
            Thread.Sleep(3000);
          using (FileStream fileStream = new FileStream(blobsIndexFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
          {
            dateTime = DateTime.Now;
            fileStream.Position = num1;
            while (fileStream.Position < fileStream.Length)
            {
              long oldKey = new BinaryReader((Stream) fileStream).ReadInt64();
              BlobInformation4Import blobInfo = this.plugin.Formatter.Deserialize((Stream) fileStream) as BlobInformation4Import;
              ++num2;
              if (File.Exists(blobInfo.LocalFileName) && cacheCategory.GetValue((object) oldKey) == null)
              {
                DateTime now = DateTime.Now;
                long blobID = customService.AddBlob(userSession.SessionGUID, blobInfo);
                ++this._totalBlobs;
                this._totalMB += (double) blobInfo.PackedFileSize / 1048576.0;
                this._totalSeconds += (DateTime.Now - now).TotalMilliseconds / 1000.0;
                cacheCategory.AddValue((object) oldKey, 0L, blobInfo.FileName, (ITagImportObject) new BlobTag(blobInfo.AttributeID, blobID, blobInfo.ObjectID, blobInfo.ModifyDate));
                File.Delete(blobInfo.LocalFileName);
              }
            }
            num1 = fileStream.Position;
          }
        }
        FileInfo[] files = new DirectoryInfo(this.plugin.BlobsPath).GetFiles("*.*", SearchOption.AllDirectories);
        bool flag = false;
        foreach (FileSystemInfo fileSystemInfo in files)
        {
          if (fileSystemInfo.FullName != blobsIndexFileName)
          {
            flag = true;
            break;
          }
        }
        FileInfo fileInfo = new FileInfo(blobsIndexFileName);
        if (flag)
        {
          fileInfo.MoveTo($"{blobsIndexFileName}.{fileInfo.LastWriteTime.ToString("yyyyMMddHHmmss")}");
        }
        else
        {
          fileInfo.Delete();
          new DirectoryInfo(this.plugin.BlobsPath).Delete(true);
        }
      }
      finally
      {
        userSession.Logout("ImpExp.SearchData.Execute");
        BasePumpHelper.AppManager.AddInfoMessage($"Фоновая закачка файлов документов завершена (средняя скорость {this.plugin.BlobThread.BlobsPerSecond:0.0} ф/с, {this.plugin.BlobThread.MBPerSecond:0.0} МБ/с)");
      }
    }
    catch (Exception ex)
    {
      BlobThread._errorMessage = ex.Message;
      BasePumpHelper.AppManager.AddErrorMessage("Фоновая закачка файлов документов завершена с ошибкой: " + BlobThread.ErrorMessage);
    }
    finally
    {
      cacheCategory.Release();
    }
  }

  public static string ErrorMessage => BlobThread._errorMessage;

  public void Start(bool runOnce = false)
  {
    this.RunOnce = runOnce;
    this.DocPumperStopTime = DateTime.MaxValue;
    this._thread = new Thread(new ThreadStart(this.Execute));
    this._thread.IsBackground = true;
    this._thread.Name = "ImportBlobsThread";
    this._thread.Start();
  }

  public bool IsAlive => this._thread != null && this._thread.IsAlive;

  public double BlobsPerSecond
  {
    get => this._totalSeconds == 0.0 ? 0.0 : (double) this._totalBlobs / this._totalSeconds;
  }

  public double MBPerSecond => this._totalSeconds == 0.0 ? 0.0 : this._totalMB / this._totalSeconds;
}

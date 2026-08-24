// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpWorkCopiesClass
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для перекачки рабочих копий документов", "Перекачка рабочих копий документов")]
public class PumpWorkCopiesClass : PumpClass
{
  private readonly DateTime MaxSearchFileDateToProceed = new DateTime(2016, 12, 31 /*0x1F*/);
  private readonly DateTime MaxIPSFileDateToProceed = new DateTime(2017, 1, 8, 23, 59, 59);
  protected SearchDataPlugin plugin;
  protected readonly DateTime MinFileDateTime = new DateTime(1899, 12, 30);
  private CacheCategory _docsCache;

  internal static Guid PumperGUID => new Guid("{DF12FE47-F575-45A4-8651-FB3B31C68359}");

  protected override Guid GUID => PumpWorkCopiesClass.PumperGUID;

  public PumpWorkCopiesClass(SearchDataPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
    throw new InvalidOperationException("OKBM only!");
  }

  public override void Exam() => this.ExamCheckPoint("Проверка данных успешно завершена", 100);

  public override void Pump()
  {
    SimpleLogger simpleLogger = new SimpleLogger(Path.Combine(Application.StartupPath, "WorkCopiesImport.log"));
    this._docsCache = PumpCache.Category[ImportingCategory.Documents];
    try
    {
      FileStores.MainDBConnection = this.plugin.idb.DbConnection;
      using (IDbCommand command = this.plugin.idb2.CreateCommand())
      {
        this.PumpCheckPoint("Определение количества рабочих копий для перекачки", 0);
        string str = "where d.doc_id = r.doc_id and d.arc_dir_id <> 0 and r.ver_status <> 0";
        command.CommandText = "select count(*) from doclist d, rc r " + str;
        int int32_1 = Convert.ToInt32(command.ExecuteScalar());
        this.SetCountPumpRecords(int32_1);
        simpleLogger.Write($"{command.CommandText}: {int32_1} result(s)");
        command.CommandText = $"select r.rec_id, r.doc_id, r.version_id, d.arc_dir_id, r.doc_type from doclist d, rc r  {str} order by 2,3";
        IDataReader dataReader = command.ExecuteReader();
        try
        {
          int index = 1;
          string format = "Перекачка рабочих копий ({0} из {1})";
          while (dataReader.Read())
          {
            int int32_2 = Convert.ToInt32(dataReader["doc_id"]);
            int int32_3 = Convert.ToInt32(dataReader["doc_id"]);
            int int32_4 = Convert.ToInt32(dataReader["version_id"]);
            int int32_5 = Convert.ToInt32(dataReader["arc_dir_id"]);
            int num1 = PumpHelper.IsTechcardDocument(Convert.ToInt32(dataReader["doc_type"])) ? 1 : 0;
            this.PumpCheckPoint(string.Format(format, (object) index, (object) int32_1), this.CalculatePercent(int32_1, index, 1, 99));
            ++index;
            if (num1 == 0 && this._docsCache.GetNewKey((object) -int32_2) <= 0L)
            {
              DictionaryValue dictionaryValue = this._docsCache.GetValue((object) int32_3);
              long num2 = 0;
              long objectID = 0;
              if (dictionaryValue != null)
              {
                num2 = dictionaryValue.NewObjectID;
                (dictionaryValue.Tag as DocumentTag).Versions.TryGetValue(int32_4, out objectID);
              }
              simpleLogger.Write($"Перекачка раб. копии Search (DocID={int32_3}, VersionID={int32_4}) -> IPS (ID={(num2 != 0L ? (object) num2.ToString() : (object) "?")}, ObjectID={(objectID != 0L ? (object) objectID.ToString() : (object) "?")})");
              if (dictionaryValue == null)
              {
                BasePumpHelper.AppManager.AddWarningMessage($"Документ (DOC_ID={int32_3}) не закачан, невозможно перекачать рабочую копию!");
              }
              else
              {
                string fileStoreAlias = PumpHelper.FileStoreAliases[int32_5];
                FileStore fileStore = FileStores.FS[fileStoreAlias];
                if (fileStore == null)
                {
                  BasePumpHelper.AppManager.AddWarningMessage($"Ошибка получения файла документа \"(DOC_ID={int32_3})\". Файловый шкаф \"{fileStoreAlias}\" не найден");
                }
                else
                {
                  using (IDataReader reader = BasePumpHelper.S4Query(fileStore.Connection, $"select FLNAME, OWNER, FILESIZE, FILEDATE, 0 as LINKTYPE, 0 as AUTHOR, FILEBODY {fileStore.AddColumns} from {fileStore.TableName} where FILE_ID=@p1 and VERSION_ID=@p2", CommandBehavior.SequentialAccess, (object) -int32_3, (object) int32_4))
                  {
                    if (reader.Read())
                    {
                      string fileName1 = reader[0].ToString();
                      int int32_6 = BasePumpHelper.ToInt32(reader[1]);
                      int int32_7 = BasePumpHelper.ToInt32(reader[2]);
                      // ISSUE: variable of a boxed type
                      __Boxed<DateTime> dateTime1 = (System.ValueType) reader.GetDateTime(3);
                      DateTime minDbDateTime = PumpHelper.MinDBDateTime;
                      DateTime dateTime2 = Convert.ToDateTime((object) dateTime1);
                      if (this.MaxSearchFileDateToProceed != DateTime.MinValue && dateTime2 > this.MaxSearchFileDateToProceed)
                      {
                        simpleLogger.Write($"Пропущено, файл в Search был модифицирован после {this.MaxSearchFileDateToProceed}");
                      }
                      else
                      {
                        int int32_8 = BasePumpHelper.ToInt32(reader[4]);
                        int int32_9 = BasePumpHelper.ToInt32(reader[5]);
                        string tempPath = BlobHelper.TempPath;
                        string nextFileName = BlobHelper.NextFileName;
                        fileStore.WriteFileBody(reader, nextFileName);
                        BlobHelper.UseFile(nextFileName);
                        ArcMethods arcMethod = int32_6 < 1 || int32_6 > 3 ? ArcMethods.NotPacked : ArcMethods.ZLibPacked;
                        string fileName2 = "";
                        if (int32_6 > 3)
                          fileName2 = CabHelper.ExtractCAB(nextFileName, tempPath);
                        if (fileName2 != "")
                        {
                          FileInfo fileInfo = new FileInfo(fileName2);
                          File.Delete(nextFileName);
                          string destFileName = nextFileName;
                          fileInfo.MoveTo(destFileName);
                        }
                        IDBObject dbObject = BasePumpHelper.Session.GetObject(objectID, false);
                        if (dbObject == null)
                        {
                          BasePumpHelper.AppManager.AddWarningMessage($"Объект ({objectID}) не найден, невозможно перекачать рабочую копию!");
                          simpleLogger.Write("Пропущено, объект в IPS не найден");
                        }
                        else if (dbObject.CheckoutBy != 0L)
                        {
                          simpleLogger.Write("Пропущено, объект в IPS взят на изменение");
                        }
                        else
                        {
                          IDBAttribute attributeById = dbObject.GetAttributeByID(PumpHelper.AttrTypeFileID);
                          if (attributeById == null)
                          {
                            simpleLogger.Write("Пропущено, объект в IPS не имеет файлов");
                          }
                          else
                          {
                            if (this.MaxIPSFileDateToProceed != DateTime.MinValue)
                            {
                              try
                              {
                                if ((attributeById as IBlobReader).OpenBlob(-1).ModifyDate > this.MaxIPSFileDateToProceed)
                                {
                                  simpleLogger.Write($"Пропущено, файл в IPS был модифицирован после {this.MaxIPSFileDateToProceed}");
                                  continue;
                                }
                              }
                              catch (Exception ex)
                              {
                                simpleLogger.Write("Ошибка чтения файла: " + ex.Message);
                              }
                            }
                            bool flag = dbObject.ObjectModifyMode == ObjectModifyModes.Checkout;
                            if (flag)
                            {
                              try
                              {
                                dbObject = dbObject.CheckOut();
                              }
                              catch (Exception ex)
                              {
                                simpleLogger.Write("Пропущено, ошибка взятия на изменение: " + ex.Message);
                                continue;
                              }
                              attributeById = dbObject.GetAttributeByID(PumpHelper.AttrTypeFileID);
                            }
                            try
                            {
                              if (attributeById is IBlobWriter blobWriter)
                              {
                                long file_author = 0;
                                if (int32_9 != 0)
                                  file_author = BasePumpHelper.UsersCache.GetNewKey((object) int32_9);
                                BlobInformation blobInfo = new BlobInformation((long) int32_7, BlobHelper.FileSize, dateTime2, fileName1, arcMethod, "", this.LinkTypeToFileTypes(int32_8), file_author);
                                if (blobWriter.OpenBlob(blobInfo, false))
                                {
                                  FileStream fileStream = new FileStream(nextFileName, FileMode.Open);
                                  try
                                  {
                                    byte[] array = new byte[131072 /*0x020000*/];
                                    int newSize;
                                    do
                                    {
                                      newSize = fileStream.Read(array, 0, array.Length);
                                      if (newSize < array.Length)
                                        Array.Resize<byte>(ref array, newSize);
                                      blobWriter.WriteDataBlock(array);
                                    }
                                    while (newSize == 131072 /*0x020000*/);
                                  }
                                  finally
                                  {
                                    fileStream.Close();
                                  }
                                }
                                this._docsCache.AddValue((object) -int32_2, 1L);
                              }
                            }
                            catch (Exception ex)
                            {
                              simpleLogger.Write("Ошибка записи файла: " + ex.Message);
                            }
                            finally
                            {
                              if (flag)
                              {
                                try
                                {
                                  dbObject.CheckIn();
                                }
                                catch (Exception ex)
                                {
                                  simpleLogger.Write("Ошибка возврата в архив: " + ex.Message);
                                }
                              }
                            }
                          }
                        }
                      }
                    }
                    else
                      simpleLogger.Write("Пропущено, файл рабочей копии не найден");
                  }
                }
              }
            }
          }
        }
        finally
        {
          dataReader.Close();
          BlobHelper.Clear();
        }
        this.PumpCheckPoint("Перекачка рабочих копий успешно завершена", 100);
        simpleLogger.Write("=========Pump end\r\n\r\n");
      }
    }
    catch (Exception ex)
    {
      simpleLogger.Write($"=========Pump abort ({ex.Message}\r\n{ex.StackTrace})\r\n\r\n");
      throw;
    }
    finally
    {
      this._docsCache.Release();
    }
  }

  private FileTypes LinkTypeToFileTypes(int linkType)
  {
    return linkType == -1 ? FileTypes.ftRedlining : (FileTypes) linkType;
  }

  private class PackFlag
  {
    public const int NotPacked = 0;
    public const int MinZLIBMethodID = 1;
    public const int MaxZLIBMethodID = 3;
  }
}

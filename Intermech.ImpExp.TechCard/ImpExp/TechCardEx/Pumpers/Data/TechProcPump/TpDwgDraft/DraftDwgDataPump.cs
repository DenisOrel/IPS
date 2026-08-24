// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCardEx.Pumpers.Data.TechProcPump.TpDwgDraft.DraftDwgDataPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Interface.Techcard;
using Intermech.ImpExp.SearchData;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCardEx.Pumpers.Data.TechProcPump.TpDwgDraft;

[TaskDescription("Инициализация для перекачки данных эскизов AutoCad", "Перекачка данных эскизов AutoCad")]
public class DraftDwgDataPump : PumpClass
{
  private IDataBase _searchConnection;
  private readonly Guid _guid = new Guid("{E3CAE356-4599-42BA-BB30-340188BCE62B}");
  private readonly HashSet<long> _importedTechProcInfo = new HashSet<long>();
  private CacheCategory _draftCache;
  private readonly SimpleLogger _logger;

  private void LoadImportedTechProcData()
  {
    IImportingData importingData1;
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      importingData1 = (IImportingData) null;
    else
      importingData1 = service.GetCache(ImportingCategory.TechProcessPump);
    IImportingData importingData2 = importingData1;
    if (importingData2 == null)
      return;
    try
    {
      IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
      command.CommandText = " SELECT\r\n                                      V.F_KEY,\r\n                                      D.F_DOCID,\r\n                                      V.F_VERSION\r\n                                    FROM\r\n                                      TP_VERSIONS V,\r\n                                      TC_ARCDOCS  D\r\n                                    WHERE\r\n                                      V.F_TCKEY = D.F_KEY";
      using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        int num = 0;
        try
        {
          while (dataReader.Read())
          {
            ++num;
            int int32 = Convert.ToInt32(dataReader[0]);
            if (importingData2.GetValue(ImportingCategory.TechProcessPump, (object) int32) != null)
              this._importedTechProcInfo.Add(TechcardConsts.Utils.CodeHashCode(Convert.ToInt32(dataReader[1]), Convert.ToInt32(dataReader[2])));
          }
        }
        finally
        {
          dataReader.Close();
        }
        if (num == 0)
          this.plugin.appManager.AddErrorMessage("Ошибка загрузки списка имортированных ТП. Дальнейшая закачка невозможна!");
        else
          this._logger.Write($"Загружено импортированных ТП: {num}");
      }
    }
    finally
    {
      service.ReleaseCache(ImportingCategory.TechProcessPump);
    }
  }

  private void AddToDraftsCache(long key, TechDraftTag tag)
  {
    try
    {
      this._draftCache.AddValue((object) key, 1L, (ITagImportObject) tag);
    }
    catch
    {
    }
  }

  private void PumpDraftData(IImportedObjectList impObjList)
  {
    if (!(ServicesManager.GetService(typeof (ITechCardTypeService)) is ITechCardTypeService service))
    {
      TechcardConsts.Plugin.appManager.AddErrorMessage($"Сервис \"{typeof (ITechCardTypeService)}\" не найден ");
    }
    else
    {
      try
      {
        FileStores.MainDBConnection = this._searchConnection.DbConnection;
        using (IDbCommand command = this._searchConnection.CreateCommand())
        {
          this.PumpCheckPoint("Определение количества докуменов для обработки", 10);
          string str1 = $"WHERE D.DOC_ID = R.DOC_ID AND D.ARC_DIR_ID <> 0 AND R.DOC_TYPE IN ({(PumpHelper.TechcardDocTypes.Keys.Count > 0 ? (object) string.Join<int>(",", (IEnumerable<int>) PumpHelper.TechcardDocTypes.Keys) : (object) "-1")})";
          command.CommandText = "SELECT COUNT(*) FROM DOCLIST D, RC R " + str1;
          int int32_1 = Convert.ToInt32(command.ExecuteScalar());
          this._logger.Write($"{command.CommandText}: {int32_1} result(s)");
          command.CommandText = $"SELECT R.REC_ID, R.DOC_ID, R.VERSION_ID, D.ARC_DIR_ID, R.DOC_TYPE FROM DOCLIST D, RC R  {str1} ORDER BY 2,3";
          IDataReader dataReader = command.ExecuteReader();
          try
          {
            int index = 1;
            string format = "Обработка документа ({0} из {1})";
            while (dataReader.Read())
            {
              int int32_2 = Convert.ToInt32(dataReader["doc_id"]);
              int int32_3 = Convert.ToInt32(dataReader["version_id"]);
              int int32_4 = Convert.ToInt32(dataReader["arc_dir_id"]);
              int num1 = PumpHelper.IsTechcardDocument(Convert.ToInt32(dataReader["doc_type"])) ? 1 : 0;
              if (index % 500 == 0 || index == int32_1 - 1)
                this.PumpCheckPoint(string.Format(format, (object) index, (object) int32_1), this.CalculatePercent(int32_1, index, 11, 99));
              ++index;
              if (num1 != 0)
              {
                long oldKey = TechcardConsts.Utils.CodeHashCode(Math.Abs(int32_2), int32_3);
                if (this._importedTechProcInfo.Contains(oldKey))
                {
                  DictionaryValue dictionaryValue = this._draftCache.GetValue((object) oldKey);
                  long num2 = 0;
                  long objectID = 0;
                  DateTime dateTime1 = DateTime.Now;
                  if (dictionaryValue != null)
                  {
                    num2 = dictionaryValue.NewObjectID;
                    if (dictionaryValue.Tag is TechDraftTag tag)
                      objectID = tag.Drafts.Values.FirstOrDefault<long>();
                  }
                  this._logger.Write($"Обработка документа Search (DocID={int32_2}, VersionID={int32_3}) -> IPS (ID={(num2 != 0L ? (object) num2.ToString() : (object) "?")}, ObjectID={(objectID != 0L ? (object) objectID.ToString() : (object) "?")})");
                  IDBObject dbObject = (IDBObject) null;
                  IDBAttribute dbAttribute = (IDBAttribute) null;
                  if (objectID != 0L)
                  {
                    dbObject = TechcardConsts.Plugin.Idw.GetUserSession().GetObject(objectID, false);
                    if (dbObject == null)
                    {
                      TechcardConsts.Plugin.appManager.AddWarningMessage($"Объект ({objectID}) не найден, невозможно проверить / обновить файл эскизов!");
                      this._logger.Write("Пропущено, объект в IPS не найден");
                      continue;
                    }
                    if (dbObject.CheckoutBy != 0L)
                    {
                      this._logger.Write("Пропущено, объект в IPS взят на изменение");
                      continue;
                    }
                    dbAttribute = dbObject.GetAttributeByID(PumpHelper.AttrTypeFileID);
                    if (dbAttribute == null)
                    {
                      this._logger.Write("Пропущено, объект в IPS не имеет файлов");
                      continue;
                    }
                    try
                    {
                      if (dbAttribute is IBlobReader blobReader)
                        dateTime1 = blobReader.OpenBlob(-1).ModifyDate;
                    }
                    catch (Exception ex)
                    {
                      this._logger.Write("Ошибка чтения файла: " + ex.Message);
                    }
                  }
                  string fileStoreAlias = PumpHelper.FileStoreAliases[int32_4];
                  FileStore fileStore = FileStores.FS[fileStoreAlias];
                  if (fileStore == null)
                  {
                    TechcardConsts.Plugin.appManager.AddErrorMessage($"Ошибка получения файла документа \"(DOC_ID={int32_2})\". Файловый шкаф \"{fileStoreAlias}\" не найден");
                  }
                  else
                  {
                    using (IDataReader reader = BasePumpHelper.S4Query(fileStore.Connection, $"SELECT FLNAME, OWNER, FILESIZE, FILEDATE, LINKTYPE, AUTHOR, FILEBODY {fileStore.AddColumns} FROM {fileStore.LinkedTableName} WHERE FILE_ID=@p1 and VERSION_ID=@p2", CommandBehavior.Default, (object) int32_2, (object) int32_3))
                    {
                      if (reader.Read())
                      {
                        string str2 = reader[0].ToString();
                        string objectCaption;
                        bool isBaseVersion;
                        Guid draftObjectType = service.GetDraftObjectType(int32_2, int32_3, str2, out objectCaption, out isBaseVersion);
                        if (draftObjectType.Equals(Guid.Empty))
                        {
                          this._logger.Write($"Файл \"{str2}\" не является чертежом");
                        }
                        else
                        {
                          int id = PumpHelper.TypeGuidToID(draftObjectType);
                          if (id == 0)
                          {
                            this._logger.Write($"Тип объекта \"{draftObjectType}\" не найден в настройках миграции");
                          }
                          else
                          {
                            int int32_5 = BasePumpHelper.ToInt32(reader[1]);
                            DateTime dateTime2 = Convert.ToDateTime((object) reader.GetDateTime(3));
                            if (dbObject != null)
                            {
                              if (dateTime1 == dateTime2.ToUniversalTime())
                              {
                                this._logger.Write($"Пропущено, файл эскиза не модифицировался после импорта {dateTime2}");
                                continue;
                              }
                              if (dateTime1 > dateTime2.ToUniversalTime())
                              {
                                this._logger.Write($"Пропущено, файл в IPS был модифицирован после даты изменения эскиза {dateTime2}");
                                continue;
                              }
                            }
                            string tempPath = BlobHelper.TempPath;
                            string nextFileName = BlobHelper.NextFileName;
                            fileStore.WriteFileBody(reader, nextFileName);
                            BlobHelper.UseFile(nextFileName);
                            string fileName = "";
                            if (int32_5 > 3)
                              fileName = CabHelper.ExtractCAB(nextFileName, tempPath);
                            if (fileName != "")
                            {
                              FileInfo fileInfo = new FileInfo(fileName);
                              System.IO.File.Delete(nextFileName);
                              string destFileName = nextFileName;
                              fileInfo.MoveTo(destFileName);
                            }
                            if (dbObject != null)
                            {
                              this.UpdateDraftData(dbObject, dbAttribute, nextFileName, reader);
                              this._logger.Write($"Обновлены данные эскиза (DocID={int32_2}, VersionID={int32_3}) -> IPS (ID={dbObject.ObjectID}, ObjectID={dbObject.ID})");
                            }
                            else
                            {
                              ObjectRecord objectRecord = impObjList.AddObject(id, 0, objectCaption);
                              DateTime universalTime = dateTime2.ToUniversalTime();
                              objectRecord.ObjCreate = universalTime;
                              objectRecord.ModifyDate = DateTime.Now;
                              objectRecord.IsBaseVersion = isBaseVersion;
                              int int32_6 = BasePumpHelper.ToInt32(reader[2]);
                              int int32_7 = BasePumpHelper.ToInt32(reader[4]);
                              int int32_8 = BasePumpHelper.ToInt32(reader[5]);
                              ArcMethods arcMethod = int32_5 < 1 || int32_5 > 3 ? ArcMethods.NotPacked : ArcMethods.ZLibPacked;
                              AttributeRecord attributeRecord = impObjList.AddAttributeBlob(PumpHelper.AttrTypeFileID, nextFileName, (long) int32_6, str2, arcMethod, 0);
                              attributeRecord.DateValue = (object) universalTime;
                              attributeRecord.FileType = (object) PumpDocumentsClass.LinkTypeToFileTypes(int32_7);
                              if (int32_8 != 0)
                                attributeRecord.FileAuthor = (object) BasePumpHelper.UsersCache.GetNewKey((object) int32_8);
                              impObjList.Items[impObjList.Items.Count - 1].Tag = (object) new TechcardDraftInfo(int32_2, int32_3, str2);
                              this._logger.Write($"Создан новый эскиз (DocID={int32_2}, VersionID={int32_3}) -> IPS (FileName = {str2} Guid = {objectRecord.ObjectGuid})");
                            }
                          }
                        }
                      }
                      else
                        this._logger.Write("Пропущено, доп. файл документа не найден");
                    }
                  }
                }
              }
            }
          }
          finally
          {
            dataReader.Close();
          }
          impObjList.Import();
          this.PumpCheckPoint("Перекачка эскизов успешно завершена", 100);
          this._logger.Write("=========Pump end\r\n\r\n");
        }
      }
      catch (Exception ex)
      {
        this._logger.Write($"=========Pump abort ({ex.Message}\r\n{ex.StackTrace})\r\n\r\n");
        throw;
      }
      finally
      {
        BlobHelper.Clear();
      }
    }
  }

  private void UpdateDraftData(
    IDBObject dbObject,
    IDBAttribute dbAttribute,
    string draftFile,
    IDataReader reader)
  {
    if (dbObject == null)
      throw new ArgumentNullException(nameof (dbObject));
    if (dbAttribute == null || string.IsNullOrEmpty(draftFile))
      return;
    bool flag = dbObject.ObjectModifyMode == ObjectModifyModes.Checkout;
    if (dbAttribute.ReadOnly & flag)
    {
      try
      {
        dbObject = dbObject.CheckOut();
      }
      catch (Exception ex)
      {
        this._logger.Write("Пропущено, ошибка взятия на изменение: " + ex.Message);
        return;
      }
      dbAttribute = dbObject.GetAttributeByID(PumpHelper.AttrTypeFileID);
    }
    try
    {
      if (!(dbAttribute is IBlobWriter blobWriter))
        return;
      string uniqueFileName = reader[0].ToString();
      int int32_1 = BasePumpHelper.ToInt32(reader[1]);
      int int32_2 = BasePumpHelper.ToInt32(reader[2]);
      DateTime dateTime = Convert.ToDateTime((object) reader.GetDateTime(3));
      int int32_3 = BasePumpHelper.ToInt32(reader[4]);
      int int32_4 = BasePumpHelper.ToInt32(reader[5]);
      long file_author = 0;
      if (int32_4 != 0)
        file_author = BasePumpHelper.UsersCache.GetNewKey((object) int32_4);
      ArcMethods arcMethod = int32_1 < 1 || int32_1 > 3 ? ArcMethods.NotPacked : ArcMethods.ZLibPacked;
      IFileNamesService service = ServiceUtils.GetService<IFileNamesService>((object) dbObject.Session, false);
      if (service != null)
        uniqueFileName = service.GetUniqueFileName(uniqueFileName, dbObject.ID, dbObject.Session.SessionGUID);
      BlobInformation blobInfo = new BlobInformation((long) int32_2, BlobHelper.FileSize, dateTime.ToUniversalTime(), uniqueFileName, arcMethod, "", PumpDocumentsClass.LinkTypeToFileTypes(int32_3), file_author);
      if (!blobWriter.OpenBlob(blobInfo, false))
        return;
      FileStream fileStream = new FileStream(draftFile, FileMode.Open);
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
    catch (Exception ex)
    {
      this._logger.Write("Ошибка записи файла: " + ex.Message);
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
          this._logger.Write("Ошибка возврата в архив: " + ex.Message);
        }
      }
    }
  }

  private void AfterImportObjectEvent(object sender, EventArgs e)
  {
    if (!(sender is IImportedObjectList importedObjectList))
      return;
    long key = 0;
    TechDraftTag tag1 = (TechDraftTag) null;
    for (int index = 0; index < importedObjectList.Items.Count; ++index)
    {
      ImportingObject importingObject = importedObjectList.Items[index];
      ObjectRecord objectRecord = importingObject.Object;
      if (importingObject.Tag is TechcardDraftInfo tag2)
      {
        if (objectRecord.Object_id == 0L)
        {
          Exception importError = importedObjectList.GetImportError(index);
          string str = $"Ошибка перекачки эскиза Техкард (DOC_ID={tag2.DocID}, VER_ID={tag2.VersionID}, доп. файл='{tag2.FileName}'): ";
          TechcardConsts.Plugin.appManager.AddWarningMessage(str + (importError != null ? importError.Message : "?"));
        }
        else
        {
          long num = BasePumpHelper.MakeCacheKey(tag2.DocID, tag2.VersionID);
          if (num != key && tag1 != null)
          {
            this.AddToDraftsCache(key, tag1);
            tag1 = (TechDraftTag) null;
          }
          if (tag1 == null)
            tag1 = new TechDraftTag();
          tag1.Drafts.Add(tag2.FileName, objectRecord.Object_id);
          key = num;
        }
      }
    }
    if (tag1 == null)
      return;
    this.AddToDraftsCache(key, tag1);
  }

  public DraftDwgDataPump(PluginClass plugin)
    : base(plugin)
  {
    if (plugin == null)
      throw new ArgumentNullException(nameof (plugin));
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
    this._logger = new SimpleLogger(Path.Combine(Application.StartupPath, "DraftDwgDataImport.log"));
  }

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    this._searchConnection = SearchConnectionsManager.GetConnection();
    if (this._searchConnection == null)
      return;
    if (PumpHelper.TechcardDocTypes.Count == 0)
    {
      TechcardConsts.Plugin.appManager.AddErrorMessage("Список типов документов Search не найден. Проверьте, загружен ли модуль Intermech.ImpExp.SearchData");
    }
    else
    {
      base.Exam();
      this.ExamCheckPoint("Проверка данных успешно завершена", 100);
    }
  }

  public override void Pump()
  {
    if (TechCache.SavePoint == null || TechCache.SavePoint.OperationTerminateType != TerminateType.Complete)
      this.PumpCheckPoint("Докачка эскизов в текущем режиме недоступна", 100);
    else if (this._searchConnection == null)
    {
      TechcardConsts.Plugin.appManager.AddErrorMessage($"Подключение к базе Search \"{"SEARCH PLUGIN CONNECTION"}\" не найдено ");
    }
    else
    {
      IImportedObjectList listWithStatistics = this.plugin.Idw.CreateImportedObjectListWithStatistics(this.GUID);
      listWithStatistics.AfterImportEvent += new AfterImportEventDelegate(this.AfterImportObjectEvent);
      this.PumpCheckPoint("Загрузка информации об импортированных ТП", 0);
      this.LoadImportedTechProcData();
      this._draftCache = PumpCache.Category[ImportingCategory.TechDrafts];
      try
      {
        this.PumpDraftData(listWithStatistics);
      }
      finally
      {
        this._draftCache.Release();
        this._draftCache = (CacheCategory) null;
      }
    }
  }
}

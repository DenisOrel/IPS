// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpDocumentsClass
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.Archives.Common;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Interface.Techcard;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для перекачки документов", "Перекачка документов")]
public class PumpDocumentsClass : PumpClass
{
  protected SearchDataPlugin plugin;
  protected readonly DateTime MinFileDateTime = new DateTime(1899, 12, 30);
  private CacheCategory _docTypes;
  private CacheCategory _arcInfos;
  private CacheCategory _arcParams;
  private CacheCategory _themeParams;
  private CacheCategory _docsCache;
  private CacheCategory _draftsCache;
  private CacheCategory _statusesToLevels;
  private CacheCategory _commonParameters;
  private CacheCategory _docLinksCache;
  private ITechCardTypeService _techService;
  private DocumentTParamsService _documentParamsService;
  private CommonParamsReader _documentCommonParamsReader;
  private IImportedObjectList _iol;
  private IImportedObjectList _tempIol;
  private FileStream _blobStream;
  private List<string> _sysFields = new List<string>((IEnumerable<string>) new string[1]
  {
    "doc_id"
  });
  private int _filesCounter;
  private DateTime _maxFileDate = PumpHelper.MinDBDateTime;
  private string _lastMainFileName = "";
  private Dictionary<int, HashSet<int>> _attributes2Exclude = new Dictionary<int, HashSet<int>>();
  private DocVerInfo _currentVersionToCache;

  internal static Guid PumperGUID => new Guid("{E7304DDB-D80F-400b-8303-30F48E8F2B22}");

  protected override Guid GUID => PumpDocumentsClass.PumperGUID;

  public PumpDocumentsClass(SearchDataPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  public override void Exam()
  {
    this.ExamCheckPoint("Проверка целостности данных таблицы DOCLIST ", 1);
    FileStores.MainDBConnection = this.plugin.idb.DbConnection;
    using (IDbCommand command = this.plugin.idb2.CreateCommand())
    {
      this.ExamCheckPoint("Проверка целостности данных таблицы RC ", 0);
      command.CommandText = "select rc.doc_id, rc.version_id, rc.doc_type, d.name, d.designatio from rc, doclist d where rc.doc_id > 0 and rc.doc_type not in (select doc_type from doctypes) and rc.doc_id = d.doc_id";
      IDataReader dataReader1 = command.ExecuteReader(CommandBehavior.SequentialAccess);
      try
      {
        while (dataReader1.Read())
        {
          int int32_1 = BasePumpHelper.ToInt32(dataReader1[0]);
          int int32_2 = BasePumpHelper.ToInt32(dataReader1[1]);
          int int32_3 = BasePumpHelper.ToInt32(dataReader1[2]);
          string str1 = dataReader1.IsDBNull(3) ? "" : dataReader1.GetString(3);
          string str2 = dataReader1.IsDBNull(4) ? "" : dataReader1.GetString(4);
          if (str1 != "")
          {
            if (str2 != "")
              str2 += " - ";
            str2 += str1;
          }
          if (str2 != "")
            str2 = $"\"{str2}\" ";
          this.plugin._settingsControl.AddInvalidObject(new InvalidObject("Версия документа " + str2, int32_1, int32_2, int32_3));
        }
      }
      finally
      {
        dataReader1.Close();
      }
      this.ExamCheckPoint("Поиск дубликатов версий документов", 90);
      command.CommandText = "select doc_id, version_id, count(*) from rc group by doc_id, version_id having count(*) > 1";
      IDataReader dataReader2 = command.ExecuteReader();
      try
      {
        int num = 0;
        while (dataReader2.Read())
        {
          ++num;
          this.plugin.appManager.AddErrorMessage($"Найден дубликат версии документа DOC_ID={BasePumpHelper.ToInt32(dataReader2[0])} VERSION_ID={BasePumpHelper.ToInt32(dataReader2[1])}");
        }
        if (num > 0)
        {
          if (MessageBox.Show($"В базе Search найдено {num} дубликатов версий документов. Рекомендуется до начала перекачки устранить дубликаты. Продолжить перекачку?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) != DialogResult.Yes)
            Application.Exit();
        }
      }
      finally
      {
        dataReader2.Close();
      }
    }
    this.ExamCheckPoint("Проверка данных успешно завершена", 100);
  }

  public override void Pump()
  {
    this.plugin.CheckVersionedTypes();
    SimpleLogger logger = BasePumpHelper.Logger;
    this._docTypes = PumpCache.Category[ImportingCategory.DocTypes];
    this._arcParams = PumpCache.Category[ImportingCategory.ArchiveParameters];
    this._themeParams = PumpCache.Category[ImportingCategory.ThematicParams];
    this._arcInfos = PumpCache.Category[ImportingCategory.Archives];
    this._docsCache = PumpCache.Category[ImportingCategory.Documents];
    this._draftsCache = PumpCache.Category[ImportingCategory.TechDrafts];
    this._docLinksCache = PumpCache.Category[ImportingCategory.DocLinksCache];
    this._statusesToLevels = PumpCache.Category[ImportingCategory.StatusesToLevels];
    this._techService = ServicesManager.GetService(typeof (ITechCardTypeService)) as ITechCardTypeService;
    this._commonParameters = PumpCache.Category[ImportingCategory.DocumentsCommonParameters];
    this._documentCommonParamsReader = new CommonParamsReader(this._commonParameters, "DOC_PARAMS", "DOC_ID");
    try
    {
      if (PluginSettings.OptimizeReadTParams)
      {
        this.PumpCheckPoint("Чтение тематических параметров для документов", 0);
        this._documentParamsService = new DocumentTParamsService(this.plugin.idb3.DbConnection, BasePumpHelper.Logger, this._docsCache, this._themeParams);
        this._documentParamsService.Read();
      }
      FileStores.MainDBConnection = this.plugin.idb.DbConnection;
      using (IDbCommand command = this.plugin.idb2.CreateCommand())
      {
        this.PumpCheckPoint("Определение количества документов для перекачки", 1);
        string str = "where d.doc_id > 0";
        command.CommandText = "select count(*) from doclist d " + str;
        int int32 = Convert.ToInt32(command.ExecuteScalar());
        this.SetCountPumpRecords(int32);
        logger.Write($"{command.CommandText}: {int32} result(s)");
        this.Iol.Items.Clear();
        command.CommandText = $"select d.* from doclist d {str} order by d.doc_id, d.version_id";
        IDataReader mainReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
        try
        {
          int index = 1;
          string format = "Перекачка документов ({0} из {1})";
          Document doc = new Document();
          DataReadResult dataReadResult;
          while ((dataReadResult = this.ReadDocument(mainReader, doc)) != DataReadResult.NoData)
          {
            this.PumpCheckPoint(string.Format(format, (object) index, (object) int32), this.CalculatePercent(int32, index, 2, 99));
            logger.Flush();
            if (dataReadResult == DataReadResult.OK)
              this.PumpDocument(doc);
            ++index;
          }
          this.CheckDataPacket(true);
          if (this.plugin.BlobThread == null)
            this.plugin.StartBlobThread(true);
        }
        finally
        {
          if (this.plugin.BlobThread != null)
            this.plugin.BlobThread.DocPumperStopTime = DateTime.Now;
          mainReader.Close();
          BlobHelper.Clear();
          if (this._blobStream != null)
          {
            this._blobStream.Close();
            this._blobStream = (FileStream) null;
          }
        }
        this.PumpCheckPoint("Перекачка документов успешно завершена", 100);
        logger.Write("=========Pump end\r\n\r\n");
      }
    }
    catch (Exception ex)
    {
      logger.Write($"=========Pump abort ({ex.Message}\r\n{ex.StackTrace})\r\n\r\n");
      throw;
    }
    finally
    {
      this._docTypes.Release();
      this._arcParams.Release();
      this._themeParams.Release();
      this._arcInfos.Release();
      this._docsCache.Release();
      this._draftsCache.Release();
      this._docLinksCache.Release();
      this._statusesToLevels.Release();
      this._commonParameters.Release();
    }
  }

  protected IImportedObjectList Iol
  {
    get
    {
      if (this._iol == null)
        this._iol = this.plugin.Idw.CreateImportedObjectList(0);
      return this._iol;
    }
  }

  protected IImportedObjectList TempIol
  {
    get
    {
      if (this._tempIol == null)
        this._tempIol = this.plugin.Idw.CreateImportedObjectList(0);
      return this._tempIol;
    }
  }

  private void AddToCache(int DocID, long NewDocID, string designation, ITagImportObject atag)
  {
    if (NewDocID != 0L)
      this._docsCache.AddValue((object) DocID, NewDocID, designation, atag);
    else
      BasePumpHelper.AppManager.AddWarningMessage($"Документ (DOC_ID={DocID}) не создан!");
  }

  private void AddToDraftsCache(long key, TechDraftTag tag)
  {
    try
    {
      this._draftsCache.AddValue((object) key, 1L, (ITagImportObject) tag);
    }
    catch
    {
    }
  }

  protected FileStream BlobStream
  {
    get
    {
      if (this._blobStream == null)
        this._blobStream = new FileStream(this.plugin.BlobsIndexFileName, FileMode.Append, FileAccess.Write, FileShare.Read);
      return this._blobStream;
    }
  }

  private void DoImport(IImportedObjectList _iol)
  {
    _iol.Import();
    int count = _iol.Items.Count;
    DocumentTag atag = (DocumentTag) null;
    long NewDocID = 0;
    int DocID = 0;
    string designation = (string) null;
    long key = 0;
    TechDraftTag tag = (TechDraftTag) null;
    for (int index = 0; index < count; ++index)
    {
      ImportingObject importingObject = _iol.Items[index];
      DocVerInfo docVerInfo = (DocVerInfo) null;
      TechcardDraftInfo techcardDraftInfo = (TechcardDraftInfo) null;
      if (importingObject.Tag is TechcardDraftInfo)
        techcardDraftInfo = importingObject.Tag as TechcardDraftInfo;
      else
        docVerInfo = importingObject.Tag as DocVerInfo;
      if (docVerInfo != null && docVerInfo.ID != 0 && atag != null)
      {
        this.AddToCache(DocID, NewDocID, designation, (ITagImportObject) atag);
        atag = (DocumentTag) null;
      }
      if (importingObject.Object.Object_id == 0L)
      {
        Exception importError = this.Iol.GetImportError(index);
        string str = "";
        if (techcardDraftInfo != null)
          str = $"Ошибка перекачки эскиза Техкард (DOC_ID={techcardDraftInfo.DocID}, VER_ID={techcardDraftInfo.VersionID}, доп. файл='{techcardDraftInfo.FileName}'): ";
        if (docVerInfo != null)
          str = $"Ошибка перекачки документа (DOC_ID={(docVerInfo.ID != 0 ? docVerInfo.ID : DocID)}, VER_ID={docVerInfo.VerID}): ";
        BasePumpHelper.AppManager.AddWarningMessage(str + (importError != null ? importError.Message : "?"));
      }
      else
      {
        long objectId = importingObject.Object.Object_id;
        if (techcardDraftInfo != null)
        {
          long num = BasePumpHelper.MakeCacheKey(techcardDraftInfo.DocID, techcardDraftInfo.VersionID);
          if (num != key && tag != null)
          {
            this.AddToDraftsCache(key, tag);
            tag = (TechDraftTag) null;
          }
          if (tag == null)
            tag = new TechDraftTag();
          tag.Drafts.Add(techcardDraftInfo.FileName, objectId);
          key = num;
        }
        else
        {
          if (docVerInfo.ID != 0 && atag == null)
          {
            DocumentTag documentTag = new DocumentTag();
            documentTag.ID = docVerInfo.ID;
            documentTag.LCStep = docVerInfo.LCStep;
            documentTag.Flags = docVerInfo.Flags;
            documentTag.VersionID = docVerInfo.ActualVerID;
            atag = documentTag;
            NewDocID = importingObject.Object.Id;
            DocID = docVerInfo.ID;
            designation = docVerInfo.Designation;
          }
          atag.Versions.Add(docVerInfo.VerID, objectId);
          atag.AddVersionInfo.Add(docVerInfo.VerID, new AddVersionInfo(docVerInfo.AdvanFilesDate, docVerInfo.FileDate, docVerInfo.FileSize, docVerInfo.ContentModifiedDate, docVerInfo.Blobs != null ? docVerInfo.Blobs.Count : 0));
        }
        Dictionary<long, BlobInformation4Import> dictionary = (Dictionary<long, BlobInformation4Import>) null;
        if (docVerInfo != null)
          dictionary = docVerInfo.Blobs;
        else if (techcardDraftInfo != null)
          dictionary = techcardDraftInfo.Blobs;
        if (dictionary != null)
        {
          foreach (KeyValuePair<long, BlobInformation4Import> keyValuePair in dictionary)
          {
            keyValuePair.Value.ObjectID = objectId;
            using (MemoryStream memoryStream = new MemoryStream())
            {
              new BinaryWriter((Stream) memoryStream).Write(keyValuePair.Key);
              this.plugin.Formatter.Serialize((Stream) memoryStream, (object) keyValuePair.Value);
              memoryStream.Position = 0L;
              memoryStream.CopyTo((Stream) this.BlobStream);
            }
            this.BlobStream.Flush();
            this.plugin.StartBlobThread();
          }
        }
      }
    }
    if (atag != null)
      this.AddToCache(DocID, NewDocID, designation, (ITagImportObject) atag);
    if (tag != null)
      this.AddToDraftsCache(key, tag);
    _iol.Items.Clear();
    BlobHelper.Reset();
  }

  private void CheckDataPacket(bool ForcePump)
  {
    if (!ForcePump && this.Iol.Items.Count < BasePumpHelper.PacketSize)
      return;
    this.DoImport(this.Iol);
  }

  private void CheckDataPacket() => this.CheckDataPacket(false);

  private DataReadResult ReadDocument(IDataReader mainReader, Document doc)
  {
    doc.Clear();
    bool flag = mainReader.Read();
    if (flag)
    {
      BasePumpHelper.ReaderRowToS4Table(mainReader, doc.Data, true);
      if (this._docsCache.GetNewKey((object) doc.ID) > 0L)
        return DataReadResult.Skipped;
      BasePumpHelper.Logger.Write($"Read document ID={doc.ID} (T={doc.TypeID})", true);
      using (IDataReader reader = BasePumpHelper.S4Query($"select d.*, dc.dir_name as workpath from RC d, dc where DOC_ID = @p1 and d.wrk_dir_id = dc.dirkey_id order by d.base_version_id{(BasePumpHelper.dbType == BasePumpHelper.DBType.Oracle ? " NULLS FIRST" : string.Empty)}, d.version_id", (object) doc.ID))
      {
        while (reader.Read())
        {
          object obj = reader["VERSION_ID"];
          int int32 = obj == DBNull.Value || obj == null ? 0 : BasePumpHelper.ToInt32(reader["VERSION_ID"]);
          S4Table tab = new S4Table();
          BasePumpHelper.ReaderRowToS4Table(reader, tab, this._sysFields, false);
          doc.RC[int32] = tab;
        }
      }
      int int32_1 = BasePumpHelper.ToInt32(doc.Data["archive_id"]);
      DictionaryValue dictionaryValue1 = this._arcInfos.GetValue((object) int32_1);
      if (dictionaryValue1 != null)
      {
        string str = "";
        if (dictionaryValue1.Tag is Archive)
          str = ((Archive) dictionaryValue1.Tag).Alias;
        using (IDataReader reader = BasePumpHelper.S4Query($"select * from {str} where DOC_ID = @p1", (object) doc.ID))
        {
          if (reader.Read())
            BasePumpHelper.ReaderRowToS4Table(reader, doc.AddData, this._sysFields, true);
        }
      }
      else
        this.plugin.appManager.AddWarningMessage($"Не найден архив с ид. {int32_1} указанный в документе {doc.ID} (тип={doc.TypeID})");
      if (PluginSettings.OptimizeReadTParams)
      {
        List<TParamValue> tparamValueList = this._documentParamsService.GetParams(doc.ID);
        if (tparamValueList != null && tparamValueList.Count > 0)
        {
          foreach (TParamValue tparamValue in tparamValueList)
            doc.ThemeData.Add(tparamValue.ParameterID.ToString(), tparamValue.Value);
        }
        this._documentParamsService.ClearValues(doc.ID);
      }
      else
      {
        HashSet<int> intSet = new HashSet<int>();
        using (IDataReader dataReader = BasePumpHelper.S4Query("select PARAM_ID from PARAM4DOC where DOC_ID = @p1", (object) doc.ID))
        {
          while (dataReader.Read())
            intSet.Add(BasePumpHelper.ToInt32(dataReader[0]));
        }
        foreach (int oldKey in intSet)
        {
          DictionaryValue dictionaryValue2 = this._themeParams.GetValue((object) oldKey);
          if (dictionaryValue2 != null)
          {
            string[] strArray = dictionaryValue2.Caption.Split(',');
            string str = strArray.Length > 1 ? strArray[1] : "";
            if (str != "")
            {
              using (IDataReader dataReader = BasePumpHelper.S4Query($"select P_VALUE from {str} where DOC_ID = @p1", (object) doc.ID))
              {
                if (dataReader.Read())
                  doc.ThemeData.Add(oldKey.ToString(), dataReader[0]);
              }
            }
          }
        }
      }
      this._documentCommonParamsReader.Read(doc.CommonParamsData, doc.ID);
    }
    return flag ? DataReadResult.OK : DataReadResult.NoData;
  }

  private void AddFileAttribute(
    FileStore fs,
    IDataReader reader,
    IImportedObjectList writer,
    bool IsMainFile,
    bool IsTechcard,
    bool IsProe,
    Dictionary<string, int> maxFileVersions,
    int DocID,
    int VersionID)
  {
    bool flag = DocID > 0;
    DocID = Math.Abs(DocID);
    string str1 = reader[0].ToString();
    if (IsMainFile)
      this._lastMainFileName = str1;
    else if (IsProe)
    {
      if (str1.StartsWith(this._lastMainFileName, StringComparison.CurrentCultureIgnoreCase))
      {
        if (int.TryParse(str1.Substring(this._lastMainFileName.Length).TrimStart('.').Trim(), out int _))
          return;
      }
      if (maxFileVersions != null)
      {
        string fn = str1;
        int fileVersionNumber = PumpHelper.ExtractFileVersionNumber(ref fn);
        if (fileVersionNumber > 0)
        {
          int num;
          if (maxFileVersions.TryGetValue(fn.ToLower(), out num) && fileVersionNumber != num)
            return;
          str1 = fn;
        }
      }
    }
    int int32_1 = BasePumpHelper.ToInt32(reader[1]);
    int int32_2 = BasePumpHelper.ToInt32(reader[2]);
    object fldvalue = (object) null;
    try
    {
      fldvalue = (object) reader.GetDateTime(3);
    }
    catch (Exception ex)
    {
      BasePumpHelper.Logger.Write($"* AddFileAttribute error in reader.GetDateTime ({ex.Message}\r\n{BasePumpHelper.LastS4Query.CommandText})\r\n");
    }
    DateTime dateTime = PumpHelper.MinDBDateTime;
    try
    {
      dateTime = Convert.ToDateTime(fldvalue);
    }
    catch
    {
    }
    BasePumpHelper.FixDateTimeField(ref fldvalue);
    if (IsMainFile & flag)
    {
      this._currentVersionToCache.FileDate = dateTime;
      this._currentVersionToCache.FileSize = int32_2;
    }
    int int32_3 = BasePumpHelper.ToInt32(reader[4]);
    int int32_4 = BasePumpHelper.ToInt32(reader[5]);
    ObjectRecord objectRecord = (ObjectRecord) null;
    if (flag)
    {
      string upper = str1.ToUpper();
      if (!upper.EndsWith(".RLF") && !upper.EndsWith(".RLF2") && int32_3 == 0)
        this._currentVersionToCache.AdvanFilesDate += Convert.ToInt64(dateTime.ToOADate() * 1000000.0);
      if (fldvalue != null && !DBNull.Value.Equals(fldvalue) && (DateTime) fldvalue > this._maxFileDate)
        this._maxFileDate = (DateTime) fldvalue;
      if (IsTechcard)
      {
        if (IsMainFile)
          return;
        string objectCaption = "";
        bool isBaseVersion = true;
        Guid draftObjectType = this._techService.GetDraftObjectType(DocID, VersionID, str1, out objectCaption, out isBaseVersion);
        if (!draftObjectType.Equals(Guid.Empty))
        {
          int id = PumpHelper.TypeGuidToID(draftObjectType);
          if (id != 0)
          {
            writer = this.TempIol;
            objectRecord = writer.AddObject(id, 0, objectCaption);
            DateTime universalTime = dateTime.ToUniversalTime();
            objectRecord.ObjCreate = universalTime;
            objectRecord.ModifyDate = universalTime;
            objectRecord.IsBaseVersion = isBaseVersion;
          }
        }
      }
    }
    else if (IsTechcard)
      return;
    string str2;
    string str3;
    if (this.plugin.PumpBlobsInParallel)
    {
      str2 = this.plugin.BlobsPath + $"{DocID % 256 /*0x0100*/:00}\\";
      if (!Directory.Exists(str2))
        Directory.CreateDirectory(str2);
      str3 = str2 + $"{DocID}_{VersionID}_{this._filesCounter}.dat";
    }
    else
    {
      str2 = BlobHelper.TempPath;
      str3 = BlobHelper.NextFileName;
    }
    fs.WriteFileBody(reader, str3);
    BlobHelper.UseFile(str3);
    ArcMethods arcMethod = int32_1 < 1 || int32_1 > 3 ? ArcMethods.NotPacked : ArcMethods.ZLibPacked;
    string fileName = "";
    if (int32_1 > 3)
      fileName = CabHelper.ExtractCAB(str3, str2);
    if (fileName != "")
    {
      FileInfo fileInfo = new FileInfo(fileName);
      File.Delete(str3);
      string destFileName = str3;
      fileInfo.MoveTo(destFileName);
    }
    long key = 0;
    BlobInformation4Import information4Import = (BlobInformation4Import) null;
    if (!this.plugin.PumpBlobsInParallel)
    {
      if (!flag)
        throw new NotImplementedException("Старый код не умеет перекачивать рабочие копии");
      AttributeRecord attributeRecord = writer.AddAttributeBlob(PumpHelper.AttrTypeFileID, str3, (long) int32_2, str1, arcMethod, objectRecord == null ? this._filesCounter : 0);
      attributeRecord.DateValue = fldvalue;
      attributeRecord.FileType = (object) PumpDocumentsClass.LinkTypeToFileTypes(int32_3);
      if (int32_4 != 0)
        attributeRecord.FileAuthor = (object) BasePumpHelper.UsersCache.GetNewKey((object) int32_4);
    }
    else
    {
      long file_author = 0;
      if (int32_4 != 0)
        file_author = BasePumpHelper.UsersCache.GetNewKey((object) int32_4);
      DateTime modifyDate = fldvalue != null ? (DateTime) fldvalue : this.MinFileDateTime;
      int attributeID = flag ? PumpHelper.AttrTypeFileID : PumpHelper.AttrWorkFileID;
      information4Import = new BlobInformation4Import((long) int32_2, BlobHelper.FileSize, modifyDate, str1, arcMethod, "", PumpDocumentsClass.LinkTypeToFileTypes(int32_3), file_author, attributeID, 0L, str3);
      key = ((long) DocID << 32 /*0x20*/) + (long) (VersionID * 100000) + (long) this._filesCounter;
      if (objectRecord == null)
      {
        if (this._currentVersionToCache.Blobs == null)
          this._currentVersionToCache.Blobs = new Dictionary<long, BlobInformation4Import>();
        this._currentVersionToCache.Blobs.Add(key, information4Import);
      }
    }
    ++this._filesCounter;
    if (objectRecord == null)
      return;
    ImportingObject importingObject = writer.Items[writer.Items.Count - 1];
    TechcardDraftInfo techcardDraftInfo1 = new TechcardDraftInfo(DocID, VersionID, str1);
    if (this.plugin.PumpBlobsInParallel)
    {
      techcardDraftInfo1.Blobs = new Dictionary<long, BlobInformation4Import>();
      techcardDraftInfo1.Blobs.Add(key, information4Import);
    }
    TechcardDraftInfo techcardDraftInfo2 = techcardDraftInfo1;
    importingObject.Tag = (object) techcardDraftInfo2;
  }

  private HashSet<int> GetAttributes2Exclude(int typeID)
  {
    HashSet<int> attributes2Exclude = (HashSet<int>) null;
    if (!this._attributes2Exclude.TryGetValue(typeID, out attributes2Exclude))
    {
      attributes2Exclude = new HashSet<int>();
      if (this._techService != null)
      {
        foreach (int num in this._techService.GetAttributes2Exclude(typeID))
          attributes2Exclude.Add(num);
      }
      this._attributes2Exclude.Add(typeID, attributes2Exclude);
    }
    return attributes2Exclude;
  }

  private Guid GetGuid(int id)
  {
    using (IDataReader dataReader = BasePumpHelper.S4Query("select f_guid from GUIDS_DOC t where t.DOC_ID=@p1", (object) id))
    {
      if (dataReader.Read())
      {
        if (!dataReader.IsDBNull(0))
          return new Guid(Convert.ToString(dataReader[0]));
      }
    }
    Guid guid = this.plugin.Imdi.NewPumpGuid();
    BasePumpHelper.S4NonQuery("INSERT INTO GUIDS_DOC (DOC_ID, F_GUID) VALUES(@p1, @p2)", (object) id, (object) guid.ToString("B").ToUpper());
    return guid;
  }

  private void PumpDocument(Document doc)
  {
    IImportedObjectList iol = this.Iol;
    this.CheckDataPacket();
    BasePumpHelper.Logger.Write($"Pump document ID={doc.ID} (T={doc.TypeID})", true);
    string caption = !(doc.Designation == "") ? (!(doc.Name == "") ? $"{doc.Designation} ({doc.Name})" : doc.Designation) : doc.Name;
    Guid guid1 = this.GetGuid(doc.ID);
    int int32_1 = BasePumpHelper.ToInt32(doc.Data["archive_id"]);
    int int32_2 = BasePumpHelper.ToInt32(doc.Data["arc_dir_id"]);
    object attrVal1 = doc.Data["format"];
    bool flag1 = BasePumpHelper.ToInt32(doc.Data["arc_dir_id"]) == 0;
    object obj1 = (object) null;
    DateTime dateTime1 = PumpHelper.MinDBDateTime;
    if (doc.Data.TryGetValue("modifdate", out obj1) && !DBNull.Value.Equals(obj1))
      dateTime1 = AttributesHelper.CorrectDbDateTimeValue(Convert.ToDateTime(obj1).ToUniversalTime());
    if (dateTime1 < PumpHelper.MinDBDateTime)
      dateTime1 = PumpHelper.MinDBDateTime;
    int int32_3 = BasePumpHelper.ToInt32(doc.Data["version_id"]);
    bool flag2 = true;
    int num1 = 0;
    if (doc.Data.TryGetValue("invisible", out object _))
      num1 = BasePumpHelper.ToInt32(doc.Data["invisible"]);
    foreach (KeyValuePair<int, S4Table> keyValuePair1 in doc.RC)
    {
      int key = keyValuePair1.Key;
      S4Table s4Table = keyValuePair1.Value;
      int int32_4 = BasePumpHelper.ToInt32(s4Table["doc_type"]);
      int num2 = 0;
      Guid empty = Guid.Empty;
      bool IsTechcard = false;
      if (PumpHelper.IsTechcardDocument(int32_4))
      {
        Guid tpObjectType = this._techService.GetTPObjectType(PumpHelper.TechcardDocTypes[int32_4]);
        if (!Guid.Empty.Equals(tpObjectType))
        {
          num2 = PumpHelper.TypeGuidToID(tpObjectType);
          IsTechcard = num2 != 0;
        }
      }
      if (num2 == 0)
        num2 = Convert.ToInt32(this._docTypes.GetNewKey((object) int32_4));
      if (num2 == 0)
      {
        BasePumpHelper.AppManager.AddWarningMessage($"Тип документа, соответствующий типу ({int32_4}), в новой базе не найден! Версия документа (DOC_ID={doc.ID}, VER_ID={key}) не может быть закачана.");
      }
      else
      {
        ObjectRecord objRec = iol.AddObject(num2, 0, caption);
        objRec.IdGuid = (object) guid1;
        string g = Convert.ToString(s4Table["doc_guid"]);
        if (!string.IsNullOrEmpty(g))
        {
          objRec.ObjectGuid = (object) new Guid(g);
        }
        else
        {
          Guid guid2 = this.plugin.Imdi.NewPumpGuid();
          BasePumpHelper.S4NonQuery("UPDATE RC SET DOC_GUID=@p1 WHERE REC_ID=@p2", (object) guid2.ToString("B").ToUpper(), (object) Convert.ToInt64(s4Table["rec_id"]));
          objRec.ObjectGuid = (object) guid2;
        }
        objRec.AccessLevel = num1;
        int num3 = PumpHelper.SetUpLCStep(objRec, int32_1, doc.ID, key, this._statusesToLevels);
        this._currentVersionToCache = (DocVerInfo) null;
        if (flag2)
        {
          string designation = PumpHelper.IsECO(int32_4) ? doc.Designation : "";
          this._currentVersionToCache = new DocVerInfo(doc.ID, key, designation, int32_3)
          {
            LCStep = num3
          };
          if (IsTechcard)
            this._currentVersionToCache.Flags |= DocumentFlag.Techcard;
          flag2 = false;
        }
        else
          this._currentVersionToCache = new DocVerInfo(0, key);
        iol.Items[iol.Items.Count - 1].Tag = (object) this._currentVersionToCache;
        objRec.VersionId = key;
        objRec.IsBaseVersion = key == int32_3;
        if (!DBNull.Value.Equals(s4Table["base_version_id"]))
        {
          int int32_5 = BasePumpHelper.ToInt32(s4Table["base_version_id"]);
          objRec.ParentVersionNo = int32_5;
        }
        iol.AddAttributeStr(PumpHelper.AttrTypeDesignationID, doc.Designation);
        iol.AddAttributeStr(PumpHelper.AttrTypeNameID, doc.Name);
        if (PluginSettings.AddDocID)
        {
          iol.AddAttributeInt(BasePumpHelper.AttrSearchID, (long) doc.ID);
          iol.AddAttributeInt(BasePumpHelper.AttrSearchVersionID, (long) key);
        }
        DictionaryValue dictionaryValue1 = this._arcInfos.GetValue((object) int32_1);
        Archive tag = dictionaryValue1 != null ? dictionaryValue1.Tag as Archive : (Archive) null;
        iol.AddAttributeLink(PumpHelper.AttrArchiveID, dictionaryValue1.NewObjectID, tag.Descriptio);
        object attrVal2 = s4Table["workpath"];
        int attrType = PumpHelper.AttrDocWorkPath;
        if (flag1)
          attrType = PumpHelper.AttrPaperDocPath;
        iol.AddAttribute(attrType, AttrValueType.stringVal, attrVal2, 0);
        iol.AddAttribute(PumpHelper.AttrFormatID, AttrValueType.stringVal, attrVal1, 0);
        object obj2 = s4Table["designerid"];
        long num4 = 0;
        if (!DBNull.Value.Equals(obj2) && BasePumpHelper.ToInt32(obj2) != -1)
          num4 = BasePumpHelper.UsersCache.GetNewKey((object) BasePumpHelper.ToInt32(obj2));
        if (num4 != 0L)
          objRec.OwnerId = num4;
        iol.AddAttributeStr(PumpHelper.AttrTypeNoteID, doc.Data["note"].ToString());
        DateTime dateTime2 = PumpHelper.MinDBDateTime;
        object obj3 = s4Table["birthday"];
        if (!DBNull.Value.Equals(obj3))
          dateTime2 = AttributesHelper.CorrectDbDateTimeValue(Convert.ToDateTime(obj3).ToUniversalTime());
        if (dateTime2 < PumpHelper.MinDBDateTime)
          dateTime2 = PumpHelper.MinDBDateTime;
        objRec.ObjCreate = dateTime2;
        objRec.ModifyDate = dateTime1;
        if (s4Table.AsInteger("otd_status", 0) != 0)
        {
          iol.AddAttribute(ConstsHolder.OTDRegisteredDateID, AttrValueType.datetimeVal, PumpHelper.ToDateTime(s4Table["otd_reg"]), 0);
          int oldKey = s4Table.AsInteger("otd_reg_user", 0);
          if (oldKey != 0)
          {
            DictionaryValue dictionaryValue2 = BasePumpHelper.UsersCache.GetValue((object) oldKey);
            if (dictionaryValue2 != null)
              iol.AddAttributeLink(ConstsHolder.OTDRegistratorID, dictionaryValue2.NewObjectID, dictionaryValue2.Caption);
          }
          if (!DBNull.Value.Equals(s4Table["otdregnum"]))
          {
            string str = s4Table["otdregnum"].ToString();
            if (str != "")
              iol.AddAttributeStr(ConstsHolder.InventoryNumberID, str);
          }
          if (!DBNull.Value.Equals(s4Table["prevotdregnum"]))
          {
            string str = s4Table["prevotdregnum"].ToString();
            if (str != "")
              iol.AddAttributeStr(ConstsHolder.PreviousInventoryNumberID, str);
          }
          if (!DBNull.Value.Equals(s4Table["dublotdregnum"]))
          {
            string str = s4Table["dublotdregnum"].ToString();
            if (str != "")
              iol.AddAttributeStr(ConstsHolder.NewInventoryNumberID, str);
          }
        }
        iol.AddAttribute(PumpHelper.AttrChangeNoID, AttrValueType.stringVal, s4Table["ver_code"], 0);
        int num5 = BasePumpHelper.ToInt32(s4Table["revdoc_id"]);
        if (num5 == doc.ID)
          num5 = 0;
        if (PumpHelper.IsECO(int32_4))
        {
          string str = PumpHelper.ConvertECOReason(BasePumpHelper.ToInt32(s4Table["reasoncode"]));
          if (str == null)
            BasePumpHelper.AppManager.AddWarningMessage($"Невозможно перекачать причину выпуска извещения (DOC_ID={doc.ID}, VERSION_ID={key}). Причины изменений ИИ не синхронизированы!");
          else
            iol.AddAttributeStr(PumpHelper.AttrReasonCodeID, str);
          this.AddDocumentDateProperty(iol, PumpHelper.AttrEndDateID, s4Table["enddate"]);
          this.AddDocumentDateProperty(iol, PumpHelper.AttrDateOfReleaseID, s4Table["chkindate"]);
          this.AddDocumentDateProperty(iol, PumpHelper.AttrTermOfChangeID, s4Table["termofchg"]);
        }
        else
        {
          object dateTime3 = PumpHelper.ToDateTime(s4Table["enddate"]);
          if (num5 != 0)
          {
            if (dateTime3 != DBNull.Value)
              iol.AddAttributeDate(PumpHelper.AttrECO_DateDueID, (DateTime) dateTime3);
            long oldKey = Convert.ToInt64(doc.ID) << 32 /*0x20*/ | (long) (uint) key;
            if (this._docLinksCache.GetNewKey((object) oldKey) == 0L)
              this._docLinksCache.AddValue((object) oldKey, Convert.ToInt64(num5));
          }
          else if (dateTime3 != DBNull.Value)
            iol.AddAttributeDate(PumpHelper.AttrEndDateID, (DateTime) dateTime3);
          this.AddDocumentDateProperty(iol, PumpHelper.AttrDateOfReleaseID, s4Table["chkindate"]);
          this.AddDocumentDateProperty(iol, PumpHelper.AttrTermOfChangeID, s4Table["termofchg"]);
        }
        HashSet<int> intSet = (HashSet<int>) null;
        if (PumpHelper.IsTechProcess(num2))
          intSet = this.GetAttributes2Exclude(num2);
        foreach (KeyValuePair<string, object> keyValuePair2 in (Dictionary<string, object>) doc.AddData)
        {
          int newKey = (int) this._arcParams.GetNewKey((object) $"{int32_1}.{keyValuePair2.Key}");
          if (newKey > 0 && (intSet == null || !intSet.Contains(newKey)))
            PumpHelper.AddAttribute(iol, newKey, keyValuePair2.Value);
        }
        foreach (KeyValuePair<string, object> keyValuePair3 in (Dictionary<string, object>) doc.ThemeData)
        {
          int newKey = (int) this._themeParams.GetNewKey((object) BasePumpHelper.ToInt32((object) keyValuePair3.Key));
          if (newKey > 0 && (intSet == null || !intSet.Contains(newKey)))
            PumpHelper.AddAttribute(iol, newKey, keyValuePair3.Value);
        }
        foreach (KeyValuePair<string, object> keyValuePair4 in (Dictionary<string, object>) doc.CommonParamsData)
          PumpHelper.AddAttribute(iol, Convert.ToInt32(keyValuePair4.Key), keyValuePair4.Value);
        this._maxFileDate = PumpHelper.MinDBDateTime;
        if (int32_2 != 0)
        {
          this._filesCounter = 0;
          this.TempIol.Items.Clear();
          string fileStoreAlias = PumpHelper.FileStoreAliases[int32_2];
          FileStore fs = FileStores.FS[fileStoreAlias];
          if (fs == null)
          {
            BasePumpHelper.AppManager.AddWarningMessage($"Ошибка получения файла документа \"(DOC_ID={doc.ID})\". Файловый шкаф \"{fileStoreAlias}\" не найден");
          }
          else
          {
            bool IsProe = PumpHelper.IsProeDocument(int32_4);
            List<int> intList = new List<int>();
            intList.Add(doc.ID);
            if (BasePumpHelper.ToInt32(s4Table["ver_status"]) != 0)
              intList.Add(-doc.ID);
            foreach (int DocID in intList)
            {
              using (IDataReader reader = BasePumpHelper.S4Query(fs.Connection, $"select FLNAME, OWNER, FILESIZE, FILEDATE, 0 as LINKTYPE, 0 as AUTHOR, FILEBODY {fs.AddColumns} from {fs.TableName} where FILE_ID=@p1 and VERSION_ID=@p2", CommandBehavior.SequentialAccess, (object) DocID, (object) key))
              {
                if (reader.Read())
                  this.AddFileAttribute(fs, reader, iol, true, IsTechcard, IsProe, (Dictionary<string, int>) null, DocID, key);
              }
              Dictionary<string, int> maxFileVersions = new Dictionary<string, int>();
              if (IsProe)
              {
                using (IDataReader dataReader = BasePumpHelper.S4Query(fs.Connection, $"select FLNAME from {fs.LinkedTableName} where FILE_ID=@p1 and VERSION_ID=@p2 order by FLNAME", CommandBehavior.SequentialAccess, (object) DocID, (object) key))
                {
                  while (dataReader.Read())
                  {
                    string lower = dataReader[0].ToString().ToLower();
                    int fileVersionNumber = PumpHelper.ExtractFileVersionNumber(ref lower);
                    if (fileVersionNumber > 0)
                    {
                      int num6 = -1;
                      if (maxFileVersions.TryGetValue(lower, out num6))
                      {
                        if (fileVersionNumber > num6)
                          maxFileVersions[lower] = fileVersionNumber;
                      }
                      else
                        maxFileVersions.Add(lower, fileVersionNumber);
                    }
                  }
                }
              }
              string str = PumpHelper.IsS4LinkedAuthorExists ? "AUTHOR, " : "0 as AUTHOR, ";
              using (IDataReader reader = BasePumpHelper.S4Query(fs.Connection, $"select FLNAME, OWNER, FILESIZE, FILEDATE, LINKTYPE, {str}FILEBODY {fs.AddColumns} from {fs.LinkedTableName} where FILE_ID=@p1 and VERSION_ID=@p2", CommandBehavior.SequentialAccess, (object) DocID, (object) key))
              {
                while (reader.Read())
                  this.AddFileAttribute(fs, reader, iol, false, IsTechcard, IsProe, maxFileVersions, DocID, key);
              }
            }
          }
        }
        if (this._maxFileDate < PumpHelper.MinDBDateTime)
          this._maxFileDate = PumpHelper.MinDBDateTime;
        this._currentVersionToCache.ContentModifiedDate = this._maxFileDate;
        iol.AddAttributeDate(PumpHelper.AttrContentModifiedDate, this._maxFileDate);
        if (!IsTechcard)
          AttributesHelper.AddObligatoryObjectAttributes(BasePumpHelper.Session, iol);
        else if (this._tempIol != null && this._tempIol.Items.Count > 0)
        {
          for (int index = 0; index < this._tempIol.Items.Count; ++index)
          {
            ImportingObject io = this._tempIol.Items[index];
            iol.Items.Add(io);
          }
          this.TempIol.Items.Clear();
        }
      }
    }
  }

  private void AddDocumentDateProperty(IImportedObjectList writer, int attributeID, object date)
  {
    object dateTime = PumpHelper.ToDateTime(date);
    if (dateTime == DBNull.Value)
      writer.AddAttributeNull(attributeID);
    else
      writer.AddAttributeDate(attributeID, (DateTime) dateTime);
  }

  public static FileTypes LinkTypeToFileTypes(int linkType)
  {
    return linkType == -1 ? FileTypes.ftRedlining : (FileTypes) linkType;
  }

  public static class PackFlag
  {
    public const int NotPacked = 0;
    public const int MinZLIBMethodID = 1;
    public const int MaxZLIBMethodID = 3;
  }
}

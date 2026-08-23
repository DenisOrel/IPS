// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetProcessor
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Checksums;
using Intermech.Client.Core;
using Intermech.Controls;
using Intermech.Document.Model;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Document;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Search.Interfaces;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>
/// Класс, выполняющий обработку УЛ в соответствии с выставленными параметрами (как непосредственно через properties, так и через визуальный контрол)
/// </summary>
public class CertSheetProcessor : ISaveToDiskProcessor
{
  /// <summary>
  /// идентификатор объекта-бланка удостоверяющих листов.
  /// -1, если не найден
  /// </summary>
  private long certSheetBlankObjectId = CertSheetProcessor.GetCertSheetBlankId();
  public static readonly Guid FileSizeSystemAttributeGuid = new Guid("cadd9973-306c-11d8-b4e9-00304f19f545");
  public static readonly Guid FileDateSystemAttributeGuid = new Guid("cadd9974-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// Атрибут (guid) для заполнения графы 9
  /// Guid.Empty, если пуст
  /// </summary>
  private Guid certSheetG09AttributeGuid = Guid.Empty;
  /// <summary>
  /// Атрибут для заполнения графы 9
  /// 0, если пуст
  /// </summary>
  private int certSheetG09AttributeId;
  /// <summary>
  /// Атрибут (guid) для заполнения графы 10
  /// Guid.Empty, если пуст
  /// </summary>
  private Guid certSheetG10AttributeGuid = Guid.Empty;
  /// <summary>
  /// Атрибут для заполнения графы 10
  /// 0, если пуст
  /// </summary>
  private int certSheetG10AttributeId;
  /// <summary>
  /// Имя общей папки для сохранения удостоверяющих листов при выполнении команды "Сохранить на диск"
  /// </summary>
  private string certSheetCommonFolderName = CertSheetProcessor.GetCertSheetCommonFolderName();
  /// <summary>Вывод только актуальных подписей</summary>
  private bool actualSignsOnly = CertSheetProcessor.GetActualSignsOnly();
  /// <summary>
  /// тихий режим, не спрашивать у пользователя в спорных случаях, действия по умолчанию
  /// </summary>
  private bool silentMode;
  private ExpiredAuthFileUsing expiredAuthFileUsing;
  /// <summary>Опции из контрола настройки получения УЛ</summary>
  private CertSheetOptions certSheetOptions;
  private CertSheetGraphSortMethod certSheetGraphSortMethod;
  /// <summary>
  /// все графы для подписей в системе парами object[]{string id, string description}
  /// </summary>
  private List<object[]> graphList = CertSheetProcessor.GetGraphs();
  /// <summary>
  /// Максимальное количество документов, при которых допускается чтение граф для подписей по отдельным документам
  /// </summary>
  public static int MaxDocsForSeparateGraphsRead = 10;
  /// <summary>кэш</summary>
  private static List<string> allExtensionsList = (List<string>) null;

  private static bool GetActualSignsOnly()
  {
    return (ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations).ReadBool("CLIENT", "CERTSHEETS", "ACTUALSIGNSONLY", CertSheetHolder.DefaultParamActualSignsOnly, DBConfigMode.GlobalOnly);
  }

  /// <summary>
  /// В случае пустого конструктора производить назначение property по отдельности
  /// </summary>
  public CertSheetProcessor()
  {
    this.certSheetGraphSortMethod = (CertSheetGraphSortMethod) (ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations).ReadInteger("CLIENT", "CERTSHEETS", "CERTSHEETGRAPHSORT", Convert.ToInt64((object) CertSheetGraphSortMethod.ByDefault), DBConfigMode.GlobalOnly);
    this.certSheetG09AttributeId = CertSheetProcessor.GetCertSheetG09AttributeId(out this.certSheetG09AttributeGuid);
    this.certSheetG10AttributeId = CertSheetProcessor.GetCertSheetG10AttributeId(out this.certSheetG10AttributeGuid);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="certSheetOptions">контрол с опциями получения УЛ</param>
  /// <param name="SaveToDiskOptions">дополнительные настройки для сохранения на диск. при открытии без сохранения null</param>
  public CertSheetProcessor(CertSheetOptions certSheetOptions)
    : this()
  {
    this.certSheetOptions = certSheetOptions;
  }

  /// <summary>Вернуть обозначение сформированного документа УЛ</summary>
  /// <param name="imDocument"></param>
  /// <returns></returns>
  public static string GetCertSheetDesignation(ImDocument imDocument)
  {
    if (imDocument == null)
      return string.Empty;
    string sheetDesignation = string.Empty;
    TextBoxElement templateRecursive = (TextBoxElement) imDocument.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.CertSheet_Designation);
    if (templateRecursive != null)
      sheetDesignation = templateRecursive.Text;
    return sheetDesignation;
  }

  private string GetGraphDescriptionByID(string graphID)
  {
    string empty = string.Empty;
    for (int index = 0; index < this.graphList.Count; ++index)
    {
      if (Convert.ToString(this.graphList[index][0]).Equals(graphID))
      {
        empty = Convert.ToString(this.graphList[index][1]);
        break;
      }
    }
    return empty;
  }

  public List<ImDocument> CreateCertSheets()
  {
    ExpiredAuthFileUsing lExpiredAuthFileUsing = ExpiredAuthFileUsing.YesForAll;
    return this.CreateCertSheets(true, ref lExpiredAuthFileUsing);
  }

  /// <summary>Функция получения УЛ</summary>
  public List<ImDocument> CreateCertSheets(
    bool silent,
    ref ExpiredAuthFileUsing lExpiredAuthFileUsing)
  {
    this.silentMode = silent;
    this.expiredAuthFileUsing = lExpiredAuthFileUsing;
    try
    {
      List<CertSheetData> certSheetDataList = new List<CertSheetData>();
      for (int index = 0; index < this.certSheetOptions.ObjectIDList.Count; ++index)
      {
        CertSheetData docs = CertSheetProcessor.ExpandObjectToDocs(this.certSheetOptions.ObjectIDList[index], this.certSheetOptions.ExpandECO, this.certSheetOptions.ExpandComposition);
        certSheetDataList.Add(docs);
      }
      CertSheetTemplate certSheetTemplate = new CertSheetTemplate();
      if (!certSheetTemplate.LoadTemplate())
      {
        int num = (int) IMMessageBox.Show(MessageDialogs.msgError, LocalizationHolder.rm.GetString("CertSheetBlankNotFound"), MessageBoxButtons.OK, IMMessageBoxImage.Error);
        return (List<ImDocument>) null;
      }
      string errorField = string.Empty;
      if (!certSheetTemplate.CheckFields(out errorField))
      {
        this.ShowBlankFieldError(errorField);
        return (List<ImDocument>) null;
      }
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        List<ImDocument> certSheets = new List<ImDocument>();
        for (int index1 = 0; index1 < certSheetDataList.Count; ++index1)
        {
          CertSheetData certSheetData = certSheetDataList[index1];
          if (certSheetData.IsDocument && sessionKeeper.Session.GetObject(certSheetData.ObjectId) != null)
          {
            string docDesignation = string.Empty;
            string docDescription = string.Empty;
            string docVersion = string.Empty;
            string docChangeNo = string.Empty;
            ImDocument document = certSheetTemplate.CreateDocument();
            int nn = 0;
            TableElement templateRecursive = document.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.CertSheet_Top_Table) as TableElement;
            TextBoxElement doc_n = (TextBoxElement) null;
            if (this.FillDocumentCustom(certSheetTemplate, templateRecursive, certSheetData.ObjectId, ref nn, ref doc_n, out docDesignation, out docDescription, out docVersion, out docChangeNo))
            {
              if (certSheetData.Docs != null)
              {
                for (int index2 = 0; index2 < certSheetData.Docs.Count; ++index2)
                  this.FillDocumentCustom(certSheetTemplate, templateRecursive, certSheetData.Docs[index2], ref nn, ref doc_n);
              }
              if (certSheetData.CertSheetDataList != null)
              {
                for (int index3 = 0; index3 < certSheetData.CertSheetDataList.Count; ++index3)
                {
                  if (this.FillDocumentCustom(certSheetTemplate, templateRecursive, certSheetData.CertSheetDataList[index3].ObjectId, ref nn, ref doc_n))
                  {
                    if (certSheetData.CertSheetDataList[index3].Docs != null)
                    {
                      for (int index4 = 0; index4 < certSheetData.CertSheetDataList[index3].Docs.Count; ++index4)
                      {
                        if (certSheetData.ObjectId != certSheetData.CertSheetDataList[index3].Docs[index4])
                          this.FillDocumentCustom(certSheetTemplate, templateRecursive, certSheetData.CertSheetDataList[index3].Docs[index4], ref nn, ref doc_n);
                      }
                    }
                    if (certSheetData.CertSheetDataList[index3].CertSheetDataList != null)
                    {
                      for (int index5 = 0; index5 < certSheetData.CertSheetDataList[index3].CertSheetDataList.Count; ++index5)
                        this.FillDocumentCustom(certSheetTemplate, templateRecursive, certSheetData.CertSheetDataList[index3].CertSheetDataList[index5].ObjectId, ref nn, ref doc_n);
                    }
                  }
                }
              }
              if (nn == 1 && doc_n != null)
                doc_n.AssignText(string.Empty, false, false, false);
              string stampDesignation = $"{docDesignation}-{LocalizationHolder.rm.GetString("CertSheetDesignationPostfix")}";
              this.FillStampBlock(document.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.CertSheet_Stamp) as TableElement, stampDesignation);
              if (nn > 0)
              {
                document.UpdateLayout(false);
                certSheets.Add(document);
              }
              else
                certSheets.Add((ImDocument) null);
            }
          }
        }
        return certSheets;
      }
    }
    finally
    {
      lExpiredAuthFileUsing = this.expiredAuthFileUsing;
    }
  }

  /// <summary>
  /// записать в документ информацию по отдельному документу, его файлам и подписям
  /// </summary>
  /// <param name="certSheetTemplate"></param>
  /// <param name="certSheet_Top_Table"></param>
  /// <param name="objId"></param>
  /// <param name="nn"></param>
  /// <param name="docDesignation"></param>
  /// <param name="docDescription"></param>
  /// <param name="docVersion"></param>
  /// <param name="docChangeNo"></param>
  /// <returns></returns>
  private bool FillDocumentCustom(
    CertSheetTemplate certSheetTemplate,
    TableElement certSheet_Top_Table,
    long objId,
    ref int nn,
    ref TextBoxElement doc_n,
    out string docDesignation,
    out string docDescription,
    out string docVersion,
    out string docChangeNo)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject documentAttributes = this.GetDocumentAttributes(sessionKeeper.Session, objId, out docDesignation, out docDescription, out docVersion, out docChangeNo);
      if (documentAttributes == null)
        return false;
      IDBAttribute attributeByGuid = documentAttributes.GetAttributeByGuid(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545"));
      if (attributeByGuid == null)
        return true;
      List<BlobInformation> biList = (List<BlobInformation>) null;
      List<int> validFileIndexes = this.GetValidFileIndexes(documentAttributes, attributeByGuid, out biList, this.silentMode, ref this.expiredAuthFileUsing);
      if (validFileIndexes == null || validFileIndexes.Count == 0)
        return true;
      ++nn;
      string docDescription1 = docDescription;
      IDBObjectType objectType = sessionKeeper.Session.GetObjectType(documentAttributes.ObjectType);
      if (objectType != null)
      {
        if (docDescription1 != string.Empty && objectType.ObjectInstanceName != string.Empty)
          docDescription1 += "\n";
        docDescription1 += objectType.ObjectInstanceName;
      }
      this.FillBodyBlock(certSheetTemplate, certSheet_Top_Table, nn, ref doc_n, docDesignation, docDescription1, docVersion, docChangeNo);
      this.FillEmptyBlock(certSheetTemplate, certSheet_Top_Table, CertSheetConsts.Empty1);
      this.FillFileChecksumBlock(certSheetTemplate, certSheet_Top_Table, attributeByGuid, validFileIndexes, biList, objId);
      IDBObject docObj = documentAttributes;
      IDBAttribute byGuid = documentAttributes.Attributes.FindByGUID(new Guid("cad001a6-306c-11d8-b4e9-00304f19f545"));
      if (byGuid != null && byGuid.Value != null)
      {
        long int64 = Convert.ToInt64(byGuid.Value);
        IDBObject dbObject = sessionKeeper.Session.GetObject(int64);
        if (dbObject != null)
          docObj = dbObject;
      }
      this.FillSignsBlock(certSheetTemplate, certSheet_Top_Table, docObj);
      this.FillEmptyBlock(certSheetTemplate, certSheet_Top_Table, CertSheetConsts.Empty4);
    }
    return true;
  }

  private bool FillDocumentCustom(
    CertSheetTemplate certSheetTemplate,
    TableElement certSheet_Top_Table,
    long objId,
    ref int nn,
    ref TextBoxElement doc_n)
  {
    return this.FillDocumentCustom(certSheetTemplate, certSheet_Top_Table, objId, ref nn, ref doc_n, out string _, out string _, out string _, out string _);
  }

  /// <summary>заполняем шапку</summary>
  /// <param name="certSheetTemplate"></param>
  /// <param name="certSheet_Top_Table"></param>
  /// <param name="nn"></param>
  /// <param name="docDesignation"></param>
  /// <param name="docDescription"></param>
  /// <param name="docVersion"></param>
  /// <param name="docChangeNo"></param>
  private void FillBodyBlock(
    CertSheetTemplate certSheetTemplate,
    TableElement certSheet_Top_Table,
    int nn,
    ref TextBoxElement doc_n,
    string docDesignation,
    string docDescription,
    string docVersion,
    string docChangeNo)
  {
    TableElement docHeader = certSheetTemplate.Get_Doc_Header();
    certSheet_Top_Table.AddChildNode((DocumentTreeNode) docHeader, false, false);
    TableElement docBody = certSheetTemplate.Get_Doc_Body();
    certSheet_Top_Table.AddChildNode((DocumentTreeNode) docBody, false, false);
    TextBoxElement templateRecursive = (TextBoxElement) docBody.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.Doc_N);
    templateRecursive.AssignText(Convert.ToString(nn), false, false, false);
    if (doc_n == null)
      doc_n = templateRecursive;
    ((TextData) docBody.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.Doc_Designation)).AssignText(docDesignation, false, false, false);
    ((TextData) docBody.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.Doc_Description)).AssignText(docDescription, false, false, false);
    ((TextData) docBody.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.Doc_Version)).AssignText(docVersion, false, false, false);
    ((TextData) docBody.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.Doc_ChangeNo)).AssignText(docChangeNo, false, false, false);
  }

  /// <summary>
  /// получить список индексов файлового атрибута, которые можно использовать
  /// </summary>
  /// <param name="iFileAttribute"></param>
  /// <returns></returns>
  private List<int> GetValidFileIndexes(
    IDBObject iDBObject,
    IDBAttribute iFileAttribute,
    out List<BlobInformation> biList,
    bool silent,
    ref ExpiredAuthFileUsing lExpiredAuthFileUsing)
  {
    biList = (List<BlobInformation>) null;
    if (iDBObject == null || iFileAttribute == null)
      return (List<int>) null;
    List<int> validFileIndexes = new List<int>();
    biList = new List<BlobInformation>();
    DateTime t1 = DateTime.MinValue;
    if (this.certSheetOptions.AuthFilesMode && lExpiredAuthFileUsing != ExpiredAuthFileUsing.YesForAll)
    {
      IDBAttribute byGuid = iDBObject.Attributes.FindByGUID(new Guid("cad0013a-306c-11d8-b4e9-00304f19f545"));
      if (byGuid != null)
        t1 = byGuid.AsDateTime;
    }
    for (int index = 0; index < iFileAttribute.ValuesCount; ++index)
    {
      iFileAttribute.Index = index;
      BlobInformation blobInformation = (iFileAttribute as IBlobReader).OpenBlob(-1);
      if ((this.certSheetOptions.AuthFilesMode && blobInformation.FileType == FileTypes.ftAuthentical || this.certSheetOptions.NormalFilesMode && blobInformation.FileType == FileTypes.ftNormal) && this.certSheetOptions.Extensions.IndexOf(Path.GetExtension(blobInformation.FileName).ToLower()) != -1)
      {
        if (this.certSheetOptions.AuthFilesMode && blobInformation.FileType == FileTypes.ftAuthentical && DateTime.Compare(t1, blobInformation.ModifyDate) > 0)
        {
          if (lExpiredAuthFileUsing != ExpiredAuthFileUsing.NoForAll)
          {
            if (silent)
            {
              if (ServicesManager.GetService(typeof (IOutputView)) is IOutputView service)
                service.WriteString(LocalizationHolder.rm.GetString("OutputView_SavingToDisk"), string.Format(LocalizationHolder.rm.GetString("ExpiredAuthFileDetected"), (object) iDBObject.Caption, (object) blobInformation.FileName));
            }
            else if (lExpiredAuthFileUsing == ExpiredAuthFileUsing.None)
            {
              switch (IMMessageBox.Show(MessageDialogs.msgQuery, $"{string.Format(LocalizationHolder.rm.GetString("ExpiredAuthFileDetected"), (object) iDBObject.Caption, (object) blobInformation.FileName)} {LocalizationHolder.rm.GetString("ExpiredAuthFileCreateQuestion")}", new IMMessageBoxButton[4]
              {
                new IMMessageBoxButton(LocalizationHolder.rm.GetString("AnswerYes"), DialogResult.Yes),
                new IMMessageBoxButton(LocalizationHolder.rm.GetString("AnswerYesForAll"), DialogResult.OK),
                new IMMessageBoxButton(LocalizationHolder.rm.GetString("AnswerNo"), DialogResult.No),
                new IMMessageBoxButton(LocalizationHolder.rm.GetString("AnswerNoForAll"), DialogResult.Ignore)
              }, IMMessageBoxImage.Question))
              {
                case DialogResult.OK:
                  lExpiredAuthFileUsing = ExpiredAuthFileUsing.YesForAll;
                  break;
                case DialogResult.Ignore:
                  lExpiredAuthFileUsing = ExpiredAuthFileUsing.NoForAll;
                  continue;
                case DialogResult.No:
                  continue;
              }
            }
          }
          else
            continue;
        }
        validFileIndexes.Add(index);
        biList.Add(blobInformation);
      }
    }
    return validFileIndexes;
  }

  /// <summary>Заполняем блок файлов</summary>
  /// <param name="certSheetTemplate"></param>
  /// <param name="iFileAttribute"></param>
  private void FillFileChecksumBlock(
    CertSheetTemplate certSheetTemplate,
    TableElement certSheet_Top_Table,
    IDBAttribute iFileAttribute,
    List<int> fileIndexList,
    List<BlobInformation> biList,
    long objectID)
  {
    if (iFileAttribute == null)
      return;
    string str1 = (string) null;
    string str2 = (string) null;
    if (fileIndexList.Count > 0 && (this.certSheetG09AttributeId != 0 || this.certSheetG10AttributeId != 0))
    {
      IDBObject idbObject = iFileAttribute.Session.GetObject(iFileAttribute.DBObjectID);
      if (idbObject != null)
      {
        if (this.certSheetG09AttributeId != 0)
          str1 = this.GetObjectAttributeValue(this.certSheetG09AttributeId, idbObject);
        if (this.certSheetG10AttributeId != 0)
          str2 = this.GetObjectAttributeValue(this.certSheetG10AttributeId, idbObject);
      }
    }
    string caption = EnumTypeHelper.GetCaption((Enum) this.certSheetOptions.ChecksumAlgorithm);
    for (int index = 0; index < fileIndexList.Count; ++index)
    {
      iFileAttribute.Index = fileIndexList[index];
      BlobInformation bi = (iFileAttribute as IBlobReader).OpenBlob(-1);
      string empty = string.Empty;
      if (iFileAttribute.Session.GetCustomService(typeof (IChecksumsService)) is IChecksumsService customService)
      {
        Guid taskGuid = customService.CalcChecksum(iFileAttribute.Session.SessionGUID, objectID, AttributableElements.Object, iFileAttribute.AttributeID, index, this.certSheetOptions.ChecksumAlgorithm);
        try
        {
          ChecksumTaskProgress checksumTaskProgress;
          for (checksumTaskProgress = customService.GetChecksumTaskProgress(taskGuid); checksumTaskProgress.Operation != ChecksumOperationType.Error && checksumTaskProgress.Operation != ChecksumOperationType.Finished; checksumTaskProgress = customService.GetChecksumTaskProgress(taskGuid))
            Thread.Sleep(250);
          if (checksumTaskProgress.Operation == ChecksumOperationType.Finished)
            empty = customService.GetChecksum(taskGuid).ToString();
        }
        finally
        {
          customService.ChecksumFree(taskGuid);
        }
      }
      TableElement fileHeader = certSheetTemplate.Get_File_Header();
      certSheet_Top_Table.AddChildNode((DocumentTreeNode) fileHeader, false, false);
      this.FillEmptyBlock(certSheetTemplate, certSheet_Top_Table, CertSheetConsts.Empty2);
      TableElement fileNotes = certSheetTemplate.Get_File_Notes();
      certSheet_Top_Table.AddChildNode((DocumentTreeNode) fileNotes, false, false);
      this.FillEmptyBlock(certSheetTemplate, certSheet_Top_Table, CertSheetConsts.Empty3);
      ((TextData) fileHeader.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.File_Checksum_Type))?.AssignText(caption, false, false, false);
      ((TextData) fileHeader.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.File_Checksum))?.AssignText(empty, false, false, false);
      ((TextData) fileNotes.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.File_Name))?.AssignText(Path.GetFileName(biList[index].FileName), false, false, false);
      if (str1 != null || this.WillBeCustomValue(this.certSheetG09AttributeGuid))
      {
        TextBoxElement templateRecursive = (TextBoxElement) fileNotes.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.File_Reserved1);
        if (templateRecursive != null)
        {
          string str3 = str1 ?? this.GetCustomValue(this.certSheetG09AttributeGuid, bi);
          templateRecursive.AssignText(str3, false, false, false);
        }
      }
      if (str2 != null || this.WillBeCustomValue(this.certSheetG10AttributeGuid))
      {
        TextBoxElement textBoxElement = (TextBoxElement) fileNotes.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.File_Reserverd2) ?? (TextBoxElement) fileNotes.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.File_Reserved2);
        if (textBoxElement != null)
        {
          string str4 = str2 ?? this.GetCustomValue(this.certSheetG10AttributeGuid, bi);
          textBoxElement.AssignText(str4, false, false, false);
        }
      }
    }
  }

  private bool WillBeCustomValue(Guid attrGuid)
  {
    return attrGuid.Equals(CertSheetProcessor.FileSizeSystemAttributeGuid) || attrGuid.Equals(CertSheetProcessor.FileDateSystemAttributeGuid);
  }

  private string GetCustomValue(Guid attrGuid, BlobInformation bi)
  {
    string customValue = "";
    if (attrGuid.Equals(CertSheetProcessor.FileSizeSystemAttributeGuid))
      customValue = bi.RealFileSize.ToString();
    else if (attrGuid.Equals(CertSheetProcessor.FileDateSystemAttributeGuid))
      customValue = bi.ModifyDate.ToString("G", (IFormatProvider) CultureInfo.CurrentUICulture);
    return customValue;
  }

  private string GetObjectAttributeValue(int certSheetAttributeId, IDBObject idbObject)
  {
    string objectAttributeValue = (string) null;
    if (certSheetAttributeId < 0)
    {
      string[] descriptionsById = idbObject.GetDescriptionsByID(certSheetAttributeId, false);
      if (descriptionsById != null && descriptionsById.Length != 0)
        objectAttributeValue = descriptionsById[0];
    }
    else
    {
      IDBAttribute byId = idbObject.Attributes.FindByID(certSheetAttributeId);
      if (byId != null)
        objectAttributeValue = byId.AsString;
    }
    return objectAttributeValue;
  }

  /// <summary>Заполняем блок подписей</summary>
  /// <param name="certSheetTemplate"></param>
  /// <param name="certSheet_Top_Table"></param>
  /// <param name="docObj"></param>
  private void FillSignsBlock(
    CertSheetTemplate certSheetTemplate,
    TableElement certSheet_Top_Table,
    IDBObject docObj)
  {
    if (docObj == null)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long num1 = Math.Abs(docObj.ObjectID);
      DateTime asDateTime = docObj.GetAttributeByGuid(new Guid("cad0013a-306c-11d8-b4e9-00304f19f545")).AsDateTime;
      int num2 = 0;
      DBRecordSetParams dbRecordSetParams = new DBRecordSetParams((ConditionStructure[]) null);
      dbRecordSetParams.Conditions = new ConditionStructure[1]
      {
        new ConditionStructure(0, RelationalOperators.EntersIn, (object) num1, LogicalOperators.AND, 0, false)
        {
          TypeID = (object) SignsHolder.SignRelationTypeID
        }
      };
      dbRecordSetParams.Columns = new object[7]
      {
        (object) -2,
        (object) SignsHolder.GraphAttrTypeID,
        (object) SignsHolder.SignUpAttrTypeID,
        (object) SignsHolder.DateOfSignatureID,
        (object) -7,
        (object) SignsHolder.ModifyDateAttrTypeID,
        (object) SignsHolder.RankAttrTypeID
      };
      DataTable dataTable = sessionKeeper.Session.ObjectsSelect(SignsHolder.SignObjectTypeID, dbRecordSetParams);
      IMSObjectType objectType1 = MetaDataHelper.GetObjectType(SignsHolder.SignObjectTypeID);
      IMSObjectType objectType2 = MetaDataHelper.GetObjectType(SignsHolder.CryptoSignObjectTypeID);
      if ((objectType1.Options & ObjectTypeOptions.LocalObjectType) != ObjectTypeOptions.None || (objectType2.Options & ObjectTypeOptions.LocalObjectType) != ObjectTypeOptions.None)
      {
        DataTable table = sessionKeeper.Session.ObjectsSelect(SignsHolder.CryptoSignObjectTypeID, dbRecordSetParams);
        dataTable.Merge(table);
      }
      CertSheetTableElementList tableElementList = new CertSheetTableElementList(this.certSheetGraphSortMethod);
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        long int64 = Convert.ToInt64(row[0]);
        int int32 = Convert.ToInt32(row[4]);
        DateTime dateTime = Convert.ToDateTime(row[5]);
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string empty3 = string.Empty;
        string empty4 = string.Empty;
        string str1 = Convert.ToString(row[1]);
        if (this.certSheetOptions.GraphEnabled(str1))
        {
          string graphDescriptionById = this.GetGraphDescriptionByID(str1);
          string str2 = Convert.ToString(row[2]);
          string str3 = Convert.ToString(row[6]);
          string str4 = Convert.ToDateTime(row[3]).ToString("d", (IFormatProvider) CultureInfo.CurrentCulture);
          if (this.actualSignsOnly)
          {
            switch (SignHelper.TranslateStatus(sessionKeeper.Session, num1, int64, int32, asDateTime, dateTime))
            {
              case SignStatuses.CryptoSignActual:
              case SignStatuses.SignActual:
                break;
              default:
                continue;
            }
          }
          TableElement signsBody = certSheetTemplate.Get_Signs_Body();
          tableElementList.Add(new CertSheetTableElement(str1, graphDescriptionById, signsBody));
          ((TextData) signsBody.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.Signs_Graph))?.AssignText(graphDescriptionById, false, false, false);
          ((TextData) signsBody.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.Signs_Username))?.AssignText(str2, false, false, false);
          ((TextData) signsBody.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.Signs_Position))?.AssignText(str3, false, false, false);
          ((TextData) signsBody.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.Signs_Date))?.AssignText(str4, false, false, false);
          ++num2;
        }
      }
      if (this.certSheetOptions.EmptyGraphs != null)
      {
        for (int index = 0; index < this.certSheetOptions.EmptyGraphs.Count; ++index)
        {
          string id = Convert.ToString(this.certSheetOptions.EmptyGraphs[index][0]);
          string descr = Convert.ToString(this.certSheetOptions.EmptyGraphs[index][1]);
          TableElement signsBody = certSheetTemplate.Get_Signs_Body();
          tableElementList.Add(new CertSheetTableElement(id, descr, signsBody, true));
          ((TextData) signsBody.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.Signs_Graph))?.AssignText(descr, false, false, false);
          ++num2;
        }
      }
      tableElementList.SortItems();
      for (int index = 0; index < tableElementList.Count; ++index)
        certSheet_Top_Table.AddChildNode((DocumentTreeNode) tableElementList[index].TableElement, false, false);
      if (num2 != 0)
        return;
      TableElement signsBody1 = certSheetTemplate.Get_Signs_Body();
      certSheet_Top_Table.AddChildNode((DocumentTreeNode) signsBody1, false, false);
    }
  }

  private void FillEmptyBlock(
    CertSheetTemplate certSheetTemplate,
    TableElement certSheet_Top_Table,
    string blockName)
  {
    TableElement empty = certSheetTemplate.Get_Empty(blockName);
    if (empty == null)
      return;
    certSheet_Top_Table.AddChildNode((DocumentTreeNode) empty, false, false);
  }

  /// <summary>заполняем штамп</summary>
  /// <param name="docCertsheet"></param>
  /// <param name="stampDesignation"></param>
  private void FillStampBlock(TableElement certSheet_Stamp, string stampDesignation)
  {
    ((TextData) certSheet_Stamp.FindFirstNodeFromTemplate_Recursive(CertSheetConsts.CertSheet_Designation)).AssignText(stampDesignation, false, false, false);
  }

  /// <summary>вернуть главные атрибуты объекта</summary>
  /// <param name="docId"></param>
  /// <param name="docDesignation"></param>
  /// <param name="docDescription"></param>
  /// <param name="docVersion"></param>
  /// <param name="docChangeNo"></param>
  /// <returns></returns>
  private IDBObject GetDocumentAttributes(
    IUserSession session,
    long docId,
    out string docDesignation,
    out string docDescription,
    out string docVersion,
    out string docChangeNo)
  {
    docDesignation = string.Empty;
    docDescription = string.Empty;
    docVersion = string.Empty;
    docChangeNo = string.Empty;
    IDBObject documentAttributes = session.GetObject(docId);
    if (documentAttributes == null)
      return (IDBObject) null;
    IDBAttribute byGuid1 = documentAttributes.Attributes.FindByGUID(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"));
    if (byGuid1 != null)
      docDesignation = Convert.ToString(byGuid1.Value);
    IDBAttribute byGuid2 = documentAttributes.Attributes.FindByGUID(new Guid("cad00020-306c-11d8-b4e9-00304f19f545"));
    if (byGuid2 != null)
      docDescription = Convert.ToString(byGuid2.Value);
    docVersion = documentAttributes.VersionID.ToString();
    IDBAttribute byGuid3 = documentAttributes.Attributes.FindByGUID(new Guid("cad00770-306c-11d8-b4e9-00304f19f545"));
    if (byGuid3 != null)
      docChangeNo = Convert.ToString(byGuid3.Value);
    return documentAttributes;
  }

  private void ShowBlankFieldError(string field)
  {
    int num = (int) IMMessageBox.Show(MessageDialogs.msgError, string.Format(LocalizationHolder.rm.GetString("CertSheetBlankFieldNotFound"), (object) field), MessageBoxButtons.OK, IMMessageBoxImage.Error);
  }

  /// <summary>Сохранение УЛ на диск в формате pdf</summary>
  /// <param name="iSaveToDiskClass">опции сохранения, в частности, базовая папка для сохранения</param>
  /// <param name="folder">конкретная папка сохранения документов для конкретно данного объекта</param>
  /// <param name="objectID"></param>
  public void Save(ISaveToDiskClass iSaveToDiskClass, string folder, long objectID)
  {
    if (iSaveToDiskClass == null || this.certSheetOptions == null || !this.certSheetOptions.ProcessCertSheets || !Directory.Exists(iSaveToDiskClass.SelectedPath) || !Directory.Exists(folder))
      return;
    this.certSheetOptions.ObjectIDList.Clear();
    this.certSheetOptions.ObjectIDList.Add(objectID);
    List<ImDocument> certSheets = this.CreateCertSheets();
    if (certSheets == null)
      return;
    string empty = string.Empty;
    string path = !this.certSheetOptions.SaveToStandaloneFolder ? folder : Path.Combine(iSaveToDiskClass.SelectedPath, this.certSheetCommonFolderName);
    for (int index = 0; index < certSheets.Count; ++index)
    {
      if (certSheets[index] != null)
      {
        string str = OSHelper.ReplaceForbiddenSymbols(CertSheetProcessor.GetCertSheetDesignation(certSheets[index]) + ImDocumentFormatHelper.GetExtension(ImDocumentFormat.PdfFormat), ' ');
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        string fileName = path + Path.DirectorySeparatorChar.ToString() + str;
        certSheets[index].SaveToPdf(fileName, false);
      }
    }
  }

  /// <summary>Вернуть id бланка УЛ</summary>
  /// <returns>-1, если не назначен</returns>
  private static long GetCertSheetBlankId()
  {
    long certSheetBlankId = -1;
    string g = (ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations).ReadString("CLIENT", "CERTSHEETS", "BLANKGUID", CertSheetHolder.DefaultParamBlankGuid, DBConfigMode.GlobalOnly);
    if (g.Trim() != string.Empty)
    {
      QuickObjectInfo objectInfo = ApplicationServices.Container.GetService<IObjectsInfoCache>().GetObjectInfo(new Guid(g));
      if (!objectInfo.Empty)
        certSheetBlankId = objectInfo.ObjectID;
    }
    return certSheetBlankId;
  }

  private static string GetCertSheetCommonFolderName()
  {
    string empty = string.Empty;
    string commonFolderName = (ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations).ReadString("CLIENT", "CERTSHEETS", "COMMONFOLDER", CertSheetHolder.DefaultParamCertSheetCommonFolder, DBConfigMode.GlobalOnly);
    if (commonFolderName == string.Empty)
      commonFolderName = CertSheetHolder.DefaultParamCertSheetCommonFolder;
    return commonFolderName;
  }

  private static int GetCertSheetG09AttributeId(out Guid lg09AttributeGuid)
  {
    lg09AttributeGuid = Guid.Empty;
    string g = (ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations).ReadString("CLIENT", "CERTSHEETS", "G09ATTRIBUTEGUID", CertSheetHolder.DefaultParamG09AttributeGuid, DBConfigMode.GlobalOnly);
    int sheetG09AttributeId = 0;
    if (g != string.Empty)
    {
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(new Guid(g));
      if (attributeType != null)
      {
        sheetG09AttributeId = attributeType.AttributeID;
        lg09AttributeGuid = attributeType.AttributeGuid;
      }
    }
    return sheetG09AttributeId;
  }

  private static int GetCertSheetG10AttributeId(out Guid lg10AttributeGuid)
  {
    lg10AttributeGuid = Guid.Empty;
    string g = (ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations).ReadString("CLIENT", "CERTSHEETS", "G10ATTRIBUTEGUID", CertSheetHolder.DefaultParamG10AttributeGuid, DBConfigMode.GlobalOnly);
    int sheetG10AttributeId = 0;
    if (g != string.Empty)
    {
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(new Guid(g));
      if (attributeType != null)
      {
        sheetG10AttributeId = attributeType.AttributeID;
        lg10AttributeGuid = attributeType.AttributeGuid;
      }
    }
    return sheetG10AttributeId;
  }

  /// <summary>
  /// Вернуть все графы для подписей в системе парами object[]{string id, string description}
  /// </summary>
  /// <returns></returns>
  public static List<object[]> GetGraphs()
  {
    List<object[]> graphs = new List<object[]>();
    DataRow[] possibleValuesRows = (ServicesManager.GetService(typeof (IClientMetadataCache)) as IClientMetadataCache).GetAttributeType(SignsHolder.GraphAttrTypeGuid, true).GetPossibleValuesRows();
    for (int index = 0; index < possibleValuesRows.Length; ++index)
      graphs.Add(new object[2]
      {
        (object) Convert.ToString(possibleValuesRows[index]["F_STRING_VALUE"]),
        (object) Convert.ToString(possibleValuesRows[index]["F_DESCRIPTION"])
      });
    return graphs;
  }

  /// <summary>
  /// Вернуть список граф в виде пар object[]{string id, string description}, имеющихся в подписях у объектов objVerList
  /// При большом количестве документов вернуть все графы для подписей в системе.
  /// </summary>
  /// <param name="objVerList">при null вернуть все графы в системе</param>
  /// <returns></returns>
  public static List<object[]> GetGraphs(List<long> objVerList)
  {
    List<object[]> graphs = CertSheetProcessor.GetGraphs();
    if (objVerList == null || objVerList.Count > CertSheetProcessor.MaxDocsForSeparateGraphsRead)
      return graphs;
    List<string> stringList = new List<string>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(SignsHolder.SignRelationTypeID);
      DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[0], new object[1]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID
      });
      for (int index = 0; index < objVerList.Count; ++index)
      {
        foreach (DataRow row in (InternalDataCollectionBase) relationCollection.ConsistFrom(paramSet, objVerList[index]).Rows)
        {
          long int64 = Convert.ToInt64(row[0]);
          IDBAttribute attributeByGuid = sessionKeeper.Session.GetObject(int64, true).GetAttributeByGuid(SignsHolder.GraphAttrTypeGuid);
          if (attributeByGuid != null)
          {
            string asString = attributeByGuid.AsString;
            if (stringList.IndexOf(asString) == -1)
              stringList.Add(asString);
          }
        }
      }
    }
    int index1 = 0;
    while (index1 < graphs.Count)
    {
      int index2 = stringList.IndexOf((string) graphs[index1][0]);
      if (index2 == -1)
      {
        graphs.RemoveAt(index1);
      }
      else
      {
        stringList.RemoveAt(index2);
        ++index1;
      }
    }
    return graphs;
  }

  /// <summary>Вернуть список всех расширений для всех документов</summary>
  /// <returns></returns>
  public static List<string> GetExtensions()
  {
    if (CertSheetProcessor.allExtensionsList != null)
      return CertSheetProcessor.allExtensionsList;
    CertSheetProcessor.allExtensionsList = new List<string>();
    for (int index = 0; index < ImDocumentData.ImDocumentFileExtensions.Count; ++index)
      CertSheetProcessor.allExtensionsList.Add("." + ImDocumentData.ImDocumentFileExtensions[index]);
    List<int> childrenIdRecursive = MetaDataHelper.GetObjectTypeChildrenIDRecursive(MetaDataHelper.GetObjectTypeID("cad00070-306c-11d8-b4e9-00304f19f545"));
    for (int index = 0; index < childrenIdRecursive.Count; ++index)
      CertSheetProcessor.AddDTSToList(childrenIdRecursive[index], CertSheetProcessor.allExtensionsList);
    return CertSheetProcessor.allExtensionsList;
  }

  /// <summary>
  /// Вернуть список расширений для основных файлов (normal) документов
  /// </summary>
  /// <param name="docVerList">список идентификаторов версий объектов документов</param>
  /// <returns>список ".ext1" ".ext2" ".ext3"</returns>
  public static List<string> GetExtensions(List<long> docVerList)
  {
    if (docVerList == null)
      return CertSheetProcessor.GetExtensions();
    List<string> list = new List<string>();
    List<int> intList = new List<int>();
    IObjectsInfoCache service = ApplicationServices.Container.GetService<IObjectsInfoCache>();
    for (int index = 0; index < docVerList.Count; ++index)
    {
      QuickObjectInfo objectInfo = service.GetObjectInfo(docVerList[index]);
      if (intList.IndexOf(objectInfo.ObjectTypeID) < 0)
      {
        intList.Add(objectInfo.ObjectTypeID);
        CertSheetProcessor.AddDTSToList(objectInfo.ObjectTypeID, list);
      }
    }
    return list;
  }

  /// <summary>
  /// добавить расширения из настроек для типа документтов docObjType в список list
  /// </summary>
  /// <param name="docObjType"></param>
  /// <param name="list"></param>
  private static void AddDTSToList(int docObjType, List<string> list)
  {
    DocumentTypeSettings settings = DocumentTypeSettingsCache.GetSettings(docObjType);
    if (settings.DocumentFileExt.Trim() != string.Empty)
    {
      string lower = settings.DocumentFileExt.Trim().ToLower();
      if (list.IndexOf(lower) == -1)
        list.Add(lower);
    }
    if (!(settings.AdditionalDocumentFileExts.Trim() != string.Empty))
      return;
    List<string> stringList = DocumentTypeSettings.SplitAdditionalFileExts(settings.AdditionalDocumentFileExts);
    for (int index = 0; index < stringList.Count; ++index)
    {
      string lower = stringList[index].ToLower();
      if (list.IndexOf(lower) == -1)
        list.Add(lower);
    }
  }

  /// <summary>Вернуть список расширений для аутентичных файлов</summary>
  /// <returns>список ".ext1" ".ext2" ".ext3"</returns>
  public static List<string> GetPossibleExtensions4AuthFiles()
  {
    return DocumentTypeSettings.SplitAdditionalFileExts((ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations).ReadString("CLIENT", "AUTHFILES", "AUTHFILESEXTENSIONS", "", DBConfigMode.GlobalOnly));
  }

  /// <summary>
  /// Развернуть извещения, если они попадутся в списке документов
  /// </summary>
  /// <param name="objectId"></param>
  /// <returns>документы, изделия</returns>
  public static List<long> ExpandECO(long objectId)
  {
    return CertSheetProcessor.ExpandObjectsByRelation(objectId, CertSheetCache.ECORelationTypeID, true);
  }

  /// <summary>Развернуть состав первого уровня для изделия</summary>
  /// <param name="objectId">идентификатор изделия</param>
  /// <returns>документы, изделия</returns>
  public static List<long> ExpandComposition(long objectId)
  {
    return CertSheetProcessor.ExpandObjectsByRelation(objectId, CertSheetCache.CompositionRelationTypeID, true);
  }

  /// <summary>Развернуть спецификацию</summary>
  /// <param name="objectId"></param>
  /// <returns></returns>
  public static List<long> ExpandSpecification(long objectId, bool withComposition)
  {
    List<long> longList1 = CertSheetProcessor.ExpandObjectsByRelation(objectId, CertSheetCache.DocumentationRelationTypeID, false);
    if (withComposition)
    {
      List<long> longList2 = new List<long>();
      for (int index = 0; index < longList1.Count; ++index)
      {
        longList2.Add(longList1[index]);
        List<long> longList3 = CertSheetProcessor.ExpandComposition(longList1[index]);
        longList2.AddRange((IEnumerable<long>) longList3.ToArray());
      }
      longList1 = longList2;
    }
    return longList1;
  }

  /// <summary>Получить объекты по связи reltypeId для objectId</summary>
  /// <param name="objectId"></param>
  /// <param name="reltypeId"></param>
  /// <returns>объекты</returns>
  private static List<long> ExpandObjectsByRelation(long objectId, int reltypeId, bool consist)
  {
    List<long> longList = new List<long>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(reltypeId);
      DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[0], new object[1]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID
      });
      foreach (DataRow row in (InternalDataCollectionBase) (consist ? relationCollection.ConsistFrom(paramSet, objectId) : relationCollection.EntersInVersion(paramSet, objectId)).Rows)
      {
        if (consist || !consist && objectId != Convert.ToInt64(row[0]))
          longList.Add(Convert.ToInt64(row[0]));
      }
    }
    return longList;
  }

  /// <summary>
  /// Головная функция по раскрутке документа - извещения или спецификации.
  /// Для иного объекта возвращает список документов на объект.
  /// </summary>
  /// <param name="objectId"></param>
  /// <param name="expandECO"></param>
  /// <param name="expandComposition"></param>
  /// <returns></returns>
  public static CertSheetData ExpandObjectToDocs(
    long objectId,
    bool expandECO,
    bool expandComposition)
  {
    CertSheetData data = (CertSheetData) null;
    DBRecordSetParams dbRecordSetParams = new DBRecordSetParams(new ConditionStructure[0], new object[1]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID
    });
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(objectId);
      bool isDocument = dbObject.isParentType(CertSheetCache.DocumentObjectTypeGuid);
      if (isDocument)
      {
        data = new CertSheetData(objectId);
        if (expandECO && dbObject.isParentType(CertSheetCache.ECOObjectTypeGuid))
        {
          List<long> docOrObjList = CertSheetProcessor.ExpandECO(objectId);
          data.Init(DocGroupType.Eco, new CertSheetDataList());
          CertSheetProcessor.ExtractDocs(data, docOrObjList);
        }
        if (expandComposition)
        {
          if (dbObject.isParentType(CertSheetCache.SpecObjectTypeGuid))
          {
            List<long> docOrObjList = CertSheetProcessor.ExpandSpecification(objectId, true);
            data.Init(DocGroupType.Composition, new CertSheetDataList());
            CertSheetProcessor.ExtractDocs(data, docOrObjList);
          }
        }
      }
      else
      {
        List<long> documentsForObject = CertSheetProcessor.GetDocumentsForObject(objectId);
        data = new CertSheetData(objectId, isDocument, documentsForObject);
      }
    }
    return data;
  }

  /// <summary>
  /// Дополнить список документов docList документами из списка документов и изделий docOrObjList
  /// </summary>
  /// <param name="docList"></param>
  /// <param name="docOrObjList"></param>
  private static void ExtractDocs(CertSheetData data, List<long> docOrObjList)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      for (int index = 0; index < docOrObjList.Count; ++index)
      {
        if (sessionKeeper.Session.GetObject(docOrObjList[index]).isParentType(CertSheetCache.DocumentObjectTypeGuid))
        {
          CertSheetData certSheetData = new CertSheetData(docOrObjList[index]);
          data.CertSheetDataList.Add(certSheetData);
        }
        else
        {
          List<long> documentsForObject = CertSheetProcessor.GetDocumentsForObject(docOrObjList[index]);
          CertSheetData certSheetData = new CertSheetData(docOrObjList[index], false, documentsForObject);
          data.CertSheetDataList.Add(certSheetData);
        }
      }
    }
  }

  /// <summary>
  /// найти список документов на объект по связи "Документация на изделие".
  /// объект может быть любым: сборкой, извещением, изделием, в том числе самим документом (для документа он же и возвращается).
  /// </summary>
  /// <param name="objectId"></param>
  /// <returns></returns>
  public static List<long> GetDocumentsForObject(long objectId)
  {
    List<long> documentsForObject = new List<long>();
    DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[0], new object[1]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID
    });
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(CertSheetCache.DocumentationRelationTypeID);
      if (relationCollection != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) relationCollection.ConsistFrom(paramSet, objectId).Rows)
        {
          long int64 = Convert.ToInt64(row[0]);
          if (documentsForObject.IndexOf(int64) == -1)
            documentsForObject.Add(int64);
        }
      }
    }
    return documentsForObject;
  }

  /// <summary>
  /// Вернуть идентификатор объекта, на котором висят подписи к документу documentId.
  /// Если у документа documentId имеется атрибут "cad001a6", то по ссылке из этого атрибута есть объект, на котором висят подписи.
  /// Если атрибута нет, то подписи на самом документе.
  /// </summary>
  /// <param name="documentId"></param>
  /// <returns>-1 - не найден;</returns>
  public long GetSignedObject(long documentId)
  {
    long signedObject = documentId;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttributable dbAttributable = (IDBAttributable) sessionKeeper.Session.GetObject(documentId);
      if (dbAttributable != null)
      {
        IDBAttribute attributeByGuid = dbAttributable.GetAttributeByGuid(new Guid("cad001a6-306c-11d8-b4e9-00304f19f545"));
        if (attributeByGuid != null)
        {
          long int64 = Convert.ToInt64(attributeByGuid.Value);
          return sessionKeeper.Session.GetObject(int64) != null ? int64 : -1L;
        }
      }
    }
    return signedObject;
  }

  /// <summary>
  /// Вернуть список объектов подписей для объекта по связи "Подписи".
  /// </summary>
  /// <param name="objectId">идентификатор объекта, к которому привязаны подписи</param>
  /// <returns></returns>
  public List<long> GetSigns(long objectId)
  {
    return CertSheetProcessor.ExpandObjectsByRelation(objectId, CertSheetCache.SignsRelationTypeID, true);
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TechcardDocsPumper.TemplateUtils
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Diagnostics;
using Intermech.Document.Client;
using Intermech.Document.DBCore;
using Intermech.Document.Model;
using Intermech.Document.Model.ImportBlanks;
using Intermech.Expert;
using Intermech.Interfaces;
using Intermech.Interfaces.Document;
using Intermech.TechCard.Document.Interfaces.Configs.Structure;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TechcardDocsPumper;

internal static class TemplateUtils
{
  private static readonly IDictionary<string, ImDocumentData> _bankFileName2TemplateCache = (IDictionary<string, ImDocumentData>) new Dictionary<string, ImDocumentData>();

  public static bool GetTemplate(string blankFileName, out ImDocumentData template)
  {
    template = (ImDocumentData) null;
    if (TemplateUtils._bankFileName2TemplateCache.TryGetValue(blankFileName, out template))
      return true;
    try
    {
      template = (ImDocumentData) ImDocument.LoadFromOldBlank(blankFileName, out BlankLoader _);
    }
    catch (Exception ex)
    {
      throw new Exception($"Ошибка конвертирования бланка {blankFileName}\n {ex.Message}", ex);
    }
    if (template == null)
      return false;
    TemplateUtils._bankFileName2TemplateCache[blankFileName] = template;
    return true;
  }

  public static void WriteTemplateToDb([NotNull] IUserSession userSession, [NotNull] Rules rules)
  {
    if (rules.Template == null || rules.Template.DBObjectID != -1L)
      return;
    IDBObject templateDbObject = TemplateUtils.CreateTemplateDbObject(userSession, rules);
    TemplateUtils.SaveImDocumentObjectFile(ref templateDbObject, rules.Template as ImDocument, "", 0, false);
    rules.Template.Reference = (ReferenceBase) new ReferenceToDBObject((DocumentTreeNode) rules.Template, templateDbObject, false);
    if (!templateDbObject.IsCreationMode)
      return;
    templateDbObject.CommitCreation(true);
  }

  private static IDBObject CreateTemplateDbObject(IUserSession userSession, Rules rules)
  {
    int attributeId1 = MetaDataHelper.GetAttributeID((object) new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"));
    int attributeId2 = MetaDataHelper.GetAttributeID((object) new Guid("cad00020-306c-11d8-b4e9-00304f19f545"));
    int attributeId3 = MetaDataHelper.GetAttributeID((object) new Guid("cad00021-306c-11d8-b4e9-00304f19f545"));
    IDBObject templateDbObject = userSession.GetObjectCollection(ExpertConsts.Consts.objTechTemplate).Create();
    string initValue = "Миграция Techcard - " + rules.FullName;
    AttributeValues[] valuesList = new AttributeValues[4]
    {
      new AttributeValues(ExpertConsts.Consts.attrCaption, (object) initValue),
      new AttributeValues(attributeId3, (object) rules.BlankNote),
      new AttributeValues(attributeId1, (object) rules.ShortName),
      new AttributeValues(attributeId2, (object) initValue)
    };
    templateDbObject.SetAttributesValues(valuesList);
    templateDbObject.CommitCreation(true);
    return templateDbObject;
  }

  private static bool SaveImDocumentObjectFile(
    [NotNull] ref IDBObject dbObject,
    [NotNull] ImDocument document,
    string fileName,
    int fileIndex,
    bool isNewDocument)
  {
    object initValue = (object) null;
    fileName = ImDocumentData.ReplaceForbiddenSymbols(fileName);
    if (!string.IsNullOrEmpty(fileName))
    {
      string str = Path.GetExtension(fileName);
      if (str == fileName)
        fileName = nameof (document) + str;
    }
    else
      fileName = "document.imdx";
    IFileNamesService service = ServiceUtils.GetService<IFileNamesService>((object) dbObject.Session, false);
    if (service != null)
    {
      long[] objectIdByFileName = service.GetObjectIDByFileName(fileName, dbObject.Session.SessionGUID);
      bool flag = false;
      for (int index = 0; !flag && index < objectIdByFileName.Length; ++index)
        flag = Math.Abs(objectIdByFileName[index]) == Math.Abs(dbObject.ObjectID);
      if (!flag)
        fileName = service.GetUniqueFileName(fileName, dbObject.ID, dbObject.Session.SessionGUID);
    }
    switch (dbObject.ObjectModifyMode)
    {
      case ObjectModifyModes.Checkout:
        if (dbObject.ObjectID > 0L)
        {
          if (dbObject.CheckoutBy == 0L)
          {
            dbObject = dbObject.CheckOut();
            break;
          }
          if (dbObject.CheckoutBy != dbObject.Session.UserID)
            return false;
          dbObject = dbObject.Session.GetObject(-dbObject.ObjectID);
          break;
        }
        break;
      case ObjectModifyModes.CreateVersion:
        return false;
      case ObjectModifyModes.CantModify:
        return false;
    }
    IDBAttribute attributeById1 = dbObject.GetAttributeByID(DocIDCache.Attr_Pages);
    if (attributeById1 != null)
      attributeById1.Value = (object) document.NodesCount;
    Guid attrTypeGuid = new Guid("cad0004b-306c-11d8-b4e9-00304f19f545");
    int attributeTypeId1 = MetaDataHelper.GetAttributeTypeID(attrTypeGuid);
    int attributeTypeId2 = MetaDataHelper.GetAttributeTypeID(attrTypeGuid);
    if (dbObject.GetAttributeByID(attributeTypeId1) == null)
      dbObject.Attributes.AddAttribute(attributeTypeId1, false);
    if (!isNewDocument && document.SaveModificationDate)
    {
      IDBAttribute attributeById2 = dbObject.GetAttributeByID(attributeTypeId2);
      if (attributeById2 != null)
        initValue = attributeById2.Value;
    }
    MemoryStream ms = new MemoryStream();
    try
    {
      if (document.Reference != null)
        document.Reference.UpdateLink(true, true);
      document.SaveToXml((Stream) ms);
      ms.Position = 0L;
      byte[] data = ZlibHelper.PackBuffer((Stream) ms);
      IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545"));
      if (attributeByGuid != null)
      {
        if (attributeByGuid is IBlobWriter blobWriter)
        {
          BlobInformation blobInfo = new BlobInformation(ms.Length, (long) data.Length, DateTime.Now, fileName, ArcMethods.ZLibPacked, string.Empty);
          if (blobWriter.OpenBlob(blobInfo, false))
            blobWriter.WriteDataBlock(data);
        }
      }
    }
    finally
    {
      ms.Close();
    }
    if (!isNewDocument && document.SaveModificationDate)
      dbObject.SetAttributesValues(new AttributeValues[1]
      {
        new AttributeValues(attributeTypeId2, initValue)
      });
    return true;
  }
}

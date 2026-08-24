// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.DocLinksHelper
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal static class DocLinksHelper
{
  public static DocsLinks GetDocLinksForArticle(int artID)
  {
    using (IDataReader dataReader = BasePumpHelper.S4Query(PumpHelper.IsNewDocsLinksFormat ? "select TODOC_ID, USER_ID, DOC_VER_ID, ART_VER_ID, DEL_VER_ID from DOCSLINKS where ART_ID = @p1" : "select TODOC_ID, USER_ID from DOCSLINKS where ART_ID = @p1", (object) artID))
    {
      DocsLinks docLinksForArticle = (DocsLinks) null;
      if (dataReader.Read())
      {
        docLinksForArticle = new DocsLinks();
        do
        {
          int userID = dataReader.IsDBNull(1) ? -1 : BasePumpHelper.ToInt32(dataReader[1]);
          DocLink docLink;
          if (PumpHelper.IsNewDocsLinksFormat)
          {
            int num = dataReader.IsDBNull(2) ? -1 : BasePumpHelper.ToInt32(dataReader[2]);
            int artVerID = dataReader.IsDBNull(3) ? -1 : BasePumpHelper.ToInt32(dataReader[3]);
            int delVerID = dataReader.IsDBNull(4) ? -1 : BasePumpHelper.ToInt32(dataReader[4]);
            DocLinkEx docLinkEx = new DocLinkEx(BasePumpHelper.ToInt32(dataReader[0]), userID, artVerID, delVerID);
            docLinkEx.VerID = num;
            docLink = (DocLink) docLinkEx;
          }
          else
            docLink = new DocLink(BasePumpHelper.ToInt32(dataReader[0]), userID);
          docLinksForArticle.Add(docLink);
        }
        while (dataReader.Read());
      }
      return docLinksForArticle;
    }
  }

  public static long GetDocumentID(
    Dictionary<object, DictionaryValue> documentsCache,
    Dictionary<object, DictionaryValue> objectGuids,
    int docID,
    out long objectID,
    out int objectTypeID,
    out bool techcardDocument)
  {
    objectTypeID = -1;
    DictionaryValue dictionaryValue1;
    if (!documentsCache.TryGetValue((object) docID, out dictionaryValue1))
    {
      techcardDocument = false;
      objectID = 0L;
      return 0;
    }
    long newObjectId = dictionaryValue1.NewObjectID;
    DocumentTag tag = dictionaryValue1.Tag as DocumentTag;
    techcardDocument = tag.HasFlag(DocumentFlag.Techcard);
    Dictionary<int, long>.Enumerator enumerator = tag.Versions.GetEnumerator();
    enumerator.MoveNext();
    objectID = enumerator.Current.Value;
    DictionaryValue dictionaryValue2;
    if (objectGuids.TryGetValue((object) objectID, out dictionaryValue2))
      objectTypeID = ((ObjectInfo) dictionaryValue2.Tag).ObjectType;
    return dictionaryValue1.NewObjectID;
  }
}

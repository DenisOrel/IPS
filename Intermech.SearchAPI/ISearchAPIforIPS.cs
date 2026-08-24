// Decompiled with JetBrains decompiler
// Type: Intermech.SearchAPI.ISearchAPIforIPS
// Assembly: Intermech.SearchAPI, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D1D502F5-7810-48B3-B639-4FF6D7A8DD6F
// Assembly location: D:\IPS\Client\Intermech.SearchAPI.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.SearchAPI;

[ComVisible(true)]
[Guid("C6760AFF-39C5-49D5-806A-CAC54DA5A094")]
[InterfaceType(ComInterfaceType.InterfaceIsDual)]
public interface ISearchAPIforIPS
{
  int ErrorCode { get; }

  string ErrorMessage { get; }

  int APIVersion { get; }

  void OpenArtDocuments(long ArtID, int DocTypeID);

  void CloseArtDocuments();

  int GetDocumentsCount();

  long GetArtDocumentID(int i);

  void OpenDocument(long DocID);

  void CloseDocument();

  string GetDocTypeName();

  string GetFieldValue(string fldName);

  void SetFieldValue(string fldName, string fldValue);

  string PrepareDocOwnersList(long DocID);

  string PrepareDocRefsList(long DocID);

  bool OpenInNewWindow(long ObjectID);

  void Minimize();

  void Restore();

  void Edit();

  void View();

  string GetComposition(long ObjectID, long SchemeID);

  void SelectDocs();

  int SelectedDocsCount();

  long GetSelectedDocID(int i);

  int GetFieldCount();

  string GetFieldName(int fldNo);

  void GetDocIDVersionIDbyFileName(string aFileName, out long aDocID, out long aVersionID);

  bool EditParameters2();

  void SaveChanges();

  long LogFileInArchive(string fileName);

  string GetDocWorkCopyPath();

  void CheckOut();

  void CheckIn();

  long FindArticle(string aDesignatio, string aName, string aOKP_Code);

  long AddNewArticle2(string aDesignation, string aOKPCode, string aName, string aSectionID);

  long AddBOMItem(
    long ProjAID,
    long PartAID,
    string CountPC,
    string Razdel,
    string Position,
    string Note);

  void DeleteAllBOMItems(long ProjAID);

  void OpenBOMItem(long prjLinkID);

  void SetFieldValue_BOM(string fldName, string fldValue);

  string GetFieldValue_BOM(string fldName);

  long LinkDocToArticle(long ArtID, long DocID, int LinkType, int LinkToIsp);

  void OpenUrl(string url);

  long GetObjectBaseVersionByID(long ID);

  long ActiveObjectID();

  string FindObjects(string ObjectTypeGuid, string AttributeName, string AttributeValue);
}

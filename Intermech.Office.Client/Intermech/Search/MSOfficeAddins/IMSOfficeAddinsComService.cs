// Decompiled with JetBrains decompiler
// Type: Intermech.Search.MSOfficeAddins.IMSOfficeAddinsComService
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.Search.MSOfficeAddins;

[ComVisible(true)]
[Guid("238BC815-1250-49DF-97E4-DAC8A98D6A7A")]
public interface IMSOfficeAddinsComService
{
  void CheckInDocument(string fileName);

  void CheckOutDocument(string fileName);

  Tuple<string, string>[] CreateObjectReference(string fileName);

  bool IsDocumentCheckedOut(string fileName);

  bool IsDocumentRegistered(string fileName);

  void OpenDocumentComposition(string fileName, string objectUrl);

  string RegisterDocument(string fileName);

  void SaveDocument(string fileName);

  string SelectAndPublishDocument(params string[] allowableFilesExtensions);

  void ShowDocumentCard(string fileName);

  Dictionary<string, Tuple<string, string>> UpdateObjectReferences(
    string fileName,
    string[] objectsUrls);

  bool IsDocumentInViewArea(string fileName);
}

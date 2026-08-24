// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.InternalDocument
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Interfaces;
using Intermech.Office.Interfaces;

#nullable disable
namespace Intermech.Office.Client;

internal class InternalDocument : OfficeDocument
{
  public InternalDocument()
    : base(OfficeDocumentTypes.Internal)
  {
  }

  protected override bool OnRegisterDocument(
    IUserSession session,
    IDBObject document,
    IDBObject parentDocument)
  {
    return true;
  }
}

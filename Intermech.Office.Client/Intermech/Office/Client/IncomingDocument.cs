// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.IncomingDocument
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;

#nullable disable
namespace Intermech.Office.Client;

internal class IncomingDocument : OfficeDocument
{
  public IncomingDocument()
    : base(OfficeDocumentTypes.Incoming)
  {
  }

  protected override bool OnRegisterDocument(
    IUserSession session,
    [NotNull] IDBObject document,
    IDBObject parentDocument)
  {
    document.Attributes.AddAttribute(OfficeConsts.AttrAddresserID, false);
    document.Attributes.AddAttribute(OfficeConsts.AttrOutgoingRegNumberID, false);
    document.Attributes.AddAttribute(OfficeConsts.AttrSignatoryID, false);
    return true;
  }
}

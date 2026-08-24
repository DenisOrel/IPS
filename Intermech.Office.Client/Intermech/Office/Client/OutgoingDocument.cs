// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OutgoingDocument
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;

#nullable disable
namespace Intermech.Office.Client;

internal class OutgoingDocument : OfficeDocument
{
  public OutgoingDocument()
    : base(OfficeDocumentTypes.Outgoing)
  {
  }

  protected override bool OnRegisterDocument(
    IUserSession session,
    [NotNull] IDBObject document,
    [CanBeNull] IDBObject parentDocument)
  {
    if (parentDocument != null)
    {
      IDBAttribute attributeById = parentDocument.GetAttributeByID(OfficeConsts.AttrAddresserID);
      if (attributeById != null && attributeById.AsInteger != 0L)
      {
        if (document.GetAttributeByID(OfficeConsts.AttrAddresseesID) == null)
          document.Attributes.AddAttribute(OfficeConsts.AttrAddresseesID, false, new object[1]
          {
            (object) attributeById.AsInteger
          });
        (document.GetAttributeByID(OfficeConsts.AttrDocRecipientID) ?? document.Attributes.AddAttribute(OfficeConsts.AttrDocRecipientID, false)).AsString = string.Empty;
        (document.GetAttributeByID(OfficeConsts.AttrInputRegNumID) ?? document.Attributes.AddAttribute(OfficeConsts.AttrInputRegNumID, false)).AsString = string.Empty;
        if (document.GetAttributeByID(OfficeConsts.AttrAddresseeRegDateID) == null)
          document.Attributes.AddAttribute(OfficeConsts.AttrAddresseeRegDateID, false);
      }
    }
    return true;
  }
}

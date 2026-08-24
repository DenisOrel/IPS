// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeDocument
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;

#nullable disable
namespace Intermech.Office.Client;

internal abstract class OfficeDocument
{
  protected OfficeDocumentTypes _DocumentType;

  protected OfficeDocument(OfficeDocumentTypes type) => this._DocumentType = type;

  public bool RegisterNewDocument(
    [NotNull] IUserSession session,
    [NotNull] IDBObject document,
    long parentDocumentID,
    bool designationEqualRegNumber)
  {
    OfficeDocumentTypeSettings settings = session.GetCustomService<IOfficeDocumentTypeService>().GetSettings(session.SessionGUID, document.ObjectType);
    bool flag = false;
    if (settings.EnableEmptyRegNumbers != null)
      settings.EnableEmptyRegNumbers.TryGetValue(this._DocumentType, out flag);
    if (!flag)
    {
      string registrationNumber = OfficeClientHelper.GetRegistrationNumber(session, document.ObjectID, document.ObjectType, this._DocumentType);
      OfficeClientHelper.AddRegistrationAttributes(document, registrationNumber, designationEqualRegNumber);
    }
    OfficeClientHelper.SetTypeOfficeDocuments(document, this._DocumentType);
    IDBObject parentDocument = (IDBObject) null;
    if (parentDocumentID != 0L)
    {
      parentDocument = session.GetObject(parentDocumentID);
      IDBObjectType objectType = session.GetObjectType(document.ObjectType, true);
      if (objectType.CaptionAttribute != 0)
        (document.GetAttributeByID(objectType.CaptionAttribute) ?? document.Attributes.AddAttribute(objectType.CaptionAttribute, false)).AsString = "Re:" + parentDocument.Caption;
      else
        document.Caption = "Re:" + parentDocument.Caption;
    }
    return this.OnRegisterDocument(session, document, parentDocument);
  }

  protected abstract bool OnRegisterDocument(
    [NotNull] IUserSession session,
    [NotNull] IDBObject document,
    [CanBeNull] IDBObject parentDocument);
}

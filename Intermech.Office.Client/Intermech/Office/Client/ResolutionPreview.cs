// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ResolutionPreview
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Office.Interfaces;

#nullable disable
namespace Intermech.Office.Client;

internal sealed class ResolutionPreview
{
  public static void OnExtend([CanBeNull] ExtendEventArgs eventArgs)
  {
    if (eventArgs == null || eventArgs.ObjectID == -1L || eventArgs.ObjectID == 0L)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long officeDocument = OfficeHelper.FindOfficeDocument(sessionKeeper.Session, eventArgs.ObjectID);
      if (officeDocument == 0L)
        return;
      IDBObject dbObject = sessionKeeper.Session.GetObject(officeDocument);
      int attributeTypeId = MetaDataHelper.GetAttributeTypeID("cad0004b-306c-11d8-b4e9-00304f19f545");
      IDBAttribute attributeById = dbObject.GetAttributeByID(attributeTypeId);
      if (attributeById == null)
        return;
      bool flag = false;
      for (int valueIndex = 0; valueIndex < attributeById.ValuesCount; ++valueIndex)
      {
        attributeById.Index = valueIndex;
        if (!attributeById.IsNull)
        {
          FileBlobItem fileBlobItem = new FileBlobItem(dbObject.ObjectID, attributeTypeId, valueIndex);
          eventArgs.Items.Add(fileBlobItem);
          flag = true;
        }
      }
      if (!flag)
        return;
      eventArgs.PreferedBlobID = attributeById.AsInteger;
    }
  }
}

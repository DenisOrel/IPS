// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ReceiptContentView
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.IO;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace Intermech.Site.Client;

[ViewDescriptionProvider(typeof (ReceiptContentView.ReceiptContentViewDescriptionProvider))]
internal sealed class ReceiptContentView : ReceiptContentBaseView
{
  private long _receiptID;

  protected override void OnInitialize(ISelectedItems items, IServiceProvider provider)
  {
    if (!(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
      return;
    this._receiptID = itemData.ObjectID;
  }

  protected override DataTable GetReceiptContent(
    IUserSession session,
    out string caption,
    out DateTime createDate)
  {
    caption = string.Empty;
    createDate = DateTime.Now;
    if (this._receiptID == 0L)
      return (DataTable) null;
    IDBObject dbObject = session.GetObject(this._receiptID);
    IDBAttribute attributeByGuid1 = dbObject.GetAttributeByGuid(PortalConsts.attributeReceiptFile);
    if (attributeByGuid1 == null || attributeByGuid1.IsNull)
      return (DataTable) null;
    IBlobReader blobReader = attributeByGuid1 as IBlobReader;
    BlobInformation blobInformation = blobReader.OpenBlob(0);
    if (blobInformation.RealFileSize <= 0L)
      return (DataTable) null;
    byte[] buffer = blobReader.ReadDataBlock(0);
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    DataTable receiptContent;
    using (Stream stream = (Stream) new MemoryStream(buffer))
    {
      stream.Position = 0L;
      if (blobInformation.ArcMethod == ArcMethods.ZLibPacked)
      {
        using (ImChunkedStream imChunkedStream = new ImChunkedStream())
        {
          ZLibStreamHelper.UnpackStream(stream, (Stream) imChunkedStream);
          imChunkedStream.Position = 0L;
          receiptContent = (DataTable) binaryFormatter.Deserialize((Stream) imChunkedStream);
        }
      }
      else
        receiptContent = (DataTable) binaryFormatter.Deserialize(stream);
    }
    IDBAttribute attributeByGuid2 = dbObject.GetAttributeByGuid(PortalConsts.attributeReceiptCreateDate);
    createDate = attributeByGuid2.AsDateTime;
    IDBAttribute attributeByGuid3 = dbObject.GetAttributeByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545"));
    caption = attributeByGuid3.AsString;
    return receiptContent;
  }

  private sealed class ReceiptContentViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      if (!(serviceProvider.GetService(typeof (INamedImageList)) is INamedImageList service))
        service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
      INamedImageList namedImageList = service;
      return new ViewDescription()
      {
        Caption = "Просмотр",
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex("imgView") : -1,
        OrderID = 40
      };
    }
  }
}

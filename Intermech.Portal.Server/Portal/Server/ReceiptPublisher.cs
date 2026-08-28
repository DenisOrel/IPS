// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.ReceiptPublisher
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server.WebPortal;
using Intermech.Interfaces.WebPortal;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal class ReceiptPublisher(
  IUserSession session,
  TransferedObject unit,
  XmlDocument xmlDocument,
  XmlNode rootNode,
  SiteInfo info,
  IDBImporter importer) : UnitPublisher(session, unit, xmlDocument, rootNode, info, 1)
{
  public override Guid Publish(
    IDBObjectCollection publishObjects,
    string enabledSites,
    GroupPublishItem item,
    PackAnalyzInfo packAnalyzInfo,
    PublishCaches caches,
    IDBRelationCollection relCollection,
    IDBRelationType relTypePublish)
  {
    ObjectInfo objectAttributes = AttributesFile.GetObjectAttributes(this.rootNode);
    IDBObject dbObject = this.session.GetObjectCollection(PortalConsts.objtypeReceipt).Create(objectAttributes.ObjectGuid);
    for (int i = 0; i < this.rootNode.ChildNodes.Count; ++i)
    {
      XmlNode childNode1 = this.rootNode.ChildNodes[i];
      if (childNode1.Name == PortalConsts.XmlNodeAttribute && childNode1.ChildNodes.Count > 0 && childNode1.ChildNodes[0].Name == PortalConsts.XmlNodeValueAttribute)
      {
        AttributeInfo attributeInfo = AttributesFile.GetAttributeInfo(childNode1);
        IDBAttribute dbAttribute = dbObject.GetAttributeByGuid(new Guid(attributeInfo.Guid)) ?? dbObject.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(attributeInfo.Guid), false);
        XmlNode childNode2 = childNode1.ChildNodes[0];
        switch (attributeInfo.FieldType)
        {
          case FieldTypes.ftString:
          case FieldTypes.ftGuid:
            dbAttribute.AsString = childNode2.Attributes["F_STRING_VALUE"].Value;
            continue;
          case FieldTypes.ftInteger:
            dbAttribute.AsInteger = Convert.ToInt64(childNode2.Attributes["F_INTEGER_VALUE"].Value);
            continue;
          case FieldTypes.ftDateTime:
            dbAttribute.AsDateTime = Convert.ToDateTime(childNode2.Attributes["F_DATE_VALUE"].Value, (IFormatProvider) CultureInfo.InvariantCulture);
            continue;
          case FieldTypes.ftFile:
          case FieldTypes.ftBlob:
            ValueInfo valueInfo = UnitXmlFile.GetValueInfo(childNode2, 0);
            if (valueInfo.FileName != string.Empty)
            {
              if (valueInfo.IntValue > 0L)
              {
                using (FileStream fileStream = new FileStream(Path.Combine(this.directoryName, valueInfo.FileName), FileMode.Open))
                {
                  IBlobWriter blobWriter = dbAttribute as IBlobWriter;
                  blobWriter.OpenBlob(new BlobInformation(valueInfo.IntValue, fileStream.Length, valueInfo.DateValue, valueInfo.FileName, valueInfo.ArcMethod, valueInfo.StringValue), false);
                  byte[] numArray1 = new byte[Consts.BlobTransferBufferLength];
                  int length;
                  while ((length = fileStream.Read(numArray1, 0, Consts.BlobTransferBufferLength)) > 0)
                  {
                    byte[] numArray2 = new byte[length];
                    Array.Copy((Array) numArray1, (Array) numArray2, length);
                    blobWriter.WriteDataBlock(numArray2);
                  }
                  continue;
                }
              }
              (dbAttribute as IBlobWriter).OpenBlob(new BlobInformation(0L, 0L, valueInfo.DateValue, valueInfo.FileName, ArcMethods.NotPacked, valueInfo.StringValue), true);
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
    PublishPacket packet = item as PublishPacket;
    dbObject.GetAttributeByGuid(PortalConsts.attributeFirstPublishSite).AsString = this.info.Code.ToString();
    IDBAttribute dbAttribute1 = dbObject.GetAttributeByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")) ?? dbObject.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID("cad00020-306c-11d8-b4e9-00304f19f545"), false);
    if (string.IsNullOrEmpty(dbAttribute1.AsString))
    {
      ReceiptTypes asInteger = (ReceiptTypes) dbObject.GetAttributeByGuid(PortalConsts.attributeReceiptType).AsInteger;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(asInteger == ReceiptTypes.Export ? "Публикация пакета " : "Импорт пакета ");
      stringBuilder.AppendFormat(Packet4Publish.Caption(packet.Designation, packet.Name, packet.DBObject.ObjectGUID));
      dbAttribute1.AsString = stringBuilder.ToString();
    }
    dbObject.CommitCreation(true);
    this.session.GetRelationCollection(this.session.IdentHelper.SimpleRelationTypeID).Create(item.DBObject.ObjectID, dbObject.ObjectID);
    PublishHelper.AddUnitFilesToPacket(packet, this.unit, this.UnitTempDirectory);
    return dbObject.ObjectGUID;
  }
}

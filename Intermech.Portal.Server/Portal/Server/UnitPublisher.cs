// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.UnitPublisher
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.Server.WebPortal;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using Intermech.Kernel.Services.PortalServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal abstract class UnitPublisher : IUnitPublisher
{
  protected string directoryName;
  protected TransferedObject unit;
  protected XmlDocument xmlDocument;
  protected XmlNode rootNode;
  protected SiteInfo info;
  protected IUserSession session;
  protected IEventLogHelper eventLog;
  protected int categoryType;

  public UnitPublisher(
    IUserSession session,
    TransferedObject unit,
    XmlDocument xmlDocument,
    XmlNode rootNode,
    SiteInfo info,
    int categoryType)
  {
    this.unit = unit;
    this.xmlDocument = xmlDocument;
    this.rootNode = rootNode;
    this.info = info;
    this.session = session;
    this.eventLog = (this.session as UserSession).EventLogHelper;
    this.directoryName = TempStorage.GetPublishUnitPath(unit.GUID);
    this.categoryType = categoryType;
  }

  public static bool IsForbiddenAttribute(AttributeInfo attrInfo)
  {
    if (!string.IsNullOrEmpty(attrInfo.Guid))
    {
      if (!GuidHelper.IsGuid(attrInfo.Guid))
        return false;
      return Array.IndexOf<Guid>(PublishRulesService.ForbiddenAttributes, new Guid(attrInfo.Guid)) >= 0 || new Guid(attrInfo.Guid).Equals(new Guid("cad0013a-306c-11d8-b4e9-00304f19f545"));
    }
    return string.IsNullOrEmpty(attrInfo.Name) || Array.IndexOf<string>(PublishRulesService.ForbiddenAttributeNames, attrInfo.Name) >= 0 || attrInfo.Name.Equals(PublishRulesService.AttributeContentModifyDateName);
  }

  protected void ParceXMLIntoAttributable(
    IUserSession session,
    IDBAttributable dbAttributable,
    IDBAttributableType typeAttributes,
    XmlDocument xmlDocument,
    XmlNode rootNode,
    string iPath,
    Dictionary<Guid, long> importedObjectsIDs,
    ref List<Guid> currentLinks)
  {
    if (TraceLog.Enabled)
      TraceLog.Write("...add attributes from xml");
    IDBAttribute attributeByGuid = dbAttributable.GetAttributeByGuid(PortalServerConsts.attributeFile, true);
    if (attributeByGuid.ValuesCount > 0)
      attributeByGuid.ClearValues();
    attributeByGuid.Index = 0;
    IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
    using (MemoryStream memoryStream = new MemoryStream())
    {
      xmlDocument.Save((Stream) memoryStream);
      memoryStream.Position = 0L;
      using (MemoryStream outStream = new MemoryStream())
      {
        service.PackStream((Stream) outStream, (Stream) memoryStream, 9);
        IBlobWriter blobWriter = attributeByGuid as IBlobWriter;
        blobWriter.OpenBlob(new BlobInformation(memoryStream.Length, outStream.Length, DateTime.Now, PortalConsts.AttributesXmlFileName, ArcMethods.ZLibPacked, string.Empty), false);
        blobWriter.WriteDataBlock(outStream.ToArray());
      }
    }
    List<AttributeValues> attributeValuesList = new List<AttributeValues>();
    for (int i1 = 0; i1 < rootNode.ChildNodes.Count; ++i1)
    {
      XmlNode childNode = rootNode.ChildNodes[i1];
      if (childNode.Name == PortalConsts.XmlNodeSysAttribute)
      {
        if (childNode.Attributes["F_OWNER_ID"] != null && GuidHelper.IsGuid(childNode.Attributes["F_OWNER_ID"].Value))
        {
          Guid guid = new Guid(childNode.Attributes["F_OWNER_ID"].Value);
          if (!currentLinks.Contains(guid))
            currentLinks.Add(guid);
        }
      }
      else if (childNode.Name == PortalConsts.XmlNodeAttribute && childNode.ChildNodes.Count > 0 && childNode.ChildNodes[0].Name == PortalConsts.XmlNodeValueAttribute)
      {
        AttributeInfo attributeInfo = AttributesFile.GetAttributeInfo(childNode);
        if (!UnitPublisher.IsForbiddenAttribute(attributeInfo))
        {
          if (Array.IndexOf<FieldTypes>(PortalConsts.EnabledFieldTypes, attributeInfo.FieldType) >= 0)
          {
            IDBAttributeType attrType = (IDBAttributeType) null;
            if (GuidHelper.IsGuid(attributeInfo.Guid))
              attrType = session.GetAttributeType(new Guid(attributeInfo.Guid), false);
            if (attrType == null)
              attrType = session.GetAttributeType(attributeInfo.Name, false);
            if (attrType != null && (attrType.MultipleValued == MultiValueModes.SingleValue || attrType.MultipleValued == MultiValueModes.SingleValueFromList) && attrType.IsCompatibleType(attributeInfo.FieldType) && typeAttributes.Attributes.GetAttributeByID(attrType.AttributeID, false) != null)
            {
              AttributeValues attributeValues = attributeValuesList.Find((Predicate<AttributeValues>) (av => av.AttributeID == attrType.AttributeID));
              if (attributeValues == null)
              {
                attributeValues = new AttributeValues(attrType.AttributeID, attrType.AttributeType, attrType.MultipleValued, attrType.Computed)
                {
                  ThrowSetException = false
                };
                attributeValuesList.Add(attributeValues);
              }
              attributeValues.Values = new object[1]
              {
                AttributesFile.ParceValue(session, attrType, AttributesFile.GetAttributeValue(childNode.ChildNodes[0]), attributeInfo, iPath, false)
              };
              if (TraceLog.Enabled)
                TraceLog.Write($"...add attribute {attrType.Name} to array");
            }
          }
          else if (attributeInfo.FieldType == FieldTypes.ftBlob || attributeInfo.FieldType == FieldTypes.ftFile || attributeInfo.FieldType == FieldTypes.ftMemo || attributeInfo.FieldType == FieldTypes.ftShortBlob)
          {
            for (int index = 0; index < childNode.ChildNodes.Count; ++index)
            {
              ValueInfo valueInfo = UnitXmlFile.GetValueInfo(childNode.ChildNodes[index], index);
              if (valueInfo.FileName != string.Empty)
              {
                if (TraceLog.Enabled)
                  TraceLog.Write($"...add blob {valueInfo.FileName}");
                attributeByGuid.AddValue((object) null);
                if (valueInfo.IntValue > 0L)
                {
                  using (FileStream fileStream = new FileStream(Path.Combine(iPath, valueInfo.FileName), FileMode.Open))
                  {
                    IBlobWriter blobWriter = attributeByGuid as IBlobWriter;
                    blobWriter.OpenBlob(new BlobInformation(valueInfo.IntValue, fileStream.Length, valueInfo.DateValue, valueInfo.FileName, valueInfo.ArcMethod, valueInfo.StringValue), false);
                    byte[] numArray1 = new byte[Consts.BlobTransferBufferLength];
                    int length;
                    while ((length = fileStream.Read(numArray1, 0, Consts.BlobTransferBufferLength)) > 0)
                    {
                      byte[] numArray2 = new byte[length];
                      Array.Copy((Array) numArray1, (Array) numArray2, length);
                      blobWriter.WriteDataBlock(numArray2);
                    }
                  }
                }
                else
                  (attributeByGuid as IBlobWriter).OpenBlob(new BlobInformation(0L, 0L, valueInfo.DateValue, valueInfo.FileName, ArcMethods.NotPacked, valueInfo.StringValue), true);
              }
            }
          }
          if (attributeInfo.FieldType == FieldTypes.ftMeasured || attributeInfo.FieldType == FieldTypes.ftObjectLink || attributeInfo.FieldType == FieldTypes.ftObjectLinkByID || attributeInfo.FieldType == FieldTypes.ftInteger)
          {
            for (int i2 = 0; i2 < childNode.ChildNodes.Count; ++i2)
            {
              AttributeValue attributeValue = AttributesFile.GetAttributeValue(childNode.ChildNodes[i2]);
              if (GuidHelper.IsGuid(attributeValue.GuidValue))
              {
                Guid guid = attributeInfo.FieldType != FieldTypes.ftObjectLinkByID ? new Guid(attributeValue.GuidValue) : new Guid(attributeValue.Description);
                if (!currentLinks.Contains(guid))
                  currentLinks.Add(guid);
              }
            }
          }
        }
      }
    }
    if (attributeValuesList.Count <= 0)
      return;
    dbAttributable.SetAttributesValues(attributeValuesList.ToArray());
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write("...set attributes from array");
  }

  protected void SetLinksAttribute(
    IUserSession session,
    IDBAttributable attributable,
    List<Guid> currentLinks)
  {
    IDBAttribute dbAttribute = attributable.GetAttributeByID(IDHelper.AttributePublishGuidLinksID);
    if (dbAttribute == null)
    {
      if (currentLinks.Count > 0)
        dbAttribute = attributable.Attributes.AddAttribute(IDHelper.AttributePublishGuidLinksID, false);
    }
    else if (currentLinks.Count == 0)
      dbAttribute.Delete(0L);
    else if (dbAttribute.ValuesCount > 0)
      dbAttribute.ClearValues();
    if (currentLinks.Count > 0)
    {
      for (int index = 0; index < currentLinks.Count; ++index)
      {
        if (index == 0)
          dbAttribute.Value = (object) currentLinks[index];
        else
          dbAttribute.AddValue((object) currentLinks[index]);
      }
    }
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write("...attributePublishLinks set values");
  }

  protected void AddEvent(ActionType actionType, long categoryID, string objectName, string note)
  {
    this.eventLog.AddEvent(this.categoryType == 1 ? categoryID : 0L, this.categoryType == 5 ? categoryID : 0L, this.categoryType, categoryID, objectName, note, actionType, EventlogRecordType.Information, this.session.UserID, this.info.Code.ToString(), this.session);
  }

  protected void AddEvent(long categoryID, string objectName, string note)
  {
    this.AddEvent(ActionType.PublishedOnPortal, categoryID, objectName, note);
  }

  public static IUnitPublisher GetPublisher(
    IUserSession session,
    out TransferedObject unit,
    string unitFilePath,
    SiteInfo info,
    IDBImporter importer,
    GroupPublishItem packet)
  {
    XmlDocument xmlDocument = new XmlDocument();
    XmlNode info1 = UnitXmlFile.GetInfo(session, out unit, unitFilePath, xmlDocument);
    if (TraceLog.Enabled)
      TraceLog.Write($"Start publish unit TransferedObject.Guid={unit.GUID} TransferedObject.Category={unit.Category}");
    IUnitPublisher publisher = (IUnitPublisher) null;
    switch (unit.Category)
    {
      case TransferedObjectCategory.Object:
      case TransferedObjectCategory.ObjectLink:
        publisher = packet == null || !(packet is PublishPacket) ? (IUnitPublisher) new ObjectPublisher(session, unit, xmlDocument, info1, info, importer) : (IUnitPublisher) new PacketObjectPublisher(session, unit, xmlDocument, info1, info, importer);
        break;
      case TransferedObjectCategory.Relation:
        publisher = packet == null || !(packet is PublishPacket) ? (IUnitPublisher) new RelationPublisher(session, unit, xmlDocument, info1, info) : (IUnitPublisher) new PacketRelationPublisher(session, unit, xmlDocument, info1, info);
        break;
      case TransferedObjectCategory.AutoTransfer:
        if (packet != null && packet is PublishPacket)
        {
          publisher = (IUnitPublisher) new PacketAutoTransferPublisher(session, unit, xmlDocument, info1, info);
          break;
        }
        break;
      case TransferedObjectCategory.AttributesContainer:
        publisher = packet == null || !(packet is PublishPacket) ? (IUnitPublisher) new AttributesContainerPublisher(session, unit, xmlDocument, info1, info) : (IUnitPublisher) new PacketAttributesContainerPublisher(session, unit, xmlDocument, info1, info);
        break;
      case TransferedObjectCategory.GroupObject:
        publisher = (IUnitPublisher) new GroupObjectPublisher(session, unit, info);
        break;
      case TransferedObjectCategory.GroupRelation:
        publisher = (IUnitPublisher) new GroupRelationPublisher(session, unit, info);
        break;
      case TransferedObjectCategory.Receipt:
        if (packet != null && packet is PublishPacket)
        {
          publisher = (IUnitPublisher) new ReceiptPublisher(session, unit, xmlDocument, info1, info, importer);
          break;
        }
        break;
      case TransferedObjectCategory.IncompleteRelation:
        publisher = (IUnitPublisher) new IncompleteRelationPublisher(session, unit, info);
        break;
    }
    return publisher;
  }

  public string UnitTempDirectory => this.directoryName;

  public abstract Guid Publish(
    IDBObjectCollection publishObjects,
    string enabledSites,
    GroupPublishItem item,
    PackAnalyzInfo packAnalyzInfo,
    PublishCaches caches,
    IDBRelationCollection relCollection,
    IDBRelationType relTypePublish);
}

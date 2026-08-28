// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.PacketAction
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.IO;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class PacketAction : PortalAction
{
  public byte[] GetPacketContent(Guid sessionGuid, long packetID)
  {
    TraceLog.Write($"Start GetPacketContent: sessionGuid={sessionGuid} packetID={packetID}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    long publicationReceipt = this.GetPublicationReceipt(userSession, packetID);
    return publicationReceipt == 0L ? (byte[]) null : this.GetReceiptContent(userSession, publicationReceipt);
  }

  private long GetPublicationReceipt(IUserSession session, long packetID)
  {
    DataTable dataTable = session.GetRelationCollection(session.IdentHelper.SimpleRelationTypeID).ConsistFrom(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeReceiptType), RelationalOperators.Equal, (object) 0, (object) null, LogicalOperators.AND, 0, false, AttributeSourceTypes.Object)
    }, new object[1]{ (object) -2 }), packetID);
    return dataTable.Rows.Count <= 0 ? 0L : Convert.ToInt64(dataTable.Rows[0][0]);
  }

  public void ImportComplete(Guid sessionGuid, long packetID)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start PacketImportComplete: sessionGuid={sessionGuid} packet={packetID}");
    IDBObject dbObject = this.GetUserSession(sessionGuid).GetObject(packetID, false);
    if (dbObject == null)
    {
      if (!TraceLog.Enabled)
        return;
      TraceLog.Write($"PacketImportComplete: packet={packetID} not found!");
    }
    else
    {
      (dbObject.GetAttributeByGuid(PortalServerConsts.attributePacketStatus, false) ?? dbObject.Attributes.AddAttribute(PortalServerConsts.attributePacketStatus, false)).AsInteger = 1L;
      if (!TraceLog.Enabled)
        return;
      TraceLog.Write($"End OnPacketImportComplete: packet={packetID}");
    }
  }

  public void DeletePackets(Guid sessionGuid, long[] packetIDs)
  {
    this.DeletePackets(this.GetUserSession(sessionGuid), packetIDs, true, out int _, out int _);
  }

  internal void DeletePackets(
    IUserSession session,
    long[] packetIDs,
    bool checkOwner,
    out int deletedPacketCount,
    out int deletedReceiptCount)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start DeletePackets: sessionGuid={session.SessionGUID} packetIDs count={packetIDs.Length}");
    deletedPacketCount = 0;
    deletedReceiptCount = 0;
    SiteInfo siteInfo = this.GetSiteInfo(session);
    IDBRelationCollection relationCollection = session.GetRelationCollection(session.IdentHelper.SimpleRelationTypeID);
    IDBTransactions service = ServiceUtils.GetService<IDBTransactions>((object) session, true);
    foreach (long packetId in packetIDs)
    {
      IDBObject dbObject1 = session.GetObject(packetId, true);
      if (checkOwner && dbObject1.GetAttributeByGuid(PortalConsts.attributeFirstPublishSite).AsString != siteInfo.Code.ToString())
        throw new Exception($"Ошибка удаления \"{dbObject1.NameInMessages}\". Удалить пакет может только пользователь узла, на котором был сформирован пакет.");
      service.StartTransaction();
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) relationCollection.ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new object[1]
        {
          (object) -2
        }), dbObject1.ObjectID).Rows)
        {
          IDBObject dbObject2 = session.GetObject(Convert.ToInt64(row[0]), true);
          if (TraceLog.Enabled)
            TraceLog.Write($"..delete receipt {dbObject2.ObjectID}");
          dbObject2.Delete(0L);
          ++deletedReceiptCount;
        }
        if (TraceLog.Enabled)
          TraceLog.Write($"..delete packet {dbObject1.ObjectID}");
        dbObject1.Delete(0L);
        ++deletedPacketCount;
        service.Commit();
        if (TraceLog.Enabled)
          TraceLog.Write("End DeletePacket");
      }
      catch
      {
        service.Rollback();
        throw;
      }
    }
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write("End DeletePackets");
  }

  private byte[] GetReceiptContent(IUserSession session, long receiptID)
  {
    TraceLog.Write($"Start GetReceiptContent: receiptID={receiptID}");
    IBlobReader attributeByGuid = session.GetObject(receiptID).GetAttributeByGuid(PortalConsts.attributeReceiptFile) as IBlobReader;
    attributeByGuid.OpenBlob(0);
    try
    {
      byte[] receiptContent = attributeByGuid.ReadDataBlock();
      TraceLog.Write($"...data length ={receiptContent.Length}");
      return receiptContent;
    }
    finally
    {
      attributeByGuid.CloseBlob();
    }
  }

  public byte[] GetReceiptContent(Guid sessionGuid, long receiptID)
  {
    TraceLog.Write($"Start GetReceipts: sessionGuid={sessionGuid} receiptID={receiptID}");
    return this.GetReceiptContent(this.GetUserSession(sessionGuid), receiptID);
  }

  public PublicationReceipt[] GetImportReceipts(Guid sessionGuid, long packetID)
  {
    TraceLog.Write($"Start GetReceipts: sessionGuid={sessionGuid} packetID={packetID}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    DataTable dataTable = userSession.GetRelationCollection(userSession.IdentHelper.SimpleRelationTypeID).ConsistFrom(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeReceiptType), RelationalOperators.Equal, (object) 1, (object) null, LogicalOperators.NONE, 0, false, AttributeSourceTypes.Object)
    }, new ColumnDescriptor[6]
    {
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) PortalConsts.attributeReceiptCreator, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) PortalConsts.attributeReceiptCreateDate, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.ASC, 1),
      new ColumnDescriptor((object) PortalConsts.attributeProcessID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) PortalConsts.attributeActionID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) PortalConsts.attributeFirstPublishSite, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0)
    }), packetID);
    List<PublicationReceipt> publicationReceiptList = new List<PublicationReceipt>(dataTable.Rows.Count);
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      PublicationReceipt publicationReceipt = new PublicationReceipt(Convert.ToInt64(row[0]), ReceiptTypes.Import, Convert.ToDateTime(row[2]), Convert.ToString(row[1]), Convert.ToChar(row[5]), (byte[]) null);
      if (row[3] != DBNull.Value)
        publicationReceipt.ProcessID = Convert.ToInt64(row[3]);
      if (row[4] != DBNull.Value)
        publicationReceipt.ActionID = Convert.ToInt64(row[4]);
      publicationReceiptList.Add(publicationReceipt);
    }
    TraceLog.Write("End GetReceipts");
    return publicationReceiptList.ToArray();
  }

  public void ImportPackets(Guid sessionGuid, Guid updateGuid, long[] packetIDs)
  {
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    this.ImportPackets(userSession, siteInfo, updateGuid, packetIDs, new long[1]
    {
      siteInfo.ID
    });
  }

  private TransferedObject LoadFromBlob(
    IBlobReader blobReader,
    out string tag,
    out long[] filesLength)
  {
    using (ImChunkedStream input = new ImChunkedStream())
    {
      byte[] buffer1 = blobReader.ReadDataBlock(4);
      input.Write(buffer1, 0, buffer1.Length);
      input.Position = 0L;
      using (BinaryReader reader = new BinaryReader((Stream) input, Encoding.UTF8))
      {
        int dataBlockSize = reader.ReadInt32();
        byte[] buffer2 = blobReader.ReadDataBlock(dataBlockSize);
        input.SetLength(0L);
        input.Write(buffer2, 0, buffer2.Length);
        input.Position = 0L;
        TransferedObject transferedObject = new TransferedObject();
        transferedObject.Load(reader);
        int count = reader.ReadInt32();
        tag = count > 0 ? new string(reader.ReadChars(count)) : string.Empty;
        int length = reader.ReadInt32();
        filesLength = new long[length];
        for (int index = 0; index < length; ++index)
          filesLength[index] = reader.ReadInt64();
        return transferedObject;
      }
    }
  }

  public void ImportPackets(
    IUserSession session,
    SiteInfo info,
    Guid updateGuid,
    long[] packetIDs,
    long[] recipientIDs)
  {
    TraceLog.Write($"Start ImportPackets: sessionGuid={session.SessionGUID}, updateGuid={updateGuid}, packetIDs count={packetIDs.Length}");
    List<TransferedObject> transferedObjectList = new List<TransferedObject>();
    List<string> createdDirectories = new List<string>();
    try
    {
      ISitesCacheService customService = (ISitesCacheService) session.GetCustomService(typeof (ISitesCacheService));
      string dirName = Path.Combine(TempStorage.RootFolder, PortalServerConsts.FolderUpdatesObjects);
      for (int index = 0; index < packetIDs.Length; ++index)
      {
        long packetId = packetIDs[index];
        IDBObject packet = session.GetObject(packetId, true);
        IDBAttribute attributeByGuid1 = packet.GetAttributeByGuid(PortalConsts.attributeEnabledSites);
        foreach (long recipientId in recipientIDs)
        {
          SiteInfo site = customService.GetSite(recipientId);
          if (attributeByGuid1.AsString.IndexOf(site.Code) < 0)
            throw new Exception($"{packet.NameInMessages} недоступен для импорта узлом {site.Caption}");
        }
        long publicationReceipt = this.GetPublicationReceipt(session, packetId);
        transferedObjectList.Add(new TransferedObject(Guid.NewGuid(), TransferedObjectCategory.Packet, (TransferedObjectTag) new PacketTag(packetId, packet.ObjectGUID, packet.Caption, attributeByGuid1.AsString, publicationReceipt != 0L)));
        IDBAttribute attributeByGuid2 = packet.GetAttributeByGuid(PortalConsts.attributeTaskData);
        if (attributeByGuid2 != null && !attributeByGuid2.IsNull)
          this.LoadFormatPacket1v(session, packet, attributeByGuid2, transferedObjectList, dirName, createdDirectories);
        else
          this.LoadFormatPacket2v(session, packet, transferedObjectList, dirName, createdDirectories);
      }
      new SiteUpdate(transferedObjectList, recipientIDs, info.Code.ToString()).SaveIntoBase(session, updateGuid);
      TraceLog.Write("End ImportPackets");
    }
    catch
    {
      if (createdDirectories.Count > 0)
      {
        foreach (string path in createdDirectories)
          Directory.Delete(path, true);
      }
      throw;
    }
  }

  private void LoadFormatPacket1v(
    IUserSession session,
    IDBObject packet,
    IDBAttribute attrData,
    List<TransferedObject> result,
    string dirName,
    List<string> createdDirectories)
  {
    for (int index = 0; index < attrData.ValuesCount; ++index)
    {
      attrData.Index = index;
      if (!attrData.IsNull)
      {
        IBlobReader blobReader = attrData as IBlobReader;
        BlobInformation blobInformation = blobReader.OpenBlob(0);
        try
        {
          byte[] bytes = blobReader.ReadDataBlock(0);
          TransferedObject unit = new TransferedObject();
          unit.Load(bytes);
          this.CorrectOwner(session, unit, blobInformation.Note);
          result.Add(unit);
        }
        finally
        {
          blobReader.CloseBlob();
        }
      }
    }
    IDBAttribute attributeByGuid = packet.GetAttributeByGuid(PortalConsts.attributePacketFiles);
    for (int index = 0; index < attributeByGuid.ValuesCount; ++index)
    {
      attributeByGuid.Index = index;
      if (!attributeByGuid.IsNull)
      {
        IBlobReader blobReader = attributeByGuid as IBlobReader;
        BlobInformation blobInformation = blobReader.OpenBlob(0);
        try
        {
          FileInfo fileInfo = new FileInfo(Path.Combine(dirName, blobInformation.FileName));
          DirectoryInfo directory = Directory.CreateDirectory(fileInfo.Directory.FullName);
          createdDirectories.Add(directory.FullName);
          using (FileStream fileStream = File.Create(fileInfo.FullName))
          {
            byte[] buffer = blobReader.ReadDataBlock();
            if (buffer.Length != 0)
              fileStream.Write(buffer, 0, buffer.Length);
          }
        }
        finally
        {
          blobReader.CloseBlob();
        }
      }
    }
  }

  private void CorrectOwner(IUserSession session, TransferedObject unit, string objectGuid)
  {
    if (unit.Category != TransferedObjectCategory.Object && unit.Category != TransferedObjectCategory.ObjectLink)
      return;
    IDBObject dbObject = !string.IsNullOrEmpty(objectGuid) && GuidHelper.IsGuid(objectGuid) ? session.GetObject(new Guid(objectGuid), true) : throw new Exception("Пакет не может быть импортирован так как имеет структуру, несовместимую с текущей версией web-портала!");
    ObjectTag tag = (ObjectTag) unit.Tag;
    tag.OwnerCode = SiteCodeHelper.GetSiteCode(dbObject, PortalConsts.attributeOwner);
    tag.CompositionOwnerCode = SiteCodeHelper.GetSiteCode(dbObject, PortalConsts.attributeCompositionOwner);
  }

  private void LoadFormatPacket2v(
    IUserSession session,
    IDBObject packet,
    List<TransferedObject> result,
    string dirName,
    List<string> createdDirectories)
  {
    IBlobReader attributeByGuid = packet.GetAttributeByGuid(PortalConsts.attributePacketFiles) as IBlobReader;
    attributeByGuid.OpenBlob(0);
    while (attributeByGuid.BlobState != BlobAttributeStates.Closed)
    {
      string tag;
      long[] filesLength;
      TransferedObject unit = this.LoadFromBlob(attributeByGuid, out tag, out filesLength);
      this.CorrectOwner(session, unit, tag);
      result.Add(unit);
      string pathFromGuid = TempStorageHelper.CreatePathFromGuid(string.Empty, unit.GUID);
      for (int index1 = 0; index1 < unit.DataFiles.Length; ++index1)
      {
        FileInfo fileInfo = new FileInfo(Path.Combine(dirName, Path.Combine(pathFromGuid, unit.DataFiles[index1])));
        if (!Directory.Exists(fileInfo.Directory.FullName))
        {
          Directory.CreateDirectory(fileInfo.Directory.FullName);
          createdDirectories.Add(fileInfo.Directory.FullName);
        }
        int dataBlockSize;
        using (FileStream fileStream = File.Create(fileInfo.FullName))
        {
          for (long index2 = filesLength[index1]; index2 > 0L; index2 -= (long) dataBlockSize)
          {
            dataBlockSize = index2 > (long) Intermech.Consts.DefaultBlobBlockSize ? Intermech.Consts.DefaultBlobBlockSize : Convert.ToInt32(index2);
            byte[] buffer = attributeByGuid.ReadDataBlock(dataBlockSize);
            if (buffer.Length != 0)
              fileStream.Write(buffer, 0, buffer.Length);
          }
        }
      }
    }
  }
}

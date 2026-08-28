// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.DBPublishRelation
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using Intermech.Localization;
using System;
using System.Data;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal class DBPublishRelation(UserSession userSession, DataTable relationParams) : DBRelation(userSession, relationParams)
{
  protected override void DoRemove(long newProjID, int newRelationTypeID)
  {
    base.DoRemove(newProjID, newRelationTypeID);
    IDBAttribute attributeByGuid = this.GetAttributeByGuid(PortalServerConsts.attributeFile);
    if (attributeByGuid == null || attributeByGuid.ValuesCount <= 0)
      return;
    for (int index = 0; index < attributeByGuid.ValuesCount; ++index)
    {
      attributeByGuid.Index = index;
      if (!(attributeByGuid.AsString != PortalConsts.AttributesXmlFileName))
      {
        IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
        MemoryStream memoryStream1 = new MemoryStream();
        try
        {
          MemoryStream memoryStream2 = new MemoryStream();
          BlobInformation blobInfo = new BlobInformation();
          try
          {
            XmlDocument xmlDocument = this.ReadAttributesFile(attributeByGuid, service, memoryStream1, memoryStream2);
            XmlNode xmlNode = (XmlNode) null;
            for (int i = 0; i < xmlDocument.ChildNodes.Count; ++i)
            {
              if (xmlDocument.ChildNodes[i].Name == PortalConsts.XmlRootNodeAttributes)
              {
                xmlNode = xmlDocument.ChildNodes[i];
                break;
              }
            }
            if (xmlNode == null)
              throw new Exception(LocalizationHolder.rm.GetString("PortalServer_35"));
            bool flag = false;
            for (int i = 0; i < xmlNode.ChildNodes.Count; ++i)
            {
              if (xmlNode.ChildNodes[i].Name == PortalConsts.XmlNodeSysAttribute && xmlNode.ChildNodes[i].Attributes["F_PROJECT_GUID"] != null)
              {
                xmlNode.ChildNodes[i].Attributes["F_PROJECT_GUID"].Value = this.GetProjectGuid(newProjID).ToString();
                flag = true;
                break;
              }
            }
            if (!flag)
              throw new Exception(LocalizationHolder.rm.GetString("PortalServer_36"));
            memoryStream2.Position = 0L;
            xmlDocument.Save((Stream) memoryStream2);
            service.PackStream((Stream) memoryStream1, (Stream) memoryStream2, 9);
            blobInfo.RealFileSize = memoryStream2.Length;
          }
          finally
          {
            memoryStream2.Close();
          }
          blobInfo.ModifyDate = DateTime.Now;
          blobInfo.ArcMethod = ArcMethods.ZLibPacked;
          blobInfo.PackedFileSize = memoryStream1.Length;
          blobInfo.Note = string.Empty;
          blobInfo.FileName = PortalConsts.AttributesXmlFileName;
          IBlobWriter blobWriter = attributeByGuid as IBlobWriter;
          blobWriter.OpenBlob(blobInfo, false);
          blobWriter.WriteDataBlock(memoryStream1.ToArray());
        }
        finally
        {
          memoryStream1.Close();
        }
      }
    }
  }

  private Guid GetProjectGuid(long projID) => this.UserSession.GetObjectInfo(projID).VersionGuid;

  internal Guid GetPartGuidFromFile(bool throwException)
  {
    IDBAttribute attributeByGuid = this.GetAttributeByGuid(PortalServerConsts.attributeFile);
    if (attributeByGuid != null && attributeByGuid.ValuesCount > 0)
    {
      for (int index = 0; index < attributeByGuid.ValuesCount; ++index)
      {
        attributeByGuid.Index = index;
        if (!(attributeByGuid.AsString != PortalConsts.AttributesXmlFileName))
        {
          IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
          MemoryStream mStream = new MemoryStream();
          try
          {
            MemoryStream unpackedStream = new MemoryStream();
            try
            {
              XmlDocument xmlDocument = this.ReadAttributesFile(attributeByGuid, service, mStream, unpackedStream);
              XmlNode xmlNode = (XmlNode) null;
              for (int i = 0; i < xmlDocument.ChildNodes.Count; ++i)
              {
                if (xmlDocument.ChildNodes[i].Name == PortalConsts.XmlRootNodeAttributes)
                {
                  xmlNode = xmlDocument.ChildNodes[i];
                  break;
                }
              }
              if (xmlNode == null)
              {
                if (throwException)
                  throw new Exception(LocalizationHolder.rm.GetString("PortalServer_35"));
                return Guid.Empty;
              }
              for (int i = 0; i < xmlNode.ChildNodes.Count; ++i)
              {
                if (xmlNode.ChildNodes[i].Name == PortalConsts.XmlNodeSysAttribute && xmlNode.ChildNodes[i].Attributes["F_PART_GUID"] != null)
                  return new Guid(xmlNode.ChildNodes[i].Attributes["F_PART_GUID"].Value);
              }
            }
            finally
            {
              unpackedStream.Close();
            }
          }
          finally
          {
            mStream.Close();
          }
        }
      }
    }
    return Guid.Empty;
  }

  private XmlDocument ReadAttributesFile(
    IDBAttribute attrData,
    IPackedStream packedStream,
    MemoryStream mStream,
    MemoryStream unpackedStream)
  {
    IBlobReader blobReader = attrData as IBlobReader;
    BlobInformation blobInformation = blobReader.OpenBlob(0);
    try
    {
      byte[] buffer = blobReader.ReadDataBlock(0);
      if (buffer != null)
        mStream.Write(buffer, 0, buffer.Length);
      XmlDocument xmlDocument = new XmlDocument();
      if (blobInformation.ArcMethod == ArcMethods.ZLibPacked)
      {
        packedStream.UnpackStream((Stream) unpackedStream, (Stream) mStream);
        unpackedStream.Position = 0L;
        xmlDocument.Load((Stream) unpackedStream);
      }
      else
      {
        mStream.Position = 0L;
        xmlDocument.Load((Stream) mStream);
      }
      return xmlDocument;
    }
    finally
    {
      blobReader.CloseBlob();
    }
  }
}

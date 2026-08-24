// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Utils.RelationsFileCorrector
// Assembly: Intermech.Portal.Utils, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 99780CCF-14B7-482E-A297-41CC169803AE
// Assembly location: D:\IPS\Client\Intermech.Portal.Utils.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using System;
using System.Data;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Utils;

internal static class RelationsFileCorrector
{
  public static void Correct(
    IUserSession session,
    IDBObjectCollection publishObjects,
    long relationID,
    Guid attributeFile,
    out string message)
  {
    message = string.Empty;
    IDBRelation relation = session.GetRelation(relationID);
    IDBAttribute attributeByGuid1 = relation.GetAttributeByGuid(attributeFile);
    if (attributeByGuid1 == null || attributeByGuid1.ValuesCount <= 0)
      return;
    for (int index = 0; index < attributeByGuid1.ValuesCount; ++index)
    {
      attributeByGuid1.Index = index;
      if (!(attributeByGuid1.AsString != PortalConsts.AttributesXmlFileName))
      {
        IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
        MemoryStream memoryStream1 = new MemoryStream();
        try
        {
          IBlobReader blobReader = attributeByGuid1 as IBlobReader;
          BlobInformation blobInformation = blobReader.OpenBlob(0);
          MemoryStream memoryStream2 = new MemoryStream();
          BlobInformation blobInfo = new BlobInformation();
          bool flag = false;
          try
          {
            byte[] buffer = blobReader.ReadDataBlock(0);
            if (buffer != null)
              memoryStream1.Write(buffer, 0, buffer.Length);
            XmlDocument xmlDocument = new XmlDocument();
            if (blobInformation.ArcMethod == ArcMethods.ZLibPacked)
            {
              service.UnpackStream((Stream) memoryStream2, (Stream) memoryStream1);
              memoryStream2.Position = 0L;
              xmlDocument.Load((Stream) memoryStream2);
            }
            else
            {
              memoryStream1.Position = 0L;
              xmlDocument.Load((Stream) memoryStream1);
            }
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
              message = $"Не найдена информация по атрибутам в файле с атрибутами связи {relation.GUID}";
              continue;
            }
            for (int i = 0; i < xmlNode.ChildNodes.Count; ++i)
            {
              if (xmlNode.ChildNodes[i].Name == PortalConsts.XmlNodeSysAttribute && xmlNode.ChildNodes[i].Attributes["F_PROJECT_GUID"] != null)
              {
                Guid guid1 = new Guid(xmlNode.ChildNodes[i].Attributes["F_PROJECT_GUID"].Value);
                Guid g = new Guid(session.GetObject(relation.ProjID).GetAttributeByGuid(PortalConsts.attributePublishObjectGUID).AsString);
                if (!guid1.Equals(g))
                {
                  message = $"Замена project в {relation.GUID}: {{{guid1}}}->{{{g}}}";
                  xmlNode.ChildNodes[i].Attributes["F_PROJECT_GUID"].Value = g.ToString();
                  flag = true;
                }
                Guid guid2 = new Guid(xmlNode.ChildNodes[i].Attributes["F_PART_GUID"].Value);
                DataTable dataTable = publishObjects.Select(new DBRecordSetParams(new ConditionStructure[1]
                {
                  new ConditionStructure(PortalConsts.attributePublishObjectGUID, RelationalOperators.Equal, (object) guid2, LogicalOperators.AND, 0)
                }, new object[1]{ (object) -2 }));
                long int64 = dataTable.Rows.Count == 0 ? 0L : Convert.ToInt64(dataTable.Rows[0][0]);
                IDBAttribute attributeByGuid2 = relation.GetAttributeByGuid(new Guid("cad001c2-306c-11d8-b4e9-00304f19f545"));
                long asInteger = attributeByGuid2 != null ? attributeByGuid2.AsInteger : 0L;
                if (int64 == 0L)
                {
                  message = $"Указанный F_PART_GUID={{{guid2}}} не найден.";
                  IDBObject dbObject = asInteger != 0L ? session.GetObject(asInteger, true) : session.GetObjectBaseVersionByID(relation.PartID, true);
                  message += $" Выполнена корректировка в xml на {(asInteger != 0L ? (object) "версию, указанную в конкретизации" : (object) "базовую версию part связи")}";
                  Guid publishObjectGuid = PortalConsts.attributePublishObjectGUID;
                  IDBAttribute attributeByGuid3 = dbObject.GetAttributeByGuid(publishObjectGuid);
                  xmlNode.ChildNodes[i].Attributes["F_PART_GUID"].Value = attributeByGuid3.AsString;
                  flag = true;
                  break;
                }
                Guid guid3 = new Guid(session.GetObject(int64).GetAttributeByGuid(PortalConsts.attributePublishObjectGUID).AsString);
                if (!guid3.Equals(guid2))
                {
                  message = $"Замена part в {relation.GUID}: {{{guid2}}}->{{{guid3}}}";
                  xmlNode.ChildNodes[i].Attributes["F_PART_GUID"].Value = guid3.ToString();
                  flag = true;
                  break;
                }
                break;
              }
            }
            if (flag)
            {
              memoryStream2.Position = 0L;
              xmlDocument.Save((Stream) memoryStream2);
              service.PackStream((Stream) memoryStream1, (Stream) memoryStream2, 9);
              blobInfo.RealFileSize = memoryStream2.Length;
            }
          }
          finally
          {
            memoryStream2.Close();
            blobReader.CloseBlob();
          }
          if (flag)
          {
            blobInfo.ModifyDate = DateTime.Now;
            blobInfo.ArcMethod = ArcMethods.ZLibPacked;
            blobInfo.PackedFileSize = memoryStream1.Length;
            blobInfo.Note = string.Empty;
            blobInfo.FileName = PortalConsts.AttributesXmlFileName;
            IBlobWriter blobWriter = attributeByGuid1 as IBlobWriter;
            blobWriter.OpenBlob(blobInfo, false);
            blobWriter.WriteDataBlock(memoryStream1.ToArray());
          }
        }
        finally
        {
          memoryStream1.Close();
        }
      }
    }
  }
}

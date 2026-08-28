// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.Classes.Publishers.Remarks
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server.WebPortal;
using Intermech.Interfaces.WebPortal;
using System;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server.Classes.Publishers;

internal sealed class Remarks
{
  private IUserSession _session;
  private IDBObject _publishObject;
  private char _siteCode;

  public Remarks(IUserSession session, IDBObject publishObject, char siteCode)
  {
    this._session = session;
    this._publishObject = publishObject;
    this._siteCode = siteCode;
  }

  public void Add(string iPath, XmlDocument xmlDocument, XmlNode rootNode, string enableSites)
  {
    RemarksStorage storage = new RemarksStorage(this._publishObject);
    for (int i = 0; i < rootNode.ChildNodes.Count; ++i)
    {
      XmlNode childNode = rootNode.ChildNodes[i];
      if ((childNode.Name == PortalConsts.XmlNodeAttribute || childNode.Name == PortalConsts.XmlRootNodeRemark) && childNode.ChildNodes.Count > 0 && childNode.ChildNodes[0].Name == PortalConsts.XmlNodeValueAttribute)
      {
        AttributeInfo attributeInfo = AttributesFile.GetAttributeInfo(childNode);
        if (TraceLog.Enabled)
          TraceLog.Write($"...write attribute guid={attributeInfo.Guid} name={attributeInfo.Name} type={attributeInfo.FieldType}");
        if (UnitPublisher.IsForbiddenAttribute(attributeInfo))
        {
          if (TraceLog.Enabled)
            TraceLog.Write("...attribute is forbidden");
        }
        else
        {
          RemarkInfo remark = new RemarkInfo(attributeInfo.Guid, attributeInfo.Name, attributeInfo.ShortName, attributeInfo.Alias, attributeInfo.FieldType, this._siteCode, DateTime.UtcNow, enableSites);
          for (int index = 0; index < childNode.ChildNodes.Count; ++index)
          {
            ValueInfo valueInfo = UnitXmlFile.GetValueInfo(childNode.ChildNodes[index], index);
            remark.Values.Add(valueInfo);
            IBlobValue blobValue = (IBlobValue) null;
            if (attributeInfo.FieldType == FieldTypes.ftBlob || attributeInfo.FieldType == FieldTypes.ftMemo || attributeInfo.FieldType == FieldTypes.ftShortBlob)
              blobValue = (IBlobValue) new BlobValue(attributeInfo, this._siteCode, valueInfo);
            else if (attributeInfo.FieldType == FieldTypes.ftFile)
              blobValue = (IBlobValue) new FileValue(attributeInfo, this._siteCode, valueInfo);
            if (blobValue != null)
            {
              blobValue.PrepareStorage(storage);
              storage.WriteRemarkFile(iPath, valueInfo, blobValue.Key);
              valueInfo.FileName = blobValue.Key;
              if (TraceLog.Enabled)
                TraceLog.Write($"...set FileName={blobValue.Key}");
            }
          }
          string key = this._siteCode.ToString() + (remark.Guid == null || !(remark.Guid != string.Empty) ? remark.Name : remark.Guid);
          if (TraceLog.Enabled)
            TraceLog.Write($"...write attrRemarkList key={key}");
          storage.ClearRemarkList(key);
          storage.WriteRemark(remark, key);
        }
      }
    }
  }
}

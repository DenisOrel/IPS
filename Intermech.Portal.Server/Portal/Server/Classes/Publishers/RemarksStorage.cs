// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.Classes.Publishers.RemarksStorage
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

#nullable disable
namespace Intermech.Portal.Server.Classes.Publishers;

internal class RemarksStorage
{
  private IDBAttribute _attrRemarkList;
  private IDBAttribute _attrRemarkFiles;
  private bool _isNew;

  public RemarksStorage(IDBObject publishObject)
  {
    this._isNew = false;
    this._attrRemarkList = publishObject.GetAttributeByGuid(PortalConsts.attributeRemarkList, false);
    if (this._attrRemarkList == null || this._attrRemarkList.ValuesCount == 0)
    {
      this._isNew = true;
      if (this._attrRemarkList == null)
        this._attrRemarkList = publishObject.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeRemarkList), false);
    }
    this._attrRemarkFiles = publishObject.GetAttributeByGuid(PortalConsts.attributeRemarkFiles, false);
    if (this._isNew)
      this._attrRemarkFiles = publishObject.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeRemarkFiles), false);
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write($"...attribute attrRemarkFiles is new={this._isNew}");
  }

  public void ClearRemarkFiles(string pattern, bool regex)
  {
    if (this._isNew)
      return;
    Regex regex1 = (Regex) null;
    if (regex)
      regex1 = new Regex(pattern);
    List<string> stringList = new List<string>();
    for (int index = 0; index < this._attrRemarkFiles.ValuesCount; ++index)
    {
      this._attrRemarkFiles.Index = index;
      bool flag = false;
      if (regex)
      {
        if (regex1.IsMatch(this._attrRemarkFiles.AsString))
        {
          stringList.Add(this._attrRemarkFiles.AsString);
          flag = true;
        }
      }
      else if (string.Compare(this._attrRemarkFiles.AsString, pattern) == 0)
      {
        stringList.Add(this._attrRemarkFiles.AsString);
        flag = true;
      }
      if (TraceLog.Enabled)
        TraceLog.Write($"...compare fileNames: {this._attrRemarkFiles.AsString} и {pattern}: {flag}");
    }
    foreach (string str in stringList)
    {
      for (int index = 0; index < this._attrRemarkFiles.ValuesCount; ++index)
      {
        this._attrRemarkFiles.Index = index;
        if (this._attrRemarkFiles.AsString == str)
        {
          if (this._attrRemarkFiles.ValuesCount == 1)
            this._attrRemarkFiles.Clear();
          else
            this._attrRemarkFiles.DeleteValue();
          if (TraceLog.Enabled)
          {
            TraceLog.Write("...old value deleted");
            break;
          }
          break;
        }
      }
    }
  }

  public void WriteRemarkFile(string iPath, ValueInfo value, string key)
  {
    using (FileStream fileStream = new FileStream(Path.Combine(iPath, value.FileName), FileMode.Open))
    {
      if (TraceLog.Enabled)
        TraceLog.Write($"...write data ({fileStream.Length}) from {Path.Combine(iPath, value.FileName)}");
      if (!this._attrRemarkFiles.IsNull)
        this._attrRemarkFiles.AddValue((object) null);
      IBlobWriter attrRemarkFiles = this._attrRemarkFiles as IBlobWriter;
      attrRemarkFiles.OpenBlob(new BlobInformation(value.IntValue, fileStream.Length, value.DateValue, key, value.ArcMethod, value.StringValue), false);
      byte[] numArray1 = new byte[Consts.BlobTransferBufferLength];
      int length;
      while ((length = fileStream.Read(numArray1, 0, Consts.BlobTransferBufferLength)) > 0)
      {
        byte[] numArray2 = new byte[length];
        Array.Copy((Array) numArray1, (Array) numArray2, length);
        attrRemarkFiles.WriteDataBlock(numArray2);
      }
    }
  }

  public void ClearRemarkList(string key)
  {
    if (this._isNew)
      return;
    for (int index = 0; index < this._attrRemarkList.ValuesCount; ++index)
    {
      this._attrRemarkList.Index = index;
      if (key.Equals(this._attrRemarkList.AsString))
      {
        if (this._attrRemarkList.ValuesCount == 1)
          this._attrRemarkList.ClearValues();
        else
          this._attrRemarkList.DeleteValue();
        if (!TraceLog.Enabled)
          break;
        TraceLog.Write("...old value deleted");
        break;
      }
    }
  }

  public void WriteRemark(RemarkInfo remark, string key)
  {
    if (TraceLog.Enabled)
      TraceLog.Write("...write remark");
    if (!this._attrRemarkList.IsNull)
      this._attrRemarkList.AddValue((object) null);
    using (MemoryStream serializationStream = new MemoryStream())
    {
      new BinaryFormatter().Serialize((Stream) serializationStream, (object) remark);
      serializationStream.Position = 0L;
      IBlobWriter attrRemarkList = this._attrRemarkList as IBlobWriter;
      attrRemarkList.OpenBlob(new BlobInformation(serializationStream.Length, serializationStream.Length, DateTime.Now, string.Empty, ArcMethods.NotPacked, key), false);
      byte[] numArray1 = new byte[Consts.BlobTransferBufferLength];
      int length;
      while ((length = serializationStream.Read(numArray1, 0, Consts.BlobTransferBufferLength)) > 0)
      {
        byte[] numArray2 = new byte[length];
        Array.Copy((Array) numArray1, (Array) numArray2, length);
        attrRemarkList.WriteDataBlock(numArray2);
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VisDialogs.VisStyle
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Expert;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Intermech.Pdm.VisDialogs;

public class VisStyle
{
  public long StyleID;
  public string Name = string.Empty;
  public List<VisStyleNode> StyleNodes;

  public VisStyle() => this.StyleNodes = new List<VisStyleNode>();

  public bool LoadFromObject(IUserSession session, long anObjectID)
  {
    this.StyleID = anObjectID;
    if (anObjectID == 0L)
      return false;
    IDBObject dbObject = session.GetObject(anObjectID, false);
    if (dbObject == null)
      return false;
    this.Name = dbObject.Caption;
    byte[] zipScr = (byte[]) null;
    if (dbObject.GetAttributeByID(ExpertConsts.Consts.attrObjData) is IDBShortBlobAttribute attributeById)
      zipScr = attributeById.GetData();
    XmlElement documentElement = ZlibHelper.UnpackXmlBuffer(zipScr).DocumentElement;
    this.StyleNodes.Clear();
    if (documentElement.HasChildNodes)
    {
      foreach (XmlNode childNode in documentElement.ChildNodes)
      {
        if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "Style")
          this.StyleNodes.Add(new VisStyleNode(childNode, session));
      }
    }
    return this.StyleNodes.Count > 0;
  }

  public void SaveToObject(IUserSession session)
  {
    IDBObject dbObject = session.GetObject(this.StyleID);
    dbObject.Caption = this.Name;
    MemoryStream memoryStream = new MemoryStream();
    MemoryStream outStream = new MemoryStream();
    XmlTextWriter writer = new XmlTextWriter((Stream) memoryStream, Encoding.UTF8);
    try
    {
      writer.Formatting = Formatting.Indented;
      writer.WriteStartDocument();
      writer.WriteStartElement(nameof (VisStyle));
      writer.WriteAttributeString("xmlns", (string) null, "http://www.intermech.ru/Visualizer");
      foreach (VisStyleNode styleNode in this.StyleNodes)
        styleNode.WriteToXml(writer);
      writer.WriteEndElement();
      writer.WriteEndDocument();
      writer.Flush();
      memoryStream.Position = 0L;
      ZLibStreamHelper.PackStream((Stream) memoryStream, ZLibCompressLevels.Level3, (Stream) outStream);
    }
    finally
    {
      writer?.Close();
    }
    byte[] array = outStream.ToArray();
    if (!(dbObject.Attributes.AddAttribute(ExpertConsts.Consts.attrObjData, false) is IBlobWriter blobWriter))
      return;
    BlobInformation blobInfo = new BlobInformation((long) array.Length, (long) array.Length, DateTime.Now, "", ArcMethods.NotPacked, "");
    if (!blobWriter.OpenBlob(blobInfo, false))
      return;
    blobWriter.WriteDataBlock(array);
  }
}

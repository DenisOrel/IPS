// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Services.ScriptPadSettingsService
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Runtime;
using Intermech.Scripting.ScriptPad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.Scripting.Services;

internal sealed class ScriptPadSettingsService : IDESettingsService
{
  private string fileName;
  private Dictionary<Tuple<string, string>, Tuple<Type, object>> storage;
  private int pendingWrites;

  public ScriptPadSettingsService(string fileName)
  {
    this.fileName = fileName != null ? fileName : throw new ArgumentNullException(nameof (fileName));
    this.storage = new Dictionary<Tuple<string, string>, Tuple<Type, object>>();
    this.LoadFromUserConfiguration();
  }

  private void LoadFromUserConfiguration()
  {
    byte[] config_file;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      sessionKeeper.Session.Configurations.LoadConfigData(this.fileName, out BlobInformation _, out config_file, sessionKeeper.Session.UserID);
    if (config_file.Length == 0)
      return;
    using (MemoryStream memoryStream = new MemoryStream(config_file, false))
      this.LoadFromStream((Stream) memoryStream);
  }

  private void LoadFromStream(Stream stream)
  {
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.PreserveWhitespace = true;
    xmlDocument.Load(stream);
    this.storage.Clear();
    foreach (XmlNode selectNode1 in xmlDocument.DocumentElement.SelectNodes("Section[@name]"))
    {
      string str1 = XmlConvert.DecodeName(selectNode1.Attributes["name"].Value);
      if (!string.IsNullOrEmpty(str1))
      {
        foreach (XmlNode selectNode2 in selectNode1.SelectNodes("Parameter[@name and @value and @type]"))
        {
          string str2 = XmlConvert.DecodeName(selectNode2.Attributes["name"].Value);
          if (!string.IsNullOrEmpty(str2))
          {
            string typeName = XmlConvert.DecodeName(selectNode2.Attributes["type"].Value);
            if (!string.IsNullOrEmpty(typeName))
            {
              string str3 = XmlConvert.DecodeName(selectNode2.Attributes["value"].Value);
              if (!string.IsNullOrEmpty(str3))
              {
                try
                {
                  Type type = Type.GetType(typeName, true);
                  object obj = Convert.ChangeType((object) str3, type);
                  this.storage[Tuple.Create<string, string>(str1, str2)] = Tuple.Create<Type, object>(type, obj);
                }
                catch (Exception ex)
                {
                  string currentMethodName = this.GetCurrentMethodName(nameof (LoadFromStream));
                  SuppressedExceptions.TraceException(ex, currentMethodName);
                }
              }
            }
          }
        }
      }
    }
  }

  private void SaveToUserConfiguration()
  {
    byte[] array;
    using (MemoryStream memoryStream = new MemoryStream())
    {
      this.SaveToStream((Stream) memoryStream);
      memoryStream.Flush();
      array = memoryStream.ToArray();
    }
    (ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations).WriteConfigData(new BlobInformation((long) array.Length, (long) array.Length, DateTime.Now, "ScriptPadSettings.xml", ArcMethods.NotPacked, string.Empty), array);
  }

  private void SaveToStream(Stream stream)
  {
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.PreserveWhitespace = true;
    xmlDocument.AppendChild((XmlNode) xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", (string) null));
    xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement("Settings"));
    foreach (KeyValuePair<Tuple<string, string>, Tuple<Type, object>> keyValuePair in this.storage)
    {
      Tuple<string, string> key = keyValuePair.Key;
      Tuple<Type, object> tuple = keyValuePair.Value;
      XmlNode newChild = xmlDocument.DocumentElement.SelectSingleNode($"Section[@name='{XmlConvert.EncodeName(key.Item1)}']");
      if (newChild == null)
      {
        newChild = (XmlNode) xmlDocument.CreateElement("Section");
        newChild.Attributes.Append(xmlDocument.CreateAttribute("name")).Value = XmlConvert.EncodeName(key.Item1);
        xmlDocument.DocumentElement.AppendChild(newChild);
      }
      XmlElement element = xmlDocument.CreateElement("Parameter");
      element.Attributes.Append(xmlDocument.CreateAttribute("name")).Value = XmlConvert.EncodeName(key.Item2);
      element.Attributes.Append(xmlDocument.CreateAttribute("type")).Value = XmlConvert.EncodeName(tuple.Item1.FullName);
      element.Attributes.Append(xmlDocument.CreateAttribute("value")).Value = XmlConvert.EncodeName(tuple.Item2.ToString());
      newChild.AppendChild((XmlNode) element);
    }
    xmlDocument.Save(stream);
  }

  protected override Tuple<Type, object> DoTryReadParameter(Tuple<string, string> key)
  {
    Tuple<Type, object> tuple;
    return this.storage.TryGetValue(key, out tuple) ? tuple : (Tuple<Type, object>) null;
  }

  protected override void DoWriteParameter(
    Tuple<string, string> key,
    Tuple<Type, object> typeAndValue)
  {
    this.storage[key] = typeAndValue;
    ++this.pendingWrites;
  }

  protected override void DoFlush()
  {
    if (this.pendingWrites == 0)
      return;
    this.SaveToUserConfiguration();
    this.pendingWrites = 0;
  }
}

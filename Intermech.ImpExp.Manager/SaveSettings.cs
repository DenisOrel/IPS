// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.SaveSettings
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.Manager;

public class SaveSettings : ISaveSettings
{
  private IConfiguration _settings;
  private const string KeyAttributeName = "NAME";

  public void Load()
  {
    try
    {
      this._settings = ConfigurationLoader.Load(Path.Combine(SettingsHelper.SettingsFolder, "PumpSettings.xml"));
    }
    catch (Exception ex)
    {
      this._settings = (IConfiguration) null;
      int num = (int) MessageBox.Show("Ошибка чтения настроек импорта:\r\n" + ex.Message);
    }
  }

  public void Save()
  {
    try
    {
      if (this._settings == null)
        return;
      (this._settings as IConfigurationService).Save(Path.Combine(SettingsHelper.SettingsFolder, "PumpSettings.xml"));
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show("Ошибка сохранения настроек импорта:\r\n" + ex.Message);
    }
  }

  public Dictionary<string, SaveSettingsAttribute[]> GetSettings(string settingsName)
  {
    if (this._settings != null)
    {
      XmlNode xmlNode = (XmlNode) null;
      foreach (XmlNode childNode in this._settings.Node.ChildNodes)
      {
        if (childNode.Name.Equals(settingsName))
        {
          xmlNode = childNode;
          break;
        }
      }
      if (xmlNode != null)
      {
        Dictionary<string, SaveSettingsAttribute[]> settings = new Dictionary<string, SaveSettingsAttribute[]>();
        string empty1 = string.Empty;
        foreach (XmlNode childNode in xmlNode.ChildNodes)
        {
          if (childNode.Name.Equals(settingsName) && childNode.Attributes != null)
          {
            string empty2 = string.Empty;
            List<SaveSettingsAttribute> settingsAttributeList = new List<SaveSettingsAttribute>();
            foreach (XmlAttribute attribute in (XmlNamedNodeMap) childNode.Attributes)
            {
              if (attribute.Name.Equals("NAME"))
                empty2 = attribute.Value;
              else
                settingsAttributeList.Add(new SaveSettingsAttribute(attribute.Name, attribute.Value));
            }
            if (empty2 != string.Empty)
              settings.Add(empty2, settingsAttributeList.ToArray());
          }
        }
        return settings;
      }
    }
    return (Dictionary<string, SaveSettingsAttribute[]>) null;
  }

  public void ClearSettings(string settingsName)
  {
    XmlNode xmlNode = (XmlNode) null;
    foreach (XmlNode childNode in this._settings.Node.ChildNodes)
    {
      if (childNode.Name.Equals(settingsName))
      {
        xmlNode = childNode;
        break;
      }
    }
    xmlNode?.RemoveAll();
  }

  public void SetSettings(
    string settingsName,
    Dictionary<string, SaveSettingsAttribute[]> settings)
  {
    XmlNode newChild = (XmlNode) null;
    foreach (XmlNode childNode in this._settings.Node.ChildNodes)
    {
      if (childNode.Name.Equals(settingsName))
      {
        newChild = childNode;
        break;
      }
    }
    if (newChild == null)
    {
      newChild = (XmlNode) this._settings.Node.OwnerDocument.CreateElement(settingsName);
      this._settings.Node.AppendChild(newChild);
    }
    if (newChild != null)
    {
      newChild.RemoveAll();
      IDictionaryEnumerator enumerator = (IDictionaryEnumerator) settings.GetEnumerator();
      while (enumerator.MoveNext())
      {
        XmlNode element = (XmlNode) newChild.OwnerDocument.CreateElement(settingsName);
        SaveSettingsAttribute[] settingsAttributeArray = enumerator.Value as SaveSettingsAttribute[];
        XmlUtils.AddXmlAtrubute(element, "NAME", enumerator.Key.ToString());
        if (settingsAttributeArray != null)
        {
          foreach (SaveSettingsAttribute settingsAttribute in settingsAttributeArray)
            XmlUtils.AddXmlAtrubute(element, settingsAttribute.AttributeName, settingsAttribute.AttributeValue);
        }
        newChild.AppendChild(element);
      }
    }
    this.Save();
  }

  public DateTime SettingsDateTime
  {
    get
    {
      FileInfo fileInfo = new FileInfo(Path.Combine(SettingsHelper.SettingsFolder, "PumpSettings.xml"));
      return fileInfo.Exists ? fileInfo.LastWriteTime : DateTime.MinValue;
    }
  }
}

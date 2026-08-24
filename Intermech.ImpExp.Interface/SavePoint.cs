// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SavePoint
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Точка сохранения</summary>
[Serializable]
public class SavePoint : ICloneable
{
  /// <summary>Операция на которой произошло падение</summary>
  public TerminateType OperationTerminateType;
  /// <summary>GUID пампера на котором произошло падение</summary>
  public Guid PumpGuid;
  /// <summary>
  /// Список GUID памперов, которые были успешно импортированы
  /// </summary>
  public List<Guid> PumpCompleted;
  /// <summary>Режим докачки (импорт новой порции данных)</summary>
  public bool RePumpMode;

  public SavePoint() => this.OperationTerminateType = TerminateType.None;

  public SavePoint(TerminateType type) => this.OperationTerminateType = type;

  public void Load(XmlDocument saved)
  {
    this.OperationTerminateType = TerminateType.None;
    XmlNode documentElement = (XmlNode) saved.DocumentElement;
    if (documentElement == null)
      return;
    foreach (XmlNode childNode in documentElement.ChildNodes)
    {
      switch (childNode.Name)
      {
        case "TerminateType":
          this.OperationTerminateType = (TerminateType) Convert.ToInt32(childNode.InnerText);
          continue;
        case "PumpGuid":
          this.PumpGuid = new Guid(childNode.InnerText);
          continue;
        case "PumpCompleted":
          string[] strArray = childNode.InnerText.Split(';');
          if (strArray.Length != 0)
          {
            this.PumpCompleted = new List<Guid>();
            foreach (string str in strArray)
            {
              if (GuidHelper.IsGuid(str))
                this.PumpCompleted.Add(new Guid(str));
            }
            continue;
          }
          continue;
        case "ResumeMode":
        case "RepumpMode":
          this.RePumpMode = Convert.ToBoolean(childNode.InnerText, (IFormatProvider) CultureInfo.InvariantCulture);
          continue;
        default:
          continue;
      }
    }
  }

  public XmlDocument Save()
  {
    XmlDocument xmlDocument = new XmlDocument();
    XmlNode element1 = (XmlNode) xmlDocument.CreateElement(nameof (SavePoint));
    XmlNode element2 = (XmlNode) xmlDocument.CreateElement("TerminateType");
    element2.InnerText = Convert.ToString((int) this.OperationTerminateType);
    element1.AppendChild(element2);
    if (this.PumpGuid != Guid.Empty)
    {
      XmlNode element3 = (XmlNode) xmlDocument.CreateElement("PumpGuid");
      element3.InnerText = Convert.ToString((object) this.PumpGuid);
      element1.AppendChild(element3);
    }
    if (this.PumpCompleted != null && this.PumpCompleted.Count > 0)
    {
      XmlNode element4 = (XmlNode) xmlDocument.CreateElement("PumpCompleted");
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Guid guid in this.PumpCompleted)
      {
        stringBuilder.Append(Convert.ToString((object) guid));
        stringBuilder.Append(';');
      }
      if (stringBuilder.Length > 0)
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
      element4.InnerText = stringBuilder.ToString();
      element1.AppendChild(element4);
    }
    XmlNode element5 = (XmlNode) xmlDocument.CreateElement("RepumpMode");
    element5.InnerText = Convert.ToString(this.RePumpMode, (IFormatProvider) CultureInfo.InvariantCulture);
    element1.AppendChild(element5);
    xmlDocument.AppendChild(element1);
    return xmlDocument;
  }

  /// <summary>Создание клона объекта</summary>
  /// <returns></returns>
  public object Clone() => this.MemberwiseClone();
}

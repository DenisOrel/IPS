// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Scenario2XmlConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Properties;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;
using Intermech.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.TechCard;

internal class Scenario2XmlConverter
{
  public const int SizeMarginLeft = 8;
  public const int SizeMarginTop = 8;
  public const int SizeMarginDefPanel = -1;
  public const int SizeSplitHoriz = 8;
  public const int SizeSplitVert = 8;
  public const int SizeHeightDefControl = 21;
  public const int SizeHeightButton = 23;
  public const int SizeWidthButton = 85;
  public const int SizeHeightHeaderPanel = 37;
  public const int SizeWidthDefLabel = 300;
  public static readonly Color ColorHeaderPanel = Color.FromArgb(40, 2, 20, 50);

  private static XmlNode WriteObject(XmlDocument xml, string type, string name, bool isControl)
  {
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Control);
    XmlAttribute attribute1 = xml.CreateAttribute(FormTokenConsts.token_Type);
    attribute1.Value = type;
    if (element.Attributes != null)
    {
      element.Attributes.Append(attribute1);
      XmlAttribute attribute2 = xml.CreateAttribute(FormTokenConsts.token_Name);
      attribute2.Value = name;
      element.Attributes.Append(attribute2);
      XmlAttribute attribute3 = xml.CreateAttribute(FormTokenConsts.token_Assembly);
      attribute3.Value = FormTokenConsts.token_AssemblyNamespase;
      element.Attributes.Append(attribute3);
    }
    return element;
  }

  private static XmlNode WriteProperty(
    XmlDocument xml,
    string propName,
    Type propType,
    object propValue)
  {
    XmlNode element1 = (XmlNode) xml.CreateElement(FormTokenConsts.token_Property);
    XmlAttribute attribute1 = xml.CreateAttribute(FormTokenConsts.token_Name);
    XmlAttribute attribute2 = xml.CreateAttribute(FormTokenConsts.token_PropertyFormat);
    attribute1.Value = propName;
    if (element1.Attributes != null)
    {
      element1.Attributes.Append(attribute1);
      TypeConverter converter = TypeDescriptor.GetConverter(propType);
      if (converter.CanConvertTo(typeof (string)) && converter.CanConvertFrom(typeof (string)))
      {
        element1.InnerText = converter.ConvertToInvariantString(propValue);
        attribute2.Value = FormTokenConsts.token_Value;
      }
      else if (propType.IsSerializable)
      {
        MemoryStream serializationStream = new MemoryStream();
        new BinaryFormatter().Serialize((Stream) serializationStream, propValue);
        XmlNode element2 = (XmlNode) xml.CreateElement(FormTokenConsts.token_Binary);
        element2.InnerText = Convert.ToBase64String(serializationStream.ToArray());
        element1.AppendChild(element2);
        attribute2.Value = FormTokenConsts.token_Serialized;
      }
      if (element1.Attributes != null)
        element1.Attributes.Append(attribute2);
    }
    return element1;
  }

  private static void WriteSl(XmlDocument xml, XmlNode node, Point location, Size size)
  {
    XmlNode newChild1 = Scenario2XmlConverter.WriteProperty(node.OwnerDocument, FormTokenConsts.token_Location, typeof (Point), (object) location);
    XmlNode newChild2 = Scenario2XmlConverter.WriteProperty(node.OwnerDocument, FormTokenConsts.token_ClientSize, typeof (Size), (object) size);
    node.AppendChild(newChild1);
    node.AppendChild(newChild2);
  }

  private static XmlNode Form(XmlDocument xml, Size size, string text)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_DesForm, "desForm1", true);
    XmlNode newChild1 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Text, typeof (string), (object) text);
    XmlNode newChild2 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_ClientSize, typeof (Size), (object) size);
    XmlNode newChild3 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Dock, typeof (string), (object) "Fill");
    XmlNode newChild4 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AutoScroll, typeof (bool), (object) true);
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    element.AppendChild(newChild1);
    element.AppendChild(newChild2);
    element.AppendChild(newChild3);
    element.AppendChild(newChild4);
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  private static XmlNode Panel(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    string anchors,
    object backColor)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_Panel, name, true);
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    xmlNode.AppendChild(element);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    if (!string.IsNullOrEmpty(anchors))
    {
      XmlNode newChild = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Anchor, typeof (string), (object) anchors);
      element.AppendChild(newChild);
    }
    if (backColor == null)
      return xmlNode;
    XmlNode newChild1 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_BackColor, backColor.GetType(), backColor);
    element.AppendChild(newChild1);
    return xmlNode;
  }

  private static XmlNode Label(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    string text,
    ContentAlignment textAlignment,
    FormTokenConsts.BorderStyle borderStyle,
    object backColor)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_Label, name, true);
    XmlNode newChild1 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Text, typeof (string), (object) text);
    XmlNode newChild2 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AutoSize, typeof (bool), (object) false);
    XmlNode newChild3 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_TextAlign, typeof (ContentAlignment), (object) textAlignment);
    XmlNode newChild4 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_BorderStyle, typeof (FormTokenConsts.BorderStyle), (object) borderStyle);
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild1);
    element.AppendChild(newChild2);
    element.AppendChild(newChild3);
    element.AppendChild(newChild4);
    xmlNode.AppendChild(element);
    if (backColor == null)
      return xmlNode;
    XmlNode newChild5 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_BackColor, backColor.GetType(), backColor);
    element.AppendChild(newChild5);
    return xmlNode;
  }

  private static XmlNode ALabel(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid guid)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrLabel, name, true);
    XmlNode newChild = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AttributeInfo, typeof (string), (object) $"{Guid.Empty};{guid}");
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild);
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  private static XmlNode ACheckBox(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid guid,
    string text,
    string defaultValue,
    bool isExpert)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrCheckBox, name, true);
    XmlNode newChild1 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AttributeInfo, typeof (string), (object) $"{Guid.Empty};{guid}");
    XmlNode newChild2 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_UseInExpertSystem, typeof (bool), (object) isExpert);
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild1);
    element.AppendChild(newChild2);
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  private static XmlNode ATextBox(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid guid,
    string defaultValue,
    bool isExpert)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrTextEdit, name, true);
    XmlNode newChild1 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AttributeInfo, typeof (string), (object) $"{Guid.Empty};{guid}");
    XmlNode newChild2 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Text, typeof (string), (object) defaultValue);
    XmlNode newChild3 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_UseInExpertSystem, typeof (bool), (object) isExpert);
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild1);
    element.AppendChild(newChild2);
    element.AppendChild(newChild3);
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  private static XmlNode AMemoEdit(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid guid,
    string defaultValue,
    bool isExpert,
    string anchor = null)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrMemoEdit, name, true);
    XmlNode newChild1 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AttributeInfo, typeof (string), (object) $"{Guid.Empty};{guid}");
    XmlNode newChild2 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Multiline, typeof (bool), (object) true);
    XmlNode newChild3 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Text, typeof (string), (object) defaultValue);
    XmlNode newChild4 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_UseInExpertSystem, typeof (bool), (object) isExpert);
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild1);
    element.AppendChild(newChild2);
    element.AppendChild(newChild3);
    element.AppendChild(newChild4);
    if (!string.IsNullOrEmpty(anchor))
    {
      XmlNode newChild5 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Anchor, typeof (string), (object) anchor);
      element.AppendChild(newChild5);
    }
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  private static XmlNode AMeasuredEdit(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid guid,
    string defaultValue,
    bool isExpert)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrMeasuredEdit, name, true);
    XmlNode newChild1 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AttributeInfo, typeof (string), (object) $"{Guid.Empty};{guid}");
    XmlNode newChild2 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Text, typeof (string), (object) defaultValue);
    XmlNode newChild3 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_UseInExpertSystem, typeof (bool), (object) isExpert);
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild1);
    element.AppendChild(newChild2);
    element.AppendChild(newChild3);
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  private static XmlNode AButton(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    object action,
    string text,
    Bitmap imageData)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrButton, name, true);
    XmlNode newChild1 = !(action is string) ? Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_FormDesignerAction, typeof (FormDesignerAction), action) : Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Action, typeof (string), action);
    XmlNode newChild2 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Text, typeof (string), (object) text);
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild1);
    element.AppendChild(newChild2);
    if (imageData != null)
    {
      XmlNode newChild3 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Image, typeof (Bitmap), (object) imageData);
      element.AppendChild(newChild3);
    }
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  private static XmlNode ACheckListBox(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid guid)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrCheckListBox, name, true);
    XmlNode newChild = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AttributeInfo, typeof (string), (object) $"{Guid.Empty};{guid}");
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild);
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  private static XmlNode AComboBox(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid guid)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrComboBox, name, true);
    XmlNode newChild = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AttributeInfo, typeof (string), (object) $"{Guid.Empty};{guid}");
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild);
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  private static XmlNode ADateEdit(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid guid)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrDateEdit, name, true);
    XmlNode newChild = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AttributeInfo, typeof (string), (object) $"{Guid.Empty};{guid}");
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild);
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  private static XmlNode AImage(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid ipsObjectGuid)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_Image, name, true);
    XmlNode newChild1 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_ImageFromLibrary, typeof (string), (object) ipsObjectGuid.ToString());
    XmlNode newChild2 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Location, typeof (Point), (object) location);
    XmlNode newChild3 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Size, typeof (Size), (object) size);
    XmlNode newChild4 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_BackgroundImageLayout, typeof (string), (object) "Zoom");
    XmlNode newChild5 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_Anchor, typeof (string), (object) "Top, Bottom, Left, Right");
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    element.AppendChild(newChild1);
    element.AppendChild(newChild2);
    element.AppendChild(newChild3);
    element.AppendChild(newChild4);
    element.AppendChild(newChild5);
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  private static XmlNode AListBox(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid guid)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrListBox, name, true);
    XmlNode newChild = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AttributeInfo, typeof (string), (object) $"{Guid.Empty};{guid}");
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild);
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  private static XmlNode AListBoxBtn(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid guid)
  {
    XmlNode node = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrListBoxBtn, name, true);
    Scenario2XmlConverter.WriteSl(xml, node, location, size);
    XmlNode newChild = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AttributeInfo, typeof (string), (object) $"{Guid.Empty};{guid}");
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild);
    node.AppendChild(element);
    return node;
  }

  private static XmlNode AMeasuredListBox(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid guid)
  {
    XmlNode node = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrMeasuredListBox, name, true);
    Scenario2XmlConverter.WriteSl(xml, node, location, size);
    XmlNode newChild = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AttributeInfo, typeof (string), (object) $"{Guid.Empty};{guid}");
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild);
    node.AppendChild(element);
    return node;
  }

  private static XmlNode APassword(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid guid)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrPassword, name, true);
    XmlNode newChild = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AttributeInfo, typeof (string), (object) $"{Guid.Empty};{guid}");
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild);
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  private static XmlNode ATextBtn(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid guid,
    bool isExpert)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrTextBtn, name, true);
    XmlNode newChild = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AttributeInfo, typeof (string), (object) $"{Guid.Empty};{guid}");
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild);
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  private static XmlNode AEditObjectLink(
    XmlDocument xml,
    string name,
    Point location,
    Size size,
    Guid guid,
    bool isExpert)
  {
    XmlNode xmlNode = Scenario2XmlConverter.WriteObject(xml, FormTokenConsts.token_Control_AttrTextBtn, name, true);
    XmlNode newChild1 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_AttributeInfo, typeof (string), (object) $"{Guid.Empty};{guid}");
    XmlNode newChild2 = Scenario2XmlConverter.WriteProperty(xml, FormTokenConsts.token_UseInExpertSystem, typeof (bool), (object) isExpert);
    XmlNode element = (XmlNode) xml.CreateElement(FormTokenConsts.token_Properties);
    Scenario2XmlConverter.WriteSl(xml, element, location, size);
    element.AppendChild(newChild1);
    element.AppendChild(newChild2);
    xmlNode.AppendChild(element);
    return xmlNode;
  }

  public static XmlDocument SaveToXml(Scenario scen)
  {
    if (scen == null)
      return (XmlDocument) null;
    XmlDocument xml1 = new XmlDocument();
    bool flag = scen.Property.SlideId != 0 && !scen.Property.SlideGuid.Equals(Guid.Empty);
    int num1 = 8;
    int width = 8;
    int num2 = 8;
    for (int index1 = 0; index1 < scen.ColCount; ++index1)
    {
      int val1 = 0;
      for (int index2 = 0; index2 < scen.RowCount; ++index2)
      {
        ScenarioCell cell = scen.Cells[index1, index2];
        if (cell != null)
          val1 = Math.Max(val1, cell.Width);
      }
      width += val1 + 8;
    }
    for (int index3 = 0; index3 < scen.RowCount; ++index3)
    {
      int val1 = 21;
      for (int index4 = 0; index4 < scen.ColCount; ++index4)
      {
        ScenarioCell cell = scen.Cells[index4, index3];
        if (cell != null)
          val1 = Math.Max(val1, cell.Height);
      }
      num2 += val1 + 8;
    }
    int num3 = num2 + 31 /*0x1F*/;
    if (flag)
      width += num3;
    int num4 = num1 + 8;
    int height = num3 + 8;
    int y = num4;
    int num5 = 1;
    XmlNode newChild1 = Scenario2XmlConverter.Form(xml1, new Size(width, height), scen.ToString());
    XmlNode newChild2 = Scenario2XmlConverter.Panel(xml1, "header_panel", new Point(-1, 0), new Size(width - -2, 37), "Top, Left, Right", (object) Scenario2XmlConverter.ColorHeaderPanel);
    int int32_1 = Convert.ToInt32(8);
    XmlNode newChild3 = Scenario2XmlConverter.Label(xml1, "headerPanelLabel", new Point(8, int32_1), new Size(300, 21), scen.ToString(), ContentAlignment.MiddleLeft, FormTokenConsts.BorderStyle.None, (object) Color.Transparent);
    newChild2.AppendChild(newChild3);
    newChild1.AppendChild(newChild2);
    int num6 = num4 + 29;
    for (int index5 = 1; index5 < scen.RowCount; ++index5)
    {
      int x = 8;
      int val1 = 21;
      for (int index6 = 0; index6 < scen.ColCount; ++index6)
      {
        ScenarioCell cell = scen.Cells[index6, index5];
        if (cell != null)
        {
          CellValueType cellValueType = cell.Type;
          if (index6 >= 1 && index5 >= 1)
            cellValueType = CellValueType.Code;
          XmlNode newChild4 = (XmlNode) null;
          switch (cellValueType)
          {
            case CellValueType.Text:
              newChild4 = Scenario2XmlConverter.Label(xml1, $"label{num5++}", new Point(x, num6), new Size(cell.Width, cell.Height), cell.Value, ContentAlignment.MiddleLeft, FormTokenConsts.BorderStyle.None, (object) null);
              break;
            case CellValueType.Code:
              Guid guid;
              if (TechcardConsts.TechcardCommon.Code2AttributeGuid.TryGetValue(cell.Value, out guid))
              {
                IAttributeTypeItem byGuid = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid(guid);
                if (byGuid != null)
                {
                  FieldTypes fieldType;
                  MultiValueModes multiVal;
                  Scenario2XmlConverter.GetCellAttrType(cell, byGuid, out fieldType, out multiVal);
                  bool isReCountButton = cell.IsReCountButton;
                  if (multiVal.Equals((object) MultiValueModes.SingleValue))
                  {
                    switch (fieldType)
                    {
                      case FieldTypes.ftString:
                      case FieldTypes.ftInteger:
                      case FieldTypes.ftDouble:
                        cell.Height = 21;
                        newChild4 = Scenario2XmlConverter.ATextBox(xml1, $"attrTextEdit{num5++}", new Point(x, num6), new Size(cell.Width, cell.Height), guid, cell.DefaultValue, isReCountButton);
                        break;
                      case FieldTypes.ftObjectLink:
                        newChild4 = Scenario2XmlConverter.AEditObjectLink(xml1, $"attrobjectLinkEdit{num5++}", new Point(x, num6), new Size(cell.Width, cell.Height), guid, isReCountButton);
                        break;
                      case FieldTypes.ftMemo:
                        newChild4 = Scenario2XmlConverter.AMemoEdit(xml1, $"attrMemoEdit{num5++}", new Point(x, num6), new Size(cell.Width, cell.Height), guid, cell.DefaultValue, isReCountButton, cell.Anchor);
                        break;
                      case FieldTypes.ftBoolean:
                        newChild4 = Scenario2XmlConverter.ACheckBox(xml1, $"attrCheckBox{num5++}", new Point(x, num6), new Size(cell.Width, cell.Height), guid, byGuid.Name, cell.DefaultValue, isReCountButton);
                        break;
                      case FieldTypes.ftMeasured:
                        newChild4 = Scenario2XmlConverter.AMeasuredEdit(xml1, $"attrMeasureEdit{num5++}", new Point(x, num6), new Size(cell.Width, cell.Height), guid, cell.DefaultValue, isReCountButton);
                        break;
                    }
                  }
                  else
                    break;
                }
                else
                  break;
              }
              else
                break;
              break;
          }
          if (newChild4 != null)
            newChild1.AppendChild(newChild4);
          x += cell.Width + 8;
          val1 = Math.Max(val1, cell.Height);
        }
      }
      num6 += val1 + 8;
    }
    int num7 = width - 16 /*0x10*/ - 150;
    if (flag)
      num7 -= height;
    if (scen.Property.IsReCountButton)
    {
      int int32_2 = Convert.ToInt32(7);
      XmlDocument xml2 = xml1;
      int num8 = num5;
      int num9 = num8 + 1;
      string name1 = $"attrButton{num8}";
      Point location1 = new Point(width - 46 - 16 /*0x10*/, int32_2);
      Size size1 = new Size(23, 23);
      FormDesignerAction calcAction = FormTokenConsts.CalcAction;
      Bitmap calc = Resources.Calc;
      XmlNode newChild5 = Scenario2XmlConverter.AButton(xml2, name1, location1, size1, (object) calcAction, "", calc);
      newChild2.AppendChild(newChild5);
      XmlDocument xml3 = xml1;
      int num10 = num9;
      num5 = num10 + 1;
      string name2 = $"attrButton{num10}";
      Point location2 = new Point(width - 23 - 8, int32_2);
      Size size2 = new Size(23, 23);
      FormDesignerAction reCalcAction = FormTokenConsts.ReCalcAction;
      Bitmap reCalc = Resources.ReCalc;
      XmlNode newChild6 = Scenario2XmlConverter.AButton(xml3, name2, location2, size2, (object) reCalcAction, "", reCalc);
      newChild2.AppendChild(newChild6);
    }
    XmlNode xmlNode1 = newChild1;
    XmlDocument xml4 = xml1;
    int num11 = num5;
    int num12 = num11 + 1;
    string name3 = $"attrButton{num11}";
    Point location3 = new Point(num7 - 20, num6);
    Size size3 = new Size(85, 23);
    string action1 = FormTokenConsts.SimpleButtonActions.Apply.ToString();
    XmlNode newChild7 = Scenario2XmlConverter.AButton(xml4, name3, location3, size3, (object) action1, "Применить", (Bitmap) null);
    xmlNode1.AppendChild(newChild7);
    int num13 = num7 + 93;
    XmlNode xmlNode2 = newChild1;
    XmlDocument xml5 = xml1;
    int num14 = num12;
    int num15 = num14 + 1;
    string name4 = $"attrButton{num14}";
    Point location4 = new Point(num13 - 20, num6);
    Size size4 = new Size(85, 23);
    string action2 = FormTokenConsts.SimpleButtonActions.Cancel.ToString();
    XmlNode newChild8 = Scenario2XmlConverter.AButton(xml5, name4, location4, size4, (object) action2, "Отмена", (Bitmap) null);
    xmlNode2.AppendChild(newChild8);
    if (flag)
    {
      Point point = new Point(num13 + height + 85 + 8 - height, y);
      XmlDocument xml6 = xml1;
      int num16 = num15;
      int num17 = num16 + 1;
      string name5 = $"attrImage{num16}";
      Point location5 = point;
      Size size5 = new Size(num6, num6);
      Guid slideGuid = scen.Property.SlideGuid;
      XmlNode newChild9 = Scenario2XmlConverter.AImage(xml6, name5, location5, size5, slideGuid);
      newChild1.AppendChild(newChild9);
    }
    XmlNode element = (XmlNode) xml1.CreateElement(FormTokenConsts.token_FormDesignerXMLRoot);
    XmlAttribute attribute = xml1.CreateAttribute(FormTokenConsts.token_Version);
    attribute.Value = "2.0";
    if (element.Attributes != null)
      element.Attributes.Append(attribute);
    element.AppendChild(newChild1);
    xml1.AppendChild(element);
    return xml1;
  }

  private static void GetCellAttrType(
    ScenarioCell cell,
    IAttributeTypeItem attrType,
    out FieldTypes fieldType,
    out MultiValueModes multiVal)
  {
    if (cell.Value == "Тепр")
    {
      fieldType = FieldTypes.ftMemo;
      multiVal = attrType.MultiValueMode;
    }
    else
    {
      fieldType = (FieldTypes) attrType.AttrValueType;
      multiVal = attrType.MultiValueMode;
    }
  }
}

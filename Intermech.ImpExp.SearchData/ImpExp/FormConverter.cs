// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.FormConverter
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp;

public class FormConverter
{
  private FormParser _parser;
  private static string asmForms = "System.Windows.Forms";
  internal static string asmImCore = "Intermech.Client.Core";
  private static string asmWfDes = "Intermech.Workflow.Design";
  public static List<string> OldClasses = new List<string>((IEnumerable<string>) new string[19]
  {
    "TFreeLabel",
    "TFreeEdit",
    "TFreeComboBox",
    "TFreeCheckBox",
    "TFreeArchivesEdit",
    "TFreeUsersCombo",
    "TFreeUsersTree",
    "TFreePageControl",
    "TFreeTabSheet",
    "TFreePanel",
    "TFreeGroupBox",
    "TFreeDateEdit",
    "TFreeRadioGroup",
    "TFreeCustomCombo",
    "TFreeDocTypesCombo",
    "TFreeButton",
    "TFreeImage",
    "TFreeMemo",
    "TFreeTimeEdit"
  });
  public static List<ComponentInfo> NewClasses = new List<ComponentInfo>();
  public static List<ComponentInfo> LabelClasses = new List<ComponentInfo>();
  public static List<ComponentInfo> EditClasses = new List<ComponentInfo>();
  private static List<string> FontStyles = new List<string>((IEnumerable<string>) new string[4]
  {
    "fsBold",
    "fsItalic",
    "fsUnderline",
    "fsStrikeOut"
  });
  private static List<string> OldVarTags = new List<string>((IEnumerable<string>) new string[3]
  {
    "VarName",
    "InVarName",
    "OutVarName"
  });
  private static List<string> NewVarTags = new List<string>((IEnumerable<string>) new string[3]
  {
    "AttributeInfo",
    "SrcVariable",
    "DstVariable"
  });
  public static ComponentInfo AttrComboBox = (ComponentInfo) null;
  private Point ParentLocation;
  private Point BottomPoint;
  private bool _parsed;

  public event FormConverter.ConvertVarValue OnConvertVarValue;

  private static ComponentInfo AddComponent(string Name, string Assembly)
  {
    ComponentInfo componentInfo = new ComponentInfo(Name, Assembly);
    FormConverter.NewClasses.Add(componentInfo);
    return componentInfo;
  }

  private static ComponentInfo AddComponent(string Name) => FormConverter.AddComponent(Name, "");

  private Color DelphiColorToColor(string name)
  {
    if (name == "clBtnFace")
      return SystemColors.ButtonFace;
    if (name.Length <= 2)
      return SystemColors.WindowText;
    name = name.Remove(0, 2);
    return Color.FromName(name);
  }

  private string SerializeObject(object obj)
  {
    using (MemoryStream serializationStream = new MemoryStream())
    {
      new BinaryFormatter().Serialize((Stream) serializationStream, obj);
      return Convert.ToBase64String(serializationStream.ToArray());
    }
  }

  static FormConverter()
  {
    FormConverter.AddComponent("L");
    FormConverter.AddComponent("E");
    FormConverter.AttrComboBox = FormConverter.AddComponent("Intermech.Client.Core.FormDesigner.Controls.AttrComboBox");
    FormConverter.AddComponent("Intermech.Client.Core.FormDesigner.Controls.AttrCheckBox");
    FormConverter.AddComponent("Intermech.Client.Core.FormDesigner.Controls.AttrTextBtn");
    FormConverter.AddComponent("Intermech.Workflow.Design.UsersComboBox", FormConverter.asmWfDes);
    FormConverter.AddComponent("Intermech.Workflow.Design.UsersTreeView", FormConverter.asmWfDes);
    FormConverter.AddComponent("System.Windows.Forms.TabControl", FormConverter.asmForms);
    FormConverter.AddComponent("System.Windows.Forms.TabPage", FormConverter.asmForms);
    FormConverter.AddComponent("System.Windows.Forms.Panel", FormConverter.asmForms);
    FormConverter.AddComponent("System.Windows.Forms.GroupBox", FormConverter.asmForms);
    FormConverter.AddComponent("Intermech.Client.Core.FormDesigner.Controls.AttrDateEdit");
    FormConverter.AddComponent("Intermech.Workflow.Design.EnhRadioGroup", FormConverter.asmWfDes);
    FormConverter.AddComponent("FC");
    FormConverter.AddComponent("?");
    FormConverter.AddComponent("?");
    FormConverter.AddComponent("?");
    FormConverter.AddComponent("Intermech.Client.Core.FormDesigner.Controls.AttrMemoEdit");
    FormConverter.AddComponent("Intermech.Client.Core.FormDesigner.Controls.AttrDateEdit");
    FormConverter.LabelClasses.Add(new ComponentInfo("Intermech.Client.Core.FormDesigner.Controls.IMLabel"));
    FormConverter.LabelClasses.Add(new ComponentInfo("Intermech.Client.Core.FormDesigner.Controls.AttrLabel"));
    FormConverter.EditClasses.Add(new ComponentInfo("Intermech.Client.Core.FormDesigner.Controls.AttrTextEdit", FormConverter.asmImCore));
    FormConverter.EditClasses.Add(new ComponentInfo("Intermech.Client.Core.FormDesigner.Controls.AttrMemoEdit", FormConverter.asmImCore));
  }

  public FormConverter(string text) => this._parser = new FormParser(text);

  private void WriteProperty(XmlTextWriter writer, string name, string value, bool IsSerialized)
  {
    writer.WriteStartElement("Property");
    writer.WriteAttributeString("Name", name);
    if (IsSerialized)
      writer.WriteAttributeString("PropertyFormat", "Serialized");
    else
      writer.WriteAttributeString("PropertyFormat", "Value");
    writer.WriteString(value);
    writer.WriteEndElement();
  }

  private void WriteProperty(XmlTextWriter writer, string name, string value)
  {
    this.WriteProperty(writer, name, value, false);
  }

  private string ValueToString(object val)
  {
    switch (val)
    {
      case Size size:
        return $"{size.Width.ToString()}, {size.Height.ToString()}";
      case Point point:
        return $"{point.X.ToString()}, {point.Y.ToString()}";
      default:
        return val.ToString();
    }
  }

  private FontStyle FontStyleFromDelphiString(string s)
  {
    FontStyle fontStyle = FontStyle.Regular;
    s = s.Trim('[', ']');
    if (s != "")
    {
      for (int index = 0; index < FormConverter.FontStyles.Count; ++index)
      {
        if (s.Contains(FormConverter.FontStyles[index]))
          fontStyle |= (FontStyle) (1 << index);
      }
    }
    return fontStyle;
  }

  private void StreamObject(XmlTextWriter writer, FormObject obj)
  {
    bool flag = false;
    int index1 = FormConverter.OldClasses.IndexOf(obj.Class);
    ComponentInfo CI = index1 != -1 ? FormConverter.NewClasses[index1] : throw new InvalidCastException("Класс не имеет соответствия в IPS: " + obj.Class);
    if (CI.Name == "L")
    {
      CI = FormConverter.LabelClasses[Convert.ToInt32(obj.ContainsKey("VarName"))];
      flag = true;
    }
    else if (CI.Name == "E")
      CI = FormConverter.EditClasses[Convert.ToInt32(obj.ContainsKey("Multiline"))];
    else if (CI.Name == "FC")
    {
      CI = FormConverter.EditClasses[0];
      string str = "";
      if (obj.TryGetValue("VarName", out str) && this.OnConvertVarValue != null)
      {
        int num = this.OnConvertVarValue("VarName", ref str, ref CI) ? 1 : 0;
      }
    }
    string name = CI.Name;
    if (name == "?")
      return;
    writer.WriteStartElement("Control");
    writer.WriteAttributeString("Name", obj.Name);
    writer.WriteAttributeString("Type", name);
    writer.WriteAttributeString("Assembly", CI.Assembly);
    writer.WriteStartElement("Properties");
    string str1 = "";
    Point point1 = new Point();
    Point point2 = new Point();
    string str2 = "";
    string str3;
    if (obj.TryGetValue("Left", out str3) && obj.TryGetValue("Top", out str2))
    {
      str1 = $"{str3}, {str2}";
      this.WriteProperty(writer, "Location", str1);
      point1.X = Convert.ToInt32(str3);
      point1.Y = Convert.ToInt32(str2);
    }
    Font font = (Font) null;
    if (obj.ContainsKey("Font.Name"))
    {
      string familyName = obj["Font.Name"];
      string str4 = (string) null;
      if (obj.TryGetValue("Font.Height", out str4) && str4 != null)
      {
        float single = Convert.ToSingle(-0.6 * (double) Convert.ToInt32(str4));
        FontStyle style = FontStyle.Regular;
        if (obj.TryGetValue("Font.Style", out str1))
          style = this.FontStyleFromDelphiString(str1);
        font = new Font(familyName, single, style);
      }
    }
    string str5 = "";
    string str6;
    if (obj.TryGetValue("Width", out str6) && obj.TryGetValue("Height", out str5))
    {
      point2.X = Convert.ToInt32(str6);
      point2.Y = Convert.ToInt32(str5);
      if (flag && font != null)
        point2.X += (int) ((double) font.Size / 2.0);
      str1 = $"{point2.X.ToString()}, {str5}";
      this.WriteProperty(writer, "Size", str1);
    }
    if (this.BottomPoint.X < this.ParentLocation.X + point1.X + point2.X)
      this.BottomPoint.X = this.ParentLocation.X + point1.X + point2.X;
    if (this.BottomPoint.Y < this.ParentLocation.Y + point1.Y + point2.Y)
      this.BottomPoint.Y = this.ParentLocation.Y + point1.Y + point2.Y;
    if (obj.ContainsKey("Text"))
      this.WriteProperty(writer, "Text", obj["Text"]);
    else if (obj.ContainsKey("Caption"))
      this.WriteProperty(writer, "Text", obj["Caption"]);
    if (obj.ContainsKey("Color"))
    {
      Color color = this.DelphiColorToColor(obj["Color"]);
      this.WriteProperty(writer, "BackColor", this.SerializeObject((object) color), true);
    }
    if (font != null)
    {
      this.WriteProperty(writer, "Font", this.SerializeObject((object) font), true);
      if (obj.TryGetValue("Font.Color", out str1))
      {
        Color color = this.DelphiColorToColor(str1);
        this.WriteProperty(writer, "ForeColor", this.SerializeObject((object) color), true);
      }
    }
    if (obj.ContainsKey("Align"))
    {
      string str7 = obj["Align"];
      if (str7.Length > 2)
      {
        string str8 = str7.Remove(0, 2);
        if (str8 != "Custom")
        {
          if (str8 == "Client")
            str8 = "Fill";
          this.WriteProperty(writer, "Dock", str8);
        }
      }
    }
    if (this.OnConvertVarValue != null)
    {
      for (int index2 = 0; index2 < FormConverter.OldVarTags.Count; ++index2)
      {
        if (obj.TryGetValue(FormConverter.OldVarTags[index2], out str1) && this.OnConvertVarValue(FormConverter.OldVarTags[index2], ref str1, ref CI))
        {
          if (FormConverter.NewVarTags[index2] == "AttributeInfo")
            str1 = $"{Guid.Empty.ToString()};{str1}";
          this.WriteProperty(writer, FormConverter.NewVarTags[index2], str1);
        }
      }
    }
    writer.WriteEndElement();
    foreach (FormObject child in (List<FormObject>) obj.Children)
    {
      this.ParentLocation.X += point1.X;
      this.ParentLocation.Y += point1.Y;
      this.StreamObject(writer, child);
      this.ParentLocation.X -= point1.X;
      this.ParentLocation.Y -= point1.Y;
    }
    writer.WriteEndElement();
  }

  private void CheckParsed()
  {
    if (this._parsed)
      return;
    this._parser.Parse();
    this._parsed = true;
  }

  public bool Empty
  {
    get
    {
      this.CheckParsed();
      return this._parser.Objects.Count == 0;
    }
  }

  public void SaveToStream(Stream stream)
  {
    this.CheckParsed();
    XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
    writer.Formatting = Formatting.Indented;
    writer.WriteStartDocument();
    writer.WriteStartElement("FormDesignerXMLRoot");
    writer.WriteAttributeString("Version", "2.0");
    writer.WriteStartElement("Control");
    writer.WriteAttributeString("Name", "desFormImp");
    writer.WriteAttributeString("Type", "Intermech.Client.Core.FormDesigner.Controls.DesForm");
    writer.WriteAttributeString("Assembly", FormConverter.asmImCore);
    foreach (FormObject formObject in (List<FormObject>) this._parser.Objects)
      this.StreamObject(writer, formObject);
    writer.WriteStartElement("Properties");
    this.BottomPoint.X += 10;
    this.BottomPoint.Y += 10;
    this.WriteProperty(writer, "Size", $"{(object) this.BottomPoint.X}, {(object) this.BottomPoint.Y}");
    this.WriteProperty(writer, "ClientSize", $"{(object) this.BottomPoint.X}, {(object) this.BottomPoint.Y}");
    this.WriteProperty(writer, "Name", "desFormImp");
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndDocument();
    writer.Flush();
  }

  public delegate bool ConvertVarValue(string Name, ref string Value, ref ComponentInfo CI);
}

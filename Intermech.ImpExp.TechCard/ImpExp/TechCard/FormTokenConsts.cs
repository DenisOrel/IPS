// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.FormTokenConsts
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard;

[Serializable]
public class FormTokenConsts
{
  public static FormDesignerAction CalcAction = new FormDesignerAction(new Guid("F93C7B60-BFBD-4213-A6FE-DC5CDD252D25"), "Рассчитать");
  public static FormDesignerAction ReCalcAction = new FormDesignerAction(new Guid("DCB9C7B7-6BD4-4a85-B651-533E220310C7"), "Пересчитать");
  public static readonly string token_BackgroundImage = "BackgroundImage";
  public static readonly string token_BackColor = "BackColor";
  public static readonly string token_Image = "Image";
  public static readonly string token_Properties = "Properties";
  public static readonly string token_Property = "Property";
  public static readonly string token_Version = "Version";
  public static readonly string token_FormDesignerXMLRoot = "FormDesignerXMLRoot";
  public static readonly string token_Control = "Control";
  public static readonly string token_Type = "Type";
  public static readonly string token_Name = "Name";
  public static readonly string token_Assembly = "Assembly";
  public static readonly string token_AttributeInfo = "AttributeInfo";
  public static readonly string token_ClientSize = "ClientSize";
  public static readonly string token_Dock = "Dock";
  public static readonly string token_BackgroundImageLayout = "BackgroundImageLayout";
  public static readonly string token_Anchor = "Anchor";
  public static readonly string token_Size = "Size";
  public static readonly string token_Action = "Action";
  public static readonly string token_FormDesignerAction = "FormDesignerAction";
  public static readonly string token_Text = "Text";
  public static readonly string token_ImageFromLibrary = "ImageFromLibrary";
  public static readonly string token_UseInExpertSystem = "UseInExpertSystem";
  public static readonly string token_Multiline = "Multiline";
  public static readonly string token_AutoSize = "AutoSize";
  public static readonly string token_AutoScroll = "AutoScroll";
  public static readonly string token_PropertyFormat = "PropertyFormat";
  public static readonly string token_BorderStyle = "BorderStyle";
  public static readonly string token_Location = "Location";
  public static readonly string token_Value = "Value";
  public static readonly string token_Binary = "Binary";
  public static readonly string token_Serialized = "Serialized";
  public static readonly string token_TextAlign = "TextAlign";
  public static readonly string token_Center = "Center";
  public static readonly string token_Control_AttrCheckBox = "Intermech.Client.Core.FormDesigner.Controls.AttrCheckBox";
  public static readonly string token_Control_AttrButton = "Intermech.Client.Core.FormDesigner.Controls.AttrButton";
  public static readonly string token_Control_AttrCheckListBox = "Intermech.Client.Core.FormDesigner.Controls.AttrCheckListBox";
  public static readonly string token_Control_AttrComboBox = "Intermech.Client.Core.FormDesigner.Controls.AttrComboBox";
  public static readonly string token_Control_AttrDateEdit = "Intermech.Client.Core.FormDesigner.Controls.AttrDateEdit";
  public static readonly string token_Control_AttrLabel = "Intermech.Client.Core.FormDesigner.Controls.AttrLabel";
  public static readonly string token_Control_AttrListBox = "Intermech.Client.Core.FormDesigner.Controls.AttrListBox";
  public static readonly string token_Control_AttrListBoxBtn = "Intermech.Client.Core.FormDesigner.Controls.AttrListBoxBtn";
  public static readonly string token_Control_AttrMeasuredEdit = "Intermech.Client.Core.FormDesigner.Controls.AttrMeasuredEdit";
  public static readonly string token_Control_AttrMeasuredListBox = "Intermech.Client.Core.FormDesigner.Controls.AttrMeasuredListBox";
  public static readonly string token_Control_AttrMemoEdit = "Intermech.Client.Core.FormDesigner.Controls.AttrMemoEdit";
  public static readonly string token_Control_AttrPassword = "Intermech.Client.Core.FormDesigner.Controls.AttrPassword";
  public static readonly string token_Control_AttrTextBtn = "Intermech.Client.Core.FormDesigner.Controls.AttrTextBtn";
  public static readonly string token_Control_AttrTextEdit = "Intermech.Client.Core.FormDesigner.Controls.AttrTextEdit";
  public static readonly string token_AssemblyNamespase = "Intermech.Client.Core";
  public static readonly string token_Control_Label = "System.Windows.Forms.Label";
  public static readonly string token_Control_DesForm = "Intermech.Client.Core.FormDesigner.Controls.DesForm";
  public static readonly string token_Control_Image = "Intermech.Client.Core.FormDesigner.Controls.IMPictureBox";
  public static readonly string token_Control_Panel = "Intermech.Client.Core.FormDesigner.Controls.IMPanel";

  internal enum PropertyFormat
  {
    Value,
    Serialized,
  }

  internal enum BorderStyle
  {
    None,
    FixedSingle,
    Fixed3D,
  }

  internal enum SimpleButtonActions
  {
    Apply,
    Cancel,
    Calc,
    ReCalc,
  }
}

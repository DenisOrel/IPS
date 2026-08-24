// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ColorTable
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class ColorTable
{
  public static Color Arrow = ColorTable.FromHex("#678CBD");
  public static Color ArrowLight = Color.FromArgb(200, Color.White);
  public static Color ArrowDisabled = ColorTable.FromHex("#B7B7B7");
  public static Color Text = SystemColors.WindowText;
  public static Color RibbonBackground = SystemColors.Window;
  public static Color TabBorder = SystemColors.ControlDark;
  public static Color DropDownBg = ColorTable.FromHex("#FAFAFA");
  public static Color DropDownImageBg = ColorTable.FromHex("#E9EEEE");
  public static Color DropDownImageSeparator = ColorTable.FromHex("#C5C5C5");
  public static Color DropDownBorder = ColorTable.FromHex("#868686");
  public static Color ButtonBgOut = SystemColors.Window;
  public static Color ButtonBgCenter = ColorTable.FromHex("#CFE0F7");
  public static Color ButtonBorderOut = ColorTable.FromHex("#B9D0ED");
  public static Color ButtonBorderIn = ColorTable.FromHex("#E3EDFB");
  public static Color ButtonGlossyNorth = ColorTable.FromHex("#DEEBFE");
  public static Color ButtonGlossySouth = ColorTable.FromHex("#CBDEF6");
  public static Color ButtonPressedBgOut = ColorTable.FromHex("#F88F2C");
  public static Color ButtonPressedBgCenter = ColorTable.FromHex("#FDF1B0");
  public static Color ButtonPressedBorderOut = ColorTable.FromHex("#8E8165");
  public static Color ButtonPressedBorderIn = ColorTable.FromHex("#F9C65A");
  public static Color ButtonPressedGlossyNorth = ColorTable.FromHex("#FDD5A8");
  public static Color ButtonPressedGlossySouth = ColorTable.FromHex("#FBB062");
  public static Color ButtonSelectedBgOut = ColorTable.FromHex("#FFD646");
  public static Color ButtonSelectedBgCenter = ColorTable.FromHex("#FFEAAC");
  public static Color ButtonSelectedBorderOut = ColorTable.FromHex("#C2A978");
  public static Color ButtonSelectedBorderIn = ColorTable.FromHex("#FFF2C7");
  public static Color ButtonSelectedGlossyNorth = ColorTable.FromHex("#FFFDDB");
  public static Color ButtonSelectedGlossySouth = ColorTable.FromHex("#FFE793");
  public static Color PanelDarkBorder = SystemColors.Control;
  public static Color PanelLightBorder = SystemColors.ControlLightLight;
  public static Color PanelBackgroundSelected = SystemColors.Control;
  public static Color PanelTextBackground = ColorTable.FromHex("#E0E0E0");
  public static Color PanelOverflowBackgroundPressed = SystemColors.ControlLight;
  public static Color PanelOverflowBackgroundSelectedNorth = SystemColors.ControlLight;
  public static Color PanelOverflowBackgroundSelectedSouth = SystemColors.ControlLight;
  public static Color PanelTextBackgroundSelected = Color.LightGray;
  public static Color PanelText = SystemColors.ControlLight;
  public static Color TabNorth = SystemColors.ControlLight;
  public static Color TabSouth = SystemColors.Control;
  public static Color TabContentNorth = SystemColors.ControlLightLight;
  public static Color TabContentSouth = SystemColors.Control;
  public static Color TabSelectedGlow = SystemColors.ControlLightLight;
  public static Color TabText = SystemColors.WindowText;
  public static Color TabActiveText = SystemColors.ControlText;

  internal static Color FromHex(string hex)
  {
    if (hex.StartsWith("#"))
      hex = hex.Substring(1);
    return hex.Length == 6 ? Color.FromArgb(int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber), int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber), int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber)) : throw new Exception("Color not valid");
  }
}

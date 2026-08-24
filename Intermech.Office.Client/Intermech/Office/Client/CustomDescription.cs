// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.CustomDescription
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using System.ComponentModel;

#nullable disable
namespace Intermech.Office.Client;

internal class CustomDescription : DescriptionAttribute
{
  public CustomDescription([NotNull] string description)
  {
    this.DescriptionValue = Localization.GetAttributesString(description);
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ComboBoxItem
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

#nullable disable
namespace Intermech.Office.Client;

public class ComboBoxItem
{
  public string Name;
  public string Value;

  public ComboBoxItem(string name, string value)
  {
    this.Name = name;
    this.Value = value;
  }

  public override string ToString() => this.Name;
}

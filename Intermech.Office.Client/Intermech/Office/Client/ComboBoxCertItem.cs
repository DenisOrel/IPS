// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ComboBoxCertItem
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Intermech.Office.Client;

public class ComboBoxCertItem
{
  public string Name;
  public X509Certificate2 Value;

  public ComboBoxCertItem(string name, X509Certificate2 value)
  {
    this.Name = name;
    this.Value = value;
  }

  public override string ToString() => this.Name;
}

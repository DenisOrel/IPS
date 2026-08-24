// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.CCBoxItem
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

#nullable disable
namespace Intermech.Office.Client;

public class CCBoxItem
{
  private int _val;
  private string _name;
  private string _smdoID;

  public int Value
  {
    get => this._val;
    set => this._val = value;
  }

  public string Name
  {
    get => this._name;
    set => this._name = value;
  }

  public string SMDOID
  {
    get => this._smdoID;
    set => this._smdoID = value;
  }

  public CCBoxItem()
  {
  }

  public CCBoxItem(string name, int val, string smdoID)
  {
    this._name = name;
    this._val = val;
    this._smdoID = smdoID;
  }

  public override string ToString()
  {
    return $"name: '{this._name}', value: '{this._val}', smdoID: '{this._smdoID}'";
  }
}

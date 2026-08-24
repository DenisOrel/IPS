// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.S4DBItem
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

#nullable disable
namespace Intermech.ImpExp.SearchData;

public class S4DBItem
{
  public S4Table Data = new S4Table();
  public S4Table AddData = new S4Table();
  public S4Table ThemeData = new S4Table();
  public S4Table CommonParamsData = new S4Table();
  protected int _id;
  private string _designation = "@#$";
  private string _name = "@#$";

  public string Designation
  {
    get
    {
      if (this._designation == "@#$")
        this._designation = this.getDesignation();
      return this._designation;
    }
  }

  protected virtual string getDesignation() => this.Data["designatio"].ToString();

  public string Name
  {
    get
    {
      if (this._name == "@#$")
        this._name = this.Data["name"].ToString();
      return this._name;
    }
  }

  internal virtual void Clear()
  {
    this.Data.Clear();
    this.AddData.Clear();
    this.ThemeData.Clear();
    this.CommonParamsData.Clear();
    this._id = 0;
    this._name = "@#$";
    this._designation = "@#$";
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ImStorageInfo
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal class ImStorageInfo
{
  private readonly string _alias;
  private string _path;
  public readonly string RawAlias;

  public string Alias => this._alias;

  public string Path
  {
    get => this._path;
    set => this._path = value;
  }

  public ImStorageInfo(string rawAlias, string alias, string path)
  {
    this.RawAlias = rawAlias;
    this._alias = alias;
    this._path = path;
  }
}

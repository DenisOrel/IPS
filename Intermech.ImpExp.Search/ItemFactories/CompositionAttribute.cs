// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.CompositionAttribute
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class CompositionAttribute : ICompositionAttribute
{
  private int _paramID;
  private string _label = string.Empty;
  private string _field = string.Empty;
  private bool _isImbaseLink;
  private int _size;
  private int _isInherited;

  public int ParamID
  {
    get => this._paramID;
    set => this._paramID = value;
  }

  public string Name
  {
    get => this._label;
    set => this._label = value;
  }

  public string DBField
  {
    get => this._field;
    set => this._field = value;
  }

  public bool IsImbaseLink
  {
    get => this._isImbaseLink;
    set => this._isImbaseLink = value;
  }

  public int Size
  {
    get => this._size;
    set => this._size = value;
  }

  public int IsInherited
  {
    get => this._isInherited;
    set => this._isInherited = value;
  }
}

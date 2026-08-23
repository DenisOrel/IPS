// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.PROV_ENUMALGS_EX
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System.IO;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>
/// Структура с описанием алгоритма поддреживаемого криптопровайдером
/// </summary>
public struct PROV_ENUMALGS_EX
{
  /// <summary>ALG_ID для идентификации алгоритма</summary>
  public int aiAlgid;
  /// <summary>длина ключа по умолчанию</summary>
  public int dwDefaultLen;
  /// <summary>минимальная длина ключа</summary>
  public int dwMinLen;
  /// <summary>максимальная длина ключа</summary>
  public int dwMaxLen;
  /// <summary>какие то флаги или 0</summary>
  public int dwProtocols;
  /// <summary>длина имени алгоритма</summary>
  public int dwNameLen;
  /// <summary>имя алгоритма</summary>
  public string szName;
  /// <summary>длина полного имения алгоритма</summary>
  public int dwLongNameLen;
  /// <summary>полное имя алгоритма</summary>
  public string szLongName;

  /// <summary>конструктор</summary>
  /// <param name="data"></param>
  public PROV_ENUMALGS_EX(byte[] data)
  {
    BinaryReader binaryReader = new BinaryReader((Stream) new MemoryStream(data));
    this.aiAlgid = binaryReader.ReadInt32();
    this.dwDefaultLen = binaryReader.ReadInt32();
    this.dwMinLen = binaryReader.ReadInt32();
    this.dwMaxLen = binaryReader.ReadInt32();
    this.dwProtocols = binaryReader.ReadInt32();
    this.dwNameLen = binaryReader.ReadInt32();
    this.szName = new string(binaryReader.ReadChars(20)).TrimEnd(new char[1]);
    this.dwLongNameLen = binaryReader.ReadInt32();
    this.szLongName = new string(binaryReader.ReadChars(40)).TrimEnd(new char[1]);
  }

  /// <summary>конструктор</summary>
  /// <param name="empty"></param>
  private PROV_ENUMALGS_EX(bool empty)
  {
    this.aiAlgid = 0;
    this.dwDefaultLen = 0;
    this.dwMinLen = 0;
    this.dwMaxLen = 0;
    this.dwProtocols = 0;
    this.dwNameLen = 0;
    this.szName = string.Empty;
    this.dwLongNameLen = 0;
    this.szLongName = string.Empty;
  }

  /// <summary>
  /// 
  /// </summary>
  public static PROV_ENUMALGS_EX Empty => new PROV_ENUMALGS_EX();

  /// <summary>вернуть полное имя алгоритма</summary>
  /// <returns></returns>
  public override string ToString() => this.szLongName;

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public override int GetHashCode() => this.aiAlgid;
}

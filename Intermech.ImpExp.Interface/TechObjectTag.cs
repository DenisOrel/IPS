// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.TechObjectTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>таг для хранения сложных технологических структур</summary>
/// <summary>Конструктор</summary>
/// <param name="techObject">Технологический объект</param>
public struct TechObjectTag(object techObject) : ITagImportObject
{
  /// <summary>Технологический объект</summary>
  private object _techObject = techObject;

  /// <summary>Технологический объект</summary>
  public object Object
  {
    get => this._techObject;
    set => this._techObject = value;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public byte[] Save()
  {
    using (MemoryStream serializationStream = new MemoryStream())
    {
      new BinaryFormatter().Serialize((Stream) serializationStream, this._techObject);
      return serializationStream.ToArray();
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="s"></param>
  public void Load(byte[] s)
  {
    using (MemoryStream serializationStream = new MemoryStream(s))
      this._techObject = new BinaryFormatter().Deserialize((Stream) serializationStream);
  }

  /// <summary>
  /// 
  /// </summary>
  public short ClassID => 13;
}

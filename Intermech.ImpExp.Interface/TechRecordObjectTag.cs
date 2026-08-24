// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.TechRecordObjectTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Таг для хранения структур записей ТП / расцеховки</summary>
public class TechRecordObjectTag : ITagImportObject
{
  /// <summary>Cтруктура - описание записи ТП / расцеховки / ..</summary>
  private object _techObject;
  /// <summary>Структура - описание клонов объекта</summary>
  private TechDiffTag _techDiffTag;

  /// <summary>Конструктор</summary>
  public TechRecordObjectTag()
    : this((object) null)
  {
  }

  /// <summary>Конструктор</summary>
  /// <param name="techObject">Cтруктура - описание записи ТП / расцеховки / ..</param>
  public TechRecordObjectTag(object techObject)
    : this(techObject, (TechDiffTag) null)
  {
  }

  /// <summary>Конструктор</summary>
  /// <param name="techObject">Cтруктура - описание записи ТП / расцеховки / ..</param>
  /// <param name="techDiffTag">Структура - описание клонов объекта</param>
  public TechRecordObjectTag(object techObject, TechDiffTag techDiffTag)
  {
    this._techObject = techObject;
    this._techDiffTag = techDiffTag;
  }

  /// <summary>Cтруктура - описание записи ТП / расцеховки / ..</summary>
  public object Object
  {
    get => this._techObject;
    set => this._techObject = value;
  }

  /// <summary>Структура - описание клонов объекта</summary>
  public TechDiffTag TechDiffTag
  {
    get => this._techDiffTag;
    set => this._techDiffTag = value;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public byte[] Save()
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      byte num = 0;
      if (this._techObject != null)
        num |= (byte) 1;
      if (this._techDiffTag != null)
        num |= (byte) 2;
      BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream, Encoding.UTF8);
      try
      {
        binaryWriter.Write(num);
      }
      finally
      {
        binaryWriter.Flush();
      }
      if (this._techObject != null)
        new BinaryFormatter().Serialize((Stream) memoryStream, this._techObject);
      if (this._techDiffTag != null)
      {
        byte[] buffer = this._techDiffTag.Save();
        memoryStream.Write(buffer, 0, buffer.Length);
      }
      return memoryStream.ToArray();
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="s"></param>
  public void Load(byte[] s)
  {
    using (MemoryStream memoryStream = new MemoryStream(s))
    {
      int num = (int) new BinaryReader((Stream) memoryStream, Encoding.UTF8).ReadByte();
      if ((num & 1) != 0)
        this._techObject = new BinaryFormatter().Deserialize((Stream) memoryStream);
      if ((num & 2) == 0 || memoryStream.Position == memoryStream.Length)
        return;
      byte[] numArray = new byte[memoryStream.Length - memoryStream.Position];
      memoryStream.Read(numArray, 0, (int) (memoryStream.Length - memoryStream.Position));
      this._techDiffTag = new TechDiffTag();
      this._techDiffTag.Load(numArray);
    }
  }

  /// <summary>Уникальный идентификатор класса</summary>
  public short ClassID => 22;
}

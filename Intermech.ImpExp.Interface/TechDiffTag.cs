// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.TechDiffTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// TechCard.
/// Таг клонов объекта
/// </summary>
public class TechDiffTag : ITagImportObject
{
  /// <summary>
  /// Список всех клонов относящихся
  /// к ДАННОМУ ОБЪЕКТУ: key - F_ARTTCKEY, value - ips object id
  /// </summary>
  public Dictionary<int, long> _cloneList;

  /// <summary>
  /// Список всех клонов относящихся
  /// к ДАННОМУ ОБЪЕКТУ: key - F_ARTTCKEY, value - ips object id
  /// </summary>
  /// <remarks>Для экономии места в кеше используем SortedList</remarks>
  public Dictionary<int, long> CloneList
  {
    get => this._cloneList ?? (this._cloneList = new Dictionary<int, long>());
    set => this._cloneList = value;
  }

  /// <summary>Проверка на наличие списка клонов и данных в нем</summary>
  public bool IsCloneListEmpty => this._cloneList == null || this._cloneList.Count == 0;

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        if (this.IsCloneListEmpty)
        {
          binaryWriter.Write(0);
        }
        else
        {
          binaryWriter.Write(this.CloneList.Count);
          foreach (KeyValuePair<int, long> clone in this.CloneList)
          {
            binaryWriter.Write(clone.Key);
            binaryWriter.Write(clone.Value);
          }
        }
      }
      finally
      {
        binaryWriter.Flush();
      }
      return output.ToArray();
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="s"></param>
  public void Load(byte[] s)
  {
    using (MemoryStream input = new MemoryStream(s))
    {
      BinaryReader binaryReader = new BinaryReader((Stream) input, Encoding.UTF8);
      try
      {
        int capacity = binaryReader.ReadInt32();
        if (capacity != 0)
          this._cloneList = new Dictionary<int, long>(capacity);
        for (int index = 0; index < capacity; ++index)
          this.CloneList.Add(binaryReader.ReadInt32(), binaryReader.ReadInt64());
      }
      finally
      {
        binaryReader.Close();
      }
    }
  }

  /// <summary>
  /// 
  /// </summary>
  public short ClassID => 11;

  /// <summary>Получение тага из элемента кеша</summary>
  /// <returns></returns>
  public static TechDiffTag GetDiffTag(DictionaryValue dictValue)
  {
    if (dictValue == null)
      return (TechDiffTag) null;
    return !(dictValue.Tag is TechRecordObjectTag tag) ? (TechDiffTag) null : tag.TechDiffTag;
  }
}

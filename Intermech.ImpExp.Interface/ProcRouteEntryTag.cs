// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ProcRouteEntryTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Запись о входимостях в МО для хранения в кэше</summary>
public class ProcRouteEntryTag : ITagImportObject
{
  public ProcRouteEntryTag() => this.ProcRouteId = Guid.Empty;

  public ProcRouteEntryTag(Guid procRouteId) => this.ProcRouteId = procRouteId;

  /// <summary>Идентификатор маршрута обработки</summary>
  public Guid ProcRouteId { get; private set; }

  /// <summary>Идентификатор объекта МО в Ips(если уже закачан)</summary>
  public long IpsProcRouteId { get; set; }

  /// <summary>Список идентификаторов объектов-входимостей в IPS</summary>
  public ISet<long> Entries { get; } = (ISet<long>) new HashSet<long>();

  /// <summary>Идентификатор объекта-владельца МО в Ips</summary>
  public long IpsOwnerObjId { get; set; }

  /// <summary>Идентификатор типа объекта-владельца МО в Ips</summary>
  /// <remarks>Объектом-владельцем может выступать как изделие, так и его производственная копия(для ПВ)</remarks>
  public int IpsOwnerObjTypeId { get; set; }

  /// <summary>Загрузка содержимого из бинарных данных</summary>
  /// <param name="ms"></param>
  public void Load(BinaryReader br)
  {
    Guid result;
    this.ProcRouteId = Guid.TryParse(br.ReadString(), out result) ? result : Guid.Empty;
    this.IpsProcRouteId = br.ReadInt64();
    this.IpsOwnerObjId = br.ReadInt64();
    this.IpsOwnerObjTypeId = br.ReadInt32();
    int num1 = br.ReadInt32();
    for (int index = 0; index < num1; ++index)
    {
      long num2 = br.ReadInt64();
      if (num2 != 0L)
        this.Entries.Add(num2);
    }
  }

  /// <summary>Сохранение содержимого в бинарном формате</summary>
  /// <returns></returns>
  public void Save(BinaryWriter bw)
  {
    bw.Write(this.ProcRouteId.ToString());
    bw.Write(this.IpsProcRouteId);
    bw.Write(this.IpsOwnerObjId);
    bw.Write(this.IpsOwnerObjTypeId);
    bw.Write(this.Entries.Count);
    foreach (long entry in (IEnumerable<long>) this.Entries)
      bw.Write(entry);
  }

  public short ClassID => 31 /*0x1F*/;

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter bw = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        this.Save(bw);
      }
      finally
      {
        bw.Flush();
      }
      return output.ToArray();
    }
  }

  public void Load(byte[] s)
  {
    using (MemoryStream input = new MemoryStream(s))
    {
      BinaryReader br = new BinaryReader((Stream) input, Encoding.UTF8);
      try
      {
        this.Load(br);
      }
      finally
      {
        br.Close();
      }
    }
  }
}

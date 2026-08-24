// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ProcRoutesTag
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

/// <summary>
/// Cгруппированные по МО входимости в маршруты обработки, которые необходимо создать
/// </summary>
public class ProcRoutesTag : ITagImportObject
{
  /// <summary>Список сгруппированных по Guid МО входимостях</summary>
  public Dictionary<Guid, ProcRouteEntryTag> Entries { get; } = new Dictionary<Guid, ProcRouteEntryTag>();

  /// <summary>Обрабатывались ли записи</summary>
  public bool Processed { get; set; }

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter bw = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        bw.Write(this.Entries.Count);
        foreach (ProcRouteEntryTag procRouteEntryTag in this.Entries.Values)
          procRouteEntryTag.Save(bw);
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
        int num = br.ReadInt32();
        for (int index = 0; index < num; ++index)
        {
          ProcRouteEntryTag procRouteEntryTag = new ProcRouteEntryTag();
          procRouteEntryTag.Load(br);
          this.Entries.Add(procRouteEntryTag.ProcRouteId, procRouteEntryTag);
        }
      }
      finally
      {
        br.Close();
      }
    }
  }

  public short ClassID => 32 /*0x20*/;
}

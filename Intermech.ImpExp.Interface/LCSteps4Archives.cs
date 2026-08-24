// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.LCSteps4Archives
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class LCSteps4Archives : ITagImportObject
{
  public Dictionary<int, int> LCSteps4;

  public LCSteps4Archives() => this.LCSteps4 = new Dictionary<int, int>();

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        int count = this.LCSteps4 == null || this.LCSteps4.Count <= 0 ? 0 : this.LCSteps4.Count;
        binaryWriter.Write(count);
        if (this.LCSteps4 != null)
        {
          if (this.LCSteps4.Count > 0)
          {
            IDictionaryEnumerator enumerator = (IDictionaryEnumerator) this.LCSteps4.GetEnumerator();
            while (enumerator.MoveNext())
            {
              binaryWriter.Write((int) enumerator.Key);
              binaryWriter.Write((int) enumerator.Value);
            }
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

  public void Load(byte[] s)
  {
    using (MemoryStream input = new MemoryStream(s))
    {
      BinaryReader binaryReader = new BinaryReader((Stream) input, Encoding.UTF8);
      try
      {
        int capacity = binaryReader.ReadInt32();
        if (capacity <= 0)
          return;
        this.LCSteps4 = new Dictionary<int, int>(capacity);
        for (int index = 0; index < capacity; ++index)
          this.LCSteps4.Add(binaryReader.ReadInt32(), binaryReader.ReadInt32());
      }
      finally
      {
        binaryReader.Close();
      }
    }
  }

  public short ClassID => 14;
}

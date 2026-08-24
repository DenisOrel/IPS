// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.UserTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class UserTag : ITagImportObject
{
  public Guid Guid = Guid.Empty;

  public UserTag()
  {
  }

  public UserTag(Guid guid) => this.Guid = guid;

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter bw = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        TagImportObjectHelper.SetString(this.Guid != Guid.Empty ? this.Guid.ToString() : string.Empty, bw);
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
        string str = TagImportObjectHelper.GetString(br);
        if (GuidHelper.IsGuid(str))
          this.Guid = new Guid(str);
        else
          this.Guid = Guid.Empty;
      }
      finally
      {
        br.Close();
      }
    }
  }

  public short ClassID => 21;
}

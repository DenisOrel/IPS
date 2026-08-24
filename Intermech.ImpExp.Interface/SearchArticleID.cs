// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SearchArticleID
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class SearchArticleID : ITagImportObject
{
  public int ArticleID { get; private set; }

  public int VersionNo { get; private set; }

  public SearchArticleID()
  {
  }

  public SearchArticleID(int articleID, int versionNo)
  {
    this.ArticleID = articleID;
    this.VersionNo = versionNo;
  }

  public short ClassID => 30;

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        binaryWriter.Write(this.ArticleID);
        binaryWriter.Write(this.VersionNo);
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
        this.ArticleID = binaryReader.ReadInt32();
        this.VersionNo = binaryReader.ReadInt32();
      }
      finally
      {
        binaryReader.Close();
      }
    }
  }
}

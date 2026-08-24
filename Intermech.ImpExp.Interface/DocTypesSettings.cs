// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DocTypesSettings
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Храним значения для типа документов для последующего сохранения на диск
/// и чтения оттуда при Pump (если вдруг произойдет остановка закачки м/у Еxam() и Pump())
/// </summary>
public class DocTypesSettings : ITagImportObject
{
  public string Guid = string.Empty;
  public string LinkedExt = string.Empty;
  public string DocExt = string.Empty;
  public string DocCode = string.Empty;
  public string DocName = string.Empty;
  public string ProtoName = string.Empty;
  public string Classif = string.Empty;
  public int DrawStamp;
  public int Suffix;
  public byte[] FileBody;

  public DocTypesSettings()
  {
  }

  public DocTypesSettings(
    string guid,
    string linkedExt,
    string docExt,
    string docCode,
    string docName,
    string protoName,
    string classif,
    int drawStamp,
    int suffix,
    byte[] fileBody)
  {
    this.Guid = guid;
    this.LinkedExt = linkedExt;
    this.DocExt = docExt;
    this.DocCode = docCode;
    this.DocName = docName;
    this.ProtoName = protoName;
    this.Classif = classif;
    this.DrawStamp = drawStamp;
    this.Suffix = suffix;
    this.FileBody = fileBody;
  }

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        binaryWriter.Write(this.Guid.Length);
        if (this.Guid.Length > 0)
          binaryWriter.Write(this.Guid.ToCharArray());
        binaryWriter.Write(this.LinkedExt.Length);
        if (this.LinkedExt.Length > 0)
          binaryWriter.Write(this.LinkedExt.ToCharArray());
        binaryWriter.Write(this.DocExt.Length);
        if (this.DocExt.Length > 0)
          binaryWriter.Write(this.DocExt.ToCharArray());
        binaryWriter.Write(this.DocCode.Length);
        if (this.DocCode.Length > 0)
          binaryWriter.Write(this.DocCode.ToCharArray());
        binaryWriter.Write(this.DocName.Length);
        if (this.DocName.Length > 0)
          binaryWriter.Write(this.DocName.ToCharArray());
        binaryWriter.Write(this.ProtoName.Length);
        if (this.ProtoName.Length > 0)
          binaryWriter.Write(this.ProtoName.ToCharArray());
        binaryWriter.Write(this.Classif.Length);
        if (this.Classif.Length > 0)
          binaryWriter.Write(this.Classif.ToCharArray());
        binaryWriter.Write(this.DrawStamp);
        binaryWriter.Write(this.Suffix);
        if (this.FileBody != null && this.FileBody.Length != 0)
        {
          binaryWriter.Write(this.FileBody.Length);
          binaryWriter.Write(this.FileBody);
        }
        else
          binaryWriter.Write(0);
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
      BinaryReader br = new BinaryReader((Stream) input, Encoding.UTF8);
      try
      {
        int length1 = br.ReadInt32();
        if (length1 > 0)
          this.Guid = TagImportObjectHelper.GetString(length1, br);
        int length2 = br.ReadInt32();
        if (length2 > 0)
          this.LinkedExt = TagImportObjectHelper.GetString(length2, br);
        int length3 = br.ReadInt32();
        if (length3 > 0)
          this.DocExt = TagImportObjectHelper.GetString(length3, br);
        int length4 = br.ReadInt32();
        if (length4 > 0)
          this.DocCode = TagImportObjectHelper.GetString(length4, br);
        int length5 = br.ReadInt32();
        if (length5 > 0)
          this.DocName = TagImportObjectHelper.GetString(length5, br);
        int length6 = br.ReadInt32();
        if (length6 > 0)
          this.ProtoName = TagImportObjectHelper.GetString(length6, br);
        int length7 = br.ReadInt32();
        if (length7 > 0)
          this.Classif = TagImportObjectHelper.GetString(length7, br);
        this.DrawStamp = br.ReadInt32();
        this.Suffix = br.ReadInt32();
        int count = br.ReadInt32();
        if (count <= 0)
          return;
        this.FileBody = br.ReadBytes(count);
      }
      finally
      {
        br.Close();
      }
    }
  }

  public short ClassID => 1;
}

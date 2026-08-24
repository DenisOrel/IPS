// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.PUBKEYBLOBHEADERS
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

#nullable disable
namespace Intermech.Office.Client;

public struct PUBKEYBLOBHEADERS
{
  public byte bType;
  public byte bVersion;
  public short reserved;
  public uint aiKeyAlg;
  public uint magic;
  public uint bitlen;
  public uint pubexp;
}

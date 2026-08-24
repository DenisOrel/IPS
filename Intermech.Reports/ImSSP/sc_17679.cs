// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_17679
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Protection;
using System;

#nullable disable
namespace ImSSP;

internal static class sc_17679
{
  internal static int ssp_imclient_17680(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/]
    {
      (byte) 206,
      (byte) 225,
      (byte) 101,
      (byte) 79,
      (byte) 20,
      (byte) 99,
      (byte) 198,
      (byte) 115,
      (byte) 204,
      (byte) 130,
      (byte) 81,
      (byte) 13,
      (byte) 128 /*0x80*/,
      (byte) 22,
      (byte) 76,
      (byte) 140,
      (byte) 202,
      (byte) 104,
      (byte) 118,
      (byte) 66,
      (byte) 169,
      (byte) 84,
      (byte) 191,
      (byte) 245,
      (byte) 123,
      (byte) 94,
      (byte) 29,
      (byte) 114,
      (byte) 153,
      (byte) 159,
      (byte) 123,
      (byte) 166,
      (byte) 116,
      (byte) 15,
      (byte) 96 /*0x60*/,
      (byte) 89,
      (byte) 27,
      (byte) 27,
      (byte) 242,
      (byte) 101,
      (byte) 241,
      (byte) 90,
      (byte) 169,
      (byte) 158,
      (byte) 18,
      (byte) 22,
      (byte) 245,
      (byte) 62
    };
    byte[] sourceArray2 = new byte[48 /*0x30*/];
    sourceArray2[23] = (byte) 21;
    sourceArray2[31 /*0x1F*/] = (byte) 252;
    sourceArray2[2] = (byte) 253;
    sourceArray2[3] = (byte) 55;
    sourceArray2[43] = (byte) 68;
    sourceArray2[14] = (byte) 209;
    sourceArray2[6] = (byte) 181;
    sourceArray2[15] = (byte) 130;
    sourceArray2[4] = (byte) 144 /*0x90*/;
    sourceArray2[22] = (byte) 70;
    sourceArray2[10] = (byte) 152;
    sourceArray2[11] = (byte) 73;
    sourceArray2[19] = (byte) 197;
    sourceArray2[40] = (byte) 15;
    sourceArray2[42] = (byte) 75;
    sourceArray2[44] = (byte) 238;
    sourceArray2[16 /*0x10*/] = (byte) 78;
    sourceArray2[17] = (byte) 220;
    sourceArray2[18] = (byte) 5;
    sourceArray2[41] = (byte) 190;
    sourceArray2[20] = (byte) 40;
    sourceArray2[47] = (byte) 74;
    sourceArray2[21] = (byte) 118;
    sourceArray2[26] = (byte) 112 /*0x70*/;
    sourceArray2[24] = (byte) 7;
    sourceArray2[25] = (byte) 27;
    sourceArray2[35] = (byte) 55;
    sourceArray2[13] = (byte) 124;
    sourceArray2[28] = (byte) 11;
    sourceArray2[29] = (byte) 141;
    sourceArray2[12] = (byte) 222;
    sourceArray2[5] = (byte) 58;
    sourceArray2[32 /*0x20*/] = byte.MaxValue;
    sourceArray2[7] = (byte) 101;
    sourceArray2[34] = (byte) 183;
    sourceArray2[37] = (byte) 101;
    sourceArray2[30] = (byte) 3;
    sourceArray2[36] = (byte) 100;
    sourceArray2[38] = (byte) 134;
    sourceArray2[27] = (byte) 240 /*0xF0*/;
    sourceArray2[46] = (byte) 76;
    sourceArray2[39] = (byte) 43;
    sourceArray2[8] = (byte) 218;
    sourceArray2[9] = (byte) 210;
    sourceArray2[33] = (byte) 102;
    sourceArray2[45] = (byte) 235;
    sourceArray2[1] = (byte) 118;
    sourceArray2[0] = (byte) 18;
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 348, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }
}

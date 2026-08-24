// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_17667
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Protection;
using System;

#nullable disable
namespace ImSSP;

internal static class sc_17667
{
  internal static int ssp_imclient_17668(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/];
    sourceArray1[45] = (byte) 93;
    sourceArray1[17] = (byte) 154;
    sourceArray1[9] = (byte) 91;
    sourceArray1[0] = (byte) 140;
    sourceArray1[3] = (byte) 114;
    sourceArray1[21] = (byte) 183;
    sourceArray1[2] = (byte) 55;
    sourceArray1[7] = (byte) 115;
    sourceArray1[12] = (byte) 205;
    sourceArray1[31 /*0x1F*/] = (byte) 213;
    sourceArray1[10] = (byte) 39;
    sourceArray1[33] = (byte) 34;
    sourceArray1[38] = (byte) 69;
    sourceArray1[13] = (byte) 151;
    sourceArray1[14] = (byte) 80 /*0x50*/;
    sourceArray1[1] = (byte) 176 /*0xB0*/;
    sourceArray1[16 /*0x10*/] = (byte) 141;
    sourceArray1[15] = (byte) 49;
    sourceArray1[20] = (byte) 226;
    sourceArray1[39] = (byte) 248;
    sourceArray1[35] = (byte) 102;
    sourceArray1[24] = (byte) 111;
    sourceArray1[22] = (byte) 138;
    sourceArray1[23] = (byte) 119;
    sourceArray1[11] = (byte) 124;
    sourceArray1[25] = (byte) 171;
    sourceArray1[26] = (byte) 114;
    sourceArray1[27] = (byte) 94;
    sourceArray1[40] = (byte) 128 /*0x80*/;
    sourceArray1[29] = (byte) 47;
    sourceArray1[43] = (byte) 86;
    sourceArray1[36] = (byte) 102;
    sourceArray1[32 /*0x20*/] = (byte) 42;
    sourceArray1[8] = (byte) 44;
    sourceArray1[46] = (byte) 90;
    sourceArray1[19] = (byte) 129;
    sourceArray1[5] = (byte) 43;
    sourceArray1[37] = (byte) 43;
    sourceArray1[4] = (byte) 41;
    sourceArray1[41] = (byte) 64 /*0x40*/;
    sourceArray1[30] = (byte) 130;
    sourceArray1[34] = (byte) 13;
    sourceArray1[42] = (byte) 234;
    sourceArray1[6] = (byte) 208 /*0xD0*/;
    sourceArray1[44] = (byte) 192 /*0xC0*/;
    sourceArray1[18] = (byte) 0;
    sourceArray1[28] = (byte) 106;
    sourceArray1[47] = (byte) 120;
    byte[] sourceArray2 = new byte[48 /*0x30*/];
    sourceArray2[25] = (byte) 33;
    sourceArray2[1] = (byte) 222;
    sourceArray2[43] = (byte) 44;
    sourceArray2[9] = (byte) 249;
    sourceArray2[22] = byte.MaxValue;
    sourceArray2[5] = (byte) 21;
    sourceArray2[6] = (byte) 197;
    sourceArray2[16 /*0x10*/] = (byte) 86;
    sourceArray2[15] = (byte) 61;
    sourceArray2[33] = (byte) 229;
    sourceArray2[10] = (byte) 6;
    sourceArray2[0] = (byte) 19;
    sourceArray2[45] = (byte) 6;
    sourceArray2[19] = (byte) 150;
    sourceArray2[40] = (byte) 166;
    sourceArray2[44] = (byte) 107;
    sourceArray2[12] = (byte) 54;
    sourceArray2[31 /*0x1F*/] = (byte) 192 /*0xC0*/;
    sourceArray2[7] = (byte) 243;
    sourceArray2[41] = (byte) 220;
    sourceArray2[47] = (byte) 34;
    sourceArray2[21] = (byte) 124;
    sourceArray2[37] = (byte) 247;
    sourceArray2[17] = (byte) 240 /*0xF0*/;
    sourceArray2[24] = (byte) 251;
    sourceArray2[14] = (byte) 55;
    sourceArray2[2] = (byte) 191;
    sourceArray2[27] = (byte) 232;
    sourceArray2[28] = (byte) 10;
    sourceArray2[29] = (byte) 246;
    sourceArray2[30] = (byte) 155;
    sourceArray2[23] = (byte) 79;
    sourceArray2[32 /*0x20*/] = (byte) 52;
    sourceArray2[11] = (byte) 82;
    sourceArray2[34] = (byte) 203;
    sourceArray2[35] = (byte) 237;
    sourceArray2[36] = (byte) 69;
    sourceArray2[42] = (byte) 163;
    sourceArray2[38] = (byte) 102;
    sourceArray2[39] = (byte) 200;
    sourceArray2[26] = (byte) 108;
    sourceArray2[18] = (byte) 144 /*0x90*/;
    sourceArray2[13] = (byte) 134;
    sourceArray2[8] = (byte) 103;
    sourceArray2[4] = (byte) 105;
    sourceArray2[20] = (byte) 187;
    sourceArray2[46] = (byte) 41;
    sourceArray2[3] = (byte) 176 /*0xB0*/;
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 348, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }
}

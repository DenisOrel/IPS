// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.SmdoBookProc
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System;

#nullable disable
namespace Intermech.Office.Client;

public class SmdoBookProc
{
  internal static Type ConvertSmdoFieldType(string smdoType)
  {
    if (smdoType.Equals("STRING", StringComparison.InvariantCultureIgnoreCase))
      return typeof (string);
    if (smdoType.Equals("NUMBER", StringComparison.InvariantCultureIgnoreCase))
      return typeof (long);
    return smdoType.Equals("REFERENCE", StringComparison.InvariantCultureIgnoreCase) ? typeof (Guid) : typeof (string);
  }
}

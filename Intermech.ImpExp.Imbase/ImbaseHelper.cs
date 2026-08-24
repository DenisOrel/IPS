// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ImbaseHelper
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System;

#nullable disable
namespace Intermech.ImpExp.Imbase;

internal static class ImbaseHelper
{
  public static int StringCalcFieldSize = 128 /*0x80*/;

  public static int ToInt32(object obj) => DBNull.Value.Equals(obj) ? 0 : Convert.ToInt32(obj);
}

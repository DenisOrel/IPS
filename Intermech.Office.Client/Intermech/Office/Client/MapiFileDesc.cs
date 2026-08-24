// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.MapiFileDesc
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.Office.Client;

[StructLayout(LayoutKind.Sequential)]
public class MapiFileDesc
{
  public int reserved;
  public int flags;
  public int position;
  public string path;
  public string name;
  public IntPtr type;
}

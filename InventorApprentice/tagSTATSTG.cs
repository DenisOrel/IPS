// Decompiled with JetBrains decompiler
// Type: InventorApprentice.tagSTATSTG
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct tagSTATSTG
{
  [MarshalAs(UnmanagedType.LPWStr)]
  public string pwcsName;
  public uint Type;
  public _ULARGE_INTEGER cbSize;
  public _FILETIME mtime;
  public _FILETIME ctime;
  public _FILETIME atime;
  public uint grfMode;
  public uint grfLocksSupported;
  public Guid clsid;
  public uint grfStateBits;
  public uint Reserved;
}

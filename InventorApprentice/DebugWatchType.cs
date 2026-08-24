// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DebugWatchType
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("F6F33555-6984-11D5-8DF3-0010B541CAA8")]
[TypeLibType(16 /*0x10*/)]
public enum DebugWatchType
{
  kNoneWatchType = 0,
  kAddRefWatchType = 1,
  kReleaseWatchType = 2,
  kQueryInterfaceWatchType = 4,
}

// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DebugInstrumentationSink_ObjectAddRefdEventHandler
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ComVisible(false)]
[TypeLibType(16 /*0x10*/)]
public delegate void DebugInstrumentationSink_ObjectAddRefdEventHandler(
  [In] int Cookie,
  [In] int ToReferenceCount);

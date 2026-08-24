// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ReferenceKeyEventsSink_OnBindKeyToObjectEventHandler
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[ComVisible(false)]
public delegate void ReferenceKeyEventsSink_OnBindKeyToObjectEventHandler(
  [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1), In, Out] ref byte[] ReferenceKey,
  [MarshalAs(UnmanagedType.IDispatch), In] object Document,
  [MarshalAs(UnmanagedType.IDispatch), In, Out] ref object Object,
  out SolutionNatureEnum MatchType,
  [MarshalAs(UnmanagedType.Interface), In] NameValueMap Context,
  [In, Out] ref HandlingCodeEnum HandlingCode);

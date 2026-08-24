// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IPersistFile
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("0000010B-0000-0000-C000-000000000046")]
[InterfaceType(1)]
[ComImport]
public interface IPersistFile : IPersist
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void GetClassID(out Guid pClassID);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void IsDirty();

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Load([MarshalAs(UnmanagedType.LPWStr), In] string pszFileName, [In] uint dwMode);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Save([MarshalAs(UnmanagedType.LPWStr), In] string pszFileName, [In] int fRemember);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SaveCompleted([MarshalAs(UnmanagedType.LPWStr), In] string pszFileName);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
}

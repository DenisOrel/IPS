// Decompiled with JetBrains decompiler
// Type: InventorApprentice.MoldSplitResult
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4112)]
[InterfaceType(2)]
[Guid("CB5F8603-7F21-4B44-A5C1-CD471AB5EA08")]
[ComImport]
public interface MoldSplitResult
{
  [DispId(50427137)]
  int Status { [DispId(50427137), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50427139)]
  int NoteCount { [DispId(50427139), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(50427140)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void _GetNote([In] int NoteIndex, out int NoteType, [MarshalAs(UnmanagedType.Interface)] out ObjectsEnumerator Entities);

  [DispId(50427141)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetNote(
    [In] int NoteIndex,
    out int NoteType,
    [MarshalAs(UnmanagedType.Interface)] out ObjectsEnumerator Entities,
    out double Value,
    [MarshalAs(UnmanagedType.BStr)] out string ErrorMessage);
}

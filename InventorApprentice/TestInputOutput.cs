// Decompiled with JetBrains decompiler
// Type: InventorApprentice.TestInputOutput
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4112)]
[Guid("2190DB7B-9EAB-45D5-B9E0-B7FE50E1F50B")]
[InterfaceType(2)]
[ComImport]
public interface TestInputOutput
{
  [DispId(50381313)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetString([MarshalAs(UnmanagedType.BStr), In] string strPrompt);

  [DispId(50381314)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  double GetValue([MarshalAs(UnmanagedType.BStr), In] string strPrompt);

  [DispId(50381315)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int GetInteger([MarshalAs(UnmanagedType.BStr), In] string strPrompt);

  [DispId(50381316)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool GetChoice([MarshalAs(UnmanagedType.BStr), In] string strPrompt);

  [DispId(50381317)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetDocument([MarshalAs(UnmanagedType.BStr), In] string strPrompt);

  [DispId(50381318)]
  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ValidateString([MarshalAs(UnmanagedType.BStr), In] string strName, [MarshalAs(UnmanagedType.BStr), In] string strValue);

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(50381319)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ValidateValue([MarshalAs(UnmanagedType.BStr), In] string strName, [In] double dValue, [MarshalAs(UnmanagedType.BStr), In] string strTolType = "");

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(50381320)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ValidateNumber([MarshalAs(UnmanagedType.BStr), In] string strName, [In] int nValue);

  [DispId(50381333)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool Verify([MarshalAs(UnmanagedType.BStr), In] string strName, [MarshalAs(UnmanagedType.Struct), In] object varExpected, [MarshalAs(UnmanagedType.Struct), In] object varActual, [In] double dTolerancePercentage = 0.0);

  [DispId(50381322)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Trace([MarshalAs(UnmanagedType.BStr), In] string strTraceText);

  [DispId(50381323)]
  string ValidationText { [DispId(50381323), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50381324)]
  string TraceText { [DispId(50381324), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50381325)]
  string InputText { [DispId(50381325), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50381326)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Error([MarshalAs(UnmanagedType.BStr), In] string strError);

  [DispId(50381327)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void AddLabel([MarshalAs(UnmanagedType.BStr), In] string strLabelName);

  [DispId(50381328)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void BeginContext([MarshalAs(UnmanagedType.BStr), In] string strContext);

  [DispId(50381329)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void EndContext();

  [DispId(50381330)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void NotImplemented([MarshalAs(UnmanagedType.BStr), In] string strMessage);

  [DispId(50381331)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Defect([MarshalAs(UnmanagedType.BStr), In] string strDefectNumMsg);

  [DispId(50381332)]
  string TestContext { [DispId(50381332), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(50381332), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(50381338)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ValidateRawXml([MarshalAs(UnmanagedType.BStr), In] string strName, [MarshalAs(UnmanagedType.BStr), In] string strValue);

  [DispId(50381339)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool AssignBadBoolValue([In] short bVal);
}

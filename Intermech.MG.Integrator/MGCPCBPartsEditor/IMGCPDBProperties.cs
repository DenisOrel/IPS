// Decompiled with JetBrains decompiler
// Type: MGCPCBPartsEditor.IMGCPDBProperties
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.CustomMarshalers;

#nullable disable
namespace MGCPCBPartsEditor;

[CompilerGenerated]
[Guid("AD815B20-512F-4373-888F-43C2103733A9")]
[DefaultMember("Item")]
[TypeIdentifier]
[ComImport]
public interface IMGCPDBProperties : _IMGCPDBBaseCollection
{
  [DispId(-4)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof (EnumeratorToEnumVariantMarshaler))]
  new IEnumerator GetEnumerator();
}

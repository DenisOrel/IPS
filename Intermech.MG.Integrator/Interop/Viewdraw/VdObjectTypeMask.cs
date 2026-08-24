// Decompiled with JetBrains decompiler
// Type: Interop.Viewdraw.VdObjectTypeMask
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.Viewdraw;

[CompilerGenerated]
[TypeIdentifier("4ADEF4E1-690A-11CE-9261-0020C5E26659", "Interop.Viewdraw.VdObjectTypeMask")]
public enum VdObjectTypeMask
{
  VDM_LINE = 1,
  VDM_BOX = 2,
  VDM_TEXT = 4,
  VDM_CIRCLE = 8,
  VDM_ARC = 16, // 0x00000010
  VDM_NET = 32, // 0x00000020
  VDM_ATTR = 64, // 0x00000040
  VDM_COMP = 128, // 0x00000080
  VDM_LABEL = 256, // 0x00000100
  VDM_PIN = 512, // 0x00000200
  VDM_OAT = 1024, // 0x00000400
  VDM_BLOCK = 2048, // 0x00000800
  VDM_COMPPIN = 4096, // 0x00001000
  VDM_SEGMENT = 8192, // 0x00002000
  VDM_ALL = 16384, // 0x00004000
}

// Decompiled with JetBrains decompiler
// Type: InventorApprentice.GraphicsDriverTypeEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("593D8CBC-6F54-4BEA-A1A3-23AD14842E26")]
public enum GraphicsDriverTypeEnum
{
  kDirect3DGraphicsDriver = 61441, // 0x0000F001
  kOpenGLGraphicsDriver = 61442, // 0x0000F002
  kConservativeOpenGLGraphicsDriver = 61443, // 0x0000F003
  kSoftwareGraphics = 61444, // 0x0000F004
  kDirect3D10GraphicsDriver = 61445, // 0x0000F005
  kDirect3D11GraphicsDriver = 61446, // 0x0000F006
}

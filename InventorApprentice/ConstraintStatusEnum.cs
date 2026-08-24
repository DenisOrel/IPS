// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ConstraintStatusEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("DDA40E53-3CC9-4E82-BBCF-76FBB57BEA5C")]
public enum ConstraintStatusEnum
{
  kFullyConstrainedConstraintStatus = 51713, // 0x0000CA01
  kUnderConstrainedConstraintStatus = 51714, // 0x0000CA02
  kOverConstrainedConstraintStatus = 51715, // 0x0000CA03
  kUnknownConstraintStatus = 51716, // 0x0000CA04
}

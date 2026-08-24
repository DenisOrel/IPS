// Decompiled with JetBrains decompiler
// Type: InventorApprentice.CommandTypesEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("AFDC9E6A-6FEA-448F-87AF-B948745D9601")]
public enum CommandTypesEnum
{
  kShapeEditCmdType = 1,
  kQueryOnlyCmdType = 2,
  kFileOperationsCmdType = 4,
  kFilePropertyEditCmdType = 8,
  kUpdateWithReferencesCmdType = 16, // 0x00000010
  kNonShapeEditCmdType = 32, // 0x00000020
  kEditMaskCmdType = 57, // 0x00000039
  kReferencesChangeCmdType = 64, // 0x00000040
  kSchemaChangeCmdType = 128, // 0x00000080
}

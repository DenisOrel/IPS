// Decompiled with JetBrains decompiler
// Type: MGCPCB.EPcbSelectionType
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace MGCPCB;

[CompilerGenerated]
[Guid("7DABAAE3-9407-11D2-89D1-0020184077E1")]
[TypeIdentifier("EDEDED00-D5F6-4B04-8FE7-EDEDEDEDED00", "MGCPCB.EPcbSelectionType")]
public enum EPcbSelectionType
{
  epcbSelectAll = 0,
  epcbSelectSelected = 1,
  epcbSelectUnselected = 2,
  epcbSelectPlaced = 4,
  epcbSelectUnplaced = 8,
  epcbSelectFixed = 128, // 0x00000080
  epcbSelectUnfixed = 256, // 0x00000100
  epcbSelectLocked = 512, // 0x00000200
  epcbSelectUnlocked = 1024, // 0x00000400
  epcbSelectJustModified = 2048, // 0x00000800
  epcbSelectMarked = 4096, // 0x00001000
  epcbSelectHighlighted = 8192, // 0x00002000
  epcbSelectUnhighlighted = 16384, // 0x00004000
}

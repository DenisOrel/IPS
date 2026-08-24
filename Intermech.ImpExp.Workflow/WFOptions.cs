// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.WFOptions
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using System;

#nullable disable
namespace Intermech.ImpExp.Workflow;

[Flags]
internal enum WFOptions
{
  PumpTerminated = 1,
  PumpBig = 2,
  PumpOldExecuted = 4,
  PumpByDateTime = 8,
  PumpCompleted = 16, // 0x00000010
  PumpSchemes = 32, // 0x00000020
  PumpProcesses = 64, // 0x00000040
}

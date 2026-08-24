// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ExecutionOrderInfo
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.Office.Client;

internal static class ExecutionOrderInfo
{
  [NotNull]
  public static readonly FormDesignerAction ExecutionOrderEditorAction = new FormDesignerAction(new Guid("42C8CA11-EB1F-44BE-A651-2CC676E2C703"), Localization.GetString("Office.Client_79"));
}

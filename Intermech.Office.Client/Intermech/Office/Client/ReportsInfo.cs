// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ReportsInfo
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.Office.Client;

public static class ReportsInfo
{
  [NotNull]
  public static readonly FormDesignerAction ReportsExecute = new FormDesignerAction(new Guid("72771386-49D8-432d-B1F4-942D41AF740F"), Localization.GetString("Office.Client_13"));
}

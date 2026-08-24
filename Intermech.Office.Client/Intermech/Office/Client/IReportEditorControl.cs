// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.IReportEditorControl
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;
using System;

#nullable disable
namespace Intermech.Office.Client;

internal interface IReportEditorControl
{
  event EventHandler Changed;

  bool OnSaveData([NotNull] IUserSession session);

  void OnLoadData([NotNull] IUserSession session, IDBResolution resolution);
}

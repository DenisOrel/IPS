// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.AddresseeInfo
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.Office.Client;

public static class AddresseeInfo
{
  public static readonly Guid AddresseeActionGuid = new Guid("39B66524-06E4-41b4-B2B3-6E7C158B20C3");
  [NotNull]
  public static readonly FormDesignerAction AddresseeEditorExecute = new FormDesignerAction(AddresseeInfo.AddresseeActionGuid, Localization.GetString("Office.Client_4"));
}

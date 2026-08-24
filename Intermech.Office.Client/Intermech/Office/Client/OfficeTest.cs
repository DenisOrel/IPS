// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeTest
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Client.Core;
using Intermech.Diagnostics;
using System;

#nullable disable
namespace Intermech.Office.Client;

internal static class OfficeTest
{
  internal static void TestEventHandle([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    OfficeTest.Test();
  }

  internal static void Test()
  {
  }

  internal static void TestEventHandle2([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    OfficeTest.Test2();
  }

  internal static void Test2()
  {
    SelectDialog.LockResoterObjectType();
    try
    {
      OfficeTest.Test();
    }
    finally
    {
      SelectDialog.UnlockResoterObjectType();
    }
  }
}

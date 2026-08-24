// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.Editors.ConfidentialReport
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.Office.Client.Editors;

internal class ConfidentialReport(long unitID) : Report(unitID)
{
  private const string PassStr = "6E1F4B4F-C8A7-4f06-82E5-B529A944AEDB";

  public override string Load([NotNull] IUserSession session, int index)
  {
    string s = this.GetValue(session, index);
    return s == string.Empty ? s : Cryptor.Decrypt(Convert.FromBase64String(s), "6E1F4B4F-C8A7-4f06-82E5-B529A944AEDB");
  }

  public override void Save(IDBObject dbResolution, int index, string text)
  {
    base.Save(dbResolution, index, Cryptor.Encrypt(text, "6E1F4B4F-C8A7-4f06-82E5-B529A944AEDB"));
  }
}

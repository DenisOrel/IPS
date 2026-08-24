// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OnSendClickEventArgs
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Office.Client;

public class OnSendClickEventArgs
{
  [NotNull]
  public string ToEmail { get; }

  [NotNull]
  public string Subject { get; }

  [NotNull]
  public string Message { get; }

  [NotNull]
  public int[] Indexes { get; }

  public Guid AccountGuid { get; }

  public OnSendClickEventArgs(
    [NotNull] string toEmail,
    [NotNull] string subject,
    [NotNull] string message,
    [CanBeNull] IEnumerable<int> indexes,
    Guid accountGuid)
  {
    this.ToEmail = toEmail;
    this.Subject = subject;
    this.Message = message;
    this.Indexes = (indexes != null ? indexes.AsArray<int>() : (int[]) null) ?? Array.Empty<int>();
    this.AccountGuid = accountGuid;
  }
}

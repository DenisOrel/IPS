// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.AppManagerExtension
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

public static class AppManagerExtension
{
  private static int MaxHistorySize = 250;
  private static readonly Lazy<MessageBuffer> LazyInfoBuffer = new Lazy<MessageBuffer>((Func<MessageBuffer>) (() => new MessageBuffer(AppManagerExtension.MaxHistorySize)));
  private static readonly Lazy<MessageBuffer> LazyWarningBuffer = new Lazy<MessageBuffer>((Func<MessageBuffer>) (() => new MessageBuffer(AppManagerExtension.MaxHistorySize)));
  private static readonly Lazy<MessageBuffer> LazyErrorBuffer = new Lazy<MessageBuffer>((Func<MessageBuffer>) (() => new MessageBuffer(AppManagerExtension.MaxHistorySize)));

  public static void AddNewWarningMessage(this IAppManager manager, string message)
  {
    if (AppManagerExtension.LazyWarningBuffer.Value.Contains(message))
      return;
    AppManagerExtension.LazyWarningBuffer.Value.Add(message);
    manager.AddWarningMessage(message);
  }

  public static void AddNewInfoMessage(this IAppManager manager, string message)
  {
    if (AppManagerExtension.LazyInfoBuffer.Value.Contains(message))
      return;
    AppManagerExtension.LazyInfoBuffer.Value.Add(message);
    manager.AddInfoMessage(message);
  }

  public static void AddNewErrorMessage(this IAppManager manager, string message)
  {
    if (AppManagerExtension.LazyErrorBuffer.Value.Contains(message))
      return;
    AppManagerExtension.LazyErrorBuffer.Value.Add(message);
    manager.AddErrorMessage(message);
  }
}

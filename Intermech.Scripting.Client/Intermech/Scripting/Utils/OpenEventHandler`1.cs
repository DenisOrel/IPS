// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Utils.OpenEventHandler`1
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;

#nullable disable
namespace Intermech.Scripting.Utils;

internal delegate void OpenEventHandler<T>(object target, object sender, T e) where T : EventArgs;

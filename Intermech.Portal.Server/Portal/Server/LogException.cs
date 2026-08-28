// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.LogException
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using System;
using System.Text;

#nullable disable
namespace Intermech.Portal.Server;

internal class LogException
{
  public static string Create(Exception ex)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendLine(ex.Message);
    stringBuilder.AppendLine(ex.StackTrace);
    return stringBuilder.ToString();
  }
}

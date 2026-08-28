// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.BackupTaskUnitFiles
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class BackupTaskUnitFiles
{
  public static List<FileInfo> FindFiles(string taskFolder)
  {
    if (TraceLog.Enabled)
      TraceLog.Write("...FindFiles taskFolder=" + taskFolder);
    string[] files1 = Directory.GetFiles(taskFolder, ActionsHelper.TransferedUnitFileName, SearchOption.AllDirectories);
    List<FileInfo> files2 = new List<FileInfo>(((IEnumerable<string>) files1).Select<string, FileInfo>((Func<string, FileInfo>) (x => new FileInfo(x))));
    files2.Sort((Comparison<FileInfo>) ((x, y) => x.CreationTime.CompareTo(y.CreationTime)));
    if (!TraceLog.Enabled)
      return files2;
    TraceLog.Write($"...End FindFiles count {files1.Length}");
    return files2;
  }
}

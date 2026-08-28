// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.DBObjectCollectionExtensions
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Kernel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Project.Server;

public static class DBObjectCollectionExtensions
{
  [NotNull]
  [DebuggerHidden]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBProjectTaskCollection AsDBTasksCollection(
    [NotNull] this DBObjectCollection dbObjectCollection)
  {
    return dbObjectCollection.CastClassToClass<DBProjectTaskCollection>();
  }

  [NotNull]
  [DebuggerHidden]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBProjectCollection AsDBProjectsCollection(
    [NotNull] this DBObjectCollection dbObjectCollection)
  {
    return dbObjectCollection.CastClassToClass<DBProjectCollection>();
  }
}

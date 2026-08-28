// Decompiled with JetBrains decompiler
// Type: Intermech.Kernel.DBObjectExtensions
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Project.Server;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Kernel;

public static class DBObjectExtensions
{
  [NotNull]
  [DebuggerHidden]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBProjectTask AsDBTask([NotNull] this DBObject dbObject)
  {
    return dbObject.CastClassToClass<DBProjectTask>();
  }

  [NotNull]
  [DebuggerHidden]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBProject AsDBProject([NotNull] this DBObject dbObject)
  {
    return dbObject.CastClassToClass<DBProject>();
  }
}

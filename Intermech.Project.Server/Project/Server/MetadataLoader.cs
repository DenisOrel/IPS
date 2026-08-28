// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.MetadataLoader
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;

#nullable disable
namespace Intermech.Project.Server;

public abstract class MetadataLoader : Intermech.Project.MetadataLoader
{
  protected internal new static void Init([NotNull] IUserSession session)
  {
    Intermech.Project.MetadataLoader.Init(session);
  }
}

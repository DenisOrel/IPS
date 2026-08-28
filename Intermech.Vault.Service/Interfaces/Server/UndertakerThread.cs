// Decompiled with JetBrains decompiler
// Type: Interfaces.Server.UndertakerThread
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using Intermech.Vault.Interfaces;
using Intermech.Vault.Service;
using System;
using System.Threading;

#nullable disable
namespace Interfaces.Server;

public class UndertakerThread
{
  internal Thread Thread;
  internal TimeSpan span = CommonVariables.WAIT_SPAN;

  internal void ThreadMethod()
  {
    while (true)
    {
      Thread.Sleep(this.span);
      DiskFileStorageCollection.DisconnectByTimeOut();
    }
  }

  public UndertakerThread()
  {
    this.Thread = new Thread(new ThreadStart(this.ThreadMethod));
    this.Thread.IsBackground = true;
    this.Thread.Name = "Timeout Disconnect";
    this.Thread.Priority = ThreadPriority.Lowest;
    this.Thread.Start();
  }
}

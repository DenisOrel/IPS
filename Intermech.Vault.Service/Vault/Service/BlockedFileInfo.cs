// Decompiled with JetBrains decompiler
// Type: Intermech.Vault.Service.BlockedFileInfo
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

#nullable disable
namespace Intermech.Vault.Service;

public struct BlockedFileInfo(long id, int history)
{
  private long blobID = id;
  private int historyID = history;
}

// Decompiled with JetBrains decompiler
// Type: TransactionType
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using System.ComponentModel;

#nullable disable
public enum TransactionType
{
  [Description("AddFile")] AddFile,
  [Description("AddFileInfo")] AddFileInfo,
  [Description("UpdateFileInfo")] UpdateFileInfo,
  [Description("MoveFile")] MoveFile,
  [Description("DeleteFile")] DeleteFile,
  [Description("DeleteStorage")] DeleteStorage,
  [Description("PurgeFile")] PurgeFile,
}

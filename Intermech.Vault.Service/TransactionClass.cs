// Decompiled with JetBrains decompiler
// Type: TransactionClass
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using Intermech.Vault.Interfaces;

#nullable disable
public class TransactionClass
{
  public TransactionType ActionType;
  public string FileNameAfterCommit;
  public string FileNameBeforeCommit;
  public FileOperationType OperationType;
  public FileInformation fileInfo;

  public TransactionClass(
    string afterCommit,
    string beforeCommit,
    TransactionType action,
    FileInformation fInfo)
  {
    this.FileNameAfterCommit = afterCommit;
    this.FileNameBeforeCommit = beforeCommit;
    this.ActionType = action;
    this.fileInfo = fInfo;
  }

  public TransactionClass(
    string afterCommit,
    string beforeCommit,
    TransactionType action,
    FileOperationType operation)
  {
    this.FileNameAfterCommit = afterCommit;
    this.FileNameBeforeCommit = beforeCommit;
    this.ActionType = action;
    this.OperationType = operation;
  }

  public TransactionClass(TransactionType action)
    : this(action, string.Empty)
  {
  }

  public TransactionClass(TransactionType action, FileInformation info)
    : this(string.Empty, string.Empty, action, FileOperationType.MoveFile)
  {
    this.fileInfo = info;
  }

  public TransactionClass(TransactionType action, string beforeCommit)
    : this(string.Empty, beforeCommit, action, FileOperationType.MoveFile)
  {
  }
}

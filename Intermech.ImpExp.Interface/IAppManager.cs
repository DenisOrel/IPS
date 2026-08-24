// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IAppManager
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.DataWriter;
using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

public interface IAppManager
{
  /// <summary>Добавление сообщения</summary>
  /// <param name="Message"></param>
  void AddInfoMessage(string Message);

  /// <summary>Добавление сообщения</summary>
  /// <param name="Message"></param>
  void AddErrorMessage(string Message);

  /// <summary>Добавление сообщения со стэком в лог</summary>
  /// <param name="ex"></param>
  void AddExceptionToLog(Exception ex);

  /// <summary>Добавление сообщения</summary>
  /// <param name="Message"></param>
  void AddWarningMessage(string Message);

  /// <summary>
  /// 
  /// </summary>
  IDataBaseManager DBManager { get; }

  /// <summary>
  /// 
  /// </summary>
  IDataWriter DataWriter { get; }

  /// <summary>this.Close()</summary>
  void CloseManager();

  /// <summary>Регистрация события импорта метаданных</summary>
  /// <param name="handler"></param>
  void AddEventOnSaveMetadata(EventHandler handler);
}

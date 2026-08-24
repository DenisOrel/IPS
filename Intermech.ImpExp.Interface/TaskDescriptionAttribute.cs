// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.TaskDescriptionAttribute
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Атрибут для строкового описания задачи</summary>
[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class TaskDescriptionAttribute : Attribute
{
  /// <summary>Строковое описание задачи инициализации</summary>
  public string ExamDescription;
  /// <summary>Строковое описание задачи перекачки</summary>
  public string PumpDescription;

  /// <summary>Конструктор</summary>
  /// <param name="initDescr">Описание задачи перекачки</param>
  /// <param name="pumpDescr">Описание задачи перекачки</param>
  public TaskDescriptionAttribute(string examDescr, string pumpDescr)
  {
    this.PumpDescription = pumpDescr;
    this.ExamDescription = examDescr;
  }
}

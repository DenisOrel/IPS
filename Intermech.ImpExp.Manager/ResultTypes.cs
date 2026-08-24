// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.ResultTypes
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Manager;

[Category("Misc")]
[Flags]
[TypeConverter(typeof (EnumDescConverter))]
[Description("Результат выполнения")]
internal enum ResultTypes
{
  [Description("Закачка не начиналась")] None = 0,
  [Description("Закачка была прервана")] Terminate = 1,
  [Description("Во время закачки были обнаружены ошибки")] ErrorsPresent = 2,
  [Description("Во время закачки были обнаружены предупреждения")] WarningPresent = 4,
  [Description("Завершена после закачки метаданных")] MetadataTerminate = 8,
}

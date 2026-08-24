// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.AddVersionInfo
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class AddVersionInfo
{
  public long AdvanFilesDate;
  /// <summary>Дата главного файла как она хранится в Search</summary>
  public DateTime FileDate;
  public int FileSize;
  /// <summary>
  /// Максимальная дата изменения содержимого (учитываются даты главного и дополнительных файлов), UTC
  /// </summary>
  public DateTime ContentModifiedDate;
  /// <summary>
  /// Количество блобов (атрибут Файл), которые будут добавлены к объекту в конце закачки
  /// </summary>
  public short FileCount;

  public AddVersionInfo()
  {
  }

  public AddVersionInfo(
    long AdvanFilesDate,
    DateTime FileDate,
    int FileSize,
    DateTime ContentModifiedDate,
    int FileCount)
  {
    this.AdvanFilesDate = AdvanFilesDate;
    this.FileDate = FileDate;
    this.FileSize = FileSize;
    this.ContentModifiedDate = ContentModifiedDate;
    this.FileCount = Convert.ToInt16(FileCount);
  }
}

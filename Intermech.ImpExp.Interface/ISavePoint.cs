// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ISavePoint
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Интерфейс на обработчик точки сохранения</summary>
public interface ISavePoint
{
  /// <summary>Получить текущую точку сохранения из файла</summary>
  /// <returns>Если null - точки сохранения нет</returns>
  SavePoint GetSavePoint();

  /// <summary>Установить новую точку</summary>
  /// <param name="point"></param>
  void SetSavePoint(SavePoint point);

  /// <summary>
  /// Удаляет точку сохранения - выполняется в конце закачки !!!!
  /// </summary>
  void RemoveSavePoint();

  bool IsResumeMode(SavePoint point);
}

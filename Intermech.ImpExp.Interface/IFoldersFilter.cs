// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IFoldersFilter
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Data;

#nullable disable
namespace Intermech.ImpExp.Interface;

public interface IFoldersFilter
{
  /// <summary>Делегат "дёргается" после выполнения шага процесса</summary>
  event ProgressEventHandler OnProgress;

  /// <summary>Делегат "дёргается" после ошибки выполнения</summary>
  event MessageEventHandler OnMessage;

  /// <summary>
  /// Перекачка данных фильтрации папок Imbase для Techcard из таблицы TC_LINKS
  /// </summary>
  /// <param name="dbConnection"></param>
  void PumpTCLinks(IDbConnection dbConnection);

  /// <summary>
  /// Перекачка данных фильтрации папок Imbase для Techcard из таблицы TC_USERLINKS
  /// </summary>
  /// <param name="dbConnection"></param>
  void PumpTCUserLinks(IDbConnection dbConnection);
}

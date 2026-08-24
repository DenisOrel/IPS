// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.ReportsClientCache
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;

#nullable disable
namespace Intermech.Reports;

/// <summary>
/// Класс для хранения констант и кэша
/// на клиенте
/// </summary>
public static class ReportsClientCache
{
  /// <summary>Services keeper for client</summary>
  public static class Services
  {
    /// <summary>Factory для навигатора</summary>
    public static IFactory Factory;
    /// <summary>Сервис для "фоновых" задач</summary>
    public static IBackgroundTaskView BackgroundTaskView;
  }
}

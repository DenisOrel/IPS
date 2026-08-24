// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.CategoryHelper
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using ImSSP;
using Intermech.Interfaces;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Reports;

/// <summary>
/// 
/// </summary>
internal static class CategoryHelper
{
  /// <summary>Guid категории</summary>
  private static readonly Guid ReportCategoryGuid = new Guid("10432e94-463d-45d6-818c-77172a5f9f0a");
  /// <summary>Идентификатор категории</summary>
  public static int ReportCategoryID = -1;

  /// <summary>Инициализация данных класса</summary>
  /// <param name="factory"></param>
  public static void Initialize(IFactory factory)
  {
    IGuidMapper service = ServiceUtils.GetService<IGuidMapper>((object) ApplicationServices.Container, false);
    if (service == null)
      return;
    CategoryHelper.ReportCategoryID = service.Register(CategoryHelper.ReportCategoryGuid);
  }

  /// <summary>Завершение роботы - освобождение всех ресурсов</summary>
  /// <param name="factory"></param>
  public static void Uninitialize(IFactory factory)
  {
    if (CategoryHelper.ReportCategoryID == -sc_17667.ssp_imclient_17668(467985577))
      return;
    ServiceUtils.GetService<IGuidMapper>((object) ApplicationServices.Container, false)?.Unregister(CategoryHelper.ReportCategoryID);
  }
}

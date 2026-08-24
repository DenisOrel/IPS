// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SearchSchemeSettings
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using System.ComponentModel;

#nullable disable
namespace Intermech.Pdm;

internal sealed class SearchSchemeSettings
{
  private bool _visibilityFilter;
  private bool _modified;
  private bool _inited;

  [TypeConverter(typeof (YesNoBooleanConverter))]
  [Description("Позволяет включать/выключать фильтрацию результирующего списка объектов по атрибуту \"Видимость объекта\". Включенная настройка может существенно замедлить поиск.")]
  [DisplayName("Фильтровать список объектов по атрибуту \"Видимость объекта\"")]
  public bool VisibilityFilter
  {
    get
    {
      this.CheckInited();
      return this._visibilityFilter;
    }
    set
    {
      this._visibilityFilter = value;
      this._modified = true;
    }
  }

  public bool Load()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetCustomService(typeof (ISearchSchemeSettingsService)) is ISearchSchemeSettingsService customService)
      {
        this._visibilityFilter = customService.VisibilityFilter;
        this._modified = false;
        return true;
      }
    }
    return false;
  }

  public void Save()
  {
    if (!this._modified)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (ISearchSchemeSettingsService)) is ISearchSchemeSettingsService customService))
        return;
      customService.SetVisibilityFilter(sessionKeeper.Session.SessionGUID, this._visibilityFilter);
      this._modified = false;
    }
  }

  public void Reset() => this._inited = false;

  private void CheckInited()
  {
    if (this._inited)
      return;
    this._inited = this.Load();
  }
}

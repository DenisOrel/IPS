// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignsUserProperties
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Signs.Interfaces;
using System.ComponentModel;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Общие настройки ЭЦП</summary>
internal class SignsUserProperties
{
  /// <summary>
  /// Диалог подтверждения при подписывании одиночных объектов
  /// </summary>
  private bool _ConfirmSingleSigning = true;
  internal bool _inited;

  internal void ApplyUpdates()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      (ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations).WriteBool("SIGNS", "INTERFACE", "USER_CONFIRM_SINGLE_SIGNING", this._ConfirmSingleSigning, sessionKeeper.Session.UserID);
      SignsHolder.SignsUserParametersInit(sessionKeeper.Session);
    }
  }

  internal void LoadCurrentValues()
  {
    this._ConfirmSingleSigning = (ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations).ReadBool("SIGNS", "INTERFACE", "USER_CONFIRM_SINGLE_SIGNING", true, DBConfigMode.UserOnly);
  }

  private void CheckInited()
  {
    if (this._inited)
      return;
    this.LoadCurrentValues();
    this._inited = true;
  }

  [CustomDescription("ConfirmSingleSigningDescription")]
  [CustomDisplayName("ConfirmSingleSigningCaption")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  [DefaultValue(true)]
  public bool ConfirmSingleSigning
  {
    get
    {
      this.CheckInited();
      return this._ConfirmSingleSigning;
    }
    set => this._ConfirmSingleSigning = value;
  }
}

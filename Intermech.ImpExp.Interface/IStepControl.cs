// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IStepControl
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface;

internal interface IStepControl
{
  /// <summary>Обновление состояния элемента управления</summary>
  void RefreshControl();

  /// <summary>Сохранение настроек элемента управления</summary>
  SaveSettingsResult SaveSettings();

  /// <summary>
  /// Контрол был покинут при переходе на следующий контрол
  /// при нажатии на клавижу "Далее", но это еще не сохранение результатов
  /// по SaveSettings, на контрол можем еще вернуться
  /// </summary>
  bool LeaveControl();

  void Cancel();
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.ItemsToCreate.MessageItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.ItemsToCreate;

/// <summary>Ошибка при привязке метаданного</summary>
public class MessageItem
{
  /// <summary>Тип</summary>
  public ItemErrorType ErrorType;

  /// <summary>
  /// Ключ, по которому переписываются результаты при повторной проверке
  /// </summary>
  public string Key { get; private set; }

  /// <summary>Сообщение</summary>
  public string Message { get; set; }

  public MessageItem(ItemErrorType errorType, string key, string message)
  {
    this.ErrorType = errorType;
    this.Key = key;
    this.Message = message;
  }
}

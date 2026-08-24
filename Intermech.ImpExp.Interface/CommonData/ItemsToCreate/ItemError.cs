// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.ItemsToCreate.ItemError
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.ItemsToCreate;

/// <summary>
/// Ошибка привязки по метаданному, например атрибут архива
/// </summary>
public class ItemError
{
  /// <summary>
  /// Ключ для обычных сообщений, для которых проверка производится только при Exam
  /// </summary>
  private const string _main = "+";
  /// <summary>Список сообщений об ошибках при привязке</summary>
  public List<MessageItem> ErrorMessages;

  public ItemError() => this.ErrorMessages = new List<MessageItem>();

  public ItemError(ItemErrorType errorType, string errorMessage)
    : this(errorType, "+", errorMessage)
  {
  }

  public ItemError(ItemErrorType errorType, string[] errorMessages)
  {
    this.ErrorMessages = new List<MessageItem>();
    foreach (string errorMessage in errorMessages)
      this.ErrorMessages.Add(new MessageItem(errorType, "+", errorMessage));
  }

  public ItemError(ItemErrorType errorType, string errorMessageKey, string errorMessage)
  {
    this.ErrorMessages = new List<MessageItem>()
    {
      new MessageItem(errorType, errorMessageKey, errorMessage)
    };
  }

  public void AddMessage(ItemErrorType errorType, string key, string message)
  {
    MessageItem messageItem = this.ErrorMessages.Find((Predicate<MessageItem>) (x => x.Key.Equals(key)));
    if (messageItem == null)
      this.ErrorMessages.Add(new MessageItem(errorType, key, message));
    else
      messageItem.Message = message;
  }

  public string[] ErrorMessage
  {
    get
    {
      if (this.ErrorMessages == null || this.ErrorMessages.Count == 0)
        return new string[0];
      string[] errorMessage = new string[this.ErrorMessages.Count];
      for (int index = 0; index < this.ErrorMessages.Count; ++index)
        errorMessage[index] = this.ErrorMessages[index].Message;
      return errorMessage;
    }
  }

  public ItemError Clone()
  {
    ItemError itemError = new ItemError();
    foreach (MessageItem errorMessage in this.ErrorMessages)
      itemError.AddMessage(errorMessage.ErrorType, errorMessage.Key, errorMessage.Message);
    return itemError;
  }

  public ItemErrorType HeavyErrorType
  {
    get
    {
      if (this.ErrorMessages.Count == 0)
        return ItemErrorType.None;
      if (this.ErrorMessages.Count == 1)
        return this.ErrorMessages[0].ErrorType;
      ItemErrorType heavyErrorType = ItemErrorType.None;
      foreach (MessageItem errorMessage in this.ErrorMessages)
      {
        if (heavyErrorType < errorMessage.ErrorType)
          heavyErrorType = errorMessage.ErrorType;
      }
      return heavyErrorType;
    }
  }
}

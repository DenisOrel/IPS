// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Server.SignsProviderList
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Server;

internal class SignsProviderList : IEnumerable<SignsProvider>, IEnumerable
{
  private List<SignsProvider> _list = new List<SignsProvider>();

  public void Add(SignsProvider item)
  {
    if (item.SignObjectID.Equals(-1L))
    {
      this._list.Add(item);
    }
    else
    {
      if (this._list.Contains(item))
        return;
      this._list.Add(item);
    }
  }

  public void Remove(SignsProvider item) => this._list.Remove(item);

  public void RemoveAt(int index)
  {
    if (index < 0 || index >= this._list.Count)
      return;
    this._list.RemoveAt(index);
  }

  public void Clear() => this._list.Clear();

  public SignsProvider GetItemWithGraphValue(string graphValue, bool younger)
  {
    SignsProvider itemWithGraphValue = (SignsProvider) null;
    foreach (SignsProvider signsProvider in this._list)
    {
      if (graphValue.Equals(signsProvider.GraphValue))
      {
        if (younger)
        {
          if (itemWithGraphValue != null)
          {
            if (itemWithGraphValue.ModifyDate < signsProvider.ModifyDate)
              itemWithGraphValue = signsProvider;
          }
          else
            itemWithGraphValue = signsProvider;
        }
        else
        {
          itemWithGraphValue = signsProvider;
          break;
        }
      }
    }
    return itemWithGraphValue;
  }

  public List<SignsProvider> GetItemsWithGraphValue(string graphValue)
  {
    List<SignsProvider> itemsWithGraphValue = new List<SignsProvider>();
    foreach (SignsProvider signsProvider in this._list)
    {
      if (graphValue.Equals(signsProvider.GraphValue) && !itemsWithGraphValue.Contains(signsProvider))
        itemsWithGraphValue.Add(signsProvider);
    }
    return itemsWithGraphValue;
  }

  public List<SignsProvider> GetItemsWithSignsError(SignsErrors error)
  {
    List<SignsProvider> itemsWithSignsError = new List<SignsProvider>();
    foreach (SignsProvider signsProvider in this._list)
    {
      if (error.Equals((object) signsProvider.ErrorCode) && !itemsWithSignsError.Contains(signsProvider))
        itemsWithSignsError.Add(signsProvider);
    }
    return itemsWithSignsError;
  }

  public List<SignsProvider> GetItemsWithSignsErrors()
  {
    List<SignsProvider> itemsWithSignsErrors = new List<SignsProvider>();
    foreach (SignsProvider signsProvider in this._list)
    {
      if (!signsProvider.ErrorCode.Equals((object) SignsErrors.NoError) && !itemsWithSignsErrors.Contains(signsProvider))
        itemsWithSignsErrors.Add(signsProvider);
    }
    return itemsWithSignsErrors;
  }

  public int Count => this._list.Count;

  public IEnumerator<SignsProvider> GetEnumerator()
  {
    return (IEnumerator<SignsProvider>) this._list.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();
}

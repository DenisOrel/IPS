// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.CreateRequestHelper
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.DataFormats;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

internal static class CreateRequestHelper
{
  internal static void CreateRequestHandler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null || items.Count == 0)
      return;
    for (int index = 0; index < items.Count; ++index)
    {
      if (items.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData)
        RequestCreator.CreateRequest(itemData.ObjectID);
    }
  }
}

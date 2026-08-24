// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ArticlesView.SubscribersArticlesView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.ArticlesView;

internal class SubscribersArticlesView
{
  public static bool Initialized;
  private static List<ISubscriberArticlesView> _subscribers;

  public static void Initialize()
  {
    SubscribersArticlesView._subscribers = new List<ISubscriberArticlesView>(1);
    SubscribersArticlesView._subscribers.Add((ISubscriberArticlesView) new StandartArticlesModel());
    SubscribersArticlesView.Initialized = true;
  }

  public static ISubscriberArticlesView GetSubscriber(int objectType)
  {
    foreach (ISubscriberArticlesView subscriber in SubscribersArticlesView._subscribers)
    {
      if (subscriber.IsHandle(objectType))
        return subscriber;
    }
    return (ISubscriberArticlesView) null;
  }
}

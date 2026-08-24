// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.IArticleTypeLinksItem
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal interface IArticleTypeLinksItem
{
  int InObjectType { get; }

  int ObjectType { get; }

  int LinkType { get; }

  int Required { get; }
}

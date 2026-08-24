// Decompiled with JetBrains decompiler
// Type: Intermech.NormaCSIntegrator.Client.ConstsHolder
// Assembly: Intermech.NormaCSIntegrator.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: BC215C8E-677A-43E5-99F7-5ED2ECAA0726
// Assembly location: D:\IPS\Client\Intermech.NormaCSIntegrator.Client.dll

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.NormaCSIntegrator.Client;

internal class ConstsHolder
{
  public static readonly Guid AttrImbaseLink = new Guid("cad00209-306c-11d8-b4e9-00304f19f545");
  public static int AttrImbaseLinkID = 0;
  public static readonly Guid AttrGost = new Guid("cad003de-306c-11d8-b4e9-00304f19f545");
  public static int AttrGostID = 0;
  public static readonly Guid AttrName = new Guid("cad00020-306c-11d8-b4e9-00304f19f545");
  public static int AttrNameID = 0;

  static ConstsHolder()
  {
    ConstsHolder.AttrImbaseLinkID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.AttrImbaseLink);
    ConstsHolder.AttrGostID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.AttrGost);
    ConstsHolder.AttrNameID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.AttrName);
  }
}

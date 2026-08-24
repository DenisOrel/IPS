// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.Caches.IntKeyCache
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using System;

#nullable disable
namespace Intermech.ImpExp.Manager.Caches;

internal class IntKeyCache : BaseCache
{
  public override void AddValue(object oldKey, long newKey, string caption, ITagImportObject tag)
  {
    base.AddValue((object) Convert.ToInt64(oldKey), newKey, caption, tag);
  }

  public override DictionaryValue GetInfo(object oldKey)
  {
    return base.GetInfo((object) Convert.ToInt64(oldKey));
  }

  public override string GetCaption(object oldKey)
  {
    return base.GetCaption((object) Convert.ToInt64(oldKey));
  }

  public override long GetNewKey(object oldKey) => base.GetNewKey((object) Convert.ToInt64(oldKey));

  public override ITagImportObject GetTag(object oldKey)
  {
    return base.GetTag((object) Convert.ToInt64(oldKey));
  }
}

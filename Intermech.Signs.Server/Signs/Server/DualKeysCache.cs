// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Server.DualKeysCache
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using Intermech.Localization;
using System;
using System.Collections.Concurrent;

#nullable disable
namespace Intermech.Signs.Server;

public class DualKeysCache
{
  private ConcurrentDictionary<DualKeysCache.Keys, object> table = new ConcurrentDictionary<DualKeysCache.Keys, object>();

  public void Add(object key1, object key2, object value)
  {
    this.table.TryAdd(new DualKeysCache.Keys(key1, key2), value);
  }

  public void Remove(object key1, object key2)
  {
    this.table.TryRemove(new DualKeysCache.Keys(key1, key2), out object _);
  }

  public void Clear() => this.table.Clear();

  public bool ContainsKeys(object key1, object key2)
  {
    return this.table.ContainsKey(new DualKeysCache.Keys(key1, key2));
  }

  public object this[object key1, object key2]
  {
    get
    {
      object obj = (object) null;
      return this.table.TryGetValue(new DualKeysCache.Keys(key1, key2), out obj) ? obj : (object) null;
    }
  }

  private class Keys
  {
    private object key1;
    private object key2;

    public Keys(object key1, object key2)
    {
      if (key1 == null)
        throw new ArgumentNullException(nameof (key1), LocalizationHolder.rm.GetString("Signs.Server_1"));
      if (key2 == null)
        throw new ArgumentNullException(nameof (key2), LocalizationHolder.rm.GetString("Signs.Server_2"));
      this.key1 = key1;
      this.key2 = key2;
    }

    public object Key1 => this.key1;

    public object Key2 => this.key2;

    private bool EqualsThis(DualKeysCache.Keys other)
    {
      return other != null && other.key1.Equals(this.key1) && other.key2.Equals(this.key2);
    }

    public override bool Equals(object obj)
    {
      return !(obj is DualKeysCache.Keys other) ? base.Equals(obj) : this.EqualsThis(other);
    }

    public override int GetHashCode()
    {
      return this.key1.GetHashCode() << 16 /*0x10*/ ^ this.key2.GetHashCode();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.PowerShell.ConcurrentLRUList`1
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.PowerShell;

internal sealed class ConcurrentLRUList<T>
{
  private List<T> list;

  public ConcurrentLRUList() => this.list = new List<T>(64 /*0x40*/);

  public void AddOrUpdate(T value)
  {
    lock (this.list)
    {
      int index = this.list.IndexOf(value);
      if (index >= 0)
      {
        if (index >= this.list.Count - 1)
          return;
        this.list.RemoveAt(index);
        this.list.Add(value);
      }
      else
        this.list.Add(value);
    }
  }

  public T TryGetLast()
  {
    lock (this.list)
      return this.list.Count != 0 ? this.list[0] : default (T);
  }
}

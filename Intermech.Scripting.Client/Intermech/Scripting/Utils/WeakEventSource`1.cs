// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Utils.WeakEventSource`1
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Intermech.Scripting.Utils;

internal sealed class WeakEventSource<T> where T : EventArgs
{
  private WeakEventSource<T>.WeakHandlerCollection handlers;
  private static readonly OpenEventHandlerBuilder<T> openEventHandlerBuilder = new OpenEventHandlerBuilder<T>();

  public WeakEventSource() => this.handlers = new WeakEventSource<T>.WeakHandlerCollection();

  public void Subscribe(EventHandler<T> handler)
  {
    if (handler == null)
      throw new ArgumentNullException(nameof (handler));
    WeakEventSource<T>.WeakHandlerEntry weakHandlerEntry = new WeakEventSource<T>.WeakHandlerEntry(handler.Method, handler.Target);
    this.handlers.Add(ref weakHandlerEntry);
    this.RemoveDeadHandlers();
  }

  public void Unsubscribe(EventHandler<T> handler)
  {
    if (handler == null)
      throw new ArgumentNullException(nameof (handler));
    WeakEventSource<T>.WeakHandlerEntry weakHandlerEntry = new WeakEventSource<T>.WeakHandlerEntry(handler.Method, handler.Target);
    int index = this.handlers.IndexOf(ref weakHandlerEntry);
    if (index < 0)
      return;
    this.handlers.RemoveAt(index);
    if (this.handlers.Count == 0)
      return;
    this.RemoveDeadHandlers();
  }

  public List<EventHandler<T>> GetInvocationList()
  {
    List<EventHandler<T>> invocationList = new List<EventHandler<T>>(this.handlers.Count);
    for (int index = 0; index < this.handlers.Count; ++index)
    {
      ref WeakEventSource<T>.WeakHandlerEntry local = ref this.handlers.GetEntry(index);
      if (local.IsInstanceHandler)
      {
        object target = local.WeakTarget.Target;
        if (target != null)
          invocationList.Add(this.CreateStrongDelegate(ref local, target));
      }
      else
        invocationList.Add(this.CreateStrongDelegate(ref local, (object) null));
    }
    return invocationList;
  }

  private EventHandler<T> CreateStrongDelegate(
    ref WeakEventSource<T>.WeakHandlerEntry entry,
    object strongTarget)
  {
    return (EventHandler<T>) Delegate.CreateDelegate(typeof (EventHandler<T>), strongTarget, entry.Method, true);
  }

  public void Raise(object sender, T eventArgs)
  {
    bool flag = false;
    for (int index = 0; index < this.handlers.Count; ++index)
    {
      ref WeakEventSource<T>.WeakHandlerEntry local = ref this.handlers.GetEntry(index);
      if (local.IsInstanceHandler)
      {
        object target = local.WeakTarget.Target;
        if (target != null)
          this.GetOpenEventHandler(ref local)(target, sender, eventArgs);
        else
          flag = ((flag ? 1 : 0) | 1) != 0;
      }
      else
        this.GetOpenEventHandler(ref local)((object) null, sender, eventArgs);
    }
    if (!flag)
      return;
    this.RemoveDeadHandlers();
  }

  private OpenEventHandler<T> GetOpenEventHandler(ref WeakEventSource<T>.WeakHandlerEntry entry)
  {
    if (entry.OpenHandlerCache == null)
      entry.OpenHandlerCache = WeakEventSource<T>.openEventHandlerBuilder.GetOpenEventHandler(entry.Method);
    return entry.OpenHandlerCache;
  }

  private void RemoveDeadHandlers()
  {
    int index = this.handlers.Count - 1;
    while (index >= 0)
    {
      ref WeakEventSource<T>.WeakHandlerEntry local = ref this.handlers.GetEntry(index);
      if (local.IsInstanceHandler && !local.WeakTarget.IsAlive)
        this.handlers.RemoveAt(index);
      else
        --index;
    }
  }

  [Flags]
  private enum HandlerEntryFlags
  {
    None = 0,
    InstanceHandler = 1,
  }

  private struct WeakHandlerEntry(MethodInfo method, object target)
  {
    private static readonly EqualityComparer<object> objectEqualityComparer = EqualityComparer<object>.Default;

    public MethodInfo Method { get; } = method;

    public WeakReference WeakTarget { get; } = new WeakReference(target);

    public WeakEventSource<T>.HandlerEntryFlags Flags { get; } = target != null ? WeakEventSource<>.HandlerEntryFlags.InstanceHandler : WeakEventSource<>.HandlerEntryFlags.None;

    public bool IsInstanceHandler
    {
      get => (this.Flags & WeakEventSource<>.HandlerEntryFlags.InstanceHandler) != 0;
    }

    public OpenEventHandler<T> OpenHandlerCache { get; set; } = (OpenEventHandler<T>) null;

    public override bool Equals(object obj)
    {
      return obj is WeakEventSource<T>.WeakHandlerEntry weakHandlerEntry && WeakEventSource<T>.WeakHandlerEntry.objectEqualityComparer.Equals(this.WeakTarget.Target, weakHandlerEntry.WeakTarget.Target) && this.Method.Equals((object) weakHandlerEntry.Method);
    }

    public override int GetHashCode()
    {
      return (1910747832 * -1521134295 + WeakEventSource<T>.WeakHandlerEntry.objectEqualityComparer.GetHashCode(this.WeakTarget.Target)) * -1521134295 + this.Method.GetHashCode();
    }

    public override string ToString() => $"WeakHandler {{Method={this.Method}}}";
  }

  private sealed class WeakHandlerCollection
  {
    private WeakEventSource<T>.WeakHandlerEntry[] items;
    private int count;
    private const int GrowCount = 4;

    public WeakHandlerCollection()
    {
      this.items = new WeakEventSource<T>.WeakHandlerEntry[4];
      this.count = 0;
    }

    public void Add(ref WeakEventSource<T>.WeakHandlerEntry item)
    {
      if (this.count == this.items.Length)
        Array.Resize<WeakEventSource<T>.WeakHandlerEntry>(ref this.items, this.count + 4);
      this.items[this.count] = item;
      ++this.count;
    }

    public void RemoveAt(int index)
    {
      if (index < 0 || index >= this.count)
        throw new ArgumentOutOfRangeException(nameof (index));
      --this.count;
      if (index < this.count)
        Array.Copy((Array) this.items, index + 1, (Array) this.items, index, this.count - index);
      this.items[this.count] = new WeakEventSource<T>.WeakHandlerEntry();
    }

    public int IndexOf(ref WeakEventSource<T>.WeakHandlerEntry item)
    {
      return Array.IndexOf<WeakEventSource<T>.WeakHandlerEntry>(this.items, item, 0, this.count);
    }

    public ref WeakEventSource<T>.WeakHandlerEntry GetEntry(int index)
    {
      if (index < 0 || index >= this.count)
        throw new ArgumentOutOfRangeException(nameof (index));
      return ref this.items[index];
    }

    public int Count => this.count;
  }
}

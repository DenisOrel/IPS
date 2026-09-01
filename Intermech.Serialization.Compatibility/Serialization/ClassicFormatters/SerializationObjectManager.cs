// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.SerializationObjectManager
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal sealed class SerializationObjectManager
{
  private readonly Dictionary<object, object> _objectSeenTable;
  private readonly StreamingContext _context;
  private SerializationEventHandler _onSerializedHandler;

  public SerializationObjectManager(StreamingContext context)
  {
    this._context = context;
    this._objectSeenTable = new Dictionary<object, object>();
  }

  public void RegisterObject(object obj)
  {
    SerializationEvents serializationEventsForType = SerializationEventsCache.GetSerializationEventsForType(obj.GetType());
    if (!serializationEventsForType.HasOnSerializingEvents || this._objectSeenTable.ContainsKey(obj))
      return;
    this._objectSeenTable[obj] = (object) true;
    serializationEventsForType.InvokeOnSerializing(obj, this._context);
    this.AddOnSerialized(obj);
  }

  public void RaiseOnSerializedEvent()
  {
    SerializationEventHandler serializedHandler = this._onSerializedHandler;
    if (serializedHandler == null)
      return;
    serializedHandler(this._context);
  }

  private void AddOnSerialized(object obj)
  {
    this._onSerializedHandler = SerializationEventsCache.GetSerializationEventsForType(obj.GetType()).AddOnSerialized(obj, this._onSerializedHandler);
  }
}

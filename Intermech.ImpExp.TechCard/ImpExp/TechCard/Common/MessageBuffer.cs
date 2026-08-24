// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.MessageBuffer
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

internal class MessageBuffer
{
  private readonly int _bufferSize;
  private readonly List<string> _messageBuffer;
  private readonly HashSet<string> _messageHash;

  public MessageBuffer(int bufferSize)
  {
    this._bufferSize = bufferSize;
    this._messageBuffer = new List<string>(bufferSize);
    this._messageHash = new HashSet<string>();
  }

  public bool Contains(string message) => this._messageHash.Contains(message);

  public void Add(string message)
  {
    if (this._messageBuffer.Count == this._bufferSize)
    {
      this._messageHash.Remove(this._messageBuffer[0]);
      this._messageBuffer.RemoveAt(0);
    }
    this._messageHash.Add(message);
    this._messageBuffer.Add(message);
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.MapiMessage
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.Office.Client;

[StructLayout(LayoutKind.Sequential)]
public class MapiMessage
{
  public int reserved;
  public string subject;
  public string noteText;
  public string messageType;
  public string dateReceived;
  public string conversationID;
  public int flags;
  public IntPtr originator;
  public int recipCount;
  public IntPtr recips;
  public int fileCount;
  public IntPtr files;
}

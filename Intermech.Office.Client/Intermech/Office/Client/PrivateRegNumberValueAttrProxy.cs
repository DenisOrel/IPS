// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.PrivateRegNumberValueAttrProxy
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;

#nullable disable
namespace Intermech.Office.Client;

internal class PrivateRegNumberValueAttrProxy
{
  [CanBeNull]
  public string Value { get; }

  public long ObjectID { get; }

  public PrivateRegNumberValueAttrProxy([CanBeNull] string val, long objectID)
  {
    this.Value = val;
    this.ObjectID = objectID;
  }

  public override string ToString() => this.Value ?? string.Empty;
}

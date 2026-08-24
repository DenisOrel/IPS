// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.DecoderItem
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System;

#nullable disable
namespace Intermech.ImpExp.Imbase;

internal sealed class DecoderItem
{
  public long FolderID;
  public string FolderCaption = string.Empty;
  public Guid FolderGuid = Guid.Empty;
  public string ErrorMessage;

  public string EncodedValue { get; private set; }

  public DecoderItem(string encodedValue) => this.EncodedValue = encodedValue;
}

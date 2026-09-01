// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.BinaryFormatterDefaults
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using Intermech.Serialization.Compatibility.Binary;
using System.Runtime.Serialization;
using System.Threading;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

public sealed class BinaryFormatterDefaults
{
  private static volatile SerializationBinder s_binder = (SerializationBinder) new BinaryFormatterCompatibilityBinder();
  private static volatile ISurrogateSelector s_surrogateSelector = (ISurrogateSelector) new BinaryFormatterCompatibilitySurrogateSelector();

  public static SerializationBinder Binder
  {
    get => BinaryFormatterDefaults.s_binder;
    set => Interlocked.Exchange<SerializationBinder>(ref BinaryFormatterDefaults.s_binder, value);
  }

  public static ISurrogateSelector SurrogateSelector
  {
    get => BinaryFormatterDefaults.s_surrogateSelector;
    set
    {
      Interlocked.Exchange<ISurrogateSelector>(ref BinaryFormatterDefaults.s_surrogateSelector, value);
    }
  }
}

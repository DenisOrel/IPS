// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.Compatibility.Binary.BinaryFormatterCompatibilitySurrogateServices
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using Intermech.Serialization.ClassicFormatters.Binary;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;

#nullable disable
namespace Intermech.Serialization.Compatibility.Binary;

public class BinaryFormatterCompatibilitySurrogateServices
{
  private volatile Func<StreamingContext, IFormatter> binaryFormatterFactory;

  public BinaryFormatterCompatibilitySurrogateServices()
  {
    this.binaryFormatterFactory = new Func<StreamingContext, IFormatter>(BinaryFormatterCompatibilitySurrogateServices.InternalBinaryFormatterFactory);
  }

  public Func<StreamingContext, IFormatter> BinaryFormatterFactory
  {
    [DebuggerStepThrough] get => this.binaryFormatterFactory;
    [DebuggerStepThrough] set
    {
      if (value == null)
        value = new Func<StreamingContext, IFormatter>(BinaryFormatterCompatibilitySurrogateServices.InternalBinaryFormatterFactory);
      Interlocked.Exchange<Func<StreamingContext, IFormatter>>(ref this.binaryFormatterFactory, value);
    }
  }

  private static IFormatter InternalBinaryFormatterFactory(StreamingContext context)
  {
    return (IFormatter) new BinaryFormatter((ISurrogateSelector) null, context);
  }
}

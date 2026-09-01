// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.Compatibility.Binary.BinaryFormatterCompatibilitySurrogateSelector
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.Compatibility.Binary;

public class BinaryFormatterCompatibilitySurrogateSelector : ISurrogateSelector
{
  private readonly BinaryFormatterCompatibilitySurrogateServices services;
  private readonly ISerializationSurrogate dbNullSurrogate;
  private readonly ISerializationSurrogate missingSurrogate;
  private readonly ISerializationSurrogate dataTableSurrogate;
  private readonly ISerializationSurrogate dataSetSurrogate;
  private readonly ISerializationSurrogate runtimeTypeSurrogate;
  private readonly NullHolderSurrogate nullHolderSurrogate;

  public BinaryFormatterCompatibilitySurrogateSelector()
    : this(new BinaryFormatterCompatibilitySurrogateServices())
  {
  }

  public BinaryFormatterCompatibilitySurrogateSelector(
    BinaryFormatterCompatibilitySurrogateServices services)
  {
    this.services = services != null ? services : throw new ArgumentNullException(nameof (services));
    this.dbNullSurrogate = (ISerializationSurrogate) new DBNullSurrogate();
    this.missingSurrogate = (ISerializationSurrogate) new MissingSurrogate();
    this.dataTableSurrogate = (ISerializationSurrogate) new DataTableSurrogate();
    this.dataSetSurrogate = (ISerializationSurrogate) new DataSetSurrogate(this.services);
    this.runtimeTypeSurrogate = (ISerializationSurrogate) new RuntimeTypeSurrogate();
    this.nullHolderSurrogate = new NullHolderSurrogate();
  }

  public void ChainSelector(ISurrogateSelector selector) => throw new NotSupportedException();

  public ISurrogateSelector GetNextSelector() => (ISurrogateSelector) null;

  public virtual ISerializationSurrogate GetSurrogate(
    Type type,
    StreamingContext context,
    out ISurrogateSelector selector)
  {
    if (type == typeof (DBNull))
    {
      selector = (ISurrogateSelector) this;
      return this.dbNullSurrogate;
    }
    if (type == typeof (Missing))
    {
      selector = (ISurrogateSelector) this;
      return this.missingSurrogate;
    }
    if (type == typeof (DataTable))
    {
      selector = (ISurrogateSelector) this;
      return this.dataTableSurrogate;
    }
    if (type == typeof (DataSet))
    {
      selector = (ISurrogateSelector) this;
      return this.dataSetSurrogate;
    }
    if (typeof (Type).IsAssignableFrom(type))
    {
      selector = (ISurrogateSelector) this;
      return this.runtimeTypeSurrogate;
    }
    if (type == typeof (NullHolder))
    {
      selector = (ISurrogateSelector) this;
      return (ISerializationSurrogate) this.nullHolderSurrogate;
    }
    selector = (ISurrogateSelector) null;
    return (ISerializationSurrogate) null;
  }
}

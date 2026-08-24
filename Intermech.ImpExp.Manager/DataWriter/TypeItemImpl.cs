// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.TypeItemImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal class TypeItemImpl : ITypeItem
{
  protected IDataWriterProxy dw;
  protected int id;
  protected Guid guid = Guid.Empty;
  protected string name = string.Empty;

  public TypeItemImpl(IDataWriterProxy dataWriter, int id, Guid guid, string name)
  {
    this.dw = dataWriter;
    this.id = id;
    this.guid = guid;
    this.name = name;
  }

  [DisplayName("Идентификатор")]
  public int ID
  {
    get => this.id;
    set
    {
      if (this.id == value)
        return;
      this.id = value;
    }
  }

  [DisplayName("Глобальный идентификатор")]
  public Guid GUID
  {
    get => this.guid;
    set
    {
      if (!(this.guid != value))
        return;
      this.guid = value;
    }
  }

  [DisplayName("Наименование")]
  public string Name
  {
    get => this.name;
    set
    {
      if (!(this.name != value))
        return;
      this.name = value;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.HasherList
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.ImpExp.Workflow;

[Serializable]
internal class HasherList : Dictionary<string, long>
{
  public HasherList()
  {
  }

  public HasherList(SerializationInfo info, StreamingContext ctxt)
    : base(info, ctxt)
  {
  }
}

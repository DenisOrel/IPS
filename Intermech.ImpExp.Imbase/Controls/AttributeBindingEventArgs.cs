// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.Controls.AttributeBindingEventArgs
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Imbase.Controls;

internal class AttributeBindingEventArgs
{
  public Guid BindingAttribute;
  public AttributeCheckResult CheckResult;
  public List<int> AttributeKeys;
  public List<TableInfo> TablesKeys;

  public AttributeBindingEventArgs(
    List<int> attributeKeys,
    List<TableInfo> tablesKeys,
    Guid bindingAttribute,
    AttributeCheckResult checkResult)
  {
    this.AttributeKeys = attributeKeys;
    this.TablesKeys = tablesKeys;
    this.BindingAttribute = bindingAttribute;
    this.CheckResult = checkResult;
  }
}

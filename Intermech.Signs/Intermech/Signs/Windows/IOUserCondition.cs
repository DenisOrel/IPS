// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Windows.IOUserCondition
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Kernel.Search;
using Intermech.Signs.Interfaces;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Windows;

internal class IOUserCondition : UserCondition
{
  public IOUserCondition(TextBox tbIOUser)
    : base(tbIOUser, (CheckBox) null)
  {
    this.attributeID = SignsHolder.SignUpIOAttrTypeID;
  }

  public new static bool IsOwnCondition(ConditionStructure cs)
  {
    return (cs.Attribute is int attribute1 && attribute1 == SignsHolder.SignUpIOAttrTypeID || cs.Attribute is Guid attribute2 && attribute2.Equals(SignsHolder.SignUpIOAttrTypeGuid)) && cs.RelationalOperator == RelationalOperators.Equal;
  }
}

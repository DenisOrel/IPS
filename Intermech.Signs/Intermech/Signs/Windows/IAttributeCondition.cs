// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Windows.IAttributeCondition
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Interfaces;
using Intermech.Kernel.Search;

#nullable disable
namespace Intermech.Signs.Windows;

internal interface IAttributeCondition
{
  ConditionStructure GetConditionStricture();

  void SetConditionStructure(
    IUserSession session,
    ConditionStructure[] structures,
    ref bool signed);

  void Clear();
}

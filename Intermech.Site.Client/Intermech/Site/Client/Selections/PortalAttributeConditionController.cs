// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Selections.PortalAttributeConditionController
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Conditions;
using Intermech.Navigator.Interfaces;

#nullable disable
namespace Intermech.Site.Client.Selections;

internal sealed class PortalAttributeConditionController : AttributeConditionController
{
  public override SelectionDataSource SupportedDataSource => SelectionDataSource.Portal;
}

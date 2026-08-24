// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionsConfigurator.ICompositionsConfiguratorClientService
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.Search.Pdm.CompositionsConfigurator;

public interface ICompositionsConfiguratorClientService
{
  void CopyApplicationConditionsToClipboard(long relationID);

  void PasteApplicationConditionsFromClipboard(IEnumerable<long> relationIds);
}

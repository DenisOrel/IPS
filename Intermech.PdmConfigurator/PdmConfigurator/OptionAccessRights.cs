// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.OptionAccessRights
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using System;

#nullable disable
namespace Intermech.PdmConfigurator;

[Flags]
[Serializable]
public enum OptionAccessRights
{
  ReadOnly = 0,
  FullAccess = 1,
}

// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TC_Configs.TechConfigCache
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TC_Configs;

[Serializable]
internal class TechConfigCache
{
  private readonly ConcurrentDictionary<int, List<TechConfigInfo>> _data = new ConcurrentDictionary<int, List<TechConfigInfo>>();

  private List<TechConfigInfo> GetConfigsById(int configId, bool createIfNotFound = false)
  {
    List<TechConfigInfo> configsById;
    if (this._data.TryGetValue(configId, out configsById) || !createIfNotFound)
      return configsById;
    configsById = new List<TechConfigInfo>();
    this._data.TryAdd(configId, configsById);
    return configsById;
  }

  private TechConfigInfo GetConfigItem(int configId, int productionId, int userId = 0)
  {
    List<TechConfigInfo> configsById = this.GetConfigsById(configId);
    return configsById == null ? (TechConfigInfo) null : configsById.FirstOrDefault<TechConfigInfo>((Func<TechConfigInfo, bool>) (item => item.Production == productionId && item.UserId == userId));
  }

  public TechConfigCache()
  {
  }

  public TechConfigCache(SerializationInfo serializationInfo, StreamingContext streamingContext)
  {
  }

  public void Add(TechConfigInfo configInfo)
  {
    if (configInfo == null)
      throw new ArgumentNullException(nameof (configInfo));
    this.GetConfigsById(configInfo.Id, true).Add(configInfo);
  }

  public string GetCustomConfigById(int configId, int productionId, out bool isCommonProduction)
  {
    isCommonProduction = false;
    TechConfigInfo configItem = this.GetConfigItem(configId, productionId);
    if (configItem == null && productionId > 0)
    {
      configItem = this.GetConfigItem(configId, 0);
      isCommonProduction = true;
    }
    if (configItem == null)
      return string.Empty;
    return !(configItem.Config != string.Empty) ? configItem.BigData : configItem.Config;
  }
}

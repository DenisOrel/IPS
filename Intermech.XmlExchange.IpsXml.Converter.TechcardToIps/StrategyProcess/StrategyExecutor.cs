// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess.StrategyExecutor
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using Intermech.Interfaces;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConvertStrategies;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyParams;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess;

internal class StrategyExecutor
{
  private IpsXmlLogger _logger;
  private ConvertSessionInfo _sessionInfo;
  private IServiceProvider _services;
  private Dictionary<string, Type> _defaultStrategyTypes = new Dictionary<string, Type>();

  public StrategyExecutor(IServiceProvider services)
  {
    this._services = services;
    this._logger = services.GetService<IpsXmlLogger>();
    this._sessionInfo = services.GetService<ConvertSessionInfo>();
    this.InitializeDefaultStrategies();
  }

  public XmlStrategyConvertResultType ExecuteStrategy(
    ConvertStrategyConfig strategyConfig,
    AddStrategyParams strategyParams)
  {
    this._logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_script_execution"), (object) strategyConfig.ScriptFileName));
    try
    {
      string scriptCode = File.ReadAllText(strategyConfig.ScriptFileName);
      ICSharpScriptExecutor service = ServiceUtils.GetService<ICSharpScriptExecutor>((object) ApplicationServices.Container, true);
      if (service.CanExecuteInSandbox(scriptCode))
      {
        CSharpScriptObjectKeeper scriptObjectKeeper = (CSharpScriptObjectKeeper) null;
        try
        {
          scriptObjectKeeper = service.CreateScriptObject(scriptCode, CSharpScriptInvocationOptions.Default);
          if (scriptObjectKeeper.ScriptObject.GetType().GetProperty("Strategy")?.GetValue(scriptObjectKeeper.ScriptObject) is XmlEntityConvertStrategy entityConvertStrategy)
          {
            entityConvertStrategy.StrategyParams = strategyParams;
            return entityConvertStrategy.Convert();
          }
          this._logger.Error("convertStrategy not found in " + strategyConfig.ScriptFileName);
          return XmlStrategyConvertResultType.FatalError;
        }
        catch (Exception ex)
        {
          this._logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_strategy_create"), (object) strategyConfig.ScriptFileName));
          this._logger.Error(ex.ToString());
          return XmlStrategyConvertResultType.FatalError;
        }
        finally
        {
          scriptObjectKeeper?.Dispose();
        }
      }
      else
      {
        this._logger.Error("Unsupported script format " + strategyConfig.ScriptFileName);
        return XmlStrategyConvertResultType.FatalError;
      }
    }
    catch (Exception ex)
    {
      if (ex is XmlConvertException)
      {
        this._logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_script_result"), (object) strategyConfig.ScriptFileName, (object) ex.Message));
        return (ex as XmlConvertException).ScriptMessage;
      }
      this._logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_script_execution"), (object) strategyConfig.ScriptFileName, (object) ex.Message));
      throw;
    }
  }

  public XmlStrategyConvertResultType ExecuteDefaultStrategy(AddStrategyParams strategyParams)
  {
    Type type = (Type) null;
    object obj;
    string name;
    if (strategyParams.TryGetValue(AddStrategyParamType.ConvertTarget, out obj) && obj != null)
    {
      name = obj.GetType().Name;
      Type[] interfaces = obj.GetType().GetInterfaces();
      int index = 0;
      while (index < interfaces.Length && !this._defaultStrategyTypes.TryGetValue(interfaces[index].Name, out type))
        ++index;
    }
    else if (strategyParams.TryGetValue(AddStrategyParamType.ConvertTargetType, out obj) && obj != null)
    {
      name = (obj as Type).Name;
      this._defaultStrategyTypes.TryGetValue(name, out type);
    }
    else
    {
      this._logger.Error(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_default_convert_strategy_no_type_provided"));
      return XmlStrategyConvertResultType.FatalError;
    }
    if (type == (Type) null)
    {
      this._logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_no_default_convert_strategy_for_type"), (object) name));
      return XmlStrategyConvertResultType.FatalError;
    }
    this._logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_default_convert_strategy"), (object) name));
    try
    {
      if (!(Activator.CreateInstance(type) is XmlEntityConvertStrategy instance))
      {
        this._logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_default_convert_strategy_create"), (object) type.Name));
        return XmlStrategyConvertResultType.FatalError;
      }
      instance.StrategyParams = strategyParams;
      int num = (int) instance.Convert();
      this._logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_default_convert_strategy_complete"), (object) name));
      return (XmlStrategyConvertResultType) num;
    }
    catch (Exception ex)
    {
      if (ex is XmlConvertException)
      {
        this._logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_script_result"), (object) name, (object) ex.Message));
        return (ex as XmlConvertException).ScriptMessage;
      }
      this._logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_script_execution"), (object) name, (object) ex.Message));
      throw;
    }
  }

  private void InitializeDefaultStrategies()
  {
    this._logger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_default_strategies_initialization"));
    ((IEnumerable<Type>) Assembly.GetExecutingAssembly().GetTypes()).Where<Type>((Func<Type, bool>) (sourceType => sourceType.IsSubclassOf(typeof (XmlEntityConvertStrategy)))).ToList<Type>().ForEach((Action<Type>) (type =>
    {
      DefaultConvertStrategyForTypeAttribute customAttribute = type.GetCustomAttribute<DefaultConvertStrategyForTypeAttribute>();
      if (customAttribute == null)
        return;
      if (this._defaultStrategyTypes.ContainsKey(customAttribute.TypeName))
      {
        this._logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_default_strategy_dublicate"), (object) customAttribute.TypeName, (object) this._defaultStrategyTypes[customAttribute.TypeName].GetType().Name, (object) type.Name));
      }
      else
      {
        this._defaultStrategyTypes.Add(customAttribute.TypeName, type);
        this._logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_default_strategy_initialized"), (object) customAttribute.TypeName, (object) type.Name));
      }
    }));
    this._logger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_default_strategies_initialization_complete"));
  }
}

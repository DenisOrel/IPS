// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.TechToIpsXmlService
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using ICSharpCode.SharpZipLib.Zip;
using Intermech.Interfaces;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyParams;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess;
using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger.Configuration;
using Intermech.XmlExchange.IpsXml.Interfaces.Utils;
using Intermech.XmlExchange.IpsXml.Provider.Ips.Serializer;
using Intermech.XmlExchange.IpsXml.Provider.Techcard;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Threading;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps;

public sealed class TechToIpsXmlService
{
  private const string ARCHIVE_NAME = "convertation.zip";

  public string Convert(
    string techXmlFileName,
    string configFileName,
    string workDir,
    IUserSession userSession,
    CancellationToken cancellationToken,
    Action<int, string> onInitProgress,
    Action<int, string> onProgress)
  {
    if (onInitProgress != null)
      onInitProgress(10, Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgLoadConfigFile"));
    Guid sessionID = Guid.NewGuid();
    ServiceContainer serviceContainer = new ServiceContainer();
    IpsXmlLogger ipsXmlLogger = new IpsXmlLogger(Path.Combine(Path.GetDirectoryName(configFileName), "convert.log"));
    try
    {
      ipsXmlLogger.Clear();
      serviceContainer.AddService<IpsXmlLogger>(ipsXmlLogger);
      ipsXmlLogger.LoggerConfig.MessageTypes = LogMessageTypes.Info | LogMessageTypes.Warn | LogMessageTypes.Error;
      ipsXmlLogger.Clear();
      if (!File.Exists(configFileName))
      {
        ipsXmlLogger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_config_file_not_found"), (object) configFileName));
        return (string) null;
      }
      ipsXmlLogger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_config_file_loading"), (object) configFileName));
      TechcardToIpsConfig service1 = new TechCardToIpsConfigLoader((IServiceProvider) serviceContainer).LoadConfig(configFileName);
      if (service1 == null)
      {
        ipsXmlLogger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_config_file_loading"), (object) configFileName));
        return (string) null;
      }
      if (!Directory.Exists(workDir))
        workDir = !string.IsNullOrEmpty(service1.OutputConfig.WorkDir) ? Path.GetFullPath(service1.OutputConfig.WorkDir) : Directory.GetCurrentDirectory();
      workDir = Path.Combine(workDir, DateTime.Now.ToString("yyyy_MM_dd_HH_MM_ss"));
      XmlUtils.RecreateDirectory(workDir);
      ipsXmlLogger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_apply_log_config"));
      ipsXmlLogger.Close();
      ipsXmlLogger = new IpsXmlLogger(Path.Combine(workDir, "convert.log"));
      ipsXmlLogger.LoggerConfig.MessageTypes = service1.LoggerConfig.Infos ? ipsXmlLogger.LoggerConfig.MessageTypes | LogMessageTypes.Info : ipsXmlLogger.LoggerConfig.MessageTypes;
      ipsXmlLogger.LoggerConfig.MessageTypes = service1.LoggerConfig.Warnings ? ipsXmlLogger.LoggerConfig.MessageTypes | LogMessageTypes.Warn : ipsXmlLogger.LoggerConfig.MessageTypes;
      ipsXmlLogger.LoggerConfig.MessageTypes = service1.LoggerConfig.Errors ? ipsXmlLogger.LoggerConfig.MessageTypes | LogMessageTypes.Error : ipsXmlLogger.LoggerConfig.MessageTypes;
      ipsXmlLogger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_log_config_applied"));
      ipsXmlLogger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_session_id"), (object) sessionID.ToString()));
      ipsXmlLogger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_work_dir"), (object) workDir));
      if (!File.Exists(techXmlFileName))
      {
        ipsXmlLogger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_input_data_file_not_found"), (object) techXmlFileName));
        return (string) null;
      }
      string str1 = Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgServiceRegistration");
      if (onProgress != null)
        onProgress(1, str1 + Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgLogService"));
      serviceContainer.RemoveService<IpsXmlLogger>();
      serviceContainer.AddService<IpsXmlLogger>(ipsXmlLogger);
      if (onProgress != null)
        onProgress(2, str1 + Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgCurrentSessionInfoService"));
      ConvertSessionInfo service2 = new ConvertSessionInfo(sessionID, techXmlFileName, workDir, userSession);
      serviceContainer.AddService<ConvertSessionInfo>(service2);
      if (onProgress != null)
        onProgress(3, str1 + Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgConfigService"));
      serviceContainer.AddService<TechcardToIpsConfig>(service1);
      if (onProgress != null)
        onProgress(4, str1 + Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgInputDataService"));
      ipsXmlLogger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_load_input_data_file"), (object) techXmlFileName));
      IXmlDataProvider dataProvider = new TechXmlDataFactory().GetDataProvider(new string[1]
      {
        techXmlFileName
      });
      ipsXmlLogger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_load_input_data_file_complete"), (object) techXmlFileName));
      serviceContainer.AddService<IXmlDataProvider>(dataProvider);
      if (onProgress != null)
        onProgress(5, str1 + Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgSerializationService"));
      IpsDataSerializer service3 = new IpsDataSerializer((IServiceProvider) serviceContainer);
      serviceContainer.AddService<IpsDataSerializer>(service3);
      if (onProgress != null)
        onProgress(6, str1 + Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgIdConfigService"));
      ConfigIdCalculator service4 = new ConfigIdCalculator((IServiceProvider) serviceContainer);
      service4.Prepare(service1.IdConfigs);
      serviceContainer.AddService<ConfigIdCalculator>(service4);
      if (onProgress != null)
        onProgress(7, str1 + Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgConvertedCacheService"));
      ConvertedDataCache service5 = new ConvertedDataCache();
      serviceContainer.AddService<ConvertedDataCache>(service5);
      if (onProgress != null)
        onProgress(8, str1 + Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgConvertStrategyService"));
      StrategyExecutor service6 = new StrategyExecutor((IServiceProvider) serviceContainer);
      serviceContainer.AddService<StrategyExecutor>(service6);
      if (onProgress != null)
        onProgress(9, str1 + Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgEntityValueConvertService"));
      ValueConverter service7 = new ValueConverter((IServiceProvider) serviceContainer);
      serviceContainer.AddService<ValueConverter>(service7);
      if (onProgress != null)
        onProgress(10, str1 + Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgUniqueIdGenService"));
      IdGenerator service8 = new IdGenerator((IServiceProvider) serviceContainer);
      serviceContainer.AddService<IdGenerator>(service8);
      ipsXmlLogger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_start_converting"));
      int num = 0;
      if (onInitProgress != null)
        onInitProgress(dataProvider.RootObjects.Count + 1, Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgConvertationStart"));
      if (dataProvider.RootObjects.Count > 0)
      {
        string str2 = Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgObjectConvertation");
        foreach (IXmlObject rootObject in (IEnumerable<IXmlObject>) dataProvider.RootObjects)
        {
          if (onProgress != null)
            onProgress(num, str2 + rootObject.Description);
          ++num;
          if (cancellationToken.IsCancellationRequested)
          {
            ipsXmlLogger.Warn("Конвертация была прервана в ручную");
            return (string) null;
          }
          ipsXmlLogger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_root_object_convertion"), (object) rootObject.Description));
          string configId = service4.FindConfigId((IXmlEntity) rootObject);
          if (string.IsNullOrEmpty(configId))
            ipsXmlLogger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_no_config_found"), (object) rootObject.Description));
          else
            ipsXmlLogger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_search_complete"), (object) rootObject.Description, (object) configId));
          ObjectConfig objectConfig = service1.ObjectConfigs[configId];
          if (objectConfig == null)
          {
            ipsXmlLogger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_no_config_found"), (object) rootObject.Description));
          }
          else
          {
            XmlStrategyConvertResultType convertResultType = XmlStrategyConvertResultType.FatalError;
            AddStrategyParams addStrategyParams = new AddStrategyParams();
            addStrategyParams.Add(AddStrategyParamType.GlobalServices, (object) serviceContainer);
            addStrategyParams.Add(AddStrategyParamType.ConvertTarget, (object) rootObject);
            addStrategyParams.Add(AddStrategyParamType.ConvertTargetConfig, (object) objectConfig);
            AddStrategyParams strategyParams = addStrategyParams;
            foreach (string id in objectConfig.ConvertStrategies.Ids)
            {
              bool flag = false;
              convertResultType = service6.ExecuteStrategy(objectConfig.ConvertStrategies[id], strategyParams);
              switch (convertResultType)
              {
                case XmlStrategyConvertResultType.FatalError:
                  ipsXmlLogger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_critical_error_object_convertion"), (object) rootObject.Description));
                  return (string) null;
                case XmlStrategyConvertResultType.MinorError:
                  ipsXmlLogger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_minor_error_object_convertion"), (object) rootObject.Description));
                  break;
                case XmlStrategyConvertResultType.WrongStrategyChoise:
                  ipsXmlLogger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_wrong_strategy_selected_for_obj"), (object) rootObject.Description));
                  break;
                case XmlStrategyConvertResultType.Converted:
                  ipsXmlLogger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_obj_converted"), (object) rootObject.Description));
                  flag = true;
                  break;
              }
              if (flag)
                break;
            }
            if (convertResultType == XmlStrategyConvertResultType.WrongStrategyChoise || objectConfig.ConvertStrategies.Count == 0)
            {
              switch (service6.ExecuteDefaultStrategy(strategyParams))
              {
                case XmlStrategyConvertResultType.FatalError:
                  ipsXmlLogger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_critical_error_object_convertion"), (object) rootObject.Description));
                  return (string) null;
                case XmlStrategyConvertResultType.MinorError:
                  ipsXmlLogger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_minor_error_object_convertion"), (object) rootObject.Description));
                  break;
                case XmlStrategyConvertResultType.WrongStrategyChoise:
                  ipsXmlLogger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_wrong_strategy_selected_for_obj"), (object) rootObject.Description));
                  break;
                case XmlStrategyConvertResultType.Converted:
                  ipsXmlLogger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_obj_converted"), (object) rootObject.Description));
                  break;
              }
            }
            ipsXmlLogger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_root_object_convertion_complete"), (object) rootObject.Description));
          }
        }
      }
      else
        ipsXmlLogger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_warn_no_root_objects_found"), (object) techXmlFileName));
      ipsXmlLogger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_start_converting_complete"));
      ipsXmlLogger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_save_results_to_disk"));
      if (onProgress != null)
        onProgress(num, Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgZiping"));
      string str3 = this.ZipResults(Path.GetDirectoryName(service3.SaveData()[0]), ipsXmlLogger);
      if (onProgress != null)
        onProgress(num, Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgConvertationComplete"));
      return str3;
    }
    catch (Exception ex)
    {
      if (ipsXmlLogger != null)
      {
        ipsXmlLogger.Error(ex.Message);
        ipsXmlLogger.Error(ex.StackTrace);
      }
    }
    finally
    {
      ipsXmlLogger?.Close();
    }
    return string.Empty;
  }

  private string ZipResults(string outputDir, IpsXmlLogger logger)
  {
    logger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_archive_results"));
    IEnumerable<string> strings = Directory.EnumerateFiles(outputDir, "*.*", SearchOption.AllDirectories);
    string fileName = Path.Combine(outputDir, "convertation.zip");
    using (ZipFile zipFile = ZipFile.Create(fileName))
    {
      try
      {
        zipFile.BeginUpdate();
        try
        {
          foreach (string str in strings)
          {
            if (this.FilterFileName(Path.GetFileName(str)))
              zipFile.Add(str, str.Replace(outputDir, string.Empty));
          }
        }
        finally
        {
          zipFile.CommitUpdate();
        }
      }
      finally
      {
        zipFile.Close();
      }
    }
    logger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_remove_temp_files"));
    foreach (string path in strings)
    {
      if (this.FilterFileName(Path.GetFileName(path)))
        File.Delete(path);
    }
    foreach (string directory in Directory.GetDirectories(outputDir))
    {
      if (Directory.Exists(directory))
        Directory.Delete(directory, true);
    }
    logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_archive_results_complete"), (object) fileName));
    return fileName;
  }

  private bool FilterFileName(string fileName)
  {
    return string.Compare(fileName, "convertation.zip", true) != 0 && string.Compare(Path.GetExtension(fileName), ".log", true) != 0;
  }
}

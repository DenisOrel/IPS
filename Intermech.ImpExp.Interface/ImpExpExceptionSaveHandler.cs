// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ImpExpExceptionSaveHandler
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.UI.ExceptionHandling;
using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

public sealed class ImpExpExceptionSaveHandler : ExceptionSaveHandler
{
  protected override void DoSaveToFile(Exception exc, string reportZipName)
  {
    new ImpExpInformationRequest(ServiceUtils.GetService<IMetadataInfo>((object) ServicesManager.ServiceContainer, true).UserSession).SaveReportToXml(exc, reportZipName);
  }

  protected override void DoSendByEmail(Exception exc, string reportTopic, string reportText)
  {
    new ImpExpInformationRequest(ServiceUtils.GetService<IMetadataInfo>((object) ServicesManager.ServiceContainer, true).UserSession).SendReport(exc, reportTopic, reportText);
  }
}

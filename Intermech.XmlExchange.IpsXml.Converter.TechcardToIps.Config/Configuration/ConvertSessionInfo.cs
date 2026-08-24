// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConvertSessionInfo
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration;

public class ConvertSessionInfo
{
  public readonly Guid SessionID;
  public readonly string InputDataFile;
  public readonly string WorkDir;
  public IUserSession UserSession;

  public ConvertSessionInfo(
    Guid sessionID,
    string inputDataFile,
    string workDir,
    IUserSession userSession)
  {
    this.SessionID = sessionID;
    this.InputDataFile = inputDataFile;
    this.WorkDir = workDir;
    this.UserSession = userSession;
  }
}

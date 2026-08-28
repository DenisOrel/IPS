// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.ProjectXmlFileFormer
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Services.PortalServices;
using System.Xml;

#nullable disable
namespace Intermech.Project.Server;

internal class ProjectXmlFileFormer(
  [NotNull] IUserSession session,
  [NotNull] ExtendedTransferedObject unit,
  [NotNull] IBackupWriter writer,
  [NotNull] string data) : CustomXMLFileFormer<string>(session, unit, writer, data)
{
  protected override void WriteRootNode([NotNull] XmlDocument xmlDocument, [NotNull] XmlNode xmlRootNode)
  {
    XmlNode element = (XmlNode) xmlDocument.CreateElement(PortalConsts.XmlNodeSysAttribute);
    XMLFileHelper.AddAttribute(xmlDocument, element, "F_PARAMS", this.data);
    xmlRootNode.AppendChild(element);
  }

  [NotNull]
  public static ExtendedTransferedObject Pack(
    [NotNull] CustomPublishDataInfo processInfo,
    [NotNull] IUserSession session,
    [NotNull] IBackupWriter writer,
    [NotNull] string data)
  {
    ExtendedTransferedObject unit = new ExtendedTransferedObject(ChangeType.ctCreate, TransferedObjectCategory.AutoTransfer);
    new ProjectXmlFileFormer(session, unit, writer, data).SaveAttributes();
    return unit;
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.DocumentCommands
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Controls;
using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Office.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.Office.Client;

internal class DocumentCommands : ICommandsProvider
{
  public IServiceProvider ServiceProvider;
  private string addMessage = string.Empty;

  public CommandsInfo GetMergedCommands(ISelectedItems items, [CanBeNull] IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  [NotNull]
  public CommandsInfo GetGroupCommands([NotNull] ISelectedItems items, [CanBeNull] IServiceProvider viewServices)
  {
    CommandsInfo groupCommands = new CommandsInfo();
    if (items.Count == 1)
    {
      List<int> childrenIdRecursive = MetaDataHelper.GetObjectTypeChildrenIDRecursive(OfficeConsts.ObjtypeDocumentsID);
      if (!(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
        return CommandsInfo.Empty;
      if (childrenIdRecursive.IndexOf(itemData.ObjectType) >= 0)
        groupCommands.Add(OfficeClientConsts.CmdRegisterDocument, new CommandInfo(0, new ClickEventHandler(DocumentCommands.RegisterDocument)));
      groupCommands.Add(OfficeClientConsts.CmdCreateResolution, new CommandInfo(0, new ClickEventHandler(DocumentCommands.CreateResolution)));
      groupCommands.Add(OfficeClientConsts.CmdCreateConfidentialResolution, new CommandInfo(0, new ClickEventHandler(DocumentCommands.CreateConfidentialResolution)));
      if (itemData.ObjectType == OfficeConsts.ObjtypeResolutionsID)
        groupCommands.Add(OfficeClientConsts.CmdCreateResolutionByProto, new CommandInfo(0, new ClickEventHandler(DocumentCommands.CreateResolutionByProto)));
      if (childrenIdRecursive.IndexOf(itemData.ObjectType) >= 0)
      {
        if (OfficeClientConsts.IsPrivateOffice)
          groupCommands.Add(OfficeClientConsts.CmdPrivateRegister, new CommandInfo(0, new ClickEventHandler(DocumentCommands.PrivateRegister)));
        groupCommands.Add(OfficeClientConsts.CmdAnswer, new CommandInfo(0, new ClickEventHandler(DocumentCommands.Answer)));
        groupCommands.Add(OfficeClientConsts.CmdSendEmail, new CommandInfo(0, new ClickEventHandler(DocumentCommands.SendEmail)));
        groupCommands.Add(OfficeClientConsts.CmdSendEmailProcess, new CommandInfo(0, new ClickEventHandler(DocumentCommands.SendEmailProcess)));
      }
    }
    return groupCommands;
  }

  private static void SendEmailProcess(
    [NotNull] ISelectedItems items,
    [CanBeNull] IServiceProvider viewServices,
    [CanBeNull] object additionalInfo)
  {
    EmailCommands.SendEmailProcess((items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID).ObjectID);
  }

  private static void RegisterDocument(
    [NotNull] ISelectedItems items,
    [CanBeNull] IServiceProvider viewServices,
    [CanBeNull] object additionalInfo)
  {
    IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      OfficeDocumentCommands.PublicRegister(sessionKeeper.Session, itemData.ObjectID);
      if (!OfficeDocumentCommands.CheckAndPrivateRegister(sessionKeeper.Session, itemData.ObjectID))
        return;
      int num = (int) IMMessageBox.Show("Регистрация документа во внутренней канцелярии", "Документ успешно зарегистрирован во внутренней канцелярии.", MessageBoxButtons.OK, IMMessageBoxImage.Information);
    }
  }

  private static void CreateResolution(
    [NotNull] ISelectedItems items,
    [CanBeNull] IServiceProvider viewServices,
    [CanBeNull] object additionalInfo)
  {
    ResolutionCommands.Create((items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID).ObjectID);
  }

  private static void CreateResolutionByProto(
    [NotNull] ISelectedItems items,
    [CanBeNull] IServiceProvider viewServices,
    [CanBeNull] object additionalInfo)
  {
    ResolutionCommands.CreateByPrototype((items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID).ObjectID);
  }

  private static void CreateConfidentialResolution(
    [NotNull] ISelectedItems items,
    [CanBeNull] IServiceProvider viewServices,
    [CanBeNull] object additionalInfo)
  {
    ResolutionCommands.Create((items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID).ObjectID, true);
  }

  private static void Answer(
    [NotNull] ISelectedItems items,
    [CanBeNull] IServiceProvider viewServices,
    [CanBeNull] object additionalInfo)
  {
    OfficeDocumentCommands.CreateAnswer((items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID).ObjectID);
  }

  private static void SendEmail(
    [NotNull] ISelectedItems items,
    [CanBeNull] IServiceProvider viewServices,
    [CanBeNull] object additionalInfo)
  {
    EmailCommands.SendEmail((items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID).ObjectID);
  }

  private static void PrivateRegister(
    [NotNull] ISelectedItems items,
    [CanBeNull] IServiceProvider viewServices,
    [CanBeNull] object additionalInfo)
  {
    IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    if (!OfficeDocumentCommands.PrivateRegister(itemData.ObjectID, itemData.ObjectType))
      return;
    int num = (int) IMMessageBox.Show("Регистрация документа во внутренней канцелярии", "Документ успешно зарегистрирован во внутренней канцелярии.", MessageBoxButtons.OK, IMMessageBoxImage.Information);
  }

  private void ImportFromSMDO(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    OpenFileDialog openFileDialog1 = new OpenFileDialog();
    openFileDialog1.InitialDirectory = ClientContext.FileVault.WorkArea.AreaPath;
    openFileDialog1.Title = "Выберите файл для импорта в IPS";
    openFileDialog1.Filter = "Файлы пакетов СМДО (xml)|*.xml";
    openFileDialog1.RestoreDirectory = true;
    OpenFileDialog openFileDialog2 = openFileDialog1;
    if (openFileDialog2.ShowDialog() != DialogResult.OK)
      return;
    string path1 = $"{ClientContext.FileVault.WorkArea.AreaPath}\\smdo\\ERROR";
    string path2 = $"{ClientContext.FileVault.WorkArea.AreaPath}\\smdo\\REFUSAL";
    if (!Directory.Exists(path1))
      Directory.CreateDirectory(path1);
    if (!Directory.Exists(path2))
      Directory.CreateDirectory(path2);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      List<long> longList = new List<long>();
      IFileImportService service = this.ServiceProvider.GetService(typeof (IFileImportService)) as IFileImportService;
      ISMDOSettingsService customService = (ISMDOSettingsService) sessionKeeper.Session.GetCustomService(typeof (ISMDOSettingsService));
      SMDOSettings settings = customService.Settings;
      if (string.IsNullOrEmpty(settings.SmdoEmail))
        throw new KernelException("Не задан e-mail адрес сервера СМДО в общих настройках Канцелярия/СМДО!");
      if (string.IsNullOrEmpty(settings.CompanySMDOid))
        throw new KernelException("Не задан идентификатор вашей организации в общих настройках Канцелярия/СМДО!");
      if (string.IsNullOrEmpty(settings.CompanyName))
        throw new KernelException("Не задано наименование вашей организации в общих настройках Канцелярия/СМДО!");
      if (string.IsNullOrEmpty(settings.SystemID))
        throw new KernelException("Не задан идентификатор(GUID) системы в справочнике СМДО в общих настройках Канцелярия/СМДО!");
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      XmlDocument xmlDocument = new XmlDocument();
      try
      {
        xmlDocument.Load(openFileDialog2.FileName);
      }
      catch (Exception ex)
      {
        throw new KernelException($"Ошибка открытия файла {openFileDialog2.FileName}", ex);
      }
      int num1 = OfficeClientConsts.TranslateSmdoVerToInt(xmlDocument.SelectSingleNode($"/{Tag.Envelop}/@{Tag.type}").Value);
      XmlNode xmlNode1 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Header}/@{Tag.msg_type}");
      XmlNode xmlNode2 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/@{Tag.msg_id}");
      string empty1 = string.Empty;
      if (xmlNode2 != null)
        empty1 = xmlNode2.Value;
      XmlNode xmlNode3 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/@{Tag.dtstamp}");
      string empty2 = string.Empty;
      if (xmlNode3 != null)
      {
        string str1 = xmlNode3.Value;
      }
      XmlNode xmlNode4 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/@{Tag.subject}");
      string empty3 = string.Empty;
      if (xmlNode4 != null)
        empty3 = xmlNode4.Value;
      if (xmlNode1 == null)
        throw new KernelException("Ошибка при разборе XML-пакета: структура XML не соответствует формату СМДО или была повреждена");
      if (xmlNode1.Value == "1" || xmlNode1.Value == "3")
      {
        XmlNode xmlNode5 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Body}/{Tag.Document}/{Tag.RegNumber}");
        string regNum = string.Empty;
        if (xmlNode5 != null)
          regNum = xmlNode5.InnerText;
        XmlNode xmlNode6 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Body}/{Tag.Document}/{Tag.RegNumber}/@{Tag.regdate}");
        string empty4 = string.Empty;
        if (xmlNode6 != null)
          empty4 = xmlNode6.Value;
        XmlNode xmlNode7 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Header}/{Tag.Sender}/@{Tag.name}");
        XmlNode xmlNode8 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Header}/{Tag.Sender}/@{Tag.id}");
        XmlNode xmlNode9 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Body}/{Tag.Document}/@{Tag.kind}");
        XmlNodeList xmlNodeList1 = xmlDocument.SelectNodes($"/{Tag.Envelop}/{Tag.Body}/{Tag.Document}/{Tag.DocTransfer}");
        string empty5 = string.Empty;
        string empty6 = string.Empty;
        bool flag = true;
        try
        {
          empty5 = xmlNode7.Value;
          empty6 = xmlNode8.Value;
          if (xmlNodeList1 != null && (num1 >= OfficeClientConsts.SmdoVer211Int ? (xmlNode9 != null ? 1 : 0) : 1) != 0 && xmlNode2 != null && xmlNode4 != null && xmlNode6 != null && xmlNode5 != null)
          {
            foreach (XmlNode xmlNode10 in xmlNodeList1)
            {
              string str2 = xmlNode10.Attributes[Tag.name].Value;
              XmlNode childNode = xmlNode10.ChildNodes[0];
              string empty7 = string.Empty;
              string s = string.Empty;
              if (childNode.Attributes[Tag.referenceid] != null)
                empty7 = childNode.Attributes[Tag.referenceid].Value;
              else
                s = childNode.InnerText;
              if (!string.IsNullOrEmpty(empty7))
              {
                if (!dictionary1.ContainsKey(empty7))
                  dictionary1.Add(empty7, str2);
              }
              else if (!dictionary1.ContainsKey("DATABYTES_" + s))
                dictionary1.Add("DATABYTES_" + s, str2);
              XmlNodeList xmlNodeList2 = xmlNode10.SelectNodes(Tag.Signature);
              if (xmlNodeList2 == null || xmlNodeList2.Count == 0)
              {
                flag = false;
                this.GenerateAckXML(1, -22, settings, empty5, empty6, empty1, (string) null, (string) null, empty3, empty4, regNum, customService, $"Файл {str2}: ");
                int num2 = (int) MessageBox.Show("Отсутствует ЭП для одного или нескольких документов");
                File.Copy(openFileDialog2.FileName, $"{path1}\\{Path.GetFileName(openFileDialog2.FileName)}", true);
                break;
              }
              byte[] numArray = string.IsNullOrEmpty(empty7) ? Convert.FromBase64String(s) : File.ReadAllBytes($"{Path.GetDirectoryName(openFileDialog2.FileName)}\\{empty7}");
              foreach (XmlNode xmlNode11 in xmlNodeList2)
              {
                int num3 = 0;
                try
                {
                  string innerText = xmlNode11.InnerText;
                  X509Certificate2 x509Certificate2 = (X509Certificate2) null;
                  byte[] messageData = numArray;
                  ref X509Certificate2 local1 = ref x509Certificate2;
                  ref int local2 = ref num3;
                  int num4 = Win32.CheckMessageSign(innerText, messageData, ref local1, out local2);
                  Dictionary<string, string> dictionary2 = (Dictionary<string, string>) null;
                  string empty8 = string.Empty;
                  string str3 = string.Empty;
                  if (x509Certificate2 != null)
                    dictionary2 = this.X509Parse(x509Certificate2.Subject);
                  string str4;
                  if (dictionary2 != null)
                  {
                    str4 = dictionary2["SN"];
                    str3 = dictionary2.ContainsKey("Отчество") ? dictionary2["Отчество"] : (dictionary2.ContainsKey("OID.2.5.4.41") ? dictionary2["OID.2.5.4.41"] : string.Empty);
                  }
                  else
                    str4 = "* недоступно из-за ошибки проверки подписи *";
                  this.addMessage = $"Файл {str2}: Владелец подписи: '{str4} {str3}': ";
                  if (num4 != 0)
                  {
                    flag = false;
                    if (num4 != -1)
                    {
                      switch (num4 - -4)
                      {
                        case 0:
                          this.addMessage = $"{this.addMessage}{"Ненадёжный корневой сертификат"}";
                          break;
                        case 1:
                          this.addMessage = $"{this.addMessage}{"Сертификат был отозван"}";
                          break;
                        case 2:
                          this.addMessage = $"{this.addMessage}{"Срок действия сертификата истёк"}";
                          break;
                      }
                      this.GenerateAckXML(1, -23, settings, empty5, empty6, empty1, (string) null, (string) null, empty3, empty4, regNum, customService, this.addMessage);
                      int num5 = (int) MessageBox.Show(this.addMessage);
                      File.Copy(openFileDialog2.FileName, $"{path1}\\{Path.GetFileName(openFileDialog2.FileName)}", true);
                      break;
                    }
                    this.GenerateAckXML(1, -21, settings, empty5, empty6, empty1, (string) null, (string) null, empty3, empty4, regNum, customService, this.addMessage);
                    string str5 = string.Empty;
                    if (num3 != 0)
                      str5 = "\nКод ошибки WinAPI: 0x" + num3.ToString("X");
                    int num6 = (int) MessageBox.Show("ЭП не верна: нарушена целостность подписанного документа(ов) или не найден файл, на который существует ссылка" + str5);
                    File.Copy(openFileDialog2.FileName, $"{path1}\\{Path.GetFileName(openFileDialog2.FileName)}", true);
                    break;
                  }
                }
                catch (Exception ex)
                {
                  this.GenerateAckXML(1, -21, settings, empty5, empty6, empty1, (string) null, (string) null, empty3, empty4, regNum, customService, this.addMessage);
                  flag = false;
                  string str6 = string.Empty;
                  if (num3 != 0)
                    str6 = "\nКод ошибки WinAPI: 0x" + num3.ToString("X");
                  int num7 = (int) MessageBox.Show("Неверная структура ЭП" + str6);
                  File.Copy(openFileDialog2.FileName, $"{path1}\\{Path.GetFileName(openFileDialog2.FileName)}", true);
                  break;
                }
              }
              if (!flag)
                break;
            }
            if (flag)
              this.GenerateAckXML(1, 0, settings, empty5, empty6, empty1, (string) null, (string) null, empty3, empty4, regNum, customService);
          }
          else
          {
            this.GenerateAckXML(1, -1, settings, empty5, empty6, empty1, (string) null, (string) null, empty3, empty4, regNum, customService);
            flag = false;
            File.Copy(openFileDialog2.FileName, $"{path1}\\{Path.GetFileName(openFileDialog2.FileName)}", true);
            int num8 = (int) MessageBox.Show("Ошибка при разборе XML-пакета: структура XML не соответствует формату СМДО или была повреждена");
          }
        }
        catch (Exception ex)
        {
          this.GenerateAckXML(1, -1, settings, empty5, empty6, empty1, (string) null, (string) null, empty3, empty4, regNum, customService);
          flag = false;
          File.Copy(openFileDialog2.FileName, $"{path1}\\{Path.GetFileName(openFileDialog2.FileName)}", true);
          int num9 = (int) MessageBox.Show(ex.Message);
        }
        if (!flag)
          return;
        string path3 = $"{ClientContext.FileVault.WorkArea.AreaPath}\\smdo\\IN";
        if (!Directory.Exists(path3))
          Directory.CreateDirectory(path3);
        foreach (KeyValuePair<string, string> keyValuePair in dictionary1)
        {
          DialogResult dialogResult = DialogResult.Yes;
          string str7 = $"{path3}\\{keyValuePair.Value}";
          try
          {
            if (!keyValuePair.Key.StartsWith("DATABYTES_"))
            {
              File.Copy($"{Path.GetDirectoryName(openFileDialog2.FileName)}\\{keyValuePair.Key}", str7, true);
              File.Copy(openFileDialog2.FileName, $"{path3}\\{Path.GetFileName(openFileDialog2.FileName)}");
            }
            else
            {
              string[] strArray = keyValuePair.Key.Split(new string[1]
              {
                "DATABYTES_"
              }, StringSplitOptions.RemoveEmptyEntries);
              File.WriteAllBytes(str7, Convert.FromBase64String(strArray[0]));
              File.Copy(openFileDialog2.FileName, $"{path3}\\{Path.GetFileName(openFileDialog2.FileName)}");
            }
          }
          catch (Exception ex)
          {
            if (File.Exists(str7))
            {
              int num10 = (int) MessageBox.Show($"Файл '{openFileDialog2.FileName}' не удалось импортировать. Директория '{path3}' уже содержит файл '{keyValuePair.Value}' и он открыт на редактирование. Продолжить импорт уже существующего файла?", "Копирование прервано", MessageBoxButtons.YesNo);
            }
            else
            {
              int num11 = (int) MessageBox.Show(ex.Message);
              dialogResult = DialogResult.None;
            }
          }
          if (dialogResult == DialogResult.Yes)
          {
            try
            {
              longList.Add(service.ImportFile(str7));
            }
            catch (FaultException ex)
            {
              this.GenerateAckXML(2, 2, settings, empty5, empty6, empty1, "В регистрации отказано.", DateTime.Now.ToString((IFormatProvider) CultureInfo.InvariantCulture), empty3, empty4, regNum, customService, ex.Message);
              int num12 = (int) MessageBox.Show(ex.Message);
            }
          }
        }
        foreach (long num13 in longList)
        {
          try
          {
            OfficeDocumentCommands.PublicRegister(sessionKeeper.Session, num13);
            IDBObject dbObject = sessionKeeper.Session.GetObject(num13);
            IDBAttribute byId = dbObject.Attributes.FindByID(OfficeConsts.AttrRegNumberID);
            string regObjectID = byId != null ? byId.AsString : dbObject.Caption;
            this.GenerateAckXML(2, 0, settings, empty5, empty6, empty1, regObjectID, dbObject.CreateDate.ToString(OfficeClientConsts.SmdoDateFormat), empty3, empty4, regNum, customService);
          }
          catch (Exception ex)
          {
            this.GenerateAckXML(2, 1, settings, empty5, empty6, empty1, "Регистрации не подлежит", DateTime.Now.ToString(OfficeClientConsts.SmdoDateFormat), empty3, empty4, regNum, customService, ex.Message);
            throw new KernelException(ex.Message, ex);
          }
        }
      }
      else
      {
        if (!(xmlNode1.Value == "0"))
          return;
        if (xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Body}/{Tag.Acknowledgement}/@{Tag.ack_type}") == null)
          throw new KernelException("Неверный формат уведомления");
        try
        {
          int num14 = (int) MessageBox.Show(xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Body}/{Tag.Acknowledgement}/{Tag.AckResult}").InnerText);
        }
        catch (Exception ex)
        {
          throw new KernelException("Неверный формат уведомления", ex);
        }
      }
    }
  }

  public Dictionary<string, string> X509Parse(string X509Value)
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    string str1 = X509Value;
    string[] separator = new string[1]{ ", " };
    foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
    {
      int length = str2.IndexOf('=');
      if (length != -1)
      {
        string key = str2.Substring(0, length);
        string str3 = str2.Remove(0, length + 1).TrimStart('"').TrimEnd('"');
        if (!dictionary.ContainsKey(key))
          dictionary[key] = str3;
      }
    }
    return dictionary;
  }

  private void GenerateAckXML(
    int ackID,
    int errorCode,
    SMDOSettings settings,
    string receiverName,
    string receiverID,
    string msgGuid,
    string regObjectID,
    string regObjectDate,
    string subjects,
    string regDate,
    string regNum,
    ISMDOSettingsService settingsService,
    string addMessage = "")
  {
    string str1 = string.Empty;
    string str2 = string.Empty;
    switch (ackID)
    {
      case 1:
        str2 = "Уведомление о неудачной доставке документа";
        switch (errorCode)
        {
          case -23:
            str1 = $"Документ отклонён. {addMessage}";
            break;
          case -22:
            str1 = $"Документ отклонён. {addMessage} Отсутствует ЭП для одного или нескольких документов";
            break;
          case -21:
            str1 = $"Документ отклонён. {addMessage} ЭП не верна: нарушена целостность подписанного документа(ов) или не найден файл, на который существует ссылка";
            break;
          case -1:
            str1 = "Документ отклонён. Ошибка при разборе XML-пакета: структура XML не соответствует формату СМДО или была повреждена";
            break;
          case 0:
            str2 = "Уведомление о доставке документа";
            str1 = string.Format("Документ исх. № {1} доставлен в систему документооборота {0}", (object) DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), (object) regNum);
            break;
          default:
            str1 = "Документ отклонён. Неизвестная ошибка";
            break;
        }
        break;
      case 2:
        str2 = "Уведомление о неудачной регистрации документа";
        switch (errorCode)
        {
          case 0:
            str2 = "Уведомление о регистрации документа";
            str1 = string.Format("Документ исх. № {2} зарегистрирован номером {0} от {1}", (object) regObjectID, (object) regObjectDate, (object) regNum);
            break;
          case 1:
            str1 = string.Format("Документ отклонён. Документ исх. № {1} относится к категории нерегистрируемых: {0}", (object) addMessage, (object) regNum);
            break;
          default:
            str1 = $"Документ отклонён. {addMessage}";
            break;
        }
        break;
    }
    string path = $"{ClientContext.FileVault.WorkArea.AreaPath}\\smdo\\OUT";
    if (!Directory.Exists(path))
      Directory.CreateDirectory(path);
    string str3 = Guid.NewGuid().ToString();
    string str4;
    for (str4 = $"{path}\\{str3}_ack.xml"; File.Exists(str4); str4 = $"{path}\\{str3}_ack.xml")
      str3 = Guid.NewGuid().ToString();
    XmlTextWriter xmlTextWriter = new XmlTextWriter(str4, Encoding.UTF8);
    xmlTextWriter.WriteStartDocument();
    xmlTextWriter.WriteStartElement(Tag.Envelop);
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.Close();
    XmlDocument xmlDocument = new XmlDocument();
    try
    {
      xmlDocument.Load(str4);
    }
    catch (Exception ex)
    {
      throw new KernelException($"Ошибка открытия файла {str4}", ex);
    }
    XmlAttribute attribute1 = xmlDocument.CreateAttribute(Tag.type);
    attribute1.Value = OfficeClientConsts.SmdoVerActualStr;
    xmlDocument.DocumentElement.Attributes.Append(attribute1);
    XmlAttribute attribute2 = xmlDocument.CreateAttribute(Tag.msg_id);
    attribute2.Value = str3;
    xmlDocument.DocumentElement.Attributes.Append(attribute2);
    XmlAttribute attribute3 = xmlDocument.CreateAttribute(Tag.dtstamp);
    attribute3.Value = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
    xmlDocument.DocumentElement.Attributes.Append(attribute3);
    XmlAttribute attribute4 = xmlDocument.CreateAttribute(Tag.subject);
    attribute4.Value = $"{str2} {subjects}";
    xmlDocument.DocumentElement.Attributes.Append(attribute4);
    XmlNode element1 = (XmlNode) xmlDocument.CreateElement(Tag.Header);
    xmlDocument.DocumentElement.AppendChild(element1);
    XmlAttribute attribute5 = xmlDocument.CreateAttribute(Tag.msg_type);
    attribute5.Value = "0";
    element1.Attributes.Append(attribute5);
    XmlNode element2 = (XmlNode) xmlDocument.CreateElement(Tag.Sender);
    element1.AppendChild(element2);
    XmlAttribute attribute6 = xmlDocument.CreateAttribute(Tag.id);
    attribute6.Value = settings.CompanySMDOid;
    element2.Attributes.Append(attribute6);
    XmlAttribute attribute7 = xmlDocument.CreateAttribute(Tag.name);
    attribute7.Value = settings.CompanyName;
    element2.Attributes.Append(attribute7);
    XmlAttribute attribute8 = xmlDocument.CreateAttribute(Tag.sys_id);
    attribute8.Value = string.Empty;
    element2.Attributes.Append(attribute8);
    XmlAttribute attribute9 = xmlDocument.CreateAttribute(Tag.system);
    attribute9.Value = "IPS";
    element2.Attributes.Append(attribute9);
    XmlAttribute attribute10 = xmlDocument.CreateAttribute(Tag.system_details);
    attribute10.Value = $"Версия {typeof (DocumentCommands).Assembly.GetName().Version.Major}.{typeof (DocumentCommands).Assembly.GetName().Version.Minor}";
    element2.Attributes.Append(attribute10);
    XmlNode element3 = (XmlNode) xmlDocument.CreateElement(Tag.Receiver);
    element1.AppendChild(element3);
    XmlAttribute attribute11 = xmlDocument.CreateAttribute(Tag.id);
    attribute11.Value = receiverID;
    element3.Attributes.Append(attribute11);
    XmlAttribute attribute12 = xmlDocument.CreateAttribute(Tag.name);
    attribute12.Value = receiverName;
    element3.Attributes.Append(attribute12);
    XmlNode element4 = (XmlNode) xmlDocument.CreateElement(Tag.Organization);
    element3.AppendChild(element4);
    XmlAttribute attribute13 = xmlDocument.CreateAttribute(Tag.organization_string);
    attribute13.Value = receiverName;
    element4.Attributes.Append(attribute13);
    XmlNode element5 = (XmlNode) xmlDocument.CreateElement(Tag.Body);
    xmlDocument.DocumentElement.AppendChild(element5);
    XmlNode element6 = (XmlNode) xmlDocument.CreateElement(Tag.Acknowledgement);
    element5.AppendChild(element6);
    XmlAttribute attribute14 = xmlDocument.CreateAttribute(Tag.ack_type);
    attribute14.Value = ackID.ToString();
    element6.Attributes.Append(attribute14);
    XmlAttribute attribute15 = xmlDocument.CreateAttribute(Tag.msg_id);
    attribute15.Value = msgGuid;
    element6.Attributes.Append(attribute15);
    XmlNode element7 = (XmlNode) xmlDocument.CreateElement(Tag.RegNumber);
    element6.AppendChild(element7);
    XmlAttribute attribute16 = xmlDocument.CreateAttribute(Tag.regdate);
    attribute16.Value = regDate;
    element7.Attributes.Append(attribute16);
    element7.InnerText = regNum;
    if (ackID == 2)
    {
      XmlNode element8 = (XmlNode) xmlDocument.CreateElement(Tag.IncNumber);
      element6.AppendChild(element8);
      element8.InnerText = regObjectID;
      XmlAttribute attribute17 = xmlDocument.CreateAttribute(Tag.regdate);
      attribute17.Value = regObjectDate;
      element8.Attributes.Append(attribute17);
    }
    XmlNode element9 = (XmlNode) xmlDocument.CreateElement(Tag.AckResult);
    element6.AppendChild(element9);
    element9.InnerText = str1;
    XmlAttribute attribute18 = xmlDocument.CreateAttribute(Tag.errorcode);
    attribute18.Value = errorCode.ToString();
    element9.Attributes.Append(attribute18);
    xmlDocument.Save(str4);
    DialogResult dialogResult = DialogResult.No;
    if (ackID == 1)
      dialogResult = MessageBox.Show("Уведомление о доставке сформировано. Отправить его на сервер СМДО? (При ответе нет откроется письмо в стандартном почтовом клиенте)", "Внимание", MessageBoxButtons.YesNo);
    else if (ackID == 2)
      dialogResult = MessageBox.Show("Уведомление о регистрации сформировано. Отправить его на сервер СМДО? (При ответе нет откроется письмо в стандартном почтовом клиенте)", "Внимание", MessageBoxButtons.YesNo);
    StringBuilder stringBuilder = new StringBuilder();
    if (string.IsNullOrEmpty(settings.MyCompanyEmail))
      stringBuilder.Append("E-mail адрес компании;\n");
    if (string.IsNullOrEmpty(settings.Password))
      stringBuilder.Append("Пароль;\n");
    if (string.IsNullOrEmpty(settings.SMDOHost))
      stringBuilder.Append("Адрес сервера СМДО;\n");
    if (settings.Port == 0)
      stringBuilder.Append("Порт сервера СМДО;\n");
    if (string.IsNullOrEmpty(settings.UserName))
      stringBuilder.Append("Имя пользователя;");
    if (dialogResult == DialogResult.Yes && stringBuilder.Length > 10)
      dialogResult = MessageBox.Show($"Внимание! В общих настройках Канцелярия/СМДО не заданы: {stringBuilder}\n Открыть письмо в почтовом клиенте?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes ? DialogResult.No : DialogResult.Cancel;
    switch (dialogResult)
    {
      case DialogResult.Yes:
        string str5 = string.Empty;
        using (FileStream fileStream = new FileStream(str4, FileMode.Open, FileAccess.Read))
        {
          Dictionary<FileStream, string> attachments = new Dictionary<FileStream, string>()
          {
            {
              fileStream,
              Path.GetFileName(str4)
            }
          };
          str5 = settingsService.SendEmail(settings, $"{str2} {subjects}", attachments);
        }
        int num = str5 == "Сообщение отправлено" ? (int) MessageBox.Show(str5) : throw new KernelException(str5);
        break;
      case DialogResult.No:
        new MAPI().ComposeMail(new string[1]
        {
          settings.SmdoEmail
        }, $"{str2} {subjects}", subjects, new string[1]
        {
          str4
        });
        break;
    }
  }

  private void ExportToSMDO(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable dataTable = sessionKeeper.Session.GetObjectCollection(OfficeConsts.ObjtypeOrganizationID).Select(new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(OfficeConsts.AttrSMDO_IdentityID, RelationalOperators.NotEmpty, (object) "", LogicalOperators.NONE, 0, true)
      }, new object[3]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID,
        (object) ObligatoryObjectAttributes.CAPTION,
        (object) OfficeConsts.AttrSMDO_IdentityID
      }));
      if (dataTable.Rows.Count == 0)
        throw new KernelException("Не найдены организации у которых заполнено поле с идентификатором в системе СМДО");
      ISMDOSettingsService customService = (ISMDOSettingsService) sessionKeeper.Session.GetCustomService(typeof (ISMDOSettingsService));
      SMDOSettings settings = customService.Settings;
      if (string.IsNullOrEmpty(settings.SmdoEmail))
        throw new KernelException("Не задан e-mail адрес сервера СМДО в общих настройках Канцелярия/СМДО!");
      if (string.IsNullOrEmpty(settings.CompanySMDOid))
        throw new KernelException("Не задан идентификатор вашей организации в общих настройках Канцелярия/СМДО!");
      if (string.IsNullOrEmpty(settings.CompanyName))
        throw new KernelException("Не задано наименование вашей организации в общих настройках Канцелярия/СМДО!");
      if (string.IsNullOrEmpty(settings.SystemID))
        throw new KernelException("Не задан идентификатор(GUID) системы в справочнике СМДО в общих настройках Канцелярия/СМДО!");
      SMDOEmailForm smdoEmailForm = new SMDOEmailForm(dataTable.Rows);
      if (smdoEmailForm.ShowDialog() != DialogResult.OK)
        return;
      for (int index1 = 0; index1 < items.Count; ++index1)
      {
        if (items.GetItemData(index1, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData)
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(itemData.ObjectID);
          IDBAttribute[] attributesByType = dbObject.Attributes.GetAttributesByType(FieldTypes.ftFile);
          if (attributesByType.Length != 0)
          {
            List<string> filesName = new List<string>();
            for (int index2 = 0; index2 < attributesByType.Length; ++index2)
            {
              PublishedObject publishedObject = ClientContext.FileVault.ViewArea.Publish((IList<DBObjectState>) ClientContext.FileVault.DBObjectsInfo.CreateStateListForObjectTree(dbObject.ObjectID, VersionsRuleSources.GetEditorRule()));
              filesName.AddRange(publishedObject.ObjectFiles.Select<PublishedFile, string>((System.Func<PublishedFile, string>) (files => files.FullName)));
            }
            this.CreateSMDOXML(dbObject, settings, smdoEmailForm.SmdoEmailDataSettings, customService, filesName, sessionKeeper.Session);
          }
          else
          {
            int num = (int) MessageBox.Show($"У объекта {dbObject.Caption} отсутствует документ для отправки в СМДО");
          }
        }
      }
    }
  }

  private void CreateSMDOXML(
    IDBObject obj,
    SMDOSettings settings,
    SMDOEmailDataSettings smdoEmailDataSettings,
    ISMDOSettingsService settingsService,
    List<string> filesName,
    IUserSession session)
  {
    List<string> stringList = new List<string>();
    string str1 = string.Empty;
    try
    {
      string path = $"{ClientContext.FileVault.WorkArea.AreaPath}\\smdo\\OUT";
      Dictionary<FileStream, string> attachments = new Dictionary<FileStream, string>();
      string empty = string.Empty;
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      string str2 = Guid.NewGuid().ToString();
      for (str1 = $"{path}\\{str2}_data.xml"; File.Exists(str1); str1 = $"{path}\\{str2}_data.xml")
        str2 = Guid.NewGuid().ToString();
      IDBAttribute byId = obj.Attributes.FindByID(OfficeConsts.AttrRegNumberID);
      XmlTextWriter xmlTextWriter = new XmlTextWriter(str1, Encoding.UTF8);
      xmlTextWriter.WriteStartDocument();
      xmlTextWriter.WriteStartElement(Tag.Envelop);
      xmlTextWriter.WriteEndElement();
      xmlTextWriter.Close();
      XmlDocument xmlDocument = new XmlDocument();
      try
      {
        xmlDocument.Load(str1);
      }
      catch (Exception ex)
      {
        throw new KernelException($"Ошибка открытия файла {str1}", ex);
      }
      XmlAttribute attribute1 = xmlDocument.CreateAttribute(Tag.type);
      attribute1.Value = OfficeClientConsts.SmdoVerActualStr;
      xmlDocument.DocumentElement.Attributes.Append(attribute1);
      XmlAttribute attribute2 = xmlDocument.CreateAttribute(Tag.msg_id);
      attribute2.Value = str2;
      xmlDocument.DocumentElement.Attributes.Append(attribute2);
      XmlAttribute attribute3 = xmlDocument.CreateAttribute(Tag.dtstamp);
      XmlAttribute xmlAttribute = attribute3;
      DateTime dateTime1 = DateTime.UtcNow;
      string str3 = dateTime1.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
      xmlAttribute.Value = str3;
      xmlDocument.DocumentElement.Attributes.Append(attribute3);
      XmlAttribute attribute4 = xmlDocument.CreateAttribute(Tag.subject);
      string str4 = byId != null ? byId.AsString : obj.Caption;
      dateTime1 = obj.CreateDate;
      string shortDateString = dateTime1.ToShortDateString();
      string str5 = $"{str4} от {shortDateString}";
      attribute4.Value = str5;
      xmlDocument.DocumentElement.Attributes.Append(attribute4);
      XmlNode element1 = (XmlNode) xmlDocument.CreateElement(Tag.Header);
      xmlDocument.DocumentElement.AppendChild(element1);
      XmlAttribute attribute5 = xmlDocument.CreateAttribute(Tag.msg_type);
      attribute5.Value = "1";
      element1.Attributes.Append(attribute5);
      XmlAttribute attribute6 = xmlDocument.CreateAttribute(Tag.msg_acknow);
      attribute6.Value = "2";
      element1.Attributes.Append(attribute6);
      XmlNode element2 = (XmlNode) xmlDocument.CreateElement(Tag.Sender);
      element1.AppendChild(element2);
      XmlAttribute attribute7 = xmlDocument.CreateAttribute(Tag.id);
      attribute7.Value = settings.CompanySMDOid;
      element2.Attributes.Append(attribute7);
      XmlAttribute attribute8 = xmlDocument.CreateAttribute(Tag.name);
      attribute8.Value = settings.CompanyName;
      element2.Attributes.Append(attribute8);
      XmlAttribute attribute9 = xmlDocument.CreateAttribute(Tag.sys_id);
      attribute9.Value = settings.SystemID;
      element2.Attributes.Append(attribute9);
      XmlAttribute attribute10 = xmlDocument.CreateAttribute(Tag.system);
      attribute10.Value = "IPS";
      element2.Attributes.Append(attribute10);
      XmlAttribute attribute11 = xmlDocument.CreateAttribute(Tag.system_details);
      attribute11.Value = $"Версия {typeof (DocumentCommands).Assembly.GetName().Version.Major}.{typeof (DocumentCommands).Assembly.GetName().Version.Minor}";
      element2.Attributes.Append(attribute11);
      foreach (KeyValuePair<string, string> organization in smdoEmailDataSettings.Organizations)
      {
        XmlNode element3 = (XmlNode) xmlDocument.CreateElement(Tag.Receiver);
        element1.AppendChild(element3);
        XmlAttribute attribute12 = xmlDocument.CreateAttribute(Tag.id);
        attribute12.Value = organization.Key;
        element3.Attributes.Append(attribute12);
        XmlAttribute attribute13 = xmlDocument.CreateAttribute(Tag.name);
        attribute13.Value = organization.Value;
        element3.Attributes.Append(attribute13);
        XmlNode element4 = (XmlNode) xmlDocument.CreateElement(Tag.Organization);
        element3.AppendChild(element4);
        XmlAttribute attribute14 = xmlDocument.CreateAttribute(Tag.organization_string);
        attribute14.Value = organization.Value;
        element4.Attributes.Append(attribute14);
      }
      XmlNode element5 = (XmlNode) xmlDocument.CreateElement(Tag.Body);
      xmlDocument.DocumentElement.AppendChild(element5);
      XmlNode element6 = (XmlNode) xmlDocument.CreateElement(Tag.Document);
      element5.AppendChild(element6);
      XmlAttribute attribute15 = xmlDocument.CreateAttribute(Tag.type);
      attribute15.Value = "0";
      element6.Attributes.Append(attribute15);
      XmlAttribute attribute16 = xmlDocument.CreateAttribute(Tag.idnumber);
      attribute16.Value = Math.Abs(obj.ObjectID).ToString();
      element6.Attributes.Append(attribute16);
      string kindName = string.Empty;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        kindName = (sessionKeeper.Session.GetCustomService(typeof (IDocumentTypeSettingsService)) as IDocumentTypeSettingsService).GetSettings(sessionKeeper.Session.SessionGUID, obj.ObjectType).DocumentTypeName;
      SmdoDocKindsBook smdoDocKindsBook = new SmdoDocKindsBook();
      smdoDocKindsBook.LoadBook();
      string smdoKindName = smdoDocKindsBook.IpsKindNameToSmdoKindName(kindName);
      XmlAttribute attribute17 = xmlDocument.CreateAttribute(Tag.kind);
      attribute17.Value = smdoKindName;
      element6.Attributes.Append(attribute17);
      XmlNode element7 = (XmlNode) xmlDocument.CreateElement(Tag.RegNumber);
      element6.AppendChild(element7);
      XmlAttribute attribute18 = xmlDocument.CreateAttribute(Tag.regdate);
      attribute18.Value = obj.CreateDate.ToString(OfficeClientConsts.SmdoDateFormat);
      element7.Attributes.Append(attribute18);
      element7.InnerText = byId != null ? byId.AsString : obj.Caption;
      XmlNode element8 = (XmlNode) xmlDocument.CreateElement("Confident");
      element6.AppendChild(element8);
      XmlAttribute attribute19 = xmlDocument.CreateAttribute(Tag.flag);
      attribute19.Value = smdoEmailDataSettings.ConfValue.ToString();
      element8.Attributes.Append(attribute19);
      element8.InnerText = smdoEmailDataSettings.ConfName;
      Dictionary<string, string> dictionary = this.X509Parse(smdoEmailDataSettings.Certificate.Subject);
      if (filesName != null)
      {
        using (CapiCertificate capiCertificate = new CapiCertificate(smdoEmailDataSettings.Certificate))
        {
          int num1 = 0;
          for (int index = 0; index < filesName.Count; ++index)
          {
            string str6 = Path.GetExtension(filesName[index]).Remove(0, 1);
            switch (str6)
            {
              case "txt":
                IDBAttribute attributeById1 = obj.GetAttributeByID(OfficeConsts.AttrPagesCountID);
                num1 += attributeById1 == null ? 1 : (int) attributeById1.AsInteger;
                break;
              case "doc":
              case "docx":
                IDBAttribute attributeById2 = obj.GetAttributeByID(OfficeConsts.AttrPagesCountID);
                num1 += attributeById2 == null ? 1 : (int) attributeById2.AsInteger;
                break;
              case "pdf":
                FileStream fileStream = new FileStream(filesName[index], FileMode.Open, FileAccess.Read);
                MatchCollection matchCollection = new Regex("/Type\\s*/Page[^s]").Matches(new StreamReader((Stream) fileStream).ReadToEnd());
                num1 += matchCollection.Count > 0 ? matchCollection.Count : 1;
                fileStream.Close();
                break;
              default:
                IDBAttribute attributeById3 = obj.GetAttributeByID(OfficeConsts.AttrPagesCountID);
                num1 += attributeById3 == null ? 1 : (int) attributeById3.AsInteger;
                break;
            }
            XmlNode element9 = (XmlNode) xmlDocument.CreateElement(Tag.DocTransfer);
            element6.AppendChild(element9);
            XmlAttribute attribute20 = xmlDocument.CreateAttribute(Tag.name);
            attribute20.Value = Path.GetFileName(filesName[index]);
            element9.Attributes.Append(attribute20);
            XmlAttribute attribute21 = xmlDocument.CreateAttribute(Tag.type);
            attribute21.Value = str6;
            element9.Attributes.Append(attribute21);
            XmlAttribute attribute22 = xmlDocument.CreateAttribute(Tag.ordernum);
            attribute22.Value = (index + 1).ToString();
            element9.Attributes.Append(attribute22);
            XmlNode element10 = (XmlNode) xmlDocument.CreateElement(Tag.Data);
            element9.AppendChild(element10);
            XmlAttribute attribute23 = xmlDocument.CreateAttribute(Tag.referenceid);
            attribute23.Value = $"{Math.Abs(obj.ObjectID)}_attach{index}";
            try
            {
              File.Copy(filesName[index], $"{path}\\{attribute23.Value}", true);
            }
            catch
            {
            }
            element10.Attributes.Append(attribute23);
            FileStream key = new FileStream($"{path}\\{attribute23.Value}", FileMode.Open, FileAccess.Read);
            attachments.Add(key, attribute23.Value);
            stringList.Add($"{path}\\{attribute23.Value}");
            if (smdoEmailDataSettings.IsHaveSigns)
            {
              try
              {
                byte[] inArray = capiCertificate.SignObjectHash(File.ReadAllBytes(filesName[index]));
                XmlNode element11 = (XmlNode) xmlDocument.CreateElement(Tag.Signature);
                element9.AppendChild(element11);
                element11.InnerText = Convert.ToBase64String(inArray);
                XmlAttribute attribute24 = xmlDocument.CreateAttribute(Tag.keyid);
                attribute24.Value = smdoEmailDataSettings.OpenKeyID;
                element11.Attributes.Append(attribute24);
                XmlAttribute attribute25 = xmlDocument.CreateAttribute(Tag.signtime);
                attribute25.Value = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
                element11.Attributes.Append(attribute25);
                XmlAttribute attribute26 = xmlDocument.CreateAttribute(Tag.signer);
                if (dictionary.ContainsKey("SN") && dictionary.ContainsKey("Отчество"))
                  attribute26.Value = $"{dictionary["SN"]} {dictionary["Отчество"]}";
                else
                  attribute26.Value = session.UserName;
                element11.Attributes.Append(attribute26);
              }
              catch (Exception ex)
              {
                throw new KernelException($"Произошла ошибка при подписании файла: Сообщение: {ex.Message}", ex.InnerException);
              }
            }
          }
          XmlAttribute attribute27 = xmlDocument.CreateAttribute(Tag.pages);
          IDBAttribute attributeById = obj.GetAttributeByID(OfficeConsts.AttrPagesCountID);
          int num2 = attributeById == null ? 1 : (int) attributeById.AsInteger;
          attribute27.Value = num2.ToString();
          element6.Attributes.Append(attribute27);
        }
      }
      XmlNode element12 = (XmlNode) xmlDocument.CreateElement(Tag.Author);
      element6.AppendChild(element12);
      XmlNode element13 = (XmlNode) xmlDocument.CreateElement(Tag.OrganizationWithSign);
      element12.AppendChild(element13);
      XmlAttribute attribute28 = xmlDocument.CreateAttribute(Tag.organization_string);
      attribute28.Value = settings.CompanyName;
      element13.Attributes.Append(attribute28);
      XmlNode element14 = (XmlNode) xmlDocument.CreateElement(Tag.OfficialPersonWithSign);
      element13.AppendChild(element14);
      XmlNode element15 = (XmlNode) xmlDocument.CreateElement(Tag.Name);
      element15.InnerText = !dictionary.ContainsKey("SN") || !dictionary.ContainsKey("Отчество") ? session.UserName : $"{dictionary["SN"]} {dictionary["Отчество"]}";
      element14.AppendChild(element15);
      XmlNode element16 = (XmlNode) xmlDocument.CreateElement(Tag.Official);
      RoleProperties[] rolesList = session.GetRolesList(session.UserID);
      long roleId = session.RoleID;
      foreach (RoleProperties roleProperties in rolesList)
      {
        if (roleProperties.RoleID == roleId)
        {
          element16.InnerText = roleProperties.RoleName;
          break;
        }
      }
      element14.AppendChild(element16);
      XmlNode element17 = (XmlNode) xmlDocument.CreateElement(Tag.SignDate);
      XmlNode xmlNode = element17;
      DateTime dateTime2 = DateTime.Now;
      dateTime2 = dateTime2.Date;
      string str7 = dateTime2.ToString(OfficeClientConsts.SmdoDateFormat);
      xmlNode.InnerText = str7;
      element14.AppendChild(element17);
      xmlDocument.Save(str1);
      DialogResult dialogResult = MessageBox.Show(string.Format("Создание отчёта по файлу {1} завершено. Он сохранён под именем {0} Отправить его на сервер СМДО? (При ответе нет откроется письмо в стандартном почтовом клиенте)", (object) Path.GetFileName(str1), (object) obj.Caption), "Внимание", MessageBoxButtons.YesNo);
      StringBuilder stringBuilder = new StringBuilder();
      if (string.IsNullOrEmpty(settings.MyCompanyEmail))
        stringBuilder.Append("Не задан e-mail адрес компании в общих настройках Канцелярия/СМДО!\n");
      if (string.IsNullOrEmpty(settings.Password))
        stringBuilder.Append("Не задан пароль в общих настройках Канцелярия/СМДО!\n");
      if (string.IsNullOrEmpty(settings.SMDOHost))
        stringBuilder.Append("Не задан адрес сервера СМДО в общих настройках Канцелярия/СМДО!\n");
      if (settings.Port == 0)
        stringBuilder.Append("Не задан порт сервера СМДО в общих настройках Канцелярия/СМДО!\n");
      if (string.IsNullOrEmpty(settings.UserName))
        stringBuilder.Append("Не задано имя пользователя в общих настройках Канцелярия/СМДО!");
      if (dialogResult == DialogResult.Yes && stringBuilder.Length > 10)
        dialogResult = MessageBox.Show($"Внимание: {stringBuilder}\n Открыть письмо в почтовом клиенте?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes ? DialogResult.No : DialogResult.Cancel;
      switch (dialogResult)
      {
        case DialogResult.Yes:
          string str8 = string.Empty;
          using (FileStream key = new FileStream(str1, FileMode.Open, FileAccess.Read))
          {
            attachments.Add(key, Path.GetFileName(str1));
            str8 = settingsService.SendEmail(settings, str5, attachments);
          }
          int num = str8 == "Сообщение отправлено" ? (int) MessageBox.Show(str8) : throw new KernelException(str8);
          break;
        case DialogResult.No:
          string[] recipients = new string[1]
          {
            settings.SmdoEmail
          };
          stringList.Add(str1);
          new MAPI().ComposeMail(recipients, str5, str5, stringList.ToArray());
          break;
      }
    }
    catch (Exception ex)
    {
      if (!string.IsNullOrEmpty(str1))
        File.Delete(str1);
      throw new KernelException(ex.Message);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.GeneralSettings
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Office.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

internal sealed class GeneralSettings : 
  IPropertyPage,
  ISortedPropertyGrid,
  IPropertyPageSearchOptionEvents
{
  public GeneralSettings([NotNull] IUserSession session) => this.ReLoad(session);

  private void ReLoad([NotNull] IUserSession session)
  {
    OfficeGeneralSettings settings = session.GetCustomService<IOfficeGeneralSettingsService>().Settings;
    this.IncomingDocResetType = settings.IncomingDocResetType;
    this.OutgoingDocResetType = settings.OutgoingDocResetType;
    this.InternalDocResetType = settings.InternalDocResetType;
    this.AutoSendTemplate = (TemplatePropertyClass) null;
    if (settings.TemplateID != 0L)
    {
      QuickObjectInfo objectInfo = session.GetObjectInfo(settings.TemplateID);
      if (!objectInfo.Empty)
        this.AutoSendTemplate = new TemplatePropertyClass(settings.TemplateID, objectInfo.Caption);
    }
    this.ConsistentControlResolutionTemplate = (TemplatePropertyClass) null;
    if (settings.ConsistentControlResolutionTemplateID != 0L)
    {
      QuickObjectInfo objectInfo = session.GetObjectInfo(settings.ConsistentControlResolutionTemplateID);
      if (!objectInfo.Empty)
        this.ConsistentControlResolutionTemplate = new TemplatePropertyClass(settings.ConsistentControlResolutionTemplateID, objectInfo.Caption);
    }
    this.ConsistentNonControlResolutionTemplate = (TemplatePropertyClass) null;
    if (settings.ConsistentNonControlResolutionTemplateID != 0L)
    {
      QuickObjectInfo objectInfo = session.GetObjectInfo(settings.ConsistentNonControlResolutionTemplateID);
      if (!objectInfo.Empty)
        this.ConsistentNonControlResolutionTemplate = new TemplatePropertyClass(settings.ConsistentNonControlResolutionTemplateID, objectInfo.Caption);
    }
    this.ParallelControlResolutionTemplate = (TemplatePropertyClass) null;
    if (settings.ParallelControlResolutionTemplateID != 0L)
    {
      QuickObjectInfo objectInfo = session.GetObjectInfo(settings.ParallelControlResolutionTemplateID);
      if (!objectInfo.Empty)
        this.ParallelControlResolutionTemplate = new TemplatePropertyClass(settings.ParallelControlResolutionTemplateID, objectInfo.Caption);
    }
    this.ParallelNonControlResolutionTemplate = (TemplatePropertyClass) null;
    if (settings.ParallelNonControlResolutionTemplateID != 0L)
    {
      QuickObjectInfo objectInfo = session.GetObjectInfo(settings.ParallelNonControlResolutionTemplateID);
      if (!objectInfo.Empty)
        this.ParallelNonControlResolutionTemplate = new TemplatePropertyClass(settings.ParallelNonControlResolutionTemplateID, objectInfo.Caption);
    }
    this.SendAddresseeTemplate = (TemplatePropertyClass) null;
    if (settings.AddresseeTemplateID != 0L)
    {
      QuickObjectInfo objectInfo = session.GetObjectInfo(settings.AddresseeTemplateID);
      if (!objectInfo.Empty)
        this.SendAddresseeTemplate = new TemplatePropertyClass(settings.AddresseeTemplateID, objectInfo.Caption);
    }
    this.AutoSendUser = (UserPropertyClass) null;
    if (settings.UserID != 0L)
    {
      QuickObjectInfo objectInfo = session.GetObjectInfo(settings.UserID);
      if (!objectInfo.Empty)
        this.AutoSendUser = new UserPropertyClass(settings.UserID, objectInfo.Caption);
    }
    this.AutoSendEmail = settings.AutoSendEmail;
    this.PrivateOffice = settings.PrivateOffice;
    this.FilterResolutions = settings.FilterResolutions;
    this.IncomingPrivateFolderEnable = settings.IncomingPrivateFolderEnable;
    this.CaptionAttributeForEmailMessages = settings.CaptionAttributeForEmailMessages != 0 ? new AttributePropertyClass(settings.CaptionAttributeForEmailMessages) : (AttributePropertyClass) null;
    if (!(ServicesManager.GetService(typeof (IDBConfigurations)) is IDBConfigurations service))
      return;
    this.AddCountWhileAddingReferenceOnCompositeMaterialParts = service.ReadBool("MSOfficeAddins", "Core", "AddCount", true, DBConfigMode.GlobalOnly);
  }

  [CustomDisplayName("Attribute.Office.Client_1")]
  [CustomDescription("Attribute.Office.Client_1")]
  [CustomCategory("Attribute.Office.Client_2")]
  public CountResetTypes IncomingDocResetType { get; set; }

  [CustomDisplayName("Attribute.Office.Client_3")]
  [CustomDescription("Attribute.Office.Client_3")]
  [CustomCategory("Attribute.Office.Client_2")]
  public CountResetTypes OutgoingDocResetType { get; set; }

  [CustomDisplayName("Attribute.Office.Client_4")]
  [CustomDescription("Attribute.Office.Client_4")]
  [CustomCategory("Attribute.Office.Client_2")]
  public CountResetTypes InternalDocResetType { get; set; }

  [CustomDisplayName("Attribute.Office.Client_5")]
  [CustomDescription("Attribute.Office.Client_5")]
  [CustomCategory("Attribute.Office.Client_6")]
  [CanBeNull]
  public TemplatePropertyClass AutoSendTemplate { get; set; }

  [CustomDisplayName("Attribute.Office.Client_16")]
  [CustomDescription("Attribute.Office.Client_16")]
  [CustomCategory("Attribute.Office.Client_13")]
  [CanBeNull]
  public TemplatePropertyClass ConsistentControlResolutionTemplate { get; set; }

  [CustomDisplayName("Attribute.Office.Client_17")]
  [CustomDescription("Attribute.Office.Client_17")]
  [CustomCategory("Attribute.Office.Client_13")]
  [CanBeNull]
  public TemplatePropertyClass ConsistentNonControlResolutionTemplate { get; set; }

  [CustomDisplayName("Attribute.Office.Client_15")]
  [CustomDescription("Attribute.Office.Client_15")]
  [CustomCategory("Attribute.Office.Client_13")]
  [CanBeNull]
  public TemplatePropertyClass ParallelNonControlResolutionTemplate { get; set; }

  [CustomDisplayName("Attribute.Office.Client_14")]
  [CustomDescription("Attribute.Office.Client_14")]
  [CustomCategory("Attribute.Office.Client_13")]
  [CanBeNull]
  public TemplatePropertyClass ParallelControlResolutionTemplate { get; set; }

  [CustomDisplayName("Attribute.Office.Client_12")]
  [CustomDescription("Attribute.Office.Client_12")]
  [CustomCategory("Attribute.Office.Client_11")]
  [CanBeNull]
  public TemplatePropertyClass SendAddresseeTemplate { get; set; }

  [CustomDisplayName("Attribute.Office.Client_7")]
  [CustomDescription("Attribute.Office.Client_7")]
  [CustomCategory("Attribute.Office.Client_6")]
  [CanBeNull]
  public string AutoSendEmail { get; set; } = string.Empty;

  [CustomDisplayName("Attribute.Office.Client_8")]
  [CustomDescription("Attribute.Office.Client_8")]
  [CustomCategory("Attribute.Office.Client_6")]
  [CanBeNull]
  public UserPropertyClass AutoSendUser { get; set; }

  [CustomDisplayName("Attribute.Office.Client_9")]
  [CustomDescription("Attribute.Office.Client_10")]
  [CustomCategory("Attribute.Office.Client_11")]
  [TypeConverter(typeof (YesNoConverter))]
  public bool PrivateOffice { get; set; }

  [CustomDisplayName("Attribute.Office.Client_18")]
  [CustomDescription("Attribute.Office.Client_19")]
  [CustomCategory("Attribute.Office.Client_11")]
  [TypeConverter(typeof (YesNoConverter))]
  public bool FilterResolutions { get; set; } = true;

  [CustomDisplayName("Attribute.Office.Client_21")]
  [CustomDescription("Attribute.Office.Client_20")]
  [CustomCategory("Attribute.Office.Client_11")]
  [TypeConverter(typeof (YesNoConverter))]
  public bool IncomingPrivateFolderEnable { get; set; } = true;

  [Description("Aтрибут создаваемого канцелярского документа, в значение которого записывается тема регистрируемого письма")]
  [DisplayName("Заголовок регистрируемого письма")]
  [CustomCategory("Attribute.Office.Client_11")]
  [TypeConverter(typeof (AttributePropertyClass))]
  [Editor(typeof (AttributeEditor), typeof (UITypeEditor))]
  [DefaultValue(null)]
  public AttributePropertyClass CaptionAttributeForEmailMessages { get; set; }

  [Description("Добавлять количество при добавлении ссылки на часть составного материала")]
  [DisplayName("Добавлять количество при добавлении ссылки на часть составного материала")]
  [Category("Плагины MS Word/Excel")]
  [TypeConverter(typeof (YesNoConverter))]
  [DefaultValue(true)]
  public bool AddCountWhileAddingReferenceOnCompositeMaterialParts { get; set; } = true;

  [Browsable(false)]
  public event EventHandler Changed;

  [Browsable(false)]
  public PropertyPageType Type => PropertyPageType.Object;

  [NotNull]
  [Browsable(false)]
  public object Control => (object) this;

  [Browsable(false)]
  [NotNull]
  public string PageName => Localization.GetString("Office.Client_1");

  [NotNull]
  [Browsable(false)]
  public string HeaderText => this.PageName;

  private void Save([NotNull] IUserSession session)
  {
    IOfficeGeneralSettingsService customService = session.GetCustomService<IOfficeGeneralSettingsService>();
    Guid sessionGuid = session.SessionGUID;
    int incomingDocResetType = (int) this.IncomingDocResetType;
    int outgoingDocResetType = (int) this.OutgoingDocResetType;
    int internalDocResetType = (int) this.InternalDocResetType;
    TemplatePropertyClass autoSendTemplate = this.AutoSendTemplate;
    long objectId1 = autoSendTemplate != null ? autoSendTemplate.ObjectID : 0L;
    string autoSendEmail = this.AutoSendEmail;
    UserPropertyClass autoSendUser = this.AutoSendUser;
    long objectId2 = autoSendUser != null ? autoSendUser.ObjectID : 0L;
    int num1 = this.PrivateOffice ? 1 : 0;
    int num2 = this.FilterResolutions ? 1 : 0;
    TemplatePropertyClass addresseeTemplate = this.SendAddresseeTemplate;
    long objectId3 = addresseeTemplate != null ? addresseeTemplate.ObjectID : 0L;
    TemplatePropertyClass resolutionTemplate1 = this.ConsistentControlResolutionTemplate;
    long objectId4 = resolutionTemplate1 != null ? resolutionTemplate1.ObjectID : 0L;
    TemplatePropertyClass resolutionTemplate2 = this.ConsistentNonControlResolutionTemplate;
    long objectId5 = resolutionTemplate2 != null ? resolutionTemplate2.ObjectID : 0L;
    TemplatePropertyClass resolutionTemplate3 = this.ParallelControlResolutionTemplate;
    long objectId6 = resolutionTemplate3 != null ? resolutionTemplate3.ObjectID : 0L;
    TemplatePropertyClass resolutionTemplate4 = this.ParallelNonControlResolutionTemplate;
    long objectId7 = resolutionTemplate4 != null ? resolutionTemplate4.ObjectID : 0L;
    int num3 = this.IncomingPrivateFolderEnable ? 1 : 0;
    AttributePropertyClass forEmailMessages = this.CaptionAttributeForEmailMessages;
    int attribute = forEmailMessages != null ? forEmailMessages.Attribute : 0;
    OfficeGeneralSettings settings = new OfficeGeneralSettings((CountResetTypes) incomingDocResetType, (CountResetTypes) outgoingDocResetType, (CountResetTypes) internalDocResetType, objectId1, autoSendEmail, objectId2, num1 != 0, num2 != 0, objectId3, objectId4, objectId5, objectId6, objectId7, num3 != 0, attribute);
    customService.Save(sessionGuid, settings);
    if (!(ServicesManager.GetService(typeof (IDBConfigurations)) is IDBConfigurations service))
      return;
    service.WriteBool("MSOfficeAddins", "Core", "AddCount", this.AddCountWhileAddingReferenceOnCompositeMaterialParts, 0L);
  }

  [NotNull]
  [Browsable(false)]
  public string HelpTopicID => "2497";

  [Browsable(false)]
  public PropertySort Sort => PropertySort.Categorized;

  public List<string> GetOptionNames()
  {
    return this.Control == null ? new List<string>() : IPropertyPageHelper.GetOptionNames(this.Control);
  }

  public void Apply()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.Save(sessionKeeper.Session);
  }

  public void Cancel()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.ReLoad(sessionKeeper.Session);
  }
}

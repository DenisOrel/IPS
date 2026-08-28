// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.OwnChecks
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using System;

#nullable disable
namespace Intermech.Portal.Server;

internal static class OwnChecks
{
  public static bool DeleteCheckCreator(SiteInfo info, IDBObject obj, bool throwException)
  {
    return OwnChecks.CheckMethod(obj, PortalConsts.attributeFirstPublishSite, (OwnChecks.UnsafePortalMethodHandler) (checkAttribute =>
    {
      if (!(checkAttribute.AsString != info.Code.ToString()))
        return true;
      if (throwException)
        throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_57"), (object) obj.NameInMessages, (object) info.Caption));
      return false;
    }));
  }

  public static bool DeleteCheckOwner(
    SiteInfo info,
    IDBObject obj,
    bool rootObject,
    bool throwException)
  {
    return OwnChecks.CheckMethod(obj, PortalConsts.attributeOwner, (OwnChecks.UnsafePortalMethodHandler) (attrOwner =>
    {
      if (rootObject)
      {
        if (attrOwner.AsString != info.Code.ToString())
        {
          if (throwException)
            throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_58"), (object) obj.NameInMessages, (object) info.Caption));
          return false;
        }
        if (obj.GetAttributeByGuid(PortalConsts.attributePublishInComposition).AsBoolean)
        {
          if (throwException)
            throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_59"), (object) obj.NameInMessages));
          return false;
        }
      }
      else if (attrOwner.AsString != string.Empty && attrOwner.AsString != info.Code.ToString())
      {
        if (throwException)
          throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_58"), (object) obj.NameInMessages, (object) info.Caption));
        return false;
      }
      return true;
    }));
  }

  public static bool DeleteCheckCompositionOwner(SiteInfo info, IDBObject obj, bool throwException)
  {
    return OwnChecks.CheckMethod(obj, PortalConsts.attributeCompositionOwner, (OwnChecks.UnsafePortalMethodHandler) (attrCompositionOwner =>
    {
      if (attrCompositionOwner == null || string.IsNullOrEmpty(attrCompositionOwner.AsString) || !(attrCompositionOwner.AsString != info.Code.ToString()))
        return true;
      if (throwException)
        throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_71"), (object) obj.NameInMessages, (object) info.Caption));
      return false;
    }));
  }

  public static bool CheckPossibilityOwn(
    IUserSession session,
    SiteInfo info,
    IDBObject obj,
    bool throwException)
  {
    return OwnChecks.CheckMethod(obj, PortalConsts.attributeParentSites, (OwnChecks.UnsafePortalMethodHandler) (attrParentSites =>
    {
      if (!(attrParentSites.AsString != string.Empty) || attrParentSites.AsString.IndexOf(info.Code) >= 0)
        return true;
      if (throwException)
        throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_14"), (object) obj.NameInMessages));
      return false;
    })) && OwnChecks.CheckMethod(obj, PortalConsts.attributeOwner, (OwnChecks.UnsafePortalMethodHandler) (attrOwner =>
    {
      if (!(attrOwner.AsString != string.Empty) || (int) attrOwner.AsString[0] == (int) info.Code)
        return true;
      SiteInfo site = ((ISitesCacheService) session.GetCustomService(typeof (ISitesCacheService))).GetSite(attrOwner.AsString[0]);
      string str = site != null ? site.Caption : attrOwner.AsString;
      if (throwException)
        throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_15"), (object) obj.NameInMessages, (object) str));
      return false;
    })) && OwnChecks.CheckMethod(obj, PortalConsts.attributeCompositionOwner, (OwnChecks.UnsafePortalMethodHandler) (attrCompOwner =>
    {
      if (!(attrCompOwner.AsString != string.Empty) || (int) attrCompOwner.AsString[0] == (int) info.Code)
        return true;
      SiteInfo site = ((ISitesCacheService) session.GetCustomService(typeof (ISitesCacheService))).GetSite(attrCompOwner.AsString[0]);
      string str = site != null ? site.Caption : attrCompOwner.AsString;
      if (throwException)
        throw new Exception($"Владение составом {obj.NameInMessages} принадлежит узлу {str}");
      return false;
    }));
  }

  private static bool CheckMethod(
    IDBObject obj,
    Guid attributeGuid,
    OwnChecks.UnsafePortalMethodHandler method)
  {
    if (method == null)
      throw new ArgumentNullException();
    return method(obj.GetAttributeByGuid(attributeGuid));
  }

  private delegate bool UnsafePortalMethodHandler(IDBAttribute checkAttribute);
}

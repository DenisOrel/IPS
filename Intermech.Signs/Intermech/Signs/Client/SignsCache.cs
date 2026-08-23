// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignsCache
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.DataFormats;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Workflow;
using Intermech.Kernel.Search;
using Intermech.Signs.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

public class SignsCache
{
  public static Dictionary<string, string> PossibleGraphs = new Dictionary<string, string>();
  public static SignsCard UserSignsCard = (SignsCard) null;
  public static Hashtable SignsViewColumns = new Hashtable();
  private static SortedList SignsCardCache = new SortedList();
  internal static int appId = 353;
  internal static byte[][] b = new byte[32 /*0x20*/][]
  {
    new byte[16 /*0x10*/]
    {
      (byte) 200,
      (byte) 35,
      (byte) 11,
      (byte) 165,
      (byte) 132,
      (byte) 78,
      (byte) 14,
      (byte) 81,
      (byte) 181,
      (byte) 222,
      (byte) 229,
      (byte) 190,
      (byte) 218,
      (byte) 28,
      (byte) 41,
      (byte) 22
    },
    new byte[16 /*0x10*/]
    {
      (byte) 124,
      (byte) 215,
      (byte) 38,
      (byte) 119,
      (byte) 93,
      (byte) 223,
      (byte) 65,
      (byte) 159,
      (byte) 6,
      (byte) 221,
      (byte) 186,
      (byte) 91,
      (byte) 156,
      (byte) 86,
      (byte) 161,
      (byte) 32 /*0x20*/
    },
    new byte[16 /*0x10*/]
    {
      (byte) 82,
      (byte) 176 /*0xB0*/,
      (byte) 182,
      (byte) 220,
      (byte) 253,
      (byte) 175,
      (byte) 77,
      (byte) 171,
      (byte) 17,
      (byte) 110,
      (byte) 154,
      (byte) 135,
      (byte) 30,
      (byte) 249,
      (byte) 59,
      (byte) 61
    },
    new byte[16 /*0x10*/]
    {
      (byte) 152,
      (byte) 88,
      (byte) 42,
      (byte) 29,
      (byte) 167,
      (byte) 114,
      (byte) 130,
      (byte) 207,
      (byte) 122,
      (byte) 128 /*0x80*/,
      (byte) 149,
      (byte) 244,
      (byte) 100,
      (byte) 146,
      (byte) 186,
      (byte) 248
    },
    new byte[16 /*0x10*/]
    {
      (byte) 209,
      (byte) 227,
      (byte) 120,
      (byte) 10,
      (byte) 211,
      (byte) 133,
      (byte) 50,
      (byte) 177,
      (byte) 89,
      (byte) 35,
      (byte) 180,
      (byte) 173,
      (byte) 172,
      (byte) 239,
      (byte) 60,
      (byte) 1
    },
    new byte[16 /*0x10*/]
    {
      (byte) 134,
      (byte) 138,
      (byte) 205,
      (byte) 20,
      (byte) 128 /*0x80*/,
      (byte) 171,
      (byte) 250,
      (byte) 19,
      (byte) 232,
      (byte) 141,
      (byte) 205,
      (byte) 84,
      (byte) 17,
      (byte) 78,
      (byte) 208 /*0xD0*/,
      (byte) 27
    },
    new byte[16 /*0x10*/]
    {
      (byte) 214,
      (byte) 214,
      (byte) 50,
      (byte) 88,
      (byte) 110,
      (byte) 78,
      (byte) 231,
      (byte) 25,
      (byte) 214,
      (byte) 96 /*0x60*/,
      (byte) 148,
      (byte) 22,
      (byte) 180,
      (byte) 135,
      (byte) 50,
      (byte) 187
    },
    new byte[16 /*0x10*/]
    {
      (byte) 126,
      (byte) 121,
      (byte) 103,
      (byte) 71,
      (byte) 40,
      (byte) 157,
      (byte) 190,
      (byte) 129,
      byte.MaxValue,
      (byte) 77,
      (byte) 14,
      (byte) 20,
      (byte) 170,
      (byte) 187,
      (byte) 70,
      (byte) 47
    },
    new byte[16 /*0x10*/]
    {
      (byte) 163,
      (byte) 168,
      (byte) 237,
      (byte) 247,
      (byte) 164,
      (byte) 31 /*0x1F*/,
      (byte) 66,
      (byte) 205,
      (byte) 125,
      (byte) 4,
      (byte) 131,
      (byte) 217,
      (byte) 250,
      (byte) 160 /*0xA0*/,
      (byte) 254,
      (byte) 33
    },
    new byte[16 /*0x10*/]
    {
      (byte) 36,
      (byte) 195,
      (byte) 203,
      (byte) 230,
      (byte) 228,
      (byte) 26,
      (byte) 209,
      (byte) 32 /*0x20*/,
      (byte) 71,
      (byte) 123,
      (byte) 35,
      (byte) 137,
      (byte) 62,
      (byte) 0,
      (byte) 127 /*0x7F*/,
      (byte) 210
    },
    new byte[16 /*0x10*/]
    {
      (byte) 49,
      (byte) 152,
      (byte) 176 /*0xB0*/,
      (byte) 72,
      (byte) 199,
      (byte) 227,
      (byte) 206,
      (byte) 131,
      (byte) 252,
      (byte) 95,
      (byte) 52,
      (byte) 115,
      (byte) 240 /*0xF0*/,
      (byte) 28,
      (byte) 253,
      (byte) 209
    },
    new byte[16 /*0x10*/]
    {
      (byte) 243,
      (byte) 105,
      (byte) 169,
      (byte) 97,
      (byte) 131,
      (byte) 61,
      (byte) 173,
      (byte) 5,
      (byte) 18,
      (byte) 196,
      (byte) 214,
      (byte) 19,
      (byte) 92,
      (byte) 78,
      (byte) 77,
      (byte) 247
    },
    new byte[16 /*0x10*/]
    {
      (byte) 241,
      (byte) 249,
      (byte) 10,
      (byte) 4,
      (byte) 2,
      (byte) 69,
      (byte) 93,
      (byte) 183,
      (byte) 148,
      (byte) 100,
      (byte) 219,
      (byte) 106,
      (byte) 203,
      (byte) 13,
      (byte) 30,
      (byte) 53
    },
    new byte[16 /*0x10*/]
    {
      (byte) 178,
      (byte) 112 /*0x70*/,
      (byte) 206,
      (byte) 93,
      (byte) 182,
      (byte) 222,
      (byte) 10,
      (byte) 201,
      (byte) 238,
      (byte) 73,
      (byte) 65,
      (byte) 184,
      (byte) 36,
      (byte) 147,
      (byte) 165,
      (byte) 22
    },
    new byte[16 /*0x10*/]
    {
      (byte) 97,
      (byte) 115,
      (byte) 228,
      (byte) 28,
      (byte) 214,
      (byte) 233,
      (byte) 243,
      (byte) 191,
      (byte) 218,
      (byte) 106,
      (byte) 250,
      (byte) 69,
      (byte) 235,
      (byte) 206,
      (byte) 221,
      (byte) 96 /*0x60*/
    },
    new byte[16 /*0x10*/]
    {
      (byte) 116,
      (byte) 121,
      (byte) 115,
      (byte) 194,
      (byte) 78,
      (byte) 30,
      (byte) 109,
      (byte) 164,
      (byte) 47,
      (byte) 32 /*0x20*/,
      (byte) 48 /*0x30*/,
      (byte) 123,
      (byte) 191,
      (byte) 145,
      (byte) 42,
      (byte) 95
    },
    new byte[16 /*0x10*/]
    {
      (byte) 7,
      (byte) 177,
      (byte) 232,
      (byte) 0,
      (byte) 150,
      (byte) 249,
      (byte) 30,
      (byte) 236,
      (byte) 58,
      (byte) 249,
      (byte) 179,
      (byte) 99,
      (byte) 8,
      (byte) 99,
      (byte) 183,
      (byte) 238
    },
    new byte[16 /*0x10*/]
    {
      (byte) 171,
      (byte) 165,
      (byte) 41,
      (byte) 101,
      (byte) 115,
      (byte) 1,
      (byte) 14,
      (byte) 227,
      (byte) 72,
      (byte) 195,
      (byte) 181,
      (byte) 193,
      (byte) 139,
      (byte) 109,
      (byte) 32 /*0x20*/,
      (byte) 135
    },
    new byte[16 /*0x10*/]
    {
      (byte) 64 /*0x40*/,
      (byte) 254,
      (byte) 18,
      (byte) 125,
      (byte) 55,
      (byte) 224 /*0xE0*/,
      (byte) 251,
      (byte) 7,
      (byte) 5,
      (byte) 74,
      (byte) 42,
      (byte) 151,
      (byte) 74,
      (byte) 24,
      (byte) 203,
      (byte) 197
    },
    new byte[16 /*0x10*/]
    {
      (byte) 227,
      (byte) 40,
      (byte) 12,
      (byte) 61,
      (byte) 17,
      (byte) 62,
      (byte) 147,
      (byte) 123,
      (byte) 241,
      (byte) 227,
      (byte) 216,
      (byte) 226,
      (byte) 201,
      (byte) 93,
      (byte) 58,
      (byte) 0
    },
    new byte[16 /*0x10*/]
    {
      (byte) 135,
      (byte) 122,
      (byte) 98,
      (byte) 25,
      (byte) 36,
      (byte) 29,
      (byte) 159,
      (byte) 103,
      (byte) 84,
      (byte) 247,
      (byte) 225,
      (byte) 221,
      (byte) 53,
      (byte) 144 /*0x90*/,
      (byte) 182,
      (byte) 118
    },
    new byte[16 /*0x10*/]
    {
      (byte) 119,
      (byte) 239,
      (byte) 109,
      (byte) 227,
      (byte) 240 /*0xF0*/,
      (byte) 169,
      (byte) 25,
      (byte) 137,
      (byte) 139,
      (byte) 45,
      (byte) 125,
      (byte) 169,
      (byte) 73,
      (byte) 202,
      (byte) 155,
      (byte) 196
    },
    new byte[16 /*0x10*/]
    {
      (byte) 178,
      (byte) 12,
      (byte) 4,
      (byte) 237,
      (byte) 188,
      (byte) 95,
      (byte) 41,
      (byte) 38,
      (byte) 181,
      (byte) 225,
      (byte) 250,
      (byte) 75,
      (byte) 207,
      (byte) 135,
      (byte) 162,
      (byte) 33
    },
    new byte[16 /*0x10*/]
    {
      (byte) 88,
      (byte) 61,
      (byte) 210,
      (byte) 174,
      (byte) 191,
      (byte) 176 /*0xB0*/,
      (byte) 228,
      (byte) 200,
      (byte) 254,
      (byte) 122,
      (byte) 61,
      (byte) 207,
      (byte) 191,
      (byte) 229,
      (byte) 212,
      (byte) 150
    },
    new byte[16 /*0x10*/]
    {
      (byte) 45,
      (byte) 238,
      (byte) 219,
      (byte) 142,
      (byte) 1,
      (byte) 158,
      (byte) 201,
      (byte) 34,
      (byte) 94,
      (byte) 170,
      (byte) 40,
      (byte) 63 /*0x3F*/,
      byte.MaxValue,
      (byte) 30,
      (byte) 209,
      (byte) 117
    },
    new byte[16 /*0x10*/]
    {
      (byte) 153,
      (byte) 194,
      (byte) 176 /*0xB0*/,
      (byte) 187,
      (byte) 22,
      (byte) 221,
      (byte) 118,
      (byte) 78,
      (byte) 193,
      (byte) 14,
      (byte) 223,
      (byte) 53,
      (byte) 76,
      (byte) 120,
      (byte) 55,
      (byte) 252
    },
    new byte[16 /*0x10*/]
    {
      (byte) 148,
      (byte) 180,
      (byte) 229,
      (byte) 61,
      (byte) 19,
      (byte) 222,
      (byte) 8,
      (byte) 39,
      (byte) 81,
      (byte) 60,
      (byte) 100,
      (byte) 67,
      (byte) 34,
      (byte) 84,
      (byte) 152,
      (byte) 205
    },
    new byte[16 /*0x10*/]
    {
      (byte) 158,
      (byte) 161,
      (byte) 141,
      (byte) 195,
      (byte) 22,
      (byte) 119,
      (byte) 243,
      (byte) 219,
      (byte) 74,
      (byte) 115,
      (byte) 91,
      (byte) 84,
      (byte) 252,
      (byte) 92,
      (byte) 110,
      (byte) 67
    },
    new byte[16 /*0x10*/]
    {
      (byte) 85,
      (byte) 191,
      (byte) 175,
      (byte) 71,
      (byte) 180,
      (byte) 40,
      (byte) 117,
      (byte) 20,
      (byte) 66,
      (byte) 226,
      (byte) 142,
      (byte) 18,
      (byte) 55,
      (byte) 233,
      (byte) 39,
      (byte) 151
    },
    new byte[16 /*0x10*/]
    {
      (byte) 168,
      (byte) 139,
      (byte) 35,
      (byte) 247,
      (byte) 2,
      (byte) 22,
      (byte) 187,
      (byte) 232,
      (byte) 38,
      (byte) 0,
      (byte) 178,
      (byte) 28,
      (byte) 114,
      (byte) 5,
      (byte) 109,
      (byte) 165
    },
    new byte[16 /*0x10*/]
    {
      (byte) 15,
      (byte) 133,
      (byte) 183,
      (byte) 27,
      (byte) 161,
      (byte) 100,
      (byte) 13,
      (byte) 79,
      (byte) 229,
      (byte) 180,
      (byte) 61,
      (byte) 196,
      (byte) 58,
      (byte) 134,
      byte.MaxValue,
      (byte) 10
    },
    new byte[16 /*0x10*/]
    {
      (byte) 145,
      (byte) 17,
      (byte) 46,
      (byte) 21,
      (byte) 156,
      (byte) 51,
      (byte) 69,
      (byte) 116,
      (byte) 218,
      (byte) 91,
      (byte) 193,
      (byte) 51,
      (byte) 127 /*0x7F*/,
      (byte) 26,
      (byte) 103,
      (byte) 245
    }
  };

  public static Dictionary<string, string> ParsePossibleGraphs(DataTable data)
  {
    Dictionary<string, string> possibleGraphs = new Dictionary<string, string>();
    if (data != null)
    {
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        string key = row["F_STRING_VALUE"].ToString();
        string str = row["F_DESCRIPTION"].ToString();
        if (str.Equals(string.Empty))
          str = key;
        possibleGraphs[key] = str;
      }
    }
    return possibleGraphs;
  }

  public static SignsCard LoadUserGraphInfo(IUserSession session, long userID)
  {
    return SignsCache.LoadUserGraphInfo(session, userID, true);
  }

  public static SignsCard LoadUserGraphInfo(IUserSession session, long userID, bool fromCache)
  {
    if (SignsCache.SignsCardCache.ContainsKey((object) userID) & fromCache)
      return SignsCache.SignsCardCache[(object) userID] as SignsCard;
    SignsCard signsCard = new SignsCard();
    IDBObject dbObject = session.GetObject(userID, false);
    if (dbObject != null)
    {
      foreach (object obj in dbObject.GetAttributeByID(SignsHolder.RankAttrTypeID).Values)
      {
        if (obj != null && obj.GetType().Equals(typeof (long)))
        {
          long int64 = Convert.ToInt64(obj);
          if (session.GetCustomService(typeof (ISignsService)) is ISignsService customService)
          {
            byte[] rankSignsSetup = customService.GetRankSignsSetup(int64, session.SessionGUID);
            if (rankSignsSetup != null)
            {
              Graphs4Type info = new Graphs4Type((Stream) new MemoryStream(rankSignsSetup), SignsCache.PossibleGraphs);
              signsCard.Add(int64, info);
            }
          }
        }
      }
    }
    SignsCache.SignsCardCache[(object) userID] = (object) signsCard;
    return signsCard;
  }

  public static long GetUserIdByUserName(string userName)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable dataTable = sessionKeeper.Session.ObjectsSelect(sessionKeeper.Session.IdentHelper.UsersTypeID, new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(sessionKeeper.Session.IdentHelper.LoginNameID, RelationalOperators.Equal, (object) userName, LogicalOperators.NONE, 0, false)
      }, new object[1]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID
      })
      {
        RecordCount = 1
      });
      if (dataTable != null)
      {
        if (dataTable.Rows.Count.Equals(1))
        {
          long int64 = Convert.ToInt64(dataTable.Rows[0][0]);
          if (sessionKeeper.Session.GetObject(int64, false) != null)
            return int64;
        }
      }
    }
    throw new InvalidLoginInfoException();
  }

  public static SignsCard LoadUserGraphInfo(SignCollection infoForSigning)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(infoForSigning.UserID, false);
      if (dbObject != null)
      {
        if ((dbObject.GetAttributeByID(sessionKeeper.Session.IdentHelper.PasswordID) as IDBEncryptedAttribute).ValidateCurrent(infoForSigning.Password))
          return SignsCache.LoadUserGraphInfo(sessionKeeper.Session, infoForSigning.UserID);
      }
    }
    throw new InvalidLoginInfoException();
  }

  public static bool Sign(
    IDBTypedObjectID typedObjectID,
    SignsCard card,
    out UserRankInformation[] rankInfo,
    out string resolutionString)
  {
    return SignsCache.Sign(new List<IDBTypedObjectID>(1)
    {
      typedObjectID
    }, card, out rankInfo, out resolutionString);
  }

  public static bool Sign(
    List<IDBTypedObjectID> typedObjectIDs,
    SignsCard card,
    out UserRankInformation[] rankInfo,
    out string resolutionString)
  {
    rankInfo = (UserRankInformation[]) null;
    resolutionString = string.Empty;
    List<long> rankIDs = new List<long>();
    if (card.IsUserCanSign(typedObjectIDs, out rankIDs))
    {
      List<string> graphs = card.GetGraphs(rankIDs[0], typedObjectIDs);
      if (rankIDs.Count == 1 && graphs.Count == 1)
      {
        UserRankInformation userRankInformation = new UserRankInformation(rankIDs[0]);
        userRankInformation.Graphs.AddRange((IEnumerable<string>) graphs);
        rankInfo = new UserRankInformation[1]
        {
          userRankInformation
        };
      }
      else
      {
        GraphsSet graphsToSign = SignsCache.GetGraphsToSign(typedObjectIDs);
        using (SelectRank selectRank = new SelectRank(rankIDs, typedObjectIDs, card, graphsToSign))
        {
          if (!selectRank.ShowDialog().Equals((object) DialogResult.OK) || selectRank.SelectedItems.Count.Equals(0))
            return false;
          rankInfo = selectRank.SelectedItems.ToArray(typeof (UserRankInformation)) as UserRankInformation[];
        }
      }
      if (typedObjectIDs.Count == 1 && !SignsHolder.ConfirmSingleSigning)
        return true;
      List<string> stringList1 = new List<string>();
      int num = 1;
      foreach (UserRankInformation userRankInformation in rankInfo)
      {
        List<string> stringList2 = new List<string>();
        foreach (string graph in userRankInformation.Graphs)
        {
          if (SignsCache.PossibleGraphs.ContainsKey(graph))
            stringList2.Add(SignsCache.PossibleGraphs[graph]);
        }
        if (stringList2.Count.Equals(0))
          return false;
        string str1 = $"'{string.Join("', '", stringList2.ToArray())}'";
        string str2 = typedObjectIDs.Count != 1 ? (!userRankInformation.Graphs.Count.Equals(1) ? string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("Signs_107"), (object) str1, (object) userRankInformation.RankCaption, rankInfo.Length > 1 ? (object) (Convert.ToString(num++) + ". ") : (object) string.Empty) : string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("Signs_106"), (object) str1, (object) userRankInformation.RankCaption, rankInfo.Length > 1 ? (object) (Convert.ToString(num++) + ". ") : (object) string.Empty)) : (!userRankInformation.Graphs.Count.Equals(1) ? string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("Signs_105"), (object) str1, (object) userRankInformation.RankCaption, rankInfo.Length > 1 ? (object) (Convert.ToString(num++) + ". ") : (object) string.Empty) : string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("Signs_104"), (object) str1, (object) userRankInformation.RankCaption, rankInfo.Length > 1 ? (object) (Convert.ToString(num++) + ". ") : (object) string.Empty));
        stringList1.Add(str2);
      }
      stringList1.Add("\r\n");
      string str3;
      if (typedObjectIDs.Count == 1)
      {
        stringList1.Add(Intermech.Localization.LocalizationHolder.rm.GetString("Signs_120"));
        str3 = Intermech.Localization.LocalizationHolder.rm.GetString("Signs_121");
      }
      else
      {
        stringList1.Add(Intermech.Localization.LocalizationHolder.rm.GetString("Signs_118"));
        str3 = Intermech.Localization.LocalizationHolder.rm.GetString("Signs_119");
      }
      for (int index = 0; index < typedObjectIDs.Count; ++index)
      {
        string str4 = typedObjectIDs[index].Caption;
        if (str4 == string.Empty)
          str4 = Intermech.Localization.LocalizationHolder.rm.GetString("Signs_126") + (object) typedObjectIDs[index].ObjectID;
        string str5 = str4 + (typedObjectIDs[index].Version > 0L ? $"[{(object) typedObjectIDs[index].Version}]" : string.Empty);
        string str6 = $"{MetaDataHelper.GetObjectName(typedObjectIDs[index].ObjectType)} {str5}";
        string str7 = index != 0 ? "\t  " + str6 : str3 + str6;
        if (index == typedObjectIDs.Count - 1)
          str7 += "\r\n";
        stringList1.Add(str7);
      }
      using (SignQuestion signQuestion = new SignQuestion())
      {
        signQuestion.MessageText = stringList1;
        if (signQuestion.ShowDialog() == DialogResult.OK)
        {
          resolutionString = signQuestion.Resolution;
          return true;
        }
      }
      return false;
    }
    if (typedObjectIDs.Count == 1)
    {
      int num1 = (int) MessageBox.Show(Intermech.Localization.LocalizationHolder.rm.GetString("Signs_98"), Intermech.Localization.LocalizationHolder.rm.GetString("Signs_102"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }
    else
    {
      int num2 = (int) MessageBox.Show(Intermech.Localization.LocalizationHolder.rm.GetString("Signs_99"), Intermech.Localization.LocalizationHolder.rm.GetString("Signs_102"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }
    return false;
  }

  public static void ClearCache(IUserSession session)
  {
    SignsCache.SignsCardCache.Clear();
    (session.GetCustomService(typeof (ISignsService)) as ISignsService).CleanCache();
  }

  public static GraphsSet GetGraphsToSign(List<IDBTypedObjectID> typedObjectIDs)
  {
    GraphsSet graphsToSign = (GraphsSet) null;
    HashSet<long> hashSet1 = new HashSet<long>();
    HashSet<int> hashSet2 = new HashSet<int>();
    foreach (IDBTypedObjectID typedObjectId in typedObjectIDs)
    {
      hashSet1.Add(typedObjectId.ObjectID);
      hashSet2.Add(typedObjectId.ObjectType);
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetCustomService(typeof (IRouterService)) is IRouterService customService)
        graphsToSign = customService.GetGraphsToSign(sessionKeeper.Session.SessionGUID, hashSet1.ToArray<long>(), hashSet2.ToArray<int>());
    }
    return graphsToSign;
  }
}

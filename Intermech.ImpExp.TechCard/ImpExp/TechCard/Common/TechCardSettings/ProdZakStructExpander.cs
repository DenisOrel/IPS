// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechCardSettings.ProdZakStructExpander
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common.TechCardSettings;

internal class ProdZakStructExpander : ObjStructExpander
{
  public override List<ArtInfoLight> CollectObjsFromObjsStructures(List<ArtInfoLight> headObjsInfo)
  {
    this.CleanArtStruTempSelection(-890880);
    this.PrepareArtVersToSelect(headObjsInfo, -890880);
    string str1 = "pSELECT_ID";
    string str2 = $"select distinct z.PART_AID, case when z.PART_VER < -1 then -2-Z.PART_VER when z.PART_VER = -1 then (select a.ART_VER_ID from ARTICLES a WHERE a.ART_ID = z.PART_AID)else z.PART_VER end as ART_VER_ID from ZPC z where ZAKAZ_ID in ({"select PART_AID from ARTSTRU where SELECT_ID = " + (SearchConnectionsManager.GetConnection().DataBaseType == "IntermechConnection.MsSQL" ? "@" + str1 : ":" + str1)})";
    using (IDbCommand command = SearchConnectionsManager.GetConnection().CreateCommand())
    {
      command.CommandText = str2;
      IDbDataParameter parameter = command.CreateParameter();
      parameter.ParameterName = str1;
      parameter.Direction = ParameterDirection.Input;
      parameter.Value = (object) -890880;
      command.Parameters.Add((object) parameter);
      List<ArtInfoLight> artInfoLightList = new List<ArtInfoLight>();
      try
      {
        using (IDataReader dataReader = command.ExecuteReader())
        {
          while (dataReader.Read())
          {
            ArtInfoLight artInfoLight = new ArtInfoLight(dataReader.GetInt32(0), dataReader.GetInt32(1));
            artInfoLightList.Add(artInfoLight);
          }
          return artInfoLightList;
        }
      }
      finally
      {
        this.CleanArtStruTempSelection(-890880);
      }
    }
  }

  private void PrepareArtVersToSelect(List<ArtInfoLight> headObjsInfo, int selectId)
  {
    string str1 = "pSELECT_ID";
    string str2 = "pZAK_ID";
    string str3 = $"insert into ARTSTRU (SELECT_ID, PART_AID)values ({(SearchConnectionsManager.GetConnection().DataBaseType == "IntermechConnection.MsSQL" ? "@" + str1 : ":" + str1)}, {(SearchConnectionsManager.GetConnection().DataBaseType == "IntermechConnection.MsSQL" ? "@" + str2 : ":" + str2)})";
    using (IDbCommand cmd = SearchConnectionsManager.GetConnection().CreateCommand())
    {
      cmd.CommandText = str3;
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = str1;
      parameter.Direction = ParameterDirection.Input;
      parameter.DbType = DbType.Int32;
      parameter.Value = (object) selectId;
      cmd.Parameters.Add((object) parameter);
      IDbDataParameter pZakId = cmd.CreateParameter();
      pZakId.ParameterName = str2;
      pZakId.DbType = DbType.Int32;
      pZakId.Direction = ParameterDirection.Input;
      cmd.Parameters.Add((object) pZakId);
      cmd.Prepare();
      headObjsInfo.ForEach((Action<ArtInfoLight>) (artInfo =>
      {
        pZakId.Value = (object) artInfo.ArtId;
        cmd.ExecuteNonQuery();
      }));
    }
  }

  private void CleanArtStruTempSelection(int selectId)
  {
    string str1 = "pSELECT_ID";
    string empty = string.Empty;
    string str2 = "delete from ARTSTRU where SELECT_ID = " + (SearchConnectionsManager.GetConnection().DataBaseType == "IntermechConnection.MsSQL" ? "@" + str1 : ":" + str1);
    using (IDbCommand command = SearchConnectionsManager.GetConnection().CreateCommand())
    {
      command.CommandText = str2;
      IDbDataParameter parameter = command.CreateParameter();
      parameter.ParameterName = str1;
      parameter.Direction = ParameterDirection.Input;
      parameter.Value = (object) selectId;
      command.Parameters.Add((object) parameter);
      command.ExecuteNonQuery();
    }
  }
}

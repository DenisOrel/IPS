// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.MigrateCatalogFilter
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Imbase;

internal sealed class MigrateCatalogFilter
{
  public List<string> EnableTables;
  public List<int> EnableFields;
  public List<string> EnableCatalogs;

  public bool ReadMigrateCatalogsList(IDbConnection db)
  {
    string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "MigrateCatalogs.txt");
    if (!File.Exists(path))
      return false;
    using (StreamReader streamReader = File.OpenText(path))
    {
      string end = streamReader.ReadToEnd();
      if (string.IsNullOrEmpty(end))
        return false;
      this.EnableCatalogs = new List<string>();
      this.EnableTables = new List<string>();
      this.EnableFields = new List<int>();
      IDbCommand command = db.CreateCommand();
      string str1 = end;
      char[] chArray = new char[1]{ ';' };
      foreach (string str2 in str1.Split(chArray))
      {
        command.CommandText = $"SELECT F_KEY, F_TABLE FROM IM_TABLES WHERE F_DESCR LIKE '{str2}'";
        string empty = string.Empty;
        int catalogKey = 0;
        using (IDataReader dataReader = command.ExecuteReader())
        {
          if (dataReader.Read())
          {
            catalogKey = Convert.ToInt32(dataReader[0]);
            empty = Convert.ToString(dataReader[1]);
          }
          dataReader.Close();
        }
        if (!string.IsNullOrEmpty(empty))
        {
          this.EnableCatalogs.Add(empty);
          string tableField = string.Empty;
          this.ReadTableFields(command, catalogKey, out tableField);
          if (!(tableField == string.Empty))
          {
            command.CommandText = $"SELECT {tableField} FROM {empty}_REC";
            List<string> stringList = new List<string>();
            using (IDataReader dataReader = command.ExecuteReader())
            {
              while (dataReader.Read())
              {
                string str3 = Convert.ToString(dataReader[0]);
                if (!string.IsNullOrEmpty(str3) && !this.EnableTables.Contains(str3))
                {
                  this.EnableTables.Add(str3);
                  stringList.Add(str3);
                }
              }
              dataReader.Close();
            }
            foreach (string str4 in stringList)
            {
              command.CommandText = $"SELECT F_KEY FROM IM_TABLES WHERE F_TABLE LIKE '{str4}'";
              object obj = command.ExecuteScalar();
              if (obj != DBNull.Value)
              {
                command.CommandText = $"SELECT F_KEY FROM IM_FIELDS WHERE F_TABLE_ID = {obj}";
                using (IDataReader dataReader = command.ExecuteReader())
                {
                  while (dataReader.Read())
                    this.EnableFields.Add(Convert.ToInt32(dataReader[0]));
                  dataReader.Close();
                }
              }
            }
          }
        }
      }
    }
    return true;
  }

  private void ReadTableFields(IDbCommand cmd, int catalogKey, out string tableField)
  {
    cmd.CommandText = $"SELECT F_KEY, F_FIELD, F_TYPE FROM IM_FIELDS WHERE F_TABLE_ID = {catalogKey}";
    using (IDataReader dataReader = cmd.ExecuteReader())
    {
      tableField = string.Empty;
      while (dataReader.Read())
      {
        this.EnableFields.Add(Convert.ToInt32(dataReader[0]));
        if (Convert.ToInt32(dataReader[2]) == 1)
          tableField = Convert.ToString(dataReader[1]);
      }
      dataReader.Close();
    }
  }
}

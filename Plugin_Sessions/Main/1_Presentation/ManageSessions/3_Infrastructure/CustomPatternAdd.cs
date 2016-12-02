﻿namespace Minary.Plugin.Main.Session.ManageSessions.Infrastructure
{
  using Minary.Plugin.Main.Session.ManageSessions.DataTypes;
  using MinaryLib;
  using System;
  using System.IO;
  using System.Runtime.Serialization.Formatters.Binary;


  public class CustomPatternAdd
  {

    #region MEMBERS

    private static CustomPatternAdd instance;
    private PluginProperties pluginProperties;

    #endregion


    #region PUBLIC

    /// <summary>
    ///
    /// </summary>
    /// <param name="pluginProperties"></param>
    /// <returns></returns>
    public static CustomPatternAdd GetInstance(PluginProperties pluginProperties)
    {
      return instance ?? (instance = new CustomPatternAdd(pluginProperties));
    }


    /// <summary>
    ///
    /// </summary>
    public void SaveNewAccountPatternRecord(SessionPattern record)
    {
      FileStream fileStream = null;
      BinaryFormatter formatter = new BinaryFormatter();

      try
      {
        formatter = new BinaryFormatter();
        fileStream = new FileStream(record.PatternFileFullPath, FileMode.Create);
        formatter.Serialize(fileStream, record);
      }
      catch (Exception ex)
      {
      }
      finally
      {
        if (fileStream != null)
        {
          fileStream.Close();
        }
      }
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomPatternAdd"/> class.
    ///
    /// </summary>
    /// <param name="pluginProperties"></param>
    private CustomPatternAdd(PluginProperties pluginProperties)
    {
      this.pluginProperties = pluginProperties;
    }

    #endregion

  }
}
﻿namespace Minary.Plugin.Main.HttpRequest.Infrastructure
{
  using Minary.Plugin.Main.HttpRequest.DataTypes;
  using MinaryLib.Plugin;
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.IO;


  public class HttpRequest
  {

    #region MEMBERS

    private static HttpRequest instance;
    private IPlugin plugin;

    #endregion


    #region PUBLIC

    private HttpRequest(IPlugin plugin)
    {
      this.plugin = plugin;
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <returns></returns>
    public static HttpRequest GetInstance(IPlugin plugin)
    {
      if (instance == null)
      {
        instance = new HttpRequest(plugin);
      }

      return (instance);
    }

    #endregion


    #region EVENTS

    /// <summary>
    ///
    /// </summary>
    /// <param name="pWebServerConfig"></param>
    public void OnInit()
    {
      List<string> pluginBasedirectories = new List<string>();

      pluginBasedirectories.Add(Path.Combine(
                                             this.plugin.Config.ApplicationBaseDir,
                                             this.plugin.Config.PluginBaseDir,
                                             this.plugin.Config.PatternSubDir,
                                             Plugin.Main.HttpRequest.DataTypes.General.PATTERN_DIR_REMOTE));

      pluginBasedirectories.Add(Path.Combine(
                                             this.plugin.Config.ApplicationBaseDir,
                                             this.plugin.Config.PluginBaseDir,
                                             this.plugin.Config.PatternSubDir,
                                             Plugin.Main.HttpRequest.DataTypes.General.PATTERN_DIR_LOCAL));

      pluginBasedirectories.Add(Path.Combine(
                                             this.plugin.Config.ApplicationBaseDir,
                                             this.plugin.Config.PluginBaseDir,
                                             this.plugin.Config.PatternSubDir,
                                             Plugin.Main.HttpRequest.DataTypes.General.PATTERN_DIR_TEMPLATE));

      pluginBasedirectories.ForEach(elem =>
      {
        try
        {
          if (!Directory.Exists(elem))
          {
            Directory.CreateDirectory(elem);
          }
        }
        catch (Exception ex)
        {
          this.plugin.Config.HostApplication.LogMessage("{0} : {1}", this.plugin.Config.PluginName, ex.Message);
        }
      });

      // Clean up template directory
      this.CleanUpTemplateDir();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="pWebServerConfig"></param>
    public void OnReset()
    {
      this.CleanUpTemplateDir();
    }

    #endregion


    #region PRIVATE

    /// <summary>
    ///
    /// </summary>
    private void CleanUpTemplateDir()
    {
      string templateDir = Path.Combine(
                                        this.plugin.Config.ApplicationBaseDir,
                                        this.plugin.Config.PluginBaseDir,
                                        this.plugin.Config.PatternSubDir,
                                        Plugin.Main.HttpRequest.DataTypes.General.PATTERN_DIR_TEMPLATE);

      if (!Directory.Exists(templateDir))
      {
        return;
      }

      string[] patternFiles = Directory.GetFiles(templateDir, Plugin.Main.HttpRequest.DataTypes.General.PATTERN_FILE_PATTERN);

      foreach (string tmpPatternFile in patternFiles)
      {
        try
        {
          File.Delete(tmpPatternFile);
        }
        catch (Exception ex)
        {
          this.plugin.Config.HostApplication.LogMessage("{0} : {1}", this.plugin.Config.PluginName, ex.Message);
        }
      }
    }

    #endregion

  }
}
﻿namespace Minary.Plugin.Main
{
  using Minary.Plugin.Main.InjectFile.DataTypes;
  using System;
  using System.IO;
  using System.Linq;


  public partial class Plugin_HttpInjectFile
  {

    #region GUI RECORDS METHODS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestedResource"></param>
    /// <param name="replacementResource"></param>
    private delegate void AddRecordDelegate(string requestedResource, string replacementResource);
    private void AddRecord(string requestedResource, string replacementResource)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new AddRecordDelegate(this.AddRecord), new object[] { requestedResource, replacementResource });
        return;
      }

      RequestURL requestUrl = this.ParseRequestedURLRegex(requestedResource);

      // Verify whether replacement file resource is valid
      if (!File.Exists(replacementResource))
      {
        throw new Exception("The injection file does not exist.");
      }

      // Verify if record already exists
      foreach (InjectFileRecord tmpRecord in this.injectFileRecords)
      {
        if (tmpRecord.RequestedHostRegex == requestUrl.HostRegex &&
            tmpRecord.RequestedPathRegex == requestUrl.PathRegex)
        {
          throw new Exception("A record with this host name already exists.");
        }
      }

      lock (this)
      {
        InjectFileRecord newRecord = new InjectFileRecord(requestUrl.HostRegex, requestUrl.PathRegex, replacementResource);

        this.dgv_InjectionTriggerURLs.SuspendLayout();
        this.injectFileRecords.Insert(0, newRecord);
        this.dgv_InjectionTriggerURLs.ResumeLayout();
      }
    }


    /// <summary>
    ///
    /// </summary>
    private delegate void DeleteSelectedRecordDelegate();
    private void DeleteSelectedRecord()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new DeleteSelectedRecordDelegate(this.DeleteSelectedRecord), new object[] { });
        return;
      }

      var isLastLine = false;
      var firstVisibleRowTopRow = -1;
      var lastRowIndex = -1;
      var selectedIndex = -1;

      lock (this)
      {

        if (this.dgv_InjectionTriggerURLs.CurrentRow != null && 
            this.dgv_InjectionTriggerURLs.CurrentRow == this.dgv_InjectionTriggerURLs.Rows[this.dgv_InjectionTriggerURLs.Rows.Count - 1])
        {
          isLastLine = true;
        }

        firstVisibleRowTopRow = this.dgv_InjectionTriggerURLs.FirstDisplayedScrollingRowIndex;
        lastRowIndex = this.dgv_InjectionTriggerURLs.Rows.Count - 1;

        if (this.dgv_InjectionTriggerURLs.CurrentCell != null)
        {
          selectedIndex = this.dgv_InjectionTriggerURLs.CurrentCell.RowIndex;
        }

        this.dgv_InjectionTriggerURLs.SuspendLayout();
        this.dgv_InjectionTriggerURLs.BeginEdit(true);
        this.dgv_InjectionTriggerURLs.RefreshEdit();

        try
        {
          var currentIndex = this.dgv_InjectionTriggerURLs.CurrentCell.RowIndex;
          this.injectFileRecords.RemoveAt(currentIndex);
        }
        catch (Exception ex)
        {
          this.pluginProperties.HostApplication.LogMessage($"{this.Config.PluginName}: {ex.Message}");
        }

        // Selected cell/row
        try
        {
          if (selectedIndex >= 0)
          {
            this.dgv_InjectionTriggerURLs.CurrentCell = this.dgv_InjectionTriggerURLs.Rows[selectedIndex].Cells[0];
          }
        }
        catch (Exception)
        {
        }

        // Reset position
        try
        {
          if (firstVisibleRowTopRow >= 0)
          {
            this.dgv_InjectionTriggerURLs.FirstDisplayedScrollingRowIndex = firstVisibleRowTopRow;
          }
        }
        catch (Exception)
        {
        }

        this.dgv_InjectionTriggerURLs.ResumeLayout();
      }
    }


    private delegate void RemoveRecordAtDelegate(int index);
    private void RemoveRecordAt(int index)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new RemoveRecordAtDelegate(this.RemoveRecordAt), new object[] { index });
        return;
      }

      lock (this)
      {
        this.dgv_InjectionTriggerURLs.SuspendLayout();

        try
        {
          this.injectFileRecords.RemoveAt(index);
        }
        catch (Exception)
        {
        }

        this.dgv_InjectionTriggerURLs.ResumeLayout();
      }
    }


    /// <summary>
    ///
    /// </summary>
    private delegate void ClearRecordListDelegate();
    private void ClearRecordList()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new ClearRecordListDelegate(this.ClearRecordList), new object[] { });
        return;
      }

      lock (this)
      {
        this.dgv_InjectionTriggerURLs.SuspendLayout();

        try
        {
          this.injectFileRecords.Clear();
        }
        catch (Exception)
        {
        }

        this.dgv_InjectionTriggerURLs.ResumeLayout();
      }
    }


    private RequestURL ParseRequestedURLRegex(string url)
    {
      RequestURL requestedUrl;
      var pathDelimiter = '/';

      if(string.IsNullOrEmpty(url) == true || 
         string.IsNullOrWhiteSpace(url) == true)
      {
        throw new Exception("The URL is invalid");
      }

      url = url.Trim();
      if(url.StartsWith("http://") == true || 
         url.StartsWith("https://") == true)
      {
        throw new Exception("The URL must not contain a scheme definition");
      }

      if(url.Contains(pathDelimiter) == false)
      {
        throw new Exception("The URL must contain a root path slash");
      }

      string[] splitter = url.Split(new char[] { pathDelimiter }, 2);

      if(splitter == null || 
         splitter.Count() != 2)
      {
        throw new Exception("The URL is invalid");
      }

      var urlPath = $"{pathDelimiter}{splitter[1]}";
      requestedUrl = new RequestURL(splitter[0], urlPath);

      return requestedUrl;
    }


    #endregion

  }
}

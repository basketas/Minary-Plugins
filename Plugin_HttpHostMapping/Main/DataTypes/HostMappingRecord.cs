﻿namespace Minary.Plugin.Main.HostMapping.DataTypes
{
  using System;
  using System.ComponentModel;


  [Serializable]
  public class HostMappingRecord : INotifyPropertyChanged
  {

    #region MEMBERS

    private string requestedHost;
    private string mappedHostScheme;
    private string mappedHost;

    [field: NonSerialized]
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public HostMappingRecord()
    {
      this.requestedHost = string.Empty;
      this.mappedHostScheme = string.Empty;
      this.mappedHost = string.Empty;
    }


    public HostMappingRecord(string requestedHost, string mappedHostScheme, string mappedHost)
    {
      this.requestedHost = requestedHost;
      this.mappedHostScheme = mappedHostScheme;
      this.mappedHost = mappedHost;
    }

    #endregion


    #region PROPERTIES

    [Browsable(true)]
    public string RequestedHost
    {
      get
      {
        return this.requestedHost;
      }

      set
      {
        this.requestedHost = value;
        this.NotifyPropertyChanged("RequestedHost");
      }
    }


    [Browsable(true)]
    public string MappedHostScheme
    {
      get
      {
        return this.mappedHostScheme;
      }

      set
      {
        this.mappedHostScheme = value;
        this.NotifyPropertyChanged("MappedHostScheme");
      }
    }


    [Browsable(true)]
    public string MappedHost
    {
      get
      {
        return this.mappedHost;
      }

      set
      {
        this.mappedHost = value;
        this.NotifyPropertyChanged("MappedHost");
      }
    }    

    #endregion


    #region PRIVATE

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    private void NotifyPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged != null)
      {
        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion

  }
}
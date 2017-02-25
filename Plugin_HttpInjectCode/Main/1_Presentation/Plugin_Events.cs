﻿namespace Minary.Plugin.Main
{
  using System;
  using System.IO;
  using System.Windows.Forms;


  public partial class Plugin_HttpInjectCode
  {

    #region EVENTS

    private void BT_Add_Click(object sender, EventArgs e)
    {
      try
      {
        string tag = this.cb_injectPisition.Text;
        string position = this.rb_Before.Checked?"before":"after";
        this.AddRecord(this.tb_RequestedURLRegex.Text, this.tb_InjectioinContentFile.Text, tag, position);
      }
      catch (Exception ex)
      {
        string msg = string.Format("Error occurred while adding inject code record: \r\n\r\n{0}", ex.Message);
        this.pluginProperties.HostApplication.LogMessage("{0}: {1}", this.Config.PluginName, ex.Message);
        MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TSMI_Delete_Click(object sender, EventArgs e)
    {
      try
      {
        this.DeleteSelectedRecord();
      }
      catch (Exception)
      {
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Spoofing_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
      {
        return;
      }

      try
      {
        DataGridView.HitTestInfo hti = this.dgv_InjectionTriggerURLs.HitTest(e.X, e.Y);
        if (hti.RowIndex >= 0)
        {
          this.cms_InjectCode.Show(this.dgv_InjectionTriggerURLs, e.Location);
        }
      }
      catch (Exception ex)
      {
        this.pluginProperties.HostApplication.LogMessage("{0}: {1}", this.Config.PluginName, ex.Message);
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Spoofing_MouseDown(object sender, MouseEventArgs e)
    {
      try
      {
        DataGridView.HitTestInfo hti = this.dgv_InjectionTriggerURLs.HitTest(e.X, e.Y);

        if (hti.RowIndex >= 0)
        {
          this.dgv_InjectionTriggerURLs.ClearSelection();
          this.dgv_InjectionTriggerURLs.Rows[hti.RowIndex].Selected = true;
          this.dgv_InjectionTriggerURLs.CurrentCell = this.dgv_InjectionTriggerURLs.Rows[hti.RowIndex].Cells[0];
        }
      }
      catch (Exception)
      {
        this.dgv_InjectionTriggerURLs.ClearSelection();
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TSMI_Clear_Click(object sender, EventArgs e)
    {
      try
      {
        this.ClearRecordList();
      }
      catch (Exception)
      {
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TB_AddRecord_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Enter)
      {
        return;
      }

      this.BT_Add_Click(sender, e);
    }

 
    private void BT_AddFile_Click(object sender, EventArgs e)
    {
      this.ofd_FileToInject.InitialDirectory = Directory.GetCurrentDirectory();

      if (this.ofd_FileToInject.ShowDialog() != DialogResult.OK)
      {
        return;
      }

      this.tb_InjectioinContentFile.Text = this.ofd_FileToInject.FileName;
      this.tb_InjectioinContentFile.TextAlign = HorizontalAlignment.Right;
      this.tb_InjectioinContentFile.SelectionStart = this.tb_InjectioinContentFile.Text.Length + 1;
    }


    private void RB_Position_CheckedChanged(object sender, EventArgs e)
    {
      this.rb_After.Checked = !this.rb_Before.Checked == true;
    }

    #endregion

  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
namespace IMKelly_EOLQA
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        string[] scan_new = new string[100];
        List<string> batchlist = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {

          //  if (!IsPostBack)
          //  {
           //     submit_button.Enabled = false;
          //  }
            if (IsPostBack)
            {

                batchlist = (List<string>)ViewState["batchlist"];
              //  Page.Validate();
             //   if (Page.IsValid)
              //  {
               //     submit_button.Enabled = true;
             //   }
             //   else {
               //     submit_button.Enabled = false;
           //     }

                
            }
       
           
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            this.PreRender += new System.EventHandler(this.preRender);
        }

        protected void preRender(object sender, System.EventArgs e)
        {
            
           
           
            string loc = tbLocation.Text;
            ViewState["batchlist"] = batchlist;
            lvValues.DataSource = batchlist;
            lvValues.DataBind();
            
            tbScan.Focus();


        }

     
            protected bool save_data(string kennVal, string Loc)
        {
            
            
            return true;
        }

        private string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        protected void submit_button_Click(object sender, EventArgs e)
        {
            String xml = "";
            string Loc = tbLocation.Text;
           // String timeStamp = GetTimestamp(DateTime.Now);
            foreach(string kennVal in batchlist) {
                using (DataSet dsData = new DataSet())

                {
                    //SAVE THE DATA IN A DB
                    using (SqlConnection sqlconn = new SqlConnection(classGlobal.primaryConnectionString))
                    {
                        string q = @"INSERT INTO dbo.BookBatchNumber (kennVal, location) VALUES (@kenn_val,@loc_val)";
                        using (SqlCommand sqlcmd = new SqlCommand(q, sqlconn))
                        {
                            sqlcmd.Parameters.AddWithValue("@kenn_val", kennVal);
                            sqlcmd.Parameters.AddWithValue("@loc_val", Loc);
                           // sqlcmd.Parameters.AddWithValue("@timestamp", timeStamp);
                            sqlcmd.Connection.Open();
                            sqlcmd.ExecuteNonQuery();


                        }
                    }


                }
              
                xml += "<Row BatchNumber = "+ kennVal +" />";
                using (SqlConnection con = new SqlConnection(classGlobal.primaryConnectionString))
                {
                    using (SqlCommand sqlcmd = new SqlCommand("stk.StockTransferV2TST", con))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@XMLBATCHS", SqlDbType.Xml).Value = xml;
                        sqlcmd.Parameters.Add("@NewLocation", SqlDbType.NVarChar).Value = Loc;
                        sqlcmd.Parameters.Add("@NewBin", SqlDbType.NVarChar).Value = "Floor";
                        sqlcmd.Parameters.Add("@TransactionNote", SqlDbType.NChar).Value = "TEST";
                        con.Open();
                        sqlcmd.ExecuteNonQuery();
                    }
                }

            }
     

        }

        protected void IvLays_ItemDataBound(Object sender, ListViewItemEventArgs e)
        {
            string thisBatch = e.Item.DataItem.ToString();
            Label scanVal = (Label)e.Item.FindControl("lblScanValues");
            scanVal.Text = thisBatch;

        }

        protected void tbScan_TextChanged(object sender, EventArgs e)
        {
            string scanVal = tbScan.Text;
    
                if (scanVal.Length > 8)
                {
                    if (scanVal.StartsWith("BN") == true)
                    {
                        
                        scan_new = scanVal.Split(',');
                        batchlist.Add(scan_new[0]);

                    }

                }

            tbScan.Text = " ";
        }
    }
}
using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using IMKelly_ClassLibrary;

namespace IMKelly_EOLQA
{
    public partial class EOLQAMaster : System.Web.UI.MasterPage
    {
        private string viewMode;
        public string ViewMode { get { return viewMode; } set { viewMode = value; } }
        public string PageTitle 
        { 
            get 
            { 
                return pageTitle.Text; 
            } 
            set 
            { 
                pageTitle.Text = value; 
            } 
        }
        private classWebMessageList oMsg = new classWebMessageList();


        //****************************************************************************
        //** Master page initialization 
        //****************************************************************************
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                classExternalConfiguration oConfig = new classExternalConfiguration(Server.MapPath("~") + @"\config.txt");
                viewMode = oConfig.htSettings["ViewMode"].ToString();

                classGlobal.verboseMessages = Convert.ToBoolean(oConfig.htSettings["VerboseMessages"]);
                classGlobal.woCloseQueue = oConfig.htSettings["WOCloseQueue"].ToString();
                
                // Get the default connection data
                string controlServer = oConfig.htSettings["MDB_Server"].ToString();
                string controlCatalog = oConfig.htSettings["MDB_Database"].ToString();
                string controlUser = oConfig.htSettings["MDB_User"].ToString();
                string controlPassword = oConfig.htSettings["MDB_Password"].ToString();

                SqlConnectionStringBuilder sqlcsbMain = new SqlConnectionStringBuilder();
                sqlcsbMain.Add("Data Source", controlServer);
                sqlcsbMain.Add("Initial Catalog", controlCatalog);
                if (controlUser.Trim() != "")
                {
                    sqlcsbMain.Add("uid", controlUser);
                    sqlcsbMain.Add("pwd", controlPassword);
                }
                else
                {
                    sqlcsbMain.IntegratedSecurity = true;
                }

                classGlobal.conMaster = new System.Data.SqlClient.SqlConnection(sqlcsbMain.ConnectionString);
                classGlobal.primaryConnectionString = sqlcsbMain.ConnectionString;
                classGlobal.closeWorksOrders = Convert.ToBoolean(oConfig.htSettings["closeWorksOrder"]);
                IMKelly_ClassLibrary.classGlobal.oCon = classGlobal.conMaster;
                IMKelly_ClassLibrary.classGlobal.primaryConnectionString = sqlcsbMain.ConnectionString;




                // Get the amalgamated extension db
                string amalServer = oConfig.htSettings["XDB_Server"].ToString();
                string amalCatalog = oConfig.htSettings["XDB_Database"].ToString();
                string amalUser = oConfig.htSettings["XDB_User"].ToString();
                string amalPassword = oConfig.htSettings["XDB_Password"].ToString();

                SqlConnectionStringBuilder sqlcsbAmal = new SqlConnectionStringBuilder();
                sqlcsbAmal.Add("Data Source", amalServer);
                sqlcsbAmal.Add("Initial Catalog", amalCatalog);
                if (amalUser.Trim() != "")
                {
                    sqlcsbAmal.Add("uid", amalUser);
                    sqlcsbAmal.Add("pwd", amalPassword);
                }
                else
                {
                    sqlcsbAmal.IntegratedSecurity = true;
                }

                classGlobal.amalgumConnectionString = sqlcsbAmal.ConnectionString;
                IMKelly_ClassLibrary.classGlobal.amalgumConnectionString = sqlcsbAmal.ConnectionString;


                // Get the amalgamated extension db
                string fbServer = oConfig.htSettings["FDB_Server"].ToString();
                string fbCatalog = oConfig.htSettings["FDB_Database"].ToString();
                string fbUser = oConfig.htSettings["FDB_User"].ToString();
                string fbPassword = oConfig.htSettings["FDB_Password"].ToString();

                SqlConnectionStringBuilder sqlcsbFb = new SqlConnectionStringBuilder();
                sqlcsbFb.Add("Data Source", fbServer);
                sqlcsbFb.Add("Initial Catalog", fbCatalog);
                if (fbUser.Trim() != "")
                {
                    sqlcsbFb.Add("uid", fbUser);
                    sqlcsbFb.Add("pwd", fbPassword);
                }
                else
                {
                    sqlcsbAmal.IntegratedSecurity = true;
                }

                classGlobal.fbConnectionString = sqlcsbFb.ConnectionString;
                IMKelly_ClassLibrary.classGlobal.fbConnectionString = sqlcsbFb.ConnectionString;



                string extensionServer = oConfig.htSettings["EDB_Server"].ToString();
                string extensionCatalog = oConfig.htSettings["EDB_Database"].ToString();
                string extensionUser = oConfig.htSettings["EDB_User"].ToString();
                string extensionPassword = oConfig.htSettings["EDB_Password"].ToString();

                SqlConnectionStringBuilder sqlcsbExtension = new SqlConnectionStringBuilder();
                sqlcsbExtension.Add("Data Source", extensionServer);
                sqlcsbExtension.Add("Initial Catalog", extensionCatalog);
                if (extensionUser.Trim() != "")
                {
                    sqlcsbExtension.Add("uid", extensionUser);
                    sqlcsbExtension.Add("pwd", extensionPassword);
                }
                else
                {
                    sqlcsbExtension.IntegratedSecurity = true;
                }
                classGlobal.extensionConnectionString = sqlcsbExtension.ConnectionString;
                classGlobal.conExtension = new System.Data.SqlClient.SqlConnection(sqlcsbExtension.ConnectionString);
                IMKelly_ClassLibrary.classGlobal.oConExtension = classGlobal.conExtension;
                IMKelly_ClassLibrary.classGlobal.extensionConnectionString = sqlcsbExtension.ConnectionString;

                string securityServer = oConfig.htSettings["SDB_Server"].ToString();
                string securityCatalog = oConfig.htSettings["SDB_Database"].ToString();
                string securityUser = oConfig.htSettings["SDB_User"].ToString();
                string securityPassword = oConfig.htSettings["SDB_Password"].ToString();

                SqlConnectionStringBuilder sqlcsbSecurity = new SqlConnectionStringBuilder();
                sqlcsbSecurity.Add("Data Source", securityServer);
                sqlcsbSecurity.Add("Initial Catalog", securityCatalog);
                if (securityUser.Trim() != "")
                {
                    sqlcsbSecurity.Add("uid", securityUser);
                    sqlcsbSecurity.Add("pwd", securityPassword);
                }
                else
                {
                    sqlcsbSecurity.IntegratedSecurity = true;
                }
                classGlobal.securityConnectionString = sqlcsbSecurity.ConnectionString;
                IMKelly_ClassLibrary.classGlobal.securityConnectionString = sqlcsbSecurity.ConnectionString;

                this.PreRender += new System.EventHandler(this.preRender);
            }
            catch(Exception ex)
            {
                oMsg.add("Error", "Error occured at:", ex.Message);
            }
        }

        //****************************************************************************
        //** Page load 
        //****************************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //*******************************************************
        // Pre render
        //*******************************************************
        protected void preRender(object sender, System.EventArgs e)
        {
            try
            {
                if (classWebMessageList.hasError)
                {
                    foreach (classMessage oMsg in classWebMessageList.errorArray)
                    {
                        HtmlGenericControl alertDiv = new HtmlGenericControl("div");
                        alertDiv.Attributes.Add("class", "alert alert-danger alert-dismissible");
                        alertDiv.Attributes.Add("role", "alert");
                        if (oMsg.input.Trim() != "")
                        {
                            Control oCont = bodyContent.FindControl(oMsg.input);
                            if (oCont != null)
                            {
                                HtmlGenericControl alertLabel = new HtmlGenericControl("label");
                                alertLabel.Attributes.Add("for", oCont.ClientID);
                                alertLabel.InnerHtml = oMsg.message;
                                alertDiv.Controls.Add(alertLabel);
                            }
                            else
                            {
                                alertDiv.InnerHtml = oMsg.message;
                            }
                        }
                        else
                        {
                            alertDiv.InnerHtml = oMsg.message;
                        }
                        HtmlGenericControl ariaSpan = new HtmlGenericControl("span");
                        ariaSpan.Attributes.Add("aria-hidden", "true");
                        ariaSpan.InnerHtml = "&times;";

                        HtmlGenericControl closeButton = new HtmlGenericControl("button");
                        closeButton.Attributes.Add("type", "button");
                        closeButton.Attributes.Add("class", "close");
                        closeButton.Attributes.Add("data-dismiss", "alert");
                        closeButton.Attributes.Add("aria-label", "Close");

                        closeButton.Controls.Add(ariaSpan);
                        alertDiv.Controls.Add(closeButton);

                        messageDiv.Controls.Add(alertDiv);
                    }
                    classMessageList.clearErrors();
                    classWebMessageList.errorArray.Clear();
                }

                foreach (classMessage oMsg in classWebMessageList.warningArray)
                {
                    HtmlGenericControl alertDiv = new HtmlGenericControl("div");
                    alertDiv.Attributes.Add("class", "alert alert-warning alert-dismissible");
                    alertDiv.Attributes.Add("role", "alert");
                    if (oMsg.input.Trim() != "")
                    {
                        Control oCont = bodyContent.FindControl(oMsg.input);
                        if (oCont != null)
                        {
                            HtmlGenericControl alertLabel = new HtmlGenericControl("label");
                            alertLabel.Attributes.Add("for", oCont.ClientID);
                            alertLabel.InnerHtml = oMsg.message;
                            alertDiv.Controls.Add(alertLabel);
                        }
                        else
                        {
                            alertDiv.InnerHtml = oMsg.message;
                        }
                    }
                    else
                    {
                        alertDiv.InnerHtml = oMsg.message;
                    }
                    HtmlGenericControl ariaSpan = new HtmlGenericControl("span");
                    ariaSpan.Attributes.Add("aria-hidden", "true");
                    ariaSpan.InnerHtml = "&times;";

                    HtmlGenericControl closeButton = new HtmlGenericControl("button");
                    closeButton.Attributes.Add("type", "button");
                    closeButton.Attributes.Add("class", "close");
                    closeButton.Attributes.Add("data-dismiss", "alert");
                    closeButton.Attributes.Add("aria-label", "Close");

                    closeButton.Controls.Add(ariaSpan);
                    alertDiv.Controls.Add(closeButton);

                    messageDiv.Controls.Add(alertDiv);
                }
                classWebMessageList.warningArray.Clear();


                foreach (classMessage oMsg in classWebMessageList.infoArray)
                {
                    HtmlGenericControl alertDiv = new HtmlGenericControl("div");
                    alertDiv.Attributes.Add("class", "alert alert-info alert-dismissible");
                    alertDiv.Attributes.Add("role", "alert");
                    if (oMsg.input.Trim() != "")
                    {
                        Control oCont = bodyContent.FindControl(oMsg.input);
                        if (oCont != null)
                        {
                            HtmlGenericControl alertLabel = new HtmlGenericControl("label");
                            alertLabel.Attributes.Add("for", oCont.ClientID);
                            alertLabel.InnerHtml = oMsg.message;
                            alertDiv.Controls.Add(alertLabel);
                        }
                        else
                        {
                            alertDiv.InnerHtml = oMsg.message;
                        }
                    }
                    else
                    {
                        alertDiv.InnerHtml = oMsg.message;
                    }
                    HtmlGenericControl ariaSpan = new HtmlGenericControl("span");
                    ariaSpan.Attributes.Add("aria-hidden", "true");
                    ariaSpan.InnerHtml = "&times;";

                    HtmlGenericControl closeButton = new HtmlGenericControl("button");
                    closeButton.Attributes.Add("type", "button");
                    closeButton.Attributes.Add("class", "close");
                    closeButton.Attributes.Add("data-dismiss", "alert");
                    closeButton.Attributes.Add("aria-label", "Close");

                    closeButton.Controls.Add(ariaSpan);
                    alertDiv.Controls.Add(closeButton);

                    messageDiv.Controls.Add(alertDiv);
                }
                classWebMessageList.infoArray.Clear();
            }
            catch (Exception ex)
            {
                oMsg.add("Error", "Error occured at:", ex.Message);
                ErrorLog.LogError(ex, this.form1.Name.ToString());
            }
        }


        public void hideTitle()
        {
            titleArea.Visible = false;
        }

    }
}
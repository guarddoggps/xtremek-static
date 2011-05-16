using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using AlarmasABC.Core.Tracking;
using AlarmasABC.BLL.ProcessRulesData;

public partial class CompanyAdmin_ucSpeeding : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                DataSet _ds = new DataSet();
                _ds = FormPermission.LoadPermission();
                if (_ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _ds.Tables[0].Rows)
                    {
                        if (row["FormName"].ToString() == "Rules")
                        {
                            if (!((bool)row["delete"]))
                            {
                            }
                            if (!((bool)row["insert"]))
                            {
                                btnSave.Enabled = false;

                            }

                            if (!((bool)row["Edit"]))
                            {


                            }



                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
    }

  

    private bool AddRules()    
    {
       
        try
        {
            RulesData _rulesObj = new RulesData();
            ProcessRulesData _rulesAdd=new ProcessRulesData(AlarmasABC.BLL.InvokeOperations.operations.INSERT);

            _rulesObj.RulesID=1;
            _rulesObj.RulesOperator="<";
            _rulesObj.RulesOperatorName="Less Than";
            _rulesObj.RulesValue=_txtValue.Text.Trim();
            _rulesObj.ComID=int.Parse(Session["trkCompany"].ToString());

            _rulesAdd.RulesObj=_rulesObj;
            _rulesAdd.invoke();
            return true;

        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            return false;
        }
    }

    private void ShowRules()
    {
        ProcessRulesData _rulesData = new ProcessRulesData(AlarmasABC.BLL.InvokeOperations.operations.SELECT);
        DataSet _ds = new DataSet();
        try
        {
            int _comID = int.Parse(Session["trkCompany"].ToString());

            _rulesData.ComID = _comID;
            _rulesData.invoke();
            _ds = _rulesData.Ds;

            _rgriDRules.DataSource = _ds;
            _rgriDRules.DataBind();
        }
        catch (Exception ex)
        { 
        
        }
    }

    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            if (AddRules())
            {

                lblMsg.Text = "Rules Added";
                _rgriDRules.Rebind();
                //ShowRules();
                ResetForm();
            }
            else
            {
                lblMsg.Text = "Rules Added failed";
            }
            _rgriDRules.Rebind();
        }
    }
    protected void _rgriDRules_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        ShowRules();
    }

    protected void _rgriDRules_DeleteCommand(object source, GridCommandEventArgs e)
    {
        //Database db = new Database();

        //GridDataItem item = (GridDataItem)e.Item;

        //string rID = item.OwnerTableView.DataKeyValues[item.ItemIndex]["RulesID"].ToString();

        //try
        //{
        //    string deleteQuery = "DELETE from tblRules where RulesID='" + rID + "'";
        //    db.executeNonQry(deleteQuery);
        //}
        //catch (Exception ex)
        //{
        //    rgriDRules.Controls.Add(new LiteralControl("Unable to delete Rules. Reason: " + ex.Message));
        //    e.Canceled = true;
        //}
        //finally
        //{
        //    db.Close();
        //}
    }
    protected void _rgriDRules_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridCommandItem)
        {
            GridCommandItem commandItem = (GridCommandItem)e.Item;
            Button addNewRecord = (Button)commandItem.Controls[0].Controls[0].Controls[0].Controls[0].Controls[0];
            addNewRecord.Visible = false;
        }
    }
   
   

    private void ResetForm()
    {
        _txtValue.Text = "";

    }
}

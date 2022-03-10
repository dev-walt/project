//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//namespace ProjectTracker
//{
//    public partial class ApproveTS : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {

//        }
//    }
//}
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//namespace ProjectTracker
//{
//    public partial class ApproveTimesheet : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {

//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ProjectTracker.Sessions;

namespace ProjectTracker
{
   
    public partial class ApproveTS : System.Web.UI.Page
    {
        SqlConnection objSqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString);
        [AdminSession]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (objSqlConnection.State == ConnectionState.Closed)
            {
                objSqlConnection.Open();
            }
            if (!IsPostBack)  //first tym page opened or reloaded after entering values means first tym on landing page
            {
                string session_name = Request.QueryString["Session_ID"];

                Label1.Text = session_name;
                 //string getUserId = "select userId from userMaster where userName= '" + session_name+"'";
                /*SqlDataAdapter objSqlAdapter = new SqlDataAdapter("select userId from userMaster where userName= '" + session_name , objSqlConnection);
                DataSet objDataSet = new DataSet();
                objSqlAdapter.Fill(objDataSet);
                dataGrid.DataSource = objDataSet;
                dataGrid.DataBind();*/
               
               /* SqlCommand cmd = new SqlCommand();
                cmd.CommandText = getUserId;
                cmd.Connection = objSqlConnection;
                cmd.ExecuteNonQuery();*/

                ddlResource.Items.Insert(0, new ListItem("--Select Resource--"));
                BindVendors();
                //string username = (string)Session["UserId"];
                //Label1.Visible = true;
                //Label1.Text = username;
               
            }
            //searchBtn.Enabled = false;
            //approveSelectedBtn.Enabled = false;
            approveSelectedBtn.Enabled = false;
            rejectSelectedBtn.Enabled = false;
            label.Visible = false;
        }
        protected void BindVendors()
        {
            try
            {
                SqlDataAdapter objSqlAdapter = new SqlDataAdapter("select * from vendorMaster", objSqlConnection);
                DataSet objDataSet = new DataSet();
                objSqlAdapter.Fill(objDataSet);
                ddlCompany.DataSource = objDataSet;
                ddlCompany.DataTextField = "vendorName";       //table column name - Company and VendorId(PK of table VendorMasterNew)
                ddlCompany.DataValueField = "vendorId";
                ddlCompany.DataBind();
                ddlCompany.Items.Insert(0, new ListItem("--Select Company--")); //on refresh it will again show select company otherwise it would show previously entered data in dropdown
            }
            catch (Exception ex)
            {
                Response.Write("Exception in Binding Vendor Dropdownlist : " + ex.Message.ToString());
            }
            finally
            {
                objSqlConnection.Close();
            }
        }
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string VendorId = Convert.ToString(ddlCompany.SelectedValue);
                SqlDataAdapter objSqlAdapter = new SqlDataAdapter("select * from userMaster where vendorId= '"+ VendorId + "'", objSqlConnection);
                DataSet objDataSet = new DataSet();
                objSqlAdapter.Fill(objDataSet);
                ddlResource.DataSource = objDataSet;
                ddlResource.DataTextField = "firstName";
                ddlResource.DataValueField = "userId";
                ddlResource.DataBind();
                ddlResource.Items.Insert(0, new ListItem("--Select Resource--"));

            }
            catch (Exception ex)
            {
                Response.Write("Exception in Binding Resource Dropdownlist : " + ex.Message.ToString());
            }
            finally
            {
                objSqlConnection.Close();
            }
        }
        protected void ddlResource_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void searchBtnClick(Object sender, EventArgs e)
        {

            try
            {
                string resourceId = Convert.ToString(ddlResource.SelectedValue);
                string timesheetStatus = Convert.ToString(ddlTimesheetStatus.SelectedValue);
                //string fromDate = txtFromDate.Text;
                //DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                if (timesheetStatus == "Approved" || timesheetStatus == "Rejected")
                {
                    //approveSelectedBtn.Visible = false;
                    //rejectSelectedBtn.Visible = false;

                    SqlDataAdapter objSqlAdapter = new SqlDataAdapter("select * from Timesheet where resourceId= '" + resourceId + "' and status= '" + timesheetStatus + "' and approvedOn between '" + txtFromDate.Text + "'and '" + txtToDate.Text + " 23:59:59.000'", objSqlConnection);
                    DataSet objDataSet = new DataSet();
                    objSqlAdapter.Fill(objDataSet);
                    dataGrid.DataSource = objDataSet;
                    dataGrid.DataBind();
                    if (dataGrid.Rows.Count <= 0)
                    {
                        label.Visible = true;
                        label.Text = "No data found";
                    }
                    else
                    {
                        //dataGrid.DataBind();
                    }
                }
                else if (timesheetStatus == "Submitted")
                {
                    //approveSelectedBtn.Visible = true;
                    //rejectSelectedBtn.Visible = true;

                    SqlDataAdapter objSqlAdapter = new SqlDataAdapter("select * from Timesheet where resourceId= '" + resourceId + "' and status= '" + timesheetStatus + "' and submittedOn between '" + txtFromDate.Text + "'and '" + txtToDate.Text + " 23:59:59.000'", objSqlConnection);
                    DataSet objDataSet = new DataSet();
                    objSqlAdapter.Fill(objDataSet);
                    dataGrid.DataSource = objDataSet;
                    dataGrid.DataBind();
                    if (dataGrid.Rows.Count <= 0)
                    {
                        label.Visible = true;
                        label.Text = "No data found";
                        //Response.Write("<script>alert('No data found!!')</script>");
                    }
                    else
                    {

                        approveSelectedBtn.Enabled = true;
                        rejectSelectedBtn.Enabled = true;
                        //dataGrid.DataBind();
                    }
                }
            }
            //else
            //{
            //    Response.Write("Exception in Search button : " + ex.Message.ToString());
            //}
            catch (Exception ex)
            {
                //Response.Write("Exception in Search button : "+ ex.Message.ToString());
                label.Text = "Exception in Search button : " + ex.Message.ToString();
                //label.Visible = true;
            }

        }

        protected void approveSelectedBtnClick(Object sender, EventArgs e)
        {
            //try
            //{

                string timesheetStatus = Convert.ToString(ddlTimesheetStatus.SelectedValue);
            string session_name = Request.QueryString["Session_ID"];

            //Label1.Text = session_name;
            //string getUserId = "select userId from userMaster where userName= '" + session_name + "'";
            foreach (GridViewRow row in dataGrid.Rows)
                {

                    CheckBox status = (row.Cells[0].FindControl("CheckBox") as CheckBox);
                    string projectId = Convert.ToString(row.Cells[1].Text);
                    if (status.Checked)
                    {

                        //updateRow(projectId, true, "Approved");
                        // var a = Response.Output.Write("<script>confirm('Are you sure you want to approve the selected projects?')</script>");
                        string confirmValue = Request.Form["confirm_value"];
                        if (confirmValue == "Yes")
                        {
                            updateRow(projectId, true, "Approved");
                            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Project approved successfully!!')", true);
                            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Approal alert')");
                        }
                        else
                        {
                            //label.Text = "";
                            //Response.Write("<script>alert('Please select  any project to approve!!')</script>");
                            //Response.Write("<script>alert('No project selected to approve!!')</script>");
                            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Project not approved!!')", true);
                        }
                        //Label1.Text = "Records have been updated";
                    }
                    else
                    {
                        //Response.Write("<script>alert('You did not select any project!')</script>");
                        //label.Text = "You did not select any project!";
                        //string confirmValue = Request.Form["confirm_value"];
                        //if (confirmValue == "Yes")
                        //{
                        //    label.Text = "You did not select any project!";
                        //}
                        //else
                        //{
                        //    label.Text = "else in else ";
                        //}   
                    }
                    //    Response.Write("<script>alert('You did not select any project to approve!!')</script>");
                
                //Label1.Text = "Records have been updated";
                dataGrid.DataBind(); //
            }
            //catch(Exception ex)
            //{
            //    Response.Write("Exception in approvebtn: " + ex.Message.ToString());
            //}
        }

        protected void rejectSelectedBtnClick(Object sender, EventArgs e)
        {
            string timesheetStatus = Convert.ToString(ddlTimesheetStatus.SelectedValue);
            //string session_name = Request.QueryString["Session_ID"];

            //Label1.Text = session_name;
            //string getUserId = "select userId from userMaster where userName= '" + session_name + "'";
            foreach (GridViewRow row in dataGrid.Rows)
            {
                CheckBox status = (row.Cells[0].FindControl("CheckBox") as CheckBox);
                string projectId = Convert.ToString(row.Cells[1].Text);
                if (status.Checked)
                {
                    string confirmValue = Request.Form["confirm_value"];
                    if (confirmValue == "Yes")
                    {
                        updateRow(projectId, false, "Rejected");
                        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Project rejected successfully!!')", true);
                    }
                    //else
                    //{
                    //    //Response.Write("<script>alert('No project selected to rejected!!')</script>");
                    //    //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Project not rejected!!')", true);
                    //}
                    //Response.Write("<script>confirm('Are you sure you want to reject the selected projects?')</script>");
                    //updateRow(projectId, false, "Rejected");
                    //if(confirmMsg)
                    //{
                    //    updateRow(projectId, false, "Rejected");
                    //}
                    // else
                    // {
                    //     Response.Write("No project approved!");
                    // }

                    //Label1.Text = "Records have been rejected";

                }
                //else
                //{
                //    Response.Write("<script>alert('No projects selected for rejection!!')</script>");
                //}
            }
            //Label1.Text = "Records have been rejected";
            dataGrid.DataBind();
        }
        private void updateRow(string projectId, bool IsApproved, string timesheetStatus)
        {
            //string mycon = "Data Source=localhost\\SQLEXPRESS; database=ProjectTracker; integrated security=true";
            string session_name = Request.QueryString["Session_ID"];
            //string getUserId = "select userId from userMaster where userName= '" + session_name + "'";
           /* SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = getUserId;*/
           /* sqlCommand.Connection = objSqlConnection;
            sqlCommand.ExecuteNonQuery();*/
            string currentdate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            //string updateData = "Update Timesheet set isApproved='" + IsApproved + "' , approvedOn= '" + currentdate.ToString() + "' , Status= '" + timesheetStatus + "', approvalRemark= '" + remarks.Text + "' where projectId = '" + projectId + "'";
            //approvedOn = '2-18-2022 21:09:08 PM'
            string updateData = "Update Timesheet set isApproved='" + IsApproved + "' , status= '" + timesheetStatus + "', approvedBy= '"+ session_name + "' , approvalRemark= '" + remarks.Text + "' , approvedOn = '"+ currentdate + "' where projectId = '" + projectId + "'";
            //SqlDataAdapter objSqlAdapter = new SqlDataAdapter("Update UserTimesheet set IsApproved='"+IsApproved+"' where ProjectId = '"+ projectId +"'", objSqlConnection);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = updateData;
            cmd.Connection = objSqlConnection;
            cmd.ExecuteNonQuery(); 
            //DataSet objDataSet = new DataSet();
            //objSqlAdapter.Fill(objDataSet);
            //dataGrid.DataSource = objDataSet;
            //dataGrid.DataBind();
        }



        //private void rejectRow(string projectId, bool IsApproved)
        //{
        //    //string mycon = "Data Source=localhost\\SQLEXPRESS; database=ProjectTracker; integrated security=true";
        //    string updateData = "Update UserTimesheet set IsApproved='" + IsApproved + "' ,  TimesheetStatus= 'Rejected'  where ProjectId = '" + projectId + "'";
        //    //SqlDataAdapter objSqlAdapter = new SqlDataAdapter("Update UserTimesheet set IsApproved='"+IsApproved+"' where ProjectId = '"+ projectId +"'", objSqlConnection);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandText = updateData;
        //    cmd.Connection = objSqlConnection;
        //    cmd.ExecuteNonQuery();
        //    //DataSet objDataSet = new DataSet();
        //    //objSqlAdapter.Fill(objDataSet);
        //    //dataGrid.DataSource = objDataSet;
        //    //dataGrid.DataBind();
        //}
        protected void ddlTimesheetStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SqlDataAdapter objSqlAdapter = new SqlDataAdapter();
            //DataSet objDataSet = new DataSet();
            //objSqlAdapter.Fill(objDataSet);

        }

        protected void grid_RowDataBound(object sender, EventArgs e)
        {

        }

        protected void dataGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}

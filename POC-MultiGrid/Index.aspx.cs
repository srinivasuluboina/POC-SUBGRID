using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace POC_MultiGrid
{
    public partial class Index : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection("Data Source=10.10.20.182;Initial Catalog=Argo_US_Phase6;Persist Security Info=True;User ID=occcuser;Password=yash@2324");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                blBindGrid();
            }
        }

        protected void gvPrintResponse_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string questionId = gvPrintResponse.DataKeys[e.Row.RowIndex].Value.ToString();
                GridView gvSubquestions = e.Row.FindControl("gvSubquestions") as GridView;
                gvSubquestions.DataSource = blGetSubQuestions(questionId);
                gvSubquestions.DataBind();
                gvSubquestions.Style.Add("display", "none");
            }
        }

        private void blBindGrid()
        {
            // SqlConnection con = new SqlConnection("Data Source=10.10.20.182;Initial Catalog=Argo_US_Phase6;Persist Security Info=True;User ID=occcuser;Password=yash@2324");
            SqlDataAdapter da = new SqlDataAdapter("select * from  ArgAMS_SelfAuditQuestion where AuditTypeID = 1 and ParentQuestionID is null", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gvPrintResponse.DataSource = dt;
            gvPrintResponse.DataBind();
        }
        private DataTable blGetSubQuestions(string questionId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select QuestionID as SubQuestionID,Description as SubQuestion from  ArgAMS_SelfAuditQuestion where AuditTypeID = 1 and ParentQuestionID =" + questionId, con);
            da.Fill(dt);
            return dt;
        }

        private DataTable GetTableSchema()
        {
            DataTable dt = new DataTable();
            dt.TableName = "ArgAMS_UserOptions";
            //creating columns for DataTable  
            dt.Columns.Add(new DataColumn("QuestionID", typeof(int)));
            dt.Columns.Add(new DataColumn("OptionYes", typeof(byte)));
            dt.Columns.Add(new DataColumn("OptionNo", typeof(byte)));
            return dt;
        }

        private DataTable GetDataTable()
        {
            DataTable dt = GetTableSchema();
           
            foreach (GridViewRow row in gvPrintResponse.Rows)
            {
                DataRow dr = dt.NewRow();
                bool isYes= Convert.ToBoolean((row.FindControl("chkPrintYes") as RadioButton).Checked);
                if (isYes)
                {
                    GridView gvSubquestions = row.FindControl("gvSubquestions") as GridView;
                    foreach(GridViewRow subRow in gvSubquestions.Rows)
                    {
                        DataRow subDr = dt.NewRow();
                        subDr["QuestionID"] = gvSubquestions.DataKeys[subRow.RowIndex].Value.ToString();
                        subDr["OptionYes"] = Convert.ToBoolean((subRow.FindControl("chkPrintYes") as RadioButton).Checked); 
                        subDr["OptionNo"] = Convert.ToBoolean((subRow.FindControl("chkPrintNo") as RadioButton).Checked);
                        dt.Rows.Add(subDr);
                    }
                }

                dr["QuestionID"] = gvPrintResponse.DataKeys[row.RowIndex].Value.ToString();
                dr["OptionYes"] = isYes;
                dr["OptionNo"] = Convert.ToBoolean((row.FindControl("chkPrintNo") as RadioButton).Checked);

                dt.Rows.Add(dr);
            }

            return dt;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Get the DataTable 
            DataTable dtInsertRows = GetDataTable();

            using (SqlBulkCopy sbc = new SqlBulkCopy(con))
            {
                sbc.DestinationTableName = "ArgAMS_UserOptions";

                // Number of records to be processed in one go
                sbc.BatchSize = 2;

                // Map the Source Column from DataTabel to the Destination Columns in SQL Server 2005 Person Table
                sbc.ColumnMappings.Add("QuestionID", "QuestionID");
                sbc.ColumnMappings.Add("OptionYes", "OptionYes");
                sbc.ColumnMappings.Add("OptionNo", "OptionNo");

                // Number of records after which client has to be notified about its status
                sbc.NotifyAfter = dtInsertRows.Rows.Count;
                // Finally write to server
                con.Open();
                sbc.WriteToServer(dtInsertRows);
                sbc.Close();
            }




        }
    }
}
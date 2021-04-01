using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReminderToDo
{
    public partial class Index : System.Web.UI.Page
    {
        DateTime temp;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DateTime temp;
                //string inDtTm = "02-29-2016" + " " + "23:00:00";
                //string inDtTm = "31/31/2016" + " " + "23:00:00";        	
                //Response.Write(DateTime.TryParse(inDtTm, out temp));

                SetDefaultSettings();
                LoadTasks();
            }
        }
        protected void SetDefaultSettings()
        {
            hdActionId.Value = "0";

        }
        protected void Reset()
        {
            hdActionId.Value = "0";
            action.Value = "";
            remarks.Value = "";
            date.Value = "";
            time.Value = "";
            ddlStatus.SelectedIndex = -1;
            ddlStatus.Items.FindByValue("Pending").Selected = true;
        }
        protected void LoadTasks(string SortExpression=null, string sortingDirection=null)
        {
            Task task = new Task();
            TaskDL objTaskDL = new TaskDL();
            ProcessResponse objResponse = new ProcessResponse();

            objResponse.Command = "SELECTALL";

            if (keyword.Value.Trim() != "")
                task.Action = keyword.Value.Trim();
            if (fromdate.Value.Trim() != "")
                objResponse.FromDate = fromdate.Value;
            if (todate.Value.Trim() != "")
                objResponse.ToDate = todate.Value;

            task.ActionStatus = ddlSearchStatus.SelectedValue;

            if (SortExpression == null)
                SortExpression = "EntryDate";
            if (sortingDirection == null)
                sortingDirection = "Asc";
            objResponse.SortBy = SortExpression;// "EntryDate";
            objResponse.SortOrder = sortingDirection;//"Asc";


            objResponse.IsDebugMode = false;
            objResponse.IsDebugStop = false;

            gvTask.DataSource = objTaskDL.GetAllTask(objResponse, task);
            gvTask.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isError = false;
            string strMsg = string.Empty;

            if (action.Value.Trim() == "")
            {
                isError = true;
                strMsg = "Action required!";
            }
            else if (date.Value.Trim() == "")
            {
                isError = true;
                strMsg = "ToDo Date required!";
            }
            else if (time.Value.Trim() == "")
            {
                isError = true;
                strMsg = "ToDo Time required!";
            }
            else if (date.Value.Trim() != "" && time.Value.Trim() != "")
            {

                string inDtTm = date.Value + " " + time.Value;

                if (!DateTime.TryParse(inDtTm, out temp))
                {
                    isError = true;
                    strMsg = "Invalid ToDo Date & Time!";
                }
            }

            if (isError)
            {
                msg.InnerText = strMsg;
                msg.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                SaveTask();
                Reset();
                LoadTasks();
            }
        }
        protected void SaveTask()
        {
            Task task = new Task();
            TaskDL objTaskDL = new TaskDL();
            ProcessResponse objResponse = new ProcessResponse();

            if (Convert.ToInt64(hdActionId.Value) > 0)
            {
                objResponse.Command = "UPDATE";
                task.ActionId = Convert.ToInt64(hdActionId.Value);
            }
            else
            {
                objResponse.Command = "INSERT";
            }

            task.Action = action.Value;
            task.Remarks = remarks.Value;
            task.ToDoDateTime = temp;
            task.ActionStatus = ddlStatus.SelectedValue;
            task.SendAlert = false;
            //task.AlertDatetime = DateTime.Now;
            task.AlertMailTo = "";
            task.AlertSent = false;
            task.EntryDate = DateTime.Now;
            task.UpdateDate = DateTime.Now;

            objResponse.IsDebugMode = false;
            objResponse.IsDebugStop = false;

            objResponse = objTaskDL.SaveTask(objResponse, task);
            msg.InnerText = objResponse.CustomMessage;
            msg.Attributes.Add("class", (objResponse.IsSuccess ? "alert alert-success" : "alert alert-danger"));
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
            msg.InnerText = "";
            msg.Attributes.Remove("class");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Reset();
            msg.InnerText = "";
            msg.Attributes.Remove("class");
            LoadTasks();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Reset();
            msg.InnerText = "";
            msg.Attributes.Remove("class");
            keyword.Value = "";
            fromdate.Value = "";
            todate.Value = "";
            ddlSearchStatus.SelectedIndex = -1;
            LoadTasks();
        }

        protected void lbAdd_Click(object sender, EventArgs e)
        {
            Reset();
            action.Focus();
        }
        protected void gvTask_RowCommand(object sender, GridViewCommandEventArgs e)
        {
          //  var current = gvTask.CurrentRow;
            if ( (e.CommandName.ToLower() !="sort")) // Means that you've not clicked the column header
            {
                btnReset_Click(sender, e);

                Task task = new Task();
                TaskDL objTaskDL = new TaskDL();
                ProcessResponse objResponse = new ProcessResponse();

                task.ActionId = Convert.ToInt64(e.CommandArgument);
                hdActionId.Value = Convert.ToString(task.ActionId);

                if (e.CommandName == "EditTask")
                {
                    objResponse.Command = "Select";
                    task = objTaskDL.GetTask(objResponse, task);


                    if (task != null)
                    {
                        action.Focus();
                        action.Value = task.Action;
                        remarks.Value = task.Remarks;
                        date.Value = task.ToDoDateTime.ToShortDateString();
                        time.Value = task.ToDoDateTime.ToShortTimeString();
                        ddlStatus.SelectedIndex = -1;
                        ddlStatus.Items.FindByValue(task.ActionStatus).Selected = true;
                    }

                }

                else if (e.CommandName == "DeleteTask")
                {


                    objResponse.Command = "Delete";

                    objResponse = objTaskDL.DeleteTask(objResponse, task);

                    msg.InnerText = objResponse.CustomMessage;
                    msg.Attributes.Add("class", (objResponse.IsSuccess ? "alert alert-success" : "alert alert-danger"));
                }

                LoadTasks();
            }
        }

        protected void gvTask_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortingDirection = string.Empty;
            if (direction == SortDirection.Ascending)
            {
                direction = SortDirection.Descending;
                sortingDirection = "Desc";


            }
            else
            {
                direction = SortDirection.Ascending;
                sortingDirection = "Asc";

            }
            //DataView sortedView = new DataView(LoadTasks());
            //sortedView.Sort = e.SortExpression + " " + sortingDirection;
            //Session["SortedView"] = sortedView;
            //gvTask.DataSource = sortedView;
            //gvTask.DataBind();
            LoadTasks(e.SortExpression, sortingDirection);
        } 
        public SortDirection direction
    {
        get
        {
            if (ViewState["directionState"] == null)
            {
                ViewState["directionState"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["directionState"];
        }
        set
        {
            ViewState["directionState"] = value;
        }
    }
    }
  
    internal class Task
    {
        private Int64 _ActionId;
        private Int32 _UserId;
        private string _Action;
        private string _Remarks;
        private DateTime _ToDoDateTime;
        private string _ActionStatus;
        private bool _SendAlert;
        private DateTime? _AlertDatetime;
        private string _AlertMailTo;
        private bool _AlertSent;
        private DateTime _EntryDate;
        private DateTime _UpdateDate;

        public Int64 ActionId
        {
            get { return _ActionId; }
            set { _ActionId = value; }
        }

        public Int32 UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public string Action
        {
            get { return _Action; }
            set { _Action = value; }
        }

        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }

        public DateTime ToDoDateTime
        {
            get { return _ToDoDateTime; }
            set { _ToDoDateTime = value; }
        }

        public string ActionStatus
        {
            get { return _ActionStatus; }
            set { _ActionStatus = value; }
        }

        public bool SendAlert
        {
            get { return _SendAlert; }
            set { _SendAlert = value; }
        }

        public DateTime? AlertDatetime
        {
            get { return _AlertDatetime; }
            set { _AlertDatetime = value; }
        }

        public string AlertMailTo
        {
            get { return _AlertMailTo; }
            set { _AlertMailTo = value; }
        }

        public bool AlertSent
        {
            get { return _AlertSent; }
            set { _AlertSent = value; }
        }

        public DateTime EntryDate
        {
            get { return _EntryDate; }
            set { _EntryDate = value; }
        }

        public DateTime UpdateDate
        {
            get { return _UpdateDate; }
            set { _UpdateDate = value; }
        }



        public Task()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    internal class TaskDL
    {
        public TaskDL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        Database objCon = new Database();
        SqlCommand objCmd = new SqlCommand();
        ProcessResponse objResponse = new ProcessResponse();

        public ProcessResponse SaveTask(ProcessResponse response, Task objTask)
        {

            SqlDataReader objReader = default(SqlDataReader);
            try
            {
                objCmd.Parameters.Add("@Command", System.Data.SqlDbType.VarChar).Value = response.Command;
                objCmd.Parameters.Add("@ActionId", System.Data.SqlDbType.BigInt).Value = objTask.ActionId;
                objCmd.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = objTask.UserId;
                objCmd.Parameters.Add("@Action", System.Data.SqlDbType.VarChar).Value = objTask.Action;
                objCmd.Parameters.Add("@Remarks", System.Data.SqlDbType.VarChar).Value = objTask.Remarks;
                objCmd.Parameters.Add("@ToDoDateTime", System.Data.SqlDbType.DateTime).Value = objTask.ToDoDateTime;
                objCmd.Parameters.Add("@ActionStatus", System.Data.SqlDbType.VarChar).Value = objTask.ActionStatus;
                objCmd.Parameters.Add("@SendAlert", System.Data.SqlDbType.Bit).Value = objTask.SendAlert;
                objCmd.Parameters.Add("@AlertDatetime", System.Data.SqlDbType.DateTime).Value = objTask.AlertDatetime;
                objCmd.Parameters.Add("@AlertMailTo", System.Data.SqlDbType.VarChar).Value = objTask.AlertMailTo;
                objCmd.Parameters.Add("@AlertSent", System.Data.SqlDbType.Bit).Value = objTask.AlertSent;
                objCmd.Parameters.Add("@EntryDate", System.Data.SqlDbType.DateTime).Value = objTask.EntryDate;
                objCmd.Parameters.Add("@UpdateDate", System.Data.SqlDbType.DateTime).Value = objTask.UpdateDate;


                //////////////////////////////////////////
                //' '' // START : For Debuging ==================================

                if (response.IsDebugMode)
                {
                    string strREsponse = "";
                    strREsponse += " <Br />exec Usp_Manage_Actions <Br />";

                    for (int i = 0; i <= objCmd.Parameters.Count - 1; i++)
                    {
                        if (objCmd.Parameters[i].Value != null)
                            strREsponse += objCmd.Parameters[i].ParameterName + " = '" + objCmd.Parameters[i].Value.ToString().Replace("'", "''") + "' ," + "<br/>";
                    }

                    HttpContext.Current.Response.Write(strREsponse);
                    if (response.IsDebugStop)
                        HttpContext.Current.Response.End();
                }
                /////// End : For Debuging //////////////////////////////////////////

                objReader = objCon.Usp_Get_DataReader("Usp_Manage_Actions", objCmd);
                if (objReader.HasRows)
                {
                    objReader.Read();
                    objResponse.IsSuccess = Convert.ToBoolean(objReader["IsSuccess"]);
                    objResponse.Identity = Convert.ToUInt32(objReader["IdentityValue"]);
                    objResponse.Message = Convert.ToString(objReader["ProcessMessage"]);
                    objResponse.CustomMessage = "Task Information saved!";
                }
                else
                {
                    objResponse.IsSuccess = false;
                    objResponse.Identity = 0;
                    objResponse.Message = "Task Information save process failed";
                }
            }
            catch (Exception ex)
            {
                objResponse.IsSuccess = false;
                objResponse.Identity = 0;
                objResponse.Message = ex.Message.ToString();
            }
            finally
            {
                if (objReader != null && !objReader.IsClosed)
                {
                    objReader.Close();
                }
                objReader = null;
                objCmd.Dispose();
                objCmd = null;
                objCon.CloseConnection();
                objCon = null;
            }

            return objResponse;
        }

        public Task GetTask(ProcessResponse response, Task objTask)
        {

            SqlDataReader objReader = default(SqlDataReader);

            objCmd.Parameters.Add("@Command", SqlDbType.VarChar, 50).Value = response.Command;
            objCmd.Parameters.Add("@ActionId", System.Data.SqlDbType.BigInt).Value = objTask.ActionId;

            objReader = objCon.Usp_Get_DataReader("Usp_Manage_Actions", objCmd);

            if (objReader.HasRows)
            {
                while (objReader.Read())
                {

                    objTask.ActionId = Convert.ToInt64(objReader["ActionId"]);
                    objTask.UserId = Convert.ToInt32(objReader["UserId"]);
                    objTask.Action = Convert.ToString(objReader["Action"]);
                    objTask.Remarks = Convert.ToString(objReader["Remarks"]);
                    objTask.ToDoDateTime = Convert.ToDateTime(objReader["ToDoDateTime"]);
                    objTask.ActionStatus = Convert.ToString(objReader["ActionStatus"]);
                    objTask.SendAlert = Convert.ToBoolean(objReader["SendAlert"]);
                    if (!String.IsNullOrEmpty(objReader["AlertDatetime"].ToString()))
                        objTask.AlertDatetime = Convert.ToDateTime(objReader["AlertDatetime"]);
                    objTask.AlertMailTo = Convert.ToString(objReader["AlertMailTo"]);
                    objTask.AlertSent = Convert.ToBoolean(objReader["AlertSent"]);
                    objTask.EntryDate = Convert.ToDateTime(objReader["EntryDate"]);
                    objTask.UpdateDate = Convert.ToDateTime(objReader["UpdateDate"]);
                }
            }


            if (!objReader.IsClosed)
            {
                objReader.Close();
            }
            objReader = null;
            objCmd.Dispose();
            objCmd = null;
            objCon.CloseConnection();
            objCon = null;


            return objTask;
        }

        public DataTable GetAllTask(ProcessResponse response, Task objTask)
        {
            DataTable dt = new DataTable();
            SqlDataReader objReader = default(SqlDataReader);
            response.Command = "SELECTALL";
            objCmd.Parameters.Add("@Command", SqlDbType.VarChar, 50).Value = response.Command;
            objCmd.Parameters.Add("@Action", SqlDbType.VarChar, 50).Value = objTask.Action;
            objCmd.Parameters.Add("@ActionStatus", SqlDbType.VarChar, 50).Value = objTask.ActionStatus;
            objCmd.Parameters.Add("@FromDate", SqlDbType.VarChar, 50).Value = response.FromDate;
            objCmd.Parameters.Add("@ToDate", SqlDbType.VarChar, 50).Value = response.ToDate;
            objCmd.Parameters.Add("@SortBy", SqlDbType.VarChar, 50).Value = response.SortBy;
            objCmd.Parameters.Add("@SortOrder", SqlDbType.VarChar, 50).Value = response.SortOrder;

            //////////////////////////////////////////
            //' '' // START : For Debuging ==================================
            if (response.IsDebugMode)
            {
                string strREsponse = "";
                strREsponse += " <Br />exec Usp_Manage_Actions <Br />";

                for (int i = 0; i <= objCmd.Parameters.Count - 1; i++)
                {
                    if (objCmd.Parameters[i].Value != null)
                        strREsponse += objCmd.Parameters[i].ParameterName + " = '" + objCmd.Parameters[i].Value.ToString().Replace("'", "''") + "' ," + "<br/>";
                }

                HttpContext.Current.Response.Write(strREsponse);
                if (response.IsDebugStop)
                    HttpContext.Current.Response.End();
            }
            /////// End : For Debuging //////////////////////////////////////////

            objReader = objCon.Usp_Get_DataReader("Usp_Manage_Actions", objCmd);

            if (objReader != null && objReader.HasRows)
            {
                dt.Load(objReader);
            }


            if (objReader != null && !objReader.IsClosed)
            {
                objReader.Close();
            }
            objReader = null;
            objCmd.Dispose();
            objCmd = null;
            objCon.CloseConnection();
            objCon = null;

            return dt;
        }

        public ProcessResponse DeleteTask(ProcessResponse response, Task objTask)
        {

            SqlDataReader objReader = default(SqlDataReader);
            try
            {
                objCmd.Parameters.Add("@Command", System.Data.SqlDbType.VarChar).Value = response.Command;
                objCmd.Parameters.Add("@ActionId", System.Data.SqlDbType.BigInt).Value = objTask.ActionId;

                objReader = objCon.Usp_Get_DataReader("Usp_Manage_Actions", objCmd);
                if (objReader.HasRows)
                {
                    objReader.Read();
                    objResponse.IsSuccess = Convert.ToBoolean(objReader["IsSuccess"]);
                    objResponse.Identity = Convert.ToUInt32(objReader["IdentityValue"]);
                    objResponse.Message = Convert.ToString(objReader["ProcessMessage"]);
                    objResponse.CustomMessage = "Task Information deleted!";
                }
                else
                {
                    objResponse.IsSuccess = false;
                    objResponse.Identity = 0;
                    objResponse.Message = "Task Information delete process failed";
                }
            }
            catch (Exception ex)
            {
                objResponse.IsSuccess = false;
                objResponse.Identity = 0;
                objResponse.Message = ex.Message.ToString();
            }
            finally
            {
                if (!objReader.IsClosed)
                {
                    objReader.Close();
                }
                objReader = null;
                objCmd.Dispose();
                objCmd = null;
                objCon.CloseConnection();
                objCon = null;
            }

            return objResponse;
        }

    }

    internal class Database
    {
        private SqlConnection ConnObj;
        //' Your Database ConnectionString
        private string ConnString = "DataConn";


        public Database()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[ConnString].ConnectionString))
            {
                ConnObj = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnString].ConnectionString);
            }
            else
            {
                throw new Exception("Connection String is Empty");
            }

        }

        public SqlDataReader Usp_Get_DataReader(string spName, SqlCommand cmd)
        {
            ConnObj.Open();
            cmd.Connection = ConnObj;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 8000;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            SqlDataReader objReader = default(SqlDataReader);

            try
            {
                objReader = cmd.ExecuteReader();

            }
            catch (Exception ex)
            {
                cmd.Dispose();
                cmd = null;
                ConnObj.Close();

            }

            return objReader;

        }

        public void CloseConnection()
        {
            if (ConnObj.State == System.Data.ConnectionState.Open)
            {
                ConnObj.Close();
            }
        }
    }
}
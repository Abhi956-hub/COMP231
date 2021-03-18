<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ReminderToDo.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
<!-- Bootstrap CSS & Jquery ================================================== -->   
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
      <h1>

      </h1>
             <div class="container">
    	<div class="row">
 
        	<div class="col-sm-12">
            	<div class="panel panel-default">
                	<div class="panel-heading form-head">
                    	Task
                	</div>
                	<div class="panel-body">
                    	<asp:Panel ID="pnlAdd" runat="server" DefaultButton="btnSubmit">
                        	<div class="form-group row">
                            	<div class="col-sm-6">
                                	<label for="action" class="sr-only">
                                    	Action:</label>
                                	<input runat="server" type="text" placeholder="Enter task" class="form-control" id="action" name="action" />
                            	</div>
                        	</div>
                        	<div class="form-group row">
                            	<div class="col-sm-6">
                                	<label for="remarks" class="sr-only">
                                        Remarks:</label>
                                	<input runat="server" type="text" placeholder="Enter remarks" class="form-control" id="remarks" name="remarks" />
                            	</div>
                        	</div>
                        	<div class="form-group row">
                            	<div class="col-sm-2">
                                	To-Do Date & Time
                            	</div>
                            	<div class="col-sm-2">
                                	<input runat="server" type="text" placeholder="MM/DD/YYYY" class="form-control" id="date" name="date" /><small>Date in MM/DD/YYYY</small>
                            	</div>
                            	<div class="col-sm-2">
                                	<input runat="server" type="text" placeholder="hh:mm:ss AM" class="form-control" id="time" name="time" /><small>Time in hh-mm-ss (12 Hrs.)</small>
                            	</div>
 
                        	</div>
                        	<div class="form-group row">
                  	          <div class="col-sm-2">
                                	<label for="remarks" class="sr-only">
                                    	Status:</label>
                                	<asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                    	<asp:ListItem Value="Pending" Text="Pending" Selected="True"></asp:ListItem>
                                    	<asp:ListItem Value="Completed" Text="Completed"></asp:ListItem>
             	                       <asp:ListItem Value="On-Hold" Text="On-Hold"></asp:ListItem>
 
                                	</asp:DropDownList>
                            	</div>
                        	</div>
 
                        	<div class="form-group row">
                            	<div class="col-sm-4">
                                	<asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                	<asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click"  />
                            	</div>
                        	</div>
                        	<div class="form-msg">
                            	<div id="msg" runat="server">
                            	</div>
                            	<input type="text" runat="server" readonly="readonly" style="width: 0px; height: 0px; border: 0px;"
                                	id="focussave" name="focussave" />
            	            </div>
                    	</asp:Panel>
                	</div>
            	</div>
        	</div>
 
    	</div>
 
    	<div class="row">
 
        	<div class="col-sm-12">
            	<div class="panel panel-default">
                	<div class="panel-heading form-head">
                    	Search Task
                	</div>
                	<div class="panel-body">
                    	<div class="form-group row">
                        	<asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearch">
                            	<div class="col-sm-3">
                                	<input runat="server" type="text" placeholder="Enter keyword" class="form-control" id="keyword" name="keyword">
                            	</div>
                            	<div class="col-sm-2">
                                	<input runat="server" type="text" placeholder="From date" class="form-control" id="fromdate" name="fromdate" /><small>Date in MM/DD/YYYY</small>
                            	</div>
                            	<div class="col-sm-2">
                                	<input runat="server" type="text" placeholder="To date" class="form-control" id="todate" name="todate" /><small>Date in MM/DD/YYYY</small>
                            	</div>
                            	<div class="col-sm-2">
                                	<label for="remarks" class="sr-only">
                                    	Status:</label>
                                	<asp:DropDownList ID="ddlSearchStatus" runat="server" CssClass="form-control">
                                    	<asp:ListItem Value="All" Text="All" Selected="True"></asp:ListItem>
                                    	<asp:ListItem Value="Pending" Text="Pending"></asp:ListItem>
                                    	<asp:ListItem Value="Completed" Text="Completed"></asp:ListItem>
                                    	<asp:ListItem Value="On-Hold" Text="On-Hold"></asp:ListItem>
                                	</asp:DropDownList>
                            	</div>
                            	<div class="col-sm-2">
                                	<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"  />
                                	<asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click"  />
                            	</div>
                        	</asp:Panel>
                    	</div>
 
                	</div>
            	</div>
        	</div>
 
    	</div>
 
    	<div class="row">
        	<div class="panel panel-default">
            	<div class="panel-heading form-head">
                	Task List                  	
                	<asp:LinkButton ID="lbAdd" runat="server" Text="New Task" style="float:right;" OnClick="lbAdd_Click" ></asp:LinkButton>
            	</div>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DataConn %>" SelectCommand="SELECT * FROM [tblActions]"></asp:SqlDataSource>
            	<div class="panel-body">
                	<div class="form-group row">
                    	<asp:GridView ID="gvTask" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover"
                        	AutoGenerateColumns="false" OnRowCommand="gvTask_RowCommand">
                        	<RowStyle CssClass="small" />
                        	<Columns>
                            	<asp:TemplateField HeaderText="No.">
                                	<ItemTemplate>
              	                      <%#Eval("RowId") %>
                                	</ItemTemplate>
                            	</asp:TemplateField>
                            	<asp:TemplateField HeaderText="Task">
                                	<ItemTemplate>
                                    	<%#Eval("Action") %>
                                	</ItemTemplate>
                            	</asp:TemplateField>
                            	<asp:TemplateField HeaderText="Remarks">
                                	<ItemTemplate>
                                    	<%#Eval("Remarks") %>
                                	</ItemTemplate>
                            	</asp:TemplateField>
                            	<asp:TemplateField HeaderText="To-Do Date & Time">
                                	<ItemTemplate>
                                    	<%#Eval("ToDoDateTime") %>
                                	</ItemTemplate>
                            	</asp:TemplateField>
                            	<asp:TemplateField HeaderText="Entry Date">
                                	<ItemTemplate>
                                    	<%#Eval("EntryDate") %>
                                	</ItemTemplate>
                            	</asp:TemplateField>
                            	<asp:TemplateField HeaderText="Status">
                                	<ItemTemplate>
                                    	<%#Eval("ActionStatus") %>
         	                       </ItemTemplate>
                            	</asp:TemplateField>
                            	<asp:TemplateField HeaderText="Action">
                                	<ItemTemplate>
                                    	<asp:LinkButton ID="lbEdit" runat="server" Text="Edit" CommandName="EditTask" CommandArgument='<%#Eval("ActionID") %>'></asp:LinkButton>&nbsp;|&nbsp;
                                                 <asp:LinkButton ID="lbDelete" runat="server" Text="Delete" CommandName="DeleteTask" CommandArgument='<%#Eval("ActionID") %>'></asp:LinkButton>
                                	</ItemTemplate>
 
                            	</asp:TemplateField>
                        	</Columns>
                        	<EmptyDataTemplate>
                            	No Task found!
 
                        	</EmptyDataTemplate>
 
                    	</asp:GridView>
                	</div>
            	</div>
        	</div>
 
    	</div>
	</div>
 
 
	<asp:HiddenField ID="hdActionId" runat="server" />
      
    </form>
</body>
</html>

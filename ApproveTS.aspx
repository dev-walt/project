<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveTS.aspx.cs" Inherits="ProjectTracker.ApproveTS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>--%>
<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveTimesheet.aspx.cs" Inherits="ProjectTracker.ApproveTimesheet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveTS.aspx.cs" Inherits="ProjectTracker.ApproveTS" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-3.4.1.js"></script>
    <link href="Styles/jquery-ui.css" rel="stylesheet" />
    <link href="Styles/StyleSheet1.css" rel="stylesheet" />
    
    <script src="Scripts/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtFromDate').datepicker({
                /*dateFormat: "dd-mm-yy",*/
                changeMonth: true,
                changeYear: true
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtToDate').datepicker({
               /* dateFormat: "dd-mm-yy",*/
                changeMonth: true,
                changeYear: true
            });
        });
    </script>
    <script>
        function ConfirmApproval() {
            var confirm_value = document.createElement("input");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure you want to approve the selected projects??")) {
                confirm_value.value = "Yes";
            }
            else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
        function ConfirmRejection() {
            var confirm_value = document.createElement("input");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure you want to reject the selected projects??")) {
                confirm_value.value = "Yes";
            }
            else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <script type="text/javascript">
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>

</head>
<body>


    <%--<div class="container">
        <div class="navbar navbar-fixed-top" style="height:-50px;padding-top:10px">
            <div class="header">
                <div class="col-md-3">
                    <img class="img-responsive" style="width:180px" src="" />
                </div>
                <div class="col-md-8"></div>
                <div class="col-md-1">
                    <img class="img-responsive" style="height:50px;width:101px" src= />
                </div>
            </div>
        </div>
    </div>--%>
    
    
    <section style="background-color:palegoldenrod">
        
        <a href="/Admin/Dashboard"><img style="width:180px; margin-top:5px; margin-left:-830px" src="Content/Images/dlplLogo.svg"/></a>

        <a href="#"><img class="img-responsive" style="height:50px;width:101px;margin-top:5px; float:right" src="Content/Images/profilepic.svg" /></a>
            
           <a href="/UserLogin/Logout"><strong style="float:right; margin-top:20px">Hi <asp:Label ID="Label1" runat="server"></asp:Label> !</strong></a>
            
            
                
                <%--<div class="profileDiv">
                    <img class="img-responsive" style="height:50px;width:101px" src="Images/image 2.png" />
                </div>--%>
            
        
    </section>
    <br />
    
    <section class="menubar" style="background-color:lavenderblush; margin-top:-19px">
        <table class="menubarTable">
            <tr>
                <td class="menubarItems"> <asp:HyperLink ID="addUpdateProject" runat="server" NavigateUrl="/manageproject/addproject">Add & Update Project</asp:HyperLink></td>
                <td class="menubarItems"><asp:HyperLink ID="viewProject" runat="server" NavigateUrl="/manageproject/viewproject">View Project</asp:HyperLink></td>
                <td class="menubarItems"><asp:HyperLink ID="searchProject" runat="server" NavigateUrl="/manageproject/searchproject">Search Project</asp:HyperLink></td>
                <td class="menubarItems"><asp:HyperLink ID="userTimesheet" runat="server" NavigateUrl="#">User Timesheet</asp:HyperLink></td>
                <td class="menubarItems"><asp:HyperLink ID="approveTimesheet" runat="server" NavigateUrl="/Admin/RedirectToAspx">Approve Timesheet</asp:HyperLink></td>
            </tr>
        </table>
    </section>
    <br />
    <section class="ui-accordion-content">
        <div >
        <form id="form1" runat="server">
            <div>
                <table class="fieldstable">  
                            <tr>  
                                <td class="fields">Select Company:</td>  
                                <td class="fieldsValue">  
                                    <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"> </asp:DropDownList>  
<%--                                     <asp:RequiredFieldValidator ID="validateDDLCompany" runat="server" ControlToValidate="ddlCompany" ErrorMessage="*Required" ForeColor="Red" Font-Bold="true" Font-Size="X-Small"></asp:RequiredFieldValidator>--%>

                                </td> 
                               <%--<td></td>--%>
                                <td class="fields">Select Resource:</td>  
                                <td class="fieldsValue"> 
                                    <asp:DropDownList ID="ddlResource" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlResource_SelectedIndexChanged"> </asp:DropDownList>  
<%--                                    <asp:RequiredFieldValidator ID="validateDDLResource" runat="server" ControlToValidate="ddlResource" ErrorMessage="*Required" ForeColor="Red" Font-Bold="true" Font-Size="X-Small"></asp:RequiredFieldValidator>--%>
                                </td>
                                <%--<td></td>--%>
                                <td class="fields">Timesheet Status:</td>  
                                <td class="fieldsValue"> 
                                    <asp:DropDownList ID="ddlTimesheetStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTimesheetStatus_SelectedIndexChanged">
                                        <asp:ListItem Text="Approved"></asp:ListItem>
                                        <asp:ListItem Text="Rejected"></asp:ListItem>
                                        <asp:ListItem Text="Submitted"></asp:ListItem>
                                    </asp:DropDownList>  
                                </td>
                                <%--<td></td>--%>
                                <td class="fields">Remarks</td> 
                                <td class="fieldsValue">
                                    <asp:TextBox ID="remarks" runat="server"></asp:TextBox>
                                </td>
                                <%--<td class="fields">Select From Date:</td>  
                                <td> 
                                    <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                                </td> 
                                <td class="fields">Select To Date:</td>  
                                <td> 
                                    <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                                </td>--%>
                            </tr>
                           <%-- <tr>  
                                <td>Select Resource:</td>  
                                <td> 
                                    <asp:DropDownList ID="ddlResource" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlResource_SelectedIndexChanged"> </asp:DropDownList>  
                                </td>  
                            </tr> --%>
                            <tr>  
                                <td class="fields">Select From Date:</td>  
                                <td class="fieldsValue"> 
                                    <asp:TextBox ID="txtFromDate" runat="server" placeholder="*required"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="validateFromDate" runat="server" ControlToValidate="txtFromDate" ErrorMessage="*Required" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                </td> 
                                <td class="fields">Select To Date:</td>  
                                <td class="fieldsValue"> 
                                    <asp:TextBox ID="txtToDate" runat="server" placeholder="*required"></asp:TextBox>
<%--                                    <asp:RequiredFieldValidator ID="validateToDate" runat="server" ControlToValidate="txtToDate" ForeColor="Red" Font-Size="X-Small"></asp:RequiredFieldValidator>--%>
                                </td>  
                                <td class="fieldsValue">
                                     <asp:Button ID="searchBtn" runat="server" Text="Search" BorderStyle="Solid" ToolTip="search" OnClick="searchBtnClick" />
                                </td>
                            </tr>
                   <%-- <tr>  
                                <td>Timesheet Status:</td>  
                                <td> 
                                    <asp:DropDownList ID="ddlTimesheetStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTimesheetStatus_SelectedIndexChanged">
                                        <asp:ListItem Text="Approved"></asp:ListItem>
                                        <asp:ListItem Text="Rejected"></asp:ListItem>
                                        <asp:ListItem Text="Submitted"></asp:ListItem>
                                    </asp:DropDownList>  
                                </td>  
                            </tr>--%>
                    <%--<tr>
                        <td>Remarks</td> 
                        <td>
                        <asp:TextBox ID="remarks" runat="server"></asp:TextBox>
                        </td>
                    </tr>--%>
                    <%--<tr>
                        <td>
                        <asp:Button ID="searchBtn" runat="server" Text="Search" BorderStyle="Solid" ToolTip="search" OnClick="searchBtnClick" />
                        </td>
                    </tr>--%>
                    </table>
                <br />
                        <asp:GridView ID="dataGrid" runat="server" AutoGenerateColumns="False" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" OnSelectedIndexChanged="dataGrid_SelectedIndexChanged" >
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this)" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox" runat="server" onclick="checkThis(this);" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="projectId" HeaderText="Project Id">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="projectName" HeaderText="Project Name">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CRNumber" HeaderText="CR Number">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="description" HeaderText="Project Description">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="reportDate" HeaderText="Report Date">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="timesheetRemark" HeaderText="Timesheet Remark">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="resourceName" HeaderText="Resource Name">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="fromTime" HeaderText="From Time">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="toTime" HeaderText="To Time">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="Totalmin" HeaderText="Total minutes">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="submittedOn" HeaderText="Submitted On">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="approvedOn" HeaderText="Approved On">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="approvedBy" HeaderText="Approved By">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="approvalRemark" HeaderText="Approval Remark">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="status" HeaderText="Timesheet Status">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <%--<asp:TemplateField HeaderText="Approve/Reject">
                                    <ItemTemplate>
                                        <asp:Button ID="btnApprove" Text="Approve"  runat="server" OnClick="btnApproveOnClick" /><asp:Button ID="btnReject" Text="Reject" runat="server" OnClick="btnRejectOnClick" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                            <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                            <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                            <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                            <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#FFF1D4" />
                            <SortedAscendingHeaderStyle BackColor="#B95C30" />
                            <SortedDescendingCellStyle BackColor="#F1E5CE" />
                            <SortedDescendingHeaderStyle BackColor="#93451F" />
                        </asp:GridView>
                            <asp:Label ID="label" runat="server"></asp:Label> <br />
                            <asp:Button ID="approveSelectedBtn" runat="server" Text="Approve  Selected" BorderStyle="Solid" ToolTip="approve" OnClick="approveSelectedBtnClick" OnClientClick="ConfirmApproval()" />
                            <asp:Button ID="rejectSelectedBtn" runat="server" Text="Reject  Selected" BorderStyle="Solid" ToolTip="reject" OnClick="rejectSelectedBtnClick" OnClientClick="ConfirmRejection()"/>

                            
                  
            </div>
        </form>
            </div>
        </section>
</body>
</html>


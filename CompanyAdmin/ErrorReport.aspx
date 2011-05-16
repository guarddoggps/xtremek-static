<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorReport.aspx.cs" Inherits="Widgets_ErrorReport" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error Log</title>
    <script src="../Js/DateTime.js" type="text/javascript"></script>
    <link href="../CSS/ErrorLog.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
       setTimeout('get2Day()',500);
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="startDate" runat="server" />
    <asp:HiddenField ID="endDate" runat="server" />
    <div class="formWrap">
        <div class="container_01">
               <div class="container_02">               
                <label>
                    Select Type
                </label>                
                <div>
                    <div class="radiobutton">
                        <asp:RadioButton ID="_rdSpeedingAlarm" runat="server" GroupName="ErrorLog" 
                            Checked="True" 
                            oncheckedchanged="_rdSpeedingAlarm_CheckedChanged" AutoPostBack="false" />
                    </div>
                    <div class="cell_label">
                        Speeding Alarm
                    </div>
                    <div class="radiobutton">
                        <asp:RadioButton ID="_rdGeofence" runat="server" GroupName="ErrorLog" 
                            oncheckedchanged="_rdGeofence_CheckedChanged" AutoPostBack="false" />
                    </div>
                    <div class="cell_label">
                       Geofence Alarm
                    </div>
                    <div class="radiobutton">
                        <asp:RadioButton ID="_rdGeocoding" runat="server" GroupName="ErrorLog" 
                            oncheckedchanged="_rdGeocoding_CheckedChanged" AutoPostBack="false" />
                    </div>
                    <div class="cell_label">
                        Geocoding
                    </div>
                </div>
             </div>
        </div>
        
        <div class="container_01 container_02">
               <label> 
                    Start Date
               </label>
               
               <div class="leftDiv">
                    <telerik:RadDateTimePicker ID="_dtStart" runat="server"></telerik:RadDateTimePicker>
               </div>
               
               <label> 
                     End Date
               </label>
               
               <div class="rightDiv">
                    <telerik:RadDateTimePicker Width="80%" ID="_dtEnd" runat="server"></telerik:RadDateTimePicker>
               </div>
                              
               <div>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Button ID="Button1" CssClass="formBtn" runat="server" Text="Search" 
                            onclick="_btnShow_Click" />   
                        </ContentTemplate>
                    </asp:UpdatePanel>
                     
               </div>
        </div>
        
        <div class="container_01 container_02">
        
                <div>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Button ID="_btn2Day" CssClass="divDayBtn" OnClientClick="get2Day();" 
                             runat="server" Text="ToDay" onclick="_btn2Day_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>                    
                </div>
                
                <div>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                               <asp:Button ID="_btnYesterDay" CssClass="divDayBtn" 
                                OnClientClick="getLastDay();" runat="server" Text="YesterDay" 
                                onclick="_btnYesterDay_Click" />     
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                </div>
                
                <div>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Button ID="_btnLastWeek" CssClass="divDayBtn" 
                            OnClientClick="getLastWeek();" runat="server" Text="Last Week" 
                            onclick="_btnLastWeek_Click" />    
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                </div>
                
                <div>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Button ID="_btnLastMonth" CssClass="divDayBtn" 
                            OnClientClick="getLastMonth();" runat="server" Text="Last Month" 
                            onclick="_btnLastMonth_Click" />    
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                </div>
        </div>
        
        <div class="formGrid">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:DataGrid ID="_grdErrorLog" PageSize="20" AllowPaging="true" runat="server" AutoGenerateColumns="false">
                        <PagerStyle HorizontalAlign="Right" Mode="NumericPages" />
                        <Columns>
                            <asp:TemplateColumn HeaderText="Error Message">
                                <ItemTemplate>
                                        <%# Eval("errorMessage") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Log Time">
                                <ItemTemplate>
                                        <%# Eval("errorTime") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Method Name">
                                <ItemTemplate>
                                        <%# Eval("methodName") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
                
        </div>
        
    </div>
    <script type="text/javascript">
           function getLastMonth()
            {
                var dateCntl1=$find("<%=_dtStart.ClientID%>");
                var dateCntl2=$find("<%=_dtEnd.ClientID%>");
                var sDate=document.getElementById("startDate");
                var eDate=document.getElementById("endDate");
                var date=new Date()
                var hr=date.getHours()
                var DateTime=getDate(1,"",hr);
                var DateTime1=getDate("","","");
                var dt1=new Date(DateTime);
                var dt2=new Date(DateTime1);
                dateCntl1.set_selectedDate(dt1);
                dateCntl2.set_selectedDate(dt2);
                sDate.value=DateTime;
                eDate.value=DateTime1;           
              
            }
           function getLastWeek()
            {
            var dateCntl1=$find("<%=_dtStart.ClientID%>");
            var dateCntl2=$find("<%=_dtEnd.ClientID%>");
            var sDate=document.getElementById("startDate");
            var eDate=document.getElementById("endDate");
            var date=new Date()
            var hr=date.getHours()
            var DateTime=getDate("",7,hr);
            var DateTime1=getDate("","","");
            var dt1=new Date(DateTime);
            var dt2=new Date(DateTime1);
            dateCntl1.set_selectedDate(dt1);
            dateCntl2.set_selectedDate(dt2);
            sDate.value=DateTime;
            eDate.value=DateTime1;   
           
        }
        function getLastDay()
        {
            var dateCntl1=$find("<%=_dtStart.ClientID%>");
            var dateCntl2=$find("<%=_dtEnd.ClientID%>");
            var sDate=document.getElementById("startDate");
            var eDate=document.getElementById("endDate");
            var date=new Date()
            var hr=date.getHours()
            var DateTime=getDate("",1,hr);
            var DateTime1=getDate("",1,-hr);
            var dt1=new Date(DateTime);
            var dt2=new Date(DateTime1);
            dateCntl1.set_selectedDate(dt1);
            dateCntl2.set_selectedDate(dt2);
            sDate.value=DateTime;
            eDate.value=DateTime1;
           
           
        }
       function get2Day()
        {
            var dateCntl1=$find("<%= _dtStart.ClientID%>");
            var dateCntl2=$find("<%= _dtEnd.ClientID%>"); 
            
            var sDate=document.getElementById("startDate");
            var eDate=document.getElementById("endDate");
            var date=new Date()
            var hr=date.getHours()
            var DateTime=getDate("","",hr);
            var DateTime1=getDate("","","");
            var dt1=new Date(DateTime);
            var dt2=new Date(DateTime1);
            dateCntl1.set_selectedDate(dt1);
            dateCntl2.set_selectedDate(dt2);
            sDate.value=DateTime;
            eDate.value=DateTime1;
            
            
        }        
        
        
    </script>
    </form>
</body>
</html>

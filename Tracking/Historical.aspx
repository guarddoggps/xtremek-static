<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Historical.aspx.cs" Inherits="Tracking_Historical" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Historical</title>
    <link href="../CSS/HistoricalPage.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/ajaxstyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css"> 
		/* Style classes for Sample5 */
		.HistoricalUpdateProgress
		{  
			margin: 0 auto;
			/*background-color:#330099;*/
			/*background-color:#7A8486;*/
			color:#fff;
			width: 150px;
			text-align: center;
			vertical-align: middle;
			position: fixed;
			top: 40%;
			left: 35%;
		}
	</style>
	
    <script src="../Js/DateTime.js" type="text/javascript"></script>
    
    <script type="text/javascript">
	    function load() {
	    	try { 
	        	Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);    
	       	} catch(E) {
	           
	        }
	  	}   
	  	
		function setHistoricalMap() {       
			var hV = document.getElementById('_hHasValue');
			                           
			if(hV == null || hV.value != '') {
			 	window.parent.SetHistoricalMap();    
			}
			            
		}
			    
		function highlightRow(row) {
	        var index=row.id; 
	       
	    	window.parent.MarkerShow(index);
	  	}
	      
	    function EndRequestHandler(sender, args) {
	     	setHistoricalMap();
	    } 
	    
	    function PrintPage()
		{
			 var printContent = document.getElementById("recordsDiv");
			 var WinPrint = window.open('','','left=0,top=0,width=1,height=1,toolbar=0,status=0');
			 WinPrint.document.write(printContent.innerHTML);
			 WinPrint.document.close();
			 WinPrint.focus();
			 WinPrint.print();
			 WinPrint.close();
			 //prtContent.innerHTML=strOldOne;
		}

    </script>
    
</head>
<body onload="load();">
    <form id="form1" runat="server">
     	<asp:ScriptManager runat="server" ID="_ScriptManager" AsyncPostBackTimeout="300"></asp:ScriptManager>
            
        <asp:UpdateProgress ID="siteUpdateProgress" DisplayAfter="50"  runat="server">
            <ProgressTemplate>
                
                 <div class="TransparentGrayBackground"></div>
                
                 <div class="HistoricalUpdateProgress">
                 <asp:Image  ID="ajaxLoadNotificationImage" 
                                    runat="server" 
                                    ImageUrl="../Images/loading.gif" 
                                    AlternateText="[image]" />
                       
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        
       	<div class="container">
       	
	  		<div class="body">
		        <div style="float: left; margin: 10px; width: 480px;">
	        		<asp:UpdatePanel runat="server" ID="topUpdatePanel">
	        			<ContentTemplate>
       						<asp:HiddenField runat="server" ID="_hHasValue" />
       							
		        			<div style="float: left; margin-right: 10px;">
	        			    	<asp:ImageButton ID="_imgBtn2Day" runat="server" OnClientClick="getToday();" ImageUrl="~/Images/Button/Today.gif" OnClick="_btn_Click" />
	        			        <asp:ImageButton ID="_imgBtnYesterday" runat="server"  OnClientClick="getYesterday();" ImageUrl="~/Images/Button/Yesterday.gif" OnClick="_btn_Click" />
	        			   		
			        		</div> 
			        		
		        			<div style="float: left; padding-top: 4px;">
					        	<telerik:RadDatePicker runat="server" 
					        						   ID="_historyDate" 
					        						   CssClass="datebox" 
					        						   OnSelectedDateChanged="_historyDate_SelectedDateChanged" 
					        						   AutoPostBack="true">
					      		</telerik:RadDatePicker>        			
	        				</div>
			        	</ContentTemplate>
	        		</asp:UpdatePanel>	
			        
		        	<div style="float: left; padding-top: 8px; padding-left: 6px;">
						<asp:LinkButton Text="Print Page" OnClientClick="PrintPage();" runat="server" />
					</div>
	 			</div>
	        </div>
	       
	       
	       <div ID="recordsDiv" class="pagebody">
	      		<asp:UpdatePanel ID="gridUpdatePanel" runat="server">
	                <ContentTemplate>	                    
	                    <asp:DataGrid ID="_grdRecords" 
	                    			  runat="server" 
	                    			  PageSize="25" 
	                    			  Width="100%" 
	                    			  AutoGenerateColumns="false" 
	                    			  AllowPaging="true" 
	                    			  OnPageIndexChanged="_grdHistorical_Paging" 
	                    			  OnItemDataBound="_gridRecords_DataBound">
                                
                      		<PagerStyle Mode="NumericPages" HorizontalAlign="Right"/>
                            <HeaderStyle CssClass="Header" />
                            <PagerStyle CssClass="footer" />                    
                            <SelectedItemStyle CssClass="gridselected" />
                                
                            <Columns>
                            	<asp:TemplateColumn HeaderText="#" 
                            						ItemStyle-CssClass="HisCol" 
                            						ItemStyle-HorizontalAlign="center">
                                	<ItemTemplate>
                                    	<%# serialNumber()%>
                                    </ItemTemplate>
                          		</asp:TemplateColumn>
                                    
                                <asp:BoundColumn DataField="location" 
                                			   	 HeaderText="Location" 
                                			   	 ItemStyle-CssClass="LocCol" 
                                			   	 ItemStyle-HorizontalAlign="center">
                               	</asp:BoundColumn>                                   
  
                                <asp:BoundColumn DataField="velocity" 
                                				 HeaderText="Vel." 
                                				 ItemStyle-CssClass="HisCol" 
                                				 ItemStyle-HorizontalAlign="center">
                                </asp:BoundColumn>                                   

                                <asp:BoundColumn DataField="distance" 
                                				 DataFormatString="{0:N2}" 
                                				 HeaderText="Dist."
                                				 ItemStyle-CssClass="HisCol" 
												 ItemStyle-HorizontalAlign="center">
								</asp:BoundColumn>
                                    
                                <asp:BoundColumn DataField="recDateTime" 
                                				 DataFormatString="{0:G}" 
                                				 HeaderText="Date and Time"
                                				 ItemStyle-CssClass="HisCol" 
												 ItemStyle-HorizontalAlign="center">
								</asp:BoundColumn>
														
                                <asp:BoundColumn DataField="alert" 
                                				 HeaderText="Alert" 
                                				 ItemStyle-CssClass="HisCol" 
                                				 ItemStyle-HorizontalAlign="center">
                                </asp:BoundColumn>                                   
                                     
                       		</Columns>
                    	</asp:DataGrid>
	                </ContentTemplate>
	            </asp:UpdatePanel>
	        </div>  
	        
	    	<div>
		    	<asp:UpdatePanel ID="errorMsgUpdatePanel" runat="server">
	           		<ContentTemplate>
	           			<div class="ErrorMessage">
	                     	<asp:Label ID="_lblMessage" runat="server" Text=""></asp:Label>    
	        			</div>
	                </ContentTemplate>
	            </asp:UpdatePanel>
		  	</div>
    	</div>
     
	<script type="text/javascript">
           
    	function getToday() {
            var dateControl = $find("<%= _historyDate.ClientID %>");
            
            var date = new Date(Date.parse(getDate('', '', '')));
            dateControl.set_selectedDate(date);         
        }
        
        function getYesterday() {
            var dateControl = $find("<%= _historyDate.ClientID %>");
            
            var date = new Date(Date.parse(getDate('', 1, '')));
            dateControl.set_selectedDate(date);
        }
    </script>
    
    </form>
</body>
</html>

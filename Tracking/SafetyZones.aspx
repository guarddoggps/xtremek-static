<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SafetyZones.aspx.cs" Inherits="Tracking_SafetyZones" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Safety Zones</title>
    <link href="../CSS/SafetyzoneGrid.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <div>
                <asp:LinkButton CssClass="button" ID="_lnkNewZone" runat="server" Text="Create New Zone" 
                    onclick="_lnkNewZone_Click"></asp:LinkButton>
            </div>
            <div>
                <asp:DataGrid ID="_grdZones" runat="server" AutoGenerateColumns="false" 
                    onitemcommand="_grdZones_ItemCommand" 
                    onselectedindexchanged="_grdZones_SelectedIndexChanged" 
                    OnItemDataBound="_grdZone_DataBound">
                    <HeaderStyle CssClass="Header" />
                    <Columns>
                    
                        <asp:BoundColumn DataField="isActive"   Visible="false"></asp:BoundColumn>
                        
                        <asp:TemplateColumn Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="_lblGeofenceID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"geofenceID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderText="Zone Name" ItemStyle-CssClass="SFGCol1">
                            <ItemTemplate>
                                <asp:Label ID="_lblZoneName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'  ></asp:Label>                              
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderText="" ItemStyle-CssClass="SFGCol2">
                            <ItemTemplate>
                                <asp:LinkButton Width="50px" ID="_lnkEdit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderText="" ItemStyle-CssClass="SFGCol3">
                            <ItemTemplate>
                                <asp:LinkButton Width="50px" ID="_lnkDelete" runat="server" Text="Delete" CommandName="Delete"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderText="Zone Status" ItemStyle-CssClass="SFGCol4">
                            <ItemTemplate>
                                <asp:Label ID="_lblStatus" Width="90px"  runat="server" ></asp:Label>                                
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                    </Columns>
                </asp:DataGrid>
            </div>
            
            <div>
                <asp:Label CssClass="lblmessage" ID="_lblMessage" runat="server"></asp:Label>
            </div>
    </div>
    </form>
</body>
</html>

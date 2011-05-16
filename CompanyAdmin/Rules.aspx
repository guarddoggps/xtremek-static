<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Rules.aspx.cs" Inherits="CompanyAdmin_Rules" %>

<%@ Register src="ucSpeeding.ascx" tagname="ucSpeeding" tagprefix="uc1" %>

<%@ Register src="AssinRules.ascx" tagname="AssinRules" tagprefix="uc2" %>

<%@ Register src="EditRules.ascx" tagname="EditRules" tagprefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rules Setup</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="Manager" runat="server"></asp:ScriptManager>
    
    <div>
    
          <asp:Menu ID="RulesMenu" runat="server"  BorderWidth="0" 
              Orientation="Horizontal" onmenuitemclick="RulesMenu_MenuItemClick" 
                >
                <Items>
                    <asp:MenuItem ImageUrl="~/Images/RSetup_over.gif" 
                                  Text="" Value="0" ></asp:MenuItem>
                    <asp:MenuItem ImageUrl="~/Images/AssignRules_normal.gif" 
                                  Text="" Value="1"></asp:MenuItem>
                    <asp:MenuItem ImageUrl="~/Images/EditRules_normal.gif" 
                                  Text="" Value="2"></asp:MenuItem>
                                    
                </Items>
            </asp:Menu>
            
            <asp:MultiView id="_mltView" runat="server" ActiveViewIndex="0">
                <asp:View runat="server">
                    <uc1:ucSpeeding ID="ucSpeeding1" runat="server" />        
                </asp:View>            
                
                <asp:View runat="server">
                     <uc2:AssinRules ID="AssinRules1" runat="server" />
                </asp:View>
                
                <asp:View runat="server">                
                    <uc3:EditRules ID="EditRules1" runat="server" />                
                </asp:View>
                
            </asp:MultiView>
    
        
    
    </div>
    </form>
</body>
</html>

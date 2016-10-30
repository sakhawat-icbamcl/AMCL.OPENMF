﻿<%@ Page Language="C#" MasterPageFile="~/UI/UnitCommon.master" AutoEventWireup="true" CodeFile="UnitReportDividendWarrant.aspx.cs" Inherits="UI_UnitReportDividendWarrant" Title=" Dividend Warrant Report Form (Design and Developed by Sakhawat)" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script language="javascript" type="text/javascript"> 
    
     
function fnOnChangeText(textObject)
	{
	    
	    if(textObject.id.indexOf("fromWar_NoTextBox")!=-1)
	    {
	        document.getElementById("<%=toWar_NoTextBox.ClientID%>").value =document.getElementById("<%=fromWar_NoTextBox.ClientID%>").value ;
	        document.getElementById("<%=toWar_NoTextBox.ClientID%>").focus();
	    }
	    else  if(textObject.id.indexOf("fromRegNoTextBox")!=-1)
	    {
	        document.getElementById("<%=toRegNoTextBox.ClientID%>").value =document.getElementById("<%=fromRegNoTextBox.ClientID%>").value ;
	        document.getElementById("<%=toRegNoTextBox.ClientID%>").focus();
	    }
	  
	}
    
    function fncInputNumericValuesOnly()
	{
		if(!(event.keyCode==48||event.keyCode==49||event.keyCode==50||event.keyCode==51||event.keyCode==52||event.keyCode==53||event.keyCode==54||event.keyCode==55||event.keyCode==56||event.keyCode==57))
		{
			alert("Please Enter Numaric Value Only");
			    event.returnValue=false;
		}
	}
	
	function  fnReset()
	{
	   
	    
	     return false;
	}
	function fnCheqInput()
	{
	 
	     if(document.getElementById("<%=fundNameDropDownList.ClientID%>").value =="0")
            {
                document.getElementById("<%=fundNameDropDownList.ClientID%>").focus();
                alert("Please Select Fund Name");
                return false;
                
            }
           if(document.getElementById("<%=DividendFYDropDownList.ClientID%>").value =="--Select FY--")
            {               
              
                document.getElementById("<%=DividendFYDropDownList.ClientID%>").focus();
                alert("Please Select Fiscal Year");
                return false;
               
            }
            if(document.getElementById("<%=ClosingDateDropDownList.ClientID%>").value =="")
            {                            
                document.getElementById("<%=ClosingDateDropDownList.ClientID%>").focus();
                alert("Please Select Closing Date");
                return false;
            }
          
	    
	}
 </script>
   <style type="text/css">
       
        .style8
        {
            font-size: small;
            font-weight: bold;
            text-align: right;
        }
      
        .style10
        {
            font-size: small;
            font-weight: bold;
        }
    </style>
    
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />               
    
<br />
<br />
 <table align="center">
        <tr>
            <td class="FormTitle" align="center">
                Dividend Warrant Print Form</td>           
            <td>
                <br />
            </td>
        </tr> 
      </table>
<br />
    <br />
  <asp:UpdatePanel ID="holderInfoUpdatePanel" runat="server">
    <ContentTemplate>  
<table width="600" align="center" cellpadding="0" cellspacing="0" >
<colgroup width="190"></colgroup>
<colgroup width="75"></colgroup>
<colgroup width="100"></colgroup>
<colgroup width="75"></colgroup>

    <tr   class="style5">
        <td class="" ><b style="font-size: small; text-align: right;">&nbsp;&nbsp;Dividend Category :</b></td>
        <td align="left"   colspan="4">
            &nbsp;
            <asp:RadioButton ID="NormalRadioButton" runat="server" Checked="True" 
                GroupName="DividendCategory" style="font-size: small; font-weight: 700;" 
                Text="Normal" TabIndex="1" /> &nbsp;&nbsp;
            <asp:RadioButton ID="IDAccountRadioButton" runat="server" Font-Bold="True" 
                style="font-size: small" Text="ID Account"  GroupName="DividendCategory" 
                TabIndex="1" />
            &nbsp;<asp:RadioButton ID="InerimRadioButton" runat="server" Checked="True" 
                GroupName="DividendCategory" style="font-size: small; font-weight: 700;" 
                TabIndex="1" Text="Inerim" />
        </td>
        
    </tr>
     <tr   class="style5">
        <td c><b style="font-size: small; text-align: right;">&nbsp;&nbsp;Dividend Type :</b></td>
        <td align="left"   colspan="4">
            &nbsp;
            <asp:CheckBox ID="DuplicateCheckBox" runat="server"  Text="Duplicate" 
                style="font-weight: 700; font-size: small" TabIndex="1"/>
            &nbsp;&nbsp;
            
        </td>
        
    </tr>
     <tr >
        <td style="font-size: small"  ><b>Fund Name :</b></td>
        
        <td align="left"   colspan="4"> &nbsp;&nbsp;          
            <asp:DropDownList ID="fundNameDropDownList" runat="server" 
                    TabIndex="2" AutoPostBack="True" 
                onselectedindexchanged="fundNameDropDownList_SelectedIndexChanged"></asp:DropDownList>
            <span class="star">*</span></td>
       
    </tr>
     <tr >
        <td align="left" class="style10" style="text-align: right"  >Branch Name :</td>
        
        <td align="left"   colspan="4"> &nbsp;&nbsp;          
            <asp:DropDownList ID="branchNameDropDownList" runat="server" 
                    TabIndex="3"></asp:DropDownList>
        </td>
       
    </tr>
    <tr class="style5">
       <td class="style8"  >
           Fiscal Year :
        </td>
        
        <td align="left" colspan="4">&nbsp;
            <asp:DropDownList ID="DividendFYDropDownList" runat="server" 
                    TabIndex="4" AutoPostBack="True" 
                onselectedindexchanged="DividendFYDropDownList_SelectedIndexChanged"></asp:DropDownList>
            <span class="star">*</span></td>
         
      
    </tr>
     <tr class="style5">
       <td class="style8"  >
           Closing Date :
        </td>
        
        <td align="left" colspan="4">&nbsp;
            <asp:DropDownList ID="ClosingDateDropDownList" runat="server" 
                    TabIndex="5"></asp:DropDownList><span class="star">*</span></td>
        
      
    </tr>
     <tr class="style5">
       <td class="style8"  >
          Warrant No :
        </td>
        
        <td align="left" colspan="4">&nbsp;
        <asp:TextBox ID="fromWar_NoTextBox" runat="server" onBlur="Javascript:fnOnChangeText(this);"
                CssClass= "TextInputStyleSmall" Width="100px" TabIndex="6" 
                onkeypress= "fncInputNumericValuesOnly()"></asp:TextBox> &nbsp;<span 
                class="style10">To
        <asp:TextBox ID="toWar_NoTextBox" runat="server" 
                CssClass= "TextInputStyleSmall" Width="100px" TabIndex="7" 
                onkeypress= "fncInputNumericValuesOnly()"></asp:TextBox> </span></td>
        
      
    </tr>
     <tr class="style5">
       <td class="style8"  >
         Registration No :
        </td>
        
        <td align="left" colspan="4">&nbsp;
        <asp:TextBox ID="fromRegNoTextBox" runat="server" onBlur="Javascript:fnOnChangeText(this);"
                CssClass= "TextInputStyleSmall" Width="100px" TabIndex="9" 
                onkeypress= "fncInputNumericValuesOnly()"></asp:TextBox> &nbsp;<span 
                class="style10">To
        <asp:TextBox ID="toRegNoTextBox" runat="server" 
                CssClass= "TextInputStyleSmall" Width="100px" TabIndex="5" 
                onkeypress= "fncInputNumericValuesOnly()"></asp:TextBox> </span></td>
        
      
    </tr>
    
    
  
    <tr>
        <td align="center" colspan="5">&nbsp;</td>
    </tr>
    <tr>
        <td align="center" colspan="5">&nbsp;</td>
    </tr>
    <tr>
        <td align="center" colspan="5">&nbsp;</td>
    </tr>
</table>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="fundNameDropDownList" EventName="SelectedIndexChanged" />
    <asp:AsyncPostBackTrigger ControlID="DividendFYDropDownList" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
<table align="center" cellpadding="0" cellspacing="0" style="width: 338px">
     <tr>
        <td align="right">
        <asp:Button ID="PrintButton" runat="server" Text="Print" CssClass="buttoncommon" OnClientClick="return fnCheqInput();"
                 AccessKey="p" onclick="PrintButton_Click" TabIndex="10" />&nbsp;
        </td>
        <td align="left">&nbsp;&nbsp;&nbsp;
        <asp:Button ID="ResetButton" runat="server" Text="Reset" 
                CssClass="buttoncommon"  OnClientClick="return fnReset();" AccessKey="r" 
                meta:resourcekey="ResetButtonResource2" />&nbsp;&nbsp;&nbsp;
        <asp:Button ID="CloseButton" runat="server" Text="Close" 
                CssClass="buttoncommon"  onclick="CloseButton_Click" AccessKey="c" meta:resourcekey="CloseButtonResource1" 
                  />
        </td>
        <td>
        &nbsp;
        </td>
    </tr>
   </table>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
</asp:Content>


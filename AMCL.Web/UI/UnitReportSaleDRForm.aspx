<%@ Page Language="C#" MasterPageFile="~/UI/UnitCommon.master" AutoEventWireup="true" CodeFile="UnitReportSaleDRForm.aspx.cs" Inherits="UI_UnitReportSaleDRForm" Title=" Sale Demat Request Report Form (Design and Developed by Sakhawat)" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script language="javascript" type="text/javascript"> 
   function fnReset()
   {
       
       
        
        document.getElementById("<%=fromSaleNoTextBox.ClientID%>").value ="";
        document.getElementById("<%=toSaleNoTextBox.ClientID%>").value ="";
        return false;
   }
  
 function fncInputNumericValuesOnly()
	{
		if(!(event.keyCode==48||event.keyCode==49||event.keyCode==50||event.keyCode==51||event.keyCode==52||event.keyCode==53||event.keyCode==54||event.keyCode==55||event.keyCode==56||event.keyCode==57))
		{
			alert("Please Enter Numaric Value Only");
			    event.returnValue=false;
		}
	}
	function fnOnChangeText1()
     {                                
         document.getElementById("<%=toSaleNoTextBox.ClientID%>").value=document.getElementById("<%=fromSaleNoTextBox.ClientID%>").value;                                                                           
          document.getElementById("<%=toSaleNoTextBox.ClientID%>").focus();
     }
    
     
</script>



    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />        
      
      <br />
      <table align="left" cellpadding="0" cellspacing="0" 
        style="width: 664px; height: 193px;">
      <colgroup width="120"></colgroup>
      <tr>
            <td class="FormTitle" align="center"  colspan="3">
           Unit Sale Demat Request Report Form (<span id="spanFundName" runat="server"></span>)
            </td>           
           
        </tr> 
      <tr>
        <td align="left" colspan="2">
         &nbsp;&nbsp;
        </td>
        
      </tr>
      <tr>
        <td align="left">
         Fund Code:
        </td>
        <td align="left" colspan="2">
        <asp:TextBox ID="fundCodeTextBox" runat="server" 
                CssClass= "TextInputStyleSmall" Enabled="False" Width="80px" TabIndex="1"></asp:TextBox>
        </td>
      </tr>
      <tr>
        <td align="left">
         Branch Code:
        </td>
        <td align="left" colspan="2">
        <asp:TextBox ID="branchCodeTextBox" runat="server" 
                CssClass= "TextInputStyleSmall" Enabled="False" Width="80px" TabIndex="2"></asp:TextBox>
        </td>
      </tr>
       <tr>
        <td align="left">
            Sale No:</td>
        <td align="left"  >
        <asp:TextBox ID="fromSaleNoTextBox" runat="server" onBlur="Javascript:fnOnChangeText1();"
                CssClass= "TextInputStyleSmall" Width="100px" TabIndex="3" onkeypress= "fncInputNumericValuesOnly()"></asp:TextBox> &nbsp;<b><span style="font-weight:bold; height:100px;">&nbsp; To</span></b>&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="toSaleNoTextBox" runat="server" 
                CssClass= "TextInputStyleSmall" Width="100px" TabIndex="4" onkeypress= "fncInputNumericValuesOnly()"></asp:TextBox> 
                
                
        &nbsp;(MA</td>
          &nbsp;</tr>
      <%-- <tr>
        <td align="left">
         Registration No:</td>
        <td align="left" colspan="2">
        <asp:TextBox ID="fromRegNoTextBox" runat="server" onBlur="Javascript:fnOnChangeText2();"
                CssClass= "TextInputStyleSmall" Width="100px" TabIndex="13" onkeypress= "fncInputNumericValuesOnly()"></asp:TextBox> &nbsp;<b><span style="font-weight:bold; height:100px;">&nbsp; To</span></b>&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="toRegNoTextBox" runat="server" 
                CssClass= "TextInputStyleSmall" Width="100px" TabIndex="14" onkeypress= "fncInputNumericValuesOnly()"></asp:TextBox>
        </td>
      </tr>
      <tr>
        <td align="left">
            Sale Date:</td>
        <td align="left" colspan="2">
        <asp:TextBox ID="fromSaleDateTextBox" runat="server" CssClass="textInputStyleDate" onBlur="Javascript:fnOnChangeText3();"
                TabIndex="15"></asp:TextBox>
            <ajaxToolkit:CalendarExtender ID="fromRegDatecalendarButtonExtender" 
                runat="server" TargetControlID="fromSaleDateTextBox" 
                PopupButtonID="fromRegDateImageButton" Format="dd-MMM-yyyy"/>
            <asp:ImageButton ID="fromRegDateImageButton" runat="server" 
                AlternateText="Click Here" ImageUrl="~/Image/Calendar_scheduleHS.png" 
                TabIndex="7" />
            <b><span style="font-weight:bold; height:100px;">&nbsp;&nbsp; To</span></b>&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="toSaleDateTextBox" runat="server" CssClass="textInputStyleDate" 
                TabIndex="16"></asp:TextBox>
            <ajaxToolkit:CalendarExtender ID="toRegDatecalendarButtonExtender" 
                runat="server" TargetControlID="toSaleDateTextBox" 
                PopupButtonID="toRegDateImageButton" Format="dd-MMM-yyyy"/>
            <asp:ImageButton ID="toRegDateImageButton" runat="server" 
                AlternateText="Click Here" ImageUrl="~/Image/Calendar_scheduleHS.png" 
                TabIndex="9" />
          
        </td>
      </tr>--%>
      <tr>
        <td align="left" colspan="2">
         &nbsp;&nbsp;
        </td>
        
      </tr>
      <tr>
        <td align="right">
        <asp:Button ID="ShowReportButton" runat="server" Text="View Repoert" CssClass="buttoncommon"
                AccessKey="V" onclick="ShowReportButton_Click"/>&nbsp;
        </td>
        <td align="left">&nbsp;&nbsp;&nbsp;
        <asp:Button ID="regResetButton" runat="server" Text="Reset" 
                CssClass="buttoncommon"  OnClientClick="return fnReset();" AccessKey="r" />&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="regCloseButton" runat="server" Text="Close" 
                CssClass="buttoncommon"  onclick="regCloseButton_Click" 
                  />
        &nbsp;&nbsp;
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
</asp:Content>


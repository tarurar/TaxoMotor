<%@ Control Language="C#" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 

<SharePoint:ScriptLink runat="server" ID="ScriptLink2" Localizable="False" OnDemand="False" Name="TM.SP.Customizations/Scripts/json-to-table.js"></SharePoint:ScriptLink>
<script type="text/javascript" src="/ProjectScripts/tm.sp.js?rev=<%=DateTime.Now.ToString("ddMMyyyy")%>"></script>

<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendRequestEGRULPage.aspx.cs" Inherits="TM.SP.AppPages.SendRequestEGRULPage" DynamicMasterPageFile="~masterurl/default.master" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/15/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/15/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/15/ButtonSection.ascx" %>

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="Page Title" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="Page Title" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription"  runat="server">
    <SharePoint:EncodedLiteral runat="server" text="Page Description" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div id="RequestList">
        <p><%=GetLocalizedString(resRequestListHeader)%></p>
        <div id="RequestListTableDiv">
            <table id="RequestListTable" style="width: 100%;">
                <thead>
                    <tr>
                        <td><%=GetLocalizedString(resRequestListTableHeader1)%></td>
                        <td><%=GetLocalizedString(resRequestListTableHeader2)%></td>
                    </tr>
                </thead>
                <tbody>

                </tbody>
            </table>
            <asp:Label runat="server" ID="TestLabel"></asp:Label>
        </div>
    </div>

    <div id="PrecautionMessage">
        <p><%=GetLocalizedString(resPrecautionMessage)%></p>
    </div>

    <div id="NoRequestMessage">
        <p><%=GetLocalizedString(resNoRequestMessage)%></p>
    </div>

	<table id="maintable" border="0" cellspacing="0" cellpadding="0" class="ms-propertysheet" width="100%">
      <wssuc:ButtonSection runat="server" ShowStandardCancelButton="False">
        <Template_Buttons>
          <asp:placeholder ID="Placeholder1" runat="server">
            <asp:Button runat="server" class="ms-ButtonHeightWidth" ID="BtnOk" Text="Ok" />
            <SeparatorHtml>
                <span id="idSpace" class="ms-SpaceBetButtons" />
            </SeparatorHtml>
            <asp:Button runat="server" class="ms-ButtonHeightWidth" ID="BtnCancel" Text="Cancel" />           
          </asp:PlaceHolder>
        </Template_Buttons>
      </wssuc:ButtonSection>
    </table>     
	    
    <SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>


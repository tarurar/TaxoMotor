<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendRequestEGRIPPage.aspx.cs" Inherits="TM.SP.AppPages.SendRequestEGRIPPage" DynamicMasterPageFile="~masterurl/default.master" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/15/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/15/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/15/ButtonSection.ascx" %>

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="Page Title" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:CssRegistration ID="CssRegistration1" Name="TM.SP.AppPages/dialog-lists.css" runat="server" />
    <SharePoint:ScriptLink ID="jsSendRequestDialog" OnDemand="false" runat="server" Localizable="false" Name="TM.SP.AppPages/Scripts/SendRequestDialog.js"></SharePoint:ScriptLink>
    <SharePoint:ScriptBlock runat="server">
        var ShowEGRIPSendWaitingScreen = function(){
            var waitingMarkup = TM.SP.getProcessingMarkup('<%=GetLocalizedString(resProcessNotifyText) %>');

            $('#ListHeader p').html(waitingMarkup); 
            $('#<%= BtnOk.ClientID %>').attr('disabled', 'disabled'); 
            $('#<%= BtnCancel.ClientID %>').attr('disabled', 'disabled'); 
        };
    </SharePoint:ScriptBlock>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="Page Title" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription"  runat="server">
    <SharePoint:EncodedLiteral runat="server" text="Page Description" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

	<asp:Panel ID="RequestList" runat="server">
        <div id="ListHeader">
            <p><%=GetLocalizedString(resRequestListCaption)%></p>
        </div>
        <div id="RequestListTableDiv" class="request-list">
            <asp:Panel ID="RequestListTablePanel" runat="server">
            </asp:Panel>
        </div>
    </asp:Panel>

    <asp:Panel ID="ErrorList" runat="server">
        <div id="ErrorListHeader">
            <p><%=GetLocalizedString(resErrorListCaption)%></p>
        </div>
        <div id="ErrorListTableDiv" class="error-list">
            <asp:Panel ID="ErrorListTablePanel" runat="server">
            </asp:Panel>
        </div>
    </asp:Panel>

    <div id="PrecautionMessage" style="display: none">
        <p><%=GetLocalizedString(resPrecautionMessage)%></p>
    </div>

    <div id="NoRequestMessage" style="display: none">
        <p><%=GetLocalizedString(resNoRequestMessage)%></p>
    </div>

	<table id="maintable" border="0" cellspacing="0" cellpadding="0" class="ms-propertysheet" width="100%">
      <wssuc:ButtonSection runat="server" ShowStandardCancelButton="False">
        <Template_Buttons>
          <asp:placeholder ID="Placeholder1" runat="server">
            <asp:Button runat="server" class="ms-ButtonHeightWidth" ID="BtnOk" Text="Ok" OnClientClick="ShowEGRIPSendWaitingScreen();" UseSubmitBehavior="false"/>              
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


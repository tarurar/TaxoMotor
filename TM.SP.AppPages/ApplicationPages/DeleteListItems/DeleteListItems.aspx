<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeleteListItems.aspx.cs" Inherits="TM.SP.AppPages.DeleteListItems" DynamicMasterPageFile="~masterurl/default.master" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/15/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/15/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/15/ButtonSection.ascx" %>

<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:ScriptLink runat="server" Name="pickertreedialog.js" OnDemand="true"></SharePoint:ScriptLink>
    <script type="text/javascript">
        var tbSelectedListID = "<%= tbSelectedList.ClientID %>";

        function LaunchPicker() {
            var callback = function (selectedListData) {
                document.getElementById(tbSelectedListID).value = selectedListData[3];
            };

            var iconUrl = "/_layouts/15/images/smt_icon.gif?rev=23";
            var rootUrl = _spPageContextInfo.webAbsoluteUrl;
            SP.SOD.executeFunc('pickertreedialog.js', 'LaunchPickerTreeDialogSelectUrl', function () {
                LaunchPickerTreeDialogSelectUrl(
                    'CbqPickerSelectListTitle',
                    'CbqPickerSelectListText',
                    'websLists', '', rootUrl, '', '', '', iconUrl, '', callback, 'true', '');
            })
        }
    </script>
    <style type="text/css">
        .comment {
            padding: 20px;
            border: 1px solid black;
            margin-top: 20px;
            font-style: italic;
            background-color: antiquewhite;
        }
        .buttons {
            margin-top: 20px;
            margin-bottom: 20px;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="Удаление всех элементов списка" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="Удаление всех элементов списка" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription"  runat="server">
    <SharePoint:EncodedLiteral runat="server" text="Удаление всех элементов списка" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

	<table id="maintable" border="0" cellspacing="0" cellpadding="0" class="ms-propertysheet" width="100%">
      <tr>
          <td>
              <asp:Label runat="server" Text="Выберите список или библиотеку:"></asp:Label>
              <asp:TextBox runat="server" ID="tbSelectedList" Width="250"></asp:TextBox>
              <input type="submit" class="ms-input" value="Выбор" id="selectButton" onclick="LaunchPicker(); return false;" />
          </td>
      </tr>
      <tr>
          <td>
              <asp:CheckBox runat="server" ID="cbDeleteFolders" Text="Удалять папки" Checked="true" />
          </td>
      </tr>
    </table>  
    <div class="comment">
        <SharePoint:EncodedLiteral runat="server" ID="LiteralComment" Text="" EncodeMethod="HtmlEncode" />   
    </div>
    <div class="buttons">
        <asp:Button runat="server" CssClass="ms-ButtonHeightWidth" ID="BtnRun" Text="Запустить" OnClick="BtnRun_Click" />
        <asp:Button runat="server" CssClass="ms-ButtonHeightWidth" ID="BtnCancel" Text="Отмена" OnClick="BtnCancel_Click" />
    </div>
	    
    <SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>


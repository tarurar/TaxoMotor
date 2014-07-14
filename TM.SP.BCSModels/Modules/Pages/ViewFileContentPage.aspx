<%@ Page Language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:webpartpageexpansion="full" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <WebPartPages:WebPartZone runat="server" ID="Main" FrameType="TitleBarOnly">
        <ZoneTemplate>
        </ZoneTemplate>
    </WebPartPages:WebPartZone>
</asp:Content>

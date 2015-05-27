<%@ Page Language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %> <%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="AppPages" Namespace="TM.SP.AppPages" Assembly="TM.SP.AppPages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=63e92b7beac312db"%>

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	Фильтрация разрешений
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Фильтрация разрешений
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderSearchArea" runat="server">
	<SharePoint:DelegateControl runat="server"
		ControlId="SmallSearchInputBox"/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
    <WebPartPages:WebPartZone runat="server" ID="Main" FrameType="TitleBarOnly">
        <ZoneTemplate>
            <AppPages:LicenseFilterWebPart runat="server" View="FilterView" 
                CatalogIconImageUrl="/_layouts/15/images/TaxoMotor/WebPartIcon_LicenseFilterWebPart.gif" 
                ChromeType="None" Description="Фильтрация разрешений" 
                ImportErrorMessage="Cannot import Фильтрация разрешений Web Part." 
                Title="Фильтрация разрешений" TitleIconImageUrl="/_layouts/15/images/TaxoMotor/WebPartIcon_LicenseFilterWebPart.gif" 
                ID="g_d77f0aed_2a1c_46f2_9dab_5434122f7d82" __MarkupType="vsattributemarkup" __WebPartId="{D77F0AED-2A1C-46F2-9DAB-5434122F7D82}" 
                WebPart="true" __designer:IsClosed="false" partorder="2"></AppPages:LicenseFilterWebPart>
        </ZoneTemplate>
    </WebPartPages:WebPartZone>
</asp:Content>

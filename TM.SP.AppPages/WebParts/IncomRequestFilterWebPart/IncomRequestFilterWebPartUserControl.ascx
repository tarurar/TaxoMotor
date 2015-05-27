<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IncomRequestFilterWebPartUserControl.ascx.cs" Inherits="TM.SP.AppPages.IncomRequestFilterWebPartUserControl, TM.SP.AppPages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=63e92b7beac312db" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>



<table id="maintable" border="0" cellspacing="0" cellpadding="0" class="ms-propertysheet" width="100%">
    <tbody>
        <tr>
            <td class="ms-formdescriptioncolumn-wide" valign="top">
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tbody>
                        <tr>
                            <td class="ms-sectionheader" style="padding-top: 4px;" height="22" valign="top">
                                <h3 class="ms-standardheader ms-inputformheader">
                                    <asp:Label runat="server" ID="InputDateText"></asp:Label>
                                </h3>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-descriptiontext ms-inputformdescription">
                                <asp:Label runat="server" ID="InputDateDescriptionText"></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
            <td class="ms-authoringcontrols ms-inputformcontrols" valign="top" aligh="left">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td width="9px">
                                <img src="/_layouts/15/images/blank.gif?rev=23" width="9" height="7" alt=""></td>
                            <td>
                                <img src="/_layouts/15/images/blank.gif?rev=23" width="150" height="7" alt=""></td>
                            <td width="10px">
                                <img src="/_layouts/15/images/blank.gif?rev=23" width="10" height="1" alt=""></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td class="ms-authoringcontrols">
                                <table class="ms-authoringcontrols" border="0" width="100%" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td class="ms-authoringcontrols" colspan="2" nowrap="nowrap">
                                                <asp:Label runat="server" ID="InputDateParamFromText"></asp:Label><font size="3">&nbsp;</font><br>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td dir="ltr">
                                                <SharePoint:DateTimeControl DateOnly="true" EnableViewState="true" ID="InputDateParamFrom" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ms-authoringcontrols" colspan="2" nowrap="nowrap">
                                                <asp:Label runat="server" ID="InputDateParamToText"></asp:Label><font size="3">&nbsp;</font><br>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td dir="ltr">
                                                <SharePoint:DateTimeControl DateOnly="true" EnableViewState="true" ID="InputDateParamTo" runat="server" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td class="ms-formdescriptioncolumn-wide" valign="top">
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tbody>
                        <tr>
                            <td class="ms-sectionheader" style="padding-top: 4px;" height="22" valign="top">
                                <h3 class="ms-standardheader ms-inputformheader">
                                    <asp:Label runat="server" ID="PrepareFactDateText"></asp:Label></h3>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-descriptiontext ms-inputformdescription">
                                <asp:Label runat="server" ID="PrepareFactDateDescriptionText"></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
            <td class="ms-authoringcontrols ms-inputformcontrols" valign="top" aligh="left">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td width="9px">
                                <img src="/_layouts/15/images/blank.gif?rev=23" width="9" height="7" alt=""></td>
                            <td>
                                <img src="/_layouts/15/images/blank.gif?rev=23" width="150" height="7" alt=""></td>
                            <td width="10px">
                                <img src="/_layouts/15/images/blank.gif?rev=23" width="10" height="1" alt=""></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td class="ms-authoringcontrols">
                                <table class="ms-authoringcontrols" border="0" width="100%" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td class="ms-authoringcontrols" colspan="2" nowrap="nowrap">
                                                <asp:Label runat="server" ID="PrepareFactDateParamFromText"></asp:Label><font size="3">&nbsp;</font><br>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td dir="ltr">
                                                <SharePoint:DateTimeControl DateOnly="true" EnableViewState="true" ID="PrepareFactDateParamFrom" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ms-authoringcontrols" colspan="2" nowrap="nowrap">
                                                <asp:Label runat="server" ID="PrepareFactDateParamToText"></asp:Label><font size="3">&nbsp;</font><br>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td dir="ltr">
                                                <SharePoint:DateTimeControl DateOnly="true" EnableViewState="true" ID="PrepareFactDateParamTo" runat="server" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td height="10" class="ms-descriptiontext">
                <img src="/_layouts/15/images/blank.gif?rev=23" width="1" height="10" alt="">
            </td>
        </tr>
        <tr>
            <td>
                <table id="buttons" border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tbody>
                        <tr>
                            <td>
                                <asp:Button ID="btnFind" runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td height="40" class="ms-descriptiontext">
                <img src="/_layouts/15/images/blank.gif?rev=23" width="1" height="40" alt="">
            </td>
        </tr>
    </tbody>
</table>

<asp:Panel ID="DataPanel" runat="server">
    <WebPartPages:XsltListViewWebPart runat="server" ChromeType="None" ID="lvRequests" ServerRender="true" ClientRender="false" IsClientRender="false" >
    </WebPartPages:XsltListViewWebPart>
</asp:Panel>


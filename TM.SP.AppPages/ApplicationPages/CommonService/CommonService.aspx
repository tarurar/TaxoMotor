﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommonService.aspx.cs" Inherits="TM.SP.AppPages.CommonService" DynamicMasterPageFile="~masterurl/default.master" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="Сервис общих функций" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="Сервис общих функций" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageDescription"  runat="server">
    <SharePoint:EncodedLiteral runat="server" text="Сервис общих функций" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <SharePoint:FormDigest ID="FormDigest2" runat="server" />
</asp:Content>

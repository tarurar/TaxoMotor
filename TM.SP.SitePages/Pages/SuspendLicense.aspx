﻿<%@ Page Language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/15/ButtonSection.ascx" %>

<asp:Content ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Приостановка действия разрешения
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Приостановка действия разрешения
</asp:Content>

<asp:Content ID="AddPageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link rel="Stylesheet" type="text/css" href="/ProjectScripts/LicenseActions.css" />
    <link rel="Stylesheet" type="text/css" href="/_layouts/15/TM.SP.Customizations/Styles/themes/base/core.css" />
    <link rel="Stylesheet" type="text/css" href="/_layouts/15/TM.SP.Customizations/Styles/themes/base/datepicker.css" />
    <link rel="Stylesheet" type="text/css" href="/_layouts/15/TM.SP.Customizations/Styles/themes/base/theme.css" />
    <script type="text/javascript" src="/ProjectScripts/CryptoPro.js"></script>

    <object id="cadesplugin" type="application/x-cades" class="hiddenObject"></object>
    <object id='certEnrollClassFactory' classid='clsid:884e2049-217d-11da-b2a4-000e7bbb2b09' class="hiddenObject"></object>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div id="needIE" data-bind="ifnot: $.browser.msie">
        <div style="background-color: bisque; padding: 10px;">
            <span>
                Для выполнения этого действия понадобится электронно-цифровая подпись, которая поддерживается только в браузерах Internet Explorer.
            </span>
        </div>
    </div>
    <div id="paramsForm" data-bind="with: Params">
        <section style="margin-top: 10px;">
            <div style="float: left; margin-right: 20px;">
                <label for="dateFrom" style="display: block">Дата с</label>
                <input id="dateFrom" style="display: block" data-bind="datepicker: DateFrom"/>
            </div>
            <div style="float: left;">
                <label for="dateTill" style="display: block">Дата по</label>
                <input id="dateTill" style="display: block" data-bind="datepicker: DateTill"/>
            </div>
            <br style="clear:both;" />
        </section>
        <section style="margin-top: 10px;">
            <div style="float: left; margin-right: 20px; width: 100%;">
                <label for="actionReason" style="display: block">Причина приостановки</label>
                <textarea id="actionReason" style="display: block; width: 100%; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; box-sizing: border-box;" rows="7" data-bind="value: ActionReason" ></textarea>
            </div>
        </section>
    </div>
    <div id="errorList" class="error-list" data-bind="visible: Errors().length > 0">
        <span>Обратите внимание на следующие ошибки</span>
        <table>
            <thead>
                <tr><th>Описание</th></tr>
            </thead>
			<tbody data-bind="foreach: Errors">
                <tr><td><span data-bind="html: MessageBody"></span></td></tr>
			</tbody>
        </table>
    </div>
    <div id="buttonPanel">
        <table id="maintable" border="0" cellspacing="0" cellpadding="0" class="ms-propertysheet" width="100%">
            <wssuc:ButtonSection runat="server" ShowStandardCancelButton="False">
            <Template_Buttons>
                <asp:placeholder ID="Placeholder1" runat="server">
                    <button type="button" id="BtnOk" class="ms-ButtonHeightWidth" data-bind="click: DoAction">Ок</button>
                <SeparatorHtml>
                    <span id="idSpace" class="ms-SpaceBetButtons" />
                </SeparatorHtml>
                <button type="button" id="BtnCancel" class="ms-ButtonHeightWidth" data-bind="click: CancelAction">Отмена</button>
                </asp:PlaceHolder>
            </Template_Buttons>
            </wssuc:ButtonSection>
        </table>
    </div>

    <script type="text/javascript" src="/ProjectScripts/SuspendLicense.js"></script>
</asp:Content>
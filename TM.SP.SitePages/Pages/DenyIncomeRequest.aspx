<%@ Page Language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/15/ButtonSection.ascx" %>

<asp:Content ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Отказ по обращению
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Отказ по обращению
</asp:Content>

<asp:Content ID="AddPageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link rel="Stylesheet" type="text/css" href="/ProjectScripts/IncomeRequestActions.css" />
    <link rel="Stylesheet" type="text/css" href="/_layouts/15/TM.SP.Customizations/Styles/themes/base/core.css" />
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
                <label for="denyReason" style="display: block">Укажите причину отказа</label>
                <select id="denyReason" data-bind="options: DenyReason, optionsText: 'Title', value: selectedReason"></select>
            </div>
            <br style="clear:both;" />
        </section>
        <section style="margin-top: 10px;">
            <div style="float: left; margin-right: 20px; width: 100%;">
                <label for="actionComment" style="display: block">Комментарий</label>
                <textarea id="actionComment" style="display: block; width: 100%; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; box-sizing: border-box;" rows="7" data-bind="value: ActionComment" ></textarea>
            </div>
        </section>
        <section style="margin-top: 10px;">
            <div style="float: left; margin-right: 20px; width: 100%;">
                <p style="display: block;">
                    Необходим очный визит
                    <input data-bind="checked: NeedPersonVisit" type="checkbox" id="needPersonVisit" />
                </p>
                <p style="display: block;">
                    Отказ в приеме документов
                    <input data-bind="checked: RefuseDocuments" type="checkbox" id="refuseDocuments" />
                </p>                
            </div>
        </section>
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

    <script type="text/javascript" src="/ProjectScripts/DenyIncomeRequest.js"></script>
</asp:Content>
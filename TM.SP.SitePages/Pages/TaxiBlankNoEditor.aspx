<%@ Page Language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/15/ButtonSection.ascx" %>

<asp:Content ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Ввод данных бланка
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Ввод данных бланка
</asp:Content>

<asp:Content ID="AddPageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link rel="Stylesheet" type="text/css" href="/ProjectScripts/TaxiActions.css" />
    <link rel="Stylesheet" type="text/css" href="/_layouts/15/TM.SP.Customizations/Styles/themes/base/core.css" />
    <link rel="Stylesheet" type="text/css" href="/_layouts/15/TM.SP.Customizations/Styles/themes/base/theme.css" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div data-bind="if: Error">
        <div id="errorList" class="ui-state-error">
            <label id="error" data-bind="text: Error" ></label>
        </div>
    </div>
    <div id="paramsForm" data-bind="with: Params">
        <section style="margin-top: 10px;">
            <div style="float: left; margin-right: 20px;">
                <label for="blankSerie" style="display: block">Серия</label>
                <input data-bind="value: BlankSerie" type="text" id="blankSerie" />
            </div>
            <br style="clear:both;" />
        </section>
        <section style="margin-top: 10px;">
            <div style="float: left; margin-right: 20px; width: 100%;">
                <label for="blankNo" style="display: block">Номер</label>
                <input id="blankNo" data-bind="value: BlankNo" type="text" />
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

    <script type="text/javascript" src="/ProjectScripts/TaxiBlankNoEditor.js"></script>
</asp:Content>
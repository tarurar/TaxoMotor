<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RatingsWPUserControl.ascx.cs" Inherits="TM.SP.Ratings.RatingsWPUserControl, TM.SP.Ratings, Version=1.0.0.0, Culture=neutral, PublicKeyToken=97107c5c4be107a3" %>

<style type="text/css">
    .error-message {
        color: red;        
    }

    /*top*/

    .top-1, .top-2, .top-3, .top-4, .top-5, .top-6, .top-7, .top-8, .top-9, .top-10{
	    width:100%;
	    margin:20px 0;
    }
    .top-1 td{
        width:100%;
	    text-align:center;
    }
    .top-2 td{
        width:50%;
	    text-align:center;
    }
    .top-3 td{
        width:33.33%;
	    text-align:center;
    }
    .top-4 td{
        width:25%;
	    text-align:center;
    }
    .top-5 td{
	    width:20%;
	    text-align:center;
    }
    .top-6 td{
        width:16.65%;
	    text-align:center;
    }
    .top-7 td{
        width:14.28%;
	    text-align:center;
    }
    .top-8 td{
        width:12.5%;
	    text-align:center;
    }
    .top-9 td{
        width:11.11%;
	    text-align:center;
    }
    .top-10 td{
        width:10%;
	    text-align:center;
    }

    .top-1-div, .top-2-div, .top-3-div, .top-4-div, .top-5-div, .top-6-div, .top-7-div, .top-8-div, .top-9-div, .top-10-div{
	    background: #fff;
        width: 80px;
        height: 80px;
	    border-radius: 50%;
	    border:5px solid #fceb15;
	    display: inline-block;
	    font-size: 25px;
	    font-weight: bold;
	    line-height: 80px;
	    color:#5c6878;
    }
    span.top-1, span.top-2, span.top-3, span.top-4, span.top-5, span.top-6, span.top-7, span.top-8, span.top-9, span.top-10{
	    display: block;
	    padding-top:10px;
	    color: #3f3f3f;
    }
</style>

<div class="data-cont">
    <div class="data-title"><span><%= Header %></span></div>
    <asp:Label ID="ErrorMessage" runat="server" CssClass="error-message" Text="Here will be information about error"></asp:Label>
    <asp:Table runat="server" ID="DataTable"></asp:Table>
</div>



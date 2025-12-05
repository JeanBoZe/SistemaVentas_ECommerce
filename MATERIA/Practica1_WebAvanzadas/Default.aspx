<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Practica1_WebAvanzadas._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/CSS_default.css") %>" />

    <div class="jumbotron">
        <h1>SISTEMA DE VENTAS</h1>
        <p class="lead">Página web para un sistema de ventas.</p>
    </div>
</asp:Content>

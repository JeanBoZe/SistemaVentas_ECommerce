<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Proveedor.aspx.cs" Inherits="Practica1_WebAvanzadas.Proveedor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <link rel ="stylesheet" href="CSS_empleado.css" />
        <h1>PROVEEDOR</h1>
        <p class="lead">Formulario para Proveedores.</p>
    </div>
    <div class="jumbotron">
        <div class="form-container">
            <asp:Label ID="Label1" runat="server" Text="ID: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_ID" runat="server" CssClass ="form-control" placeholder ="Ej: 1004"></asp:TextBox>

            <asp:Label ID="Label2" runat="server" Text="Nombre de la Empresa: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_nombreE" runat="server" CssClass ="form-control" placeholder ="Coca Cola"></asp:TextBox>

            <asp:Label ID="Label3" runat="server" Text="RFC: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_rfc" runat="server" CssClass ="form-control" placeholder ="VECJ880326"></asp:TextBox>

            <asp:Label ID="Label4" runat="server" Text="Direccion: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_direccion" runat="server" CssClass ="form-control" placeholder ="Independencia"></asp:TextBox>

            <asp:Label ID="Label5" runat="server" Text="Telefono: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_telefono" runat="server" CssClass ="form-control" placeholder ="6871983244"></asp:TextBox>

            <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                <button class="btn btn-primary me-md-2" type="button">Actualizar</button>
                <button class="btn btn-primary" type="button">Eliminar</button>
            </div>
            </div>
    </div>
    <div class="jumbotron">
        <h2>Lista de Proveedores</h2>
        <asp:Table ID="tbl_proveedores" runat="server"></asp:Table>
    </div>

</asp:Content>

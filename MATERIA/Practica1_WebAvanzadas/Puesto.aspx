<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Puesto.aspx.cs" Inherits="Practica1_WebAvanzadas.Puesto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     
    <div class="jumbotron">
        <link rel ="stylesheet" href="CSS_empleado.css" />
        <h1>Puesto</h1>
        <p class="lead">Formulario para Puestos.</p>
    </div>

    <div class="jumbotron">
        <div class="form-container">
            <asp:Label ID="Label1" runat="server" Text="ID: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_ID" runat="server" CssClass ="form-control" placeholder ="Ej: 1004"></asp:TextBox>

            <asp:Label ID="Label2" runat="server" Text="Nombre: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_nombre" runat="server" CssClass ="form-control" placeholder ="Jean Carlo"></asp:TextBox>

            <asp:Label ID="Label3" runat="server" Text="Departamento: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_departamento" runat="server" CssClass ="form-control" placeholder ="Bojorquez Zenon"></asp:TextBox>

            <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                <button class="btn btn-primary me-md-2" type="button">Actualizar</button>
                <button class="btn btn-primary" type="button">Eliminar</button>
            </div>
            </div>
    </div>

    <div class="jumbotron">
        <h2>Lista de Puestos</h2>
        <asp:Table ID="tbl_puestos" runat="server"></asp:Table>
    </div>
</asp:Content>

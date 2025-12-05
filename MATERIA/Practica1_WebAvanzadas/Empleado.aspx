<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Empleado.aspx.cs" Inherits="Practica1_WebAvanzadas.Empleado" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <link rel ="stylesheet" href="CSS_empleado.css" />
        <h1>EMPLEADO</h1>
        <p class="lead">Formulario para Empleados.</p>
    </div>
    <div class="jumbotron">
        <div class="form-container">
            <asp:Label ID="Label1" runat="server" Text="Codigo: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_codigo" runat="server" CssClass ="form-control" placeholder ="Ej: 1004"></asp:TextBox>

            <asp:Label ID="Label2" runat="server" Text="Nombre: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_nombre" runat="server" CssClass ="form-control" placeholder ="Jean Carlo"></asp:TextBox>

            <asp:Label ID="Label3" runat="server" Text="Apellidos: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_apellidos" runat="server" CssClass ="form-control" placeholder ="Bojorquez Zenon"></asp:TextBox>

            <asp:Label ID="Label4" runat="server" Text="Direccion: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_direccion" runat="server" CssClass ="form-control" placeholder ="Independencia"></asp:TextBox>

            <asp:Label ID="Label5" runat="server" Text="Telefono: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_telefono" runat="server" CssClass ="form-control" placeholder ="6871983244"></asp:TextBox>

            <asp:Label ID="Label6" runat="server" Text="Fecha de Nacimiento: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_fechaN" runat="server" CssClass ="form-control" placeholder ="25/05/2002"></asp:TextBox>

            <asp:Label ID="label7" runat="server" Text="Puesto de Trabajo: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:DropDownList ID="ddl_puesto" runat="server">
                <asp:ListItem Text="Seleccione un puesto" Value="" />
                <asp:ListItem Text="Gerente" Value="1" />
                <asp:ListItem Text="Supervisor" Value="2" />
                <asp:ListItem Text="Analista" Value="3" />
                <asp:ListItem Text="Desarrollador" Value="4" />
            </asp:DropDownList>

            <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                <button class="btn btn-primary me-md-2" type="button">Actualizar</button>
                <button class="btn btn-primary" type="button">Eliminar</button>
            </div>
            </div>
    </div>
    <div class="jumbotron">
        <h2>Lista de Empleados</h2>
        <asp:Table ID="tbl_empleados" runat="server"></asp:Table>
    </div>

</asp:Content>

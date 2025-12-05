<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cliente.aspx.cs" Inherits="Practica1_WebAvanzadas.Cliente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <link rel ="stylesheet" href="CSS_Cliente.css" />
        <h1>CLIENTE</h1>
        <p class="lead">Formulario para Clientes.</p>
    </div>
    <div class="jumbotron">
        <div class="form-container">
            <asp:Label ID="Label1" runat="server" Text="ID: " CssClass="text-primary-enfasis" Visible ="false"></asp:Label>
            <asp:TextBox ID="txt_ID" runat="server" CssClass ="form-control" placeholder ="Ej: 1004" Text ="0" Visible="false"></asp:TextBox>

            <asp:Label ID="Label2" runat="server" Text="Nombre: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_nombre" runat="server" CssClass ="form-control" placeholder ="Luis Enrique"></asp:TextBox>

            <asp:Label ID="Label3" runat="server" Text="Apellidos: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_apellidos" runat="server" CssClass ="form-control" placeholder ="Bojorquez Zenon"></asp:TextBox>

            <asp:Label ID="Label4" runat="server" Text="Direccion: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_direccion" runat="server" CssClass ="form-control" placeholder ="Independencia"></asp:TextBox>

            <asp:Label ID="Label5" runat="server" Text="Telefono: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_telefono" runat="server" CssClass ="form-control" placeholder ="6871983244"></asp:TextBox>

            <asp:Label ID="Label6" runat="server" Text="Fecha de Nacimiento: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:TextBox ID="txt_fechana" runat="server" CssClass ="form-control" placeholder ="25/05/02"></asp:TextBox>

            <asp:Label ID="label7" runat="server" Text="Tipo de Pago: " CssClass="text-primary-enfasis"></asp:Label>
            <asp:DropDownList ID="ddl_puesto" runat="server">
                <asp:ListItem Text="Seleccione Pago" Value="" />
                <asp:ListItem Text="Tarjeta de Debito" Value="1" />
                <asp:ListItem Text="Tarjeta de Credito" Value="2" />
                <asp:ListItem Text="Deposito" Value="3" />
            </asp:DropDownList>

            <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary me-md-2" Text="Guardar" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnBorrar" runat="server" CssClass="btn btn-primary me-md-2" Text="Borrar" OnClick="btnBorrar_Click" />                
            </div>

            </div>
    </div>
    <div class="jumbotron">
        <link rel="stylesheet" href="Content/fontawesome/css/all.min.css">

        <h2>Lista de Empleados</h2>

        <asp:Panel ID="pnlSearch" runat="server" CssClass="input-group mb-3">
            <asp:TextBox ID="txtBusqueda" runat="server" CssClass="form-control" Placeholder="Buscar..."></asp:TextBox>
            <div class="input-group-append">
                <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-primary" OnClick="btnBuscar_Click">
                    <i class="fas fa-search"></i>
                </asp:LinkButton>
            </div>
        </asp:Panel>



        <asp:GridView ID="gv_clientes" runat="server" AutoGenerateColumns ="false" CssClass="table table-striped my-gridview" OnRowCommand="gv_clientes_RowCommand" ShowHeader="true">
            <Columns>
                <asp:BoundField DataField ="CLI_ID" HeaderText="ID"/>
                <asp:BoundField DataField ="CLI_NOMBRE" HeaderText="NOMBRE" />
                <asp:BoundField DataField ="CLI_APELLIDOS" HeaderText="APELLIDOS" />
                <asp:BoundField DataField ="CLI_CELULAR" HeaderText="CELULAR" />
                <asp:BoundField DataField ="CLI_DIRECCION" HeaderText="DIRECCION" />
                <asp:BoundField DataField ="CLI_FECHA" HeaderText="FECHA NACIMIENTO" />
                <asp:BoundField DataField ="CLI_TIPOPAGO" HeaderText="TIPO DE PAGO" />
                <asp:TemplateField HeaderText="ACCION">
                    <ItemTemplate>
                        <asp:Button ID="btn_seleccionar" runat="server" Text="Seleccionar"
                            CommandName="Seleccionar" CommandArgument='<%# Container.DataItemIndex %>'
                            CssClass="btn btn-primary" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>


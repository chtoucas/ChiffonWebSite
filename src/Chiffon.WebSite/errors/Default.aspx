<%@ Page Language="C#" MasterPageFile="~/Errors/Error.Master"
 CodeBehind="Default.aspx.cs" Inherits="Chiffon.Errors.DefaultPage" %>

<asp:Content ContentPlaceHolderID="ErrorMessageCph" runat="server">
 <%= ErrorMessage %>
</asp:Content>

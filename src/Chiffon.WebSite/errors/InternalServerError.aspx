<%@ Page Language="C#" MasterPageFile="~/Errors/Error.Master"
 CodeBehind="InternalServerError.aspx.cs" Inherits="Chiffon.Errors.InternalServerErrorPage" %>

<asp:Content ContentPlaceHolderID="ErrorMessageCph" runat="server">
 <%= VR.Error_InternalServerError %>
</asp:Content>

<asp:Content ContentPlaceHolderID="ReturnHomeCph" runat="server">
</asp:Content>


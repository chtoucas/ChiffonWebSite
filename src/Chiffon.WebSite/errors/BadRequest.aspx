<%@ Page Language="C#" MasterPageFile="~/Errors/Error.Master"
 CodeBehind="BadRequest.aspx.cs" Inherits="Chiffon.Errors.BadRequestPage" %>

<asp:Content ContentPlaceHolderID="ErrorMessageCph" runat="server">
 <%= VR.Error_BadRequest %>
</asp:Content>

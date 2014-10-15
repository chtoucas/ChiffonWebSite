<%@ Page Language="C#" MasterPageFile="~/Errors/Error.Master"
 CodeBehind="NotFound.aspx.cs" Inherits="Chiffon.Errors.NotFoundPage" %>

<asp:Content ContentPlaceHolderID="ErrorMessageCph" runat="server">
 <%= VR.Error_NotFound %>
</asp:Content>

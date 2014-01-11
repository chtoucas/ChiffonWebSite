<%@ Page Language="C#" MasterPageFile="~/Errors/Error.Master"
 CodeBehind="ServiceUnavailable.aspx.cs" Inherits="Chiffon.Errors.ServiceUnavailablePage" %>

<asp:Content ContentPlaceHolderID="ErrorMessageCph" runat="server">
 <%= VR.Error_ServiceUnavailable %>
</asp:Content>

<asp:Content ContentPlaceHolderID="ReturnHomeCph" runat="server">
</asp:Content>

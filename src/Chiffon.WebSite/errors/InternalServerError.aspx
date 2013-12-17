<%@ Page Language="C#" MasterPageFile="~/Errors/Error.Master"
 CodeBehind="InternalServerError.aspx.cs" Inherits="Chiffon.Errors.InternalServerErrorPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <p>
  <%: VR.Error_InternalServerError %>
 </p>
</asp:Content>

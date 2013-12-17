<%@ Page Language="C#" MasterPageFile="~/Errors/Error.Master"
 CodeBehind="BadRequest.aspx.cs" Inherits="Chiffon.Errors.BadRequestPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <p>
  <%: VR.Error_BadRequest %>
 </p>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/Errors/Error.Master"
 CodeBehind="NotFound.aspx.cs" Inherits="Chiffon.Errors.NotFoundPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <p>
  <%: VR.Error_NotFound %>
 </p>
</asp:Content>

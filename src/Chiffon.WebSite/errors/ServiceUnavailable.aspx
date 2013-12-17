<%@ Page Language="C#" MasterPageFile="~/Errors/Error.Master"
 CodeBehind="ServiceUnavailable.aspx.cs" Inherits="Chiffon.Errors.ServiceUnavailablePage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <p>
  <%: VR.Error_ServiceUnavailable %>
 </p>
</asp:Content>

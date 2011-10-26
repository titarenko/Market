<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Market.Cqrsnes.Test.ExecutionResult>>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitlePlaceHolder">Article Specifications</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="ContentPlaceHolder">

    <h1>Specifications:</h1>

    <% foreach (var result in Model) { %>
    <pre class="<%= result.IsPassed ? "success" : "failure" %>">
        <%= result.Details %>
    </pre>
    <% } %>

    <%= Html.ActionLink("Back", "List") %>

</asp:Content>
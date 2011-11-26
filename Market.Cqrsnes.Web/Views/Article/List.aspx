<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Market.Cqrsnes.Projection.ArticleListViewModel>" MasterPageFile="~/Views/Shared/Site.master" %>
<asp:Content runat="server" ContentPlaceHolderID="TitlePlaceHolder">Article List</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder">

    <h1>Market.Cqrsnes.Web</h1>

    <%= Html.ActionLink("Run Domain Specifications (Tests)", "TestDomain") %>
    <%= Html.ActionLink("Run Projection Specifications (Tests)", "TestProjections") %>

    <table>
    <thead>
    <tr>
        <th>Name</th>
        <th>Count</th>
        <th>Controls</th>
    </tr>
    </thead>
    <tbody>
    <% foreach (var article in Model.Articles) { %>
    <tr>
        <td><%= article.Name %></td>
        <td><%= article.Count %></td>
        <td>
            <% using (Html.BeginForm("ChangeCount", "Article", FormMethod.Post)) { %>
                <%= Html.Hidden("Id", article.Id) %>
                <%= Html.TextBox("Count") %>
                <input type="submit" value="Deliver" name="Deliver" />
                <input type="submit" value="Buy" name="Buy" />
            <% } %>
        </td>
    </tr>
    <% } %>
    </tbody>
    </table>

    <% using (Html.BeginForm("CreateArticle", "Article", FormMethod.Post)) { %>
        <%= Html.TextBox("Name") %>
        <input type="submit" value="Create" />
    <% } %>
    
</asp:Content>
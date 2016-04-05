<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="DNNAuth.LinkedIn.Login" %>
<li id="loginItem" runat="server" class="linkedin" >
    <asp:LinkButton runat="server" ID="loginButton" CausesValidation="False">
        <span><%=LocalizeString("LoginLinkedIn") %></span>
    </asp:LinkButton>
</li>
<li id="registerItem" runat="Server" class="linkedin">
    <asp:LinkButton ID="registerButton" runat="server" CausesValidation="False">
        <span><%=LocalizeString("RegisterLinkedIn") %></span>
    </asp:LinkButton>
</li>
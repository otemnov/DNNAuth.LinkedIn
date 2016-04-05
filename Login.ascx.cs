#region Usings

using System;
using DNNAuth.LinkedIn.Components;
using DotNetNuke.Services.Authentication;
using DotNetNuke.Services.Authentication.OAuth;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins.Controls;

#endregion

namespace DNNAuth.LinkedIn
{
    public partial class Login : OAuthLoginBase
    {
        protected override string AuthSystemApplicationName
        {
            get { return "LinkedIn"; }
        }

        public override bool SupportsRegistration
        {
            get { return true; }
        }

        protected override UserData GetCurrentUser()
        {
            return OAuthClient.GetCurrentUser<LinkedInUserData>();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            loginButton.Click += loginButton_Click;
            registerButton.Click += loginButton_Click;

            OAuthClient = new LinkedInClient(PortalId, Mode);

            loginItem.Visible = (Mode == AuthMode.Login);
            registerItem.Visible = (Mode == AuthMode.Register);
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            AuthorisationResult result = OAuthClient.Authorize();
            if (result == AuthorisationResult.Denied)
            {
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, Localization.GetString("PrivateConfirmationMessage", Localization.SharedResourceFile), ModuleMessage.ModuleMessageType.YellowWarning);
            }
        }
    }
}
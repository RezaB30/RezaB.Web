using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Owin;
using RadiusR.DB;

namespace RezaB.Web.Authentication.TestUnit
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void SignInButton_Click(object sender, EventArgs e)
        {
            ResultsListbox.Items.Clear();
            OwinContext context = new OwinContext();
            var simpleAuthenticator = new Authenticator<RadiusREntities, AppUser, SHA256>(u => u.ID, u => u.Name, u => u.Email, u => u.Password, u => u.IsEnabled);
            var result = simpleAuthenticator.SignIn(context, UsernameTextbox.Text, PasswordTextbox.Text);
            if (result)
            {
                var identity = context.Authentication.AuthenticationResponseGrant.Principal;
                ResultsListbox.Items.AddRange(new string[]
                {
                    $"User Id: { identity.GiveUserId()}",
                    $"Name: {identity.Identity.Name}"
                });
                ResultsListbox.Items.AddRange(GetClaims(identity).ToArray());
            }
            else
            {
                MessageBox.Show("Authentication failed.");
            }
        }

        private IEnumerable<string> GetClaims(IPrincipal user)
        {
            var claimsIdentity = user.Identity as ClaimsIdentity;
            return claimsIdentity.Claims.Select(c => $"{c.Type}={c.Value}");
        }

        private void SignInWithPermissionsButton_Click(object sender, EventArgs e)
        {
            ResultsListbox.Items.Clear();
            OwinContext context = new OwinContext();
            var authenticatorWithRoles = new Authenticator<RadiusREntities, AppUser, Role, Permission, SHA256>(u => u.ID, u => u.Name, u => u.Email, u => u.Password, u => u.IsEnabled, r => r.Name, p => p.Name);
            var result = authenticatorWithRoles.SignIn(context, UsernameTextbox.Text, PasswordTextbox.Text);
            if (result)
            {
                var identity = context.Authentication.AuthenticationResponseGrant.Principal;
                ResultsListbox.Items.AddRange(new string[]
                {
                    $"User Id: { identity.GiveUserId()}",
                    $"Name: {identity.Identity.Name}"
                });
                ResultsListbox.Items.AddRange(identity.GetPermissions().Select(p => $"Permission={p}").ToArray());
                ResultsListbox.Items.AddRange(GetClaims(identity).ToArray());
            }
            else
            {
                MessageBox.Show("Authentication failed.");
            }
        }
    }
}

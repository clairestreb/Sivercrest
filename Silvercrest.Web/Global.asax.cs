using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using System.Web.SessionState;

namespace Silvercrest.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
            // setup application variables to write versions in razor (including .min extension when not debugging)
            //string addMin = ".min";
            //if (System.Diagnostics.Debugger.IsAttached) { addMin = ""; }  // don't use minified files when executing locally
            Application["JSVer"] = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace('.', '0') /*+ addMin*/ + ".js";
            Application["CSSVer"] = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace('.', '0') /*+ addMin*/ + ".css";
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            System.Diagnostics.Debug.WriteLine(exception);
            Response.Redirect("/Error/Error");
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
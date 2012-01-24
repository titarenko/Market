using System;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using Market.Cqrsnes.Projection;
using Market.Cqrsnes.Projection.Models;

namespace Market.Cqrsnes.WebUi.Infrastructure
{
    /// <summary>
    /// Represents system context.
    /// </summary>
    public class WebSystemContext : ISystemContext
    {
        private const string SESSION_KEY = "__market_session";

        private User user;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WebSystemContext"/> class.
        /// </summary>
        public WebSystemContext()
        {
            Principal = new Principal(user = LoadUser());
        }
        
        /// <summary>
        /// Gets principal.
        /// </summary>
        public IPrincipal Principal
        {
            get
            {
                return GetContext().User;
            }

            private set
            {
                GetContext().User = value;
            }
        }

        /// <summary>
        /// Gets or sets user.
        /// </summary>
        public User User
        {
            get
            {
                return user;
            }

            set
            {
                SaveUser(user = value);
                Principal = new Principal(user);
            }
        }

        private User LoadUser()
        {
            var context = GetContext();
            var cookie = context.Request.Cookies[SESSION_KEY];
            return cookie != null
                       ? context.Cache[cookie.Value] as User
                       : null;
        }

        private void SaveUser(User user)
        {
            var context = GetContext();

            if (user != null)
            {
                var key = Guid.NewGuid().ToString();

                context.Response.Cookies.Add(new HttpCookie(SESSION_KEY, key));
                context.Cache.Add(
                    key,
                    user,
                    null,
                    Cache.NoAbsoluteExpiration,
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.NotRemovable,
                    null);
            }
            else
            {
                var cookie = context.Request.Cookies[SESSION_KEY];
                if (cookie != null)
                {
                    context.Cache.Remove(cookie.Value);
                }
            }
        }

        private HttpContextBase GetContext()
        {
            var context = HttpContext.Current;
            if (context == null)
            {
                throw new ApplicationException("WebSystemContext used outside of web application.");
            }

            return new HttpContextWrapper(context);
        }
    }
}

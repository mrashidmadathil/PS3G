using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ps3g.Models
{
    public class SessionUtility
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpContextAccessor _Accessor;

        public SessionUtility()
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            _httpContextAccessor = httpContextAccessor;
            _Accessor = new HttpContextAccessor();
        }

        //Set Session syntax

        public void SetSession(string key, string value)
        {
            _Accessor.HttpContext.Session.SetString(key, value);
        }

        //Get Session syntax

        public string GetSession(string key)
        {
            return _Accessor.HttpContext.Session.GetString(key);
        }
        public void RemoveSession(string key)
        {
            _Accessor.HttpContext.Session.Remove(key);
        }
    }
}


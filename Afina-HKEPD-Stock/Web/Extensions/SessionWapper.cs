﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Extensions
{
    public interface ISessionWapper
    {
        UserModel User { get; set; }
    }

    public class UserModel
    {
        public string ID {get;set;}
    }

    public class SessionWapper : ISessionWapper
    {
        private static readonly string _userKey = "session.user";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionWapper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session
        {
            get
            {
                return _httpContextAccessor.HttpContext.Session;
            }
        }

        public UserModel User
        {
            get
            {
                return Session.GetObject<UserModel>(_userKey);
            }
            set
            {
                Session.SetObject(_userKey, value);
            }
        }
    }
 
}

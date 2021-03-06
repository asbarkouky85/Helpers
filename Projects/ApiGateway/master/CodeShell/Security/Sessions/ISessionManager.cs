﻿using System;

namespace CodeShell.Security.Sessions
{
    public interface ISessionManager
    {
        
        TimeSpan DefaultSessionTime { get; }
        void StartSession(IUser user, TimeSpan? length = null);
        void EndSession();
        bool IsLoggedIn();
        object GetCurrentUserId();
        IUser GetUserData();
        void AuthorizationRequest();
        void ResetUserCache(object id);
    }
}

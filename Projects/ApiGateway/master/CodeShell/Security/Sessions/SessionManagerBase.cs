using System.Collections.Generic;

namespace CodeShell.Security.Sessions
{
    public abstract class SessionManagerBase
    {
        static Dictionary<object, IUser> _UsersCache;
        protected Dictionary<object, IUser> UsersCache
        {
            get
            {
                if (_UsersCache == null)
                    _UsersCache = new Dictionary<object, IUser>();
                return _UsersCache;
            }
        }

        public virtual void ResetUserCache(object id)
        {
            if (UsersCache.ContainsKey(id))
            {
                UsersCache.Remove(id);
            }
        }

        public abstract object GetCurrentUserId();
        public virtual IUser GetUserData()
        {
            object c = GetCurrentUserId();
            if (c == null)
                return null;
            if (!UsersCache.ContainsKey(c))
            {
                IUser use = Shell.Unit.UserRepository.GetByUserId(c);

                if (use == null)
                    return null;

                UsersCache[c] = use;
            }
            return UsersCache[c];
        }
    }
}

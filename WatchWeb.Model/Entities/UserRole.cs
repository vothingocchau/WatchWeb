﻿namespace WatchWeb.Model.Entities
{
    public class UserRole
    {
        public int UserAccountId {  get; set; }
        public int RoleId { get; set; }


        public virtual Role Role { get; set; }
    }
}

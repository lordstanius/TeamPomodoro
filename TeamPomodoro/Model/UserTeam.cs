﻿using System;

namespace Model
{
    public class UserTeam : IEquatable<UserTeam>, IEntity
    {
        public Guid UserTeamId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public virtual User User { get; set; }
        public Team Team { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserTeam ? ((UserTeam)obj).UserTeamId == UserTeamId : false;
        }

        public bool Equals(UserTeam userTeam)
        {
            return userTeam.UserTeamId == UserTeamId;
        }

        public override int GetHashCode()
        {
            return BitConverter.ToInt32(UserTeamId.ToByteArray(), 0);
        }

        public Guid GetId()
        {
            return UserTeamId;
        }
    }
}
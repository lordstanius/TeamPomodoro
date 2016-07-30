using System;

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
            UserTeam u = obj as UserTeam;
            return u != null ? u.UserTeamId == UserTeamId : false;
        }

        public bool Equals(UserTeam other)
        {
            return other != null ? other.UserTeamId == UserTeamId : false;
        }

        public override int GetHashCode()
        {
            return BitConverter.ToInt32(UserTeamId.ToByteArray(), 0);
        }

        public Guid Id
        {
            get { return UserTeamId; }
        }
    }
}

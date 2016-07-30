using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class User : IEquatable<User>, IEntity
    {
        public Guid UserId { get; set; }

        [MaxLength(20)]
        public string UserName { get; set; }

        [MaxLength(64)]
        public string Password { get; set; }
        public bool ShowWarningAfterPomodoroExpires { get; set; }
        public int PomodoroDurationInMin { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? CurrentUserTeamId { get; set; }

        public Team Team { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<UserTeam> UserTeams { get; set; }

        public override bool Equals(object obj)
        {
            User u = obj as User;
            return u != null ? u.UserId == UserId : false;
        }

        public bool Equals(User other)
        {
            return other != null ? other.UserId == UserId : false;
        }

        public override int GetHashCode()
        {
            return BitConverter.ToInt32(UserId.ToByteArray(), 0);
        }

        public override string ToString()
        {
            return UserName;
        }

        public Guid Id
        {
            get { return UserId; }
        }
    }
}

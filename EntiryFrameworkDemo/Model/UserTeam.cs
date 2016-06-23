using System;

namespace Model
{
	public class UserTeam
	{
		public Guid UserTeamId { get; set; }
		public Guid? TeamId { get; set; }
		public Guid UserId { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? StopTime { get; set; }
		public User User { get; set; }
		public Team Team { get; set; }
	}
}

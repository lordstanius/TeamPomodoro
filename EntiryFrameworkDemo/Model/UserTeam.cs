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
		public virtual User User { get; set; }
		public virtual Team Team { get; set; }
	}
}

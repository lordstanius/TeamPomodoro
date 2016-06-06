using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
	public class UserTeam
	{
		[Key]
		public Guid UserTeamId { get; set; }
		public Guid TeamId { get; set; }
		public Guid UserId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime StopTime { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		[ForeignKey("TeamId")]
		public virtual Team Team { get; set; }
	}
}

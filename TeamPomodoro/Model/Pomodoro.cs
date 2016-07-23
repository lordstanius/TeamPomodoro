using System;

namespace Model
{
    public class Pomodoro : IEquatable<Pomodoro>, IEntity
    {
        public Guid PomodoroId { get; set; }
        public Guid TaskId { get; set; }
        public DateTime? StartTime { get; set; }
        public int DurationInMin { get; set; }
        public bool? IsSuccessfull { get; set; }

        public Task Task { get; set; }

        public override bool Equals(object obj)
        {
            Pomodoro p = obj as Pomodoro;
            return p != null ? p.PomodoroId == PomodoroId : false;
        }

        public bool Equals(Pomodoro other)
        {
            return other.PomodoroId == PomodoroId;
        }

        public override int GetHashCode()
        {
            return BitConverter.ToInt32(PomodoroId.ToByteArray(), 0);
        }

        public Guid GetId()
        {
            return PomodoroId;
        }
    }
}

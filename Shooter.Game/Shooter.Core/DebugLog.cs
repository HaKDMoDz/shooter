using System;

namespace Shooter.Core
{
    public class DebugLog
    {
        public readonly string Value;
        public readonly DateTime DeletionDate;

        public DebugLog(string value)
            : this(value, DateTime.UtcNow)
        {
        }

        public DebugLog(string value, DateTime deletionDate)
        {
            this.Value = value;
            this.DeletionDate = deletionDate;
        }
    }
}
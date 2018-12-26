using System;
// ReSharper disable All

namespace MakiseSharpServer.Domain.SeedWork
{
    public abstract class Entity
    {
        private int? requestedHashCode;

        public virtual Guid Id { get; protected set; }

        public static bool operator ==(Entity left, Entity right)
        {
            //return left?.Equals(right) ?? Equals(right, null);
            return Equals(left, null) ? Equals(right, null) : left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        public bool IsTransient()
        {
            return Id == default(Guid);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Entity))
#pragma warning disable SA1503 // Braces must not be omitted
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var item = (Entity)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            return item.Id == Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!requestedHashCode.HasValue)
                    requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return requestedHashCode.Value;
            }

            return base.GetHashCode();
        }
    }
}
#pragma warning restore SA1503 // Braces must not be omitted
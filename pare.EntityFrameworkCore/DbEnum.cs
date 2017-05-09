using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using pare.Common;

namespace pare.EntityFrameworkCore
{
    public class DbEnum<TEnum> : IDbEnum where TEnum : struct
    {
        private Enum _;
        private string _display;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id
        {
            get { return Convert.ToInt32(_); }
            set { _internalSet((Enum)Enum.ToObject(typeof(TEnum), value)); }
        }

        public string Description
        {
            get { return _.ToString(); }
            set { _internalSet((Enum)Enum.Parse(typeof(TEnum), value)); }
        }

        public string Display
        {
            get { return _display = (!string.IsNullOrEmpty(_display) ? _display : _.GetDisplayString()); }
            set { _display = value; }
        }

        private void _internalSet(Enum value)
        {
            if (!_.Equals(value))
            {
                _ = value;
            }
        }

        public DbEnum()
        {
            _ = (Enum)Enum.Parse(typeof(TEnum), default(TEnum).ToString());
        }

        protected DbEnum(Enum value)
        {
            _ = value;
        }

        public TEnum ToEnum()
        {
            return (TEnum)Convert.ChangeType(_, typeof(TEnum));
        }

        public static implicit operator DbEnum<TEnum>(Enum value)
        {
            return new DbEnum<TEnum>(value);
        }

        public static implicit operator TEnum(DbEnum<TEnum> value)
        {
            return value.ToEnum();
        }
    }
}

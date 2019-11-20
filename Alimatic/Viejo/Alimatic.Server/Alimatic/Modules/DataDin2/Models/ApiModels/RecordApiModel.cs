using System;

namespace Alimatic.DataDin2.Models
{
    public class RecordApiModel : IEquatable<RecordApiModel>
    {
        public int Year { get; set; } = DateTime.Now.Year;
        public int? Day { get; set; }
        public int? Day2 { get; set; }
        public int? Month { get; set; }
        public int? Month2 { get; set; }
        public int? Row { get; set; }
        public int? Group { get; set; }
        public int? Column { get; set; }
        public int? Division { get; set; }
        public decimal? Value { get; set; }
        public int? Enterprise { get; set; }
        public bool PartialData { get; set; }
        public int Model { get; set; } = 5920;
        public int? ColumnApaisado { get; set; }

        public RecordApiModel CreateCopy()
            => new RecordApiModel
            {
                Year = Year,
                Day = Day,
                Day2 = Day2,
                Month = Month,
                Month2 = Month2,
                Row = Row,
                Group = Group,
                Column = Column,
                Division = Division,
                Value = Value,
                Enterprise = Enterprise,
                Model = Model,
            };

        public bool Equals(RecordApiModel other)
        {
            if (Year == other.Year &&
                Month == other.Month &&
                Month2 == other.Month2 &&
                Day == other.Day &&
                Day2 == other.Day2 &&
                Division == other.Division &&
                Group == other.Group &&
                Row == other.Row &&
                Enterprise == other.Enterprise &&
                Model == other.Model)
                return true;

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is RecordApiModel other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode() =>
            1_000_000_000 + Enterprise ?? 0 +
              100_000_000 + Model +
               10_000_000 + Row ?? 0 +
                1_000_000 + Year +
                  100_000 + Month ?? 0 +
                   10_000 + Day ?? 0 +
                    1_000 + Division ?? 0 +
                      100 + Group ?? 0;
    }
}

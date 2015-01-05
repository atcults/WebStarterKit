namespace Common.Enumerations
{
    public class Day : Enumeration<Day>
    {
        public Day(string value, string displayName, int displayOrder) : base(value, displayName, displayOrder)
        {

        }

        public static readonly Day Sunday = new Day("1", "Sunday", 1);
        public static readonly Day Monday = new Day("2", "Monday", 2);
        public static readonly Day Tuesday = new Day("3", "Tuesday", 3);
        public static readonly Day Wednesday = new Day("4", "Wednesday", 4);
        public static readonly Day Thursday = new Day("5", "Thursday", 5);
        public static readonly Day Friday = new Day("6", "Friday", 6);
        public static readonly Day Saturday = new Day("7", "Saturday", 7);

        
    }
}
using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.Helpers
{
    public static class DayHelper
    {
        public static DateOnly GetDateThisWeek(Day day)
        {
            var today = DateTime.Today;

            Day todayAsCustomDay = today.DayOfWeek switch
            {
                DayOfWeek.Friday => Day.Friday,
                DayOfWeek.Saturday => Day.Saturday,
                DayOfWeek.Sunday => Day.Sunday,
                DayOfWeek.Monday => Day.Monday,
                DayOfWeek.Tuesday => Day.Tuesday,
                DayOfWeek.Wednesday => Day.Wednesday,
                DayOfWeek.Thursday => Day.Thursday,
                _ => throw new Exception("Invalid day")
            };

            int todayIndex = (int)todayAsCustomDay;
            int targetIndex = (int)day;

            int diff = (targetIndex - todayIndex + 7) % 7;

            DateTime result = today.AddDays(diff);

            return DateOnly.FromDateTime(result);
        }

        public static bool IsDayInPast(Day dayOfWeek)
        {
            var day = DateTime.Now.DayOfWeek switch
            {
                DayOfWeek.Saturday => Day.Saturday,
                DayOfWeek.Sunday => Day.Sunday,
                DayOfWeek.Monday => Day.Monday,
                DayOfWeek.Tuesday => Day.Tuesday,
                DayOfWeek.Wednesday => Day.Wednesday,
                DayOfWeek.Thursday => Day.Thursday,
                DayOfWeek.Friday => Day.Friday,
                _ => throw new ArgumentOutOfRangeException()
            };

            return dayOfWeek < day || dayOfWeek == day;
        }
    }
}

using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.Helpers
{
    public static class DayHelper
    {
        public static DateOnly GetDateThisWeek(Day day)
        {
            var today = DateTime.Today;

            int currentDayIndex = today.DayOfWeek switch
            {
                DayOfWeek.Saturday => 0,
                DayOfWeek.Sunday => 1,
                DayOfWeek.Monday => 2,
                DayOfWeek.Tuesday => 3,
                DayOfWeek.Wednesday => 4,
                DayOfWeek.Thursday => 5,
                DayOfWeek.Friday => 6,
                _ => throw new Exception("Invalid day")
            };

            var startOfWeek = today.AddDays(-currentDayIndex);

            if (today.DayOfWeek == DayOfWeek.Friday)
            {
                startOfWeek = startOfWeek.AddDays(7);
            }

            int targetDayIndex = day switch
            {
                Day.Saturday => 0,
                Day.Sunday => 1,
                Day.Monday => 2,
                Day.Tuesday => 3,
                Day.Wednesday => 4,
                Day.Thursday => 5,
                Day.Friday => 6,
                _ => throw new Exception("Invalid day")
            };

            var result = startOfWeek.AddDays(targetDayIndex);

            return DateOnly.FromDateTime(result);
        }

        public static bool IsDayInPast(Day day)
        {
            var date = GetDateThisWeek(day);

            return date < DateOnly.FromDateTime(DateTime.Today);
        }
    }
}

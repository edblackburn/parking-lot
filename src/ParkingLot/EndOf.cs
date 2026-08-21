using System;

namespace ParkingLot {

	public delegate long Product(long start);

	public static class EndOf {
		public static long Day(long start) {
			var time = DateTimeOffset.FromUnixTimeSeconds(start);
			var nextDay = new DateTimeOffset(time.Date.AddDays(1));
			return nextDay.ToUnixTimeSeconds();
		}

		public static long Week(long start) {
			var time = DateTimeOffset.FromUnixTimeSeconds(start);
			var nextWeek = new DateTimeOffset(time.Date.AddDays(7));
			return nextWeek.ToUnixTimeSeconds();
		}
	}
}

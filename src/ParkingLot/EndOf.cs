using System;

namespace ParkingLot {
	
	public delegate long Product(in long start);
	
	public static class EndOf {
		public static long Day(in long start) {
			var time = DateTimeOffset.FromUnixTimeSeconds(start);
			var day = new DateTimeOffset(time.Date.AddDays(1));
			var until = day.AddSeconds(-1);
			return until.ToUnixTimeSeconds();
		}

		public static long Week(in long start) {
			var time = DateTimeOffset.FromUnixTimeSeconds(start);
			var week = new DateTimeOffset(time.Date.AddDays(7));
			var until = week.AddSeconds(-1);
			return until.ToUnixTimeSeconds();
		}
	}
}

namespace ParkingLot {
	public sealed record Booking(string Location, Period Period) {
		public bool Covers(string location, long when) =>
			Location == location && Period.Contains(when);
	}
}

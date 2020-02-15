namespace ParkingLot {
	public class VehicleBooked {
		public readonly string LicencePlate;
		public readonly string Location;
		public readonly long Start;
		public readonly long Finish;

		public VehicleBooked(string licencePlate, string location, long start, long finish) {
			LicencePlate = licencePlate;
			Location = location;
			Start = start;
			Finish = finish;
		}
	}
}

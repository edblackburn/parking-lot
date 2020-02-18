namespace ParkingLot {
	public class VehicleUnbooked {
		public readonly string InspectionId;
		public readonly string LicencePlate;
		public readonly string Location;

		public VehicleUnbooked(in string inspectionId, in string licencePlate, in string location) {
			InspectionId = inspectionId;
			LicencePlate = licencePlate;
			Location = location;
		}
	}
}

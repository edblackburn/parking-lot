namespace ParkingLot {
	public abstract record VehicleEvent;

	public sealed record VehicleRegistered(string LicencePlate) : VehicleEvent;

	public sealed record VehicleBooked(
		string LicencePlate,
		string Location,
		long Start,
		long Finish) : VehicleEvent;

	public sealed record InspectionFailed(
		string InspectionId,
		string LicencePlate,
		string Location) : VehicleEvent;
}

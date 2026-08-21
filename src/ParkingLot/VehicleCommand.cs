using System;

namespace ParkingLot {
	public abstract record VehicleCommand;

	public sealed record Book(
		LicencePlate LicencePlate,
		string Location,
		long Start,
		Product Product) : VehicleCommand;

	public sealed record Inspect(
		InspectionId InspectionId,
		DateTimeOffset When,
		LicencePlate LicencePlate,
		string Location) : VehicleCommand;
}

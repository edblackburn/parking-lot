using System.Collections.Immutable;

namespace ParkingLot {
	public sealed record VehicleState(
		RegistrationStatus Registration,
		ImmutableList<Booking> Bookings) {

		public static VehicleState Initial => new VehicleState(
			Registration: RegistrationStatus.Unregistered,
			Bookings: ImmutableList<Booking>.Empty);
	}
}

using System;
using System.Linq;
using Xunit;
using Xunit.Asserts.Compare;

namespace ParkingLot.Tests {
	public class When_vehicle_is_booked {
		[Fact]
		public void It_should_book_registered_vehicle() {
			var licencePlate = LicencePlate.Create("123-123");
			var registeredVehicle = new VehicleRegistered("123-123");

			var vehicle = Vehicle.FromStream(
				licencePlate: licencePlate,
				events: registeredVehicle);

			vehicle.Book(licencePlate, "Kernow", 1579132800, EndOf.Week);

			var expected = new object[] {
				new VehicleBooked(
					licencePlate: "123-123",
					location: "Kernow",
					start: new DateTimeOffset(new DateTime(2020, 01, 16)).ToUnixTimeSeconds(),
					finish: new DateTimeOffset(new DateTime(2020, 01, 16)).AddDays(7).AddSeconds(-1).ToUnixTimeSeconds()
				)
			};

			DeepAssert.Equal(expected, vehicle.TakeChanges().ToArray());
			Assert.Equal(0, vehicle.Version);
		}

		[Fact]
		public void It_should_book_unregistered_vehicle() {
			var vehicle = Vehicle.FromStream(LicencePlate.Create("123-123"));

			vehicle.Book(LicencePlate.Create("123-123"), "Kernow", 1579132800, EndOf.Day);

			var expected = new object[] {
				new VehicleRegistered(licencePlate: "123-123"),
				new VehicleBooked(
					licencePlate: "123-123",
					location: "Kernow",
					start: new DateTimeOffset(new DateTime(2020, 01, 16)).ToUnixTimeSeconds(),
					finish: new DateTimeOffset(new DateTime(2020, 01, 17)).AddSeconds(-1).ToUnixTimeSeconds()
				)
			};

			DeepAssert.Equal(expected, vehicle.TakeChanges().ToArray());
			Assert.Equal(1, vehicle.Version);
		}
	}
}

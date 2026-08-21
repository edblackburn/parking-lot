using System;
using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace ParkingLot.Tests {
	public class When_vehicle_is_booked {
		[Test]
		public async Task it_should_book_registered_vehicle() {
			var licencePlate = LicencePlate.Create("123-123").OkValue;

			var events = VehicleDeciderScenario
				.Given(new VehicleRegistered("123-123"))
				.When(new Book(licencePlate, "Kernow", 1579132800, EndOf.Week));

			var expected = new VehicleEvent[] {
				new VehicleBooked(
					LicencePlate: "123-123",
					Location: "Kernow",
					Start: new DateTimeOffset(new DateTime(2020, 01, 16)).ToUnixTimeSeconds(),
					Finish: new DateTimeOffset(new DateTime(2020, 01, 16)).AddDays(7).ToUnixTimeSeconds())
			};

			await Assert.That(events).IsEquivalentTo(expected);
		}

		[Test]
		public async Task it_should_book_unregistered_vehicle() {
			var licencePlate = LicencePlate.Create("123-123").OkValue;

			var events = VehicleDeciderScenario
				.Given()
				.When(new Book(licencePlate, "Kernow", 1579132800, EndOf.Day));

			var expected = new VehicleEvent[] {
				new VehicleRegistered(LicencePlate: "123-123"),
				new VehicleBooked(
					LicencePlate: "123-123",
					Location: "Kernow",
					Start: new DateTimeOffset(new DateTime(2020, 01, 16)).ToUnixTimeSeconds(),
					Finish: new DateTimeOffset(new DateTime(2020, 01, 17)).ToUnixTimeSeconds())
			};

			await Assert.That(events).IsEquivalentTo(expected);
		}
	}
}

using System;
using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace ParkingLot.Tests {
	public class When_unbooked_vehicle_is_inspected {
		[Test]
		public async Task it_should_fail_inspection() {
			var inspectionId = InspectionId.Create("inspection-identifier");
			var licencePlate = LicencePlate.Create("123-456").OkValue;
			const string location = "Kernow";
			var inspectionTime = new DateTimeOffset(2020, 2, 15, 10, 00, 00, 00, TimeSpan.Zero);

			var events = VehicleDeciderScenario
				.Given(new VehicleRegistered(licencePlate.ToString()))
				.When(new Inspect(inspectionId, inspectionTime, licencePlate, location));

			var expected = new VehicleEvent[] {
				new InspectionFailed(
					InspectionId: inspectionId.ToString(),
					LicencePlate: licencePlate.ToString(),
					Location: location)
			};

			await Assert.That(events).IsEquivalentTo(expected);
		}

		[Test]
		public async Task it_should_fail_inspection_again_after_a_prior_failed_inspection() {
			var firstInspectionId = InspectionId.Create("inspection-1");
			var secondInspectionId = InspectionId.Create("inspection-2");
			var licencePlate = LicencePlate.Create("123-456").OkValue;
			const string location = "Kernow";
			var inspectionTime = new DateTimeOffset(2020, 2, 15, 10, 00, 00, 00, TimeSpan.Zero);

			var events = VehicleDeciderScenario
				.Given(
					new VehicleRegistered(licencePlate.ToString()),
					new InspectionFailed(firstInspectionId.ToString(), licencePlate.ToString(), location))
				.When(new Inspect(secondInspectionId, inspectionTime, licencePlate, location));

			var expected = new VehicleEvent[] {
				new InspectionFailed(
					InspectionId: secondInspectionId.ToString(),
					LicencePlate: licencePlate.ToString(),
					Location: location)
			};

			await Assert.That(events).IsEquivalentTo(expected);
		}

		[Test]
		public async Task it_should_fail_inspection_when_the_vehicle_has_never_been_registered() {
			var inspectionId = InspectionId.Create("inspection-identifier");
			var licencePlate = LicencePlate.Create("123-456").OkValue;
			const string location = "Kernow";
			var inspectionTime = new DateTimeOffset(2020, 2, 15, 10, 00, 00, 00, TimeSpan.Zero);

			var events = VehicleDeciderScenario
				.Given()
				.When(new Inspect(inspectionId, inspectionTime, licencePlate, location));

			var expected = new VehicleEvent[] {
				new InspectionFailed(
					InspectionId: inspectionId.ToString(),
					LicencePlate: licencePlate.ToString(),
					Location: location)
			};

			await Assert.That(events).IsEquivalentTo(expected);
		}
	}
}

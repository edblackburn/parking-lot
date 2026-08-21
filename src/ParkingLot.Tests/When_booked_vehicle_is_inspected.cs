using System;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;
using System.Threading.Tasks;

namespace ParkingLot.Tests {
	public class When_booked_vehicle_is_inspected {
		[Test]
		public async Task it_should_pass_inspection_for_all_day_product() {
			var inspectionId = InspectionId.Create("inspection-identifier");
			var licencePlate = LicencePlate.Create("123-456").OkValue;
			const string location = "Kernow";
			var inspectionTime = new DateTimeOffset(2020, 2, 15, 10, 00, 00, 00, TimeSpan.Zero);

			var events = VehicleDeciderScenario
				.Given(
					new VehicleRegistered(licencePlate.ToString()),
					// start at half-eight am, ends at midnight (exclusive).
					new VehicleBooked(
						LicencePlate: licencePlate.ToString(),
						Location: location,
						Start: 1581755400, // 02/15/2020 @ 8:30am (UTC)
						Finish: 1581811200)) // 02/16/2020 @ 12:00am (UTC)
				.When(new Inspect(inspectionId, inspectionTime, licencePlate, location));

			await Assert.That(events).IsEquivalentTo(Array.Empty<VehicleEvent>());
		}

		[Test]
		public async Task it_should_pass_inspection_for_weekly_product() {
			var inspectionId = InspectionId.Create("inspection-identifier");
			var licencePlate = LicencePlate.Create("123-456").OkValue;
			const string location = "Kernow";
			var inspectionTime = new DateTimeOffset(2020, 2, 15, 10, 00, 00, 00, TimeSpan.Zero);

			var events = VehicleDeciderScenario
				.Given(
					new VehicleRegistered(licencePlate.ToString()),
					// In the past
					new VehicleBooked(
						LicencePlate: licencePlate.ToString(),
						Location: location,
						Start: inspectionTime.AddDays(-57).ToUnixTimeSeconds(),
						Finish: inspectionTime.AddDays(-50).ToUnixTimeSeconds()),
					// Covers current inspection
					new VehicleBooked(
						LicencePlate: licencePlate.ToString(),
						Location: location,
						Start: inspectionTime.AddDays(-2).ToUnixTimeSeconds(),
						Finish: inspectionTime.AddDays(5).ToUnixTimeSeconds()),
					// In the future
					new VehicleBooked(
						LicencePlate: licencePlate.ToString(),
						Location: location,
						Start: inspectionTime.AddDays(1).ToUnixTimeSeconds(),
						Finish: inspectionTime.AddDays(8).ToUnixTimeSeconds()))
				.When(new Inspect(inspectionId, inspectionTime, licencePlate, location));

			await Assert.That(events).IsEquivalentTo(Array.Empty<VehicleEvent>());
		}

		[Test]
		public async Task it_should_fail_inspection_when_booking_ends_exactly_at_inspection_time() {
			var inspectionId = InspectionId.Create("inspection-identifier");
			var licencePlate = LicencePlate.Create("123-456").OkValue;
			const string location = "Kernow";
			var inspectionTime = new DateTimeOffset(2020, 2, 15, 11, 00, 00, 00, TimeSpan.Zero);

			var events = VehicleDeciderScenario
				.Given(
					new VehicleRegistered(licencePlate.ToString()),
					new VehicleBooked(
						LicencePlate: licencePlate.ToString(),
						Location: location,
						Start: new DateTimeOffset(2020, 2, 15, 10, 00, 00, 00, TimeSpan.Zero).ToUnixTimeSeconds(),
						Finish: inspectionTime.ToUnixTimeSeconds()))
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
		public async Task it_should_fail_inspection_when_booking_covers_a_different_location() {
			var inspectionId = InspectionId.Create("inspection-identifier");
			var licencePlate = LicencePlate.Create("123-456").OkValue;
			var inspectionTime = new DateTimeOffset(2020, 2, 15, 10, 30, 00, 00, TimeSpan.Zero);

			var events = VehicleDeciderScenario
				.Given(
					new VehicleRegistered(licencePlate.ToString()),
					new VehicleBooked(
						LicencePlate: licencePlate.ToString(),
						Location: "Plymouth",
						Start: new DateTimeOffset(2020, 2, 15, 10, 00, 00, 00, TimeSpan.Zero).ToUnixTimeSeconds(),
						Finish: new DateTimeOffset(2020, 2, 15, 11, 00, 00, 00, TimeSpan.Zero).ToUnixTimeSeconds()))
				.When(new Inspect(inspectionId, inspectionTime, licencePlate, "Exeter"));

			var expected = new VehicleEvent[] {
				new InspectionFailed(
					InspectionId: inspectionId.ToString(),
					LicencePlate: licencePlate.ToString(),
					Location: "Exeter")
			};

			await Assert.That(events).IsEquivalentTo(expected);
		}

		[Test]
		public async Task it_should_fail_inspection_when_the_inspected_plate_does_not_match_the_registered_vehicle() {
			var inspectionId = InspectionId.Create("inspection-identifier");
			var registeredPlate = LicencePlate.Create("123-456").OkValue;
			var inspectedPlate = LicencePlate.Create("999-999").OkValue;
			const string location = "Kernow";
			var inspectionTime = new DateTimeOffset(2020, 2, 15, 10, 30, 00, 00, TimeSpan.Zero);

			var events = VehicleDeciderScenario
				.Given(
					new VehicleRegistered(registeredPlate.ToString()),
					new VehicleBooked(
						LicencePlate: registeredPlate.ToString(),
						Location: location,
						Start: new DateTimeOffset(2020, 2, 15, 10, 00, 00, 00, TimeSpan.Zero).ToUnixTimeSeconds(),
						Finish: new DateTimeOffset(2020, 2, 15, 11, 00, 00, 00, TimeSpan.Zero).ToUnixTimeSeconds()))
				.When(new Inspect(inspectionId, inspectionTime, inspectedPlate, location));

			var expected = new VehicleEvent[] {
				new InspectionFailed(
					InspectionId: inspectionId.ToString(),
					LicencePlate: inspectedPlate.ToString(),
					Location: location)
			};

			await Assert.That(events).IsEquivalentTo(expected);
		}
	}
}

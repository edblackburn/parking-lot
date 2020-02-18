using System;
using System.Linq;
using Xunit;
using Xunit.Asserts.Compare;

namespace ParkingLot.Tests {
	public class When_booked_vehicle_is_inspected {
		[Fact]
		public void it_should_pass_inspection_for_all_day_product() {
			var inspectionId = InspectionId.Create("inspection-identifier");
			LicencePlate licencePlate = LicencePlate.Create("123-456");
			const string location = "Kernow";
			var inspectionTime = new DateTimeOffset(2020, 2, 15, 10, 00, 00, 00, TimeSpan.Zero);
			var stream = new object[] {
				new VehicleRegistered(licencePlate.ToString()),
				// start at half-eight pm, ends one second before midnight.
				new VehicleBooked(
					licencePlate: licencePlate.ToString(),
					location: location,
					start: 1581755400, // 02/15/2020 @ 8:30am (UTC)
					finish: 1581811199) // 02/15/2020 @ 11:59pm (UTC)
			};
			var vehicle = Vehicle.FromStream(licencePlate, stream);

			vehicle.Inspect(inspectionId, inspectionTime, licencePlate, location);

			DeepAssert.Equal(Array.Empty<object>(), vehicle.TakeChanges().ToArray());
		}

		[Fact]
		public void it_should_pass_inspection_for_weekly_product() {
			var inspectionId = InspectionId.Create("inspection-identifier");
			LicencePlate licencePlate = LicencePlate.Create("123-456");
			const string location = "Kernow";
			var inspectionTime = new DateTimeOffset(2020, 2, 15, 10, 00, 00, 00, TimeSpan.Zero);
			var stream = new object[] {
				new VehicleRegistered(licencePlate.ToString()),
				// In the past
				new VehicleBooked(
					licencePlate: licencePlate.ToString(),
					location: location,
					start: inspectionTime.AddDays(-57).ToUnixTimeSeconds(),
					finish: inspectionTime.AddDays(-50).ToUnixTimeSeconds()),
				// Covers current inspection
				new VehicleBooked(
					licencePlate: licencePlate.ToString(),
					location: location,
					start: inspectionTime.AddDays(-2).ToUnixTimeSeconds(),
					finish: inspectionTime.AddDays(5).ToUnixTimeSeconds()),
				// In the future
				new VehicleBooked(
					licencePlate: licencePlate.ToString(),
					location: location,
					start: inspectionTime.AddDays(1).ToUnixTimeSeconds(),
					finish: inspectionTime.AddDays(8).ToUnixTimeSeconds())
			};
			var vehicle = Vehicle.FromStream(licencePlate, stream);

			vehicle.Inspect(inspectionId, inspectionTime, licencePlate, location);

			DeepAssert.Equal(Array.Empty<object>(), vehicle.TakeChanges().ToArray());
		}
	}
}

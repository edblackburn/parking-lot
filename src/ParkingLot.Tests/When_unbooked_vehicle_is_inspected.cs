using System;
using System.Linq;
using Xunit;
using Xunit.Asserts.Compare;

namespace ParkingLot.Tests
{
	public class When_unbooked_vehicle_is_inspected {
		[Fact]
		public void it_should_fail_inspection() {
			var inspectionId = InspectionId.Create("inspection-identifier");
			LicencePlate licencePlate = LicencePlate.Create("123-456");
			const string location = "Kernow";
			var inspectionTime = new DateTimeOffset(2020, 2, 15, 10, 00, 00, 00, TimeSpan.Zero);
			var vehicle = Vehicle.FromStream(licencePlate);

			vehicle.Inspect(inspectionId, inspectionTime, licencePlate, location);

			var expected = new object[] {
				new VehicleRegistered(licencePlate: licencePlate.ToString()),
				new VehicleUnbooked(
					inspectionId: inspectionId.ToString(),
					licencePlate: licencePlate.ToString(),
					location: location)
			};
			DeepAssert.Equal(expected, vehicle.TakeChanges().ToArray());
		}
	}
}

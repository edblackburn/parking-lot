using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace ParkingLot.Tests {
	public class When_inspection_id_is_created {
		[Test]
		public async Task it_should_treat_the_same_identifier_as_equal() {
			var first = InspectionId.Create("inspection-1");
			var second = InspectionId.Create("inspection-1");

			await Assert.That(first).IsEqualTo(second);
		}

		[Test]
		public async Task it_should_treat_different_identifiers_as_unequal() {
			var first = InspectionId.Create("inspection-1");
			var second = InspectionId.Create("inspection-2");

			await Assert.That(first).IsNotEqualTo(second);
		}
	}
}

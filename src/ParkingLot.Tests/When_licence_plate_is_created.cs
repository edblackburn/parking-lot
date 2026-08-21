using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace ParkingLot.Tests {
	public class When_licence_plate_is_created {
		[Test]
		public async Task it_should_normalise_a_valid_plate() {
			var result = LicencePlate.Create(" ab-12 ");

			await Assert.That(result.IsOk).IsTrue();
			await Assert.That(result.OkValue.ToString()).IsEqualTo("AB-12");
		}

		[Test]
		public async Task it_should_reject_an_empty_plate() {
			var result = LicencePlate.Create("--");

			await Assert.That(result.IsOk).IsFalse();
			await Assert.That(result.ErrorValue.Message).IsEqualTo("Invalid licence plate '--'");
		}

		[Test]
		public async Task it_should_reject_a_single_character_plate() {
			var result = LicencePlate.Create("a");

			await Assert.That(result.IsOk).IsFalse();
			await Assert.That(result.ErrorValue.Message).IsEqualTo("Invalid licence plate 'a'");
		}

		[Test]
		public async Task it_should_treat_plates_with_the_same_normalised_value_as_equal() {
			var first = LicencePlate.Create("ab-12").OkValue;
			var second = LicencePlate.Create("AB-12").OkValue;

			await Assert.That(first).IsEqualTo(second);
		}

		[Test]
		public async Task it_should_treat_plates_with_different_normalised_values_as_unequal() {
			var first = LicencePlate.Create("ab-12").OkValue;
			var second = LicencePlate.Create("cd-34").OkValue;

			await Assert.That(first).IsNotEqualTo(second);
		}
	}
}

using System;
using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace ParkingLot.Tests {
	public class When_result_is_used {
		[Test]
		public async Task it_should_match_the_ok_branch() {
			var result = Result<int, string>.Ok(42);

			var matched = result.Match(onOk: v => $"ok:{v}", onError: e => $"error:{e}");

			await Assert.That(matched).IsEqualTo("ok:42");
		}

		[Test]
		public async Task it_should_match_the_error_branch() {
			var result = Result<int, string>.Error("failed");

			var matched = result.Match(onOk: v => $"ok:{v}", onError: e => $"error:{e}");

			await Assert.That(matched).IsEqualTo("error:failed");
		}

		[Test]
		public async Task it_should_expose_the_error_value_when_an_error() {
			var result = Result<int, string>.Error("failed");

			await Assert.That(result.ErrorValue).IsEqualTo("failed");
		}

		[Test]
		public async Task it_should_throw_when_reading_ok_value_of_an_error() {
			var result = Result<int, string>.Error("failed");

			await Assert.That(() => result.OkValue).Throws<InvalidOperationException>();
		}

		[Test]
		public async Task it_should_throw_when_reading_error_value_of_an_ok() {
			var result = Result<int, string>.Ok(42);

			await Assert.That(() => result.ErrorValue).Throws<InvalidOperationException>();
		}
	}
}

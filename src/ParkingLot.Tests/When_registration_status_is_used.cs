using System;
using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace ParkingLot.Tests {
	public class When_registration_status_is_used {
		[Test]
		public async Task it_should_match_the_unregistered_branch() {
			var status = RegistrationStatus.Unregistered;

			var matched = status.Match(onUnregistered: () => "unregistered", onRegistered: plate => $"registered:{plate}");

			await Assert.That(matched).IsEqualTo("unregistered");
		}

		[Test]
		public async Task it_should_match_the_registered_branch() {
			var status = RegistrationStatus.Registered("123-456");

			var matched = status.Match(onUnregistered: () => "unregistered", onRegistered: plate => $"registered:{plate}");

			await Assert.That(matched).IsEqualTo("registered:123-456");
		}

		[Test]
		public async Task it_should_expose_the_licence_plate_when_registered() {
			var status = RegistrationStatus.Registered("123-456");

			await Assert.That(status.LicencePlate).IsEqualTo("123-456");
		}

		[Test]
		public async Task it_should_throw_when_reading_licence_plate_of_an_unregistered_vehicle() {
			var status = RegistrationStatus.Unregistered;

			await Assert.That(() => status.LicencePlate).Throws<InvalidOperationException>();
		}
	}
}

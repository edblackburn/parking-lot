using System;
using OneOf.Types;

namespace ParkingLot {
	public sealed record RegistrationStatus {
		private readonly TwoCase<None, string> _value;

		public static readonly RegistrationStatus Unregistered =
			new RegistrationStatus(TwoCase<None, string>.First(new None()));

		public static RegistrationStatus Registered(string licencePlate) =>
			new RegistrationStatus(TwoCase<None, string>.Second(licencePlate));

		private RegistrationStatus(TwoCase<None, string> value) => _value = value;

		public bool IsRegistered => !_value.IsFirst;

		public TResult Match<TResult>(Func<TResult> onUnregistered, Func<string, TResult> onRegistered) =>
			_value.Match(_ => onUnregistered(), onRegistered);

		public string LicencePlate => _value.AsSecond(
			() => new InvalidOperationException("Vehicle is not yet registered; there is no licence plate."));
	}
}

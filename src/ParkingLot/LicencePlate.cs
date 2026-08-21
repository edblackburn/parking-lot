using System.Linq;

namespace ParkingLot {
	public sealed record LicencePlateError(string Message);

	public sealed record LicencePlate {
		private readonly string _licencePlate;

		public static Result<LicencePlate, LicencePlateError> Create(string licencePlate) {
			var plate = licencePlate
				.Trim()
				.Trim('-')
				.Where(c => char.IsLetterOrDigit(c) || c == '-')
				.Select(c => !char.IsLetter(c) ? c : char.ToUpper(c))
				.ToArray();

			if (!plate.Any() || 1 == plate.Length) {
				return Result<LicencePlate, LicencePlateError>.Error(
					new LicencePlateError($"Invalid licence plate '{licencePlate}'"));
			}

			return Result<LicencePlate, LicencePlateError>.Ok(new LicencePlate(new string(plate)));
		}

		private LicencePlate(string licencePlate) => _licencePlate = licencePlate;

		public override string ToString() => _licencePlate;
	}
}

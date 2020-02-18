using System;
using System.Linq;

namespace ParkingLot {
	public class LicencePlate {
		private readonly string _licencePlate;

		public static readonly LicencePlate Unassigned = new LicencePlate("unassigned");

		public static LicencePlate Create(string licencePlate) {
			var plate = licencePlate
				.Trim()
				.Trim('-')
				.Where(c => char.IsLetterOrDigit(c) || c == '-')
				.Select(c => !char.IsLetter(c) ? c : char.ToUpper(c))
				.ToArray();

			if (!plate.Any() || 1 == plate.Length) {
				throw new ArgumentException($"Invalid licence plate '{licencePlate}'");
			}

			return new LicencePlate(new string(plate));
		}

		private LicencePlate(string licencePlate) => _licencePlate = licencePlate;

		public override string ToString() => _licencePlate;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingLot {
	public class Vehicle {
		public static Vehicle FromStream(LicencePlate licencePlate, params object[] events) {
			var vehicle = new Vehicle();

			var stream = events ?? Array.Empty<object>();

			if (stream.Any()) {
				foreach (var @event in stream) {
					vehicle.When(@event);
				}
			} else {
				vehicle.Apply(new VehicleRegistered(licencePlate.ToString()));
			}

			return vehicle;
		}

		private Vehicle() {
		}

		private readonly Queue<object> _events = new Queue<object>();
		private LicencePlate _licencePlate = LicencePlate.Unassigned;
		private readonly List<(long start, long finish)> _bookings = new List<(long start, long finish)>();

		public void Book(in LicencePlate licencePlate, in string location, in long start, in Product product) {
			var finish = product(start);
			Apply(new VehicleBooked(
				licencePlate: licencePlate.ToString(),
				location: location,
				start: start,
				finish: finish));
		}

		public void Inspect(
			InspectionId inspectionId,
			DateTimeOffset when,
			LicencePlate licencePlate,
			string location) {
			var unixTimeSeconds = when.ToUnixTimeSeconds();
			var inRange = _bookings.Any(booking =>
				unixTimeSeconds >= booking.start && unixTimeSeconds <= booking.finish);

			if (!inRange) {
				Apply(new VehicleUnbooked(inspectionId.ToString(), licencePlate.ToString(), location));
			}
		}

		private void Apply(object @event) {
			When(@event);
			_events.Enqueue(@event);
			Version++;
		}

		/// <summary>
		/// Assign state
		/// </summary>
		/// <param name="event"></param>
		private void When(object @event) {
			//NOTE: we never, ever, ever throw in here
			switch (@event) {
				case VehicleRegistered registered:
					_licencePlate = LicencePlate.Create(registered.LicencePlate);
					break;
				case VehicleBooked booked:
					_bookings.Add((booked.Start, booked.Finish));
					break;
			}
		}

		public int Version { get; private set; } = -1;

		public IEnumerable<object> TakeChanges() {
			while (_events.Any()) {
				yield return _events.Dequeue();
			}
		}
	}
}

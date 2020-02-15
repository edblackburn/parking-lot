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

		public void Book(LicencePlate licencePlate, string location, int start, Product product) {
			var finish = product(start);

			Apply(new VehicleBooked(
				licencePlate: licencePlate.ToString(),
				location: location,
				start: start,
				finish: finish));
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

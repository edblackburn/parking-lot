namespace ParkingLot {
	public sealed record Period(long Start, long Finish) {
		public bool Contains(long when) => Start <= when && when < Finish;
	}
}

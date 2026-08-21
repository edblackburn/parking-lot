using System;
using OneOf;

namespace ParkingLot {
	internal sealed record TwoCase<TFirst, TSecond> {
		private readonly OneOf<TFirst, TSecond> _value;

		public static TwoCase<TFirst, TSecond> First(TFirst value) => new TwoCase<TFirst, TSecond>(value);

		public static TwoCase<TFirst, TSecond> Second(TSecond value) => new TwoCase<TFirst, TSecond>(value);

		private TwoCase(OneOf<TFirst, TSecond> value) => _value = value;

		public bool IsFirst => _value.IsT0;

		public TResult Match<TResult>(Func<TFirst, TResult> onFirst, Func<TSecond, TResult> onSecond) =>
			_value.Match(onFirst, onSecond);

		public TFirst AsFirst(Func<Exception> onSecond) =>
			IsFirst ? _value.AsT0 : throw onSecond();

		public TSecond AsSecond(Func<Exception> onFirst) =>
			!IsFirst ? _value.AsT1 : throw onFirst();
	}
}

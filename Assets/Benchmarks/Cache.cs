using NUnit.Framework;

using Unity.PerformanceTesting;

public class Cache
{
	private const int VALUES_COUNT = 1_000_000;
	private const int SKIP = 10;
	private long[] _values;

	[SetUp]
	public void SetUp() => _values = new long[VALUES_COUNT * SKIP];

	[Test, Performance]
	public void Sum_Con_Test() => Measure.Method( Sum_Con ).Run();

	[Test, Performance]
	public void Sum_Skip_Test() => Measure.Method( Sum_Skip ).Run();

	private void Sum_Con()
	{
		long sum = 0;
		for ( int i = 0; i < VALUES_COUNT; ++i )
		{
			sum += _values[i];
		}
	}

	private void Sum_Skip()
	{
		long sum = 0;
		for ( int i = 0; i < VALUES_COUNT * SKIP; i += SKIP )
		{
			sum += _values[i];
		}
	}
}
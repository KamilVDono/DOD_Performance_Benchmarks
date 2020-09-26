using NUnit.Framework;

using System.Linq;
using System.Runtime.CompilerServices;

using Unity.PerformanceTesting;

public class Branches
{
	private const int SAMPLES = 160;
	private const int COUNT = 10_000_000 * SAMPLES;

	private bool[] _true;
	private bool[] _false;
	private bool[] _half;
	private bool[] _every_fourth_true;
	private bool[] _every_fourth_false;
	private bool[] _every_eight_true;
	private bool[] _every_eight_false;
	private bool[] _random;
	private byte[] _byteRandom;

	[SetUp]
	public void SetUp()
	{
		var rng = new System.Random();

		_true = new bool[SAMPLES];
		_false = new bool[SAMPLES];
		_half = new bool[SAMPLES];
		_every_fourth_true = new bool[SAMPLES];
		_every_fourth_false = new bool[SAMPLES];
		_every_eight_true = new bool[SAMPLES];
		_every_eight_false = new bool[SAMPLES];
		_random = new bool[SAMPLES];
		_byteRandom = new byte[SAMPLES];

		for ( int i = 0; i < SAMPLES; i++ )
		{
			_true[i] = true;
			_false[i] = false;
			_half[i] = i % 2 == 0;
			_every_fourth_true[i] = i % 4 == 0;
			_every_fourth_true[i] = i % 4 != 0;
			_every_eight_true[i] = i % 8 == 0;
			_every_eight_false[i] = i % 8 != 0;
			_random[i] = rng.NextDouble() >= 0.5;
			_byteRandom[i] = (byte)rng.Next( 2 );
		}

		_random = _random.OrderBy( x => rng.Next() ).ToArray();
		_byteRandom = _byteRandom.OrderBy( x => rng.Next() ).ToArray();
	}

	[Test, Performance]
	public void Sum_True_Test() => Measure.Method( Sum_True ).Run();

	[Test, Performance]
	public void Sum_False_Test() => Measure.Method( Sum_False ).Run();

	[Test, Performance]
	public void Sum_Half_Test() => Measure.Method( Sum_Half ).Run();

	[Test, Performance]
	public void Sum_4_True_Test() => Measure.Method( Sum_4_True ).Run();

	[Test, Performance]
	public void Sum_4_Flase_Test() => Measure.Method( Sum_4_False ).Run();

	[Test, Performance]
	public void Sum_8_True_Test() => Measure.Method( Sum_8_True ).Run();

	[Test, Performance]
	public void Sum_8_Flase_Test() => Measure.Method( Sum_8_False ).Run();

	[Test, Performance]
	public void Sum_Random_Test() => Measure.Method( Sum_Random ).Run();

	[Test, Performance]
	public void Sum_Random_Byte_Test() => Measure.Method( Sum_Random_Byte ).Run();

	private void Sum_True() => Sum( _true );

	private void Sum_False() => Sum( _false );

	private void Sum_Half() => Sum( _half );

	private void Sum_4_True() => Sum( _every_fourth_true );

	private void Sum_4_False() => Sum( _every_fourth_false );

	private void Sum_8_True() => Sum( _every_eight_true );

	private void Sum_8_False() => Sum( _every_eight_false );

	private void Sum_Random() => Sum( _random );

	private void Sum_Random_Byte()
	{
		long sum = 0;
		for ( int i = 0; i < COUNT; i++ )
		{
			var byteBool = _byteRandom[i % SAMPLES];
			sum += ( OptionA() * byteBool ) + ( OptionB() * ( 1 - byteBool ) );
		}
	}

	private void Sum( bool[] values )
	{
		long sum = 0;
		for ( int i = 0; i < COUNT; i++ )
		{
			if ( values[i % SAMPLES] )
			{
				sum += OptionA();
			}
			else
			{
				sum += OptionB();
			}
		}
	}

	[MethodImpl( MethodImplOptions.NoInlining )]
	private byte OptionA() => 1 * 1;

	[MethodImpl( MethodImplOptions.NoInlining )]
	private byte OptionB() => 1 * 2;
}
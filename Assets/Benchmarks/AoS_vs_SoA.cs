using NUnit.Framework;

using System;

using Unity.PerformanceTesting;

public class AoS_vs_SoA
{
	private const int RUNS = 120 * 10;
	private const int PERSONS = 100000;
	private static readonly int[] EARNINGS = {1, 2, 3 };

	[Test, Performance]
	public void Person_AoS_Test() => Measure.Method( Person_AoS_Tets_Method ).Run();

	[Test, Performance]
	public void Person_AoS_References_Test() => Measure.Method( Person_AoS_References_Tets_Method ).Run();

	[Test, Performance]
	public void Persons_SoA_Test() => Measure.Method( Persons_SoA_Tets_Method ).Run();

	private void Person_AoS_Tets_Method()
	{
		Person_AoS[] persons = new Person_AoS[PERSONS];
		for ( int i = 0; i < PERSONS; i++ )
		{
			persons[i] = new Person_AoS( EARNINGS[i % 3] );
		}

		long sum = 0;
		for ( int r = 0; r < RUNS; r++ )
		{
			int acc = 0;
			for ( int i = 0; i < PERSONS; i++ )
			{
				acc += persons[i].AnnualEarnings;
			}
			sum += acc;
		}
	}

	private void Person_AoS_References_Tets_Method()
	{
		Person_AoS_References[] persons = new Person_AoS_References[PERSONS];
		for ( int i = 0; i < PERSONS; i++ )
		{
			persons[i] = new Person_AoS_References( EARNINGS[i % 3] );
		}

		long sum = 0;
		for ( int r = 0; r < RUNS; r++ )
		{
			int acc = 0;
			for ( int i = 0; i < PERSONS; i++ )
			{
				acc += persons[i].Earnings.Annual;
			}
			sum += acc;
		}
	}

	private void Persons_SoA_Tets_Method()
	{
		Persons_SoA persons = new Persons_SoA(PERSONS);

		long sum = 0;
		for ( int r = 0; r < RUNS; r++ )
		{
			var earnings = persons.AnnualEarnings;
			int acc = 0;
			for ( int i = 0; i < PERSONS; i++ )
			{
				acc += earnings[i];
			}
			sum += acc;
		}
	}

	public enum Gender : byte
	{
		Male,
		Female,
		Other
	}

	private class Person_AoS
	{
		public string Name = "Name";
		public string MiddleName = "MiddleName";
		public string LastName = "LastName";
		public DateTime BornYear = DateTime.Now;
		public Gender Gender = Gender.Other;
		public int AnnualEarnings = 0;

		public Person_AoS( int earnings ) => AnnualEarnings = earnings;
	}

	private class Person_AoS_References
	{
		public string Name = "Name";
		public string MiddleName = "MiddleName";
		public string LastName = "LastName";
		public DateTime BornYear = DateTime.Now;
		public Gender Gender = Gender.Other;
		public Earnings Earnings;

		public Person_AoS_References( int earnings ) => Earnings = new Earnings( earnings );
	}

	private class Earnings
	{
		public int Annual;
		public int Monthly;

		public Earnings( int annual )
		{
			Annual = annual;
			Monthly = Annual / 12;
		}
	}

	private class Persons_SoA
	{
		public string[] Names;
		public string[] MiddleName;
		public string[] LastName;
		public DateTime[] BornYear;
		public Gender[] Gender;
		public int[] AnnualEarnings;
		public int[] MonthlyEarnings;

		public Persons_SoA( int personsCount )
		{
			Names = new string[personsCount];
			MiddleName = new string[personsCount];
			LastName = new string[personsCount];
			BornYear = new DateTime[personsCount];
			Gender = new Gender[personsCount];
			AnnualEarnings = new int[personsCount];
			MonthlyEarnings = new int[personsCount];
			for ( int i = 0; i < personsCount; i++ )
			{
				Names[i] = "Name";
				MiddleName[i] = "MiddleName";
				LastName[i] = "LastName";
				BornYear[i] = DateTime.Now;
				Gender[i] = AoS_vs_SoA.Gender.Other;
				AnnualEarnings[i] = EARNINGS[i % 3];
				MonthlyEarnings[i] = AnnualEarnings[i] / 12;
			}
		}
	}
}
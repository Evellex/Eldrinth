﻿namespace Augmenta.Combat
{
	[System.Flags]
	public enum Faction
	{
		Team1 = 1 << 0,
		Team2 = 1 << 1,
		Team3 = 1 << 2,
		Team4 = 1 << 3,
	}
}
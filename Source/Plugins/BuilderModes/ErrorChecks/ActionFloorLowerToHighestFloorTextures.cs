
#region ================== Namespaces

using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Map;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	public class ActionFloorLowerToHighestTextures : BaseActionTextures
	{
		#region ================== Methods

		// Gather the lower to highest floor specials from the configuration.
		protected override bool InspectsAction(LinedefActionInfo info)
		{
			return info.ErrorCheckerExemptions.FloorLowerToHighest;
		}

		// Determine whether a lower texture is needed after the sector lowers to the highest neighbour floor.
		protected override bool HasAdjustedSector(Sidedef side, ActionTrigger actiontrigger)
		{
			int nextheight = side.Sector.FloorHeight;

			// Find height of the neighbouring sector with the highest floor.
			foreach (Sidedef s in side.Sector.Sidedefs)
			{
				if (s.Other != null && s.Other.Sector != side.Sector && s.Other.Sector.FloorHeight > nextheight)
				{
					nextheight = s.Other.Sector.FloorHeight;
				}
			}

			return side.Other.Sector.FloorHeight < (nextheight + actiontrigger.Units);
		}

		#endregion
	}
}

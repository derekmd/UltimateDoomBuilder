
#region ================== Namespaces

using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Map;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	public class ActionFloorLowerToNearestTextures : BaseActionTextures
	{
		#region ================== Methods

		// Gather the lower to nearest floor specials from the configuration.
		protected override bool InspectsAction(LinedefActionInfo info)
		{
			return info.ErrorCheckerExemptions.FloorLowerToNearest;
		}

		// Determine whether a lower texture is needed after the sector lowers to the nearest neighbour floor.
		protected override bool HasAdjustedSector(Sidedef side, ActionTrigger actiontrigger)
		{
			int? nextheight = null;

			// Find height of the nearest neighbouring sector with a lower floor.
			foreach (Sidedef s in side.Sector.Sidedefs)
			{
				if (s.Other != null &&
					s.Other.Sector != side.Sector &&
					s.Other.Sector.FloorHeight < s.Sector.FloorHeight &&
					(!nextheight.HasValue || s.Other.Sector.FloorHeight > nextheight))
				{
					nextheight = s.Other.Sector.FloorHeight;
				}
			}

			return nextheight.HasValue;
		}

		#endregion
	}
}

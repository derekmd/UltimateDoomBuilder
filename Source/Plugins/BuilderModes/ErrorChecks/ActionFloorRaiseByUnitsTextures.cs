
#region ================== Namespaces

using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Map;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	public class ActionFloorRaiseByUnitsTextures : BaseActionTextures
	{
		#region ================== Methods

		// Gather the raise floor by units floor specials from the configuration.
		protected override bool InspectsAction(LinedefActionInfo info)
		{
			return info.ErrorCheckerExemptions.FloorRaiseByUnits;
		}

		// Determine whether a lower texture is needed after the sector raises by a number of units.
		protected override bool HasAdjustedSector(Sidedef side, ActionTrigger actiontrigger)
		{
			// Number of units must come from this specific trigger's metadata.
			if (!actiontrigger.ActionInfo.ErrorCheckerExemptions.FloorRaiseByUnits || actiontrigger.Units == 0)
				return false;

			int destinationheight = side.Sector.FloorHeight + actiontrigger.Units;

			// Find if any neighbouring sector is below the destination floor height.
			foreach (Sidedef s in side.Sector.Sidedefs)
			{
				if (s.Other != null && s.Other.Sector.FloorHeight < destinationheight)
				{
					return true;
				}
			}

			return false;
		}

		#endregion
	}
}


#region ================== Namespaces

using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Map;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	public class ActionCeilingLowerToFloorTextures : BaseActionTextures
	{
		#region ================== Methods

		// Gather the door close and ceiling lower to floor specials from the configuration.
		protected override bool InspectsAction(LinedefActionInfo info)
		{
			return info.ErrorCheckerExemptions.CeilingLowerToFloor;
		}

		// Determine whether an upper texture is needed after the sector ceiling lowers to the floor.
		protected override bool HasAdjustedSector(Sidedef side, ActionTrigger actiontrigger)
		{
			// Number of units must come from this specific trigger's metadata.
			if (!actiontrigger.ActionInfo.ErrorCheckerExemptions.CeilingLowerToFloor)
				return false;

			// Crusher actions may define a "lip" gap above the floor.
			int destinationheight = side.Sector.FloorHeight + actiontrigger.Units;

			// Find height of the neighbouring sector with the lowest floor.
			foreach (Sidedef s in side.Sector.Sidedefs)
			{
				if (s.Other != null && s.Other.Sector != side.Sector && s.Other.Sector.CeilHeight > destinationheight)
				{
					return true;
				}
			}

			return false;
		}

		#endregion
	}
}

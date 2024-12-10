
#region ================== Namespaces

using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Map;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	public class ActionFloorRaiseToLowestCeilingTextures : BaseActionTextures
	{
		#region ================== Methods

		// Gather the raise floor to lowest ceiling floor specials from the configuration.
		protected override bool InspectsAction(LinedefActionInfo info)
		{
			return info.ErrorCheckerExemptions.FloorRaiseToLowestCeiling;
		}

		// Determine whether a lower texture is needed after the sector raises to the lowest neighbour ceiling.
		protected override bool HasAdjustedSector(Sidedef side, ActionTrigger actiontrigger)
		{
			int? nextheight = null;

			// Find height of the lowest neighbouring sector ceiling.
			foreach (Sidedef s in side.Sector.Sidedefs)
			{
				if (s.Other != null &&
					s.Other.Sector != side.Sector &&
					(!nextheight.HasValue || s.Other.Sector.CeilHeight < nextheight))
				{
					nextheight = s.Other.Sector.CeilHeight;
				}
			}

			// Instant-move sectors to a ceiling below the floor are ignored because this is a floor lower action.
			return nextheight.HasValue && side.Other.Sector.FloorHeight < (nextheight - actiontrigger.Units);
		}

		#endregion
	}
}

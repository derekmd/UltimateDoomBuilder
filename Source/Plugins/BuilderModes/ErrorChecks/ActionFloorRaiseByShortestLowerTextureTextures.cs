
#region ================== Namespaces

using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Map;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	public class ActionFloorRaiseByShortestLowerTextureTextures : BaseActionTextures
	{
		#region ================== Methods

		// Gather the raise floor to shortest lower texture specials from the configuration.
		protected override bool InspectsAction(LinedefActionInfo info)
		{
			return info.ErrorCheckerExemptions.FloorRaiseByShortestLowerTexture;
		}

		// Determine whether a lower texture is needed after the sector raises to the shortest lower texture.
		protected override bool HasAdjustedSector(Sidedef side, ActionTrigger actiontrigger)
		{
			// Find if any sidedef within the sector contains a lower texture.
			foreach (Sidedef s in side.Sector.Sidedefs)
			{
				if (s.LongLowTexture != MapSet.EmptyLongName)
				{
					return true;
				}
			}

			return false;
		}

		#endregion
	}
}

// 
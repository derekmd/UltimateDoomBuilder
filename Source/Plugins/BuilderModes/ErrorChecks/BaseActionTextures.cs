
#region ================== Namespaces

using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Types;
using System.Collections.Generic;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	public abstract class BaseActionTextures
	{
		protected struct ActionTrigger
		{
			public ActionTrigger(LinedefActionInfo actionInfo, int tag, int units)
			{
				ActionInfo = actionInfo;
				Tag = tag;
				Units = units;
			}

			public LinedefActionInfo ActionInfo { get; }
			public int Tag { get; }
			public int Units { get; }
		}

		#region ================== Variables

		private List<ActionTrigger> actiontriggers;

		#endregion

		#region ================== Constructor / Destructor

		public BaseActionTextures()
		{
			actiontriggers = FindActionTriggers();
		}

		#endregion

		#region ================== Methods

		// Determine if the sidedef's sector tag is for an inspected action and the sidedef will require a texture.
		public bool RequiresTexture(Sidedef side, int tag)
		{
			return side.Other != null &&
				side.Other.Sector != side.Sector &&
				!side.Other.Sector.Tags.Contains(tag) &&
				actiontriggers.Exists(actiontrigger => actiontrigger.Tag == tag &&
					HasAdjustedSector(side, actiontrigger));
		}

		// Gather all sector tags for the linedef actions being inspected.
		private List<ActionTrigger> FindActionTriggers()
		{
			List<int> actions = FindActions();
			List<ActionTrigger> actiontriggers = new List<ActionTrigger>();
			LinedefActionInfo actionInfo;
			int tag;

			foreach (Linedef ld in General.Map.Map.Linedefs)
			{
				if (ld.Action > 0 && actions.Contains(ld.Action))
				{
					actionInfo = General.Map.Config.GetLinedefActionInfo(ld.Action);

					if (General.Map.HEXEN || General.Map.UDMF)
						tag = FindArgumentsSectorTag(actionInfo, ld.Args);
					else
						tag = ld.Tag;

					if (tag > 0)
						actiontriggers.Add(new ActionTrigger(actionInfo, tag, FindArgumentsUnits(actionInfo, ld.Args)));
				}
			}

			if (General.Map.HEXEN || General.Map.UDMF)
			{
				foreach (Thing t in General.Map.Map.Things)
				{
					if (t.Action > 0 && actions.Contains(t.Action))
					{
						actionInfo = General.Map.Config.GetLinedefActionInfo(t.Action);
						tag = FindArgumentsSectorTag(actionInfo, t.Args);

						if (tag > 0)
							actiontriggers.Add(new ActionTrigger(actionInfo, tag, FindArgumentsUnits(actionInfo, t.Args)));
					}
				}
			}

			return actiontriggers;
		}

		// Get the sector tag from linedef/things action arguments.
		private int FindArgumentsSectorTag(LinedefActionInfo actionInfo, int[] args)
		{
			if (actionInfo.Args[0].Type == (int)UniversalType.SectorTag)
				return args[0];

			return 0;
		}

		// Get the sector floor/ceiling units shift from linedef/things action arguments.
		private int FindArgumentsUnits(LinedefActionInfo actionInfo, int[] args)
		{
			int i = actionInfo.ErrorCheckerExemptions.ActionUnitsArg;

			// Actions that lower/raise by hardcoded units have priority.
			if (actionInfo.ErrorCheckerExemptions.ActionUnits > 0)
				return actionInfo.ErrorCheckerExemptions.ActionUnits;

			// Actions with units defined as a custom argument.
			if (i >= 0 && i < actionInfo.Args.Length && actionInfo.Args[i].Type == (int)UniversalType.Integer && args[i] > 0)
				return args[i];

			return 0;
		}

		// Gather the linedef specials from the configuration that this will inspect.
		private List<int> FindActions()
		{
			List<int> actions = new List<int>();

			foreach (LinedefActionInfo info in General.Map.Config.LinedefActions.Values)
				if (InspectsAction(info))
					actions.Add(info.Index);

			return actions;
		}

		// Determine if tagged sectors for the given action will have its textures analyzed.
		protected abstract bool InspectsAction(LinedefActionInfo info);

		// Determine whether an upper or lower texture is required after the sector tag is activated.
		protected abstract bool HasAdjustedSector(Sidedef side, ActionTrigger actiontrigger);

		#endregion
	}
}

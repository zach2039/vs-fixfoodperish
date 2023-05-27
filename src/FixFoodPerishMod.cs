
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace FixFoodPerish
{
    class FixFoodPerishMod : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);

            api.Logger.Notification("Fixing 1.18.5 food perish bug");

            var harmony = new Harmony("fixfoodperish");
			harmony.PatchAll(Assembly.GetExecutingAssembly());

			api.Logger.Notification("Patched");
		}
    }

	[HarmonyPatch(typeof(BlockEntityContainer), "Inventory_OnAcquireTransitionSpeed")]
	class Inventory_OnAcquireTransitionSpeedPatch
	{
		static bool Prefix(BlockEntityContainer __instance, ref float __result, EnumTransitionType transType, float baseMul)
		{
			float positionAwarePerishRate = (__instance.Api != null && transType == EnumTransitionType.Perish) ? __instance.GetPerishRate() : 1f;
			if (transType == EnumTransitionType.Dry)
			{
				positionAwarePerishRate = 0.25f;
			}
			__result = baseMul * positionAwarePerishRate;

			return false;
		}
	}
}

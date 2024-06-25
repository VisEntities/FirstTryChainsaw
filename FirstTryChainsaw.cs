using System.Collections.Generic;

namespace Oxide.Plugins
{
    [Info("First Try Chainsaw", "VisEntities", "1.0.0")]
    [Description(" ")]
    public class FirstTryChainsaw : RustPlugin
    {
        #region Fields

        private static FirstTryChainsaw _plugin;

        #endregion Fields

        #region Oxide Hooks

        private void Init()
        {
            _plugin = this;
            PermissionUtil.RegisterPermissions();
        }

        private void Unload()
        {
            _plugin = null;
        }

        private void OnEntityActiveCheck(BaseEntity entity, BasePlayer player, uint id, string debugName)
        {
            if (entity.ShortPrefabName != "chainsaw.entity" || player == null || debugName != "Server_StartEngine")
                return;

            if (!PermissionUtil.HasPermission(player, PermissionUtil.USE))
                return;
 
            Chainsaw chainsaw = entity as Chainsaw;
            if (chainsaw != null)
            {
                chainsaw.engineStartChance = 1.0f;

                NextTick(() =>
                {
                    if (chainsaw != null)
                        chainsaw.engineStartChance = 0.33f;
                });
            }      
        }

        #endregion Oxide Hooks

        #region Permissions

        private static class PermissionUtil
        {
            public const string USE = "firsttrychainsaw.use";
            private static readonly List<string> _permissions = new List<string>
            {
                USE,
            };

            public static void RegisterPermissions()
            {
                foreach (var permission in _permissions)
                {
                    _plugin.permission.RegisterPermission(permission, _plugin);
                }
            }

            public static bool HasPermission(BasePlayer player, string permissionName)
            {
                return _plugin.permission.UserHasPermission(player.UserIDString, permissionName);
            }
        }

        #endregion Permissions
    }
}
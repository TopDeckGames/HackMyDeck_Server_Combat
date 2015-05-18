using System;

namespace CombatServer.Manager
{
    public static class ManagerFactory
    {
        private static MasterManager masterManager;

        /// <summary>
        /// Retourne le manager du serveur maitre
        /// </summary>
        /// <returns></returns>
        public static MasterManager getMasterManager()
        {
            if(ManagerFactory.masterManager == null)
            {
                ManagerFactory.masterManager = new MasterManager();
            }
            return ManagerFactory.masterManager;
        }
    }
}
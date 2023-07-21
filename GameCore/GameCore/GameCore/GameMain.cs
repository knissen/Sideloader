using System;
using System.Collections.Generic;
using DefaultNamespace;

namespace GameCore
{
    public class GameMain : IGameCore
    {
        public static GameMain Instance { get; private set; }

        public float deltaTime;
        private GameObject[] viewMarkers;

        //private static Dictionary<Type, GameModel> registry = new Dictionary<Type, GameModel>();

        //public static void Register<T>(T model) where T : GameModel
        //{
        //    registry[model.GetType()] = model;
        //}

        public void Load(object previous)
        {
            Instance = this;

            try
            {
                viewMarkers = Resources.FindObjectsOfTypeAll<ViewMarker>().Select(x => x.gameObject).ToArray();
                rootModel = new RootModel(registry);

                if (previous != null)
                    rootModel.LoadReloadState(previous as Dictionary<string, object>);
                rootModel.SafeInit();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public object Save(bool destroy)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}

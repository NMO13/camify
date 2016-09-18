using System;
using RenderEngine.GraphicObjects.ObjectTypes;
using RenderEngine.GraphicObjects.ObjectTypes.Dynamic;
using RenderEngine.GraphicObjects.ObjectTypes.Static;

namespace RenderEngine.GraphicObjects
{
    class RenderObjectFactory
    {
        private static RenderObjectFactory _instance;
        internal static RenderObjectFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RenderObjectFactory();
                }
                return _instance;
            }
        }
        public StaticRenderObject BuildStaticRenderObject(ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.Background: return new Background();
                case ObjectType.CoordinateSystem: return new CoordinateSystem();
                default: throw  new ArgumentException("Object type not supported");
            }
        }

        public DynamicRenderObject BuildDynamicRenderObject(DynamicObjectDataContainer container)
        {
            return new DynamicRenderObject(container);   
        }
    }
}

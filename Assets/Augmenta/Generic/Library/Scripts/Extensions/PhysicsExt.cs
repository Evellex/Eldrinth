using UnityEngine;

namespace Augmenta
{
    public static class PhysicsExt
    {
        public static int GetCollisionMask(int layer)
        {
            int mask = 0;
            for (int x = 0; x < 32;++x)
            {                
                if(!Physics.GetIgnoreLayerCollision(x, layer))
                {
                    mask |= 1 << x;
                }                
            }
            return mask;                
        }
    }
}

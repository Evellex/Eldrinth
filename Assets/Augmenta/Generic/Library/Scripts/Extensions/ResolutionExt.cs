using UnityEngine;
namespace Augmenta
{
    public static class ResolutionExt
    {
        public static string ToStringExt(this Resolution res)
        {
            return res.width + " x " + res.height;
        }
    }
}
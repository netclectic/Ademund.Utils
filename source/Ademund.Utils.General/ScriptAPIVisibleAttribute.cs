using System;

namespace Ademund.Utils
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Event)]
    public class ScriptAPIVisibleAttribute : Attribute
    {
        public bool Visible { get; set; }

        public ScriptAPIVisibleAttribute(bool visible)
        {
            Visible = visible;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoOS.Platform.Client;

namespace HelloWorldMIPPlugin.Client
{
    public class HelloWorldMIPPluginWorkSpaceViewItemManager : ViewItemManager
    {
        public HelloWorldMIPPluginWorkSpaceViewItemManager() : base("HelloWorldMIPPluginWorkSpaceViewItemManager")
        {
        }

        public override ViewItemUserControl GenerateViewItemUserControl()
        {
            return new HelloWorldMIPPluginWorkSpaceViewItemUserControl();
        }

        public override PropertiesUserControl GeneratePropertiesUserControl()
        {
            return new PropertiesUserControl(); //no special properties
        }

    }
}

/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;
using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;

using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Input;

using Sce.Pss.Framework;

namespace Sample
{


public class AppMain
{
    public static void Main(string[] args)
    {
		using( GameFrameworkSample game = new GameFrameworkSample())
		{
			game.Run(args);
		}
    }
}

}

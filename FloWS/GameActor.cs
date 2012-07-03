/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;

using Sce.Pss.Core;
using Sce.Pss.Framework;


namespace Sample
{
	public class GameActor : Actor
	{
		protected GameSample gs;
		
		public GameActor(GameSample gs, string name) : base(name) 
		{	
			this.gs = gs;	
		}
	}
}



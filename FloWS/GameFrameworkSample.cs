/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;
using System.Collections.Generic;

using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Input;

using Sce.Pss.Framework;
using Tutorial.Utility;


namespace Sample
{
	public class GameFrameworkSample: GameFramework
	{
		Int32 counter=0;
		public ImageRect rectScreen;
		
		SimpleSprite spritePlayer;
		
		
	    public override void Initialize()
		{
			base.Initialize();
			
			rectScreen = graphics.Screen.Rectangle;
			
			Texture2D texturePlayer = new Texture2D("/Application/resources/Player.png", false);

			spritePlayer = new SimpleSprite(graphics, texturePlayer);	
			spritePlayer.Position.X = rectScreen.Width/2.0f;
			spritePlayer.Position.Y = rectScreen.Height/2.0f;
			spritePlayer.Position.Z = 0.0f;
		}
		

	    public override void Update()
	    {
			base.Update();

#if DEBUG			
			debugString.WriteLine("counter "+counter);
			debugString.WriteLine("Buttons="+PadData.Buttons);
#endif
			
			int speed = 4;
			
			if((PadData.Buttons & GamePadButtons.Left) != 0)
			{
				spritePlayer.Position.X -= speed;
			}
			if((PadData.Buttons & GamePadButtons.Right) != 0)
			{
				spritePlayer.Position.X += speed;
			}
			if((PadData.Buttons & GamePadButtons.Up) != 0)
			{
				spritePlayer.Position.Y -= speed;
			}
			if((PadData.Buttons & GamePadButtons.Down) != 0)
			{
				spritePlayer.Position.Y += speed;
			}

			++counter;
	    }
		

	    public override void Render()
	    {
			graphics.Clear();
			
			graphics.Enable(EnableMode.DepthTest);
			
			spritePlayer.Render();
				
	        base.Render();
	    }
	}
}

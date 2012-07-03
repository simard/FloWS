/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;
using System.Diagnostics;

using Sce.Pss.Core;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Input;

using Tutorial.Utility;


namespace Sce.Pss.Framework 
{
	public class GameFramework : IDisposable
	{
		protected GraphicsContext graphics;
		public GraphicsContext Graphics 
		{ 
			get {return graphics;}
		}
		
		GamePadData gamePadData;
		public GamePadData PadData 
		{ 
			get { return gamePadData;}
		}
		
		protected bool loop = true;
		protected bool drawDebugString = true;
		
		Stopwatch stopwatch;
		const int pinSize=3;
		int[] time= new int[pinSize];
		int[] preTime= new int[pinSize];
		float[] timePercent= new float[pinSize];
		
		protected Texture2D textureFont;
		public DebugString debugString;
		
		public void Run(string[] args)
		{
			Initialize();
			
			while (loop)
			{
				time[0] = (int)stopwatch.ElapsedTicks;// start
				SystemEvents.CheckEvents();
				Update();
				time[1] = (int)stopwatch.ElapsedTicks;
				Render();
			}
			
			Terminate();
		}
		
		virtual public void Initialize()
		{
			Console.WriteLine("Initialize()");
			
			stopwatch = new Stopwatch();
			stopwatch.Start();
			
			graphics = new GraphicsContext();
			graphics.SetClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Enable(EnableMode.Blend);
			graphics.Enable(EnableMode.DepthTest);
			graphics.SetBlendFunc(BlendFuncMode.Add, BlendFuncFactor.SrcAlpha, BlendFuncFactor.OneMinusSrcAlpha);
			
			textureFont = new Texture2D("/Application/resources/DebugFont.png", false);
			debugString = new DebugString(graphics, textureFont, 10,20);
		}
		
		
		virtual public void Terminate()
		{
			Console.WriteLine("Terminate()");
		}
		
		public void Dispose()
		{
			graphics.Dispose();
			textureFont.Dispose();
			debugString.Dispose();
		}
		
		
		virtual public void Input()
		{
			gamePadData = GamePad.GetData(0);
			
		}
		
		
		virtual public void Update()
		{
			debugString.Clear();
			
			Input();
			
			//@j StartボタンとSelectボタンの同時押しでプログラムを終了します。
			//@e Terminate a program with simultaneously pressing Start button and Select button.
			if((gamePadData.Buttons & GamePadButtons.Start) != 0 &&  (gamePadData.Buttons & GamePadButtons.Select) != 0)
			{
				Console.WriteLine("exit."); 
				loop = false;
				return;
			}
			
			// Q key and E key
			if((gamePadData.Buttons & GamePadButtons.L) != 0 &&  (gamePadData.ButtonsDown & GamePadButtons.R) != 0)
			{
				drawDebugString = (drawDebugString == true) ? false : true;
			}
			
			CalculateProcessTime();
		}
		
		
		virtual public void Render()
		{
#if DEBUG	
			if(drawDebugString==true)
				debugString.Render();
#endif
			
			time[2] = (int)stopwatch.ElapsedTicks;
			
			graphics.SwapBuffers();	
			
			preTime=(int[])time.Clone();
		}
		
		
		/// <summary>
		/// 処理にかかった時間を計算する。
		/// </summary>
		void CalculateProcessTime()
		{
			float fps = 60.0f;
			
			float ticksPerFrame = Stopwatch.Frequency / fps;	
			timePercent[0]=(preTime[1]-preTime[0])/ticksPerFrame;
			timePercent[1]=(preTime[2]-preTime[1])/ticksPerFrame;
			
			debugString.WriteLine(string.Format("Update={0,6:N}%", timePercent[0] * 100));
			debugString.WriteLine(string.Format("Render={0,6:N}%", timePercent[1] * 100));
		}
	}
}

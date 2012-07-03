/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;

using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;

using System.IO;
using System.Reflection;


namespace Tutorial.Utility
{
	/// <summary>
	/// スプライト
	/// </summary>
	public class SimpleSprite
	{
		static ShaderProgram shaderProgram;
		
		protected GraphicsContext graphics;
		
		
		//@j 頂点座標。
		//@e Vertex coordinate
		float[] vertices=new float[12];
		
		
		//@j テクスチャ座標。
		//@e Texture coordinate
		float[] texcoords = {
			0.0f, 0.0f,	// left top
			0.0f, 1.0f,	// left bottom
			1.0f, 0.0f,	// right top
			1.0f, 1.0f,	// right bottom
		};
		
		
		//@j 頂点色。
		//@e Vertex color
		float[] colors = {
			1.0f,	1.0f,	1.0f,	1.0f,	// left top
			1.0f,	1.0f,	1.0f,	1.0f,	// left bottom
			1.0f,	1.0f,	1.0f,	1.0f,	// right top
			1.0f,	1.0f,	1.0f,	1.0f,	// right bottom
		};
		
		//@j インデックス。
		//@e Index
		const int indexSize = 4;
		ushort[] indices;
		
		//@j 頂点バッファ。
		//@e Vertex buffer 
		VertexBuffer vertexBuffer;
		
		protected Texture2D texture;
		
		
		//@j プロパティではPosition.Xという記述ができないため、publicの変数にしています。
		//@e Property cannot describe Position.X, public variable is used.
		/// <summary>
		/// スプライトの表示位置。
		/// </summary>
		public Vector3 Position ;
		
		/// <summary>
		/// スプライトの中心座標を指定。0.0f～1.0fの範囲で指定してください。
		/// スプライトの中心を指定する場合 X=0.5f, Y=0.5fとなります。
		/// </summary>
		public Vector2 Center;
		
		public Vector2 Scale=Vector2.One;
		
		protected Vector4 color=Vector4.One;
		

		float width,height;
		/// <summary>
		/// スプライトの幅。
		/// </summary>
		public float Width 
		{
			get { return width * Scale.X;}
		}
		/// <summary>
		/// スプライトの高さ。
		/// </summary>
		public float Height 
		{
			get { return height * Scale.Y;}
		}

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public SimpleSprite(GraphicsContext graphics, Texture2D texture)
		{
			if(shaderProgram == null)
			{
				shaderProgram=CreateSimpleSpriteShader();
			}
			
			if (texture == null)
			{
				throw new Exception("ERROR: texture is null.");
			}
			
			this.graphics = graphics;
			this.texture = texture;
			this.width = texture.Width;
			this.height = texture.Height;
			
			vertices[0]=0.0f;	// x0
			vertices[1]=0.0f;	// y0
			vertices[2]=0.0f;	// z0
			
			vertices[3]=0.0f;	// x1
			vertices[4]=1.0f;	// y1
			vertices[5]=0.0f;	// z1
			
			vertices[6]=1.0f;	// x2
			vertices[7]=0.0f;	// y2
			vertices[8]=0.0f;	// z2
			
			vertices[9]=1.0f;	// x3
			vertices[10]=1.0f;	// y3
			vertices[11]=0.0f;	// z3
			

			indices = new ushort[indexSize];
			indices[0] = 0;
			indices[1] = 1;
			indices[2] = 2;
			indices[3] = 3;
			
			
			//@j                                                頂点座標,                テクスチャ座標,     頂点色
			//@e                                                Vertex coordinate,               Texture coordinate,     Vertex color
			vertexBuffer = new VertexBuffer(4, indexSize, VertexFormat.Float3, VertexFormat.Float2, VertexFormat.Float4);
			
		}


		//@j 頂点色の設定。
		//@e Vertex color settings
		public void SetColor(Vector4 color)
		{
			SetColor(color.R, color.G, color.B, color.A);
		}
		
		//@j 頂点色の設定。
		//@e Vertex color settings
		public void SetColor(float r, float g, float b, float a)
		{
			this.color.R = r;
			this.color.G = g;
			this.color.B = b;
			this.color.A = a;
			
			for (int i = 0; i < colors.Length; i+=4)
			{
				colors[i] = r;
				colors[i+1] = g;
				colors[i+2] = b;
				colors[i+3] = a;
			}
		}
		
		
		/// <summary>
		/// テクスチャ座標を指定します。ピクセル単位で指定してください。
		/// </summary>
		public void SetTextureCoord(float x0, float y0, float x1, float y1)
		{
			texcoords[0] = x0 / texture.Width;	// left top u
			texcoords[1] = y0 / texture.Height; // left top v
			
			texcoords[2] = x0 / texture.Width;	// left bottom u
			texcoords[3] = y1 / texture.Height;	// left bottom v
			
			texcoords[4] = x1 / texture.Width;	// right top u
			texcoords[5] = y0 / texture.Height;	// right top v
			
			texcoords[6] = x1 / texture.Width;	// right bottom u
			texcoords[7] = y1 / texture.Height;	// right bottom v
		}
		
		/// <summary>
		/// テクスチャ座標を指定します。ピクセル単位で指定してください。
		/// </summary>
		public void SetTextureCoord(Vector2 topLeft, Vector2 bottomRight)
		{
			SetTextureCoord(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
		}
		
		
		/// <summary>
		/// スプライトの描画。
		/// </summary>
		public void Render()
		{
			graphics.SetShaderProgram(shaderProgram);
			
			vertexBuffer.SetVertices(0, vertices);
			vertexBuffer.SetVertices(1, texcoords);
			vertexBuffer.SetVertices(2, colors);
			
			vertexBuffer.SetIndices(indices);
			graphics.SetVertexBuffer(0, vertexBuffer);
			graphics.SetTexture(0, texture);
			
			float screenWidth=graphics.Screen.Rectangle.Width;
			float screenHeight=graphics.Screen.Rectangle.Height;
			
			Matrix4 unitScreenMatrix = new Matrix4(
				 Width*2.0f/screenWidth,	0.0f,		0.0f, 0.0f,
				 0.0f,	 Height*(-2.0f)/screenHeight,	0.0f, 0.0f,
				 0.0f,	 0.0f, 1.0f, 0.0f,
				 -1.0f+(Position.X-Width * Center.X)*2.0f/screenWidth,  1.0f+(Position.Y-Height*Center.Y)*(-2.0f)/screenHeight, 0.0f, 1.0f
			);
			
			shaderProgram.SetUniformValue(0, ref unitScreenMatrix);

			graphics.DrawArrays(DrawMode.TriangleStrip, 0, indexSize);
		}
		
		
		//@j シェーダープログラムの初期化。
		//@e Initialization of shader program
		private static ShaderProgram CreateSimpleSpriteShader()
		{
			string resourceName = "TutoLib.shaders.SimpleSprite.cgx";
				
			Assembly resourceAssembly = Assembly.GetExecutingAssembly();
			if (resourceAssembly.GetManifestResourceInfo(resourceName) == null)
			{
				throw new FileNotFoundException("File not found.", resourceName);
			}
	
			Stream fileStream = resourceAssembly.GetManifestResourceStream(resourceName);
			Byte[] dataBuffer = new Byte[fileStream.Length];
			
			//@j dataBufferにファイルを読み込む。
			//@e Read files into the dataBuffer.
			fileStream.Read(dataBuffer, 0, dataBuffer.Length);
				
			var shaderProgram = new ShaderProgram(dataBuffer);
			shaderProgram.SetUniformBinding(0, "u_WorldMatrix");
	
			return shaderProgram;
		}
	}
}

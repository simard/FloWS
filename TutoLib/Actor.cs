/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;
using System.Collections.Generic;


namespace Sce.Pss.Framework
{
	public class Actor
	{
		/// <summary>
		/// このアクターの名前。
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// アクターの状態。
		/// </summary>
		public enum ActorStatus
		{
			Action,
			NoDisp,
			NoAction,
			Vacant,
			Dead,
		}
		
		public ActorStatus Status { get; set; }
		
		/// <summary>
		/// 一番上の階層から数えた階層。
		/// </summary>
		protected Int32 level=0;
		
		protected List<Actor> children = new List<Actor>();
		
		public List<Actor> Children
		{
			get { return children;}	
		}
		
		public Actor()
		{
			this.Name = "no_name";
			this.Status = ActorStatus.Action;
		}
		
		
		public Actor(string name)
		{
			this.Name = name;
			this.Status = ActorStatus.Action;
		}
		
		/// <summary>
		/// 更新。
		/// </summary>
		virtual public void Update()
		{ 
			foreach( Actor actorChild in children)
			{
				if(actorChild.Status == ActorStatus.Action || actorChild.Status == ActorStatus.NoDisp)
					actorChild.Update();	
			}
		}

		/// <summary>
		/// 描画。
		/// </summary>
		virtual public void Render() 
		{ 
			foreach( Actor actorChild in children)
			{
				actorChild.Render();	
			}		}
		
		virtual public void AddChild(Actor actor)
		{
			children.Add(actor);
			actor.level = this.level+1;
		}
		
		/// <summary>
		/// nameのアクターを、自分を含めて子孫から探す。
		/// 最初に見つかったアクターを返す。
		/// </summary>
		virtual public Actor Search(string name)
		{
			if( this.Name == name)
				return this;
			
			foreach(Actor actor in children)
			{
				if( actor.Name == name)
				{
					return actor;	
				}
			}
			return null;
		}
		
		/// <summary>
		/// 死亡しているアクターをリストから削除する。
		/// </summary>
		virtual public void CleanUpDeadActor()
		{
			//@j 自分が死亡していたら、子すべてに死亡フラグを立てる。
			//@e Set dead flags for all the children if the player is dead.
			if( this.Status == ActorStatus.Dead)
			{
				foreach(Actor actor in children)
				{
					actor.Status = ActorStatus.Dead;
				}
			}
			
			//@j 再帰処理で子を巡回していく。
			//@e Visit children with recursive call.
			foreach(Actor actor in children)
			{
				actor.CleanUpDeadActor();
			}
			
			//@j 死亡フラグのたっている子をリストから削除。
			//@e Delete a child where the dead flag is set from a list.
			children.RemoveAll(CheckStatus);
			//children.RemoveAll(actor=> actor.Status == ActorStatus.Dead);
			
		}

		
		static bool CheckStatus(Actor actor)
		{
			//@j この条件で真になる要素を削除。
			//@e Delete the elements to be proper with this condition.
			return actor.Status == ActorStatus.Dead; 
		}
		
		
#if DEBUG
		/*
		/// <summary>
		/// デストラクタ。
		/// </summary>
		~Actor()
		{
			Console.WriteLine("~"+Name);
		}
		*/
#endif
		
	}
}

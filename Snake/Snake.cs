using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRageMath;

namespace IngameScript
{
	partial class Program
	{
		public class Pellet
		{
			public int x;
			public int y;
			Screen screen;

			public Pellet(int _x, int _y, Screen _screen)
			{
				x = _x;
				y = _y;
				screen = _screen;
			}

			public void Draw()
			{
				screen.rect(x, y, 5, 5, Screen.DrawType.FILLED);
			}
		}

		public class Snake
		{
			struct Box
			{
				public int x;
				public int y;

				public Box(int _x, int _y)
				{
					x = _x;
					y = _y;
				}
			}

			public int x;
			public int y;
			int dir;
			public int length;
			public Queue body;
			Screen screen;
			public int score;

			public Snake(int _x, int _y, int _dir, int _length, Screen _screen)
			{
				x = _x;
				y = _y;
				dir = _dir;
				length = _length;
				screen = _screen;

				body = new Queue();
				score = 0;
			}

			public void SetDir(int newDir)
			{
				if(newDir < 0 || newDir > 3 || Math.Abs(newDir-dir) == 2)
					return;
				dir = newDir;
			}

			public void Move()
			{
				while(body.Count >= length)
					body.Dequeue();
				
				body.Enqueue(new Box(x, y));

				switch(dir)
				{
					case 0:
						y -= 5;
						break;
					case 1:
						x += 5;
						break;
					case 2:
						y += 5;
						break;
					case 3:
						x -= 5;
						break;
					default:
						break;
				}
			}

			public bool IsColliding()
			{
				if(x < 5 || x > screen.width-10 || y < 5 || y > screen.height-10)
					return true;

				foreach(Box box in body)
				{
					if(box.x == x && box.y == y)
						return true;
				}

				return false;
			}

			public bool TryEat(Pellet pellet)
			{
				if(x == pellet.x && y == pellet.y)
				{
					score++;
					length++;
					return true;
				}
				return false;
			}

			public void Draw()
			{
				screen.rect(x, y, 5, 5, Screen.DrawType.FILLED);

				foreach (Box box in body)
					screen.rect(box.x, box.y, 5, 5, Screen.DrawType.OUTLINE);
			}
		}
	}
}

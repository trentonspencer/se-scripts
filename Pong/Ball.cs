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
		public class Ball
		{
			public float x;
			public float y;
			public int r;
			public Vector2 v;
			public Screen screen;

			public Ball(int _x, int _y, int _r, Vector2 _v, Screen _screen)
			{
				x = _x;
				y = _y;
				r = _r;
				v = _v;
				screen = _screen;
			}

			public void collision(Player obj)
			{
				int ox = obj.x+(obj.w/2);
				int oy = obj.y+(obj.h/2);
				
				Vector2 vec;
				vec.X = ox-x;
				vec.Y = oy-y;
				vec.Normalize();

				Vector2 point = new Vector2(x, y) + vec*r;
				int px = (int)point.X;
				int py = (int)point.Y;

				if(px > obj.x && px < obj.x+obj.w && py > obj.y && py < obj.y+obj.h)
				{
					v.X = v.X * -1.2f;
					v.Y += y-oy;
				}
			}

			public void update()
			{
				x += v.X*(1.0f/60.0f);
				y += v.Y*(1.0f/60.0f);

				if(y-r <= 10 || y+r >= screen.height)
					v.Y = v.Y * -1.2f;
			}

			public void draw()
			{
				screen.circle((int)x, (int)y, r, Screen.DrawType.OUTLINE);
			}
		}
	}
}

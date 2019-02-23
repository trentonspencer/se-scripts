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
		public class Player
		{
			public int x;
			public int y;
			public int w;
			public int h;
			public int score;
			public IMyPistonBase piston;
			public Screen screen;

			public Player(int _x, int _y, int _w, int _h, int _score, IMyPistonBase _piston, Screen _screen)
			{
				x = _x;
				y = _y;
				w = _w;
				h = _h;
				score = _score;
				piston = _piston;
				screen = _screen;
			}

			public void update()
			{
				y = (int)(11 + (screen.height - h - 12)*(piston.CurrentPosition/piston.MaxLimit));
			}

			public void draw()
			{
				screen.rect(x, y, w, h, Screen.DrawType.FILLED);
			}
		}
	}
}

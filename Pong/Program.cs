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
	partial class Program : MyGridProgram
	{
		int tick;
		Random rnd;

		Screen s;
		Player p1;
		Player p2;
		Ball b;

		int WIDTH;
		int HEIGHT;
		int HWIDTH;
		int HHEIGHT;

		public Program()
		{
			Runtime.UpdateFrequency = UpdateFrequency.Update1;

			WIDTH = 263;
			HEIGHT = 89;
			HWIDTH = WIDTH/2;
			HHEIGHT = HEIGHT/2;

			tick = 0;
			rnd = new Random();

			IMyTextPanel lcd = GridTerminalSystem.GetBlockWithName("Pong") as IMyTextPanel;
			IMyPistonBase p1p = GridTerminalSystem.GetBlockWithName("Player 1") as IMyPistonBase;
			IMyPistonBase p2p = GridTerminalSystem.GetBlockWithName("Player 2") as IMyPistonBase;

			s = new Screen(WIDTH, HEIGHT, lcd);
			p1 = new Player(10, HHEIGHT+10, 5, 10, 0, p1p, s);
			p2 = new Player(WIDTH-15, HHEIGHT+10, 5, 10, 0, p2p, s);
			b = new Ball(HWIDTH, HHEIGHT, 3, new Vector2(0.0f, 0.0f), s);
		}

		public void Main(string argument, UpdateType updateSource)
		{
			if(argument == "reset")
			{
				tick = 0;
				p1.score = 0;
				p2.score = 0;
				b.x = HWIDTH;
				b.y = HHEIGHT+10;
				b.v.X = 0.0f;
				b.v.Y = 0.0f;
				tick = 0;
			}

			if(tick == 120)
			{
				b.v.Y = (float)rnd.NextDouble() * rnd.Next(-20, 20);
				b.v.X = Math.Sign(b.v.Y)*40.0f;
			}

			s.clear();

			p1.update();
			p2.update();

			b.collision(p1);
			b.collision(p2);
			b.update();

			if(b.x-b.r <= 0)
			{
				p2.score++;
				b.x = HWIDTH;
				b.y = HHEIGHT+10;
				b.v.X = 0.0f;
				b.v.Y = 0.0f;
				tick = 0;
			}
			else if(b.x+b.r >= WIDTH)
			{
				p1.score++;
				b.x = HWIDTH;
				b.y = HHEIGHT+10;
				b.v.X = 0.0f;
				b.v.Y = 0.0f;
				tick = 0;
			}

			p1.draw();
			p2.draw();
			b.draw();

			string text = p1.score.ToString() + " | " + p2.score.ToString();
			s.text(HWIDTH-(int)(s.getTextLength(text)/2.0f), 2, text);

			s.rect(0, 10, WIDTH, HEIGHT-10, Screen.DrawType.OUTLINE);

			s.update();

			tick++;
		}
	}
}
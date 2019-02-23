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
		Screen s;
		Snake player;
		Pellet food;

		IMySensorBlock upSensor;
		IMySensorBlock rightSensor;
		IMySensorBlock downSensor;
		IMySensorBlock leftSensor;
		MyDetectedEntityInfo controller;
		Vector3D lastPosition;

		Random rnd;

		int tick;

		int WIDTH;
		int HEIGHT;
		int HWIDTH;
		int HHEIGHT;

		public void debug(string msg)
		{
			IMyTextPanel lcd = GridTerminalSystem.GetBlockWithName("Debug") as IMyTextPanel;
			lcd.WritePublicText(msg);
		}

		public void ResetFood()
		{
			food.x = 5*rnd.Next(1, (WIDTH-10)/5);
			food.y = 5*rnd.Next(1, (HEIGHT-10)/5);
		}

		public Program()
		{
			Runtime.UpdateFrequency = UpdateFrequency.Update1;
			tick = 0;

			WIDTH = 263;
			HEIGHT = 89;
			HWIDTH = WIDTH/2;
			HHEIGHT = HEIGHT/2;

			rnd = new Random();

			IMyTextPanel lcd = GridTerminalSystem.GetBlockWithName("Snake") as IMyTextPanel;

			upSensor = GridTerminalSystem.GetBlockWithName("Snake Up") as IMySensorBlock;
			rightSensor = GridTerminalSystem.GetBlockWithName("Snake Right") as IMySensorBlock;
			downSensor = GridTerminalSystem.GetBlockWithName("Snake Down") as IMySensorBlock;
			leftSensor = GridTerminalSystem.GetBlockWithName("Snake Left") as IMySensorBlock;

			s = new Screen(WIDTH, HEIGHT, lcd);
			player = new Snake(150, 45, 1, 5, s);
			food = new Pellet(5*rnd.Next(1, (WIDTH-10)/5), 5*rnd.Next(1, (HEIGHT-10)/5), s);
		}

		public void Main(string argument, UpdateType updateSource)
		{
			if(tick % 5 == 0)
			{
				controller = upSensor.LastDetectedEntity;
				if(controller.IsEmpty() == false)
				{
					Vector3D sub = (lastPosition-controller.Position)*100;

					//debug((Math.Abs(sub.X) > Math.Abs(sub.Z)).ToString() + "\n" + sub.ToString() + "\n" + sub.Z.ToString());
					if(Math.Abs(sub.X) > Math.Abs(sub.Z))
						player.SetDir(Math.Sign(sub.X) == 1 ? 3:1);
					else if(sub.Z != 0.0f)
						player.SetDir(Math.Sign(sub.Z) == 1 ? 2:0);
				}
				lastPosition = controller.Position;

				s.clear();

				player.Move();
			
				if(player.IsColliding())
				{
					player.score = 0;
					player.length = 5;
					player.x = 150;
					player.y = 45;
					player.body.Clear();

					ResetFood();
				}

				if(player.TryEat(food))
					ResetFood();

				player.Draw();
				food.Draw();

				s.rect(4, 4, WIDTH-11, HEIGHT-12, Screen.DrawType.OUTLINE);
			
				s.update();
			}

			tick++;
		}
	}
}

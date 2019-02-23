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
		public class Screen
		{
			char pixel_on = '|';
			char pixel_off = ' ';

			static readonly int[] letters = 
			{
				0,4329476,10813440,11512810,16398526,17895697,6632015,4325376,2232450,8523912,
				22483413,4357252,68,31744,4,1118480,15390382,4608142,15239320,31504446,
				1841462,33060926,33062463,32540808,33095231,33094719,131200,131208,2236546,
				1016800,8521864,32051204,15392270,33095217,32045630,33047071,32032318,
				33061407,33062416,33050175,18415153,14815374,14748236,20673235,17318431,18732593,18667121,
				15255086,32045584,15259213,32045779,33299071,32641156,18400814,18393412,18405233,18157905,18157700,
				32575775,14950670,17043521,14747726,4539392,31,6389760,33095217,32045630,
				33047071,32032318,33061407,33062416,33050175,18415153,14815374,14748236,
				20673235,17318431,18732593,18667121,15255086,32045584,15259213,32045779,33299071,
				32641156,18400814,18393412,18405233,18157905,18157700,32575775,2240642,4329604,
				8525960,14016
			};

			public enum DrawType
			{
				OUTLINE,
				FILLED
			}

			public int width;
			public int height;
			public IMyTextPanel screenObject;
			StringBuilder pixels;

			public Screen(int w, int h, IMyTextPanel obj)
			{
				width = w;
				height = h;
				screenObject = obj;

				pixels = new StringBuilder();
				for(int i = 0; i < h; i++)
				{
					pixels.Append(pixel_off, w);
					pixels.Append('\n');
				}

				obj.ShowPublicTextOnScreen();
				obj.FontSize = 0.2f;
			}

			public void clear()
			{
				pixels.Clear();
				for(int i = 0; i < height; i++)
				{
					pixels.Append(pixel_off, width);
					pixels.Append('\n');
				}
			}

			public void update()
			{
				screenObject.WritePublicText(pixels);
			}

			public void pixel(int x, int y)
			{
				if(x < 0 || x >= width || y < 0 || y >= height)
					return;
				
				pixels[y*width+x+y] = pixel_on;
			}

			public void line(int x, int y, int ex, int ey)
			{
				int dx = ex-x;
				int dy = ey-y;
				
				if(dx == 0)
				{
					for(int i = y; i <= ey; i++)
						pixel(x, i);
				}
				else if(dy == 0)
				{
					for(int i = x; i <= ex; i++)
						pixel(i, y);
				}
				else
				{
					float derr = Math.Abs((float)dy/(float)dx);
					float err = 0.0f;
					for(int i = x; i < ex; i++)
					{
						pixel(i, y);

						err = err+derr;
						if(err >= 0.5f)
						{
							y = y + Math.Sign(dy);
							err = err-1.0f;
						}
					}
				}
			}

			public void rect(int x, int y, int w, int h, DrawType type)
			{
				if(type == DrawType.OUTLINE)
				{
					int ex = x+w-1;
					int ey = y+h-1;

					for(int i = x; i <= ex; i++)
					{
						pixel(i, y);
						pixel(i, ey);
					}

					for(int i = y; i <= ey; i++)
					{
						pixel(x, i);
						pixel(ex, i);
					}
				}
				else if(type == DrawType.FILLED)
				{
					int ex = x+w-1;
					int ey = y+h-1;

					for(int i = x; i <= ex; i++)
					{
						for(int j = y; j <= ey; j++)
							pixel(i, j);
					}
				}
			}

			public void circle(int x0, int y0, int radius, DrawType type)
			{
				if(type == DrawType.OUTLINE)
				{
					int x = radius-1;
					int y = 0;
					int dx = 1;
					int dy = 1;
					int err = dx - (radius << 1);

					while(x >= y)
					{
						pixel(x0 + x, y0 + y);
						pixel(x0 + y, y0 + x);
						pixel(x0 - y, y0 + x);
						pixel(x0 - x, y0 + y);
						pixel(x0 - x, y0 - y);
						pixel(x0 - y, y0 - x);
						pixel(x0 + y, y0 - x);
						pixel(x0 + x, y0 - y);

						if(err <= 0)
						{
							y++;
							err += dy;
							dy += 2;
						}

						if(err > 0)
						{
							x--;
							dx += 2;
							err += dx - (radius << 1);
						}
					}
				}
				else if(type == DrawType.FILLED)
				{
					int r2 = radius*radius;
					int area = r2 << 2;
					int rr = radius << 1;

					for(int i = 0; i < area; i++)
					{
						int tx = (i % rr) - radius;
						int ty = (i / rr) - radius;

						if(tx*tx + ty*ty <= r2)
							pixel(x0 + tx, y0 + ty);
					}
				}
			}

			public void letter(int x, int y, int code)
			{
				int letter = letters[code-0x20];

				for(int i = 0; i < 5; i++)
				{
					for(int j = 0; j < 5; j++)
					{
						if((letter & (int)Math.Pow(2,24-(i+j*5))) > 0)
							pixel(x+i, y+j);
					}
				}
			}

			public void text(int x, int y, string str)
			{
				int off = 0;
				for(int i = 0; i < str.Length; i++)
				{
					if(str[i] == '\n')
					{
						y += 7;
						off = 0;
						continue;
					}

					off += 7;
					letter(x+off, y, str[i]);
				}
			}

			public int getTextLength(string str)
			{
				return str.Length*7;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Gungeon;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System.Globalization;
using UnityEngine;

namespace RenameMod
{
    public class Module : ETGModule
    {
		public static string namefile = "CustomItemNames.json";
		public static string shortdescfile = "CustomItemShortDesc.json";
		public static string longdescfile = "CustomItemLongDesc.json";


		public static string RenameDirectory = Path.Combine(ETGMod.ResourcesDirectory, "CustomRenames/");
		public static string NameFilePath = Path.Combine(Module.RenameDirectory, "CustomItemNames.json");
		public static string ShortDescPath = Path.Combine(Module.RenameDirectory, "CustomItemShortDesc.json");
		public static string LongDescPath = Path.Combine(Module.RenameDirectory, "CustomItemLongDesc.json");

		Dictionary<string, string> ItemNames = new Dictionary<string, string>();
		Dictionary<string, string> ItemShortDesc = new Dictionary<string, string>();
		Dictionary<string, string> ItemLongDesc = new Dictionary<string, string>();


		public static readonly string MOD_NAME = "Item Rename Mod";
        public static readonly string VERSION = "1.0.0";
        public static readonly string TEXT_COLOR = "#00FFFF";

        public override void Start()
        {
            try
            {
                //global::ETGMod.GameFolder = Path.Combine(Application.dataPath, "..");
                if (!Directory.Exists(Module.RenameDirectory))
                {
                    Directory.CreateDirectory(RenameDirectory);

					ETGModConsole.Log("Rename: Unable to find existing (NameFilePath) config, making a new one!", false);
					File.Create(NameFilePath).Close();

					ETGModConsole.Log("Rename: Unable to find existing (ShortDesc) config, making a new one!", false);
					File.Create(ShortDescPath).Close();

					ETGModConsole.Log("Rename: Unable to find existing (LongDescPath) config, making a new one!", false);
					File.Create(LongDescPath).Close();

				}


                if (!File.Exists(NameFilePath))
                {
                    ETGModConsole.Log("Rename: Unable to find existing (NameFilePath) config, making a new one!", false);
                    File.Create(NameFilePath).Close();
                }

                if (!File.Exists(ShortDescPath))
                {
					ETGModConsole.Log("Rename: Unable to find existing (ShortDesc) config, making a new one!", false);
					//Directory.CreateDirectory(RenameDirectory);
					File.Create(ShortDescPath).Close();
				}

                if (!File.Exists(LongDescPath))
                {
                    ETGModConsole.Log("Rename: Unable to find existing (LongDescPath) config, making a new one!", false);
                    //Directory.CreateDirectory(RenameDirectory);
                    File.Create(LongDescPath).Close();
                }
				if (File.Exists(NameFilePath))
				{
					var res = File.ReadAllLines(NameFilePath).Select((v, i) => new { Index = i, Value = v }).GroupBy(p => p.Index / 2).ToDictionary(g => g.First().Value, g => g.Last().Value);
					for (int index = 0; index < res.Count; index++)
					{
						ETGModConsole.Log($"{res.ElementAt(index).Value}'s name is now: {res.ElementAt(index).Key}");
						
						Gun gun = PickupObjectDatabase.GetByName(res.ElementAt(index).Value) as Gun;		
						var thing = PickupObjectDatabase.GetByName(res.ElementAt(index).Value);

                        if (thing is Gun && thing != null)
                        {
                            thing.SetName(res.ElementAt(index).Key);
                        }
						else if (thing != null)
                        {
                            thing.SetName(res.ElementAt(index).Key);
                        }
                    }
				}
				if(File.Exists(ShortDescPath))
                {
					var res = File.ReadAllLines(ShortDescPath).Select((v, I) => new { Index = I, Value = v }).GroupBy(p => p.Index / 2).ToDictionary(g => g.First().Value, g => g.Last().Value);
					for (int index = 0; index < res.Count; index++)
					{
						ETGModConsole.Log($"{res.ElementAt(index).Value}'s short description is now: {res.ElementAt(index).Key}");

						Gun gun = PickupObjectDatabase.GetByName(res.ElementAt(index).Value) as Gun;
						var thing = PickupObjectDatabase.GetByName(res.ElementAt(index).Value);

						if (thing is Gun && thing != null)
						{
							thing.SetShortDescription(res.ElementAt(index).Key);
						}
						else if (thing != null)
						{
							thing.SetShortDescription(res.ElementAt(index).Key);
						}
					}
				}
				
				if (File.Exists(LongDescPath))
				{
					var res = File.ReadAllLines(LongDescPath).Select((v, I) => new { Index = I, Value = v }).GroupBy(p => p.Index / 2).ToDictionary(g => g.First().Value, g => g.Last().Value);
					for (int index = 0; index < res.Count; index++)
					{
						ETGModConsole.Log($"{res.ElementAt(index).Value}'s long description is now: {res.ElementAt(index).Key}");

						Gun gun = PickupObjectDatabase.GetByName(res.ElementAt(index).Value) as Gun;
						var thing = PickupObjectDatabase.GetByName(res.ElementAt(index).Value);

						if (thing is Gun && thing != null)
						{
							thing.SetLongDescription(res.ElementAt(index).Key.CapitalizeFirst());
						}
						else if (thing != null)
						{
							thing.SetLongDescription(res.ElementAt(index).Key.CapitalizeFirst());
						}
					}
				}
				ETGModConsole.Commands.AddGroup("rnm", delegate (string[] args)
                {
                    ETGModConsole.Log("<size=100><color=#ff0000ff>Rename Mod V1 by An3s & N0tAB0t</color></size>", false);
                    ETGModConsole.Log("<size=100><color=#ff0000ff>--------------------------------</color></size>", false);
                    ETGModConsole.Log("Use \"rnm help\" for help!", false);
                });
                ETGModConsole.Commands.GetGroup("rnm").AddUnit("help", delegate (string[] args)
                {
                    Log("type in: rnm setitemname item_id new_item_name, to rename an item (UNDERSCORES REQUIRED FOR SPACES)");
                    Log("type in: rnm setgunname gun_id new_gun_name, to rename a gun (UNDERSCORES REQUIRED FOR SPACES)");
					Log("this also works with things like descriptions");
                });
                //ETGModConsole.Commands.GetGroup("rnm").AddUnit("setitemname", new Action<string[]>(this.RenameItem), Module.GiveAutocompletionSettings);
                ETGModConsole.Commands.GetGroup("rnm").AddUnit("setitemname", new Action<string[]>(this.RenameItem), Module.GiveAutocompletionSettings);
                ETGModConsole.Commands.GetGroup("rnm").AddUnit("setgunname", new Action<string[]>(this.renameGun), Module.GiveAutocompletionSettings);
                ETGModConsole.Commands.GetGroup("rnm").AddUnit("setitemshortdesc", new Action<string[]>(this.SetItemShortDesc), Module.GiveAutocompletionSettings);
                ETGModConsole.Commands.GetGroup("rnm").AddUnit("setgunshortdesc", new Action<string[]>(this.SetGunShortDesc), Module.GiveAutocompletionSettings);
                ETGModConsole.Commands.GetGroup("rnm").AddUnit("setitemlongdesc", new Action<string[]>(this.SetItemLongDesc), Module.GiveAutocompletionSettings);
                ETGModConsole.Commands.GetGroup("rnm").AddUnit("setgunlongdesc", new Action<string[]>(this.SetGunLongDesc), Module.GiveAutocompletionSettings);

				LoadRenames();
			}
            catch (Exception e)
            {
                ETGModConsole.Log("mod Broke heres why: " + e);
            }
            Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);
        }

        public static void Log(string text, string color="FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }
		public StreamWriter writer;

		public override void Exit() { }
        public override void Init() { }
		public static void LoadRenames()
		{
			string[] directories = Directory.GetDirectories(RenameDirectory);
			for (int i = 0; i < directories.Length; i++)
			{
				string text = Path.Combine(RenameDirectory, directories[i]);
				string text2 = Path.Combine(text, namefile) ;
				string text3 = Path.Combine(text, shortdescfile);
				string text4 = Path.Combine(text, longdescfile);
				bool flag = File.Exists(text2);
				if (flag)
				{
					ReadNames(text2);
				}
					

				bool flag2 = File.Exists(text3);
				if(flag2)
                {
					ReadShortDesc(text3);
				}

				bool flag3 = File.Exists(text4);
				if(flag3)
                {
					ReadLongDesc(text4);
                }
				
			}
		}

		public static void ReadNames(string path)
        {
			var res = File.ReadAllLines(path).Select((v, I) => new { Index = I, Value = v }).GroupBy(p => p.Index / 2).ToDictionary(g => g.First().Value, g => g.Last().Value);
			for (int index = 0; index < res.Count; index++)
			{
				ETGModConsole.Log($"{res.ElementAt(index).Value}'s name is now: {res.ElementAt(index).Key}");

				Gun gun = PickupObjectDatabase.GetByName(res.ElementAt(index).Value) as Gun;
				var thing = PickupObjectDatabase.GetByName(res.ElementAt(index).Value);

				if (thing is Gun && thing != null)
				{
					thing.SetName(res.ElementAt(index).Key);
				}
				else if (thing != null)
				{
					thing.SetName(res.ElementAt(index).Key);
				}
			}
		}
		public static void ReadShortDesc(string path)
		{
			var res = File.ReadAllLines(path).Select((v, I) => new { Index = I, Value = v }).GroupBy(p => p.Index / 2).ToDictionary(g => g.First().Value, g => g.Last().Value);
			for (int index = 0; index < res.Count; index++)
			{
				ETGModConsole.Log($"{res.ElementAt(index).Value}'s short description is now: {res.ElementAt(index).Key}");

				Gun gun = PickupObjectDatabase.GetByName(res.ElementAt(index).Value) as Gun;
				var thing = PickupObjectDatabase.GetByName(res.ElementAt(index).Value);

				if (thing is Gun && thing != null)
				{
					thing.SetShortDescription(res.ElementAt(index).Key);
				}
				else if (thing != null)
				{
					thing.SetShortDescription(res.ElementAt(index).Key);
				}
			}
		}
		public static void ReadLongDesc(string path)
		{
			var res = File.ReadAllLines(path).Select((v, I) => new { Index = I, Value = v }).GroupBy(p => p.Index / 2).ToDictionary(g => g.First().Value, g => g.Last().Value);
			for (int index = 0; index < res.Count; index++)
			{
				ETGModConsole.Log($"{res.ElementAt(index).Value}'s long description is now: {res.ElementAt(index).Key}");

				Gun gun = PickupObjectDatabase.GetByName(res.ElementAt(index).Value) as Gun;
				var thing = PickupObjectDatabase.GetByName(res.ElementAt(index).Value);

				if (thing is Gun && thing != null)
				{
					thing.SetLongDescription(res.ElementAt(index).Key.CapitalizeFirst());
				}
				else if (thing != null)
				{
					thing.SetLongDescription(res.ElementAt(index).Key.CapitalizeFirst());
				}
			}
		}
		private void renameGun(string[] args)
		{
			bool flag = !Module.ArgCount(args, 2, 2);
			if (!flag)
			{

				string text = args[0];
				bool flag3 = !Game.Items.ContainsID(text);
				if (flag3)
				{
					ETGModConsole.Log(string.Format("Invalid item ID {0}!", text), false);
				}
				else
				{
					ETGModConsole.Log(string.Concat(new object[]
					{
						"Attempting to change gun name",
						args[0],
						" (numeric ",
						text,
						"), class ",
						Game.Items.Get(text).GetType()
					}), false);

					bool flag4 = args.Length < 2;
					if (flag4)
					{
						Log("At least 2 arguments required.");
					}
					
					
					string name = args[1];
					TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
					PickupObject pickupObject = Game.Items[text];
					Gun gun = PickupObjectDatabase.GetByName(pickupObject.name) as Gun;
					name = textInfo.ToTitleCase(name.ToLower().Replace("_", " ") );
					gun.SetName(name);
					Log($"{text}'s name is now " + name);

					ItemNames.Add(pickupObject.name, name);

					using (StreamWriter file = new StreamWriter(NameFilePath, true))
					{
							file.WriteLine(name);
							file.WriteLine($"{gun}".Split(' ')[0]);
					}

				}
			}
		}

		private void RenameItem(string[] args)
		{
			bool flag = !Module.ArgCount(args, 2, 2);
			if (!flag)
			{
				string text = args[0];
				bool flag3 = !Game.Items.ContainsID(text);
				if (flag3)
				{
					ETGModConsole.Log(string.Format("Invalid item ID {0}!", text), false);
				}
				else
				{
					ETGModConsole.Log(string.Concat(new object[]
					{
							"Attempting to change item name",
							args[0],
							" (numeric ",
							text,
							"), class ",
							Game.Items.Get(text).GetType()
					}), false);
					bool flag4 = args.Length < 2;
					if (flag4)
					{
						Log("At least 2 arguments required.");
					}
					string name = args[1];
					TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
					PickupObject pickupObject = Game.Items[text];
					PickupObject item = PickupObjectDatabase.GetByName(pickupObject.name);
					name = textInfo.ToTitleCase(name.ToLower().Replace("_", " "));
					item.SetName(name);
					Log($"{text}'s name is now " + name);
					using (StreamWriter file = new StreamWriter(NameFilePath, true))
					{
						if (item.PickupObjectId > 823 || item.PickupObjectId < 0)
						{
							file.WriteLine(name);
							file.WriteLine($"{text}");
						}
						else
						{
							file.WriteLine(name);
							file.WriteLine($"{item}".Split(' ')[0]);
						}
					}
				}
			}
		}
		private void SetItemShortDesc(string[] args)
		{
			bool flag = !Module.ArgCount(args, 2, 2);
			if (!flag)
			{

				string text = args[0];
				bool flag3 = !Game.Items.ContainsID(text);
				if (flag3)
				{
					ETGModConsole.Log(string.Format("Invalid item ID {0}!", text), false);
				}
				else
				{
					ETGModConsole.Log(string.Concat(new object[]
					{
							"Attempting to change item short description",
							args[0],
							" (numeric ",
							text,
							"), class ",
							Game.Items.Get(text).GetType()
					}), false);
					bool flag4 = args.Length < 2;
					if (flag4)
					{
						Log("At least 2 arguments required.");
					}


					string name = args[1];
					TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
					PickupObject pickupObject = Game.Items[text];
					PickupObject item = PickupObjectDatabase.GetByName(pickupObject.name);
					name = textInfo.ToTitleCase(name.ToLower().Replace("_", " "));
					item.SetShortDescription(name);
					Log($"{text}'s short description is now " + name);
					using (StreamWriter file = new StreamWriter(ShortDescPath, true))
					{
						file.WriteLine(name);
						file.WriteLine($"{item}".Split(' ')[0]);
					}
				}
			}
		}
		private void SetGunShortDesc(string[] args)
		{
			bool flag = !Module.ArgCount(args, 2, 2);
			if (!flag)
			{

				string text = args[0];
				bool flag3 = !Game.Items.ContainsID(text);
				if (flag3)
				{
					ETGModConsole.Log(string.Format("Invalid item ID {0}!", text), false);
				}
				else
				{
					ETGModConsole.Log(string.Concat(new object[]
					{
							"Attempting to change gun short description",
							args[0],
							" (numeric ",
							text,
							"), class ",
							Game.Items.Get(text).GetType()
					}), false);
					bool flag4 = args.Length < 2;
					if (flag4)
					{
						Log("At least 2 arguments required.");
					}


					string name = args[1];
					TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
					PickupObject pickupObject = Game.Items[text];
					Gun gun = PickupObjectDatabase.GetByName(pickupObject.name) as Gun;
					name = textInfo.ToTitleCase(name.ToLower().Replace("_", " "));
					gun.SetShortDescription(name);
					Log($"{text}'s short description is now " + name);
					using (StreamWriter file = new StreamWriter(ShortDescPath, true))
					{
						file.WriteLine(name);
						file.WriteLine($"{gun}".Split(' ')[0]);
					}
				}
			}
		}
		private void SetItemLongDesc(string[] args)
		{
			bool flag = !Module.ArgCount(args, 2, 2);
			if (!flag)
			{

				string text = args[0];
				bool flag3 = !Game.Items.ContainsID(text);
				if (flag3)
				{
					ETGModConsole.Log(string.Format("Invalid item ID {0}!", text), false);
				}
				else
				{
					ETGModConsole.Log(string.Concat(new object[]
					{
							"Attempting to change item long description",
							args[0],
							" (numeric ",
							text,
							"), class ",
							Game.Items.Get(text).GetType()
					}), false);
					bool flag4 = args.Length < 2;
					if (flag4)
					{
						Log("At least 2 arguments required.");
					}


					string name = args[1];
					PickupObject pickupObject = Game.Items[text];
					PickupObject item = PickupObjectDatabase.GetByName(pickupObject.name);
					name = name.ToLower().Replace("_", " ");
					item.SetLongDescription(name.CapitalizeFirst());
					Log($"{text}'s long description is now " + name);
					using (StreamWriter file = new StreamWriter(LongDescPath, true))
					{
						file.WriteLine(name);
						file.WriteLine($"{item}".Split(' ')[0]);
					}
				}
			}
		}

		private void SetGunLongDesc(string[] args)
		{
			bool flag = !Module.ArgCount(args, 2, 2);
			if (!flag)
			{

				string text = args[0];
				bool flag3 = !Game.Items.ContainsID(text);
				if (flag3)
				{
					ETGModConsole.Log(string.Format("Invalid item ID {0}!", text), false);
				}
				else
				{
					ETGModConsole.Log(string.Concat(new object[]
					{
							"Attempting to change gun long description",
							args[0],
							" (numeric ",
							text,
							"), class ",
							Game.Items.Get(text).GetType()
					}), false);
					bool flag4 = args.Length < 2;
					if (flag4)
					{
						Log("At least 2 arguments required.");
					}


					string name = args[1];
					PickupObject pickupObject = Game.Items[text];
					Gun gun = PickupObjectDatabase.GetByName(pickupObject.name) as Gun;
					name = name.ToLower().Replace("_", " ");
					gun.SetLongDescription(name.CapitalizeFirst());
					Log($"{text}'s long description is now " + name);
					using (StreamWriter file = new StreamWriter(LongDescPath, true))
					{
						file.WriteLine(name);
						file.WriteLine($"{gun}".Split(' ')[0]);
					}
				}
			}
		}
		public static bool ArgCount(string[] args, int min, int max)
		{
			bool flag = args.Length >= min && args.Length <= max;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = min == max;
				if (flag2)
				{
				}
				result = false;
			}
			return result;
		}

		protected static AutocompletionSettings GiveAutocompletionSettings = new AutocompletionSettings(delegate (string input)
        {
            List<string> list = new List<string>();
            foreach (string text in Game.Items.IDs)
            {
                bool flag = text.AutocompletionMatch(input.ToLower());
                if (flag)
                {
                    Console.WriteLine(string.Format("INPUT {0} KEY {1} MATCH!", input, text));
                    list.Add(text.Replace("gungeon:", ""));
                }
                else
                {
                    Console.WriteLine(string.Format("INPUT {0} KEY {1} NO MATCH!", input, text));
                }
            }
            return list.ToArray();
        });
		
		private static Action<string[]> NameSet;
		protected static AutocompletionSettings AutocompletionSettings;
	}

	public static class StringExtension
	{
		public static string CapitalizeFirst(this string s)
		{
			bool IsNewSentense = true;
			var result = new StringBuilder(s.Length);
			for (int i = 0; i < s.Length; i++)
			{
				if (IsNewSentense && char.IsLetter(s[i]))
				{
					result.Append(char.ToUpper(s[i]));
					IsNewSentense = false;
				}
				else
					result.Append(s[i]);

				if (s[i] == '!' || s[i] == '?' || s[i] == '.')
				{
					IsNewSentense = true;
				}
			}

			return result.ToString();
		}

		
	}
}

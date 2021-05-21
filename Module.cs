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

namespace RenameMod
{
	public class Module : ETGModule
	{
		public static ETGModuleMetadata metadata;
		public static string namefile = "CustomItemNames.json";
		public static string shortdescfile = "CustomItemShortDesc.json";
		public static string longdescfile = "CustomItemLongDesc.json";


		public static string RenameDirectory = "CustomRenames/";
		public static string NameFilePath = Path.Combine(Module.RenameDirectory, "CustomItemNames.json");
		public static string ShortDescPath = Path.Combine(Module.RenameDirectory, "CustomItemShortDesc.json");
		public static string LongDescPath = Path.Combine(Module.RenameDirectory, "CustomItemLongDesc.json");

		public static readonly string MOD_NAME = "Item Rename Mod";
		public static readonly string VERSION = "1.0.0";
		public static readonly string TEXT_COLOR = "#00FFFF";

		JObject nameJson = new JObject();
		JObject ShortDescJson = new JObject();
		JObject LongDescJson = new JObject();

		public override void Start()
		{
			try
			{
				metadata = this.Metadata;
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
					if (new FileInfo(NameFilePath).Length > 0)
					{
						var output = File.ReadAllText(NameFilePath);
						JObject o = JObject.Parse(output);

						foreach (var pair in o)
						{
							ETGModConsole.Log($"{pair.Key}'s name is now: {pair.Value}");

							Gun gun = PickupObjectDatabase.GetByName(pair.Key.ToString()) as Gun;
							var thing = PickupObjectDatabase.GetByName(pair.Key.ToString());

							if (thing is Gun && thing != null)
							{
								thing.SetName(pair.Value.ToString());
							}
							else if (thing != null)
							{
								thing.SetName(pair.Value.ToString());
							}
						}
					}
				}
				if (File.Exists(ShortDescPath))
				{
					
					if (new FileInfo(ShortDescPath).Length > 0)
					{
						var output = File.ReadAllText(ShortDescPath);
						JObject o = JObject.Parse(output);

						foreach (var pair in o)
						{
							ETGModConsole.Log($"{pair.Key}'s name is now: {pair.Value}");

							Gun gun = PickupObjectDatabase.GetByName(pair.Key.ToString()) as Gun;
							var thing = PickupObjectDatabase.GetByName(pair.Key.ToString());

							if (thing is Gun && thing != null)
							{
								thing.SetShortDescription(pair.Value.ToString());
							}
							else if (thing != null)
							{
								thing.SetShortDescription(pair.Value.ToString());
							}
						}
					}
				}

				if (File.Exists(LongDescPath))
				{
					
					if (new FileInfo(LongDescPath).Length > 0)
					{
						var output = File.ReadAllText(LongDescPath);
						JObject o = JObject.Parse(output);
						foreach (var pair in o)
						{
							ETGModConsole.Log($"{pair.Key}'s name is now: {pair.Value}");

							Gun gun = PickupObjectDatabase.GetByName(pair.Key.ToString()) as Gun;
							var thing = PickupObjectDatabase.GetByName(pair.Key.ToString());

							if (thing is Gun && thing != null)
							{
								thing.SetLongDescription(pair.Value.ToString());
							}
							else if (thing != null)
							{
								thing.SetLongDescription(pair.Value.ToString());
							}
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
					Log("type in: rnm setname gun_id new gun name, to rename an item");
					Log("this also works with things like descriptions");
				});


				ETGModConsole.Commands.GetGroup("rnm").AddUnit("setname", new Action<string[]>(this.RenameItem), Module.GiveAutocompletionSettings);
				ETGModConsole.Commands.GetGroup("rnm").AddUnit("setshortdesc", new Action<string[]>(this.SetItemShortDesc), Module.GiveAutocompletionSettings);
				ETGModConsole.Commands.GetGroup("rnm").AddUnit("setlongdesc", new Action<string[]>(this.SetItemLongDesc), Module.GiveAutocompletionSettings);
				ETGModConsole.Commands.GetGroup("rnm").AddUnit("clearname", new Action<string[]>(this.clearname), Module.GiveAutocompletionSettings);
				ETGModConsole.Commands.GetGroup("rnm").AddUnit("clearshortdesc", new Action<string[]>(this.clearShortDesc), Module.GiveAutocompletionSettings);
				ETGModConsole.Commands.GetGroup("rnm").AddUnit("clearlongdesc", new Action<string[]>(this.clearLongDesc), Module.GiveAutocompletionSettings);

				if (new FileInfo(NameFilePath).Length > 0)
					nameJson = JObject.Parse(File.ReadAllText(NameFilePath));
				if (new FileInfo(ShortDescPath).Length > 0)
					ShortDescJson = JObject.Parse(File.ReadAllText(ShortDescPath));
				if (new FileInfo(LongDescPath).Length > 0)
					LongDescJson = JObject.Parse(File.ReadAllText(LongDescPath));


				LoadRenames();
			}
			catch (Exception e)
			{
				AdvancedLogging.Log("mod Broke heres why: " + e, Color.red, true);
			}
			AdvancedLogging.Log($"{MOD_NAME} v{VERSION} started successfully.", Color.green, false, true);
		}

		protected static AutocompletionSettings GiveAutocompletionSettingsEnemy = new AutocompletionSettings(delegate (string input) {
			List<string> ret = new List<string>();
			foreach (string key in Game.Enemies.IDs)
			{
				if (key.AutocompletionMatch(input.ToLower()))
				{
					ret.Add(key.Replace("gungeon:", ""));
				}
			}
			return ret.ToArray();
		});

		public static void Log(string text, string color = "FFFFFF")
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
				string text2 = Path.Combine(text, namefile);
				string text3 = Path.Combine(text, shortdescfile);
				string text4 = Path.Combine(text, longdescfile);
				bool flag = File.Exists(text2);
				if (flag)
				{
					ReadNames(text2);
				}


				bool flag2 = File.Exists(text3);
				if (flag2)
				{
					ReadShortDesc(text3);
				}

				bool flag3 = File.Exists(text4);
				if (flag3)
				{
					ReadLongDesc(text4);
				}

			}
		}

		public static void ReadNames(string path)
		{

			if (new FileInfo(path).Length > 0)
			{
				var output = File.ReadAllText(path);
				JObject o = JObject.Parse(output);

				foreach (var pair in o)
				{
					ETGModConsole.Log($"{pair.Key}'s name is now: {pair.Value}");

					Gun gun = PickupObjectDatabase.GetByName(pair.Key.ToString()) as Gun;
					var thing = PickupObjectDatabase.GetByName(pair.Key.ToString());

					if (thing is Gun && thing != null)
					{
						thing.SetName(pair.Value.ToString());
					}
					else if (thing != null)
					{
						thing.SetName(pair.Value.ToString());
					}
				}
			}
		}

		public static void ReadShortDesc(string path)
		{
			if (new FileInfo(path).Length > 0)
			{
				var output = File.ReadAllText(path);
				JObject o = JObject.Parse(output);

				foreach (var pair in o)
				{
					ETGModConsole.Log($"{pair.Key}'s name is now: {pair.Value}");

					Gun gun = PickupObjectDatabase.GetByName(pair.Key.ToString()) as Gun;
					var thing = PickupObjectDatabase.GetByName(pair.Key.ToString());

					if (thing is Gun && thing != null)
					{
						thing.SetShortDescription(pair.Value.ToString());
					}
					else if (thing != null)
					{
						thing.SetShortDescription(pair.Value.ToString());
					}
				}
			}
		}
		public static void ReadLongDesc(string path)
		{
			if (new FileInfo(path).Length > 0)
			{
				var output = File.ReadAllText(path);
				JObject o = JObject.Parse(output);

				foreach (var pair in o)
				{
					ETGModConsole.Log($"{pair.Key}'s name is now: {pair.Value}");

					Gun gun = PickupObjectDatabase.GetByName(pair.Key.ToString()) as Gun;
					var thing = PickupObjectDatabase.GetByName(pair.Key.ToString());

					if (thing is Gun && thing != null)
					{
						thing.SetLongDescription(pair.Value.ToString());
					}
					else if (thing != null)
					{
						thing.SetLongDescription(pair.Value.ToString());
					}
				}
			}
		}
		

		private void RenameItem(string[] args)
		{

				string text = args[0];
				bool flag3 = !Game.Items.ContainsID(text);
				if (flag3)
				{
					ETGModConsole.Log(string.Format("Invalid item ID {0}!", text), false);
				}
				else
				{
		
					bool flag4 = args.Length < 2;
					if (flag4)
					{
						Log("At least 2 arguments required.");
					}
					string name = string.Empty;
					List<string> nameSetter = new List<string>();
					string[] nameSetter2 = new string[] { };
					// WHY DO I NEED SO MANY FUCKING NAME SETTERS;
					IEnumerable<int> words = Enumerable.Range(1, args.Length - 1);
					foreach (int num in words)
					{
						nameSetter.Add(args[num]);
						nameSetter2 = nameSetter.ToArray();
					}
					name = string.Join(" ", nameSetter2);
					TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
					PickupObject pickupObject = Game.Items[text];
					PickupObject item = PickupObjectDatabase.GetByName(pickupObject.name);
					item.SetName(name.ToTitleCaseInvariant());
					Log($"{text}'s name is now " + name.ToTitleCaseInvariant());
					string itemname = $"{item}".Split('(')[0].TrimEnd();
					if (nameJson[itemname] != null)
						nameJson.Property(itemname).Remove();
					nameJson.Add(itemname, name);
					File.WriteAllText(NameFilePath, nameJson.ToString());
				}
		}
		private void SetItemShortDesc(string[] args)
		{

				string text = args[0];
				bool flag3 = !Game.Items.ContainsID(text);
				if (flag3)
				{
					ETGModConsole.Log(string.Format("Invalid item ID {0}!", text), false);
				}
				else
				{

					bool flag4 = args.Length < 2;
					if (flag4)
					{
						Log("At least 2 arguments required.");
					}
					string name = string.Empty;
					List<string> nameSetter = new List<string>();
					string[] nameSetter2 = new string[] { };
					// WHY DO I NEED SO MANY FUCKING NAME SETTERS;
					IEnumerable<int> words = Enumerable.Range(1, args.Length - 1);
					foreach (int num in words)
					{
						nameSetter.Add(args[num]);
						nameSetter2 = nameSetter.ToArray();
					}
					name = string.Join(" ", nameSetter2);
					PickupObject pickupObject = Game.Items[text];
					PickupObject item = PickupObjectDatabase.GetByName(pickupObject.name);
					item.SetShortDescription(name.ToTitleCaseInvariant());
					Log($"{text}'s short description is now " + name.ToTitleCaseInvariant());
					string itemname = $"{item}".Split('(')[0].TrimEnd();
					if (ShortDescJson[itemname] != null)
						ShortDescJson.Property(itemname).Remove();
					ShortDescJson.Add(itemname, name);
					File.WriteAllText(ShortDescPath, ShortDescJson.ToString());
				}
		}

		private void SetItemLongDesc(string[] args)
		{
				string text = args[0];
				bool flag3 = !Game.Items.ContainsID(text);
				if (flag3)
				{
					ETGModConsole.Log(string.Format("Invalid item ID {0}!", text), false);
				}
				else
				{
					bool flag4 = args.Length < 2;
					if (flag4)
					{
						Log("At least 2 arguments required.");
					}

					string name = string.Empty;
					List<string> nameSetter = new List<string>();
					string[] nameSetter2 = new string[] { };
					// WHY DO I NEED SO MANY FUCKING NAME SETTERS;
					IEnumerable<int> words = Enumerable.Range(1, args.Length - 1);
					foreach (int num in words)
					{
						nameSetter.Add(args[num]);
						nameSetter2 = nameSetter.ToArray();
					}
					name = string.Join(" ", nameSetter2);
					PickupObject pickupObject = Game.Items[text];
					PickupObject item = PickupObjectDatabase.GetByName(pickupObject.name);
					item.SetLongDescription(name.CapitalizeFirst());
					Log($"{text}'s long description is now " + name);
					string itemname = $"{item}".Split('(')[0].TrimEnd();
					if (LongDescJson[itemname] != null)
						LongDescJson.Property(itemname).Remove();
					LongDescJson.Add(itemname, name);
					File.WriteAllText(LongDescPath, LongDescJson.ToString());
				}
		}

		private void clearname(string[] args)
		{
				string text = args[0];
				bool flag3 = !Game.Items.ContainsID(text);
				if (flag3)
				{
					ETGModConsole.Log(string.Format("Invalid item ID {0}!", text), false);
				}
				else
				{
					bool flag4 = args.Length < 1;
					if (flag4)
					{
						Log("At least 1 argument required.");
					}

					PickupObject pickupObject = Game.Items[text];
					PickupObject item = PickupObjectDatabase.GetByName(pickupObject.name);

					string itemname = $"{item}".Split('(')[0].TrimEnd();
					if (nameJson[itemname] != null)
					{
						nameJson.Property(itemname).Remove();
					}
					File.WriteAllText(NameFilePath, nameJson.ToString());
					ETGModConsole.Log($"{text}'s name successfully cleared!, restart game for it to take effect.");
				}
		}
		private void clearShortDesc(string[] args)
		{
			string text = args[0];
			bool flag3 = !Game.Items.ContainsID(text);
			if (flag3)
			{
				ETGModConsole.Log(string.Format("Invalid item ID {0}!", text), false);
			}
			else
			{
				bool flag4 = args.Length < 1;
				if (flag4)
				{
					Log("At least 1 argument required.");
				}

				PickupObject pickupObject = Game.Items[text];
				PickupObject item = PickupObjectDatabase.GetByName(pickupObject.name);

				string itemname = $"{item}".Split('(')[0].TrimEnd();
				if (ShortDescJson[itemname] != null)
				{
					ShortDescJson.Property(itemname).Remove();
				}
				File.WriteAllText(NameFilePath, nameJson.ToString());
				ETGModConsole.Log($"{text}'s name successfully cleared!, restart game for it to take effect.");
			}
		}
		private void clearLongDesc(string[] args)
		{
			string text = args[0];
			bool flag3 = !Game.Items.ContainsID(text);
			if (flag3)
			{
				ETGModConsole.Log(string.Format("Invalid item ID {0}!", text), false);
			}
			else
			{
				bool flag4 = args.Length < 1;
				if (flag4)
				{
					Log("At least 1 argument required.");
				}

				PickupObject pickupObject = Game.Items[text];
				PickupObject item = PickupObjectDatabase.GetByName(pickupObject.name);

				string itemname = $"{item}".Split('(')[0].TrimEnd();
				if (LongDescJson[itemname] != null)
				{
					LongDescJson.Property(itemname).Remove();
				}
				File.WriteAllText(NameFilePath, nameJson.ToString());
				ETGModConsole.Log($"{text}'s name successfully cleared!, restart game for it to take effect.");
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

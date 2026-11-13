using System;
using System.Linq;
using Innersloth.DebugTool;
using UnityEngine;

public class DebugToolTestCommands : MonoBehaviour
{
	public enum DogTypes
	{
		GoldenRetriever,
		BlackLab,
		GermanShepherd,
		Husky
	}

	public enum CatTypes
	{
		HouseCat,
		Panther,
		Bobcat,
		Meercat
	}

	public enum EggStyle
	{
		Fried,
		OverEasy,
		OverMedium,
		OverHard,
		Scrambled,
		SunnySideUp
	}

	public enum BreakfastSide
	{
		None,
		Beans,
		Fries,
		Fruit,
		Hashbrowns,
		Sausages
	}

	[SerializeField]
	private DebugToolOverlay overlay;

	[SerializeField]
	private bool testLogsOnStartup;

	private void Start()
	{
		overlay.RegisterInputProvider(new DebugToolKeyboardInputProvider());
		overlay.RegisterKeyboardInputProvider(new DebugToolNullKeyboardInputProvider());
		DebugTool.AddCustomParser((string name) => (DogTypes)Enum.Parse(typeof(DogTypes), name));
		DebugTool.AddCustomParser((string name) => (CatTypes)Enum.Parse(typeof(CatTypes), name));
		DebugTool.AddCustomParser((string name) => (EggStyle)Enum.Parse(typeof(EggStyle), name));
		DebugTool.AddCustomParser((string name) =>
		{
			if (Enum.TryParse<BreakfastSide>(name, out var result))
			{
				return result;
			}
			throw new ArgumentException("Invalid BreakfastSide '" + name + "'");
		});
		DebugTool.AddCustomDropdownValueCollector<DogTypes>(() => Enum.GetNames(typeof(DogTypes)).ToArray());
		DebugTool.AddCustomDropdownValueCollector<CatTypes>(() => Enum.GetNames(typeof(CatTypes)).ToArray());
		DebugTool.AddCustomDropdownValueCollector<EggStyle>(() => Enum.GetNames(typeof(EggStyle)).ToArray());
		DebugTool.AddCustomDropdownValueCollector<BreakfastSide>(() => Enum.GetNames(typeof(BreakfastSide)).ToArray());
		DebugTool.RegisterCommandHandler(this);
		DebugTool.ListenToLogs();
		if (testLogsOnStartup)
		{
			LogABunchOfRandomStuff();
		}
	}

	private void OnDestroy()
	{
		DebugTool.UnregisterCommandHandler(this);
	}

	private void LogABunchOfRandomStuff()
	{
		Debug.Log("The following logs are being sent to populate the Debug Tool log with content, for testing!");
		for (int i = 0; i < 100; i++)
		{
			switch (UnityEngine.Random.Range(0, 4))
			{
			case 0:
				Debug.Log("This is a test log ~~~");
				break;
			case 1:
				Debug.LogWarning("Omg Warning Warning.");
				break;
			case 2:
				Debug.LogError("This is super bad~~~! Maybe you should investigate!");
				break;
			case 3:
				Debug.LogException(new Exception("I am an exception I bet"));
				break;
			}
		}
	}

	[DebugCommand("Thing/Animal/Ape/Baboon", "", false)]
	public static void ThingAnimalApeBaboon()
	{
		DebugTool.LogCommandSuccess("Baboon");
	}

	[DebugCommand("Thing/Animal/Ape/Chimpanzee", "", false)]
	public static void ThingAnimalApeChimpanzee()
	{
		DebugTool.LogCommandInfo("Chimpanzee");
	}

	[DebugCommand("Thing/Animal/Bear", "", false)]
	public static void ThingAnimalBear()
	{
		DebugTool.LogCommandInfo("Bear");
	}

	[DebugCommand("Thing/Animal/Cat", "", false)]
	public static void ThingAnimalCat([DebugParameter("House Cat, Bobcat, Panther, etc..")] CatTypes catType)
	{
		DebugTool.LogCommandSuccess($"Cat (Type: {catType})");
	}

	[DebugCommand("Thing/Animal/Dog", "", false)]
	public static void ThingAnimalDog([DebugParameter("Golden, German Shepherd, Black Lab etc..")] DogTypes dogType)
	{
		DebugTool.LogCommandSuccess($"Dog (Type: {dogType})");
	}

	[DebugCommand("Thing/Animal/Human", "", false)]
	public static void ThingAnimalHuman()
	{
		DebugTool.LogCommandInfo("Humans are animals!!!");
	}

	[DebugCommand("Thing/Fruit/Apple", "", false)]
	public static void ThingFruitApple([DebugParameter("Red Delicions, Gala, etc")] string type)
	{
		DebugTool.LogCommandSuccess("Apple " + type);
	}

	[DebugCommand("Thing/Fruit/Banana", "", false)]
	public static void ThingFruitBanana([DebugParameter("Inches: 2.4")] float inches, [DebugParameter("0 - 1")] float ripeness)
	{
		DebugTool.LogCommandSuccess($"Banana (Length: {inches} inches) (Ripeness: {ripeness})");
	}

	[DebugCommand("Thing/Fruit/Citrus", "", false)]
	public static void ThingFruitCitrus()
	{
		DebugTool.LogCommandInfo("Citrus");
	}

	[DebugCommand("Thing/Fruit/Durian", "", false)]
	public static void ThingFruitDurian()
	{
		DebugTool.LogCommandInfo("Durian");
	}

	[DebugCommand("Thing/Rainbow", "", false)]
	public static void Rainbow()
	{
		DebugTool.LogCommandSuccess("RAINBOW!!!");
	}

	[DebugCommand("Bacon and Eggs", "This is a breakfast food that many enjoy. Asks how many pieces of bacon, what style of eggs, and what kind of side you'd like. Note: You MUST say 'yes' to Avocado Toast.", false)]
	public static void BaconAndEggs([DebugParameter("1 - 100")] int bacon, EggStyle eggStyle, BreakfastSide side, [DebugParameter("YES")] string avocadoToast)
	{
		if (avocadoToast.ToLower() != "yes")
		{
			throw new Exception("You must always have avocado toast. Enter 'Yes' for that field.");
		}
		DebugTool.LogCommandSuccess($"Bacon: {bacon}, Eggs: {eggStyle}, Side of: {side}, Avocado toast: {avocadoToast}");
	}

	[DebugCommand("Error/Throw an exception", "", false)]
	public static void ThrowAnException()
	{
		throw new Exception("I'm a fun exception");
	}

	[DebugCommand("Error/Display a LogCommandError", "", false)]
	public static void LogCommandError()
	{
		DebugTool.LogCommandError("This is a LogCommandError!");
	}

	[DebugCommand("More/Commands/Here", "", false)]
	public static void MoreCommandsHere()
	{
		DebugTool.LogCommandInfo("More commands here");
	}

	[DebugCommand("More/Commands/There", "", false)]
	public static void MoreCommandsThere()
	{
		DebugTool.LogCommandInfo("More commands there");
	}

	[DebugCommand("More/Commands/Everywhere", "", false)]
	public static void MoreCommandsEverywhere()
	{
		DebugTool.LogCommandInfo("More commands everywhere");
	}

	[DebugCommand("Cosmetics/Test Hat", "", false)]
	public static void TestHat([DebugParameter("Hat ID")] HatData hat)
	{
		DebugTool.LogCommandInfo(hat.ProdId);
	}

	[DebugCommand("Cosmetics/Test Visor", "", false)]
	public static void TestVisor([DebugParameter("Visor ID")] VisorData visor)
	{
		DebugTool.LogCommandInfo(visor.ProdId);
	}

	[DebugCommand("Cosmetics/Test Skin", "", false)]
	public static void TestSkin([DebugParameter("Skin ID")] SkinData skin)
	{
		DebugTool.LogCommandInfo(skin.ProdId);
	}

	[DebugCommand("Cosmetics/Test Pet", "", false)]
	public static void TestPet([DebugParameter("Pet ID")] PetData pet)
	{
		DebugTool.LogCommandInfo(pet.ProdId);
	}
}

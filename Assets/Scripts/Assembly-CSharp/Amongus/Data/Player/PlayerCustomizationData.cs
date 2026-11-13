using Newtonsoft.Json;
using System;
using Cpp2ILInjected;

namespace AmongUs.Data.Player
{
	[JsonObject(MemberSerialization.OptIn)]
	public class PlayerCustomizationData : AbstractSaveDataGroup
	{
		public const string DEFAULT_NAME = "";

		public const byte DEFAULT_COLOR_ID = 0;

		public const string DEFAULT_PET = "";

		public const string DEFAULT_HAT = "";

		public const string DEFAULT_SKIN = "";

		public const string DEFAULT_VISOR = "";

		public const string DEFAULT_NAME_PLATE = "";

		[JsonProperty("name")]
		public string name;

		[JsonProperty("colorID")]
		public byte colorID;

		[JsonProperty("pet")]
		public string pet;

		[JsonProperty("hat")]
		public string hat;

		[JsonProperty("skin")]
		public string skin;

		[JsonProperty("visor")]
		public string visor;

		[JsonProperty("namePlate")]
		public string namePlate;

		private Action m_OnNameChanged;

		private Action m_OnColorChanged;

		private Action m_OnPetChanged;

		private Action m_OnHatChanged;

		private Action m_OnSkinChanged;

		private Action m_OnVisorChanged;

		private Action m_OnNamePlateChanged;

		public string Name
		{
				get
			{
				// TODO(verify): rebuilt (decomp stub)
				if (string.IsNullOrWhiteSpace(name))
				{
					name = DestroyableSingleton<AccountManager>.Instance.GetRandomName();
				}
				if (name.Length > 10) name = name.Substring(0, 10);
				return name;
			}
				set
			{
				// TODO(verify): rebuilt (decomp stub)
				SetValue(ref name, value, m_OnNameChanged);
			}
		}

		public byte Color
		{
			get
			{
				return colorID;
			}
			set
			{
				SetValue(ref colorID, value, m_OnColorChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public string Pet
		{
			get
			{
				return pet;
			}
			set
			{
				SetValue(ref pet, value, m_OnPetChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public string Hat
		{
			get
			{
				return hat;
			}
			set
			{
				SetValue(ref hat, value, m_OnHatChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public string Skin
		{
			get
			{
				return skin;
			}
			set
			{
				SetValue(ref skin, value, m_OnSkinChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public string Visor
		{
			get
			{
				return visor;
			}
			set
			{
				SetValue(ref visor, value, m_OnVisorChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public string NamePlate
		{
			get
			{
				return namePlate;
			}
			set
			{
				SetValue(ref namePlate, value, m_OnNamePlateChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public event Action OnNameChanged
		{
			add
			{
				m_OnNameChanged = (Action)System.Delegate.Combine(m_OnNameChanged, value);
			}
			remove
			{
				m_OnNameChanged = (Action)System.Delegate.Remove(m_OnNameChanged, value);
			}
		}

		public event Action OnColorChanged
		{
			add
			{
				m_OnColorChanged = (Action)System.Delegate.Combine(m_OnColorChanged, value);
			}
			remove
			{
				m_OnColorChanged = (Action)System.Delegate.Remove(m_OnColorChanged, value);
			}
		}

		public event Action OnPetChanged
		{
			add
			{
				m_OnPetChanged = (Action)System.Delegate.Combine(m_OnPetChanged, value);
			}
			remove
			{
				m_OnPetChanged = (Action)System.Delegate.Remove(m_OnPetChanged, value);
			}
		}

		public event Action OnHatChanged
		{
			add
			{
				m_OnHatChanged = (Action)System.Delegate.Combine(m_OnHatChanged, value);
			}
			remove
			{
				m_OnHatChanged = (Action)System.Delegate.Remove(m_OnHatChanged, value);
			}
		}

		public event Action OnSkinChanged
		{
			add
			{
				m_OnSkinChanged = (Action)System.Delegate.Combine(m_OnSkinChanged, value);
			}
			remove
			{
				m_OnSkinChanged = (Action)System.Delegate.Remove(m_OnSkinChanged, value);
			}
		}

		public event Action OnVisorChanged
		{
			add
			{
				m_OnVisorChanged = (Action)System.Delegate.Combine(m_OnVisorChanged, value);
			}
			remove
			{
				m_OnVisorChanged = (Action)System.Delegate.Remove(m_OnVisorChanged, value);
			}
		}

		public event Action OnNamePlateChanged
		{
			add
			{
				m_OnNamePlateChanged = (Action)System.Delegate.Combine(m_OnNamePlateChanged, value);
			}
			remove
			{
				m_OnNamePlateChanged = (Action)System.Delegate.Remove(m_OnNamePlateChanged, value);
			}
		}

		public PlayerCustomizationData()
		{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
			name = "";
			pet = "";
			hat = "";
			skin = "";
			visor = "";
			namePlate = "";
		}
	}
}

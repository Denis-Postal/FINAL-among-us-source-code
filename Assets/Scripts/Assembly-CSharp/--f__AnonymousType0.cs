using System;
using Cpp2ILInjected;

internal sealed class _003C_003Ef__AnonymousType0<_003Crole_003Ej__TPar, _003Cchance_003Ej__TPar>
{
	private readonly _003Crole_003Ej__TPar _003Crole_003Ei__Field;

	private readonly _003Cchance_003Ej__TPar _003Cchance_003Ei__Field;

	public _003Crole_003Ej__TPar role => _003Crole_003Ei__Field;

	public _003Cchance_003Ej__TPar chance => _003Cchance_003Ei__Field;

	public _003C_003Ef__AnonymousType0(_003Crole_003Ej__TPar role, _003Cchance_003Ej__TPar chance)
	{
		_003Crole_003Ei__Field = role;
		_003Cchance_003Ei__Field = chance;
	}

	public override bool Equals(object value)
	{
		_003C_003Ef__AnonymousType0<_003Crole_003Ej__TPar, _003Cchance_003Ej__TPar> other = value as _003C_003Ef__AnonymousType0<_003Crole_003Ej__TPar, _003Cchance_003Ej__TPar>;
		return other != null
			&& System.Collections.Generic.EqualityComparer<_003Crole_003Ej__TPar>.Default.Equals(_003Crole_003Ei__Field, other._003Crole_003Ei__Field)
			&& System.Collections.Generic.EqualityComparer<_003Cchance_003Ej__TPar>.Default.Equals(_003Cchance_003Ei__Field, other._003Cchance_003Ei__Field);
	}

	public override int GetHashCode()
	{
		return unchecked((-1521134295 * (-1521134295 * -1253459369 + System.Collections.Generic.EqualityComparer<_003Crole_003Ej__TPar>.Default.GetHashCode(_003Crole_003Ei__Field))) + System.Collections.Generic.EqualityComparer<_003Cchance_003Ej__TPar>.Default.GetHashCode(_003Cchance_003Ei__Field));
	}

	public override string ToString()
	{
		return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{{ role = {0}, chance = {1} }}", _003Crole_003Ei__Field, _003Cchance_003Ei__Field);
	}
}

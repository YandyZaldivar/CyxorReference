/*
  { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/>
  Copyright (C) 2017  Yandy Zaldivar

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU Affero General Public License as
  published by the Free Software Foundation, either version 3 of the
  License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Affero General Public License for more details.

  You should have received a copy of the GNU Affero General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.ComponentModel.DataAnnotations;

#if !NET35 && !NET40
using System.ComponentModel.DataAnnotations.Schema;
#endif

namespace Cyxor.Models
{
    public interface IKeyApiModel<TKey>
    {
#if !NET35
        [Key]
#endif
        TKey Id { get; set; }
    }

    public class KeyApiModel<TKey> : IKeyApiModel<TKey>
    {
#if !NET35
        [Key]
#endif
        public TKey Id { get; set; }
    }

    public interface IKeyApiModel<TKey1, TKey2>
    {
#if !NET35 && !NET40
        [NotMapped]
#endif
        TKey1 Id1 { get; set; }

#if !NET35 && !NET40
        [NotMapped]
#endif
        TKey2 Id2 { get; set; }
    }

    public class KeyApiModel<TKey1, TKey2> : IKeyApiModel<TKey1, TKey2>
    {
#if !NET35 && !NET40
        [NotMapped]
#endif
        public TKey1 Id1 { get; set; }

#if !NET35 && !NET40
        [NotMapped]
#endif
        public TKey2 Id2 { get; set; }
    }

    public interface IKeyApiModel<TKey1, TKey2, TKey3>
        : IKeyApiModel<TKey1, TKey2>
    {
#if !NET35 && !NET40
        [NotMapped]
#endif
        TKey3 Id3 { get; set; }
    }

    public class KeyApiModel<TKey1, TKey2, TKey3>
        : KeyApiModel<TKey1, TKey2>, IKeyApiModel<TKey1, TKey2, TKey3>
    {
#if !NET35 && !NET40
        [NotMapped]
#endif
        public TKey3 Id3 { get; set; }
    }

    public class KeyApiModel<TKey1, TKey2, TKey3, TKey4>
        : KeyApiModel<TKey1, TKey2, TKey3>
    {
#if !NET35 && !NET40
        [NotMapped]
#endif
        public TKey4 Id4 { get; set; }
    }

    public class KeyApiModel<TKey1, TKey2, TKey3, TKey4, TKey5>
        : KeyApiModel<TKey1, TKey2, TKey3, TKey4>
    {
#if !NET35 && !NET40
        [NotMapped]
#endif
        public TKey5 Id5 { get; set; }
    }

    public class KeyApiModel<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6>
        : KeyApiModel<TKey1, TKey2, TKey3, TKey4>
    {
#if !NET35 && !NET40
        [NotMapped]
#endif
        public TKey6 Id6 { get; set; }
    }

    public class KeyApiModel<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6, TKey7>
        : KeyApiModel<TKey1, TKey2, TKey3, TKey4, TKey5>
    {
#if !NET35 && !NET40
        [NotMapped]
#endif
        public TKey7 Id7 { get; set; }
    }

    public class KeyApiModel<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6, TKey7, TKey8>
        : KeyApiModel<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6, TKey7>
    {
#if !NET35 && !NET40
        [NotMapped]
#endif
        public TKey7 Id8 { get; set; }
    }

    public class KeyApiModel<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6, TKey7, TKey8, TKey9>
        : KeyApiModel<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6, TKey7, TKey8>
    {
#if !NET35 && !NET40
        [NotMapped]
#endif
        public TKey7 Id9 { get; set; }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */

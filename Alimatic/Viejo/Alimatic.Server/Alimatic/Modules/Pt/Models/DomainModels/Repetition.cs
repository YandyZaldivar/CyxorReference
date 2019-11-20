/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;

namespace Alimatic.Pt.Models
{
    public class Repetition
    {
        public int Id { get; set; }

        public RepetitionValue Value { get; set; }

        public string Name => Value.ToString();
    }

    [Flags]
    public enum RepetitionValue : long
    {
        Non = 0,

        _01 = 2,
        _02 = 4,
        _03 = 8,
        _04 = 16,
        _05 = 32,
        _06 = 64,
        _07 = 128,
        _08 = 256,
        _09 = 512,
        _10 = 1024,
        _11 = 2048,
        _12 = 4096,
        _13 = 8192,
        _14 = 16384,
        _15 = 32768,
        _16 = 65536,
        _17 = 131072,
        _18 = 262144,
        _19 = 524288,
        _20 = 1048576,
        _21 = 2097152,
        _22 = 4194304,
        _23 = 8388608,
        _24 = 16777216,
        _25 = 33554432,
        _26 = 67108864,
        _27 = 134217728,
        _28 = 268435456,
        _29 = 536870912,
        _30 = 1073741824,
        _31 = 2147483648,

        Anl = 4294967296,
        Sem = 8589934592,
        Tri = 17179869184,
        Bim = 34359738368,
        Men = 68719476736,
        Qui = 137438953472,
        Sm1 = 274877906944,
        Sm2 = 549755813888,
        Sm3 = 1099511627776,
        Sm4 = 2199023255552,
        Sml = 4398046511104,
        Dia = 8796093022208,
        Evt = 17592186044416,

        Lun = 35184372088832,
        Mar = 70368744177664,
        Mie = 140737488355328,
        Jue = 281474976710656,
        Vie = 562949953421312,
        Sab = 1125899906842624,
        Dom = 2251799813685248,
        //4503599627370496,
        //9007199254740992,

        Ene = Dom * 2,
        Feb = Ene * 2,
        Mrz = Feb * 2,
        Abr = Mrz * 2,
        May = Abr * 2,
        Jun = May * 2,
        Jul = Jun * 2,
        Ago = Jul * 2,
        Sep = Ago * 2,
        Oct = Sep * 2,
        Nov = Oct * 2,
        //Dic = Nov * 2,
    }
}
/* { Alimatic.Server } */

using System;

namespace Cyxor.Terminal
{
    enum Dest
    {
        one,
        two
    }

    struct bbb
    {
        public Dest? gh { get; set; }
    }

    class ProgramQQ
    {
        static void MainQQ()// => Networking.App.Run(Networking.Client.Instance, "Cyxor.Terminal");
        {


            Console.ReadKey();
        }

        static void SerTest()
        {
            var ser = new Serialization.Serializer();
            Dest? dest = Dest.two;
            bbb? bb = new bbb();
            var ski = default(Dest?);
            //ser.Serialize((object)dest);
            ser.Serialize(dest);
            ser.Serialize(bb);
            ser.Position = 0;
            var si = ser.DeserializeObject<Dest?>();
            //var sj = ser.DeserializeObject<bbb?>();
            var sj = ser.DeserializeNullableT<bbb>();
            var sf = ser.Length;

            System.Console.ReadKey();
        }
    }
}
